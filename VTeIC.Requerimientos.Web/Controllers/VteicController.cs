using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SerachKey;
using VTeIC.Requerimientos.Web.ViewModels;
using VTeIC.Requerimientos.Web.Views.WebService;

namespace VTeIC.Requerimientos.Web.Controllers
{
    [Authorize]
    public class VTeICController : Controller
    {
        private readonly QuestionDBContext _db = new QuestionDBContext();
        private readonly QuestionManager _manager = new QuestionManager();

        // GET: VTeIC
        public ActionResult Index()
        {
            Session session = new Session();
            _db.Sessions.Add(session);
            _db.SaveChanges();

            var questions = _manager.QuestionList();
            return View((IEnumerable)questions);
        }

        [HttpPost]
        public ActionResult PostAnswer(AnswerViewModel answer)
        {
            // Obtiene la pregunta actual
            Question q = _db.Questions.Find(answer.QuestionId);

            // Guarda la respuesta a esta pregunta
            SaveAnswer(q, answer);

            // Si es una pregunta pivot almacena la respuesta para usarla en las próximas preguntas
            if(q.IsPivot)
            {
                Session["pivot"] = answer.TextAnswer;
            }

            // Obtiene los enlaces correspondientes a esta pregunta 
            QuestionLink ql = _db.QuestionLinks.FirstOrDefault(a => a.Question.Id == q.Id);

            // Si la pregunta no tiene enlaces, entonces fue la última del cuestionario
            if (ql == null)
            {
                return Json(new
                {
                    Finished = true
                });
            }

            Question next;

            // Determina la próxima pregunta de acuerdo a la respuesta Sí / No
            if (q.QuestionType.Type == QuestionTypes.BOOLEAN && !answer.BooleanAnswer.Value)
            {
                next = ql.NextNegative; 
            }
            else
            {
                next = ql.Next;
            }

            // Determinar si es la última pregunta
            QuestionLink qln = _db.QuestionLinks.FirstOrDefault(a => a.Question.Id == next.Id);
            QuestionViewModel qvm = new QuestionViewModel(next);

            qvm.Text = qvm.Text.Replace("[previous_answer]", "<em>" + Session["pivot"].ToString() + "</em>");

            return Json(new
            {
                Question = qvm,
                Finished = false,
                LastQuestion = qln == null
            });
        }

        /**
        * Guarda la respuesta en la base de datos
        **/
        private void SaveAnswer(Question question, AnswerViewModel answer)
        {
            var session = _db.Sessions.OrderByDescending(s => s.Id).First();

            var dbAnswer = new Answer
            {
                Question = question,
                AnswerType = question.QuestionType,
                TextAnswer = answer.TextAnswer,
                BooleanAnswer = answer.BooleanAnswer,
                MultipleChoiceAnswer = GetDBChoices(answer.OptionsAnswer)
            };

            session.Answers.Add(dbAnswer);

            if (!ModelState.IsValid) return;

            _db.Entry(session).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }

        /**
         * Transforma la lista de enteros con IDs de opciones en una lista de ChoiceOptions
         **/
        private List<ChoiceOption> GetDBChoices(List<int> ids)
        {
            List<ChoiceOption> options = new List<ChoiceOption>();

            if (ids != null)
            {
                foreach (int id in ids)
                {
                    options.Add(_db.QuestionChoices.Find(id));
                }
            }

            return options;
        }

        [HttpPost]
        public ActionResult SearchKey()
        {
            Session session = _db.Sessions.OrderByDescending(s => s.Id).First();
            SearchKeyGenerator generator = new SearchKeyGenerator();

            var searchKeys = generator.BuildSearchKey(session.Answers);

            GisiaClient webservice = new GisiaClient();
            webservice.SendKeys(searchKeys);

            return Json(new
            {
                searchKeys = searchKeys
            });
        }
    }
}