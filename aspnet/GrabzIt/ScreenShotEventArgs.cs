using System.Runtime.InteropServices;
namespace GrabzIt
{
    [ComVisible(false)]
    public class ScreenShotEventArgs
    {
        public string Filename { get; internal set; }
        public string ID { get; internal set; }
        public string Message { get; internal set; }
        public string CustomID { get; internal set; }
        public string Format { get; internal set; }

        public ScreenShotEventArgs(string filename, string id, string message, string customId, string format)
        {
            Filename = filename;
            ID = id;
            Message = message;
            CustomID = customId;
            Format = format;
        }
    }
}