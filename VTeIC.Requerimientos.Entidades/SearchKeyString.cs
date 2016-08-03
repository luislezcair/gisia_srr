namespace VTeIC.Requerimientos.Entidades
{
    public class SearchKeyString
    {
        public int Id { get; set; }

        //public ChoiceOption choice { get; set; }
        public string SearchKeyParam { get; set; }
        public virtual Language Language { get; set; }
    }
}
