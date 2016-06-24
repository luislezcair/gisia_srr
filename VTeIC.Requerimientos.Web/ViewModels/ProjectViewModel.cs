using System.IO;
using VTeIC.Requerimientos.Entidades;
using System.Web;
using System.Diagnostics;

namespace VTeIC.Requerimientos.Web.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel(Project project)
        {
            Id = project.Id;
            Nombre = project.Nombre;
            Directorio = project.Directorio;
            Activo = project.Activo;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public bool Activo { get; set; }

        public bool DirectoryExists()
        {
            DirectoryInfo dir = new DirectoryInfo(GetAbsoluteDirectoryPath());
            return dir.Exists;
        }

        public void RemoveProjectDirectory()
        {
            DirectoryInfo dir = new DirectoryInfo(GetAbsoluteDirectoryPath());
            if(dir.Exists)
            {
                dir.Delete(true);
            }
        }

        private string GetAbsoluteDirectoryPath()
        {
            return HttpContext.Current.Server.MapPath("/Archivos") + '\\' + HttpContext.Current.User.Identity.Name + '\\' + Directorio;
        }
    }
}