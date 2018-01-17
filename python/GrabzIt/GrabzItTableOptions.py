#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItTableOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a Table capture

            Attributes:

            tableNumberToInclude    the index of the table to be converted, were all tables in a web page are ordered from the top of the web page to bottom
            format                  the format the table should be in: csv, xlsx or json
            includeHeaderNames      set to true to include header names into the table
            includeAllTables        set to true to extract every table on the web page into a separate spreadsheet sheet. Only available with the XLSX and JSON formats
            targetElement           the id of the only HTML element in the web page that should be used to extract tables from
            requestAs               which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
        """
        
        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.tableNumberToInclude = 1
                self.format = 'csv'
                self.includeHeaderNames = True
                self.includeAllTables = False
                self.targetElement = ''
                self.requestAs = 0
                
        #
        # Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
        # GrabzIt to perform a HTTP post.
        #
        # name - The name of the HTTP Post parameter
        # value - The value of the HTTP Post parameter
        #               
        def AddPostParameter(self, name, value):
                self.post = self._appendParameter(self.post, name, value)
                
        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["includeAllTables"] = int(self.includeAllTables)
                params["includeHeaderNames"] = int(self.includeHeaderNames)
                params["format"] = str(self.format) 
                params["tableToInclude"] = int(self.tableNumberToInclude) 
                params["target"] = str(self.targetElement) 
                params["requestmobileversion"] = str(self.requestAs)            
                params["post"] = str(self.post)             

                return params

        def _getSignatureString(self, applicationSecret, callBackURL, url = ''):
                urlParam = '';
                if (url != None and url != ''):
                        urlParam = str(url)+"|"

                callBackURLParam = '';
                if (callBackURL != None and callBackURL != ''):
                        callBackURLParam = str(callBackURL)

                return applicationSecret +"|"+ urlParam + callBackURLParam + \
                "|"+str(self.customId)+"|"+str(int(self.tableNumberToInclude))+"|"+str(int(self.includeAllTables))+"|"+str(int(self.includeHeaderNames))+"|"+str(self.targetElement)+ \
                "|"+str(self.format)+"|"+str(int(self.requestAs))+"|"+str(self.country)+"|"+str(self.exportURL)+"|"+str(self.encryptionKey)+"|"+str(self.post)+"|"+str(self.proxy)
                