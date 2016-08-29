namespace VTeIC.Requerimientos.Entidades
{
    public class ProjectSearchKey
    {
        public int Id { get; set; }
        public string KeyString { get; set; }
        public virtual Project Project { get; set; }
    }
}
