import os
import cgi
import GrabzItClient
from ConfigParser import SafeConfigParser

form = cgi.FieldStorage()

message = form.getvalue("message")
customId = form.getvalue("customid")
id = form.getvalue("id")
filename = form.getvalue("filename")

parser = SafeConfigParser()
parser.read('GrabzIt.ini')

#Custom id can be used to store user ids or whatever is needed for the later processing of the
#resulting screenshot

grabzIt = GrabzItClient.GrabzItClient(parser.get('GrabzIt', 'applicationKey'), parser.get('GrabzIt', 'applicationSecret'))
result = grabzIt.GetPicture(id)

if result != None:
	#Ensure that the application has the correct rights for this directory.
	fo = open("images" + os.sep + filename, "wb")
	fo.write(result)
	fo.close()
	
print "Status: 200 OK"
print