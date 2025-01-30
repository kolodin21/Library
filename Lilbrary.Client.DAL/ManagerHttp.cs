namespace Library.Client.Http
{
    public class ManagerHttp
    {
        public UserHttpClient UserHttpClient { get; private set; }

        public ManagerHttp(UserHttpClient userHttpClient)
        {
            UserHttpClient = userHttpClient;
        }
    }
}
