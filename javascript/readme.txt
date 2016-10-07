GrabzIt 3.00
============

This library allows you to dynamically add a screenshot to any HTML element or webpage.

An example of the GrabzIt in action can be found in the demo.html file. Remember to replace the "Your Application Key" with your actual
application key found here: http://grabz.it/api/

GrabzIt JavaScript library provides four methods:

- ConvertURL([url to capture], [options])
- ConvertHTML([html to convert], [options])
- AddTo([element | element id])
- Create()

The ConvertURL or ConvertHTML must be called and then the AddTo or Create method must be called. The AddTo method must specify the id of a element or the element 
were the capture should be displayed, so the first two calls below will work. The Create method just creates the capture on the body tag or the root element of 
the HTML document if the body tag doesn't exist. An example of this is shown in calls three and four below. 


	<html>
	<body>
	<div id="screenshot"></div>
	<script>
	//Call One
	GrabzIt("YOUR APPLICATION KEY").ConvertURL('http://www.google.com').AddTo('screenshot');

	//Call Two
	var elem = document.getElementById('screenshot');
	GrabzIt("YOUR APPLICATION KEY").ConvertHTML('<h1>Hello World</h1>').AddTo(elem);
	
	//Call Three
	GrabzIt("YOUR APPLICATION KEY").ConvertURL('http://www.google.com').Create();
	
	//Call Four
	GrabzIt("YOUR APPLICATION KEY").ConvertHTML('<h1>Hello World</h1>').Create();	
	</script>
	</body>
	</html>


The options parameter takes all of the parameters found here (excluding the URL and application key parameters): http://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 256 x 200 you could do this like so:


	GrabzIt("YOUR APPLICATION KEY").ConvertURL('http://www.google.com', {"width": 250, "height": 200}).AddTo('div_id');