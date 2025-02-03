namespace Library.Models.ModelsDTO
{
    public record TakeBookDto
    {
        public required int Userid { get; set; }
        public required int Bookid { get; set; }
        public required DateTime DateIssuance { get; set; }

    }
    public record ReturnBookDto
    {
        public required int UserId { get; set; }
        public required int BookId { get; set; }
        public required DateTime DateReturn { get; set; }
    }
}
