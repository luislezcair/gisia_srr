using System.Collections.Generic;

namespace VTeIC.Requerimientos.Entidades
{
    public class Question
    {
        public Question()
        {
            ChoiceOptions = new List<ChoiceOption>();
        }

        public int Id { get; set; }
        public string Text { get; set; }

        // Si es true, la respuesta a esta pregunta se va a utilzar como parte del texto
        // de las siguientes preguntas
        public bool IsPivot { get; set; }

        // Indica si la pregunta puede responderse varias veces, como en el caso de "Ingrese característica"
        public bool HasManyAnswers { get; set; }
        public int Weight { get; set; }
        public string Title { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        public virtual ICollection<ChoiceOption> ChoiceOptions { get; set; }

        public virtual QuestionGroup QuestionGroup { get; set; }
    }
}