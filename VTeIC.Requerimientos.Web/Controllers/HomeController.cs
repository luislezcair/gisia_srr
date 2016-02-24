using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SerachKey;
using VTeIC.Requerimientos.Web.ViewModels;


namespace VTeIC.Requerimientos.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private QuestionDBContext db = new QuestionDBContext();
        private static string nombreGenerico = "";
        private static int bucle = 0;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VTeIC()
        {
            Question question = db.Questions.First();

            Session session = new Session();
            db.Sessions.Add(session);
            db.SaveChanges();
            
            return View(question);
        }

        [HttpPost]
        public ActionResult Check(AnswerViewModel answer)
        {
            //bool testBucle = false;
            var respuestas = new List<Answer>();
            Question nextQuestion = null;

            // Comprobar que QuestionId sea válido
            var question = db.Questions.Find(answer.QuestionId);
            if (question == null)
            {
                throw new InvalidOperationException("ERROR: No se encuentra la pregunta actual");
            }

            //Evitar que cada pregunta tipo texto tenga mas de dos respuestas
            //Session session = db.Sessions.OrderByDescending(s => s.Id).First();
            //foreach (Answer respuesta in session.Answers)
            //{
            //    if (question.QuestionType.Id == 1) 
            //    {
            //        if (question.Id == respuesta.Question.Id)
            //        {
            //            bucle += 1;
            //            if (bucle > 2)
            //            {
            //                testBucle = true;
            //            }
                        
            //        }
            //    }                
            //}
            
            SaveAnswer(question, answer);

            // Que la siguiente pregunta sea la siguiente en el orden pre-establecido.
            //if (testBucle)
            //{
            //    nextQuestion = question.NextQuestion.NextQuestionNegative;
            //}
            //else
            //{
            //    // Obtener la próxima pregunta de acuerdo al tipo de respuesta (Sí/No)
            //    if (question.QuestionTypeId == 2 && !answer.BooleanAnswer.Value)
            //    {
            //        nextQuestion = question.NextQuestionNegative;
            //    }
            //    else
            //    {
            //        nextQuestion = question.NextQuestion;
            //    }
            //}
           
            // Si no hay siguiente pregunta evniar la condición de fin
            if (nextQuestion == null)
            {
                bucle = 0;
                return Json(new { finished = true });
            }

            // Si es pivot, guardar la respuesta para usarlo en las próximas preguntas
            if (question.IsPivot)
            {
                nombreGenerico = answer.TextAnswer;
            }

            // Pasar las opciones de la base de datos a una lista más simple para evitar ciclos de relaciones
            List<ChoiceViewModel> choices = new List<ChoiceViewModel>();
            foreach (ChoiceOption choice in nextQuestion.ChoiceOptions)
            {
                choices.Add(new ChoiceViewModel { Id = choice.Id, Text = choice.Text });
            }

            return Json(new
            {
                idPregunta = nextQuestion.Id,
                textoPregunta = nextQuestion.Text.Replace("[previous_answer]", nombreGenerico),
                tipoPregunta = nextQuestion.QuestionType.Id,
                opcionesPregunta = choices
            });
             
        }

        /**
         * Guarda la respuesta en la base de datos
         **/
        private void SaveAnswer(Question question, AnswerViewModel answer)
        {
            var session = db.Sessions.OrderByDescending(s => s.Id).First();

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

            db.Entry(session).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
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
                    options.Add(db.QuestionChoices.Find(id));
                }
            }

            return options;
        }

        [HttpPost]
        public ActionResult VerClaveProvisoria()
        {
            Session session = db.Sessions.OrderByDescending(s => s.Id).First();
            SearchKeyGenerator generator = new SearchKeyGenerator();
         
            return Json(new 
            { 
                respuesta = generator.BuildSearchKey(session.Answers)
            });
        }
    }
}