namespace Library.Models.ModelsDTO
{
    public record BookViewDto
    {
        public string NameAuthor { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Condition { get; set; }
        public string Publisher { get; set; }
        public DateTime DateIssuance { get; set; }
    }
}
