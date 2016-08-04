using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SerachKey;
using VTeIC.Requerimientos.Web.ViewModels;
using VTeIC.Requerimientos.Web.WebService;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace VTeIC.Requerimientos.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly QuestionDBContext _db = new QuestionDBContext();
        private readonly QuestionManager _manager = new QuestionManager();

        // GET: Project
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId(); 
            var projects = _db.Projects.Where(t => t.UserId == userid).ToList();

            return View((from p in projects select new ProjectViewModel(p)).ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            //Project project = new Project { UserId = User.Identity.GetUserId() };
            //ViewBag.Languages = _db.Languages.ToList();
            var project = new ProjectViewModel
            {
                UserId = User.Identity.GetUserId(),
                Langauges = GetLanguages()
            };

            return View(project);
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,UserId,LanguageId")] ProjectViewModel projectVM)
        {
            var lang = _db.Languages.Find(projectVM.LanguageId);
            if(lang == null)
            {
                return View(projectVM);
            }

            Project project = new Project
            {
                Nombre = projectVM.Nombre,
                UserId = projectVM.UserId,
                Language = lang,
                Directorio = projectVM.Nombre.Replace(" ", "")
            };

            if (ModelState.IsValid)
            {
                _db.Projects.Add(project);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(project).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = _db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = _db.Projects.Find(id);

            var projectVM = new ProjectViewModel(project);
            projectVM.RemoveProjectDirectory();

            _db.Projects.Remove(project);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("Project/{projectId:int}/Files")]
        public ActionResult Files(int projectId)
        {
            Project project = _db.Projects.Find(projectId);

            if (project == null)
            {
                return HttpNotFound();
            }

            var fileList = new List<FileViewModel>();

            //identify the virtual path
            string filePath = "/Archivos";
            string absoluteDir = Server.MapPath(filePath) + "\\" + User.Identity.Name + "\\" + project.Directorio;
            DirectoryInfo dir = new DirectoryInfo(absoluteDir);

            if (!dir.Exists)
            {
                return View(fileList);
            }

            FileInfo[] files = dir.GetFiles();

            //iterate through each file, get its name and set its path, and add to my VM
            foreach (FileInfo file in files)
            {
                if (file.FullName.EndsWith("json"))
                    continue;

                FileViewModel newFile = new FileViewModel(file.FullName);
                newFile.VirtualPath = filePath + "/" + User.Identity.Name + "/" + project.Directorio + "/" + file.Name; //set path to virtual directory + file name
                fileList.Add(newFile);
            }

            return View(fileList);
        }

        [Route("Project/{projectId:int}/VTeIC")]
        public ActionResult VTeIC(int projectId)
        {
            Project project = _db.Projects.Find(projectId);

            if (project == null)
            {
                return HttpNotFound();
            }

            Session session = new Session();
            _db.Sessions.Add(session);
            _db.SaveChanges();

            var vteic = new ProjectVTeICViewModel(project, _manager.QuestionList(), _db.QuestionGroups.ToList());
            return View("Work", vteic);
        }

        [HttpPost]
        [Route("Project/{projectId:int}/SearchKey")]
        public ActionResult SearchKey(int projectId)
        {
            var project = _db.Projects.Find(projectId);

            if (project == null || project.State == ProjectState.ACTIVE)
            {
                return HttpNotFound();
            }

            Session session = _db.Sessions.OrderByDescending(s => s.Id).First();
            var searchKeys = SearchKeyGenerator.BuildSearchKey(session.Answers, project.Language);

            try
            {
                GisiaClient webservice = new GisiaClient(User.Identity.Name, project);
                webservice.SendRequest(searchKeys);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    result = false,
                    error = "No se ha podido conectar con el servicio web. ERROR: " + e.Message
                });
            }

            // Se inició el proceso de búsqueda. Establecer como activo este proyecto
            project.State = ProjectState.ACTIVE;
            _db.SaveChanges();

            return Json(new
            {
                result = true,
                searchKeys = searchKeys
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private IEnumerable<SelectListItem> GetLanguages()
        {
            var lang = _db.Languages
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });

            return new SelectList(lang, "Value", "Text");
        }
    }
}
