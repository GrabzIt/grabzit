#!/usr/bin/python

import os
import cgi
import cgitb
import glob
import configparser

cgitb.enable()

from GrabzIt import GrabzItClient
from configparser import SafeConfigParser

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
                if form.getvalue("format") == "pdf":
                    grabzIt.SetPDFOptions(form.getvalue("url"))
                elif form.getvalue("format") == "gif":
                    grabzIt.SetAnimationOptions(form.getvalue("url"))
                else:
                    grabzIt.SetImageOptions(form.getvalue("url"))
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
<p><span id="spnScreenshot">Enter the URL of the website you want to take a screenshot of. The resulting screenshot</span><span class="hidden" id="spnGif">Enter the URL of the online video you want to convert into a animated GIF. The resulting animated GIF</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>''')

if os.environ['REQUEST_METHOD'] == 'POST' and form.getvalue("delete") != "1":
    if message != '':
        print ('<p><span class="error">')
        print (message)
        print ('</span></p>')
    else:
        print ('<p><span style="color:green;font-weight:bold;">Processing...</span></p>')
        
print ('''<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/> <select name="format" onchange="selectChanged(this)">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
  <option value="gif">GIF</option>
</select>
<input type="submit" value="Grabz It"></input>
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
