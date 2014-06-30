#!/usr/bin/python

class ScreenShotStatus:

    def __init__(self, processing, cached, expired, message):
        self.Processing = processing
        self.Cached = cached
        self.Expired = expired
        self.Message = message
