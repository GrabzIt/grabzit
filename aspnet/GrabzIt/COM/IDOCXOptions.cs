using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.COM
{
    interface IDOCXOptions
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

        bool IncludeBackground
        {
            get;
            set;
        }

        PageSize PageSize
        {
            get;
            set;
        }

        int PageHeight
        {
            get;
            set;
        }

        int PageWidth
        {
            get;
            set;
        }

        PageOrientation Orientation
        {
            get;
            set;
        }

        bool IncludeLinks
        {
            get;
            set;
        }

        bool IncludeImages
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }

        int MarginTop
        {
            get;
            set;
        }

        int MarginLeft
        {
            get;
            set;
        }

        int MarginBottom
        {
            get;
            set;
        }

        int MarginRight
        {
            get;
            set;
        }

        int Delay
        {
            get;
            set;
        }

        BrowserType RequestAs
        {
            get;
            set;
        }

        string TemplateId
        {
            get;
            set;
        }

        int Quality
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

        string MergeId
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

        int BrowserWidth
        {
            get;
            set;
        }

        void AddPostParameter(string name, string value);
        void AddTemplateParameter(string name, string value);
    }
}
