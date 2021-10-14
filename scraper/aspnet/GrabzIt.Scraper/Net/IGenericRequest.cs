namespace GrabzIt.Scraper.Net
{
    interface IGenericRequest
    {
        IGenericFile GetFile();
        string UserAgent { get; }
    }
}
