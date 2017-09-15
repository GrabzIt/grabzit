using System;
using System.ComponentModel;

namespace GrabzIt.Scraper.Property
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public class KeyValue
    {
        public string Key;
        public string Value;

        public KeyValue()
        {
        }

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
