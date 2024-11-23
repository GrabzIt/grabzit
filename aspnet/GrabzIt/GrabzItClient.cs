using System;
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
using GrabzIt.COM;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using GrabzIt.Parameters;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrabzIt
{
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItClient : IGrabzItClient
    {        
        private GrabzItRequest request;
        private string protocol = "http";
        internal WebProxy Proxy { get; private set; } = null;
        internal HttpClient HttpClient { get; private set; }
        private Object thisLock = new Object();
        private SemaphoreSlim thisSlim = new SemaphoreSlim(1, 1);
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

        private const string BaseURL = "://api.grabz.it/services/";
        private const string TakeDOCX = "takedocx";
        private const string TakePDF = "takepdf";
        private const string TakeTable = "taketable";
        private const string TakeImage = "takepicture";
        private const string TakeHTML = "takehtml";
        private const string TakeVideo = "takevideo";

        //Required by COM
        public GrabzItClient()
            : this(string.Empty, string.Empty, false, null)
        {
        }

        /// <summary>
        /// Create a new GrabzIt client.
        /// </summary>
        /// <param name="applicationKey">The application key of your GrabzIt account</param>
        /// <param name="applicationSecret">The application secret of your GrabzIt account</param>
        public GrabzItClient(string applicationKey, string applicationSecret, HttpClient client = null) : this(applicationKey, applicationSecret, false, client)
        {
        }

        private GrabzItClient(string applicationKey, string applicationSecret, bool isStatic, HttpClient client)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationSecret = applicationSecret;
            if (client == null)
            {
                client = new HttpClient();
            }
            this.HttpClient = client;
            request = new GrabzItRequest();
        }

        /// <summary>
        /// This method enables a local proxy server to be used for all requests.
        /// </summary>
        /// <param name="proxyUrl">The URL, which can include a port if required, of the proxy. Providing a null will remove any previously set proxy.</param>
        public void SetLocalProxy(string proxyUrl)
        {
            if (string.IsNullOrEmpty(proxyUrl))
            {
                this.Proxy = null;
                return;
            }
            this.Proxy = new WebProxy(proxyUrl);
        }

        /// <summary>
        /// This method sets if requests to GrabzIt's API should use SSL or not
        /// </summary>
        /// <param name="value">true if should use SSL</param>
        public void UseSSL(bool value)
        {
            if (value)
            {
                this.protocol = "https";
                return;
            }
            this.protocol = "http";
        }

        /// <summary>
        /// This method creates an encryption key to pass to the <see cref="EncryptionKey"/> parameter.
        /// </summary>
        /// <returns>The encryption key</returns>
        public string CreateEncryptionKey()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);

            return Convert.ToBase64String(tokenData);
        }

        /// <summary>
        /// This method will decrypt a encrypted capture, using the key you passed to the <see cref="EncryptionKey"/> parameter.
        /// </summary>
        /// <param name="path">The path of the encrypted capture</param>
        /// <param name="key">The encryption key</param>
        public void Decrypt(string path, string key)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
            }
            if (!File.Exists(path))
            {
                throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
            }
            File.WriteAllBytes(path, Decrypt(File.ReadAllBytes(path), key));
        }

        /// <summary>
        /// This method will decrypt a encrypted capture, using the key you passed to the <see cref="EncryptionKey"/> parameter.
        /// </summary>
        /// <param name="file">The encrypted GrabzItFile</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The decrypted GrabzItFile</returns>
        public GrabzItFile Decrypt(GrabzItFile file, string key)
        {
            if (file == null)
            {
                return null;
            }
            return new GrabzItFile(Decrypt(file.Bytes, key));
        }

        /// <summary>
        /// This method will decrypt a encrypted capture, using the key you passed to the <see cref="EncryptionKey"/> parameter.
        /// </summary>
        /// <param name="data">The encrypted bytes</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The decrypted bytes</returns>
        public byte[] Decrypt(byte[] data, string key)
        {
            if (data == null || string.IsNullOrEmpty(key))
            {
                return new byte[0];
            }

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Convert.FromBase64String(key);
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.BlockSize = 128;
                rijAlg.Padding = PaddingMode.Zeros;

                byte[] iv = new byte[16];
                Array.Copy(data, 0, iv, 0, iv.Length);

                byte[] fileData = new byte[data.Length - 16];
                Buffer.BlockCopy(data, 16, fileData, 0, fileData.Length);

                using (ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, iv))
                {
                    using (MemoryStream msDecrypt = new MemoryStream())
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                        {
                            csDecrypt.Write(fileData, 0, fileData.Length);
                            csDecrypt.FlushFinalBlock();
                            return msDecrypt.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method specifies the URL of the online video that should be converted into a animated GIF.
        /// </summary>
        /// <param name="url">The URL to convert into a animated GIF.</param>
        public void URLToAnimation(string url)
        {
            URLToAnimation(url, null);
        }

        /// <summary>
        /// This method specifies the URL of the online video that should be converted into a animated GIF.
        /// </summary>
        /// <param name="url">The URL to convert into a animated GIF.</param>
        /// <param name="options">A instance of the AnimationOptions class that defines any special options to use when creating the animated GIF.</param>
        public void URLToAnimation(string url, AnimationOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new AnimationOptions();
                }

                request.Store(GetRootURL() + "takeanimation", false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the URL of the web page that should be converted into a SEO report.
        /// </summary>
        /// <param name="url">The URL to convert into a report.</param>
        /// <param name="options">A instance of the SEOOptions class that defines any special options to use when creating the report.</param>
        public void URLToSEOReport(string url, SEOOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new SEOOptions();
                }

                request.Store(GetRootURL() + "takeseo", false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a image screenshot.
        /// </summary>
        /// <param name="url">The URL to capture as a screenshot.</param>
        public void URLToImage(string url)
        {
            URLToImage(url, null);
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a image screenshot.
        /// </summary>
        /// <param name="url">The URL to capture as a screenshot.</param>
        /// <param name="options">A instance of the ImageOptions class that defines any special options to use when creating the screenshot.</param>
        public void URLToImage(string url, ImageOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new ImageOptions();
                }

                request.Store(GetRootURL() + TakeImage, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a image.
        /// </summary>
        /// <param name="html">The HTML to convert into a image.</param>
        public void HTMLToImage(string html)
        {
            HTMLToImage(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a image.
        /// </summary>
        /// <param name="html">The HTML to convert into a image.</param>
        /// <param name="options">A instance of the ImageOptions class that defines any special options to use when creating the screenshot.</param>
        public void HTMLToImage(string html, ImageOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new ImageOptions();
                }

                request.Store(GetRootURL() + TakeImage, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a video.
        /// </summary>
        /// <param name="url">The URL to capture as a video.</param>
        public void URLToVideo(string url)
        {
            URLToVideo(url, null);
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a video.
        /// </summary>
        /// <param name="url">The URL to capture as a video.</param>
        /// <param name="options">A instance of the VideoOptions class that defines any special options to use when creating the video.</param>
        public void URLToVideo(string url, VideoOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new VideoOptions();
                }

                request.Store(GetRootURL() + TakeVideo, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a video.
        /// </summary>
        /// <param name="html">The HTML to convert into a video.</param>
        public void HTMLToVideo(string html)
        {
            HTMLToVideo(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a video.
        /// </summary>
        /// <param name="html">The HTML to convert into a video.</param>
        /// <param name="options">A instance of the VideoOptions class that defines any special options to use when creating the video.</param>
        public void HTMLToVideo(string html, VideoOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new VideoOptions();
                }

                request.Store(GetRootURL() + TakeVideo, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a image.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a image.</param>
        public void FileToImage(string path)
        {
            FileToImage(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a image.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a image.</param>
        /// <param name="options">A instance of the ImageOptions class that defines any special options to use when creating the screenshot.</param>
        public void FileToImage(string path, ImageOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToImage(File.ReadAllText(path), options);
            }
        }

        /// <summary>
        /// This method specifies the URL that should be converted into rendered HTML.
        /// </summary>
        /// <param name="url">The URL to capture as rendered HTML.</param>
        public void URLToRenderedHTML(string url)
        {
            URLToRenderedHTML(url, null);
        }

        /// <summary>
        /// This method specifies the URL that should be converted into rendered HTML.
        /// </summary>
        /// <param name="url">The URL to capture as rendered HTML.</param>
        /// <param name="options">A instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML.</param>
        public void URLToRenderedHTML(string url, HTMLOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new HTMLOptions();
                }

                request.Store(GetRootURL() + TakeHTML, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into rendered HTML.
        /// </summary>
        /// <param name="html">The HTML to convert into rendered HTML.</param>
        public void HTMLToRenderedHTML(string html)
        {
            HTMLToRenderedHTML(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into rendered HTML.
        /// </summary>
        /// <param name="html">The HTML to convert into rendered HTML.</param>
        /// <param name="options">A instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML.</param>
        public void HTMLToRenderedHTML(string html, HTMLOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new HTMLOptions();
                }

                request.Store(GetRootURL() + TakeHTML, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into rendered HTML.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into rendered HTML.</param>
        public void FileToRenderedHTML(string path)
        {
            FileToRenderedHTML(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into rendered HTML.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into rendered HTML.</param>
        /// <param name="options">A instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML.</param>
        public void FileToRenderedHTML(string path, HTMLOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToRenderedHTML(File.ReadAllText(path), options);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a video.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a video.</param>
        public void FileToVideo(string path)
        {
            FileToVideo(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a video.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a video.</param>
        /// <param name="options">A instance of the VideoOptions class that defines any special options to use when creating the video.</param>
        public void FileToVideo(string path, VideoOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToVideo(File.ReadAllText(path), options);
            }
        }

        /// <summary>
        /// This method specifies the URL that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="url">The URL to extract HTML tables from.</param>
        public void URLToTable(string url)
        {
            URLToTable(url, null);
        }

        /// <summary>
        /// This method specifies the URL that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="url">The URL to extract HTML tables from.</param>
        /// <param name="options">A instance of the TableOptions class that defines any special options to use when converting the HTML table.</param>
        public void URLToTable(string url, TableOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new TableOptions();
                }

                request.Store(GetRootURL() + TakeTable, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="html">The HTML to extract HTML tables from.</param>
        /// <param name="options">A instance of the TableOptions class that defines any special options to use when converting the HTML table.</param>
        public void HTMLToTable(string html)
        {
            HTMLToTable(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="html">The HTML to extract HTML tables from.</param>
        /// <param name="options">A instance of the TableOptions class that defines any special options to use when converting the HTML table.</param>
        public void HTMLToTable(string html, TableOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new TableOptions();
                }

                request.Store(GetRootURL() + TakeTable, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="path">The file path of a HTML file to extract HTML tables from.</param>
        public void FileToTable(string path)
        {
            FileToTable(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that the HTML tables should be extracted from.
        /// </summary>
        /// <param name="path">The file path of a HTML file to extract HTML tables from.</param>
        /// <param name="options">A instance of the TableOptions class that defines any special options to use when converting the HTML table.</param>
        public void FileToTable(string path, TableOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToTable(File.ReadAllText(path), options);
            }
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a PDF</param>
        public void URLToPDF(string url)
        {
            URLToPDF(url, null);
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a PDF.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a PDF</param>
        /// <param name="options">A instance of the PDFOptions class that defines any special options to use when creating the PDF.</param>
        public void URLToPDF(string url, PDFOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new PDFOptions();
                }

                request.Store(GetRootURL() + TakePDF, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a PDF.
        /// </summary>
        /// <param name="html">The HTML to convert into a PDF.</param>
        public void HTMLToPDF(string html)
        {
            HTMLToPDF(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a PDF.
        /// </summary>
        /// <param name="html">The HTML to convert into a PDF.</param>
        /// <param name="options">A instance of the PDFOptions class that defines any special options to use when creating the PDF.</param>
        public void HTMLToPDF(string html, PDFOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new PDFOptions();
                }

                request.Store(GetRootURL() + TakePDF, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a PDF.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a PDF.</param>
        public void FileToPDF(string path)
        {
            FileToPDF(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a PDF.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a PDF.</param>
        /// <param name="options">A instance of the PDFOptions class that defines any special options to use when creating the PDF.</param>
        public void FileToPDF(string path, PDFOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToPDF(File.ReadAllText(path), options);
            }
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a DOCX.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a DOCX</param>
        public void URLToDOCX(string url)
        {
            URLToDOCX(url, null);
        }

        /// <summary>
        /// This method specifies the URL that should be converted into a DOCX.
        /// </summary>
        /// <param name="url">The URL that the should be converted into a DOCX</param>
        /// <param name="options">A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.</param>
        public void URLToDOCX(string url, DOCXOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new DOCXOptions();
                }

                request.Store(GetRootURL() + TakeDOCX, false, options, url);
            }
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a DOCX.
        /// </summary>
        /// <param name="html">The HTML to convert into a DOCX.</param>
        public void HTMLToDOCX(string html)
        {
            HTMLToDOCX(html, null);
        }

        /// <summary>
        /// This method specifies the HTML that should be converted into a DOCX.
        /// </summary>
        /// <param name="html">The HTML to convert into a DOCX.</param>
        /// <param name="options">A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.</param>
        public void HTMLToDOCX(string html, DOCXOptions options)
        {
            lock (thisLock)
            {
                if (options == null)
                {
                    options = new DOCXOptions();
                }

                request.Store(GetRootURL() + TakeDOCX, true, options, html);
            }
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a DOCX.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a DOCX.</param>
        public void FileToDOCX(string path)
        {
            FileToDOCX(path, null);
        }

        /// <summary>
        /// This method specifies a HTML file that should be converted into a DOCX.
        /// </summary>
        /// <param name="path">The file path of a HTML file to convert into a DOCX.</param>
        /// <param name="options">A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.</param>
        public void FileToDOCX(string path, DOCXOptions options)
        {
            lock (thisLock)
            {
                if (!File.Exists(path))
                {
                    throw new GrabzItException(string.Concat("File: ", path, " does not exist"), ErrorCode.FileNonExistantPath);
                }

                HTMLToDOCX(File.ReadAllText(path), options);
            }
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
        public string Save(string callBackURL = "")
        {
            return SaveAsync(callBackURL).Result;
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
        public async Task<string> SaveAsync(string callBackURL = "")
        {
            await thisSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                if (this.request == null || this.request.Options == null)
                {
                    throw new GrabzItException("No parameters have been set.", ErrorCode.ParameterMissingParameters);
                }
                string sig = Encrypt(request.Options.GetSignatureString(this.ApplicationSecret, callBackURL, request.TargetUrl));

                TakePictureResult webResult = await TakeAsync(callBackURL, sig);                

                if (webResult == null)
                {
                    webResult = await TakeAsync(callBackURL, sig);
                }

                if (webResult == null)
                {
                    throw new GrabzItException("An unknown network error occurred, please try calling this method again.", ErrorCode.NetworkGeneralError);
                }

                return webResult.ID;
            }
            finally
            {
                thisSlim.Release();
            }
        }

        private async Task<TakePictureResult> TakeAsync(string callBackURL, string sig)
        {
            TakePictureResult webResult;
            if (request.IsPost)
            {
                webResult = await PostAsync<TakePictureResult>(request.WebServiceURL, request.Options.GetQueryString(this.ApplicationKey, sig, callBackURL, "html", HttpUtility.UrlEncode(this.request.Data)));
            }
            else
            {
                webResult = await GetAsync<TakePictureResult>(request.WebServiceURL + "?" + request.Options.GetQueryString(this.ApplicationKey, sig, callBackURL, "url", this.request.Data));
            }
            CheckForException(webResult);
            return webResult;
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
            return SaveToAsync().Result;
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
            return SaveToAsync(saveToFile).Result;
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot and saves it to the target path provided
        /// </summary>
        /// <param name="saveToFile">The file path that the screenshot should saved to.</param>
        /// <returns>Returns the true if it is successful otherwise it throws an exception.</returns>
        public async Task<bool> SaveToAsync(string saveToFile)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrabzItFile result = await SaveToAsync().ConfigureAwait(false);

                    if (result == null)
                    {
                        return false;
                    }

                    result.Save(saveToFile);
                    break;
                }
                catch (GrabzItException e)
                {
                    throw e;
                }
                catch (Exception ex)
                {
                    if (attempt < 3)
                    {
                        attempt++;
                        continue;
                    }
                    throw new GrabzItException("An error occurred trying to save the capture to: " +
                                        saveToFile, ErrorCode.FileSaveError, ex);
                }
            }
            return true;
        }

        /// <summary>
        /// Calls the GrabzIt web service to take the screenshot and returns a GrabzItFile object
        /// </summary>
        /// <remarks>
        /// Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
        /// </remarks>
        /// <returns>Returns a GrabzItFile object containing the screenshot data.</returns>
        public async Task<GrabzItFile> SaveToAsync()
        {
            string id = await SaveAsync(string.Empty);

            await thisSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }

                //Wait until it is possible to be ready
                await Task.Delay(3000 + request.Options.GetStartDelay()).ConfigureAwait(false);

                //Wait for it to be ready.
                while (true)
                {
                    Status status = await GetStatusAsync(id).ConfigureAwait(false);

                    if (!status.Cached && !status.Processing)
                    {
                        throw new GrabzItException("The capture did not complete with the error: " + status.Message, ErrorCode.RenderingError);
                    }

                    if (status.Cached)
                    {
                        GrabzItFile result = await GetResultAsync(id).ConfigureAwait(false);

                        if (result == null)
                        {
                            throw new GrabzItException("The capture could not be found on GrabzIt.", ErrorCode.RenderingMissingScreenshot);
                        }

                        return result;
                    }

                    await Task.Delay(3000).ConfigureAwait(false);
                }
            }
            finally
            {
                thisSlim.Release();
            }
        }

        private async Task<T> GetAsync<T>(string url)
        {
            try
            {
                string result = await HttpClient.GetStringAsync(url).ConfigureAwait(false);
                return DeserializeResult<T>(result);
            }
            catch (WebException e)
            {
                HandleWebException(e);
                return default(T);
            }
        }

        private async Task<T> PostAsync<T>(string url, string parameters)
        {
            try
            {
                using (StringContent content = new StringContent(parameters, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"))
                {
                    using (HttpResponseMessage response = await this.HttpClient.PostAsync(url, content))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return DeserializeResult<T>(result);
                    }
                }
            }
            catch (WebException e)
            {
                HandleWebException(e);
                return default(T);
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
                    throw new GrabzItException("A network error occurred when connecting to GrabzIt.", ErrorCode.NetworkGeneralError);
                }
            }
            else if (e.Status == WebExceptionStatus.NameResolutionFailure)
            {
                throw new GrabzItException("A network error occurred when connecting to GrabzIt.", ErrorCode.NetworkGeneralError);
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
        /// Get the current status of a GrabzIt screenshot
        /// </summary>
        /// <param name="id">The id of the screenshot</param>
        /// <returns>A Status object representing the screenshot</returns>
        public Status GetStatus(string id)
        {
            return GetStatusAsync(id).Result;
        }

        /// <summary>
        /// Get the current status of a GrabzIt screenshot
        /// </summary>
        /// <param name="id">The id of the screenshot</param>
        /// <returns>A Status object representing the screenshot</returns>
        public async Task<Status> GetStatusAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            string url = string.Format("{0}getstatus?id={1}",
                                                        GetRootURL(), id);
            GetStatusResult webResult = await GetAsync<GetStatusResult>(url).ConfigureAwait(false);

            if (webResult == null)
            {
                return null;
            }

            return webResult.GetStatus();
        }

        /// <summary>
        /// Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
        /// </summary>
        /// <param name="domain">The domain to return cookies for.</param>
        /// <returns>A array of cookies</returns>
        public GrabzItCookie[] GetCookies(string domain)
        {
            return GetCookiesAsync(domain).Result;
        }

        /// <summary>
        /// Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
        /// </summary>
        /// <param name="domain">The domain to return cookies for.</param>
        /// <returns>A array of cookies</returns>
        public async Task<GrabzItCookie[]> GetCookiesAsync(string domain)
        {
            await thisSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, domain));

                string url = string.Format("{0}getcookies?domain={1}&key={2}&sig={3}",
                                                          GetRootURL(), domain, ApplicationKey, sig);

                GetCookiesResult webResult = await GetAsync<GetCookiesResult>(url);

                if (webResult == null)
                {
                    return new GrabzItCookie[0];
                }

                CheckForException(webResult);

                return webResult.Cookies;
            }
            finally
            {
                thisSlim.Release();
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
        /// <param name="value">The value of the cookie.</param>
        /// <param name="path">The website path the cookie relates to.</param>
        /// <param name="httponly">Is the cookie only used on HTTP</param>
        /// <param name="expires">When the cookie expires. Pass a null value if it does not expire.</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool SetCookie(string name, string domain, string value, string path, bool httponly, DateTime expires)
        {
            return SetCookieAsync(name, domain, value, path, httponly, expires).Result;
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
        public async Task<bool> SetCookieAsync(string name, string domain, string value = "", string path = "", bool httponly = false, DateTime? expires = null)
        {
            await thisSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                string expiresStr = string.Empty;
                if (expires.HasValue)
                {
                    expiresStr = expires.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
                }

                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", ApplicationSecret, name, domain,
                                               value, path, (httponly ? 1 : 0), expiresStr, 0));

                string url = string.Format("{0}setcookie?name={1}&domain={2}&value={3}&path={4}&httponly={5}&expires={6}&key={7}&sig={8}",
                                                           GetRootURL(), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), HttpUtility.UrlEncode(value), HttpUtility.UrlEncode(path), (httponly ? 1 : 0), HttpUtility.UrlEncode(expiresStr), ApplicationKey, sig);

                GenericResult webResult = await GetAsync<GenericResult>(url);

                if (webResult == null)
                {
                    return false;
                }

                CheckForException(webResult);

                return Convert.ToBoolean(webResult.Result);
            }
            finally
            {
                thisSlim.Release();
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
            return DeleteCookieAsync(name, domain).Result;
        }

        /// <summary>
        /// Delete a custom cookie or block a global cookie from being used.
        /// </summary>
        /// <param name="name">The name of the cookie to delete</param>
        /// <param name="domain">The website the cookie belongs to</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public async Task<bool> DeleteCookieAsync(string name, string domain)
        {
            await thisSlim.WaitAsync().ConfigureAwait(false);
            try
            {
                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}", ApplicationSecret, name, domain, 1));

                string url = string.Format("{0}setcookie?name={1}&domain={2}&delete=1&key={3}&sig={4}",
                                                          GetRootURL(), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), ApplicationKey, sig);

                GenericResult webResult = await GetAsync<GenericResult>(url);

                CheckForException(webResult);

                return Convert.ToBoolean(webResult.Result);
            }
            finally
            {
                thisSlim.Release();
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

            string url = string.Format("{0}addwatermark", GetRootURL());

            NameValueCollection nvc = new NameValueCollection
            {
                { "key", ApplicationKey },
                { "identifier", identifier },
                { "xpos", ((int)xpos).ToString() },
                { "ypos", ((int)ypos).ToString() },
                { "sig", sig }
            };

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
            return DeleteWaterMarkAsync(identifier).Result;
        }

        /// <summary>
        /// Delete a custom watermark.
        /// <summary>
        /// <param name="identifier">The identifier of the custom watermark you want to delete</param>
        /// <returns>Returns true if the watermark was successfully deleted.</returns>
        public async Task<bool> DeleteWaterMarkAsync(string identifier)
        {
            string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, identifier));

            string url = string.Format("{0}deletewatermark?key={1}&identifier={2}&sig={3}",
                                                          GetRootURL(), HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);


            GenericResult webResult = await GetAsync<GenericResult>(url);

            CheckForException(webResult);

            return Convert.ToBoolean(webResult.Result);
        }

        /// <summary>
        /// Get all your custom watermarks.
        /// </summary>
        /// <returns>Returns an array of WaterMark</returns>
        public WaterMark[] GetWaterMarks()
        {
            return GetWaterMarksAsync(string.Empty).Result;
        }

        /// <summary>
        /// Get a particular custom watermark.
        /// </summary>
        /// <param name="identifier">The identifier of a particular custom watermark you want to view</param>
        /// <returns>Returns a WaterMark</returns>
        public WaterMark GetWaterMark(string identifier)
        {
            return GetWaterMarkAsync(identifier).Result;
        }

        /// <summary>
        /// Get a particular custom watermark.
        /// </summary>
        /// <param name="identifier">The identifier of a particular custom watermark you want to view</param>
        /// <returns>Returns a WaterMark</returns>
        public async Task<WaterMark> GetWaterMarkAsync(string identifier)
        {
            WaterMark[] watermarks = await GetWaterMarksAsync(identifier);

            if (watermarks != null && watermarks.Length == 1)
            {
                return watermarks[0];
            }

            return null;
        }

        private async Task<WaterMark[]> GetWaterMarksAsync(string identifier)
        {
            string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, identifier));

            string url = string.Format("{0}getwatermarks?key={1}&identifier={2}&sig={3}",
                                                          GetRootURL(), HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);

            GetWatermarksResult webResult = await GetAsync<GetWatermarksResult>(url);

            if (webResult == null)
            {
                return new WaterMark[0];
            }

            CheckForException(webResult);

            return webResult.WaterMarks;
        }

        /// <summary>
        /// This method returns the capture.
        /// </summary>
        /// <param name="id">The unique identifier of the capture, returned by the callback handler or the Save method</param>
        /// <returns>GrabzItFile - which represents the capture</returns>
        public GrabzItFile GetResult(string id)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (!WriteResultToStream(id, ms))
                {
                    return null;
                }
                return new GrabzItFile(ms.ToArray());
            }
        }

        /// <summary>
        /// This method writes the capture to the output stream.
        /// </summary>
        /// <param name="id">The unique identifier of the capture, returned by the callback handler or the Save method</param>
        /// <param name="outStream">The output stream to write the capture to</param>
        /// <returns>true if the capture is successfullly written</returns>
        public bool WriteResultToStream(string id, Stream outStream)
        {
            if (string.IsNullOrEmpty(id) || outStream == null)
            {
                return false;
            }

            lock (thisLock)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(
                                                                                "{0}getfile?id={1}",
                                                                                GetRootURL(), id));
                request.KeepAlive = false;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentLength == 0)
                    {
                        return false;
                    }

                    using (Stream stream = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[131072];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// This method returns the capture.
        /// </summary>
        /// <param name="id">The unique identifier of the capture, returned by the callback handler or the Save method</param>
        /// <returns>GrabzItFile - which represents the capture</returns>
        public async Task<GrabzItFile> GetResultAsync(string id)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (!await WriteResultToStreamAsync(id, ms).ConfigureAwait(false))
                {
                    return null;
                }
                return new GrabzItFile(ms.ToArray());
            }
        }

        /// <summary>
        /// This method writes the capture to the output stream.
        /// </summary>
        /// <param name="id">The unique identifier of the capture, returned by the callback handler or the Save method</param>
        /// <param name="outStream">The output stream to write the capture to</param>
        /// <returns>true if the capture is successfullly written</returns>
        public async Task<bool> WriteResultToStreamAsync(string id, Stream outStream)
        {
            if (string.IsNullOrEmpty(id) || outStream == null)
            {
                return false;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(
                                                                            "{0}getfile?id={1}",
                                                                            GetRootURL(), id));
            request.KeepAlive = false;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
            {
                if (response.ContentLength == 0)
                {
                    return false;
                }

                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[131072];
                    int read;
                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        await outStream.WriteAsync(buffer, 0, read).ConfigureAwait(false);
                    }
                }
            }

            return true;
        }

        private string GetRootURL()
        {
            return this.protocol + BaseURL;
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
            byte[] bs = Encoding.ASCII.GetBytes(EncodeNonAsciiCharacters(plainText));
            using (MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider())
            {
                return toHex(hasher.ComputeHash(bs));
            }
        }

        private static string EncodeNonAsciiCharacters(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            StringBuilder sb = new StringBuilder();
            foreach (int codePoint in AsCodePoints(value))
            {
                if (codePoint > 127)
                {
                    sb.Append("?");
                }
                else
                {
                    sb.Append(Char.ConvertFromUtf32(codePoint));
                }
            }

            return sb.ToString();
        }

        private static IEnumerable<int> AsCodePoints(string s)
        {
            for (int i = 0; i < s.Length; ++i)
            {
                yield return char.ConvertToUtf32(s, i);
                if (char.IsHighSurrogate(s, i))
                {
                    i++;
                }
            }
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
            wr.Proxy = this.Proxy;
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
    }
}