=== GrabzIt Web Capture ===
Contributors: grabzit
Tags: screenshot,html to pdf,html to image,html to docx,video to animated gif
Requires at least: 4.5.0
Tested up to: 6.6.2
Requires PHP: 5
Stable tag: trunk
License: GPL2
License URI: https://www.gnu.org/licenses/gpl-2.0.html

Use a simple shortcode to screenshot a webpage or convert any text or HTML snippet into images, PDF's, DOCX, GIF's, CSV, JSON, MP4 and more!

== Description ==
Use a simple shortcode to screenshot a webpage or convert any text or HTML snippet into images, PDF's, DOCX, Animated GIF's, CSV, JSON, MP4 and more! To get started: activate the GrabzIt Web Capture and go to the GrabzIt API page to get your Application Key.

== Frequently Asked Questions ==
To get started, you must first [authorize your domain](https://grabz.it/account/domains/) to ensure no one else can use your application key.

Then to create a capture you need to specify the grabzit tags around the content you wish to capure. For instance you could convert a URL into a screenshot or you could convert a HTML snippet into a image, as shown in the two examples below.

[grabzit]https://www.google.com[/grabzit]
[grabzit]&lt;h1&gt;Hello there&lt;/h1&gt;[/grabzit]

To further customize your captures just choose one of these [available options](https://grabz.it/api/javascript/parameters/) and then add the option as an attribute to the grabzit tag. In the below example the download attribute has been set to true and the format attribute has been set to PDF, which means a screenshot of google.com will be automatically downloaded as a PDF.

[grabzit format="pdf" download="1"]https://www.google.com[/grabzit]

If you have any questions please [ask our support team](https://grabz.it/contact/?subject=WordPress+Plugin+Support) or read about [what else](https://grabz.it/plugins/wordpress/) you can do with the GrabzIt plugin.

== Changelog ==
- Localized Plugin
- Added customizable HTTP proxies
- Added the ability to hide cookie notifications
- Added HD images
- Added CSS Media Types for PDF conversions
- Added the ability to click on HTML elements