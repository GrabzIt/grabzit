using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.COM
{
    interface ITableOptions
    {
        Country Country
        {
            get;
            set;
        }

        string ExportURL
        {
            get;
            set;
        }

        string EncryptionKey
        {
            get;
            set;
        }

        string CustomId
        {
            get;
            set;
        }

        int TableNumberToInclude
        {
            get;
            set;
        }

        TableFormat Format
        {
            get;
            set;
        }

        bool IncludeHeaderNames
        {
            get;
            set;
        }

        bool IncludeAllTables
        {
            get;
            set;
        }

        string TargetElement
        {
            get;
            set;
        }

        BrowserType RequestAs
        {
            get;
            set;
        }

        string Proxy
        {
            get;
            set;
        }

        string Address
        {
            get;
            set;
        }

        void AddPostParameter(string name, string value);
    }
}
