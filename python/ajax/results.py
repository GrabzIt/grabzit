#!/usr/bin/python

import os
import cgi
import cgitb
import glob
import json

cgitb.enable()

print "Status: 200 OK"
print "Content-Type: application/json;charset=utf-8"
print

results = []

for infile in glob.glob("../results" + os.sep + "*.*"):
    if ".txt" in infile:
    	continue
    
    results.append(infile.replace("../", ""));
    
print json.dumps(results);