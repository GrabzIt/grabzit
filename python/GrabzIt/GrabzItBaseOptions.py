#!/usr/bin/python

class GrabzItBaseOptions():
        """ Common options when creating a capture on GrabzIt

            Attributes:

            customId    A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
            country	    The country the capture should be created from: Default = "", UK = "UK", US = "US"
        """
        def __init__(self):
                self.customId = ""
                self.country = ""
                self.delay = 0

        def _createParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                return {"key":str(applicationKey), "country": str(self.country), "customid": str(self.customId), "callback": str(callBackURL), "sig": str(sig), str(dataName):str(dataValue)}