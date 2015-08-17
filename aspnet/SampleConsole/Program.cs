using System;
using System.Configuration;
using System.Windows.Forms;
using GrabzIt;

namespace SampleConsole
{
    class Program
    {
        private static string PDF = "P";
        private static string GIF = "G";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please specify a URL to take picture of. For example http://www.google.com");
                string url = Console.ReadLine();
                Console.WriteLine("Return URL as PDF (P), JPEG (J) or Animated GIF (G)? Enter P, J or G.");
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
                    else if (formatType == GIF)
                    {
                        format = ".gif";
                    }

                    string filename = url.Substring(url.IndexOf("://") + 3).Replace("?", string.Empty).Replace("/", string.Empty) + format;

                    if (formatType == PDF)
                    {
                        grabzIt.SetPDFOptions(url);
                    }
                    else if (formatType == GIF)
                    {
                        grabzIt.SetAnimationOptions(url);
                    }
                    else
                    {
                        grabzIt.SetImageOptions(url);
                    }                    
                    if (grabzIt.SaveTo(filename))
                    {
                        if (formatType == GIF)
                        {
                            Console.WriteLine("Animated GIF has been saved to: " + filename);
                        }
                        else
                        {
                            Console.WriteLine("Screenshot has been saved to: " + filename);
                        }
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
