using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Controllers;
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
            QuestionType = question.QuestionType.Type;
            Title = question.Title;
            IsPivot = question.IsPivot;
            HasManyAnswers = question.HasManyAnswers;

            Link = new QuestionLinkViewModel(_db.QuestionLinks.FirstOrDefault(q => q.Question.Id == question.Id));

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
        public bool HasManyAnswers { get; set; }
        public QuestionLinkViewModel Link { get; set; }
    }
}