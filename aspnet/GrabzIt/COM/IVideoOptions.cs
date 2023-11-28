using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IVideoOptions
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

        string WaitForElement
        {
            get;
            set;
        }

        BrowserType RequestAs
        {
            get;
            set;
        }

        string CustomWaterMarkId
        {
            get;
            set;
        }

        bool NoAds
        {
            get;
            set;
        }

        bool NoCookieNotifications
        {
            get;
            set;
        }

        string Proxy
        {
            get;
            set;
        }

        string Address
        {
            get;
            set;
        }

        int Start
        {
            get;
            set;
        }

        int Duration
        {
            get;
            set;
        }

        float FramesPerSecond
        {
            get;
            set;
        }

        void AddPostParameter(string name, string value);
    }
}
