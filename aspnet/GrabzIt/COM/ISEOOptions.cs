using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface ISEOOptions
    {
        Country Country
        {
            get;
            set;
        }

        string ExportURL
        {
            get;
            set;
        }

        string EncryptionKey
        {
            get;
            set;
        }

        string CustomId
        {
            get;
            set;
        }

        int BrowserWidth
        {
            get;
            set;
        }

        int BrowserHeight
        {
            get;
            set;
        }

        int Delay
        {
            get;
            set;
        }

        bool NoAds
        {
            get;
            set;
        }

        string Proxy
        {
            get;
            set;
        }
    }
}
