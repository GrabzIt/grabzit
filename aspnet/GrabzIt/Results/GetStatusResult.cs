using System;
using System.Xml.Serialization;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GetStatusResult
    {
        public string Processing;
        public string Cached;
        public string Expired;
        public string Message;

        public GetStatusResult()
        {            
        }

        internal ScreenShotStatus GetStatus()
        {
            ScreenShotStatus status = new ScreenShotStatus();
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
