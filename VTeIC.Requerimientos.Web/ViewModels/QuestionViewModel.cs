using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class QuestionViewModel
    {
        private readonly QuestionDBContext _db = new QuestionDBContext();

        public QuestionViewModel(Question question)
        {
            Id = question.Id;
            Text = question.Text;
            QuestionType = question.QuestionType;
            IsPivot = question.IsPivot;
            Group = question.QuestionGroup.Id;
            HintText = question.HintText;

            Link = new QuestionLinkViewModel(_db.QuestionLinks.FirstOrDefault(q => q.Question.Id == question.Id));

            ChoiceOptions = new List<ChoiceViewModel>();
            foreach (var choice in question.ChoiceOptions)
            {
                ChoiceOptions.Add(new ChoiceViewModel(choice));
            }
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public List<ChoiceViewModel> ChoiceOptions { get; set; }
        public bool IsPivot { get; set; }
        public QuestionLinkViewModel Link { get; set; }
        public int Group { get; set; }
        public string HintText { get; set; }
    }
}