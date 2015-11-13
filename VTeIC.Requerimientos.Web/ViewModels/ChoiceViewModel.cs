using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ChoiceViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? QuestionId { get; set; }

        public bool UseInSearchKey { get; set; }
        public string UseInSearchKeyAs { get; set; }
    }
}