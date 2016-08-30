using System.Collections.Generic;

namespace VTeIC.Requerimientos.Entidades
{
    public class Answer
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }

        public virtual Question Question { get; set; }
        public QuestionTypes AnswerType { get; set; }

        public string TextAnswer { get; set; }
        public bool? BooleanAnswer { get; set; }
        public virtual ICollection<ChoiceOption> MultipleChoiceAnswer { get; set; }
    }
}