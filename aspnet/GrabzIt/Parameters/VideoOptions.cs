using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using GrabzIt.COM;
using GrabzIt.Enums;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class VideoOptions : BaseOptions, IVideoOptions
    {
        public VideoOptions()
        {
            RequestAs = BrowserType.StandardBrowser;
            CustomWaterMarkId = string.Empty;
            BrowserWidth = 0;
            BrowserHeight = 0;
            Duration = 10;
            Address = string.Empty;
            ClickElement = string.Empty;
            JSCode = string.Empty;
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
        /// The CSS selector(s) of the HTML element in the web page to click
        /// </summary>
        public string ClickElement
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
        /// Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
        /// </summary>
        public BrowserType RequestAs
        {
            get;
            set;
        }

        /// <summary>
        /// Add a custom watermark to the image
        /// </summary>
        public string CustomWaterMarkId
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
        /// The starting time of the web page that should be converted into a video
        /// </summary>
        public int Start
        {
            get;
            set;
        }

        /// <summary>
        /// The length in seconds of the web page that should be converted into a video
        /// </summary>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// The number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10
        /// </summary>
        public float FramesPerSecond
        {
            get;
            set;
        }

        /// <summary>
        /// The height of the resulting video in pixels
        /// </summary>
        public int OutputHeight
        {
            get;
            set;
        }

        /// <summary>
        /// The width of the resulting video in pixels
        /// </summary>
        public int OutputWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The JavaScript code that will be execute in the web page before the capture is performed
        /// </summary>
        public string JSCode
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
            + "|" + BrowserWidth + "|" + CustomId 
            + "|" + CustomWaterMarkId + "|" + Start + "|" + ((int)RequestAs).ToString() + "|" + ConvertCountryToString(Country) 
            + "|" + ExportURL + "|" + WaitForElement + "|" + EncryptionKey
             + "|" + Convert.ToInt32(NoAds) + "|" + post + "|" + Proxy + "|" + Address + "|" + Convert.ToInt32(NoCookieNotifications)
             + "|" + ClickElement + "|" + FramesPerSecond + "|" + Duration + "|" + OutputWidth + "|" + OutputHeight + "|" + JSCode;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("bwidth", BrowserWidth.ToString());
            parameters.Add("customwatermarkid", CustomWaterMarkId);
            parameters.Add("bheight", BrowserHeight.ToString());
            parameters.Add("start", Start.ToString());
            parameters.Add("waitfor", WaitForElement);
            parameters.Add("requestmobileversion", Convert.ToInt32(RequestAs).ToString());
            parameters.Add("noads", Convert.ToInt32(NoAds).ToString());
            parameters.Add("post", post);
            parameters.Add("address", Address);
            parameters.Add("nonotify", Convert.ToInt32(NoCookieNotifications).ToString());
            parameters.Add("click", ClickElement);
            parameters.Add("fps", FramesPerSecond.ToString());
            parameters.Add("duration", Duration.ToString());
            parameters.Add("width", OutputWidth.ToString());
            parameters.Add("height", OutputHeight.ToString());
            parameters.Add("jscode", JSCode);

            return parameters;
        }
    }
}
