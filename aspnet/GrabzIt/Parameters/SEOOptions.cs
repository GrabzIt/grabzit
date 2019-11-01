using GrabzIt.COM;
using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class SEOOptions : BaseOptions, ISEOOptions
    {
        public SEOOptions()
        {
            BrowserWidth = 0;
            BrowserHeight = 0;
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
        /// True if adverts should be automatically hidden.
        /// </summary>
        public bool NoAds
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
            "|" + BrowserHeight
            + "|" + BrowserWidth + "|" + CustomId + "|" + delay + "|" + ConvertCountryToString(Country)
            + "|" + ExportURL + "|" + EncryptionKey
             + "|" + Convert.ToInt32(NoAds) + "|" + Proxy;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
            parameters.Add("bwidth", BrowserWidth.ToString());
            parameters.Add("bheight", BrowserHeight.ToString());
            parameters.Add("delay", Delay.ToString());
            parameters.Add("noads", Convert.ToInt32(NoAds).ToString());

            return parameters;
        }
    }
}
