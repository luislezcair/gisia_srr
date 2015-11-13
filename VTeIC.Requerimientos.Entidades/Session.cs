using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTeIC.Requerimientos.Entidades
{
    public class Session
    {
        public int Id { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
