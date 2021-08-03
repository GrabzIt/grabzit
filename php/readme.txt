GrabzIt 3.5
===========

This library allows you to programmatically convert HTML and URL's into images, DOCX documents, rendered HTML, PDF's, CSV's, spreadsheets and JSON. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these package files in their own directory.

To run the demo

Ensure the MBString extension is installed.

Open config.php and change the application key and application secret parameters to match what is found here: https://grabz.it/api/

If you are not on your local machine you can optionally change the handlerUrl to match the publicly accessible location of the GrabzItHandler.php file. Otherwise leave it as it is and the capture will be downloaded synchronously.

$grabzItApplicationKey = "APPLICATION KEY";
$grabzItApplicationSecret = "APPLICATION SECRET";
$grabzItHandlerUrl = "http://www.example.com/grabzit/handler.php";

Ensure your application has read and write access to the "results" directory.

Finally run the index.php to start converting web pages.

More documentation can be found at: https://grabz.it/api/php/