GrabzIt Preview Readme Version 1.00
===================================

An example of the GrabzIt Preview in action can be found in the demo.html file. Remember to replace the "Your Application Key" with your actual
application key found here: http://grabz.it/api/

Also ensure the call to:

new GrabzItPreview("YOUR APPLICATION KEY");

Is at the bottom of the page just below the body tag. 

To add the preview ability to a link simply add the "grabzit-preview" class to the link. This will then be read automatically and a preview generated.

You do not make any other configurations than that, however you can specify all of the general, image and animated GIF parameters found here: 
http://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 256 x 200 you could do 
this like so:

new GrabzItPreview("YOUR APPLICATION KEY", {"width": 250, "height": 200});

Feel free to alter the JavaScript or the CSS as you wish!

Installing GrabzIt Preview on a Blog or CMS
===========================================

GrabzIt Preview is full compatible with a blog or CMS, just open blog-or-cms-install.txt this includes all the required JavaScript and CSS in one block. This can be inserted into a widget that accepts raw HTML in most blog or Content Management Systems. Remember to locate it at the bottom of the page after the content.