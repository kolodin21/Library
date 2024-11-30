namespace Library.DAL.Models
{
    public class TakeReturnBooks
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime DateIssuance { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}