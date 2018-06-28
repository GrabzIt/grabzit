using GrabzIt.COM;
using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GrabzIt.Parameters
{
    [ClassInterface(ClassInterfaceType.None)]
    public class TableOptions : BaseOptions, ITableOptions
    {
        public TableOptions()
        {
            RequestAs = BrowserType.StandardBrowser;
            Format = TableFormat.csv;
            TargetElement = string.Empty;
            TableNumberToInclude = 1;
            IncludeHeaderNames = true;
            IncludeAllTables = false;
            Address = string.Empty;
        }
        /// <summary>
        /// Which table to include, in order from the begining of the page to the end
        /// </summary>
        public int TableNumberToInclude
        {
            get;
            set;
        }
        
        /// <summary>
        /// The format the table should be in: 'csv', 'xlsx' or 'json'
        /// </summary>
        public TableFormat Format
        {
            get;
            set;
        }
        
        /// <summary>
        /// If true header names will be included in the table
        /// </summary>
        public bool IncludeHeaderNames
        {
            get;
            set;
        }
        
        /// <summary>
        /// If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX and JSON format.
        /// </summary>
        public bool IncludeAllTables
        {
            get;
            set;
        }
        
        /// <summary>
        /// The id of the only HTML element in the web page that should be used to extract tables from
        /// </summary>
        public string TargetElement
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
            "|" + CustomId + "|" + TableNumberToInclude + "|" + Convert.ToInt32(IncludeAllTables) + "|" + Convert.ToInt32(IncludeHeaderNames) + "|" + 
            TargetElement + "|" + Format + "|" + (int)RequestAs + "|" + ConvertCountryToString(Country) + "|" + ExportURL + "|" + EncryptionKey + "|" +
            post + "|" + Proxy + "|" + Address;
        }

        protected override Dictionary<string, string> GetParameters(string applicationKey, string signature, string callBackURL, string dataName, string dataValue)
        {
            Dictionary<string, string> parameters = CreateParameters(applicationKey, signature, callBackURL, dataName, dataValue);
		    parameters.Add("includeAllTables", Convert.ToInt32(IncludeAllTables).ToString());
		    parameters.Add("includeHeaderNames", Convert.ToInt32(IncludeHeaderNames).ToString());
		    parameters.Add("format", Format.ToString());
		    parameters.Add("tableToInclude", TableNumberToInclude.ToString());
		    parameters.Add("target", TargetElement);
            parameters.Add("requestmobileversion", Convert.ToInt32(RequestAs).ToString());
            parameters.Add("post", post);
            parameters.Add("address", Address);

            return parameters;
        }
    }
}