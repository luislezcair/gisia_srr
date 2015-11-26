using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class QuestionViewModel
    {
        public QuestionViewModel(Question question)
        {
            Id = question.Id;
            Text = question.Text;
            QuestionType = question.QuestionType.Type;
            Title = question.Title;
            IsPivot = question.IsPivot;

            ChoiceOptions = new List<QuestionChoiceViewModel>();
            foreach (var choice in question.ChoiceOptions)
            {
                ChoiceOptions.Add(new QuestionChoiceViewModel(choice));
            }
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionTypes QuestionType { get; set; }
        public List<QuestionChoiceViewModel> ChoiceOptions { get; set; }
        public string Title { get; set; }
        public bool IsPivot { get; set; }
    }
}