GrabzIt 3.2
===========

This library allows you to dynamically convert HTML or a URL to a image, DOCX document or PDF, a online video to a animated gif or a HTML table into a CSV, JSON or excel spreadsheet. The resulting capture can then be added to anywhere within the webpage or returned as a data URI.

An example of the GrabzIt in action can be found in the demo.html and demoDataURI.html files. Remember to replace the "Your Application Key" with your actual application key found here: https://grabz.it/api/

GrabzIt JavaScript library provides four methods:

- ConvertURL([url to capture], [options])
- ConvertHTML([html to convert], [options])
- UseSSL()
- Encrypt()
- AddPostVariable(name, value)
- AddTemplateVariable(name, value)
- AddTo([element | element id])
- Create()
- DataURI([callback], [decrypt])

The ConvertURL or ConvertHTML method must be called and then either the AddTo, Create or DataURI method. The AddTo method must specify the id of a element or the element were the capture should be displayed, so the first two calls below will work. The Create method just creates the capture on the body tag or the root element of the HTML document if the body tag doesn't exist. An example of this is shown in calls three and four below. 


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

The DataURI method requires a callback function this function must have one parameter that the dataUri will be returned in. In both of the examples below the capture is created and then the function is called which appends the dataURI to the datauri div.

	<html>
	<body>
	<div id="datauri" style="width:100%;word-break:break-all"></div>
	<script>
	function callback(dataUri)
	{
		document.getElementById('datauri').innerHTML += dataUri;
	}	
	
	//Call One
	GrabzIt("YOUR APPLICATION KEY").ConvertURL('http://www.google.com').DataURI(callback);

	//Call Two
	GrabzIt("YOUR APPLICATION KEY").ConvertHTML('<h1>Hello World</h1>').DataURI(callback);
	</script>
	</body>
	</html>	

The Encrypt method automatically makes a cryptographically secure key and passes it to GrabzIt's API to encrypt your capture. To access encrypted captures use the DataURI method, if a true is passed to the decrypt parameter of the DataURI method any encrypted data will be automatically decrypted using the key created in the Encrypt method.

The options parameter takes all of the parameters found here (excluding the URL and application key parameters): https://grabz.it/api/javascript/parameters.aspx in the options object. For instance if you wanted to set the width and height to be 250 x 200 you could do this like so:


	GrabzIt("YOUR APPLICATION KEY").ConvertURL('http://www.google.com', {"width": 250, "height": 200}).AddTo('div_id');
