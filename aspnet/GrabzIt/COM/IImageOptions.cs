using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.COM
{
    interface IImageOptions
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

        int OutputHeight
        {
            get;
            set;
        }

        int OutputWidth
        {
            get;
            set;
        }

        ImageFormat Format
        {
            get;
            set;
        }

        int Delay
        {
            get;
            set;
        }

        string TargetElement
        {
            get;
            set;
        }

        string HideElement
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
    
        int Quality
        {
            get;
            set;
        }

        bool Transparent
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

        string Address
        {
            get;
            set;
        }

        void AddPostParameter(string name, string value);
    }
}
