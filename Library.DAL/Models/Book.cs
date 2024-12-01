namespace Library.DAL.Models
{
    public record Author
    {
        public int AuthorId { get; set; }
        public required string AuthorName { get; set; }
    }

    public record Publisher
    {
        public int PublisherId { get; set; }
        public required string PublisherName { get; set; }
    }

    public record Condition
    {
        public int ConditionId { get; set; }
        public required string ConditionName { get; set; }
    }

    public record Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int BalanceBook { get; set; }
        public int CountActivity { get; set; }
        public int CountBookIssuance { get; set; }
        public required Author Author{ get; set; }
        public required  Publisher Publisher { get; set; }
        public required Condition  Condition{ get; set; }
    }
    //Todo
    //Изменить класс 
}