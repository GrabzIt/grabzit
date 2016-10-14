GrabzIt 3.0
===========

This library allows you to programmatically convert HTML and URL's into images, PDF's, CSV's, spreadsheets and JSON. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these package files in their own directory.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ As this example uses a callback handler it must be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To run the sample

Open config.properties and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api/
the third parameter is the location of the handler (Handler.java in the sample application) on your website.


ApplicationKey=APPLICATION KEY
ApplicationSecret=APPLICATION SECRET
HandlerUrl=http://www.example.com/grabzit/handler

Ensure your application has read and write access to the "results" directory.

Finally run the sample web application to start converting web pages into screenshots.

More documentation can be found at: http://grabz.it/api/java/