namespace Library.Server.BL.ModelsDTO.BookDto
{
    public class BookAddDto(string title, int authorId, int year, int publisherId, int conditionId, int quantity)
    {
        public string Title { get; set; } = title;
        public int AuthorId { get; set; } = authorId;
        public int Year { get; set; } = year;
        public int PublisherId { get; set; } = publisherId;
        public int ConditionId { get; set; } = conditionId;
        public int Quantity { get; set; } = quantity;
    }
}
