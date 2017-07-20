#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItPDFOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a PDF capture

            Attributes:

            includeBackground       set to true if the background of the web page should be included in the PDF
            pagesize                the page size of the PDF to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
            orientation             the orientation of the PDF to be returned: 'Landscape' or 'Portrait'
            includeLinks            set to true if links should be included in the PDF
            includeOutline          set to true if the PDF outline should be included
            title                   the title for the PDF document
            coverURL                the URL of a web page that should be used as a cover page for the PDF
            marginTop               the margin that should appear at the top of the PDF document page
            marginLeft              the margin that should appear at the left of the PDF document page
            marginBottom            the margin that should appear at the bottom of the PDF document page
            marginRight             the margin that should appear at the right of the PDF document
            delay                   the number of milliseconds to wait before creating the capture
            targetElement           the CSS selector of the only HTML element in the web page to capture
            hideElement             the CSS selector(s) of the one or more HTML elements in the web page to hide
            waitForElement          the CSS selector of the HTML element in the web page that must be visible before the capture is performed
            requestAs               which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
            templateId              a PDF template ID that specifies the header and footer of the PDF document
            customWaterMarkId       a custom watermark to add to the PDF
            quality                 the quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
        """
        
        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.includeBackground = True
                self.pagesize = 'A4'
                self.orientation = 'Portrait'
                self.includeLinks = True
                self.includeOutline = False
                self.title = ''
                self.coverURL = ''
                self.marginTop = 10
                self.marginLeft = 10
                self.marginBottom = 10
                self.marginRight = 10
                self.requestAs = 0
                self.templateId = ''
                self.customWaterMarkId = ''
                self.quality = -1
                self.targetElement = ''
                self.hideElement = ''
                self.waitForElement = ''
                
        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["background"] = int(self.includeBackground)
                params["pagesize"] = str(self.pagesize.upper())
                params["orientation"] = str(self.orientation.title())
                params["includelinks"] = int(self.includeLinks)
                params["includeoutline"] = int(self.includeOutline)
                params["title"] = str(self.title)
                params["coverurl"] = str(self.coverURL) 
                params["mtop"] = int(self.marginTop)
                params["mleft"] = int(self.marginLeft)
                params["mbottom"] = int(self.marginBottom)
                params["mright"] = int(self.marginRight)
                params["delay"] = int(self.delay)
                params["templateid"] = str(self.templateId)
                params["requestmobileversion"] = int(self.requestAs)
                params["customwatermarkid"] = str(self.customWaterMarkId) 
                params["quality"] = int(self.quality)
                params["target"] = str(self.targetElement)
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
                "|"+str(self.customId)+"|"+str(int(self.includeBackground))+"|"+str(self.pagesize.upper()) +"|"+str(self.orientation.title())+"|"+str(self.customWaterMarkId)+ \
                "|"+str(int(self.includeLinks))+"|"+str(int(self.includeOutline))+"|"+str(self.title)+"|"+str(self.coverURL)+"|"+str(int(self.marginTop))+ \
                "|"+str(int(self.marginLeft))+"|"+str(int(self.marginBottom))+"|"+str(int(self.marginRight))+"|"+str(int(self.delay))+"|"+str(int(self.requestAs))+ \
                "|"+str(self.country)+"|"+str(int(self.quality))+"|"+str(self.templateId)+"|"+str(self.hideElement)+"|"+str(self.targetElement)+"|"+str(self.exportURL)+"|"+str(self.waitForElement)+"|"+str(self.encryptionKey)