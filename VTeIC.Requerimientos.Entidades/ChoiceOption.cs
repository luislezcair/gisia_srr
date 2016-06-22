using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTeIC.Requerimientos.Entidades
{
    public class ChoiceOption
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [EnforceQuestionType(QuestionTypes.MULTIPLE_CHOICE)]
        public virtual Question Question { get; set; }

        public bool UseInSearchKey { get; set; }
        public string UseInSearchKeyAs { get; set; }

        public int Weight { get; set; }
    }

    public class EnforceQuestionType : ValidationAttribute
    {
        public EnforceQuestionType(QuestionTypes questionType)
        {
            this.QuestionType = questionType;
        }

        public QuestionTypes QuestionType { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var q = (Question)value;
            return q.QuestionType.Type != this.QuestionType ?
                new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName)) : null;
        }
    }
}
