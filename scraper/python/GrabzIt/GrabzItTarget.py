#!/usr/bin/python

class GrabzItTarget(object):
    """ Common options when changing the target of a scrape
        Attributes:
        seedURLs        Specify the seed URL's of a scrape, if any
        url             Specify the URL to start the scrape on
    """
    def __init__(self):
        self.seedURLs = None
        self.url = None
        
    def GetTypeName(self):
        return "Target"
        
    def ToXML(self):
        xml = '<?xml version="1.0" encoding="UTF-8"?><Target xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">'
        if self.seedURLs != None and hasattr(self.seedURLs, '__len__') and (not isinstance(self.seedURLs, str)):
            xml += '<SeedURLs>'
            for value in self.seedURLs:
                xml += '<string>'
                xml += str(value)
                xml += '</string>'
            xml += '</SeedURLs>'
        if self.url != None:
            xml += '<URL>'
            xml += str(self.url)
            xml += '</URL>'
        xml += '</Target>'
        return xml