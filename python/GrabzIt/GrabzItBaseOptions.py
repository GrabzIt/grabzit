#!/usr/bin/python

try:
	from urllib.parse import urlencode
except ImportError:
	from urllib import urlencode

class GrabzItBaseOptions():
        """ Common options when creating a capture on GrabzIt

            Attributes:

            customId        A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
            country         The country the capture should be created from: Default = "", Singapore = "SG", UK = "UK", US = "US"
            exportURL       The export URL that should be used to transfer the capture to a third party location
            encryptionKey   The encryption key that will be used to encrypt your capture
        """
        def __init__(self):
                self.customId = ""
                self.country = ""
                self.exportURL = ""
                self.encryptionKey = ""
                self.delay = 0
                self.post = ""
                self.proxy = ""

        def _appendParameter(self, qs, name, value):
            val = ""
            if (name != None and name != ""):
                if (value == None):
                    value = ""
                val = urlencode({name:value})
        
            if (val == ""):
                return qs
                
            if (qs != ""):
                qs += "&"
                
            qs += val
            return qs
                
        def _createParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
            return {"key":str(applicationKey), "country": str(self.country), "export": str(self.exportURL), "encryption": str(self.encryptionKey), "proxy": str(self.proxy), "customid": str(self.customId), "callback": str(callBackURL), "sig": str(sig), str(dataName):str(dataValue)}