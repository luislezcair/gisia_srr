using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class QuestionGroupViewModel
    {
        public QuestionGroupViewModel(QuestionGroup questionGroup)
        {
            Id = questionGroup.Id;
            Title = questionGroup.Title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}