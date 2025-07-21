#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItHTMLOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating rendered HTML

            Attributes:

            browserWidth        the width of the browser in pixels
            browserHeight       the height of the browser in pixels. Use -1 to screenshot the whole web page
            delay               the number of milliseconds to wait before creating the capture
            waitForElement      the CSS selector of the HTML element in the web page that must be visible before the capture is performed
            requestAs           the user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
            noAds               set to true if adverts should be automatically hidden
            address                 the URL to execute the HTML code in
            noCookieNotifications   set to true if cookie notifications should be automatically hidden
            clickElement            the CSS selector of the HTML element in the web page to click
            jsCode                  the JavaScript code to execute in the web page            
        """

        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.browserWidth = 0
                self.browserHeight = 0
                self.waitForElement = ''
                self.requestAs = 0
                self.noAds = False
                self.noCookieNotifications = False
                self.address = ''
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
        
        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["bwidth"] = int(self.browserWidth)
                params["bheight"] = int(self.browserHeight)
                params["delay"] = int(self.delay)
                params["waitfor"] = str(self.waitForElement)                
                params["requestmobileversion"] = int(self.requestAs)
                params['noads'] = int(self.noAds)
                params["post"] = str(self.post)
                params["address"] = str(self.address)
                params["nonotify"] = int(self.noCookieNotifications)
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
                "|"+str(int(self.browserHeight))+"|"+str(int(self.browserWidth))+"|"+str(self.customId)+ \
                "|"+str(int(self.delay))+"|"+str(int(self.requestAs))+"|"+str(self.country)+"|"+str(self.exportURL)+ \
                "|"+str(self.waitForElement)+"|"+str(self.encryptionKey)+"|"+str(int(self.noAds))+"|"+str(self.post)+ \
                "|"+str(self.proxy)+"|"+str(self.address)+"|"+str(int(self.noCookieNotifications))+ \
                "|"+str(self.clickElement)+"|"+str(self.jsCode)
                