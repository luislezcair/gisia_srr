using System.Collections.Generic;

namespace VTeIC.Requerimientos.Entidades
{
    public class Session
    {
        public int Id { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
