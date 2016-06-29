namespace VTeIC.Requerimientos.Entidades
{
    public enum QuestionOperator
    {
        AND,
        OR,
        NOT
    }

    public class QuestionRelationshipOperator
    {
        public int Id { get; set; }
        public Question First { get; set; }
        public Question Second { get; set; }
        public QuestionOperator Operator { get; set; }
    }
}
