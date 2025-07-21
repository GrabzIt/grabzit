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
            noAds                   set to true if adverts should be automatically hidden
            templateId              a template ID that specifies the header and footer of the DOCX document
            pageHeight              set the height of the resulting DOCX in mm
            pageWidth               set the width of the resulting DOCX in mm
            targetElement           the CSS selector of the only HTML element in the web page to capture
            browserWidth            the width of the browser in pixels
            mergeId                 the ID of a capture that should be merged at the beginning of the new DOCX document
            address                 the URL to execute the HTML code in
            noCookieNotifications   set to true if cookie notifications should be automatically hidden
            password                the password to protect the DOCX document with
            clickElement            the CSS selector of the HTML element in the web page to click
            jsCode                  the JavaScript code to execute in the web page
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
                self.noAds = False
                self.templateVariables = ''
                self.pageHeight = 0
                self.pageWidth = 0
                self.browserWidth = 0
                self.templateId = ''
                self.targetElement = ''
                self.mergeId = ''
                self.noCookieNotifications = False
                self.address = ''
                self.password = ''
                self.clickElement = ''
                self.jsCode = ''
                
        #
        # Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
        # GrabzIt to perform a HTTP post.
        #
        # name - The name of the HTTP Post parameter
        # value - The value of the HTTP Post parameter
        #               
        def AddPostParameter(self, name, value):
                self.post = self._appendParameter(self.post, name, value)
                
        #
        # Define a custom template parameter and value, this method can be called multiple times to add multiple parameters.
        #
        # name - The name of the template parameter
        # value - The value of the template parameter
        #               
        def AddTemplateParameter(self, name, value):
                self.templateVariables = self._appendParameter(self.templateVariables, name, value)
                
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
                params['noads'] = int(self.noAds)           
                params["post"] = str(self.post)
                params["bwidth"] = int(self.browserWidth)
                params["tvars"] = str(self.templateVariables)
                params["width"] = int(self.pageWidth)
                params["height"] = int(self.pageHeight)
                params["target"] = str(self.targetElement)
                params["templateid"] = str(self.templateId)
                params["mergeid"] = str(self.mergeId)
                params["address"] = str(self.address)
                params["nonotify"] = int(self.noCookieNotifications)
                params["password"] = str(self.password)
                params["click"] = str(self.clickElement)
                params["jscode"] = str(self.jsCode)
                
                return params
                
        def _getSignatureString(self, applicationSecret, callBackURL, url = ''):
                urlParam = ''
                if (url != None and url != ''):
                        urlParam = str(url)+"|"

                callBackURLParam = ''
                if (callBackURL != None and callBackURL != ''):
                        callBackURLParam = str(callBackURL)

                return applicationSecret +"|"+ urlParam + callBackURLParam + \
                "|"+str(self.customId)+"|"+str(int(self.includeBackground))+"|"+str(self.pagesize.upper()) +"|"+str(self.orientation.title())+"|"+str(int(self.includeImages))+ \
                "|"+str(int(self.includeLinks))+"|"+str(self.title)+"|"+str(int(self.marginTop))+ \
                "|"+str(int(self.marginLeft))+"|"+str(int(self.marginBottom))+"|"+str(int(self.marginRight))+"|"+str(int(self.delay))+"|"+str(int(self.requestAs))+ \
                "|"+str(self.country)+"|"+str(int(self.quality))+"|"+str(self.hideElement)+"|"+str(self.exportURL)+"|"+str(self.waitForElement)+\
                "|"+str(self.encryptionKey)+"|"+str(int(self.noAds))+"|"+str(self.post)+"|"+str(self.targetElement)+"|"+str(self.templateId)+"|"+\
                str(self.templateVariables)+"|"+str(int(self.pageHeight))+"|"+str(int(self.pageWidth))+"|"+str(int(self.browserWidth))+"|"+\
                str(self.proxy)+"|"+str(self.mergeId)+"|"+str(self.address)+"|"+str(int(self.noCookieNotifications))+"|"+str(self.password)+"|"+\
                str(self.clickElement)+"|"+str(self.jsCode)