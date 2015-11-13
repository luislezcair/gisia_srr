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
            id = question.Id;
            text = question.Text;
            questionType = question.QuestionType.Type;
            title = question.Title;

            choiceOptions = new List<QuestionChoiceViewModel>();
            foreach (var choice in question.ChoiceOptions)
            {
                choiceOptions.Add(new QuestionChoiceViewModel(choice));
            }
        }

        public int id { get; set; }
        public string text { get; set; }
        public QuestionTypes questionType { get; set; }
        public List<QuestionChoiceViewModel> choiceOptions { get; set; }
        public string title { get; set; }
    }
}