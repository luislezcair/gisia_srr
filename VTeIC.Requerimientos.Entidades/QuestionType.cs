using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTeIC.Requerimientos.Entidades
{
    public class QuestionType
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public QuestionTypes Type { get; set; }
    }

    public enum QuestionTypes
    {
        TEXT_FIELD,
        BOOLEAN,
        MULTIPLE_CHOICE
    }
}
