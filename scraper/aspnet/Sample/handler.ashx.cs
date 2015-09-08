using GrabzIt.Scraper;
using System;
using System.Collections.Generic;
using System.Web;

namespace Sample
{
    /// <summary>
    /// Summary description for Hhndler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ScrapeResult scrapeResult = new ScrapeResult(context.Request);
            scrapeResult.Save(context.Server.MapPath("~/results/" + scrapeResult.FileName));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}