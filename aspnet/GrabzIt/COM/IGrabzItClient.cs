using GrabzIt.Cookies;
using GrabzIt.Enums;
using GrabzIt.Screenshots;
using System;

namespace GrabzIt.COM
{
    public interface IGrabzItClient
    {
        string ApplicationKey { get; set; }
        string ApplicationSecret { get; set; }
        void SetAnimationOptions(string url);
        void SetAnimationOptions(string url, string customId);
        void SetAnimationOptions(string url, string customId, int width, int height, int start, int duration, float speed, float framesPerSecond, int repeat, bool reverse, string customWaterMarkId);
        void SetAnimationOptions(string url, string customId, int width, int height, int start, int duration, float speed, float framesPerSecond, int repeat, bool reverse, string customWaterMarkId, int quality, Country country);
        void SetImageOptions(string url, string customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, string targetElement, BrowserType requestAs, string customWaterMarkId, int quality, Country country);
        void SetImageOptions(string url);
        void SetImageOptions(string url, string customId);
        void SetImageOptions(string url, string customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, string targetElement, BrowserType requestAs, string customWaterMarkId);
        void SetTableOptions(string url, string customId, int tableNumberToInclude, TableFormat format, bool includeHeaderNames, bool includeAllTables, string targetElement, BrowserType requestAs, Country country);
        void SetTableOptions(string url, string customId, int tableNumberToInclude, TableFormat format, bool includeHeaderNames, bool includeAllTables, string targetElement, BrowserType requestAs);
        void SetTableOptions(string url);
        void SetTableOptions(string url, string customId);
        void SetPDFOptions(string url, string customId, bool includeBackground, PageSize pagesize, PageOrientation orientation, bool includeLinks, bool includeOutline, string title, string coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, string customWaterMarkId, int quality, Country country);
        void SetPDFOptions(string url);
        void SetPDFOptions(string url, string customId);
        void SetPDFOptions(string url, string customId, bool includeBackground, PageSize pagesize, PageOrientation orientation, bool includeLinks, bool includeOutline, string title, string coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, string customWaterMarkId);
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
