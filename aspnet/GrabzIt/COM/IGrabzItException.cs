using GrabzIt.Enums;
using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IGrabzItException
    {
        ErrorCode Code
        {
            get;
        }
    }
}
