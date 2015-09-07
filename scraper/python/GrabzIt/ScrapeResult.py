#!/usr/bin/python

import cgi
import cgitb
import json
import os
import xml.etree.ElementTree as ET

class ScrapeResult:

    def __init__(self, path = None):
        cgitb.enable()
        
        self.data = None
        self.extension = None
        self.filename = None
        self.file = None
        
        if path != None:
            fo = open(path)
            self.data = fo.read()
            fo.close()
            
            self.extension = os.path.splitext(path)[1][1:].lower()
            self.filename = os.path.basename(path)            
        elif "HTTP_USER_AGENT" not in os.environ or os.environ["HTTP_USER_AGENT"] != "GrabzIt":
            raise Exception("A call originating from a non-GrabzIt server has been detected")

    def __getFile(self):
        if self.file != None:
            return self.file
                
        form = cgi.FieldStorage()

        if "file" in form:
            self.file = form["file"]
                
        return self.file
    
    def toJSON(self):
        if self.getExtension() == 'json':
            return json.loads(self.toString())
        return None
    
    def toXML(self):
        if self.getExtension() == 'xml':
            return ET.ElementTree(ET.fromstring(self.toString()))
        return None    
    
    def toString(self):
        if self.data != None:
            return self.data
            
        obj = self.__getFile()
        if obj != None:
            self.data = obj.file.read()
            
        return self.data
            
    def __str__(self):
        return self.toString()

    def getFilename(self):
        if self.filename != None:
            return self.filename
            
        obj = self.__getFile()
        if obj != None:
            self.filename = obj.filename.split("\\")[-1]
            
        return self.filename

    def getExtension(self):
        if self.extension != None:
            return self.extension
            
        name = self.getFilename()
        if name != None:
            self.extension = name.split(".")[-1]
        
        return self.extension

    def save(self, path):
        try:
            fo = open(path, "wb")
            fo.write(self.toString())
            fo.close()
            return True
        except:
            return False