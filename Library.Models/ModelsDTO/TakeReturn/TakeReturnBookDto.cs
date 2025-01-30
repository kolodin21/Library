namespace Library.Models.ModelsDTO
{
    public record TakeReturnBookDto
    {
        public required int Id { get; set; }
        public int PersonId { get; set; }
        public required string NameAuthor { get; set; }
        public required string TitleBook { get; set; }
        public DateTime DateIssuance { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}
