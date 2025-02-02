namespace Library.Models
{
    public record Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string NameAuthor { get; set; }
        public int Year { get; set; }
        public required string Publisher { get; set; }
        public required string Condition { get; set; }
        public int BalanceBook { get; set; }
        public int CountActivity { get; set; }
        public int CountBookIssuance { get; set; }
    }
}