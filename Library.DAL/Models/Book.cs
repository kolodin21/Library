namespace Library.DAL.Models
{
    public class Author
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class Publisher
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class Condition
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public record Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int AuthorId { get; set; }
        public int Year { get; set; }
        public int PublisherId { get; set; }
        public int ConditionId { get; set; }
        public int BalanceBook { get; set; }
        public int CountActivity { get; set; }
        public int CountBookIssuance { get; set; }
    }
}