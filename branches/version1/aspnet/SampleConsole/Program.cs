using System;
using System.Configuration;
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
                    string filename = url.Substring(url.IndexOf("://") + 3) + ".jpg";

                    if (grabzIt.SavePicture(url, filename))
                    {
                        Console.WriteLine("Screenshot has been saved to: " + filename);
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
