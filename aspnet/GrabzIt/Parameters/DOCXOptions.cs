using GrabzIt.COM;
using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class DOCXOptions : BaseOptions, IDOCXOptions
    {
        private string templateVariables = string.Empty;

        public DOCXOptions()
        {
            IncludeBackground = true;
            PageSize = PageSize.A4;
            Orientation = PageOrientation.Portrait;
            IncludeLinks = true;
            IncludeImages = true;
            Title = string.Empty;
            MarginTop = 10;
            MarginLeft = 10;
            MarginBottom = 10;
            MarginRight = 10;
            RequestAs = BrowserType.StandardBrowser;
            Quality = -1;
            HideElement = string.Empty;
            TemplateId = string.Empty;
            TargetElement = string.Empty;
            Address = string.Empty;
            ClickElement = string.Empty;
        }
        /// <summary>
        /// If true background images should be included in the DOCX
        /// </summary>
        public bool IncludeBackground
        {
            get;
            set;
        }

        /// <summary>
        /// The page size of the DOCX to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter', 'Legal'.
        /// </summary>
        public PageSize PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// The width of the browser in pixels
        /// </summary>
        public int BrowserWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The height of the resulting DOCX in mm
        /// </summary>
        public int PageHeight
        {
            get;
            set;
        }

        /// <summary>
        /// The width of the resulting DOCX in mm
        /// </summary>
        public int PageWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Add a PDF template ID that specifies the header and footer of the PDF document
        /// </summary>
        public string TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// The orientation of the DOCX to be returned: 'Landscape' or 'Portrait'
        /// </summary>
        public PageOrientation Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// True if links should be included in the DOCX
        /// </summary>
        public bool IncludeLinks
        {
            get;
            set;
        }

        /// <summary>
        /// True if the images should be included in the DOCX
        /// </summary>
        public bool IncludeImages
        {
            get;
            set;
        }

        /// <summary>
        /// Provide a title to the DOCX document
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The margin that should appear at the top of the DOCX document page
        /// </summary>
        public int MarginTop
        {
            get;
            set;
        }

        /// <summary>
        /// The margin that should appear at the left of the DOCX document page
        /// </summary>
        public int MarginLeft
        {
            get;
            set;
        }

        /// <summary>
        /// The margin that should appear at the bottom of the DOCX document page
        /// </summary>
        public int MarginBottom
        {
            get;
            set;
        }

        /// <summary>
        /// The margin that should appear at the right of the DOCX document
        /// </summary>
        public int MarginRight
        {
            get;
            set;
        }

        /// <summary>
        /// The number of milliseconds to wait before creating the capture
        /// </summary>
        public int Delay
        {

            get
            {
                return delay;
            }
            set
            {
                delay = value;
            }
        }

        /// <summary>
        /// Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
        /// </summary>
        public BrowserType RequestAs
        {
            get;
            set;
        }

        /// <summary>
        /// The quality of the DOCX where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
        /// </summary>
        public int Quality
        {
            get;
            set;
        }

        /// <summary>
        /// The CSS selector(s) of the HTML element in the web page to click
        /// </summary>
        public string ClickElement
        {
            get;
            set;
        }

        /// <summary>
        /// The CSS selector of the only HTML element in the web page to capture
        /// </summary>
        public string TargetElement
        {
            get;
            set;
        }

        /// <summary>
        /// The CSS selector(s) of the one or more HTML elements in the web page to hide
        /// </summary>
        public string HideElement
        {
            get;
            set;
        }

        /// <summary>
        /// The CSS selector of the HTML element in the web page that must be visible before the capture is performed
        /// </summary>
        public string WaitForElement
        {
            get;
            set;
        }

        /// <summary>
        /// The ID of a capture that should be merged at the beginning of the new DOCX document
        /// </summary>
        public string MergeId
        {
            get;
            set;
        }

        /// <summary>
        /// True if adverts should be automatically hidden.
        /// </summary>
        public bool NoAds
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie notification should be automatically hidden.
        /// </summary>
        public bool NoCookieNotifications
        {
            get;
            set;
        }

        /// <summary>
        /// The URL to execute the HTML code in.
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Protect the DOCX document with this password
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
        /// GrabzIt to perform a HTTP post.
        /// </summary>
        /// <param name="name">The name of the HTTP Post parameter</param>
        /// <param name="value">The value of the HTTP Post parameter</param>
        public void AddPostParameter(string name, string value)
        {
            post = AppendParameter(post, name, value);
        }

        /// <summary>
        /// Define a custom template parameter and value, this method can be called multiple times to add multiple parameters.
        /// </summary>
        /// <param name="name">The name of the template parameter</param>
        /// <param name="value">The value of the template parameter</param>
        public void AddTemplateParameter(string name, string value)
        {
            templateVariables = AppendParameter(templateVariables, name, value);
        }

        internal override string GetSignatureString(string applicationSecret, string callBackURL, string url)
        {
            string urlParam = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                urlParam = url + "|";
            }

            string callBackURLParam = string.Empty;
            if (!string.IsNullOrEmpty(callBackURL))
            {
                callBackURLParam = callBackURL;
            }

            return applicationSecret + "|" + urlParam + callBackURLParam + "|"
            + CustomId + "|" + Convert.ToInt32(IncludeBackground) + "|" + PageSize.ToString().ToUpper() + "|" + Orientation + "|" + Convert.ToInt32(IncludeImages) + "|"
            + Convert.ToInt32(IncludeLinks) + "|" + Title + "|" + MarginTop + "|" + MarginLeft + "|"
            + MarginBottom + "|" + MarginRight + "|" + Delay + "|" + (int)RequestAs + "|" + ConvertCountryToString(Country) + "|" + Quality + "|"
            + HideElement + "|" + ExportURL + "|" + WaitForElement + "|" + EncryptionKey + "|" + Convert.ToInt32(NoAds) + "|" + post + "|" +
            TargetElement + "|" + TemplateId + "|" + templateVariables + "|" + PageHeight + "|" + PageWidth + "|" + BrowserWidth + "|" + Proxy + "|" + MergeId + "|" + Address
            + "|" + Convert.ToInt32(NoCookieNotifications) + "|" + Password + "|" + ClickElement;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("background", Convert.ToInt32(IncludeBackground).ToString());
            parameters.Add("pagesize", PageSize.ToString().ToUpper());
            parameters.Add("orientation", Orientation.ToString());
            parameters.Add("includelinks", Convert.ToInt32(IncludeLinks).ToString());
            parameters.Add("includeimages", Convert.ToInt32(IncludeImages).ToString());
            parameters.Add("title", Title);
            parameters.Add("mleft", MarginLeft.ToString());
            parameters.Add("mright", MarginRight.ToString());
            parameters.Add("mtop", MarginTop.ToString());
            parameters.Add("mbottom", MarginBottom.ToString());
            parameters.Add("delay", Delay.ToString());
            parameters.Add("requestmobileversion", Convert.ToInt32(RequestAs).ToString());
            parameters.Add("quality", Quality.ToString());
            parameters.Add("hide", HideElement);
            parameters.Add("waitfor", WaitForElement);
            parameters.Add("noads", Convert.ToInt32(NoAds).ToString());
            parameters.Add("post", post);
            parameters.Add("templateid", TemplateId);
            parameters.Add("target", TargetElement);
            parameters.Add("bwidth", BrowserWidth.ToString());
            parameters.Add("width", PageWidth.ToString());
            parameters.Add("height", PageHeight.ToString());
            parameters.Add("tvars", templateVariables);
            parameters.Add("mergeid", MergeId);
            parameters.Add("address", Address);
            parameters.Add("nonotify", Convert.ToInt32(NoCookieNotifications).ToString());
            parameters.Add("password", Password);
            parameters.Add("click", ClickElement);

            return parameters;
        }
    }
}
