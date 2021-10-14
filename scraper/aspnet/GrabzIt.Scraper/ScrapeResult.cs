using GrabzIt.Scraper.Net;
#if NETSTANDARD
using Microsoft.AspNetCore.Http;
#endif
#if NETFRAMEWORK
using System.Web;
#endif
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt.Scraper
{
    public class ScrapeResult
    {
        private IGenericRequest genericRequest;
        private string data;
        private string filename;
        private string extension;

#if NETFRAMEWORK
        public ScrapeResult(HttpRequest request) : this(new HttpRequestWrapper(request)){}

        public ScrapeResult(HttpRequestBase request)
        {
            this.genericRequest = new FrameworkRequest(request);
            UserAgentCheck();
        }
#endif
#if NETSTANDARD
        public ScrapeResult(HttpRequest request)
        {
            this.genericRequest = new CoreRequest(request);
            UserAgentCheck();
        }
#endif
        private void UserAgentCheck()
        {
            if (genericRequest.UserAgent != "GrabzIt")
            {
                throw new Exception("A call originating from a non-GrabzIt server has been detected");
            }
        }

        public ScrapeResult(string path)
        {
            this.data = File.ReadAllText(path);
            this.filename = Path.GetFileName(path);
            this.extension = Path.GetExtension(path).Substring(1).ToLower();
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(filename))
                {
                    IGenericFile file = genericRequest.GetFile();
                    if (file != null)
                    {
                        filename = Path.GetFileName(file.FileName);
                    }
                }
                return filename;
            }
        }

        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(extension))
                {
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        extension = Path.GetExtension(FileName).Substring(1).ToLower();
                    }
                }
                return extension;
            }
        }

        public bool Save(string path)
        {
            try
            {
                File.WriteAllText(path, ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T FromJSON<T>()
        {
            if (Extension == "json")
            {                
                return JsonConvert.DeserializeObject<T>(ToString());
            }
            return default(T);
        }

        public T FromXML<T>()
        {
            if (Extension == "xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(ToString()))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            return default(T);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(data))
            {
                IGenericFile file = genericRequest.GetFile();
                if (file != null)
                {
                    using (StreamReader sr = new StreamReader(file.Data))
                    {
                        data = sr.ReadToEnd();
                    }
                }
            }
            return data;
        }
    }
}
