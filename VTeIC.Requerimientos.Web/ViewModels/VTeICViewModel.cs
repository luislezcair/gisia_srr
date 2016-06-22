using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class VTeICViewModel
    {
        public VTeICViewModel(List<Question> questions, List<QuestionGroup> groups)
        {
            Questions = (from q in questions select new QuestionViewModel(q)).ToList();
            Groups = (from g in groups select new QuestionGroupViewModel(g)).ToList();
        }

        public List<QuestionViewModel> Questions { get; set; }
        public List<QuestionGroupViewModel> Groups { get; set; }
    }
}