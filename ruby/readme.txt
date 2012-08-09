GrabzIt

This software allows you to programmatically take screenshots of websites.

Its usually best to place these files in thier own directory.

To run the sample

First of all navigate to GrabzItDemo and in the command line run "bundle install"

Open GrabzItDemo\app\controllers\home_controller.rb and GrabzItDemo\app\controllers\handler_controller.rb then change "YOUR APPLICATION KEY" and "YOUR APPLICATION SECRET" to match the first two parameters can be got from http://grabz.it/api

Change the third parameter "URL OF YOUR GrabzIt Handler FILE (http://www.example.com/handler/index)" to match the location you have placed the handler/index at on your website so :http://www.example.com/handler/index

Ensure your application has read and write access to the "GrabzItDemo\public\screenshots" directory.

Finally run the GrabzItDemo to start taking screenshots.

More documentation can be found at: http://grabz.it/api/ruby