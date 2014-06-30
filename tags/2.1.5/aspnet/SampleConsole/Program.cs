using System;
using System.Configuration;
using System.Windows.Forms;
using GrabzIt;

namespace SampleConsole
{
    class Program
    {
        private static string PDF = "P";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please specify a URL to take picture of. For example http://www.google.com");
                string url = Console.ReadLine();
                Console.WriteLine("Return URL as PDF (P) or JPEG (J)? Enter P or J.");
                string formatType = Console.ReadLine();
                GrabzItClient grabzIt = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], 
                                                                ConfigurationManager.AppSettings["ApplicationSecret"]);

                try
                {
                    string format = ".jpg";
                    if (formatType == PDF)
                    {
                        format = ".pdf";
                    }
                    string filename = url.Substring(url.IndexOf("://") + 3) + format;

                    if (formatType == PDF)
                    {
                        grabzIt.SetPDFOptions(url);
                    }
                    else
                    {
                        grabzIt.SetImageOptions(url);
                    }                    
                    if (grabzIt.SaveTo(filename))
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
