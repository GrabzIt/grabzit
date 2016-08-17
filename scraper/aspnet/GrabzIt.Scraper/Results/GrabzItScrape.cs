using System;

namespace GrabzIt.Scraper.Results
{
    [Serializable]
    public class GrabzItScrape
    {
        public string Identifier
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
