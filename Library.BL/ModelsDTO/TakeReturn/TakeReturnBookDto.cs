namespace Library.BL.ModelsDTO.TakeReturn
{
    public record TakeReturnBookDto
    {
        public required string NameAuthor { get; set; }
        public required string TitleBook { get; set; }
        public DateTime DateIssuance { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}
