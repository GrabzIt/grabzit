using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.COM
{
    interface IPDFOptions
    {
        Country Country
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

        bool IncludeOutline
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }

        string CoverURL
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
    }
}
