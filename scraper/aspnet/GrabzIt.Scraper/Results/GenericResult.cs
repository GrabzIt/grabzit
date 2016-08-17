using System;
using System.Xml.Serialization;

namespace GrabzIt.Scraper.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GenericResult : IException
    {
        public string Result;

        public string Message
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public GenericResult()
        {            
        }
    }
}