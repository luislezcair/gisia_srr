using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class QuestionLinkViewModel
    {
        public QuestionLinkViewModel(QuestionLink link)
        {
            if (link == null)
                return;

            QuestionId = link.Question.Id;
            NextQuestionId = link.Next != null ? link.Next.Id : -1;
            NextQuestionNegativeId = link.NextNegative != null ? link.NextNegative.Id : -1;
        }

        public int QuestionId { get; set; }
        public int NextQuestionId { get; set; }
        public int NextQuestionNegativeId { get; set; }
    }
}