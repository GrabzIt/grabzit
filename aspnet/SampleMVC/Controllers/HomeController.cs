using GrabzIt;
using GrabzIt.Screenshots;
using SampleMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace SampleMVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormModel form)
        {
            GrabzItClient grabzItClient = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);

            form.UseCallbackHandler = !HttpContext.Request.IsLocal;
            try
            {
                if (form.Format == "jpg")
                {
                    if (form.Convert == "html")
                    {
                        grabzItClient.HTMLToImage(form.HTML);
                    }
                    else
                    {
                        grabzItClient.URLToImage(form.URL);
                    }
                }
                else if (form.Format == "docx")
                {
                    if (form.Convert == "html")
                    {
                        grabzItClient.HTMLToDOCX(form.HTML);
                    }
                    else
                    {
                        grabzItClient.URLToDOCX(form.URL);
                    }
                }
                else if (form.Format == "csv")
                {
                    if (form.Convert == "html")
                    {
                        grabzItClient.HTMLToTable(form.HTML);
                    }
                    else
                    {
                        grabzItClient.URLToTable(form.URL);
                    }
                }
                else if (form.Format == "gif")
                {
                    grabzItClient.URLToAnimation(form.URL);
                }
                else
                {
                    if (form.Convert == "html")
                    {
                        grabzItClient.HTMLToPDF(form.HTML);
                    }
                    else
                    {
                        grabzItClient.URLToPDF(form.URL);
                    }
                }
                if (form.UseCallbackHandler)
                {
                    grabzItClient.Save(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + HttpContext.Request.ApplicationPath + "Home/Handler");                    
                }
                else
                {
                    grabzItClient.SaveTo(Server.MapPath("~/results/" + Guid.NewGuid().ToString() + "." + form.Format));
                }
                form.Message = "Processing...";
            }
            catch (Exception ex)
            {
                form.Error = ex.Message;
            }
            return View(form);
        }

        public ActionResult Handler(string filename, string id, string message, string customId, string format, int targeterror)
        {
            GrabzItClient grabzItClient = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
            GrabzItFile file = grabzItClient.GetResult(id);
            //Ensure that the application has the correct rights for this directory.
            file.Save(Server.MapPath("~/results/" + filename));

            return null;
        }

        public ActionResult Index()
        {
            FormModel form = new FormModel();
            form.UseCallbackHandler = !HttpContext.Request.IsLocal;
            return View(form);
        }

        public JsonResult Results()
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/results"));
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

            return this.Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete()
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/results"));
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }

            return RedirectToAction("Index");
        }
    }
}