using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.Results
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
