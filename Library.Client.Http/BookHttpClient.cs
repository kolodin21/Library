using Library.Models.ModelsDTO;
using System.Net.Http.Json;
using Library.Models;

namespace Library.Client.Http
{
    public class BookHttpClient : LibraryHttpBase
    {
        #region URI
        private static Uri GetActivityBookUserUri => new Uri($"{Host}/ActivityBooks");
        private static Uri GetActualBooksLibraryUri => new Uri($"{Host}/ActualBooksLibrary");

        #endregion

        #region Http

        public async Task<IEnumerable<BookViewDto>?> GetActivityBooks(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetActivityBookUserUri, param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<IEnumerable<BookViewDto>>()
                : null;
        }

        public async Task<IEnumerable<Book>?> GetActualBooksLibrary() => await
            Client.GetFromJsonAsync<IEnumerable<Book>?>(GetActualBooksLibraryUri);

        #endregion
    }
}
