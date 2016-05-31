using VTeIC.Requerimientos.Entidades;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel(Project project)
        {
            Id = project.Id;
            ProjectName = project.Nombre;
        }

        public int Id { get; set; }
        public string ProjectName { get; set; }
    }
}