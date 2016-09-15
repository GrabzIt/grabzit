#!/usr/bin/python

import os
import cgi
import cgitb
import glob
import json

cgitb.enable()

print ("Content-Type: application/json\n\n")

results = []

for infile in glob.glob(".."  + os.sep + "results" + os.sep + "*.*"):
    if ".txt" in infile:
        continue
    
    results.append(infile.replace("." + os.sep, "").replace(os.sep, "/"));
    
print (json.dumps(results))
