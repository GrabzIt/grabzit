GrabzIt

This software allows you to programmatically take screenshots of websites.

Its usually best to place these files in thier own directory.

To run the sample

Open GrabzItConfig.php and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api
the third parameter is the location you have placed the GrabzItHandler.php on your website.


$grabzItApplicationKey = "APPLICATION KEY";
$grabzItApplicationSecret = "APPLICATION SECRET";
$grabzItHandlerUrl = "http://www.example.com/grabzit/GrabzItHandler.php";

Ensure your application has read and write access to the "images" directory.

Finally run the index.php to start taking screenshots.

More documentation can be found at: http://grabz.it/api/php