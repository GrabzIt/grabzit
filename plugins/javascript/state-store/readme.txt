GrabzIt Web Recorder Readme Version 1.1
=======================================

GrabzIt Web Recorder allows you to take screenshots of HTML elements after they have been altered by the user or other means.

An example of the GrabzIt Web Recorder in action can be found in the demo.html file. Remember to replace the "YOUR APPLICATION KEY" with your actual
application key found here: https://grabz.it/api/

WARNING!! This plugin can not capture all the changes made to a page you must choose a small part of the page were you wish to capture changes the user
has made.

GrabzIt Web Recorder provides the following methods:

- AppendURL([url to append changes to])
- AddTo([application key], [element | element id], [options])
- Create([application key], [url to take a screenshot of], [options])

The Append method returns the current URL with a querystring parameter that represents the changes made to the web page.

GrabzItWebRecorder.AppendURL();

If you don't want the querystring parameter addded to the currrent URL you can pass the URL you would like the querystring parameter appended to.

GrabzItWebRecorder.AppendURL('http://www.example.com');

The AddTo method is very flexible and can accept an element id or element so both of these
calls below will work.


	<html>
	<body>
	<div id="screenshot"></div>
	<script>
	//Call One
	GrabzItWebRecorder.AddTo('YOUR APPLICATION KEY', 'screenshot');

	//Call Two
	var elem = document.getElementById('screenshot');
	GrabzItWebRecorder.AddTo('YOUR APPLICATION KEY', elem);
	</script>
	</body>
	</html>


The options parameter takes all of the parameters found here (excluding the URL and application key parameters): https://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 256 x 200 you could do this like so:


	GrabzItWebRecorder.AddTo('YOUR APPLICATION KEY', 'div_id', {"width": 250, "height": 200});
	
If you need to test to see if the target webpage is currently being accessed by GrabzIt's screenshot servers then use the isRecording method. This could be useful to disable unwanted page features etc.

	if (GrabzItWebRecorder.isRecording())
	{
		//disable something that shouldn't appear when a screenshot is taken.
	}