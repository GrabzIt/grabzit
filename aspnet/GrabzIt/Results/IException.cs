using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Results
{
    [ComVisible(false)]
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
