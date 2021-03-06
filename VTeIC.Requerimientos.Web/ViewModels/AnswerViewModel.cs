﻿using System.Collections.Generic;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class AnswerViewModel
    {
        public int ProjectId { get; set; }
        public int QuestionId { get; set; }

        public string TextAnswer { get; set; }
        public bool? BooleanAnswer { get; set; }

        public List<int> OptionsAnswer { get; set; }
    }
}