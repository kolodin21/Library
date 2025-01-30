using System.Net.Http.Json;
using Library.Models;

namespace Library.Client.Http
{
    public class UserHttpClient : LibraryHttpBase
    {
        private Uri GetAllUsersUri => new Uri($"{Host}/AllUsers");

        private Uri GetSingleUserUri => new Uri($"{Host}/SingleUser");

        private Uri _getSingleUser(Dictionary<string, object> param)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var kvp in param)
            {
                query[kvp.Key] = kvp.Value switch
                {
                    null => "",
                    bool b => b.ToString().ToLower(),
                    DateTime dt => dt.ToString("o"),
                    _ => kvp.Value.ToString()
                };
            }

            return new Uri($"{Host}/SingleUser{(query.Count > 0 ? "?" + query : "")}");
        }

        public async Task<IEnumerable<User>?> GetAllUsers() => await
            Client.GetFromJsonAsync<IEnumerable<User>>(GetAllUsersUri);

        public async Task<User?> GetSingleUser(Dictionary<string, object> param)
        {

            var response = await Client.PostAsJsonAsync($"{Host}/SingleUser", param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<User>()
                : null;
        }
            
           
    }
}
