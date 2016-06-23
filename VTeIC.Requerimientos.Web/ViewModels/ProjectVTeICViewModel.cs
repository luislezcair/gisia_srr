using System.Collections.Generic;
using System.Linq;
using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ProjectVTeICViewModel
    {
        public ProjectVTeICViewModel(Project project, List<Question> questions, List<QuestionGroup> groups)
        {
            Project = new ProjectViewModel(project);
            Questions = (from q in questions select new QuestionViewModel(q)).ToList();
            Groups = (from g in groups select new QuestionGroupViewModel(g)).ToList();
        }

        public ProjectViewModel Project { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public List<QuestionGroupViewModel> Groups { get; set; }
    }
}