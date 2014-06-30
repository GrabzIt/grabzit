GrabzIt

This software allows you to programmatically take screenshots of websites.

Its usually best to place these files in thier own directory.

To run the sample

Open config.ini and change the three parameters to match your settings the first two parameters can be got from http://grabz.it/api
the third parameter is the location you have placed the handler.pl on your website.


applicationKey=APPLICATION KEY
applicationSecret=APPLICATION SECRET
handlerUrl=http://www.example.com/grabzit/handler.pl


Ensure your application has read and write access to the "results" directory.

Finally run the index.pl to start taking screenshots.

More documentation can be found at: http://grabz.it/api/perl