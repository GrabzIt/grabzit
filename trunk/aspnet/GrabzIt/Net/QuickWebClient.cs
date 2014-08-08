using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GrabzIt.Net
{
    internal class QuickWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            if (request != null)
            {
                request.Proxy = null;
                request.KeepAlive = false;
            }
            return request;
        }
    }
}
