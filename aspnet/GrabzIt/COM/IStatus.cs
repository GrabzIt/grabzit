using System;

namespace GrabzIt.COM
{
    public interface IStatus
    {
        bool Processing
        {
            get;
        }

        bool Cached
        {
            get;
        }

        bool Expired
        {
            get;
        }

        string Message
        {
            get;
        }
    }
}
