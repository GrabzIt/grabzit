using System;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt.Scraper.Property
{
    /// <summary>
    /// Change the target of a scrape
    /// </summary>
    [Serializable]    
    public class Target : IProperty
    {
        /// <summary>
        /// Specify the seed URL's of a scrape, if any
        /// </summary>
        public string[] SeedURLs;
        /// <summary>
        /// Specify the URL to start the scrape on
        /// </summary>
        public string URL;

        [XmlIgnore]
        public string Type
        {
            get
            {
                return "Target";
            }
        }

        public string ToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Target));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, this);
                return textWriter.ToString();
            }
        }
    }
}
