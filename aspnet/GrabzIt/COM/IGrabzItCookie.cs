using System;

namespace GrabzIt.COM
{
    public interface IGrabzItCookie
    {
        string Name
        {
            get;
        }

        string Value
        {
            get;
        }

        string Domain
        {
            get;
        }

        string Path
        {
            get;
        }

        string HttpOnly
        {
            get;
        }

        string Expires
        {
            get;
        }

        string Type
        {
            get;
        }
    }
}
