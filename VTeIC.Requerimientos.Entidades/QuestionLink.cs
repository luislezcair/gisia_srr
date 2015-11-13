namespace VTeIC.Requerimientos.Entidades
{
    public class QuestionLink
    {
        public int Id { get; set; }
        public virtual Question Question { get; set; }
        public virtual Question Next { get; set; }
        public virtual Question NextNegative { get; set; }
    }
}
