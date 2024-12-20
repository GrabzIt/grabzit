﻿using System;
using System.Net;

namespace GrabzIt.Scraper.Net
{
    internal class QuickWebClient : WebClient
    {
        private WebProxy proxy = null;

        internal QuickWebClient(WebProxy proxy)
        {
            this.proxy = proxy;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            if (request != null)
            {
                request.Proxy = this.proxy;
                request.KeepAlive = false;
            }
            return request;
        }
    }
}
