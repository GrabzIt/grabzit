#if NETSTANDARD
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrabzIt.Scraper.Net
{
    internal class CoreRequest : IGenericRequest
    {
        private readonly HttpRequest request;

        public CoreRequest(HttpRequest request)
        {
            this.request = request;
        }
        public string UserAgent
        {
            get
            {
                if (request == null || !request.Headers.ContainsKey("User-Agent"))
                {
                    return string.Empty;
                }
                return request.Headers["User-Agent"].ToString();
            }
        }

        public IGenericFile GetFile()
        {
            if (request == null)
            {
                return null;
            }
            return new CoreFile(request.Form.Files["file"]);
        }
    }
}
#endif