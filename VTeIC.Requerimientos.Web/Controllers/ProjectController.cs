using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;

namespace VTeIC.Requerimientos.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private QuestionDBContext db = new QuestionDBContext();

        // GET: Project
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId(); 
            return View(db.Projects.Where(t => t.UserId == userid ));
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            Project project = new Project();
            project.UserId = User.Identity.GetUserId();

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

                string path1 = VTeIC.Requerimientos.Web.Properties.Settings.Default.PathDeDirectorios;

                string userName = User.Identity.GetUserName();
                //me fijo si esta la carpeta de usuario
                bool exists = System.IO.Directory.Exists(path1 + "\\" + userName);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path1 + "\\" + userName);
                //me fijo si esta la carpeta de proyecto
                exists = System.IO.Directory.Exists(path1 + "\\" + userName + "\\" + project.Directorio);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path1 + "\\" + userName + "\\" + project.Directorio);
                
                project.Activo = true;
                
                db.Projects.Add(project);
                db.SaveChanges();
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
            Project project = db.Projects.Find(id);
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
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
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
            Project project = db.Projects.Find(id);
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
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Work(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
