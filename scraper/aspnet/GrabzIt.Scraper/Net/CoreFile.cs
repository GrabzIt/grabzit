#if NETSTANDARD
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GrabzIt.Scraper.Net
{
    internal class CoreFile : IGenericFile
    {
        private readonly IFormFile file;

        internal CoreFile(IFormFile file)
        {
            this.file = file;
        }
        public string FileName
        {
            get
            {
                if (file == null)
                {
                    return string.Empty;
                }
                return file.FileName;
            }
        }

        public Stream Data
        {
            get
            {
                if (file == null) 
                {
                    return null;
                }
                return file.OpenReadStream();
            }
        }
    }
}
#endif