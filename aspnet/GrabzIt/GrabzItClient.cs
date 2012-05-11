using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

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
            return TakePicture(url, string.Empty, 0, 0, 0, 0, string.Empty, string.Empty, 0);
        }

        /// <summary>
        /// This method calls the GrabzIt web service to take the screenshot.
        /// </summary>
        /// <param name="url">The URL that the screenshot should be made of</param>
        /// <param name="callback">The handler the GrabzIt web service should call after it has completed its work</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url, string callback)
        {
            return TakePicture(url, callback, 0, 0, 0, 0, string.Empty, string.Empty, 0);
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
            return TakePicture(url, callback, 0, 0, 0, 0, customId, string.Empty, 0);
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
        /// <param name="format">The format the screenshot should be in: jpg, gif, png</param>
        /// <param name="delay">The number of milliseconds to wait before taking the screenshot</param>
        /// <returns>The unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.</returns>
        public string TakePicture(string url, string callback, int browserWidth, int browserHeight, int outputHeight, int outputWidth, string customId, string format, int delay)
        {
            string sig = Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", applicationSecret, url, callback, format, outputHeight, outputWidth, browserHeight, browserWidth, customId, delay));

            WebClient client = new WebClient();
            string result = client.DownloadString(string.Format(
                                                      "{0}takepicture.ashx?callback={1}&url={2}&key={3}&width={4}&height={5}&bwidth={6}&bheight={7}&format={8}&customid={9}&delay={10}&sig={11}",
                                                      BaseURL, HttpUtility.UrlEncode(callback), HttpUtility.UrlEncode(url), applicationKey, outputWidth, outputHeight,
                                                      browserWidth, browserHeight, format, HttpUtility.UrlEncode(customId), delay, HttpUtility.UrlEncode(sig)));

            XmlSerializer serializer = new XmlSerializer(typeof(WebResult));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            WebResult webResult = (WebResult)serializer.Deserialize(memStream);

            if (!string.IsNullOrEmpty(webResult.Message))
            {
                throw new Exception(webResult.Message);
            }

            return webResult.ID;
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