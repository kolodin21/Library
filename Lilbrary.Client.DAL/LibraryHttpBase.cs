namespace Library.Client.Http
{
    public abstract class LibraryHttpBase
    {
        protected static readonly HttpClient Client = new();
        protected static string Host { get; private set; }

        protected LibraryHttpBase()
        {
            Host = "http://localhost:5241";
        }
    }
}