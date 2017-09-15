using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt.Scraper.Property
{
    /// <summary>
    /// Change a global variable contained within the scrape instructions
    /// </summary>
    [Serializable]
    public class Variable : IProperty
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<KeyValue> Array = null;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Value;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name;

        [XmlIgnore]
        public string Type
        {
            get
            {
                return "Variable";
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Variable() { }

        /// <summary>
        /// Create the variable with the desired name. If a variable with the same name exists it will be overwritten
        /// </summary>
        /// <param name="name">The name of the variable</param>
        public Variable(string name) { this.Name = name; }

        /// <summary>
        /// Set the value of the variable
        /// </summary>
        /// <param name="value">A string value</param>
        public void SetValue(string value)
        {
            if (value == null)
            {
                return;
            }
            Value = value;
        }

        /// <summary>
        /// Set the value of the variable
        /// </summary>
        /// <param name="value">A integer value</param>
        public void SetValue(int value)
        {
            Value = value.ToString();
        }

        /// <summary>
        /// Set the value of the variable
        /// </summary>
        /// <param name="value">A double value</param>
        public void SetValue(double value)
        {
            Value = value.ToString();
        }

        /// <summary>
        /// Set the value of the variable
        /// </summary>
        /// <param name="value">A long value</param>
        public void SetValue(long value)
        {
            Value = value.ToString();
        }

        /// <summary>
        /// Add an array item to the variable
        /// </summary>
        /// <param name="value">A string array item</param>
        public void AddArrayItem(string value)
        {
            if (value == null)
            {
                return;
            }
            if (Array == null)
            {
                Array = new List<KeyValue>();
            }
            Array.Add(new KeyValue(Array.Count.ToString(), value));
        }

        /// <summary>
        /// Add an array item to the variable
        /// </summary>
        /// <param name="value">A int array item</param>
        public void AddArrayItem(int value)
        {
            AddArrayItem(value.ToString());
        }

        /// <summary>
        /// Add an array item to the variable
        /// </summary>
        /// <param name="value">A double array item</param>
        public void AddArrayItem(double value)
        {
            AddArrayItem(value.ToString());
        }

        /// <summary>
        /// Add an array item to the variable
        /// </summary>
        /// <param name="value">A long array item</param>
        public void AddArrayItem(long value)
        {
            AddArrayItem(value.ToString());
        }

        /// <summary>
        /// Add an dictionary item to the variable
        /// </summary>
        /// <param name="key">A key</param>
        /// <param name="value">A value</param>
        public void AddDictionaryItem(string key, string value)
        {
            if (value == null)
            {
                return;
            }
            if (Array == null)
            {
                Array = new List<KeyValue>();
            }
            Array.Add(new KeyValue(key, value));
        }

        public string ToXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Variable));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, this);
                return textWriter.ToString();
            }
        }
    }
}
