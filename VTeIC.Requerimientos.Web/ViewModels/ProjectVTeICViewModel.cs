using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ProjectVTeICViewModel
    {
        public ProjectVTeICViewModel(Project project, List<Question> questions)
        {
            Project = new ProjectViewModel(project);
            Questions = (from q in questions select new QuestionViewModel(q)).ToList();
        }

        public ProjectViewModel Project { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}