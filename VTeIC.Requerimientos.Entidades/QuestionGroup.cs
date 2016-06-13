using System.Collections.Generic;

namespace VTeIC.Requerimientos.Entidades
{
    public class QuestionGroup
    {
        public QuestionGroup()
        {
            Questions = new List<Question>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
