#if NETFRAMEWORK
using System.IO;
using System.Web;

namespace GrabzIt.Scraper.Net
{
    public class FrameworkFile : IGenericFile
    {
        private readonly HttpPostedFileBase httpPostedFileBase;

        public FrameworkFile(HttpPostedFileBase httpPostedFileBase)
        {
            this.httpPostedFileBase = httpPostedFileBase;
        }
        public string FileName
        {
            get
            {
                if (this.httpPostedFileBase == null)
                {
                    return string.Empty;
                }
                return this.httpPostedFileBase.FileName;
            }
        }

        public Stream Data
        {
            get
            {
                if (this.httpPostedFileBase == null)
                {
                    return null;
                }
                return this.httpPostedFileBase.InputStream;
            }
        }
    }
}
#endif