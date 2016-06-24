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
            string filePath = "/Archivos";
            string absoluteDir = HttpContext.Current.Server.MapPath(filePath) + '\\' + HttpContext.Current.User.Identity.Name + '\\' + Directorio;
            DirectoryInfo dir = new DirectoryInfo(absoluteDir);
            return dir.Exists;
        }
    }
}