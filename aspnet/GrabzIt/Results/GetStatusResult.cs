using System;
using System.Xml.Serialization;
using GrabzIt.Screenshots;
using System.Runtime.InteropServices;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    [ComVisible(false)]
    public class GetStatusResult : IException
    {
        public string Processing;
        public string Cached;
        public string Expired;

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

        public GetStatusResult()
        {            
        }

        internal Status GetStatus()
        {
            Status status = new Status();
            status.Cached = StringToBool(Cached);
            status.Expired = StringToBool(Expired);
            status.Processing = StringToBool(Processing);
            status.Message = Message;

            return status;
        }

        private bool StringToBool(string str)
        {
            if (str.Trim().ToUpper() == "TRUE")
            {
                return true;
            }
            return false;
        }
    }
}
