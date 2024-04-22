#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItVideoOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a video

            Attributes:

            browserWidth        the width of the browser in pixels
            browserHeight       the height of the browser in pixels
            width               the width of the resulting video in pixels
            height              the height of the resulting video in pixels
            start               set starting time of the web page that should be converted into a video
            duration            set length in seconds of the web page that should be converted into a video
            framesPerSecond     set number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10
            customWaterMarkId   set a custom watermark to add to the video
            clickElement        the CSS selector of the HTML element in the web page to click
            waitForElement      the CSS selector of the HTML element in the web page that must be visible before the capture is performed
            requestAs           the user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
            noAds               set to true if adverts should be automatically hidden
            noCookieNotifications   set to true if cookie notifications should be automatically hidden
            address                 the URL to execute the HTML code in
        """

        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.browserWidth = 0
                self.browserHeight = 0
                self.width = 0
                self.height = 0                
                self.start = 0
                self.duration = 10
                self.framesPerSecond = 0
                self.customWaterMarkId = ''
                self.clickElement = ''
                self.waitForElement = ''
                self.requestAs = 0
                self.noAds = False
                self.noCookieNotifications = False
                self.address = ''

        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["bwidth"] = int(self.browserWidth)
                params["bheight"] = int(self.browserHeight)
                params["width"] = int(self.width)
                params["height"] = int(self.height)                
                params["duration"] = int(self.duration) 
                params["start"] = int(self.start) 
                params["fps"] = self._toString(self.framesPerSecond)
                params["waitfor"] = str(self.waitForElement)     
                params["requestmobileversion"] = int(self.requestAs)
                params["customwatermarkid"] = str(self.customWaterMarkId) 
                params['noads'] = int(self.noAds)
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
                "|"+str(int(self.browserHeight))+"|"+str(int(self.browserWidth))+"|"+str(self.customId)+"|"+str(self.customWaterMarkId)+"|"+str(int(self.start))+"|"+str(int(self.requestAs)) + \
                "|"+str(self.country)+"|"+str(self.exportURL)+"|"+str(self.waitForElement)+"|"+str(self.encryptionKey)+"|"+str(int(self.noAds))+"|"+str(self.post)+"|"+str(self.proxy) + \
                "|"+str(self.address)+"|"+str(int(self.noCookieNotifications))+"|"+str(self.clickElement)+"|"+self._toString(self.framesPerSecond)+"|"+str(int(self.duration)) + \
                "|"+str(int(self.width))+"|"+str(int(self.height))

        def _toString(self, value):
                if ((value % 1) == 0):
                        return str(int(value))
                return str(float(value))