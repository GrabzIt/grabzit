using System;
using System.Configuration;
using System.Drawing;
using System.Net.Mime;
using System.Threading;
using System.Windows.Forms;
using GrabzIt;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please specify a URL to take picture of. For example http://www.google.com");
                string url = Console.ReadLine();
                GrabzItClient grabzIt = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"],
                                                             ConfigurationManager.AppSettings["ApplicationSecret"]);
                try
                {
                    string id = grabzIt.TakePicture(url);
                    bool completed = false;
                    while (!completed)
                    {
                        Image image = grabzIt.GetPicture(id);
                        if (image != null)
                        {
                            string filename = url.Substring(url.IndexOf("://") + 3) + ".jpg";
                            Console.WriteLine("Screenshot has been saved to: " + filename);
                            image.Save(filename);
                            completed = true;
                        }
                        Thread.Sleep(1000);
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("Do you wish to exit? (y/n)");
                string exit = Console.ReadKey().KeyChar.ToString();
                if (exit.ToLower() == "y")
                {
                    break;
                }
                Application.DoEvents();
            }
        }
    }
}
