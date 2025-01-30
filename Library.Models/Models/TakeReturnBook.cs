namespace Library.Models
{
    public record TakeReturnBooks
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public int BookId { get; set; }
        public required string NameAuthor { get; set; }
        public required string TitleBook { get; set; }
        public DateTime DateIssuance { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}