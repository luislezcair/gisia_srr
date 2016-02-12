using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.Models
{
    class QuestionManager
    {
        private readonly QuestionDBContext _db = new QuestionDBContext();

        /**
         * Busca la primera pregunta de la secuencia
         **/
        public Question FirstQuestion()
        {
            var linksA = _db.QuestionLinks.Select(link => link.Next.Id)
                                          .Distinct()
                                          .ToList();
            var linksN = _db.QuestionLinks.Where(link => link.NextNegative != null)
                                          .Select(link => link.NextNegative.Id)
                                          .Distinct()
                                          .ToList();

            return _db.Questions.First(q => !linksA.Contains(q.Id) && !linksN.Contains(q.Id));
        }

        /**
         * Construye la secuencia de preguntas a partir de los vínculos entre sí
         **/
        public List<Question> QuestionList()
        {
            var questions = new List<Question>();
            var current = FirstQuestion();

            questions.Add(current);
            var link = _db.QuestionLinks.First(q => q.Question.Id == current.Id);

            while (link != null && link.Next != null)
            {
                if (questions.Contains(link.Next))
                {
                    questions.Add(link.NextNegative);
                    link = _db.QuestionLinks.First(q => q.Question.Id == link.NextNegative.Id);
                }
                else
                {
                    questions.Add(link.Next);
                    link = _db.QuestionLinks.FirstOrDefault(q => q.Question.Id == link.Next.Id);
                }
            }

            return questions;
        }

        /**
         * Busca la pregunta siguiente a _current_
         */
        public Question GetNextQuestion(Question q)
        {
            return null;
        }
    }
}
