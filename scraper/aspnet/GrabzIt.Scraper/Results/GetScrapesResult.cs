using System;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using GrabzIt.Scraper.Results;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GetScrapesResult : IException
    {
        [XmlArray("Scrapes")]
        [XmlArrayItem("Scrape")]
        public GrabzItScrape[] Scrapes;

        public string Message
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public GetScrapesResult()
        {            
        }
    }
}