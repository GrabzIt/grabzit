using GrabzIt.COM;
using System.Runtime.InteropServices;
namespace GrabzIt.Screenshots
{
    [ClassInterface(ClassInterfaceType.None)]
    public class Status : IStatus
    {
        public bool Processing
        {
            get;
            internal set;
        }

        public bool Cached
        {
            get;
            internal set;
        }

        public bool Expired
        {
            get;
            internal set;
        }

        public string Message
        {
            get;
            internal set;
        }
    }
}
