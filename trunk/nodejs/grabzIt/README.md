GrabzIt 2.2
===========

This library allows you to programmatically convert web pages into images, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's. For more help please read our [documentation](http://grabz.it/api/nodejs).

Its usually best to place these files in their own directory.

Before the package can be used you must [register](http://grabz.it/register.aspx) to get your application key and secret. Once you have this you can take a screenshot like so:

    var grabzit = require("grabzit");
    var client = new grabzit("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET");
    client.set_image_options("http://www.google.com");
    client.save(http://www.mysite.com/handler");

The handler then gets passed the following query string parameters:

- id
- customid
- message
- filename
- format

These query string parameters can be used to edit and save the screenshot, so in the handler something like this could be used:

    var grabzit = require("grabzit");
    var client = new grabzit("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET");
    client.get_result(id, function(err, result){
        file.writeFile('test.jpg', result);
    });