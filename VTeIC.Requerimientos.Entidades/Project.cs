using System.ComponentModel.DataAnnotations;

namespace VTeIC.Requerimientos.Entidades
{
    public class Project
    {
        public Project()
        {
            // Valor por defecto para este atributo
            State = ProjectState.INACTIVE;
        }

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public string UserId { get; set; }
        public ProjectState State { get; set; }

        public virtual Language Language { get; set; }

        //[Display(Name = "Idioma")]
        //public ProjectLanguage Language { get; set; } 
    }

    //public enum ProjectLanguage
    //{
    //    [Display(Name = "Inglés")]
    //    ENGLISH,

    //    [Display(Name = "Español")]
    //    SPANISH
    //}

    public enum ProjectState
    {
        ACTIVE,
        FINISHED,
        INACTIVE
    }
}
