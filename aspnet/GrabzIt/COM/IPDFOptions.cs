using GrabzIt.Enums;

namespace GrabzIt.COM
{
    interface IPDFOptions
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

        bool NoAds
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
    }
}
