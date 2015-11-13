using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
        public int Weight { get; set; }
        public string Title { get; set; }

        // Enlace a la siguiente pregunta para cualquier tipo.
        // En caso de una pregunta de Sí/No, es el enlace para una respuesta afirmativa (sí)
        //public int? NextQuestionId { get; set; }
        //public virtual Question NextQuestion { get; set; }

        // Enlace a la siguiente pregunta en caso de una respuesta NO
        //public int? NextQuestionNegativeId { get; set; }
        //public virtual Question NextQuestionNegative { get; set; }

        //public int QuestionTypeId { get; set; }
        public virtual QuestionType QuestionType { get; set; }

        public virtual ICollection<ChoiceOption> ChoiceOptions { get; set; }
    }

    public class ChoiceOption
    {
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; }

        [EnforceQuestionType(QuestionTypes.MULTIPLE_CHOICE)]
        public virtual Question Question { get; set; }

        public bool UseInSearchKey { get; set; }
        public string UseInSearchKeyAs { get; set; }
    }

    public enum QuestionOperator
    {
        AND,
        OR,
        NOT
    }

    public class QuestionRelationshipOperator
    {
        public int Id { get; set; }
        public Question First { get; set; }
        public Question Second { get; set; }
        public QuestionOperator Operator { get; set; }
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
            Question q = (Question)value;
            return q.QuestionType.Type != this.QuestionType ? 
                new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName)) : null;
        }
    }
}