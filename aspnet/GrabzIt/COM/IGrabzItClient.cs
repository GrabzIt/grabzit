using GrabzIt.Cookies;
using GrabzIt.Enums;
using GrabzIt.Parameters;
using GrabzIt.Screenshots;
using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    /// <summary>
    /// To avoid overloading which may not work in certain enviroments such as VB6 we do not include all overrides possible:
    /// https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1402-avoid-overloads-in-com-visible-interfaces?view=vs-2017
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IGrabzItClient
    {
        string ApplicationKey { get; set; }
        string ApplicationSecret { get; set; }
        void SetLocalProxy(string proxyUrl);
        void UseSSL(bool value);
        string CreateEncryptionKey();
        void Decrypt(string path, string key);
        void URLToAnimation(string url, AnimationOptions options);
        void URLToImage(string url, ImageOptions options);
        void URLToPDF(string url, PDFOptions options);
        void URLToTable(string url, TableOptions options);
        void URLToDOCX(string url, DOCXOptions options);
        void URLToRenderedHTML(string url, HTMLOptions options);
        void HTMLToImage(string html, ImageOptions options);
        void HTMLToTable(string html, TableOptions options);
        void HTMLToPDF(string html, PDFOptions options);
        void HTMLToDOCX(string html, DOCXOptions options);
        void HTMLToRenderedHTML(string html, HTMLOptions options);
        void FileToImage(string path, ImageOptions options);        
        void FileToTable(string path, TableOptions options);
        void FileToPDF(string path, PDFOptions options);
        void FileToDOCX(string path, DOCXOptions options);
        void FileToRenderedHTML(string path, HTMLOptions options);
        string Save(string callBackURL);
        bool SaveTo(string saveToFile);
        Status GetStatus(string id);
        GrabzItCookie[] GetCookies(string domain);
        bool SetCookie(string name, string domain, string value, string path, bool httponly, DateTime expires);
        bool DeleteCookie(string name, string domain);
        bool AddWaterMark(string identifier, string path, HorizontalPosition xpos, VerticalPosition ypos);
        bool DeleteWaterMark(string identifier);
        WaterMark[] GetWaterMarks();
        WaterMark GetWaterMark(string identifier);
        GrabzItFile GetResult(string id);
    }
}
