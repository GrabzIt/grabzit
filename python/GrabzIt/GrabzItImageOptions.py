#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItImageOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a image capture

            Attributes:

            width               the width of the resulting screenshot in pixels. Use -1 to not reduce the width of the screenshot
            height              the height of the resulting screenshot in pixels. Use -1 to not reduce the height of the screenshot
            browserWidth        the width of the browser in pixels
            browserHeight       the height of the browser in pixels. Use -1 to screenshot the whole web page
            format              the format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
            delay               the number of milliseconds to wait before creating the capture
            targetElement       the CSS selector of the only HTML element in the web page to capture
            hideElement         the CSS selector(s) of the one or more HTML elements in the web page to hide
            waitForElement      the CSS selector of the HTML element in the web page that must be visible before the capture is performed
            requestAs           the user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
            customWaterMarkId   set a custom watermark to add to the screenshot
            quality             set the quality of the screenshot where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
            transparent         set to true if the image capture should be transparent. This is only compatible with png and tiff images
            noAds               set to true if adverts should be automatically hidden
            hd                  set to true if the image capture should be in high definition
            address                 the URL to execute the HTML code in
            noCookieNotifications   set to true if cookie notifications should be automatically hidden
            clickElement            the CSS selector of the HTML element in the web page to click            
        """

        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.browserWidth = 0
                self.browserHeight = 0
                self.width = 0
                self.height = 0
                self.format = ''
                self.targetElement = ''
                self.hideElement = ''
                self.waitForElement = ''
                self.requestAs = 0
                self.customWaterMarkId = ''
                self.quality = -1
                self.transparent = False
                self.noAds = False
                self.hd = False
                self.noCookieNotifications = False
                self.address = ''
                self.clickElement = ''                
        
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
                params["width"] = int(self.width)
                params["height"] = int(self.height)
                params["bwidth"] = int(self.browserWidth)
                params["bheight"] = int(self.browserHeight)
                params["delay"] = int(self.delay)
                params["format"] = str(self.format)
                params["target"] = str(self.targetElement)
                params["hide"] = str(self.hideElement)
                params["waitfor"] = str(self.waitForElement)                
                params["requestmobileversion"] = int(self.requestAs)
                params["customwatermarkid"] = str(self.customWaterMarkId) 
                params["quality"] = int(self.quality)
                params["transparent"] = int(self.transparent)                
                params['noads'] = int(self.noAds)
                params['hd'] = int(self.hd)
                params["post"] = str(self.post)
                params["address"] = str(self.address)
                params["nonotify"] = int(self.noCookieNotifications)
                params["click"] = str(self.clickElement)
                
                return params

        def _getSignatureString(self, applicationSecret, callBackURL, url = ''):
                urlParam = '';
                if (url != None and url != ''):
                        urlParam = str(url)+"|"

                callBackURLParam = '';
                if (callBackURL != None and callBackURL != ''):
                        callBackURLParam = str(callBackURL)

                return applicationSecret +"|"+ urlParam + callBackURLParam + \
                "|"+str(self.format)+"|"+str(int(self.height))+"|"+str(int(self.width))+"|"+str(int(self.browserHeight))+"|"+str(int(self.browserWidth))+"|"+str(self.customId)+ \
                "|"+str(int(self.delay))+"|"+str(self.targetElement)+"|"+str(self.customWaterMarkId)+"|"+str(int(self.requestAs))+"|"+str(self.country)+"|"+str(int(self.quality))+"|"+str(self.hideElement)+"|"+str(self.exportURL)+"|"+str(self.waitForElement)+"|"+str(int(self.transparent))+"|"+str(self.encryptionKey)+"|"+str(int(self.noAds))+"|"+str(self.post)+"|"+str(self.proxy)+"|"+str(self.address)+"|"+str(int(self.noCookieNotifications))+"|"+str(int(self.hd))+"|"+str(self.clickElement)
                