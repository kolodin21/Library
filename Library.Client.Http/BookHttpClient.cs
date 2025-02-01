using Library.Models.ModelsDTO;
using System.Net.Http.Json;

namespace Library.Client.Http
{
    public class BookHttpClient : LibraryHttpBase
    {
        #region URI
        private static Uri GetActivityUri => new Uri($"{Host}/ActivityBooks");

        #endregion

        #region Http

        public async Task<IEnumerable<BookViewDto>?> GetActivityBooks(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetActivityUri, param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<IEnumerable<BookViewDto>>()
                : null;
        }
        #endregion
    }
}
