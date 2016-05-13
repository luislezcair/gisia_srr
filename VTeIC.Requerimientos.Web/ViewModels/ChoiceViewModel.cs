using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ChoiceViewModel
    {
        public ChoiceViewModel(ChoiceOption choice)
        {
            Id = choice.Id;
            Text = choice.Text;
            QuestionId = choice.Question.Id;
            UseInSearchKey = choice.UseInSearchKey;
            UseInSearchKeyAs = choice.UseInSearchKeyAs;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public int? QuestionId { get; set; }

        public bool UseInSearchKey { get; set; }
        public string UseInSearchKeyAs { get; set; }
    }
}