using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.ViewModels;

namespace VTeIC.Requerimientos.Web.Models
{
    public class QuestionChoiceController : Controller
    {
        private QuestionDBContext db = new QuestionDBContext();

        // GET: QuestionChoice
        public ActionResult Index()
        {
            var questionChoices = db.QuestionChoices.Include(c => c.Question)
                                                    .Where(c => c.Question.QuestionType.Id == 3)
                                                    .OrderBy(c => c.Question.Text);
            return View(questionChoices.ToList());
        }

        // GET: QuestionChoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoiceOption choiceOption = db.QuestionChoices.Find(id);
            if (choiceOption == null)
            {
                return HttpNotFound();
            }
            return View(choiceOption);
        }

        // GET: QuestionChoice/Create
        public ActionResult Create()
        {
            loadQuestionBox();
            return View();
        }

        // POST: QuestionChoice/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChoiceViewModel choice)
        {
            Question q = db.Questions.Find(choice.QuestionId);
            ChoiceOption choiceOption = new ChoiceOption
            {
                Question = q,
                Text = choice.Text,
                UseInSearchKey = choice.UseInSearchKey,
                UseInSearchKeyAs = choice.UseInSearchKeyAs
            };

            if (ModelState.IsValid)
            {
                db.QuestionChoices.Add(choiceOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            loadQuestionBox();
            return View(choiceOption);
        }

        // GET: QuestionChoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoiceOption choiceOption = db.QuestionChoices.Find(id);
            if (choiceOption == null)
            {
                return HttpNotFound();
            }

            loadQuestionBox(choiceOption.Question.Id);
            return View(choiceOption);
        }

        // POST: QuestionChoice/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChoiceViewModel choice)
        {
            Question q = db.Questions.Find(choice.QuestionId);
            ChoiceOption choiceOption = new ChoiceOption
            {
                Id = choice.Id,
                Question = q,
                Text = choice.Text,
                UseInSearchKey = choice.UseInSearchKey,
                UseInSearchKeyAs = choice.UseInSearchKeyAs
            };

            if (ModelState.IsValid)
            {
                db.Entry(choiceOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            loadQuestionBox(choice.QuestionId);
            return View(choiceOption);
        }

        // GET: QuestionChoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoiceOption choiceOption = db.QuestionChoices.Find(id);
            if (choiceOption == null)
            {
                return HttpNotFound();
            }
            return View(choiceOption);
        }

        // POST: QuestionChoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChoiceOption choiceOption = db.QuestionChoices.Find(id);
            db.QuestionChoices.Remove(choiceOption);
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

        private void loadQuestionBox(object defaultValue = null) {
            // En el ComboBox de respuestas carga solamente las preguntas del tipo "Opción múltiple"
            ViewBag.QuestionId = new SelectList(db.Questions.Where(q => q.QuestionType.Id == 3), "Id", "Text", defaultValue);
        }
    }
}
