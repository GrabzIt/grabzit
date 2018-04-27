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
using GrabzIt.COM;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using GrabzIt.Parameters;

namespace GrabzIt
{
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItClient : IGrabzItClient
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
        private string protocol = "http";
        private WebProxy proxy = null;
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

        private const string BaseURLGet = "://api.grabz.it/services/";
        private const string BaseURLPost = "://grabz.it/services/";
        private const string TakeDOCX = "takedocx.ashx";
        private const string TakePDF = "takepdf.ashx";
        private const string TakeTable = "taketable.ashx";
        private const string TakeImage = "takepicture.ashx";

        //Required by COM
        public GrabzItClient()
            : this(string.Empty, string.Empty, false)
        {
        }

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
        /// This method sets the proxy for all requests to GrabzIt's web services to use.
        /// </summary>
        /// <param name="proxyUrl">The URL, which can include a port if required, of the proxy. Providing a null will remove any previously set proxy.</param>
        public void SetProxy(string proxyUrl)
        {
            if (string.IsNullOrEmpty(proxyUrl))
            {
                this.proxy = null;
                return;
            }
            this.proxy = new WebProxy(proxyUrl);
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
        public string CreateEncrpytionKey()
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

                request.Store(GetRootURL(false) + "takeanimation.ashx", false, options, url);
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

                request.Store(GetRootURL(false) + TakeImage, false, options, url);
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

                request.Store(GetRootURL(true) + TakeImage, true, options, html);
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

                request.Store(GetRootURL(false) + TakeTable, false, options, url);
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

                request.Store(GetRootURL(true) + TakeTable, true, options, html);
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

                request.Store(GetRootURL(false) + TakePDF, false, options, url);
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

                request.Store(GetRootURL(true) + TakePDF, true, options, html);
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

                request.Store(GetRootURL(false) + TakeDOCX, false, options, url);
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

                request.Store(GetRootURL(true) + TakeDOCX, true, options, html);
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
                if (this.request == null || this.request.Options == null)
                {
                    throw new GrabzItException("No parameters have been set.", ErrorCode.ParameterMissingParameters);
                }
                string sig = Encrypt(request.Options.GetSignatureString(this.ApplicationSecret, callBackURL, request.TargetUrl));

                TakePictureResult webResult = null;

                if (request.IsPost)
                {
                    webResult = Post<TakePictureResult>(request.WebServiceURL, request.Options.GetQueryString(this.ApplicationKey, sig, callBackURL, "html", HttpUtility.UrlEncode(this.request.Data)));
                }
                else
                {
                    webResult = Get<TakePictureResult>(request.WebServiceURL + "?" + request.Options.GetQueryString(this.ApplicationKey, sig, callBackURL, "url", this.request.Data));
                }

                CheckForException(webResult);

                if (webResult == null)
                {
                    throw new GrabzItException("An unknown network error occurred, please try calling this method again.", ErrorCode.NetworkGeneralError);
                }

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
                Thread.Sleep(3000 + request.Options.GetStartDelay());

                //Wait for it to be ready.
                while (true)
                {
                    Status status = GetStatus(id);

                    if (!status.Cached && !status.Processing)
                    {
                        throw new GrabzItException("The capture did not complete with the error: " + status.Message, ErrorCode.RenderingError);
                    }

                    if (status.Cached)
                    {
                        GrabzItFile result = GetResult(id);

                        if (result == null)
                        {
                            throw new GrabzItException("The capture could not be found on GrabzIt.", ErrorCode.RenderingMissingScreenshot);
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

        private T Get<T>(string url)
        {
            using (QuickWebClient client = new QuickWebClient(this.proxy))
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

        private T Post<T>(string url, string parameters)
        {
            using (QuickWebClient client = new QuickWebClient(this.proxy))
            {
                try
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string result = client.UploadString(url, parameters);
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
                                                          GetRootURL(false), id);
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
                                                          GetRootURL(false), domain, ApplicationKey, sig);

                GetCookiesResult webResult = Get<GetCookiesResult>(url);

                if (webResult == null)
                {
                    return new GrabzItCookie[0];
                }

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
                                                           GetRootURL(false), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), HttpUtility.UrlEncode(value), HttpUtility.UrlEncode(path), (httponly ? 1 : 0), HttpUtility.UrlEncode(expiresStr), ApplicationKey, sig);

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
                                                          GetRootURL(false), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), ApplicationKey, sig);

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

            string url = "http://grabz.it/services/addwatermark.ashx";

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
                                                          GetRootURL(false), HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);


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
                                                          GetRootURL(false), HttpUtility.UrlEncode(ApplicationKey), HttpUtility.UrlEncode(identifier), sig);

            GetWatermarksResult webResult = Get<GetWatermarksResult>(url);

            if (webResult == null)
            {
                return new WaterMark[0];
            }

            CheckForException(webResult);

            return webResult.WaterMarks;
        }

        /// <summary>
        /// This method returns the screenshot.
        /// </summary>
        /// <param name="id">The unique identifier of the screenshot, returned by the callback handler or the Save method</param>
        /// <returns>GrabzItFile - which represents the screenshot</returns>
        public GrabzItFile GetResult(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            lock (thisLock)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(
                                                                                "{0}getfile.ashx?id={1}",
                                                                                GetRootURL(false), id));
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

        private string GetRootURL(bool isPost)
        {
            if (isPost)
            {
                return this.protocol + BaseURLPost;
            }
            return this.protocol + BaseURLGet;
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
            wr.Proxy = this.proxy;
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
        [ComVisible(false)]
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