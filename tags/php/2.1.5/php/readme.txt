GrabzIt 2.1
===========

This software allows you to programmatically convert webpages into screenshots, PDF's and CSV's and spreadsheets.

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