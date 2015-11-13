using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;

namespace VTeIC.Requerimientos.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private QuestionDBContext db = new QuestionDBContext();

        // GET: Questions
        public ActionResult Index()
        {
            //var questions = db.Questions.Include(q => q.NextQuestion)
            //                            .Include(t => t.QuestionType)
            //                            .Include(qn => qn.NextQuestionNegative);
            //return View(questions.ToList());
            return View();
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.NextQuestionId = new SelectList(db.Questions, "Id", "Text");
            ViewBag.NextQuestionNegativeId = new SelectList(db.Questions, "Id", "Text");
            ViewBag.QuestionTypeId = new SelectList(db.QuestionTypes, "Id", "Description");
            return View();
        }

        // POST: Questions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,NextQuestionId,NextQuestionNegativeId,QuestionTypeId,IsPivot,Weight")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            loadSelectLists(question);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            loadSelectLists(question);
            return View(question);
        }

        // POST: Questions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,NextQuestionId,NextQuestionNegativeId,QuestionTypeId,IsPivot,Weight")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            loadSelectLists(question);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void loadSelectLists(Question question)
        {
            //ViewBag.NextQuestionId = new SelectList(db.Questions, "Id", "Text", question.NextQuestionId);
            //ViewBag.NextQuestionNegativeId = new SelectList(db.Questions, "Id", "Text", question.NextQuestionNegativeId);
            //ViewBag.QuestionTypeId = new SelectList(db.QuestionTypes, "Id", "Description", question.QuestionTypeId);
        }
    }
}
