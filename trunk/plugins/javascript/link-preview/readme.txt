GrabzIt Preview Readme Version 1.00
===================================

An example of the GrabzIt Preview in action can be found in the demo.html file. Remember to replace the "Your Application Key" with your actual
application key found here: http://grabz.it/api/

To add the preview ability to a link simply add the "grabzit-preview" class to the link. This will then be read automatically and a preview generated.

You do not make any other configurations than that, however you can specify all of the general, image and animated GIF parameters found here: 
http://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 256 x 200 you could do 
this like so:

new GrabzItPreview("YOUR APPLICATION KEY", {"width": 250, "height": 200});