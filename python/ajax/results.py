#!/usr/bin/python

import os
import cgi
import cgitb
import glob
import json

cgitb.enable()

print ("Content-Type: application/json\n\n")

results = []
path = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', "results") + os.sep

for infile in glob.glob(path + "*.*"):
    if ".txt" in infile:
        continue
    
    results.append('results/' + os.path.basename(infile));
    
print (json.dumps(results))
