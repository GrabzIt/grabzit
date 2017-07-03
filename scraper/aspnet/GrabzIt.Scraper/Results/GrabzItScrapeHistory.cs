using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GrabzIt.Scraper.Results
{
    public class GrabzItScrapeHistory
    {
        [XmlElement("Identifier")]
        public string ID
        {
            get;
            set;
        }

        public string Finished
        {
            get;
            set;
        }

    }
}
