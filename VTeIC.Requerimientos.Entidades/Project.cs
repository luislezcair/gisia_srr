using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Directorio { get; set; }
        public string UserId { get; set; }
        public ProjectState State { get; set; }

        // Cuando no se pudo iniciar el proyecto por algún error, esta propiedad va a
        // contener la descripción del error.
        public string StateReason { get; set; }

        public virtual Language Language { get; set; }

        public virtual ICollection<ProjectSearchKey> SearchKeys { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public enum ProjectState
    {
        [Display(Name = "Activo")]
        ACTIVE,

        [Display(Name = "Finalizado")]
        FINISHED,

        [Display(Name = "Inactivo")]
        INACTIVE,

        [Display(Name = "Trabajando")]
        WORKING,

        [Display(Name = "Error")]
        ERROR
    }
}
