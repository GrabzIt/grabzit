#if NETFRAMEWORK
using System.Web;

namespace GrabzIt.Scraper.Net
{
    internal class FrameworkRequest : IGenericRequest
    {
        private readonly HttpRequestBase httpRequestBase;

        public FrameworkRequest(HttpRequestBase httpRequestBase)
        {
            this.httpRequestBase = httpRequestBase;
        }
        public string UserAgent
        {
            get
            {
                if (httpRequestBase == null)
                {
                    return string.Empty;
                }
                return httpRequestBase.UserAgent;
            }
        }

        public IGenericFile GetFile()
        {
            if (httpRequestBase == null)
            {
                return null;
            }
            return new FrameworkFile(httpRequestBase.Files["file"]);
        }
    }
}
#endif