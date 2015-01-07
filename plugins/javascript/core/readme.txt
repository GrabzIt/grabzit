GrabzIt Core Readme Version 1.00
================================

GrabzIt Core allows you to dynamically add a screenshot to any HTML element.

An example of the GrabzIt Core in action can be found in the demo.html file. Remember to replace the "Your Application Key" with your actual
application key found here: http://grabz.it/api/

GrabzIt Core provides to methods:

- AddTo([element | element id], [url to take a screenshot of], [options])
- Create([url to take a screenshot of], [options])

The options parameter takes all of the parameters found here (excluding the URL and application key parameters): http://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 256 x 200 you could do this like so:

GrabzIt("YOUR APPLICATION KEY").AddTo('div_id', 'http://www.google.com', {"width": 250, "height": 200});