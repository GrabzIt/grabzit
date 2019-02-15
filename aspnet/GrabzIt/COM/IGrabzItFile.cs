using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IGrabzItFile
    {
        byte[] Bytes
        {
            get;
        }
        void Save(string path);
        string ToString();
    }
}
