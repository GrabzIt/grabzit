#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItDOCXOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a DOCX capture

            Attributes:

            includeBackground       set to true if the background images of the web page should be included in the DOCX
            pagesize                the page size of the DOCX to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
            orientation             the orientation of the DOCX to be returned: 'Landscape' or 'Portrait'
            includeLinks            set to true if links should be included in the DOCX
            includeImages           set to true if the images of the webpage should be included in the DOCX
            title                   the title for the DOCX document
            marginTop               the margin that should appear at the top of the DOCX document page
            marginLeft              the margin that should appear at the left of the DOCX document page
            marginBottom            the margin that should appear at the bottom of the DOCX document page
            marginRight             the margin that should appear at the right of the DOCX document
            delay                   the number of milliseconds to wait before creating the capture
            hideElement             the CSS selector(s) of the one or more HTML elements in the web page to hide
            waitForElement          the CSS selector of the HTML element in the web page that must be visible before the capture is performed
            requestAs               which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
            quality                 the quality of the DOCX where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
        """
        
        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.includeBackground = True
                self.pagesize = 'A4'
                self.orientation = 'Portrait'
                self.includeLinks = True
                self.includeImages = True
                self.title = ''
                self.marginTop = 10
                self.marginLeft = 10
                self.marginBottom = 10
                self.marginRight = 10
                self.requestAs = 0
                self.quality = -1
                self.hideElement = ''
                self.waitForElement = ''
                
        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["background"] = int(self.includeBackground)
                params["pagesize"] = str(self.pagesize.upper())
                params["orientation"] = str(self.orientation.title())
                params["includelinks"] = int(self.includeLinks)
                params["includeimages"] = int(self.includeImages)
                params["title"] = str(self.title)
                params["mtop"] = int(self.marginTop)
                params["mleft"] = int(self.marginLeft)
                params["mbottom"] = int(self.marginBottom)
                params["mright"] = int(self.marginRight)
                params["delay"] = int(self.delay)
                params["requestmobileversion"] = int(self.requestAs) 
                params["quality"] = int(self.quality)
                params["hide"] = str(self.hideElement)  
                params["waitfor"] = str(self.waitForElement)                    

                return params
                
        def _getSignatureString(self, applicationSecret, callBackURL, url = ''):
                urlParam = '';
                if (url != None and url != ''):
                        urlParam = str(url)+"|"

                callBackURLParam = '';
                if (callBackURL != None and callBackURL != ''):
                        callBackURLParam = str(callBackURL)

                return applicationSecret +"|"+ urlParam + callBackURLParam + \
                "|"+str(self.customId)+"|"+str(int(self.includeBackground))+"|"+str(self.pagesize.upper()) +"|"+str(self.orientation.title())+"|"+str(int(self.includeImages))+ \
                "|"+str(int(self.includeLinks))+"|"+str(self.title)+"|"+str(int(self.marginTop))+ \
                "|"+str(int(self.marginLeft))+"|"+str(int(self.marginBottom))+"|"+str(int(self.marginRight))+"|"+str(int(self.delay))+"|"+str(int(self.requestAs))+ \
                "|"+str(self.country)+"|"+str(int(self.quality))+"|"+str(self.hideElement)+"|"+str(self.exportURL)+"|"+str(self.waitForElement)
                +"|"+str(self.encryptionKey)