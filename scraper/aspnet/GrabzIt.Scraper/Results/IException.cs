using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Scraper.Results
{
    public interface IException
    {
        string Message
        {
            get;
        }
        string Code
        {
            get;
        }
    }
}
