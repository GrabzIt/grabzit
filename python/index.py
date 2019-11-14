#!/usr/bin/python

import os
import cgi
import cgitb
import glob
import uuid
import re
try:
	import configparser
except ImportError:
	import ConfigParser as configparser

cgitb.enable()

from GrabzIt import GrabzItClient
from GrabzIt import GrabzItPDFOptions
from GrabzIt import GrabzItDOCXOptions
from GrabzIt import GrabzItImageOptions
from GrabzIt import GrabzItAnimationOptions
from GrabzIt import GrabzItTableOptions
try:
	from configparser import SafeConfigParser
except ImportError:
	from ConfigParser import SafeConfigParser

parser = SafeConfigParser()
parser.read(os.path.dirname(os.path.abspath(__file__)) + os.path.sep + "config.ini")

def useCallbackHandler():
	regex = re.compile(r'^(?:http|ftp)s?://' # http:// or https://
	r'(?:(?:[A-Z0-9](?:[A-Z0-9-]{0,61}[A-Z0-9])?\.)+(?:[A-Z]{2,6}\.?|[A-Z0-9-]{2,}\.?)|' # domain...
	r'localhost|' # localhost...
	r'\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}|' # ...or ipv4
	r'\[?[A-F0-9]*:[A-F0-9:]+\]?)' # ...or ipv6
	r'(?::\d+)?' # optional port
	r'(?:/?|[/?]\S+)$', re.IGNORECASE)
	if not(regex.match(parser.get("GrabzIt", "handlerUrl"))):
		return False
	return cgi.escape(os.environ["REMOTE_ADDR"]) != "::1" and cgi.escape(os.environ["REMOTE_ADDR"]) != "127.0.0.1"

message = ""
if os.environ['REQUEST_METHOD'] == 'POST':
		form = cgi.FieldStorage()
		if form.getvalue("delete") == "1":
			r = glob.glob(os.path.dirname(os.path.abspath(__file__)) + os.path.sep + 'results/*')
			for i in r:
				os.remove(i)
		else:
			try:
				grabzIt = GrabzItClient.GrabzItClient(parser.get("GrabzIt", "applicationKey"), parser.get("GrabzIt", "applicationSecret"))
				isHtml = form.getvalue("convert") == "html"
				
				if form.getvalue("format") == "pdf":
					if (isHtml == True):
							grabzIt.HTMLToPDF(form.getvalue("html"))
					else:
							grabzIt.URLToPDF(form.getvalue("url"))
				elif form.getvalue("format") == "docx":
					if (isHtml == True):
							grabzIt.HTMLToDOCX(form.getvalue("html"))
					else:
							grabzIt.URLToDOCX(form.getvalue("url"))							   
				elif form.getvalue("format") == "gif":
					grabzIt.URLToAnimation(form.getvalue("url"))
				else:
					if (isHtml == True):
							grabzIt.HTMLToImage(form.getvalue("html"))
					else:
							grabzIt.URLToImage(form.getvalue("url"))
				if useCallbackHandler():
					grabzIt.Save(parser.get("GrabzIt", "handlerUrl"))
				else:
					grabzIt.SaveTo(os.path.dirname(os.path.abspath(__file__)) + os.path.sep + "results" + os.sep + str(uuid.uuid4()) + "." + form.getvalue("format"))
			except Exception as e:
				message = str(e)

print ("Content-type: text/html\n\n")
print ('''<html>
<head>
<title>GrabzIt Demo</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<link rel="stylesheet" type="text/css" href="css/style.css">
<script src="ajax/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
<script>
var ui = new UI('ajax/results.py?r=', 'css');
</script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.py" class="inputForms">
<p><span id="spnIntro">Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="https://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>''')

if not(useCallbackHandler()):
	print('''<p>Either you have not updated the handlerUrl variable found in config.ini file to match the URL of the handler.py file found in this demo app or you are using this demo application on your local machine.</p><p>This demo will still work although it will create captures synchronously, which will cause the web page to freeze when captures are generated. <u>Please wait for the capture to complete</u>.</p>''')

if os.environ['REQUEST_METHOD'] == 'POST' and form.getvalue("delete") != "1":
	if message != '':
		print ('<p><span class="error">')
		print (message)
		print ('</span></p>')
	elif useCallbackHandler():
		print ('<p><span style="color:green;font-weight:bold;">Processing...</span></p>')
		
print ('''<div class="Row" id="divConvert">
<label>Convert </label><select name="convert" onchange="ui.selectConvertChanged(this)">
  <option value="url">URL</option>
  <option value="html">HTML</option>
</select>
</div>
<div id="divHTML" class="Row hidden">
<label>HTML </label><textarea name="html"><html><body><h1>Hello world!</h1></body></html></textarea>
</div>
<div id="divURL" class="Row">
<label>URL </label><input text="input" name="url" placeholder="http://www.example.com"/>
</div>
<div class="Row">
<label>Format </label><select name="format" onchange="ui.selectChanged(this)">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
  <option value="docx">DOCX</option>  
  <option value="gif">GIF</option>
  <option value="csv">CSV</option>
</select>
</div>
<input type="submit" value="Grabz It" style="margin-left:12em"></input>
</form>
<form method="post" action="index.py" class="inputForms">
<input type="hidden" name="delete" value="1"></input>
<input type="submit" value="Clear Results"></input>
</form>
	<br />
	<h2>Completed Screenshots</h2>
	<div id="divResults"></div>
</body>
</html>''')
