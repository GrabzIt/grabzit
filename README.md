Convert the Web!
================
GrabzIt aspires to allow any programming language no matter how basic to turn a website or HTML into a fully rendered Image, video, PDF, DOCX. As well as converting HTML tables into JSON, XML, CSV or Excel and online videos into animated GIF's.

To do this we provide client libraries that enables a developer to create a capture, once complete, our server then sends a callback to your app to allow it to be processed.

While we currently support [ASP.NET](https://grabz.it/api/aspnet/), [Java](https://grabz.it/api/java/), [Javascript](https://grabz.it/api/javascript/), [Node.js](https://grabz.it/api/nodejs/), [PHP](https://grabz.it/api/php/), [Perl](https://grabz.it/api/perl/), [Python](https://grabz.it/api/python/) and [Ruby](https://grabz.it/api/ruby/) we aim to create a library for as many programming languages as possible. If you would like to help us achieve this please do!

Example
-------

We have made the programming libraries as simple as possible to use. To create a capture you must first call a method that specifies what you want to capture such as the URLToImage method followed by the Save method. You will need a application key and application secret, but you can get these for free from [https://grabz.it](https://grabz.it).

```php
include("GrabzItClient.php");

//Create the GrabzItClient class
//Replace "APPLICATION KEY", "APPLICATION SECRET" with the values from your account!
$grabzIt = new \GrabzIt\GrabzItClient("APPLICATION KEY", "APPLICATION SECRET");
//Take the picture the method will return the unique identifier assigned to this task
$grabzIt->URLToImage("http://www.google.com");
$id = $grabzIt->Save("http://www.example.com/handler.php");
```

Notice that in the above example the location of the handler.php is defined this is used to handle the callback when the capture is complete.

The handler that accepts the callback and saves the image is below.

```php
include("GrabzItClient.php");

$message = $_GET["message"];
$customId = $_GET["customid"];
$id = $_GET["id"];
$filename = $_GET["filename"];
$format = $_GET["format"];

//Custom id can be used to store user ids or whatever is needed for the later processing of the
//resulting screenshot

$grabzIt = new \GrabzIt\GrabzItClient("APPLICATION KEY", "APPLICATION SECRET");
$result = $grabzIt->GetResult($id);

if (!$result)
{
	return;
}

//Ensure that the application has the correct rights for this directory.
file_put_contents("results" . DIRECTORY_SEPARATOR . $filename, $result);
```

And that's it! Your website should now be taking screenshots!

Documentation
-------------

For further more in depth documentation go to [https://grabz.it/api](https://grabz.it/api).
