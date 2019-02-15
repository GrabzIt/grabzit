using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IGrabzItCookie
    {
        string Name
        {
            get;
        }

        string Value
        {
            get;
        }

        string Domain
        {
            get;
        }

        string Path
        {
            get;
        }

        string HttpOnly
        {
            get;
        }

        string Expires
        {
            get;
        }

        string Type
        {
            get;
        }
    }
}
