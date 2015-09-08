using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace GrabzIt.Scraper
{
    public class ScrapeResult
    {
        private HttpRequestBase request;
        private string data;
        private string filename;
        private string extension;

        public ScrapeResult(HttpRequest request) : this(new HttpRequestWrapper(request)){}

        public ScrapeResult(HttpRequestBase request)
        {
            this.request = request;
            if (this.request.UserAgent != "GrabzIt")
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

        private HttpPostedFileBase getFile()
        {
            return request.Files["file"];
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(filename))
                {
                    HttpPostedFileBase file = getFile();
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
                    HttpPostedFileBase file = getFile();
                    if (file != null)
                    {
                        extension = Path.GetExtension(file.FileName).Substring(1).ToLower();
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
                return new JavaScriptSerializer().Deserialize<T>(ToString());
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
                HttpPostedFileBase file = getFile();
                if (file != null)
                {
                    using (StreamReader sr = new StreamReader(file.InputStream))
                    {
                        data = sr.ReadToEnd();
                    }
                }
            }
            return data;
        }
    }
}
