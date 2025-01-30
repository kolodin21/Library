using System.Net.Http.Json;
using Library.Models;

namespace Library.Client.Http
{
    public class UserHttpClient : LibraryHttpBase
    {
        #region URI

        private Uri GetAllUsersUri => new Uri($"{Host}/AllUsers");

        private Uri GetSingleUserUri => new Uri($"{Host}/SingleUser");

        #endregion

        #region Http

        public async Task<IEnumerable<User>?> GetAllUsers() => await
            Client.GetFromJsonAsync<IEnumerable<User>>(GetAllUsersUri);


        public async Task<User?> GetSingleUser(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetSingleUserUri,param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<User>()
                : null;
        }
            
        #endregion
           
    }
}
