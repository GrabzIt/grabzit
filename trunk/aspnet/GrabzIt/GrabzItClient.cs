using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Serialization;
using System.Collections.Specialized;
using GrabzIt.Cookies;
using GrabzIt.Results;
using GrabzIt.Enums;
using GrabzIt.Screenshots;
using GrabzIt.Net;

namespace GrabzIt
{
    public class GrabzItClient
    {
        private static GrabzItClient grabzItClient;        

        public delegate void ScreenShotHandler(object sender, ScreenShotEventArgs result);

        private event ScreenShotHandler screenShotComplete;
        /// <summary>
        /// Only one screenshot event handler can be set, due to the possibility of multiple event handlers being assigned before a response is recieved. 
        /// </summary>
        public event ScreenShotHandler ScreenShotComplete
        {
            add
            {
                lock (eventLock)
                {
                    if ((screenShotComplete == null || screenShotComplete.GetInvocationList().Length == 0) && value.GetInvocationList().Length == 1)
                    {
                        screenShotComplete += value;
                    }
                }
            }
            remove
            {
                lock (eventLock)
                {
                    screenShotComplete -= value;
                }
            }
        }

        private GrabzItRequest request;
        private Object thisLock = new Object();
        private Object eventLock = new Object();

        public string ApplicationKey
        {
            get;
            set;
        }

        public string ApplicationSecret
        {
            get;
            set;
        }

        private const string BaseURL = "http://grabz.it/services/";

        /// <summary>
        /// Create a new GrabzIt client.
        /// </summary>
        /// <param name="applicationKey">The application key of your GrabzIt account</param>
        /// <param name="applicationSecret">The application secret of your GrabzIt account</param>
        public GrabzItClient(string applicationKey, string applicationSecret) : this(applicationKey, applicationSecret, false)
        {
        }

        private GrabzItClient(string applicationKey, string applicationSecret, bool isStatic)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationSecret = applicationSecret;
            request = new GrabzItRequest(isStatic);
        }

        internal void OnScreenShotComplete(object sender, ScreenShotEventArgs result)
        {
            if (screenShotComplete != null)
            {
                screenShotComplete(this, result);
            }
        }

        /// <summary>
        /// This method sets the parameters required to turn a online video into a animated GIF
        /// </summary>
        /// <param name="url">The URL of the online video</param>
        public void SetAnimationOptions(string url)
        {
            SetAnimationOptions(url, string.Empty, 0, 0, 0, 0, 0, 0, 0, false, string.Empty, -1, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to turn a online video into a animated GIF
        /// </summary>
        /// <param name="url">The URL of the online video</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified</param>
        public void SetAnimationOptions(string url, string customId)
        {
            SetAnimationOptions(url, customId, 0, 0, 0, 0, 0, 0, 0, false, string.Empty, -1, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to turn a online video into a animated GIF
        /// </summary>
        /// <param name="url">The URL of the online video</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified</param>
        /// <param name="width">The width of the resulting animated GIF in pixels</param>
        /// <param name="height">The height of the resulting animated GIF in pixels</param>
        /// <param name="start">The starting position of the video that should be converted into a animated GIF</param>
        /// <param name="duration">The length in seconds of the video that should be converted into a animated GIF</param>
        /// <param name="speed">The speed of the animated GIF from 0.2 to 10 times the original speed</param>
        /// <param name="framesPerSecond">The number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60</param>
        /// <param name="repeat">The number of times to loop the animated GIF. If 0 it will loop infinitley</param>
        /// <param name="reverse">If true the frames of the animated GIF are reversed</param>
        /// <param name="customWaterMarkId">Add a custom watermark to the animated GIF</param>
        public void SetAnimationOptions(string url, string customId, int width, int height, int start, int duration, float speed, float framesPerSecond, int repeat, bool reverse, string customWaterMarkId)
        {
            SetAnimationOptions(url, customId, width, height, start, duration, speed, framesPerSecond, repeat, reverse, customWaterMarkId, -1, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to turn a online video into a animated GIF
        /// </summary>
        /// <param name="url">The URL of the online video</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified</param>
        /// <param name="width">The width of the resulting animated GIF in pixels</param>
        /// <param name="height">The height of the resulting animated GIF in pixels</param>
        /// <param name="start">The starting position of the video that should be converted into a animated GIF</param>
        /// <param name="duration">The length in seconds of the video that should be converted into a animated GIF</param>
        /// <param name="speed">The speed of the animated GIF from 0.2 to 10 times the original speed</param>
        /// <param name="framesPerSecond">The number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60</param>
        /// <param name="repeat">The number of times to loop the animated GIF. If 0 it will loop forever</param>
        /// <param name="reverse">If true the frames of the animated GIF are reversed</param>
        /// <param name="customWaterMarkId">Add a custom watermark to the animated GIF</param>
        /// <param name="quality">The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality</param>
        /// <param name="country">Request the screenshot from different countries: Default, UK or US</param>
        public void SetAnimationOptions(string url, string customId, int width, int height, int start, int duration, float speed, float framesPerSecond, int repeat, bool reverse, string customWaterMarkId, int quality, Country country)
        {
            lock (thisLock)
            {
                request.StartDelay = 0;
                request.Request = string.Format("{0}takeanimation.ashx?url={1}&key={2}&width={3}&height={4}&duration={5}&speed={6}&start={7}&customid={8}&fps={9}&repeat={10}&customwatermarkid={11}&reverse={12}&country={13}&quality={14}&callback=",
                                                              BaseURL, HttpUtility.UrlEncode(url), ApplicationKey, width, height, duration, speed,
                                                              start, HttpUtility.UrlEncode(customId), framesPerSecond, repeat, HttpUtility.UrlEncode(customWaterMarkId), Convert.ToInt32(reverse), ConvertCountryToString(country), quality);
                request.SignaturePartOne = ApplicationSecret + "|" + url + "|";
                request.SignaturePartTwo = "|" + height + "|" + width + "|" + customId + "|" + framesPerSecond + "|" + speed + "|" + duration + "|" + repeat + "|" + Convert.ToInt32(reverse) + "|" + start + "|" + customWaterMarkId + "|" + ConvertCountryToString(country) + "|" + quality;
            }
        }

        /// <summary>
        /// This method sets the parameters required to take a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="format">The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="targetElement">The id of the only HTML element in the web page to turn into a screenshot</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        /// <param name="customWaterMarkId">Add a custom watermark to the image</param>
        /// <param name="quality">The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality for the specified image format</param>
        /// <param name="country">Request the screenshot from different countries: Default, UK or US</param>
        public void SetImageOptions(string url, string customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, string targetElement, BrowserType requestAs, string customWaterMarkId, int quality, Country country)
        {
            lock (thisLock)
            {
                request.StartDelay = delay;
                request.Request = string.Format("{0}takepicture.ashx?url={1}&key={2}&width={3}&height={4}&bwidth={5}&bheight={6}&format={7}&customid={8}&delay={9}&target={10}&customwatermarkid={11}&requestmobileversion={12}&country={13}&quality={14}&callback=",
                                                              BaseURL, HttpUtility.UrlEncode(url), ApplicationKey, outputWidth, outputHeight,
                                                              browserWidth, browserHeight, format, HttpUtility.UrlEncode(customId), delay, HttpUtility.UrlEncode(targetElement), HttpUtility.UrlEncode(customWaterMarkId), (int)requestAs, ConvertCountryToString(country), quality);
                request.SignaturePartOne = ApplicationSecret + "|" + url + "|";
                request.SignaturePartTwo = "|" + format + "|" + outputHeight + "|" + outputWidth + "|" + browserHeight + "|" + browserWidth + "|" + customId + "|" + delay + "|" + targetElement + "|" + customWaterMarkId + "|" + (int)requestAs + "|" + ConvertCountryToString(country) + "|" + quality;
            }
        }

        /// <summary>
        /// This method sets the parameters required to take a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        public void SetImageOptions(string url)
        {
            SetImageOptions(url, string.Empty, 0, 0, 0, 0, ImageFormat.jpg, 0, string.Empty, BrowserType.StandardBrowser, string.Empty);
        }

        /// <summary>
        /// This method sets the parameters required to take a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>        
        public void SetImageOptions(string url, string customId)
        {
            SetImageOptions(url, customId, 0, 0, 0, 0, ImageFormat.jpg, 0, string.Empty, BrowserType.StandardBrowser, string.Empty);
        }

        /// <summary>
        /// This method sets the parameters required to take a screenshot of a web page.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="format">The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="targetElement">The id of the only HTML element in the web page to turn into a screenshot</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        /// <param name="customWaterMarkId">Add a custom watermark to the image</param>
        public void SetImageOptions(string url, string customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, string targetElement, BrowserType requestAs, string customWaterMarkId)
        {
            SetImageOptions(url, customId, browserWidth, browserHeight, outputWidth, outputHeight, format, delay, targetElement, requestAs, customWaterMarkId, -1, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to extract all tables from a web page.
        /// </summary>
        /// <param name="url">The URL that the should be used to extract tables</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="tableNumberToInclude">Which table to include, in order from the begining of the page to the end</param>
        /// <param name="format">The format the tableshould be in: csv, xlsx</param>
        /// <param name="includeHeaderNames">If true header names will be included in the table</param>
        /// <param name="includeAllTables">If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX format.</param>
        /// <param name="targetElement">The id of the only HTML element in the web page that should be used to extract tables from</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        /// <param name="country">Request the screenshot from different countries: Default, UK or US</param>
        public void SetTableOptions(string url, string customId, int tableNumberToInclude, TableFormat format, bool includeHeaderNames, bool includeAllTables, string targetElement, BrowserType requestAs, Country country)
        {
            lock (thisLock)
            {
                request.StartDelay = 0;
                request.Request = BaseURL + "taketable.ashx?key=" + HttpUtility.UrlEncode(ApplicationKey) + "&url=" + HttpUtility.UrlEncode(url) + "&includeAllTables=" + Convert.ToInt32(includeAllTables) + "&includeHeaderNames=" + Convert.ToInt32(includeHeaderNames) + "&format=" + format + "&tableToInclude=" + tableNumberToInclude + "&customid=" + HttpUtility.UrlEncode(customId) + "&target=" + HttpUtility.UrlEncode(targetElement) + "&requestmobileversion=" + (int)requestAs + "&country=" + ConvertCountryToString(country) + "&callback=";
                request.SignaturePartOne = ApplicationSecret + "|" + url + "|";
                request.SignaturePartTwo = "|" + customId + "|" + tableNumberToInclude + "|" + Convert.ToInt32(includeAllTables) + "|" + Convert.ToInt32(includeHeaderNames) + "|" + targetElement + "|" + format + "|" + (int)requestAs + "|" + ConvertCountryToString(country);
            }
        }

        /// <summary>
        /// This method sets the parameters required to extract all tables from a web page.
        /// </summary>
        /// <param name="url">The URL that the should be used to extract tables</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="tableNumberToInclude">Which table to include, in order from the begining of the page to the end</param>
        /// <param name="format">The format the tableshould be in: csv, xlsx</param>
        /// <param name="includeHeaderNames">If true header names will be included in the table</param>
        /// <param name="includeAllTables">If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX format.</param>
        /// <param name="targetElement">The id of the only HTML element in the web page that should be used to extract tables from</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        public void SetTableOptions(string url, string customId, int tableNumberToInclude, TableFormat format, bool includeHeaderNames, bool includeAllTables, string targetElement, BrowserType requestAs)
        {
            SetTableOptions(url, customId, tableNumberToInclude, format, includeHeaderNames, includeAllTables, targetElement, requestAs, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to extract all tables from a web page.
        /// </summary>
        /// <param name="url">The URL that the should be used to extract tables</param>
        public void SetTableOptions(string url)
        {
            SetTableOptions(url, string.Empty, 1, TableFormat.csv, true, false, string.Empty, BrowserType.StandardBrowser, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to extract all tables from a web page.
        /// </summary>
        /// <param name="url">The URL that the should be used to extract tables</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>
        public void SetTableOptions(string url, string customId)
        {
            SetTableOptions(url, string.Empty, 1, TableFormat.csv, true, false, string.Empty, BrowserType.StandardBrowser, Country.Default);
        }

        /// <summary>
        /// This method sets the parameters required to convert a web page into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a pdf</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="includeBackground">If true the background of the web page should be included in the screenshot</param>
        /// <param name="pagesize">The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5', 'Letter'.</param>
        /// <param name="orientation">The orientation of the PDF to be returned: 'Landscape' or 'Portrait'</param>
        /// <param name="includeLinks">True if links should be included in the PDF</param>
        /// <param name="includeOutline">True if the PDF outline should be included</param>
        /// <param name="title">Provide a title to the PDF document</param>
        /// <param name="coverURL">The URL of a web page that should be used as a cover page for the PDF</param>
        /// <param name="marginTop">The margin that should appear at the top of the PDF document page</param>
        /// <param name="marginLeft">The margin that should appear at the left of the PDF document page</param>
        /// <param name="marginBottom">The margin that should appear at the bottom of the PDF document page</param>
        /// <param name="marginRight">The margin that should appear at the right of the PDF document</param>                
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        /// <param name="customWaterMarkId">Add a custom watermark to each page of the PDF document</param>
        /// <param name="quality">The quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality</param>        
        /// <param name="country">Request the screenshot from different countries: Default, UK or US</param>
        public void SetPDFOptions(string url, string customId, bool includeBackground, PageSize pagesize, PageOrientation orientation, bool includeLinks, bool includeOutline, string title, string coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, string customWaterMarkId, int quality, Country country)
        {
            lock (thisLock)
            {
                request.StartDelay = delay;
                request.Request = BaseURL + "takepdf.ashx?key=" + HttpUtility.UrlEncode(ApplicationKey) + "&url=" + HttpUtility.UrlEncode(url) + "&background=" + Convert.ToInt32(includeBackground) + "&pagesize=" + pagesize + "&orientation=" + orientation + "&customid=" + HttpUtility.UrlEncode(customId) + "&customwatermarkid=" + HttpUtility.UrlEncode(customWaterMarkId) + "&includelinks=" + Convert.ToInt32(includeLinks) + "&includeoutline=" + Convert.ToInt32(includeOutline) + "&title=" + HttpUtility.UrlEncode(title) + "&coverurl=" + HttpUtility.UrlEncode(coverURL) + "&mleft=" + marginLeft + "&mright=" + marginRight + "&mtop=" + marginTop + "&mbottom=" + marginBottom + "&delay=" + delay + "&requestmobileversion=" + (int)requestAs + "&country=" + ConvertCountryToString(country) + "&quality=" + quality + "&callback=";
                request.SignaturePartOne = ApplicationSecret + "|" + url + "|";
                request.SignaturePartTwo = "|" + customId + "|" + Convert.ToInt32(includeBackground) + "|" + pagesize.ToString().ToUpper() + "|" + orientation + "|" + customWaterMarkId + "|" + Convert.ToInt32(includeLinks) + "|" + Convert.ToInt32(includeOutline) + "|" + title + "|" + coverURL + "|" + marginTop + "|" + marginLeft + "|" + marginBottom + "|" + marginRight + "|" + delay + "|" + (int)requestAs + "|" + ConvertCountryToString(country) + "|" + quality;
            }
        }

        /// <summary>
        /// This method sets the parameters required to convert a web page into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a pdf</param>
        public void SetPDFOptions(string url)
        {
            SetPDFOptions(url, string.Empty, true, PageSize.A4, PageOrientation.Portrait, true, false, string.Empty, string.Empty, 10, 10, 10, 10, 0, BrowserType.StandardBrowser, string.Empty);
        }

        /// <summary>
        /// This method sets the parameters required to convert a web page into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a pdf</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>        
        public void SetPDFOptions(string url, string customId)
        {
            SetPDFOptions(url, customId, true, PageSize.A4, PageOrientation.Portrait, true, false, string.Empty, string.Empty, 10, 10, 10, 10, 0, BrowserType.StandardBrowser, string.Empty);
        }

        /// <summary>
        /// This method sets the parameters required to convert a web page into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a pdf</param>
        /// <param name="customId">A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="includeBackground">If true the background of the web page should be included in the screenshot</param>
        /// <param name="pagesize">The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5', 'Letter'.</param>
        /// <param name="orientation">The orientation of the PDF to be returned: 'Landscape' or 'Portrait'</param>
        /// <param name="includeLinks">True if links should be included in the PDF</param>
        /// <param name="includeOutline">True if the PDF outline should be included</param>
        /// <param name="title">Provide a title to the PDF document</param>
        /// <param name="coverURL">The URL of a web page that should be used as a cover page for the PDF</param>
        /// <param name="marginTop">The margin that should appear at the top of the PDF document page</param>
        /// <param name="marginLeft">The margin that should appear at the left of the PDF document page</param>
        /// <param name="marginBottom">The margin that should appear at the bottom of the PDF document page</param>
        /// <param name="marginRight">The margin that should appear at the right of the PDF document</param>                
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="requestAs">Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine</param>
        /// <param name="customWaterMarkId">Add a custom watermark to each page of the PDF document</param>
        public void SetPDFOptions(string url, string customId, bool includeBackground, PageSize pagesize, PageOrientation orientation, bool includeLinks, bool includeOutline, string title, string coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, string customWaterMarkId)
        {
            SetPDFOptions(url, customId, includeBackground, pagesize, orientation, includeLinks, includeOutline, title, coverURL, marginTop, marginLeft, marginBottom, marginRight, delay, requestAs, customWaterMarkId, -1, Country.Default);
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot
        /// </summary>
        /// <remarks>
        /// This is the recommended method of saving a screenshot
        /// 
        /// The handler will be passed a URL with the following query string parameters:
        ///  - message (is any error message associated with the screenshot)
        ///  - customId (is a custom id you may have specified in the {#set_image_options}, {#set_table_options} or {#set_pdf_options} method)
        ///  - id (is the unique id of the screenshot which can be used to retrieve the screenshot with the {#get_result} method)
        ///  - filename (is the filename of the screenshot)
        ///  - format (is the format of the screenshot)
        /// </remarks>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetResult method</returns>
        public string Save()
        {
            return Save(string.Empty);
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot
        /// </summary>
        /// <remarks>
        /// This is the recommended method of saving a screenshot
        /// 
        /// The handler will be passed a URL with the following query string parameters:
        ///  - message (is any error message associated with the screenshot)
        ///  - customId (is a custom id you may have specified in the {#set_image_options}, {#set_table_options} or {#set_pdf_options} method)
        ///  - id (is the unique id of the screenshot which can be used to retrieve the screenshot with the {#get_result} method)
        ///  - filename (is the filename of the screenshot)
        ///  - format (is the format of the screenshot)
        /// </remarks>
        /// <param name="callBackURL">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetResult method</returns>
        public string Save(string callBackURL)
        {
            lock (thisLock)
            {
                if (string.IsNullOrEmpty(request.SignaturePartOne) && string.IsNullOrEmpty(request.SignaturePartTwo) && string.IsNullOrEmpty(request.Request))
                {
                    throw new GrabzItException("No screenshot parameters have been set.", ErrorCode.ParameterMissingParameters);
                }
                string sig = Encrypt(request.SignaturePartOne + callBackURL + request.SignaturePartTwo);
                //this allows multiple calls with the same parameters
                string currentRequest = request.Request;
                currentRequest += HttpUtility.UrlEncode(callBackURL) + "&sig=" + HttpUtility.UrlEncode(sig);

                TakePictureResult webResult = Get<TakePictureResult>(currentRequest);

                CheckForException(webResult);

                return webResult.ID;
            }
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot and returns a GrabzItFile object
        /// </summary>
        /// <remarks>
        /// Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
        /// </remarks>
        /// <returns>Returns a GrabzItFile object containing the screenshot data.</returns>
        public GrabzItFile SaveTo()
        {
            lock (thisLock)
            {
                string id = Save();

                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }

                //Wait until it is possible to be ready
                Thread.Sleep(3000 + request.StartDelay);

                //Wait for it to be ready.
                while (true)
                {
                    Status status = GetStatus(id);

                    if (!status.Cached && !status.Processing)
                    {
                        throw new GrabzItException("The screenshot did not complete with the error: " + status.Message, ErrorCode.RenderingError);
                    }

                    if (status.Cached)
                    {
                        GrabzItFile result = GetResult(id);

                        if (result == null)
                        {
                            throw new GrabzItException("The screenshot could not be found on GrabzIt.", ErrorCode.RenderingMissingScreenshot);
                        }

                        return result;
                    }

                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot and saves it to the target path provided
        /// </summary>
        /// <remarks>
        /// Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
        /// </remarks>
        /// <param name="saveToFile">The file path that the screenshot should saved to.</param>
        /// <returns>Returns the true if it is successful otherwise it throws an exception.</returns>
        public bool SaveTo(string saveToFile)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrabzItFile result = SaveTo();

                    if (result == null)
                    {
                        return false;
                    }

                    result.Save(saveToFile);
                    break;
                }
                catch (Exception)
                {
                    if (attempt < 3)
                    {
                        attempt++;
                        continue;
                    }
                    throw new GrabzItException("An error occurred trying to save the screenshot to: " +
                                        saveToFile, ErrorCode.FileSaveError);
                }
            }
            return true;
        }

        private T Get<T>(string url)
        {
            using (QuickWebClient client = new QuickWebClient())
            {
                try
                {
                    string result = client.DownloadString(url);
                    return DeserializeResult<T>(result);
                }
                catch (WebException e)
                {
                    HandleWebException(e);
                    return default(T);
                }
            }
        }

        private void HandleWebException(WebException e)
        {
            if (e == null)
            {
                return;
            }

            if (e.Status == WebExceptionStatus.ProtocolError)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Forbidden)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        throw new GrabzItException(reader.ReadToEnd(), ErrorCode.NetworkDDOSAttack);
                    }
                }
                else if (((int)response.StatusCode) >= 400)
                {
                    throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NetworkGeneralError);
                }
            }
            else if (e.Status == WebExceptionStatus.NameResolutionFailure)
            {
                throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NetworkGeneralError);
            }
        }

        private T DeserializeResult<T>(string result)
        {
            using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memStream);
            }
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        [Obsolete("Use the SetImageOptions and Save methods instead")]
        public string TakePicture(string url)
        {
            return TakePicture(url, string.Empty, 0, 0, 0, 0, string.Empty, ImageFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        [Obsolete("Use the SetImageOptions and Save methods instead")]
        public string TakePicture(string url, string callback)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, string.Empty, ImageFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        [Obsolete("Use the SetImageOptions and Save methods instead")]
        public string TakePicture(string url, string callback, string customId)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, customId, ImageFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="format">The format the screenshot should be in.</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        [Obsolete("Use the SetImageOptions and Save methods instead")]
        public string TakePicture(string url, string callback, int browserWidth, int browserHeight, int outputHeight, int outputWidth, string customId, ImageFormat format, int delay)
        {
            return TakePicture(url, callback, browserWidth, browserHeight, outputHeight, outputWidth, customId, format, delay, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <param name="format">The format the screenshot should be in.</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="targetElement">The id of the only HTML element in the web page to take a screenshot of.</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        [Obsolete("Use the SetImageOptions and Save methods instead")]
        public string TakePicture(string url, string callback, int browserWidth, int browserHeight, int outputHeight, int outputWidth, string customId, ImageFormat format, int delay, string targetElement)
        {
            SetImageOptions(url, customId, browserWidth, browserHeight, outputWidth, outputHeight, format, delay, targetElement, 0, string.Empty);
            return Save(callback);
        }

        /// <summary>
        /// This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="saveToFile">The file path that the screenshot should saved to: e.g. images/test.jpg</param>
        /// <returns>This function returns the true if it is successfull otherwise it throws an exception.</returns>
        [Obsolete("Use the SetImageOptions and SaveTo methods instead")]
        public bool SavePicture(string url, string saveToFile)
        {
            return SavePicture(url, saveToFile, 0, 0, 0, 0, ImageFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="saveToFile">The file path that the screenshot should saved to: e.g. images/test.jpg</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="format">The format the screenshot should be in.</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <returns>This function returns the true if it is successfull otherwise it throws an exception.</returns>
        [Obsolete("Use the SetImageOptions and SaveTo methods instead")]
        public bool SavePicture(string url, string saveToFile, int browserWidth, int browserHeight, int outputHeight, int outputWidth, ImageFormat format, int delay)
        {
            return SavePicture(url, saveToFile, browserWidth, browserHeight, outputHeight, outputWidth, format, delay, string.Empty);
        }


        /// <summary>
        /// This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="saveToFile">The file path that the screenshot should saved to: e.g. images/test.jpg</param>
        /// <param name="browserWidth">The width of the browser in pixels</param>
        /// <param name="browserHeight">The height of the browser in pixels</param>
        /// <param name="outputHeight">The height of the resulting thumbnail in pixels</param>
        /// <param name="outputWidth">The width of the resulting thumbnail in pixels</param>
        /// <param name="format">The format the screenshot should be in.</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <param name="targetElement">The id of the only HTML element in the web page to take a screenshot of.</param>
        /// <returns>This function returns the true if it is successfull otherwise it throws an exception.</returns>
        [Obsolete("Use the SetImageOptions and SaveTo methods instead")]
        public bool SavePicture(string url, string saveToFile, int browserWidth, int browserHeight, int outputHeight, int outputWidth, ImageFormat format, int delay, string targetElement)
        {
            SetImageOptions(url, string.Empty, browserWidth, browserHeight, outputWidth, outputHeight, format, delay, targetElement, BrowserType.StandardBrowser, string.Empty);
            return SaveTo(saveToFile);
        }

        /// <summary>
        /// Get the current status of a GrabzIt screenshot
        /// </summary>
        /// <param name="id">The id of the screenshot</param>
        /// <returns>A Status object representing the screenshot</returns>
        public Status GetStatus(string id)
        {
            lock (thisLock)
            {
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }

                string url = string.Format("{0}getstatus.ashx?id={1}",
                                                          BaseURL, id);
                GetStatusResult webResult = Get<GetStatusResult>(url);

                if (webResult == null)
                {
                    return null;
                }

                return webResult.GetStatus();
            }
        }

        /// <summary>
        /// Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
        /// </summary>
        /// <param name="domain">The domain to return cookies for.</param>
        /// <returns>A array of cookies</returns>
        public GrabzItCookie[] GetCookies(string domain)
        {
            lock (thisLock)
            {
                string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, domain));

                string url = string.Format("{0}getcookies.ashx?domain={1}&key={2}&sig={3}",
                                                          BaseURL, domain, ApplicationKey, sig);

                GetCookiesResult webResult = Get<GetCookiesResult>(url);

                CheckForException(webResult);

                return webResult.Cookies;
            }
        }

        /// <summary>
        /// Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        /// cookie is overridden.
        /// 
        /// This can be useful if a websites functionality is controlled by cookies.
        /// </summary>
        /// <param name="name">The name of the cookie to set.</param>
        /// <param name="domain">The domain of the website to set the cookie for.</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain)
        {
            return SetCookie(name, domain, string.Empty, string.Empty, false, null);
        }

        /// <summary>
        /// Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        /// cookie is overridden.
        /// 
        /// This can be useful if a websites functionality is controlled by cookies.
        /// </summary>
        /// <param name="name">The name of the cookie to set.</param>
        /// <param name="domain">The domain of the website to set the cookie for.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain, string value)
        {
            return SetCookie(name, domain, value, string.Empty, false, null);
        }

        /// <summary>
        /// Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        /// cookie is overridden.
        /// 
        /// This can be useful if a websites functionality is controlled by cookies.
        /// </summary>
        /// <param name="name">The name of the cookie to set.</param>
        /// <param name="domain">The domain of the website to set the cookie for.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="path">The website path the cookie relates to.</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain, string value, string path)
        {
            return SetCookie(name, domain, value, path, false, null);
        }

        /// <summary>
        /// Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        /// cookie is overridden.
        /// 
        /// This can be useful if a websites functionality is controlled by cookies.
        /// </summary>
        /// <param name="name">The name of the cookie to set.</param>
        /// <param name="domain">The domain of the website to set the cookie for.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="path">The website path the cookie relates to.</param>
        /// <param name="httponly">Is the cookie only used on HTTP</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain, string value, string path, bool httponly)
        {
            return SetCookie(name, domain, value, path, httponly, null);
        }

        /// <summary>
        /// Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        /// cookie is overridden.
        /// 
        /// This can be useful if a websites functionality is controlled by cookies.
        /// </summary>
        /// <param name="name">The name of the cookie to set.</param>
        /// <param name="domain">The domain of the website to set the cookie for.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="path">The website path the cookie relates to.</param>
        /// <param name="httponly">Is the cookie only used on HTTP</param>
        /// <param name="expires">When the cookie expires. Pass a null value if it does not expire.</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain, string value, string path, bool httponly, DateTime? expires)
        {
            lock (thisLock)
            {
                string expiresStr = string.Empty;
                if (expires.HasValue)
                {
                    expiresStr = expires.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
                }

                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", ApplicationSecret, name, domain,
                                               value, path, (httponly ? 1 : 0), expiresStr, 0));

                string url = string.Format("{0}setcookie.ashx?name={1}&domain={2}&value={3}&path={4}&httponly={5}&expires={6}&key={7}&sig={8}",
                                                           BaseURL, HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), HttpUtility.UrlEncode(value), HttpUtility.UrlEncode(path), (httponly ? 1 : 0), HttpUtility.UrlEncode(expiresStr), ApplicationKey, sig);

                GenericResult webResult = Get<GenericResult>(url);

                if (webResult == null)
                {
                    return false;
                }

                CheckForException(webResult);

                return Convert.ToBoolean(webResult.Result);
            }
        }

        /// <summary>
        /// Delete a custom cookie or block a global cookie from being used.
        /// </summary>
        /// <param name="name">The name of the cookie to delete</param>
        /// <param name="domain">The website the cookie belongs to</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool DeleteCookie(string name, string domain)
        {
            lock (thisLock)
            {
                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}", ApplicationSecret, name, domain, 1));

                string url = string.Format("{0}setcookie.ashx?name={1}&domain={2}&delete=1&key={3}&sig={4}",
                                                          BaseURL, HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), ApplicationKey, sig);

                GenericResult webResult = Get<GenericResult>(url);

                CheckForException(webResult);

                return Convert.ToBoolean(webResult.Result);
            }
        }

        /// <summary>
        /// Add a new custom watermark.
        /// </summary>
        /// <param name="identifier">The identifier you want to give the custom watermark. It is important that this identifier is unique.</param>
        /// <param name="path">The absolute path of the watermark on your server. For instance C:/watermark/1.png</param>
        /// <param name="xpos">The horizontal position you want the screenshot to appear at</param>
        /// <param name="ypos">The vertical position you want the screenshot to appear at</param>
        /// <returns>Returns true if the watermark was successfully set.</returns>
        public bool AddWaterMark(string identifier, string path, HorizontalPosition xpos, VerticalPosition ypos)
        {
            if (!File.Exists(path))
            {
                throw new GrabzItException("File: " + path + " does not exist", ErrorCode.FileNonExistantPath);
            }

            string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}", ApplicationSecret, identifier, (int)xpos, (int)ypos));

            string url = string.Format("{0}addwatermark.ashx", BaseURL);

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("key", ApplicationKey);
            nvc.Add("identifier", identifier);
            nvc.Add("xpos", ((int)xpos).ToString());
            nvc.Add("ypos", ((int)ypos).ToString());
            nvc.Add("sig", sig);

            string result = HttpUploadFile(url, path, "watermark", "image/jpeg", nvc);

            GenericResult webResult = DeserializeResult<GenericResult>(result);

            CheckForException(webResult);

            return Convert.ToBoolean(webResult.Result);
        }

        /// <summary>
        /// Delete a custom watermark.
        /// <summary>
        /// <param name="identifier">The identifier of the custom watermark you want to delete</param>
        /// <returns>Returns true if the watermark was successfully deleted.</returns>
        public bool DeleteWaterMark(string identifier)
        {
            string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, identifier));

            string url = string.Format("{0}deletewatermark.ashx?key={1}&identifier={2}&sig={3}",
                                                          BaseURL, HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);


            GenericResult webResult = Get<GenericResult>(url);

            CheckForException(webResult);

            return Convert.ToBoolean(webResult.Result);
        }

        /// <summary>
        /// Get all your custom watermarks.
        /// </summary>
        /// <returns>Returns an array of WaterMark</returns>
        public WaterMark[] GetWaterMarks()
        {
            return GetWaterMarks(string.Empty);
        }

        /// <summary>
        /// Get a particular custom watermark.
        /// </summary>
        /// <param name="identifier">The identifier of a particular custom watermark you want to view</param>
        /// <returns>Returns a WaterMark</returns>
        public WaterMark GetWaterMark(string identifier)
        {
            WaterMark[] watermarks = GetWaterMarks(identifier);

            if (watermarks != null && watermarks.Length == 1)
            {
                return watermarks[0];
            }

            return null;
        }

        private WaterMark[] GetWaterMarks(string identifier)
        {
            string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, identifier));

            string url = string.Format("{0}getwatermarks.ashx?key={1}&identifier={2}&sig={3}",
                                                          BaseURL, HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);

            GetWatermarksResult webResult = Get<GetWatermarksResult>(url);

            CheckForException(webResult);

            return webResult.WaterMarks;
        }

        /// <summary>
        /// This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.
        /// </summary>
        /// <param name="id">The unique identifier of the screenshot, returned by the callback handler or the TakePicture method</param>
        /// <returns>The screenshot</returns>
        public Image GetPicture(string id)
        {
            lock (thisLock)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(
                                                                                "{0}getfile.ashx?id={1}",
                                                                                BaseURL, id));
                request.KeepAlive = false;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentLength == 0)
                    {
                        return null;
                    }
                    using (Stream stream = response.GetResponseStream())
                    {
                        return Image.FromStream(stream, true, false);
                    }
                }
            }
        }

        /// <summary>
        /// This method returns the screenshot.
        /// </summary>
        /// <param name="id">The unique identifier of the screenshot, returned by the callback handler or the Save method</param>
        /// <returns>GrabzItFile - which represents the screenshot</returns>
        public GrabzItFile GetResult(string id)
        {
            lock (thisLock)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(
                                                                                "{0}getfile.ashx?id={1}",
                                                                                BaseURL, id));
                request.KeepAlive = false;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentLength == 0)
                    {
                        return null;
                    }

                    using (Stream stream = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[131072];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            return new GrabzItFile(ms.ToArray());
                        }
                    }
                }
            }
        }

        private void CheckForException(IException result)
        {
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    throw new GrabzItException(result.Message, result.Code);
                }
            }
        }

        private static string Encrypt(string plainText)
        {
            byte[] bs = Encoding.ASCII.GetBytes(plainText);
            using (MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider())
            {
                return toHex(hasher.ComputeHash(bs));
            }
        }

        private static string ConvertCountryToString(Country country)
        {
            if (country == Country.Default)
            {
                return string.Empty;
            }
            return country.ToString();
        }

        private static string toHex(byte[] bytes)
        {
            //Fastest method to convert bytes to hex according to http://stackoverflow.com/questions/623104/c-sharp-byte-to-hex-string/3974535#3974535
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }

        private string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            using (Stream rs = wr.GetRequestStream())
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
            }

            try
            {
                using (WebResponse wresp = wr.GetResponse())
                {
                    using (Stream stream2 = wresp.GetResponseStream())
                    {
                        using (StreamReader reader2 = new StreamReader(stream2))
                        {
                            return reader2.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                HandleWebException(e);
                return string.Empty;
            }
        }

        /// <summary>
        /// Create a new GrabzIt client. Note if you are not using the ScreenShotComplete event or you are using multiple threads to call this class, consider using the public constructor to improve performance.
        /// </summary>
        /// <param name="applicationKey">The application key of your GrabzIt account</param>
        /// <param name="applicationSecret">The application secret of your GrabzIt account</param>
        /// <returns>GrabzItClient</returns>
        public static GrabzItClient Create(string applicationKey, string applicationSecret)
        {
            if (grabzItClient == null)
            {
                Interlocked.CompareExchange(ref grabzItClient, new GrabzItClient(applicationKey, applicationSecret, true), null);
            }
            return grabzItClient;
        }

        internal static GrabzItClient WebClient
        {
            get { return grabzItClient; }
        }
    }
}