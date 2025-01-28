namespace Library.Server.BL.ModelsDTO.TakeReturn
{
    public record TakeBookDto
    {
        public required int userid { get; set; }
        public required int bookid { get; set; }
        public required DateTime dateissuance { get; set; }

    }
    public record ReturnBookDto
    {
        public required int UserId { get; set; }
        public required int BookId { get; set; }
        public required DateTime DateReturn { get; set; }

    }
}
