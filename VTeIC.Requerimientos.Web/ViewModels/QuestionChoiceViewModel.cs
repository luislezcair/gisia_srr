using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class QuestionChoiceViewModel
    {
        public QuestionChoiceViewModel(ChoiceOption choice)
        {
            id = choice.Id;
            text = choice.Text;
        }

        public int id { get; set; }
        public string text { get; set; }
    }
}