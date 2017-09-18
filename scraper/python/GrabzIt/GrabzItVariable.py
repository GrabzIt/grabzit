#!/usr/bin/python

class GrabzItVariable(object):
    """ Common options when changing a global variable contained within the scrape instructions
        Attributes:
        name        Create the variable with the desired name. If a variable with the same name exists it will be overwritten
        value       Set the value of the variable
    """
    #
    #Create the variable with the desired name. If a variable with the same name exists it will be overwritten
    #
    #name - name of the variable
    #
    def __init__(self, name):
        self.name = name
        self.value = None
        
    def GetTypeName(self):
        return "Variable"
        
    def ToXML(self):
        xml = '<?xml version="1.0" encoding="UTF-8"?><Variable xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">'
        if self.value != None and ((hasattr(self.value, '__len__') and (not isinstance(self.value, str))) or (isinstance(self.value, dict))):
            xml += '<Array>';
            if (isinstance(self.value, dict)):
                for k, v in self.value.items():
                    if (k == None and v == None):
                        continue
                    xml += '<KeyValue>'
                    if (k != None):
                        xml += '<Key>'
                        xml += str(k)
                        xml += '</Key>'
                    if (v != None):
                        xml += '<Value>'
                        xml += str(v)
                        xml += '</Value>'
                    xml += '</KeyValue>'
            else:
                for k, v in enumerate(self.value):
                    if (v == None):
                        continue
                    xml += '<KeyValue>'
                    if (v != None):
                        xml += '<Key>'
                        xml += str(k)
                        xml += '</Key>'
                        xml += '<Value>'
                        xml += str(v)
                        xml += '</Value>'
                    xml += '</KeyValue>'
            xml += '</Array>'
        elif (self.value != None):
            xml += '<Value>'
            xml += str(self.value)
            xml += '</Value>'
        if (self.name != None):
            xml += '<Name>'
            xml += str(self.name)
            xml += '</Name>'
        xml += '</Variable>'
        return xml