using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleMVC.Models
{
    public class FormModel
    {
        public string Convert
        {
            get;set;
        }

        public string HTML
        {
            get; set;
        }

        public string URL
        {
            get; set;
        }

        public string Format
        {
            get; set;
        }

        public bool UseCallbackHandler
        {
            get;set;
        }

        public string Message
        {
            get; set;
        }

        public string Error
        {
            get; set;
        }
    }
}