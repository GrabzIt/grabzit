import cgi
import cgitb
cgitb.enable()

import GrabzItClient
from ConfigParser import SafeConfigParser

form = cgi.FieldStorage()

parser = SafeConfigParser()
parser.read("GrabzIt.ini")

grabzIt = GrabzItClient.GrabzItClient(parser.get("GrabzIt", "applicationKey"), parser.get("GrabzIt", "applicationSecret"))
grabzIt.TakePicture(form.getvalue("url"), parser.get("GrabzIt", "handlerUrl"))

print 'Location: index.html'
print
