using Library.Models.ModelsDTO;
using System.Net.Http.Json;
using Library.Models;
using System.Text.Json;

namespace Library.Client.Http
{
    public class BookHttpClient : LibraryHttpBase
    {
        #region URI
        private static Uri GetActivityBookUserUri => new Uri($"{Host}/ActivityUserBooks");
        private static Uri GetActualBooksLibraryUri => new Uri($"{Host}/ActualBooksLibrary");
        private static Uri GetHistoryBookUserUri => new Uri($"{Host}/HistoryUserBooks");

        private static Uri ReturnBookUserUri => new Uri($"{Host}/ReturnBook");
        private static Uri TakeBookUserUri => new Uri($"{Host}/TakeBook");


        #endregion

        #region Http

        public async Task<IEnumerable<BookUserActivityViewDto>?> GetActivityBooks(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetActivityBookUserUri, param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<IEnumerable<BookUserActivityViewDto>>()
                : null;
        }
        public async Task<IEnumerable<BookUserHistoryViewDto>?> GetHistoryBooks(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetHistoryBookUserUri, param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<IEnumerable<BookUserHistoryViewDto>>()
                : null;
        }

        public async Task<IEnumerable<Book>?> GetActualBooksLibrary() => await
            Client.GetFromJsonAsync<IEnumerable<Book>?>(GetActualBooksLibraryUri);

        ////===================//
        
        public async Task<bool> ReturnBookUser(ReturnBookRequest returnBook)
        {
            var response = await Client.PostAsJsonAsync(ReturnBookUserUri, returnBook);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> TakeBookUser(TakeBookRequest takeBook)
        {
            var response = await Client.PostAsJsonAsync(TakeBookUserUri, takeBook);

            return response.IsSuccessStatusCode;
        }
        
        #endregion
    }
}
