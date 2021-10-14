using System.IO;

namespace GrabzIt.Scraper.Net
{
    interface IGenericFile
    {
        string FileName { get; }
        Stream Data { get; }
    }
}
