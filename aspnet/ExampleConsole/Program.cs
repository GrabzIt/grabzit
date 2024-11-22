using GrabzIt;
using Microsoft.Extensions.Configuration;

namespace ExampleConsole
{
    internal class Program
    {
        private static string PDF = "P";
        private static string DOCX = "D";
        private static string GIF = "G";
        private static string VIDEO = "V";
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();

            while (true)
            {
                Console.WriteLine("Return Capture as PDF (P), DOCX (D), JPEG (J), Video (V) or Animated GIF (G)? Enter P, J, V or G.");
                string formatType = Console.ReadLine();

                bool convertUrl = true;
                if (formatType != GIF)
                {
                    Console.WriteLine("Do you want to convert a URL (U) or to convert HTML (H)? Enter U or H.");
                    convertUrl = (Console.ReadLine() == "U");
                }

                if (convertUrl)
                {
                    Console.WriteLine("Please specify a URL to capture. For example http://www.google.com");
                }
                else
                {
                    Console.WriteLine("Please specify some HTML to convert.");
                }

                string inputData = Console.ReadLine();

                GrabzItClient grabzIt = new GrabzItClient(config["ApplicationKey"],
                                                                config["ApplicationSecret"]);

                try
                {
                    string format = ".jpg";
                    if (formatType == PDF)
                    {
                        format = ".pdf";
                    }
                    else if (formatType == DOCX)
                    {
                        format = ".docx";
                    }
                    else if (formatType == GIF)
                    {
                        format = ".gif";
                    }
                    else if (formatType == VIDEO)
                    {
                        format = ".mp4";
                    }

                    string filename = Guid.NewGuid().ToString() + format;

                    if (formatType == PDF)
                    {
                        if (convertUrl)
                        {
                            grabzIt.URLToPDF(inputData);
                        }
                        else
                        {
                            grabzIt.HTMLToPDF(inputData);
                        }
                    }
                    else if (formatType == DOCX)
                    {
                        if (convertUrl)
                        {
                            grabzIt.URLToDOCX(inputData);
                        }
                        else
                        {
                            grabzIt.HTMLToDOCX(inputData);
                        }
                    }
                    else if (formatType == VIDEO)
                    {
                        if (convertUrl)
                        {
                            grabzIt.URLToVideo(inputData);
                        }
                        else
                        {
                            grabzIt.HTMLToVideo(inputData);
                        }
                    }
                    else if (formatType == GIF)
                    {
                        grabzIt.URLToAnimation(inputData);
                    }
                    else
                    {
                        if (convertUrl)
                        {
                            grabzIt.URLToImage(inputData);
                        }
                        else
                        {
                            grabzIt.HTMLToImage(inputData);
                        }
                    }
                    if (grabzIt.SaveTo(filename))
                    {
                        if (formatType == GIF)
                        {
                            Console.WriteLine("Animated GIF has been saved to: " + filename);
                        }
                        else if (formatType == PDF)
                        {
                            Console.WriteLine("PDF has been saved to: " + filename);
                        }
                        else if (formatType == DOCX)
                        {
                            Console.WriteLine("DOCX has been saved to: " + filename);
                        }
                        else
                        {
                            Console.WriteLine("Image has been saved to: " + filename);
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
            }
        }
    }
}
