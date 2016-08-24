using System;
using System.Xml.Serialization;

namespace GrabzIt.Scraper.Results
{
    [Serializable]
    public class GrabzItScrape
    {
        [XmlElement("Identifier")]
        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string NextRun
        {
            get;
            set;
        }

        public GrabzItScrape()
        {
        }
    }
}
