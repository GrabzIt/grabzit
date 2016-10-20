#!/usr/bin/python

import os
import cgi
import cgitb
import glob
try:
	import configparser
except ImportError:
	import ConfigParser as configparser

cgitb.enable()

from GrabzIt import GrabzItClient
try:
	from configparser import SafeConfigParser
except ImportError:
	from ConfigParser import SafeConfigParser

message = ""
if os.environ['REQUEST_METHOD'] == 'POST':
        form = cgi.FieldStorage()
        if form.getvalue("delete") == "1":
            r = glob.glob('results/*')
            for i in r:
                os.remove(i)
        else:
            try:
                parser = SafeConfigParser()
                parser.read("config.ini")
                grabzIt = GrabzItClient.GrabzItClient(parser.get("GrabzIt", "applicationKey"), parser.get("GrabzIt", "applicationSecret"))
                isHtml = form.getvalue("convert") == "html"
                
                if form.getvalue("format") == "pdf":
                    if (isHtml == True):
                            grabzIt.HTMLToPDF(form.getvalue("html"))
                    else:
                            grabzIt.URLToPDF(form.getvalue("url"))
                elif form.getvalue("format") == "gif":
                    grabzIt.URLToAnimation(form.getvalue("url"))
                else:
                    if (isHtml == True):
                            grabzIt.HTMLToImage(form.getvalue("html"))
                    else:
                            grabzIt.URLToImage(form.getvalue("url"))
                grabzIt.Save(parser.get("GrabzIt", "handlerUrl"))
            except Exception as e:
                message = str(e)

print ("Content-type: text/html\n\n")
print ('''<html>
<head>
<title>GrabzIt Demo</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<link rel="stylesheet" type="text/css" href="css/style.css">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.py" class="inputForms">
<p><span id="spnScreenshot">Enter the HTML or URL you want to convert into a PDF or Image. The resulting capture</span><span class="hidden" id="spnGif">Enter the URL of the online video you want to convert into a animated GIF. The resulting animated GIF</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>''')

if os.environ['REQUEST_METHOD'] == 'POST' and form.getvalue("delete") != "1":
    if message != '':
        print ('<p><span class="error">')
        print (message)
        print ('</span></p>')
    else:
        print ('<p><span style="color:green;font-weight:bold;">Processing...</span></p>')
        
print ('''<div class="Row" id="divConvert">
<label>Convert </label><select name="convert" onchange="selectConvertChanged(this)">
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
<label>Format </label><select name="format" onchange="selectChanged(this)">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
  <option value="gif">GIF</option>
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
