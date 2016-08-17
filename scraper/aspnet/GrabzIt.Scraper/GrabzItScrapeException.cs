using System;
using GrabzIt.Scraper.Enums;
using System.Runtime.InteropServices;

namespace GrabzIt.Scraper
{
    public class GrabzItScrapeException : Exception
    {
        public ErrorCode code;

        internal GrabzItScrapeException(string message, string code)
            : this(message, (ErrorCode)Convert.ToInt32(code))
        {
        }

        internal GrabzItScrapeException(string message, ErrorCode code)
            : base(message)
        {
            this.code = code;
        }

        public ErrorCode Code
        {
            get
            {
                return code;
            }
            set
            {
                Code = code;
            }
        }
    }
}
