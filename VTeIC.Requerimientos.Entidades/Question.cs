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

        public string HintText { get; set; }

        // Si es true, la respuesta a esta pregunta se va a utilzar como parte del texto
        // de las siguientes preguntas
        public bool IsPivot { get; set; }

        public int Weight { get; set; }
        public QuestionTypes QuestionType { get; set; }

        public virtual ICollection<ChoiceOption> ChoiceOptions { get; set; }
        public virtual QuestionGroup QuestionGroup { get; set; }
    }

    public enum QuestionTypes
    {
        TEXT_FIELD,
        BOOLEAN,
        MULTIPLE_CHOICE,
        EXCLUSION_TERMS
    }
}