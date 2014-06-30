using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;

namespace Sample.ajax
{
    public class results : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string[] files = Directory.GetFiles(context.Server.MapPath("~/results"));
            List<string> results = new List<string>();
            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file) || file.Contains(".txt"))
                {
                    continue;
                }
                FileInfo info = new FileInfo(file);
                results.Add("../results/" + info.Name);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(results.ToArray());
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
