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
            VTeIC = new VTeICViewModel(questions, groups);
        }

        public ProjectViewModel Project { get; set; }
        public VTeICViewModel VTeIC { get; set; }
    }
}