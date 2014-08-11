GrabzIt 2.2
===========

This library allows you to programmatically convert web pages into images, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's.

To run the sample

Open config.ini and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api

The third parameter is the location you have placed the handler.py on your website.

applicationKey=APPLICATION KEY
applicationSecret=APPLICATION SECRET
handlerUrl=http://www.example.com/grabzit/handler.py

Ensure your application has read and write access to the "results" directory.

Finally run the index.py to start taking screenshots.

More documentation can be found at: http://grabz.it/api/python