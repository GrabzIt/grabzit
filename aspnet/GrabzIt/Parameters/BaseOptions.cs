using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GrabzIt.Parameters
{
    public abstract class BaseOptions
    {
        protected int delay = 0;

        public BaseOptions()
        {
            Country = Country.Default;
            CustomId = string.Empty;
            ExportURL = string.Empty;
        }

        /// <summary>
        /// Request the screenshot from different countries: Default, UK or US
        /// </summary>
        public Country Country
        {
            get;
            set;
        }

        /// <summary>
        /// A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified.
        /// </summary>
        public string CustomId
        {
            get;
            set;
        }

        /// <summary>
        /// The export URL that should be used to transfer the capture to a third party location.
        /// </summary>
        public string ExportURL
        {
            get;
            set;
        }

        protected Dictionary<string, string> CreateParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
	    {
		    Dictionary<string, string> parameters = new Dictionary<string, string>();
		    parameters.Add("key", applicationKey);
            parameters.Add("country", ConvertCountryToString(Country));
		    parameters.Add("customid", CustomId);
            parameters.Add("export", ExportURL);
            parameters.Add("callback", callBackURL);
		    parameters.Add("sig", signature);		
		    parameters.Add(dataName, dataValue);

            return parameters;
	    }

        protected string ConvertCountryToString(Country country)
        {
            if (country == Country.Default)
            {
                return string.Empty;
            }
            if (country == Country.Singapore)
            {
                return "SG";
            }
            return country.ToString();
        }

        internal int GetStartDelay()
        {
            return delay;
        }

        internal string GetSignatureString(string applicationSecret, string callBackURL)
        {
            return GetSignatureString(applicationSecret, callBackURL, null);
        }

        internal virtual string GetSignatureString(string applicationSecret, string callBackURL, string url)
        {
            return string.Empty;
        }

        protected virtual Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            return new Dictionary<string, string>();
        }

        internal string GetQueryString(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = GetParameters(applicationKey, signature, callBackURL, dataName, dataValue);

            StringBuilder sb = new StringBuilder();            
            foreach(KeyValuePair<string, string> kvp in parameters)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(kvp.Key);
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(kvp.Value));
            }

            return sb.ToString();
        }
    }
}
