using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTeIC.Requerimientos.Entidades
{
    public class Answer
    {
        public int Id { get; set; }

        public virtual Session Session { get; set; }

        public virtual Question Question { get; set; }
        public virtual QuestionType AnswerType { get; set; }

        public string TextAnswer { get; set; }
        public bool? BooleanAnswer { get; set; }
        public virtual ICollection<ChoiceOption> MultipleChoiceAnswer { get; set; }
    }
}