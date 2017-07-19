GrabzIt 3.2
===========

This library allows you to programmatically convert HTML and URL's into images, DOCX documents, PDF's, CSV's, spreadsheets and JSON. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these package files in their own directory.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ As this example uses a callback handler it must be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To run the sample

Open config.properties and change the three parameters to match your settings the first two parameters can be got from https://grabz.it/api/
the third parameter is the location of the handler (Handler.java in the sample application) on your website.


ApplicationKey=APPLICATION KEY
ApplicationSecret=APPLICATION SECRET
HandlerUrl=http://www.example.com/grabzit/handler

Ensure your application has read and write access to the "results" directory.

Finally run the sample web application to start converting web pages into screenshots.

More documentation can be found at: https://grabz.it/api/java/

Encryption
----------

In order to use encrypted captures with Java 6, 7 and 8 please install the Java Cryptography Extension (JCE) Unlimited Strength Jurisdiction Policy Files into all of the /jre/lib/security/ folders of the Java installation folders.
