using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SerachKey;
using VTeIC.Requerimientos.Web.ViewModels;
using VTeIC.Requerimientos.Web.WebService;
using System.Web;
using System;

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
            return View(_db.Projects.Where(t => t.UserId == userid ));
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
            Project project = new Project { UserId = User.Identity.GetUserId() };
            return View(project);
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Directorio,UserId,Activo")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Directorio = project.Nombre.Replace(" ","");

                string path1 = Properties.Settings.Default.PathDeDirectorios;

                string userName = User.Identity.GetUserName();
                ////me fijo si esta la carpeta de usuario
                //bool exists = System.IO.Directory.Exists(path1 + "\\" + userName);
                //if (!exists)
                //    System.IO.Directory.CreateDirectory(path1 + "\\" + userName);
                ////me fijo si esta la carpeta de proyecto
                //exists = System.IO.Directory.Exists(path1 + "\\" + userName + "\\" + project.Directorio);
                //if (!exists)
                //    System.IO.Directory.CreateDirectory(path1 + "\\" + userName + "\\" + project.Directorio);
                
                project.Activo = true;
                
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
        public ActionResult Edit([Bind(Include = "Id,Nombre,Directorio,UserId,Activo")] Project project)
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
            _db.Projects.Remove(project);
            _db.SaveChanges();
            return RedirectToAction("Index");
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

            var vteic = new ProjectVTeICViewModel(project, _manager.QuestionList());
            return View("Work", vteic);
        }

        [HttpPost]
        [Route("Project/{projectId:int}/SearchKey")]
        public ActionResult SearchKey(int projectId)
        {
            Session session = _db.Sessions.OrderByDescending(s => s.Id).First();
            SearchKeyGenerator generator = new SearchKeyGenerator();

            var searchKeys = generator.BuildSearchKey(session.Answers);
            var project = _db.Projects.Find(projectId);

            try
            {
                GisiaClient webservice = new GisiaClient(User.Identity.Name, project);
                webservice.SendRequest(searchKeys);
            }
            catch (HttpException httpError)
            {
                return Json(new
                {
                    result = false,
                    error = "No se ha podido conectar con el servicio web. ERROR: " + httpError.Message
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    result = false,
                    error = "No se ha podido conectar con el servicio web. ERROR: " + e.Message
                });
            }

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
    }
}
