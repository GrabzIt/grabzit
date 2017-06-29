#!/usr/bin/python

class GrabzItBaseOptions():
        """ Common options when creating a capture on GrabzIt

            Attributes:

            customId    A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
            country	    The country the capture should be created from: Default = "", Singapore = "SG", UK = "UK", US = "US"
            exportURL   The export URL that should be used to transfer the capture to a third party location
        """
        def __init__(self):
                self.customId = ""
                self.country = ""
                self.exportURL = ""
                self.delay = 0

        def _createParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                return {"key":str(applicationKey), "country": str(self.country), "export": str(self.exportURL), "customid": str(self.customId), "callback": str(callBackURL), "sig": str(sig), str(dataName):str(dataValue)}