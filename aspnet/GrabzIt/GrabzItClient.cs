using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Serialization;
using GrabzIt.Cookies;
using GrabzIt.Results;

namespace GrabzIt
{
    public class GrabzItClient
    {
        private static GrabzItClient grabzItClient;
        private static MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

        public delegate void ScreenShotHandler(object sender, ScreenShotEventArgs result);
        public event ScreenShotHandler ScreenShotComplete;

        private Object thisLock = new Object();

        private readonly string applicationKey;
        private readonly string applicationSecret;
        private const string BaseURL = "http://grabz.it/services/";

        public GrabzItClient(string applicationKey, string applicationSecret)
        {
            this.applicationKey = applicationKey;
            this.applicationSecret = applicationSecret;
        }

        internal void OnScreenShotComplete(object sender, ScreenShotEventArgs result)
        {
            if (ScreenShotComplete != null)
            {
                ScreenShotComplete(this, result);
            }
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url)
        {
            return TakePicture(url, string.Empty, 0, 0, 0, 0, string.Empty, ScreenShotFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url, string callback)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, string.Empty, ScreenShotFormat.jpg, 0, string.Empty);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <param name="customId">A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url, string callback, string customId)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, customId, ScreenShotFormat.jpg, 0, string.Empty);
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
        public string TakePicture(string url, string callback, int browserWidth, int browserHeight, int outputHeight, int outputWidth, string customId, ScreenShotFormat format, int delay)
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
        public string TakePicture(string url, string callback, int browserWidth, int browserHeight, int outputHeight, int outputWidth, string customId, ScreenShotFormat format, int delay, string targetElement)
        {
            lock (thisLock)
            {
                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", applicationSecret, url, callback, format, outputHeight, outputWidth, browserHeight, browserWidth, customId, delay, targetElement));

                WebClient client = new WebClient();
                string result = client.DownloadString(string.Format(
                                                          "{0}takepicture.ashx?callback={1}&url={2}&key={3}&width={4}&height={5}&bwidth={6}&bheight={7}&format={8}&customid={9}&delay={10}&target={11}&sig={12}",
                                                          BaseURL, HttpUtility.UrlEncode(callback), HttpUtility.UrlEncode(url), applicationKey, outputWidth, outputHeight,
                                                          browserWidth, browserHeight, format, HttpUtility.UrlEncode(customId), delay, HttpUtility.UrlEncode(targetElement), HttpUtility.UrlEncode(sig)));

                XmlSerializer serializer = new XmlSerializer(typeof(TakePictureResult));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                TakePictureResult webResult = (TakePictureResult)serializer.Deserialize(memStream);

                if (!string.IsNullOrEmpty(webResult.Message))
                {
                    throw new Exception(webResult.Message);
                }

                return webResult.ID;
            }
        }

        /// <summary>
        /// This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="saveToFile">The file path that the screenshot should saved to: e.g. images/test.jpg</param>
        /// <returns>This function returns the true if it is successfull otherwise it throws an exception.</returns>
        public bool SavePicture(string url, string saveToFile)
        {
            return SavePicture(url, saveToFile, 0, 0, 0, 0, ScreenShotFormat.jpg, 0, string.Empty);
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
        public bool SavePicture(string url, string saveToFile, int browserWidth, int browserHeight, int outputHeight, int outputWidth, ScreenShotFormat format, int delay)
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
        public bool SavePicture(string url, string saveToFile, int browserWidth, int browserHeight, int outputHeight, int outputWidth, ScreenShotFormat format, int delay, string targetElement)
        {
            lock (thisLock)
            {
                string id = TakePicture(url, null, browserWidth, browserHeight, outputHeight, outputWidth, null, format, delay, targetElement);

                //Wait for it to be ready.
                while (true)
                {
                    ScreenShotStatus status = GetStatus(id);

                    if (!status.Cached && !status.Processing)
                    {
                        throw new Exception("The screenshot did not complete with the error: " + status.Message);
                    }

                    if (status.Cached)
                    {
                        using (Image result = GetPicture(id))
                        {
                            if (result == null)
                            {
                                throw new Exception("The screenshot image could not be found on GrabzIt.");
                            }
                            try
                            {
                                result.Save(saveToFile);
                            }
                            catch (Exception)
                            {
                                throw new Exception("An error occurred trying to save the screenshot to: " + saveToFile);
                            }
                        }
                        break;
                    }

                    Thread.Sleep(1);
                }

                return true;
            }
        }

        /// <summary>
        /// Get the current status of a GrabzIt screenshot
        /// </summary>
        /// <param name="id">The id of the screenshot</param>
        /// <returns>A Status object representing the screenshot</returns>
        public ScreenShotStatus GetStatus(string id)
        {
            lock (thisLock)
            {
                WebClient client = new WebClient();
                string result = client.DownloadString(string.Format(
                                                          "{0}getstatus.ashx?id={1}",
                                                          BaseURL, id));

                XmlSerializer serializer = new XmlSerializer(typeof(GetStatusResult));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                GetStatusResult status = (GetStatusResult)serializer.Deserialize(memStream);

                if (status == null)
                {
                    return null;
                }

                return status.GetStatus();
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
                string sig = Encrypt(string.Format("{0}|{1}", applicationSecret, domain));

                WebClient client = new WebClient();
                string result = client.DownloadString(string.Format(
                                                          "{0}getcookies.ashx?domain={1}&key={2}&sig={3}",
                                                          BaseURL, domain, applicationKey, sig));

                XmlSerializer serializer = new XmlSerializer(typeof(GetCookiesResult));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                GetCookiesResult getCookiesResult = (GetCookiesResult)serializer.Deserialize(memStream);

                if (!string.IsNullOrEmpty(getCookiesResult.Message))
                {
                    throw new Exception(getCookiesResult.Message);
                }

                return getCookiesResult.Cookies;
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
                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", applicationSecret, name, domain,
                                              value, path, (httponly ? 1 : 0), expires, 0));

                WebClient client = new WebClient();
                string result = client.DownloadString(string.Format(
                                                          "{0}setcookie.ashx?name={1}&domain={2}&value={3}&path={4}&httponly={5}&expires={6}&key={7}&sig={8}",
                                                          BaseURL, HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), HttpUtility.UrlEncode(value), HttpUtility.UrlEncode(path), (httponly ? 1 : 0), expires, applicationKey, sig));

                XmlSerializer serializer = new XmlSerializer(typeof(SetCookiesResult));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                SetCookiesResult setCookiesResult = (SetCookiesResult)serializer.Deserialize(memStream);

                if (!string.IsNullOrEmpty(setCookiesResult.Message))
                {
                    throw new Exception(setCookiesResult.Message);
                }

                return Convert.ToBoolean(setCookiesResult.Result);
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
                string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}", applicationSecret, name, domain, 1));

                WebClient client = new WebClient();

                string url = string.Format(
                                                          "{0}setcookie.ashx?name={1}&domain={2}&delete=1&key={3}&sig={4}",
                                                          BaseURL, HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(domain), applicationKey, sig);

                string result = client.DownloadString(url);

                XmlSerializer serializer = new XmlSerializer(typeof(SetCookiesResult));
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                SetCookiesResult setCookiesResult = (SetCookiesResult)serializer.Deserialize(memStream);

                if (!string.IsNullOrEmpty(setCookiesResult.Message))
                {
                    throw new Exception(setCookiesResult.Message);
                }

                return Convert.ToBoolean(setCookiesResult.Result);
            }
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
                                                                                "{0}getpicture.ashx?id={1}",
                                                                                BaseURL, id));
                Image image;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentLength == 0)
                    {
                        return null;
                    }

                    using (Stream stream = response.GetResponseStream())
                    {
                        image = Image.FromStream(stream);
                    }
                }

                return image;
            }
        }

        private static string Encrypt(string plainText)
        {
            byte[] bs = Encoding.ASCII.GetBytes(plainText);
            return toHex(hasher.ComputeHash(bs));
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

        /// <summary>
        /// Create a new GrabzIt client. Note if you are using Multi-Threading consider using the public constructor to improve performance.
        /// </summary>
        /// <param name="applicationKey">The application key of your GrabzIt account</param>
        /// <param name="applicationSecret">The application secret of your GrabzIt account</param>
        /// <returns>A copy of GrabzIt</returns>
        public static GrabzItClient Create(string applicationKey, string applicationSecret)
        {
            if (grabzItClient == null)
            {
                grabzItClient = new GrabzItClient(applicationKey, applicationSecret);
            }
            return grabzItClient;
        }

        internal static GrabzItClient WebClient
        {
            get { return grabzItClient; }
        }
    }
}