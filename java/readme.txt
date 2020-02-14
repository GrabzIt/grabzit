GrabzIt 3.4
===========

This library allows you to programmatically convert HTML and URL's into images, DOCX documents, PDF's, rendered HTML, CSV's, spreadsheets and JSON. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these package files in their own directory.

To run the demo

Open config.properties and change the application key and application secret parameters to match what is found here: https://grabz.it/api/

If you are not on your local machine you can optionally change the HandlerUrl to match it's publicly accessible location. Otherwise leave it as it is and the capture will be downloaded synchronously.

ApplicationKey=APPLICATION KEY
ApplicationSecret=APPLICATION SECRET
HandlerUrl=http://www.example.com/grabzit/handler

Ensure your application has read and write access to the "results" directory.

Finally run the demo web application to start converting web pages into screenshots.

More documentation can be found at: https://grabz.it/api/java/

Encryption
----------

In order to use encrypted captures with Java 6, 7 and 8 please install the Java Cryptography Extension (JCE) Unlimited Strength Jurisdiction Policy Files into all of the /jre/lib/security/ folders of the Java installation folders.
