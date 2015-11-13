using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }

        public string TextAnswer { get; set; }
        public bool? BooleanAnswer { get; set; }

        public List<int> OptionsAnswer { get; set; }
    }
}