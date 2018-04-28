GrabzIt 3.2
===========
This library allows you to programmatically convert web pages and HTML into images, DOCX documents, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's.

To run the demo

Open config.ini and change the application key and application secret parameters to match what is found here: https://grabz.it/api/

If you are not on your local machine you can optionally change the handlerUrl to match the publicly accessible location of the handler.py file. Otherwise leave it as it is and the capture will be downloaded synchronously.
applicationKey=APPLICATION KEY
applicationSecret=APPLICATION SECRET
handlerUrl=http://www.example.com/grabzit/handler.py

Ensure your application has read and write access to the "results" directory.
Finally run the index.py to start taking screenshots.
More documentation can be found at: https://grabz.it/api/python/