GrabzIt 3.0
===========

This library allows you to programmatically convert web pages or HTML into images, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these files in their own directory.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ As this example uses a callback handler it must be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

To run the sample

First of all navigate to GrabzItDemo and in the command line run "bundle install"

Open GrabzItDemo\config\config.yml then change YOUR APPLICATION KEY and YOUR APPLICATION SECRET to match the your application key and secret that can be retrieved from https://grabz.it/api/

Change the third parameter "URL OF YOUR GrabzIt Handler FILE (http://www.example.com/handler/index)" to match the location you have placed the handler/index at on your website so: http://www.example.com/handler/index

Ensure your application has read and write access to the "GrabzItDemo\public\screenshots" directory.

Finally run the GrabzItDemo to start taking screenshots.

More documentation can be found at: https://grabz.it/api/ruby/