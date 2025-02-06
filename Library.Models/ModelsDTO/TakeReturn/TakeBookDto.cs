using System.Text.Json;

namespace Library.Models.ModelsDTO
{
    public class TakeBookDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime DateIssuance { get; set; }

    }
    public class ReturnBookDto
    {
        public int UserId { get; init; }
        public int BookId { get; init; }
        public DateTime DateReturn { get; init; }

        public ReturnBookDto(int userId, int bookId, DateTime dateReturn)
        {
            UserId = userId;
            BookId = bookId;
            DateReturn = dateReturn;
        }
    }

    public class ReturnBookRequest
    {
        public ReturnBookDto ReturnBook { get; set; }
        public Dictionary<string, JsonElement> Param { get; set; }
    }
}
