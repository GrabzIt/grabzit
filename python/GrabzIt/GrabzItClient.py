#!/usr/bin/python

import os
import urllib.request
import hashlib
import http.client
import mimetypes
import io
from xml.dom import minidom
from time import sleep
from GrabzIt import GrabzItCookie
from GrabzIt import ScreenShotStatus
from GrabzIt import GrabzItWaterMark

class GrabzItClient:

        WebServicesBaseURL = "http://grabz.it/services/"
        TrueString = "True"

        def __init__(self, applicationKey, applicationSecret):
                self.applicationKey = applicationKey
                self.applicationSecret = applicationSecret
                self.signaturePartOne = ""
                self.signaturePartTwo = ""
                self.request = ""
                self.requestParams = {}
                
        #
        #This method sets the parameters required to take a screenshot of a web page.
        #
        #url - The URL that the screenshot should be made of
        #browserWidth - The width of the browser in pixels
        #browserHeight - The height of the browser in pixels
        #outputHeight - The height of the resulting thumbnail in pixels
        #outputWidth - The width of the resulting thumbnail in pixels
        #customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
        #format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
        #delay - The number of milliseconds to wait before taking the screenshot
        #targetElement - The id of the only HTML element in the web page to turn into a screenshot
        #requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1 and Search Engine = 2
        #customWaterMarkId - add a custom watermark to the image
        #country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
        #
        def SetImageOptions(self, url, callback = '', customId = '', browserWidth = 0, browserHeight = 0, width = 0, height = 0, format = '', delay = 0, targetElement = '', requestAs = 0, customWaterMarkId = '', country = ''):
                self.requestParams = {"key":self.applicationKey, "url":str(url), "width":int(width),"height":int(height),"format":str(format),"bwidth":int(browserWidth),"bheight":int(browserHeight),"callback":str(callback),"customid":str(customId),"delay":int(delay),"target":str(targetElement),"customwatermarkid":str(customWaterMarkId), "requestmobileversion": int(requestAs), "country": str(country)}                   
                
                self.request = self.WebServicesBaseURL + "takepicture.ashx?"
                self.signaturePartOne = self.applicationSecret+"|"+str(url)+"|"
                self.signaturePartTwo = "|"+str(format)+"|"+str(int(height))+"|"+str(int(width))+"|"+str(int(browserHeight))+"|"+str(int(browserWidth))+"|"+str(customId)+"|"+str(int(delay))+"|"+str(targetElement)+"|"+str(customWaterMarkId)+"|"+str(int(requestAs))+"|"+str(country)

        #
        #This method sets the parameters required to extract all tables from a web page.
        #
        #url - The URL that the should be used to extract tables
        #format - The format the tableshould be in: csv, xlsx
        #customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
        #includeHeaderNames - If true header names will be included in the table
        #includeAllTables - If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX format.
        #targetElement - The id of the only HTML element in the web page that should be used to extract tables from
        #requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1 and Search Engine = 2
        #country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
        #
        def SetTableOptions(self, url, customId = '', tableNumberToInclude = 1, format = 'csv', includeHeaderNames = True, includeAllTables = False, targetElement = '', requestAs = 0, country = ''):
                self.requestParams = {"key":self.applicationKey, "url":url, "includeAllTables":int(includeAllTables),"includeHeaderNames":int(includeHeaderNames),"format":str(format),"tableToInclude":int(tableNumberToInclude),"customid":str(customId),"target":str(targetElement),"requestmobileversion":int(requestAs),"country":str(country)}                
        
                self.request = self.WebServicesBaseURL + "taketable.ashx?"
                
                self.signaturePartOne = self.applicationSecret+"|"+url+"|"
                self.signaturePartTwo = "|"+str(customId)+"|"+str(int(tableNumberToInclude))+"|"+str(int(includeAllTables))+"|"+str(int(includeHeaderNames))+"|"+str(targetElement)+"|"+str(format)+"|"+str(int(requestAs))+"|"+str(country)

        #
        #This method sets the parameters required to convert a web page into a PDF.
        #
        #url - The URL that the should be converted into a pdf
        #customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
        #includeBackground - If true the background of the web page should be included in the screenshot
        #pagesize - The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5'.
        #orientation - The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
        #includeLinks - True if links should be included in the PDF
        #includeOutline - True if the PDF outline should be included
        #title - Provide a title to the PDF document
        #coverURL - The URL of a web page that should be used as a cover page for the PDF
        #marginTop - The margin that should appear at the top of the PDF document page
        #marginLeft - The margin that should appear at the left of the PDF document page
        #marginBottom - The margin that should appear at the bottom of the PDF document page
        #marginRight - The margin that should appear at the right of the PDF document
        #delay - The number of milliseconds to wait before taking the screenshot
        #requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1 and Search Engine = 2
        #customWaterMarkId - add a custom watermark to each page of the PDF document
        #country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
        #
        def SetPDFOptions(self, url, customId = '', includeBackground = True, pagesize = 'A4', orientation = 'Portrait', includeLinks = True, includeOutline = False, title = '', coverURL = '', marginTop = 10, marginLeft = 10, marginBottom = 10, marginRight = 10, delay = 0, requestAs = 0, customWaterMarkId = '', country = ''):
                pagesize = pagesize.upper()
                orientation = orientation.title()
                
                self.requestParams = {"key":self.applicationKey, "url":url, "background":int(includeBackground),"pagesize":str(pagesize),"orientation":str(orientation),"customid":str(customId),"customWaterMarkId":customWaterMarkId,"includelinks":int(includeLinks),"includeoutline":int(includeOutline),"title":str(title),"coverurl":str(coverURL),"mleft":int(marginLeft),"mright":int(marginRight),"mtop":int(marginTop),"mbottom":int(marginBottom),"delay":int(delay),"requestmobileversion":int(requestAs),"country":str(country)}                                       

                self.request = self.WebServicesBaseURL + "takepdf.ashx?"

                self.signaturePartOne = self.applicationSecret+"|"+url+"|"
                self.signaturePartTwo = "|"+str(customId)+"|"+str(int(includeBackground))+"|"+str(pagesize) +"|"+str(orientation)+"|"+str(customWaterMarkId)+"|"+str(int(includeLinks))+"|"+str(int(includeOutline))+"|"+str(title)+"|"+str(coverURL)+"|"+str(int(marginTop))+"|"+str(int(marginLeft))+"|"+str(int(marginBottom))+"|"+str(int(marginRight))+"|"+str(int(delay))+"|"+str(int(requestAs))+"|"+str(country)

        #
        #This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the #GetResult method.
        #
        #This is the recommended method of saving a file.
        #
        def Save(self, callBackURL = ''):
                if (self.signaturePartOne == None and self.signaturePartTwo == None and self.request == None ):
                          raise Exception("No screenshot parameters have been set.")
                
                self.requestParams["callback"] = str(callBackURL)
                encoded_qs = urllib.parse.urlencode(self.requestParams)
                
                sig = self.CreateSignature(self.signaturePartOne+str(callBackURL)+self.signaturePartTwo)                               

                self.request += encoded_qs+"&sig="+sig
                
                return self.GetResultObject(self.HTTPGet(self.request), "ID")

        #
        #This function attempts to Save the result synchronously to a file.
        #
        #WARNING this method is synchronous so will cause a application to pause while the result is processed.
        #
        #This function returns the true if it is successful otherwise it throws an exception.
        #
        def SaveTo(self, saveToFile):
                id = self.Save()
                
                if (id == None or id == ""):
                        return False;
                
                #Wait for it to be ready.
                while(1):
                    status = self.GetStatus(id)
                    if not(status.Cached) and not(status.Processing):
                        raise Exception("The screenshot did not complete with the error: " + status.Message)
                        break
                        
                    elif status.Cached:
                        result = self.GetPicture(id)
                        
                        if result == None:
                                raise Exception("The screenshot could not be found on GrabzIt.")
                                break
                                
                        fo = open(saveToFile, "wb")
                        fo.write(result)
                        fo.close()
                        
                        break

                    sleep(3)
                    
                return True
        
        #
        #This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet.
        #
        #id - The unique identifier of the screenshot, returned by the callback handler or the Save method
        #
        #This function returns the screenshot
        #
        def GetResult(self, id):
                result = io.BytesIO(self.HTTPGet(self.WebServicesBaseURL + "getfile.ashx?id=" + id)).getvalue()
                
                if result == "":
                        return None
                
                return result                           
        
        #
        #Get the current status of a GrabzIt screenshot
        #
        #id - The id of the screenshot
        #
        #This function returns a Status object representing the screenshot
        #
        def GetStatus(self, id):
                
                if (id == "" or id == None):
                        return None
                
                result = self.HTTPGet(self.WebServicesBaseURL + "getstatus.ashx?id=" + id)    
                
                dom = minidom.parseString(result)
                
                processing = False
                cached = False
                expired = False
                message = ""
                
                messageNodes = dom.getElementsByTagName("Message")
                processingNodes = dom.getElementsByTagName("Processing")
                cachedNodes = dom.getElementsByTagName("Cached")
                expiredNodes = dom.getElementsByTagName("Expired")
                
                for messageNode in messageNodes:
                        if messageNode.firstChild == None:
                                break
                        message = messageNode.firstChild.nodeValue
                        break

                for processingNode in processingNodes:
                        if processingNode.firstChild == None:
                                break
                        processing = (processingNode.firstChild.nodeValue == self.TrueString)
                        break                   
                        
                for cachedNode in cachedNodes:
                        if cachedNode.firstChild == None:
                                break
                        cached = (cachedNode.firstChild.nodeValue == self.TrueString)
                        break                   
                        
                for expiredNode in expiredNodes:
                        if expiredNode.firstChild == None:
                                break
                        expired = (expiredNode.firstChild.nodeValue == self.TrueString)
                        break                                           

                return ScreenShotStatus.ScreenShotStatus(processing, cached, expired, message);         

        #
        #Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
        #
        #domain - The domain to return cookies for.
        #
        #This function returns an array of cookies
        #
        def GetCookies(self, domain):
                sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(domain))
                qs = {"key":self.applicationKey, "domain":domain}
                encoded_qs = urllib.urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                dom = minidom.parseString(self.HTTPGet(self.WebServicesBaseURL + "getcookies.ashx?" + encoded_qs))

                message = ""

                messageNodes = dom.getElementsByTagName("Message")
                
                for messageNode in messageNodes:
                        if messageNode.firstChild == None:
                                break
                        message = messageNode.firstChild.nodeValue
                        break           

                if len(message) > 0:
                        raise Exception(message)
                        
                results = []

                cookieNodes = dom.getElementsByTagName("Cookie")
                
                for cookieNode in cookieNodes:
                        name = cookieNode.getElementsByTagName('Name')
                        domain = cookieNode.getElementsByTagName('Domain')
                        value = cookieNode.getElementsByTagName('Value')
                        path = cookieNode.getElementsByTagName('Path')
                        expires = cookieNode.getElementsByTagName('Expires')
                        httpOnly = cookieNode.getElementsByTagName('HttpOnly')
                        type = cookieNode.getElementsByTagName('Type')
                        
                        results.append(GrabzItCookie.GrabzItCookie(self.GetFirstValue(name), self.GetFirstValue(value), self.GetFirstValue(domain), self.GetFirstValue(path), (self.GetFirstValue(httpOnly) == self.TrueString), self.GetFirstValue(expires), self.GetFirstValue(type)))                                                                

                return results
                
        #
        #Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
        #cookie is overridden.
        #
        #This can be useful if a websites functionality is controlled by cookies.
        #
        #name - The name of the cookie to set.
        #domain - The domain of the website to set the cookie for.
        #value - The value of the cookie.
        #path - The website path the cookie relates to.
        #httponly - Is the cookie only used on HTTP
        #expires - When the cookie expires. Pass a null value if it does not expire.
        #
        #This function returns true if the cookie was successfully set.
        #
        def SetCookie(self, name, domain, value = "", path = "/", httponly = False, expires = ""):
                sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(name)+"|"+str(domain)+"|"+str(value)+"|"+str(path)+"|"+str(int(httponly))+"|"+str(expires)+"|0")        

                qs = {"key":self.applicationKey, "domain":domain, "name":name, "value":value, "path":path, "httponly":int(httponly), "expires":expires}

                encoded_qs = urllib.urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                return self.IsSuccessful(self.HTTPGet(self.WebServicesBaseURL + "setcookie.ashx?" + encoded_qs))               

        #
        #Delete a custom cookie or block a global cookie from being used.
        #
        #name - The name of the cookie to delete
        #domain - The website the cookie belongs to
        #
        #This function returns true if the cookie was successfully set.
        #               
        def DeleteCookie(self, name, domain):
                sig =  self.CreateSignature((str(self.applicationSecret)+"|"+str(name)+"|"+str(domain)+"|1"))

                qs = {"key":self.applicationKey, "domain":domain, "name":name, "delete":1}

                encoded_qs = urllib.urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                return self.IsSuccessful(self.HTTPGet(self.WebServicesBaseURL + "setcookie.ashx?" + encoded_qs))               

        #
        #Add a new custom watermark.
        #
        #identifier - The identifier you want to give the custom watermark. It is important that this identifier is unique.
        #path - The absolute path of the watermark on your server. For instance C:/watermark/1.png
        #xpos - The horizontal position you want the screenshot to appear at: Left = 0, Center = 1, Right = 2
        #ypos - The vertical position you want the screenshot to appear at: Top = 0, Middle = 1, Bottom = 2
        #
        #This function returns true if the watermark was successfully set.
        #
        def AddWaterMark(self, identifier, path, xpos, ypos):
                files = []
                try:                    
                        files.append(['watermark', os.path.basename(path), open(path, 'rb').read()])
                except:
                        raise Exception("File: " + path + " does not exist")
                
                sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier)+"|"+str(xpos)+"|"+str(ypos))
                
                fields = {}
                fields['key'] = self.applicationKey
                fields['identifier'] = identifier
                fields['xpos'] = str(xpos)
                fields['ypos'] = str(ypos)
                fields['sig'] = sig
        
                return self.IsSuccessful(self.HTTPPost("grabz.it", "/services/addwatermark.ashx", fields, files))
                
        #
        #Delete a custom watermark.
        #
        #identifier - The identifier of the custom watermark you want to delete
        #
        #This function returns true if the watermark was successfully deleted.
        #
        def DeleteWaterMark(self, identifier):
                sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier))

                qs = {"key":self.applicationKey, "identifier":identifier}

                encoded_qs = urllib.urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                return self.IsSuccessful(self.HTTPGet(self.WebServicesBaseURL + "deletewatermark.ashx?" + encoded_qs));        

        #
        #Get your uploaded custom watermarks
        #
        #A GrabzItWaterMark array
        #
        def GetWaterMarks(self):
                return self.getWaterMarks()

        #
        #Get your uploaded custom watermark
        #
        #identifier - The identifier of a particular custom watermark you want to view
        #
        #the GrabzItWaterMark with the specified identifier
        #       
        def GetWaterMark(self, identifier):
                watermarks = self.getWaterMarks(identifier)
                if watermarks.Length == 1:
                        return watermarks.get(0)
                return None      
        
        def getWaterMarks(self, identifier = ""):
                sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier))

                qs = {"key":self.applicationKey, "identifier":identifier}

                encoded_qs = urllib.urlencode(qs)
                
                encoded_qs += "&sig="+sig;              

                dom = minidom.parseString(self.HTTPGet(self.WebServicesBaseURL + "getwatermarks.ashx?" + encoded_qs))

                message = ""

                messageNodes = dom.getElementsByTagName("Message")
                
                for messageNode in messageNodes:
                        if messageNode.firstChild == None:
                                break
                        message = messageNode.firstChild.nodeValue
                        break           

                if len(message) > 0:
                        raise Exception(message)
                        
                results = []

                waterMarkNodes = dom.getElementsByTagName("WaterMark")
                
                for waterMarkNode in waterMarkNodes:
                        identifier = waterMarkNode.getElementsByTagName('Identifier')
                        xPosition = waterMarkNode.getElementsByTagName('XPosition')
                        yPosition = waterMarkNode.getElementsByTagName('YPosition')
                        format = waterMarkNode.getElementsByTagName('Format')
                        
                        results.append(GrabzItWaterMark.GrabzItWaterMark(self.GetFirstValue(identifier), self.GetFirstValue(xPosition), self.GetFirstValue(yPosition), self.GetFirstValue(format)))                                                             

                return results
                
        #
        #DEPRECATED - Use the GetResult method instead
        #               
        def GetPicture(self, id):
                return self.GetResult(id)
                
        #
        #DEPRECATED - Use SetImageOptions and Save method instead
        #               
        def TakePicture(self, url, callback = '', customId = '', browserWidth = 0, browserHeight = 0, width = 0, height = 0, format = '', delay = 0, targetElement = ''):       
                self.SetImageOptions(url, callback, customId, browserWidth, browserHeight, width, height, format, delay, targetElement)
                return self.Save(callback)
        
        #
        #DEPRECATED - Use the SetImageOptions and SaveTo methods instead
        #
        def SavePicture(self, url, saveToFile, browserWidth = 0, browserHeight = 0, width = 0, height = 0, format = '', delay = 0, targetElement = ''):
                self.SetImageOptions(url, '', customId, browserWidth, browserHeight, width, height, format, delay, targetElement)
                return self.SaveTo(saveToFile)
        
        def IsSuccessful(self, result):
                return self.GetResultObject(result, "Result") == self.TrueString
        
        def GetResultObject(self, result, resultTagName):
                dom = minidom.parseString(result)
                
                message = ""
                result = ""
                
                messageNodes = dom.getElementsByTagName("Message")
                nodes = dom.getElementsByTagName(resultTagName)
                
                for messageNode in messageNodes:
                        if messageNode.firstChild == None:
                                break
                        message = messageNode.firstChild.nodeValue
                        break

                for node in nodes:
                        if node.firstChild == None:
                                break
                        result = node.firstChild.nodeValue
                        break                   
                
                if len(message) > 0:
                        raise Exception(message)
                        
                return result
        
                
        def GetFirstValue(self, node):
                if node.length > 0 and node[0].firstChild:
                        return node[0].firstChild.nodeValue
                return ""
                
        def HTTPPost(self, host, selector, fields, files):
            content_type, body = self.EncodeMultipartFormdata(fields, files)
            h = http.client.HTTP(host)
            h.putrequest('POST', selector)
            h.putheader('content-type', content_type)
            h.putheader('content-length', str(len(body)))
            h.endheaders()
            h.send(body)
            errcode, errmsg, headers = h.getreply()
            self.CheckResponseHeader(errorcode);
            return h.file.read()

        def EncodeMultipartFormdata(self, fields, files):
            LIMIT = '----------lImIt_of_THE_fIle_eW_$'
            CRLF = '\r\n'
            L = []
            for (key, value) in fields.iteritems():
                L.append('--' + LIMIT)
                L.append('Content-Disposition: form-data; name="%s"' % key)
                L.append('')
                L.append(value)
            for (key, filename, value) in files:
                L.append('--' + LIMIT)
                L.append('Content-Disposition: form-data; name="%s"; filename="%s"' % (key, filename))
                L.append('Content-Type: %s' % self.GetContentType(filename))
                L.append('')
                L.append(value)
            L.append('--' + LIMIT + '--')
            L.append('')
            body = CRLF.join(L)
            content_type = 'multipart/form-data; boundary=%s' % LIMIT
            return content_type, body

        def GetContentType(self, filename):
            return mimetypes.guess_type(filename)[0] or 'application/octet-stream'
        
        def HTTPGet(self, url):
                connection = urllib.request.urlopen(url)
                self.CheckResponseHeader(connection.getcode())
                return connection.read()
        
        
        def CheckResponseHeader(self, httpCode):
                if httpCode == 403:
                        raise Exception('Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.')
                elif httpCode >= 400:
                        raise Exception('A network error occured when connecting to the GrabzIt servers.')

        def CreateSignature(self, value):
                md5 = hashlib.md5()
                md5.update(value.encode('utf-8'))
                return md5.hexdigest()