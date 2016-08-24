using GrabzIt.Results;
using GrabzIt.Scraper.Enums;
using GrabzIt.Scraper.Net;
using GrabzIt.Scraper.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace GrabzIt.Scraper
{
    public class GrabzItScrapeClient
    {
        public string ApplicationKey
        {
            get;
            private set;
        }

        public string ApplicationSecret
        {
            get;
            private set;
        }

        private const string BaseURL = "http://api.grabz.it/services/scraper/";

        public GrabzItScrapeClient(string applicationKey, string applicationSecret)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationSecret = applicationSecret;
        }

        public GrabzItScrape[] GetScrapes()
        {
            return GetScrapes(string.Empty);
        }

        public GrabzItScrape GetScrape(string identifier)
        {
            GrabzItScrape[] scrapes = GetScrapes(identifier);
            if (scrapes.Length == 1)
            {
                return scrapes[0];
            }
            return null;
        }

        private GrabzItScrape[] GetScrapes(string identifier)
        {
            string sig = Encrypt(string.Format("{0}|{1}", ApplicationSecret, identifier));

            string url = string.Format("{0}getscrapes.ashx?key={1}&identifier={2}&sig={3}",
                                                      BaseURL, ApplicationKey, identifier, sig);

            GetScrapesResult webResult = Get<GetScrapesResult>(url);

            CheckForException(webResult);

            return webResult.Scrapes;
        }

        public bool SetScrapeStatus(string id, ScrapeStatus status)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            string sig = Encrypt(string.Format("{0}|{1}|{2}", ApplicationSecret, id, status.ToString()));

            string url = string.Format("{0}setscrapestatus.ashx?key={1}&id={2}&status={3}&sig={4}",
                                                      BaseURL, ApplicationKey, id, status.ToString(), sig);

            GenericResult webResult = Get<GenericResult>(url);

            if (webResult == null)
            {
                return false;
            }

            CheckForException(webResult);

            return Convert.ToBoolean(webResult.Result);
        }

        private void CheckForException(IException result)
        {
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    throw new GrabzItScrapeException(result.Message, result.Code);
                }
            }
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
                if (((int)response.StatusCode) >= 400)
                {
                    throw new GrabzItScrapeException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NetworkGeneralError);
                }
            }
            else if (e.Status == WebExceptionStatus.NameResolutionFailure)
            {
                throw new GrabzItScrapeException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NetworkGeneralError);
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

        private T DeserializeResult<T>(string result)
        {
            using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memStream);
            }
        }
    }
}
