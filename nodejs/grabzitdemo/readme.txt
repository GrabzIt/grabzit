GrabzIt 3.0
===========

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ As this example uses a callback handler it must be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To run the sample

Open config.js and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api/
the third parameter is the location of the handler on your website.


config.applicationKey=APPLICATION KEY
config.applicationSecret=APPLICATION SECRET
config.callbackHandlerUrl=http://www.example.com/grabzit/handler

Ensure your application has read and write access to the "/public/results" directory.

Finally run the sample web application to start converting web pages into screenshots.

More documentation can be found at: http://grabz.it/api/nodejs/