namespace Library.Client.Http
{
    public class ManagerHttp
    (
        UserHttpClient userHttpClient,
        BookHttpClient bookHttpClient
        )
    {
        public UserHttpClient UserHttpClient { get; private set; } = userHttpClient;
        public BookHttpClient BookHttpClient { get; private set; } = bookHttpClient;
    }
}
