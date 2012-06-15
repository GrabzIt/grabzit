using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.Collections.Generic;
using GrabzIt.Result;
using GrabzIt.Cookies;

namespace GrabzIt
{
    public class GrabzItClient
    {
        private static GrabzItClient grabzItClient;

        public delegate void ScreenShotHandler(object sender, ScreenShotEventArgs result);
        public event ScreenShotHandler ScreenShotComplete;

        private readonly string applicationKey;
        private readonly string applicationSecret;
        private const string BaseURL = "http://grabz.it/services/";

        internal GrabzItClient(string applicationKey, string applicationSecret)
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
            return TakePicture(url, string.Empty, 0, 0, 0, 0, string.Empty, ScreenShotFormat.jpg, 0);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url, string callback)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, string.Empty, ScreenShotFormat.jpg, 0);
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
            return TakePicture(url, callback, 0, 0, 0, 0, customId, ScreenShotFormat.jpg, 0);
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
            string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", applicationSecret, url, callback, format, outputHeight, outputWidth, browserHeight, browserWidth, customId, delay));

            WebClient client = new WebClient();
            string result = client.DownloadString(string.Format(
                                                      "{0}takepicture.ashx?callback={1}&url={2}&key={3}&width={4}&height={5}&bwidth={6}&bheight={7}&format={8}&customid={9}&delay={10}&sig={11}",
                                                      BaseURL, HttpUtility.UrlEncode(callback), HttpUtility.UrlEncode(url), applicationKey, outputWidth, outputHeight,
                                                      browserWidth, browserHeight, format, HttpUtility.UrlEncode(customId), delay, HttpUtility.UrlEncode(sig)));

            XmlSerializer serializer = new XmlSerializer(typeof(TakePictureResult));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            TakePictureResult webResult = (TakePictureResult)serializer.Deserialize(memStream);

            if (!string.IsNullOrEmpty(webResult.Message))
            {
                throw new Exception(webResult.Message);
            }

            return webResult.ID;
        }

        /// <summary>
        /// Get the current status of a GrabzIt screenshot
        /// </summary>
        /// <param name="id">The id of the screenshot</param>
        /// <returns>A Status object representing the screenshot</returns>
        public ScreenShotStatus GetStatus(string id)
        {
            WebClient client = new WebClient();
            string result = client.DownloadString(string.Format(
                                                      "{0}getstatus.ashx?id={1}",
                                                      BaseURL, id));
            
            XmlSerializer serializer = new XmlSerializer(typeof(ScreenShotStatus));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            ScreenShotStatus status = (ScreenShotStatus)serializer.Deserialize(memStream);

            return status;
        }

        /// <summary>
        /// Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
        /// </summary>
        /// <param name="domain">The domain to return cookies for.</param>
        /// <returns>A array of cookies</returns>
        public GrabzItCookie[] GetCookies(string domain)
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

        /// <summary>
        /// Delete a custom cookie or block a global cookie from being used.
        /// </summary>
        /// <param name="name">The name of the cookie to delete</param>
        /// <param name="domain">The website the cookie belongs to</param>
        /// <returns>Returns true if the cookie was successfully set.</returns>
        public bool DeleteCookie(string name, string domain)
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

        /// <summary>
        /// This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.
        /// </summary>
        /// <param name="id">The unique identifier of the screenshot, returned by the callback handler or the TakePicture method</param>
        /// <returns>The screenshot</returns>
        public Image GetPicture(string id)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format(
                                                                            "{0}getpicture.ashx?id={1}",
                                                                            BaseURL, id));
            Image image;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.ContentLength == 0)
                {
                    return null;
                }

                Stream stream = response.GetResponseStream();

                image = Image.FromStream(stream);
            }

            return image;
        }

        private static string Encrypt(string plainText)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.ASCII.GetBytes(plainText);
            bs = x.ComputeHash(bs);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                sb.Append(bs[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Create a new GrabzIt client
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