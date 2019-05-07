#!/usr/bin/python

import os
import cgi
import cgitb
try:
    from configparser import SafeConfigParser
except ImportError:
    from ConfigParser import SafeConfigParser
	
cgitb.enable()

from GrabzIt import GrabzItClient
try:
	from configparser import SafeConfigParser
except ImportError:
	from ConfigParser import SafeConfigParser

form = cgi.FieldStorage()

message = form.getvalue("message")
customId = form.getvalue("customid")
id = form.getvalue("id")
filename = form.getvalue("filename")
format = form.getvalue("format")
targeterror = form.getvalue("targeterror")

parser = SafeConfigParser()
parser.read('config.ini')

#Custom id can be used to store user ids or whatever is needed for the later processing of the
#resulting screenshot

grabzIt = GrabzItClient.GrabzItClient(parser.get('GrabzIt', 'applicationKey'), parser.get('GrabzIt', 'applicationSecret'))
result = grabzIt.GetResult(id)

if result != None:
	#Ensure that the application has the correct rights for this directory.
	fo = open("results" + os.sep + filename, "wb")
	fo.write(result)
	fo.close()
    
print ("Content-type: text/html\n\n")
