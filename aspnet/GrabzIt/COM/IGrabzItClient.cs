using GrabzIt.Cookies;
using GrabzIt.Enums;
using GrabzIt.Parameters;
using GrabzIt.Screenshots;
using System;

namespace GrabzIt.COM
{
    public interface IGrabzItClient
    {
        string ApplicationKey { get; set; }
        string ApplicationSecret { get; set; }
        void SetProxy(string proxyUrl);
        void URLToAnimation(string url);
        void URLToAnimation(string url, AnimationOptions options);
        void URLToImage(string url);
        void URLToImage(string url, ImageOptions options);        
        void HTMLToImage(string html);
        void HTMLToImage(string html, ImageOptions options);
        void FileToImage(string path);
        void FileToImage(string path, ImageOptions options);
        void URLToTable(string url);
        void URLToTable(string url, TableOptions options);
        void HTMLToTable(string html);
        void HTMLToTable(string html, TableOptions options);
        void FileToTable(string path);
        void FileToTable(string path, TableOptions options);
        void URLToPDF(string url);
        void URLToPDF(string url, PDFOptions options);
        void HTMLToPDF(string html);
        void HTMLToPDF(string html, PDFOptions options);
        void FileToPDF(string path);
        void FileToPDF(string path, PDFOptions options);
        string Save();
        string Save(string callBackURL);
        GrabzItFile SaveTo();
        bool SaveTo(string saveToFile);
        Status GetStatus(string id);
        GrabzItCookie[] GetCookies(string domain);
        bool SetCookie(string name, string domain);
        bool SetCookie(string name, string domain, string value);
        bool SetCookie(string name, string domain, string value, string path);
        bool SetCookie(string name, string domain, string value, string path, bool httponly);
        bool SetCookie(string name, string domain, string value, string path, bool httponly, DateTime? expires);
        bool DeleteCookie(string name, string domain);
        bool AddWaterMark(string identifier, string path, HorizontalPosition xpos, VerticalPosition ypos);
        bool DeleteWaterMark(string identifier);
        WaterMark[] GetWaterMarks();
        WaterMark GetWaterMark(string identifier);
        GrabzItFile GetResult(string id);
    }
}
