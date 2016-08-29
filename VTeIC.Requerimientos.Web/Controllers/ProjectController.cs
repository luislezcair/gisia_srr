using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.ViewModels;
using System.IO;
using System.Collections.Generic;
using Hangfire;
using VTeIC.Requerimientos.Web.BackgroundJobs;
using System.Data.Entity.Infrastructure;
using VTeIC.Requerimientos.Web.SearchKey;
using VTeIC.Requerimientos.Web.Util;

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

            return View(new ProjectViewModel(project));
        }

        // GET: Project/Create
        public ActionResult Create()
        {
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

            string directory = projectVM.Nombre.Slugify() + "_" + RandomString.GetRandomString(10);

            Project project = new Project
            {
                Nombre = projectVM.Nombre,
                UserId = projectVM.UserId,
                Language = lang,
                Directorio = directory
            };

            if (ModelState.IsValid)
            {
                _db.Projects.Add(project);

                try
                {
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("Nombre", "Ya existe un proyecto con este nombre.");
                }
            }
            projectVM.Langauges = GetLanguages();
            return View(projectVM);
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

            var projectVM = new ProjectViewModel(project);

            //identify the virtual path
            string filePath = "/Archivos";
            string absoluteDir = Server.MapPath(filePath) + "\\" + User.Identity.Name + "\\" + project.Directorio;
            DirectoryInfo dir = new DirectoryInfo(absoluteDir);

            if (!dir.Exists)
            {
                return View(projectVM);
            }

            FileInfo[] files = dir.GetFiles();

            //iterate through each file, get its name and set its path, and add to my VM
            foreach (FileInfo file in files)
            {
                if (file.FullName.EndsWith("json"))
                    continue;

                FileViewModel newFile = new FileViewModel(file.FullName);

                //set path to virtual directory + file name
                newFile.VirtualPath = filePath + "/" + User.Identity.Name + "/" + project.Directorio + "/" + file.Name;
                projectVM.Files.Add(newFile);
            }

            return View(projectVM);
        }

        [Route("Project/{projectId:int}/VTeIC")]
        public ActionResult VTeIC(int projectId)
        {
            Project project = _db.Projects.Find(projectId);

            if (project == null)
            {
                return HttpNotFound();
            }

            var vteic = new ProjectVTeICViewModel(project, _manager.QuestionList(), _db.QuestionGroups.ToList());

            if(project.State == ProjectState.ACTIVE)
            {
                return View("ProjectActiveError", vteic);
            }

            Session session = new Session();
            _db.Sessions.Add(session);
            _db.SaveChanges();

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

            BackgroundJob.Enqueue(() => VTeICJob.Perform(projectId, User.Identity.Name));

            return Json(new { result = true });
        }

        [HttpPost]
        [Route("Project/{projectId:int}/SearchKeys")]
        public ActionResult SearchKeys(int projectId)
        {
            var project = _db.Projects.Find(projectId);

            if (project == null || project.State == ProjectState.ACTIVE)
            {
                return HttpNotFound();
            }

            Session session = _db.Sessions.OrderByDescending(s => s.Id).First();
            var searchKeys = SearchKeyGenerator.BuildSearchKey(session.Answers, project.Language);

            return View("SearchKeysDebug", searchKeys);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        /**
         * Transforma la lista de idiomas de la base de datos a una lista de ListItems para
         * mostrarse en una lista desplegable en la interfaz.
         */
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
