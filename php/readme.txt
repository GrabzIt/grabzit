GrabzIt 3.0
===========

This library allows you to programmatically convert HTML and URL's into images, PDF's, CSV's, spreadsheets and JSON. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these package files in their own directory.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ As this example uses a callback handler it must be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To run the sample

Open config.php and change the three parameters to match your settings the first two parameters can be got from https://grabz.it/api/
the third parameter is the location you have placed the GrabzItHandler.php on your website.


$grabzItApplicationKey = "APPLICATION KEY";
$grabzItApplicationSecret = "APPLICATION SECRET";
$grabzItHandlerUrl = "http://www.example.com/grabzit/handler.php";

Ensure your application has read and write access to the "results" directory.

Finally run the index.php to start converting web pages.

More documentation can be found at: https://grabz.it/api/php/