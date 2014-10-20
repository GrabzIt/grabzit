GrabzIt 2.2
===========

This library allows you to programmatically convert web pages into images, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's.

Its usually best to place these package files in their own directory.

To run the sample

Open config.php and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api
the third parameter is the location you have placed the GrabzItHandler.php on your website.


$grabzItApplicationKey = "APPLICATION KEY";
$grabzItApplicationSecret = "APPLICATION SECRET";
$grabzItHandlerUrl = "http://www.example.com/grabzit/handler.php";

Ensure your application has read and write access to the "results" directory.

Finally run the index.php to start converting web pages.

More documentation can be found at: http://grabz.it/api/php