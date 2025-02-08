using System.Net.Http.Json;
using Library.Models;
using Library.Models.ModelsDTO;

namespace Library.Client.Http
{
    public class UserHttpClient : LibraryHttpBase
    {
        #region URI
        private static Uri GetAllUsersUri => new Uri($"{Host}/AllUsers");

        private static Uri GetSingleUserUri => new Uri($"{Host}/SingleUser");

        private static Uri AddUserUri => new Uri($"{Host}/AddUser");

        #endregion

        #region Http

        public async Task<IEnumerable<User>?> GetAllUsers() =>
            await Client.GetFromJsonAsync<IEnumerable<User>>(GetAllUsersUri);

        public async Task<User?> GetSingleUser(Dictionary<string, object> param)
        {
            var response = await Client.PostAsJsonAsync(GetSingleUserUri,param);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<User>()
                : null;
        }

        public async Task AddUser(UserAddDto userAddDto) => await
            Client.PostAsJsonAsync(AddUserUri, userAddDto);

        #endregion
    }
}
