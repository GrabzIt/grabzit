using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IStatus
    {
        bool Processing
        {
            get;
        }

        bool Cached
        {
            get;
        }

        bool Expired
        {
            get;
        }

        string Message
        {
            get;
        }
    }
}
