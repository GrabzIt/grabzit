GrabzIt Save As PDF Readme Version 1.07
=======================================

An example of the GrabzIt Save As PDF Plugin in action can be found in the demo.html file. Remember to replace the "Your Application Key" with your actual application key found here: https://grabz.it/api/

Also ensure the call to:


	new GrabzItSaveAsPDF("YOUR APPLICATION KEY");


Is at the bottom of the page just above the closing body tag. 

To add the save as PDF ability to a link, image, button or other HTML element simply add the "grabzit-pdf-save" class to the element. This will then be read automatically and when the element is clicked a PDF of the webpage will be downloaded. 

You do not have to set any other configurations options than that, however you can specify all of the PDF parameters found here: 
https://grabz.it/api/javascript/parameters.aspx#pdf in the options object. For instance if you wanted to set the fliename to be mypage.pdf you could do this like so:


	new GrabzItSaveAsPDF("YOUR APPLICATION KEY", {"filename": "mypage.pdf"});


If you want to specify a URL other than the url of the webpage. You can specify one by using the grabzit-url attribute for instance: <a href="#" grabzit-url="http://www.google.com" class="grabzit-pdf-save">My Example</a>

Feel free to alter the JavaScript or the CSS as you wish!

Installing GrabzIt Preview on a Blog or CMS
===========================================

GrabzIt Save As PDF is full compatible with a blog or CMS, just open blog-or-cms-install.txt this includes all the required JavaScript and CSS in one block. This can be inserted into a widget that accepts raw HTML in most blog or Content Management Systems. Remember to locate it at the bottom of the page after the content.
