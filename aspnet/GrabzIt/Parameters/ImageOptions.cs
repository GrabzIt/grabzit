using GrabzIt.COM;
using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class ImageOptions : BaseOptions, IImageOptions
    {
        public ImageOptions()
        {
            RequestAs = BrowserType.StandardBrowser;
            CustomWaterMarkId = string.Empty;
            Quality = -1;
            BrowserWidth = 0;
            BrowserHeight = 0;
            OutputWidth = 0;
            OutputHeight = 0;
            Format = ImageFormat.jpg;
            TargetElement = string.Empty;
            HideElement = string.Empty;
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
        /// The height of the resulting thumbnail in pixels
        /// </summary>
        public int OutputHeight
        {
            get;
            set;
        }

        /// <summary>
        /// The width of the resulting thumbnail in pixels
        /// </summary>
        public int OutputWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
        /// </summary>
        public ImageFormat Format
        {
            get;
            set;
        }

        /// <summary>
        /// The number of milliseconds to wait before taking the screenshot
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
        /// The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality for the specified image format
        /// </summary>
        public int Quality
        {
            get;
            set;
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
            "|" + Format + "|" + OutputHeight + "|" + OutputWidth + "|" + BrowserHeight
            + "|" + BrowserWidth + "|" + CustomId + "|" + delay + "|" + TargetElement
            + "|" + CustomWaterMarkId + "|" + ((int)RequestAs).ToString() + "|" + ConvertCountryToString(Country) + "|" +
            Quality + "|" + HideElement;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("width", OutputWidth.ToString());
            parameters.Add("height", OutputHeight.ToString());
            parameters.Add("format", Format.ToString());
            parameters.Add("bwidth", BrowserWidth.ToString());
            parameters.Add("customwatermarkid", CustomWaterMarkId);
            parameters.Add("bheight", BrowserHeight.ToString());
            parameters.Add("delay", Delay.ToString());
            parameters.Add("target", TargetElement);
            parameters.Add("hide", HideElement);
            parameters.Add("requestmobileversion", Convert.ToInt32(RequestAs).ToString());
            parameters.Add("quality", Quality.ToString());

            return parameters;
        }
    }
}
