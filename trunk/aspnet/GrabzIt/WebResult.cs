using System;
using System.IO;

namespace GrabzIt
{
    [Serializable]
    public class WebResult
    {
        public string Result;
        public string ID;
        public string Message;

        public WebResult()
        {            
        }

        public WebResult(Stream stream, string message) : this (stream, false.ToString(), message, string.Empty)
        {
        }

        public WebResult(Stream stream, string result, string message, string id)
        {
            Result = result;
            Message = message;
            ID = id;
            
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
            x.Serialize(stream, this);
        }
    }
}