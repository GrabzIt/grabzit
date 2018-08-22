GrabzIt 3.3
===========

To run the demo

Open config.js and change the application key and application secret parameters to match what is found here: https://grabz.it/api/

If you are not on your local machine you can optionally change the config.callbackHandlerUrl to match it's publicly accessible location. Otherwise leave it as it is and the capture will be downloaded synchronously.

config.applicationKey=APPLICATION KEY
config.applicationSecret=APPLICATION SECRET
config.callbackHandlerUrl=http://www.example.com/grabzit/handler

Ensure your application has read and write access to the "/public/results" directory.

Finally run the demo web application to start converting web pages into screenshots.

More documentation can be found at: https://grabz.it/api/nodejs/