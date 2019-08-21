using GrabzIt.COM;
using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class HTMLOptions : BaseOptions, IHTMLOptions
    {
        public HTMLOptions()
        {
            RequestAs = BrowserType.StandardBrowser;
            BrowserWidth = 0;
            BrowserHeight = 0;
            Address = string.Empty;
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
        /// The height of the browser in pixels
        /// </summary>
        public int BrowserHeight
        {
            get;
            set;
        }

        /// <summary>
        /// The number of milliseconds to wait before creating the rendered HTML
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
        /// The CSS selector of the HTML element in the web page that must be visible before the capture is performed
        /// </summary>
        public string WaitForElement
        {
            get;
            set;
        }

        /// <summary>
        /// Request the rendered HTML in different forms: Standard Browser, Mobile Browser and Search Engine
        /// </summary>
        public BrowserType RequestAs
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
        /// Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
        /// GrabzIt to perform a HTTP post.
        /// </summary>
        /// <param name="name">The name of the HTTP Post parameter</param>
        /// <param name="value">The value of the HTTP Post parameter</param>
        public void AddPostParameter(string name, string value)
        {
            post = AppendParameter(post, name, value);
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

            return applicationSecret + "|" + urlParam + callBackURLParam +
            "|" + BrowserHeight
            + "|" + BrowserWidth + "|" + CustomId + "|" + delay + "|" + ((int)RequestAs).ToString() + "|" + ConvertCountryToString(Country)
            + "|" + ExportURL + "|" + WaitForElement + "|" + EncryptionKey
             + "|" + Convert.ToInt32(NoAds) + "|" + post + "|" + Proxy + "|" + Address + "|" + Convert.ToInt32(NoCookieNotifications);
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("bwidth", BrowserWidth.ToString());
            parameters.Add("bheight", BrowserHeight.ToString());
            parameters.Add("delay", Delay.ToString());
            parameters.Add("waitfor", WaitForElement);
            parameters.Add("requestmobileversion", Convert.ToInt32(RequestAs).ToString());
            parameters.Add("noads", Convert.ToInt32(NoAds).ToString());
            parameters.Add("post", post);
            parameters.Add("address", Address);
            parameters.Add("nonotify", Convert.ToInt32(NoCookieNotifications).ToString());

            return parameters;
        }
    }
}
