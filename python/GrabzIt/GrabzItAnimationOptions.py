#!/usr/bin/python

from GrabzIt import GrabzItBaseOptions

class GrabzItAnimationOptions(GrabzItBaseOptions.GrabzItBaseOptions):
        """ Available options when creating a animated GIF

            Attributes:

            width               set the width of the resulting animated GIF in pixels
            height              set the height of the resulting animated GIF in pixels
            start               set the starting position of the video that should be converted into an animated GIF
            duration            set the length in seconds of the video that should be converted into a animated GIF
            speed               set the speed of the animated GIF from 0.2 to 10 times the original speed
            framesPerSecond     set the number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60
            repeat              set the number of times to loop the animated GIF. If 0 it will loop forever
            reverse             set to true if the frames of the animated GIF should be reversed
            customWaterMarkId   set a custom watermark to add to the animated GIF
            quality             set the quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
        """

        def __init__(self):
                GrabzItBaseOptions.GrabzItBaseOptions.__init__(self)
                self.width = 0
                self.height = 0
                self.start = 0
                self.duration = 1
                self.speed = 0
                self.framesPerSecond = 0
                self.repeat = 0
                self.reverse = False
                self.customWaterMarkId = ''
                self.quality = -1

        def _getParameters(self, applicationKey, sig, callBackURL, dataName, dataValue):
                params = self._createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
                params["width"] = int(self.width)
                params["height"] = int(self.height)
                params["duration"] = int(self.duration) 
                params["start"] = int(self.start) 
                params["speed"] = self._toString(self.speed) 
                params["fps"] = self._toString(self.framesPerSecond)
                params["repeat"] = int(self.repeat) 
                params["reverse"] = int(self.reverse)
                params["customwatermarkid"] = str(self.customWaterMarkId) 
                params["quality"] = int(self.quality) 

                return params

        def _getSignatureString(self, applicationSecret, callBackURL, url = ''):
                urlParam = '';
                if (url != None and url != ''):
                        urlParam = str(url)+"|"

                callBackURLParam = '';
                if (callBackURL != None and callBackURL != ''):
                        callBackURLParam = str(callBackURL)

                return applicationSecret +"|"+ urlParam + callBackURLParam + \
                "|"+str(int(self.height))+"|"+str(int(self.width))+"|"+str(self.customId)+"|"+self._toString(self.framesPerSecond)+"|"+self._toString(self.speed)+"|"+str(int(self.duration))+ \
                "|"+str(int(self.repeat))+"|"+str(int(self.reverse))+"|"+str(int(self.start))+"|"+str(self.customWaterMarkId)+"|"+str(self.country)+"|"+str(int(self.quality))+"|"+str(self.exportURL)

        def _toString(self, value):
                if ((value % 1) == 0):
                        return str(int(value))
                return str(float(value))