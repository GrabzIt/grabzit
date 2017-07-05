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

from GrabzIt import GrabzItScrapeClient

try:
	from configparser import SafeConfigParser
except ImportError:
	from ConfigParser import SafeConfigParser

message = ""
parser = SafeConfigParser()
parser.read("config.ini")
grabzIt = GrabzItScrapeClient.GrabzItScrapeClient(parser.get("GrabzIt", "applicationKey"), parser.get("GrabzIt", "applicationSecret"))

print ("Content-type: text/html\n\n")
print ('''<html>
<head>
<title>GrabzIt Scraper Demo</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</head>
<body>''')

form = cgi.FieldStorage()
if form.getvalue('status') != None and form.getvalue('id') != None:
	try:
		grabzIt.SetScrapeStatus(form.getvalue('id'), form.getvalue('status'))
	except Exception as e:
		message = str(e)
elif form.getvalue('resultId') != None and form.getvalue('id') != None:		   
	try:
		grabzIt.SendResult(form.getvalue('id'), form.getvalue('resultId'))
	except Exception as e:
		message = str(e)
		
if form.getvalue('id') != None:
	if message != '':
		print ('<p><span style="color:red;font-weight:bold;">')
		print (message)
		print ('</span></p>')
	else:
		print ('<p><span style="color:green;font-weight:bold;">Succesfully updated scrape</span></p>')
		
print ('<table><tr><th>Scrape Name</th><th>Scrape Status</th><th></th></tr>')
scrapes = grabzIt.GetScrapes()
for scrape in scrapes:
	print('<tr><td>')
	print(scrape.Name)
	print('</td><td>')
	print(scrape.Status)
	print('</td><td><a href="index.py?id=')
	print(scrape.ID)
	print('&status=Start">Start</a> <a href="index.py?id=')
	print(scrape.ID)
	print('&status=Stop">Stop</a> <a href="index.py?id=')
	print(scrape.ID)
	print('&status=Disable">Disable</a> <a href="index.py?id=')
	print(scrape.ID)
	print('&status=Enable">Enable</a>')
	if len(scrape.Results) > 0:
		print(' <a href="index.py?id=')
		print(scrape.ID)
		print('&resultId=')
		print(scrape.Results[0].ID)
		print('">Resend</a>')
	print('</td></tr>')
	
print('</table></body></html>')
