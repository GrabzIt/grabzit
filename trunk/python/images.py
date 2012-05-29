import os
import cgi
import cgitb
import glob

cgitb.enable()

print "Status: 200 OK"
print "Content-Type: text/html;charset=utf-8"
print
print "<html>"
print "<head>"
print "</head>"
print "<body>"
print "<div style='width:800px;'>"

for infile in glob.glob("images" + os.sep + "*.*"):
    if ".txt" in infile:
    	continue
    	
    print "<img src='"+infile+"' style='margin-right:1em;'/>";
    
print "</div>"
print "</body>"
print "<html>"