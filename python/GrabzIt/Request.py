#!/usr/bin/python

class Request:
        def __init__(self, url, isPost, options, data = ''):
                self.url = url
                self.isPost = isPost
                self.options = options
                self.data = data

        def _targetUrl(self):
                if (self.isPost):
                        return ''
                return self.data