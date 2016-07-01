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
        public string Nombre { get; set; }
        public string Directorio { get; set; }
        public string UserId { get; set; }
        public ProjectState State { get; set; }
    }

    public enum ProjectState
    {
        ACTIVE,
        FINISHED,
        INACTIVE
    }
}
