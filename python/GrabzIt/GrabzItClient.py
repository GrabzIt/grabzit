#!/usr/bin/python

import os

try:
	from urllib.parse import urlencode	
	from urllib.request import urlopen
	from urllib.parse import urlparse
	from urllib.parse import quote
	import http.client as httpClient
except ImportError:
	from urllib import urlopen
	from urlparse import urlparse
	from urllib import urlencode
	from urllib import quote
	import httplib as httpClient

import hashlib	
import mimetypes
import io
import base64
from xml.dom import minidom
from time import sleep
from GrabzIt import GrabzItCookie
from GrabzIt import ScreenShotStatus
from GrabzIt import GrabzItWaterMark
from GrabzIt import GrabzItException
from GrabzIt import Request
from GrabzIt import GrabzItBaseOptions
from GrabzIt import GrabzItAnimationOptions
from GrabzIt import GrabzItImageOptions
from GrabzIt import GrabzItPDFOptions
from GrabzIt import GrabzItDOCXOptions
from GrabzIt import GrabzItTableOptions
from GrabzIt import AES
from GrabzIt import BlockFeeder

class GrabzItClient:

		WebServicesBaseURLGet = "://api.grabz.it/services/"
		WebServicesBaseURLPost = "/services/"
		TakePicture = "takepicture.ashx"
		TakePDF = "takepdf.ashx"
		TakeDOCX = "takedocx.ashx"
		TakeTable = "taketable.ashx"
		TrueString = "True"

		def __init__(self, applicationKey, applicationSecret):
				self.applicationKey = applicationKey
				self.applicationSecret = applicationSecret
				self.request = None
				self.protocol = "http"
				
		#
		# This method specifies the URL of the online video that should be converted into a animated GIF
		#
		# url - The URL to convert into a animated GIF
		# options - A instance of the GrabzItAnimationOptions class that defines any special options to use when creating the animated GIF
		#
		def URLToAnimation(self, url, options = None):
				if (options == None):
						options = GrabzItAnimationOptions.GrabzItAnimationOptions()
				
				self.request = Request.Request(self.protocol + self.WebServicesBaseURLGet + "takeanimation.ashx", False, options, url)
			
		#
		# This method specifies the URL that should be converted into a image screenshot.
		#
		# url - The URL to capture as a screenshot
		# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the screenshot
		#
		def URLToImage(self, url, options = None):
				if (options == None):
						options = GrabzItImageOptions.GrabzItImageOptions()
				
				self.request = Request.Request(self.protocol + self.WebServicesBaseURLGet + self.TakePicture, False, options, url)

		#
		# This method specifies the HTML that should be converted into a image.
		#
		# html - The HTML to convert into a image
		# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image
		#				 
		def HTMLToImage(self, html, options = None):
				if (options == None):
						options = GrabzItImageOptions.GrabzItImageOptions()
				
				self.request = Request.Request(self.WebServicesBaseURLPost + self.TakePicture, True, options, html)

		#
		# This method specifies a HTML file that should be converted into a image.
		#
		# path - The file path of a HTML file to convert into a image
		# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image
		#				 
		def FileToImage(self, path, options = None):
				self.HTMLToImage(self.ReadFile(path), options)
			   
		#
		# This method specifies the URL that the HTML tables should be extracted from.
		#
		# url - The URL to extract HTML tables from.
		# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table
		#
		def URLToTable(self, url, options = None):
				if (options == None):
						options = GrabzItTableOptions.GrabzItTableOptions()
				
				self.request = Request.Request(self.protocol + self.WebServicesBaseURLGet + self.TakeTable, False, options, url)

		#
		# This method specifies the HTML that the HTML tables should be extracted from.
		#
		# html - The HTML to extract HTML tables from
		# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table	
		#
		def HTMLToTable(self, html, options = None):
				if (options == None):
						options = GrabzItTableOptions.GrabzItTableOptions()
				
				self.request = Request.Request(self.WebServicesBaseURLPost + self.TakeTable, True, options, html)				

		#
		# This method specifies a HTML file that the HTML tables should be extracted from.
		#
		# path - The file path of a HTML file to extract HTML tables from.
		# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table 
		#
		def FileToTable(self, path, options = None):
				self.HTMLToTable(self.ReadFile(path), options) 
				
		#
		# This method specifies the URL that should be converted into a PDF.
		#
		# url - The URL to capture as a PDF
		# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF
		#
		def URLToPDF(self, url, options = None):
				if (options == None):
						options = GrabzItPDFOptions.GrabzItPDFOptions()
				
				self.request = Request.Request(self.protocol + self.WebServicesBaseURLGet + self.TakePDF, False, options, url)

		#
		# This method specifies the HTML that should be converted into a PDF.
		#
		# html - The HTML to convert into a PDF
		# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF.
		#
		def HTMLToPDF(self, html, options = None):
				if (options == None):
						options = GrabzItPDFOptions.GrabzItPDFOptions()
				
				self.request = Request.Request(self.WebServicesBaseURLPost + self.TakePDF, True, options, html)				  

		#
		# This method specifies a HTML file that should be converted into a PDF.
		#
		# path - The file path of a HTML file to convert into a PDF
		# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF 
		#
		def FileToPDF(self, path, options = None):
				self.HTMLToPDF(self.ReadFile(path), options)

		#
		# This method specifies the URL that should be converted into a DOCX.
		#
		# url - The URL to capture as a DOCX
		# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX
		#
		def URLToDOCX(self, url, options = None):
				if (options == None):
						options = GrabzItDOCXOptions.GrabzItDOCXOptions()
				
				self.request = Request.Request(self.protocol + self.WebServicesBaseURLGet + self.TakeDOCX, False, options, url)

		#
		# This method specifies the HTML that should be converted into a PDF.
		#
		# html - The HTML to convert into a DOCX
		# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX.
		#
		def HTMLToDOCX(self, html, options = None):
				if (options == None):
						options = GrabzItDOCXOptions.GrabzItDOCXOptions()
				
				self.request = Request.Request(self.WebServicesBaseURLPost + self.TakeDOCX, True, options, html)			   

		#
		# This method specifies a HTML file that should be converted into a DOCX.
		#
		# path - The file path of a HTML file to convert into a DOCX
		# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX 
		#
		def FileToDOCX(self, path, options = None):
				self.HTMLToDOCX(self.ReadFile(path), options)
				
		#
		# This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the #GetResult method.
		#
		# This is the recommended method of saving a file.
		#
		def Save(self, callBackURL = ''):
				if (self.request == None ):
						raise GrabzItException.GrabzItException("No parameters have been set.", GrabzItException.GrabzItException.PARAMETER_MISSING_PARAMETERS)
				
				sig = self.CreateSignature(self.request.options._getSignatureString(self.applicationSecret, callBackURL, self.request._targetUrl()))							   

				if (self.request.isPost == False):				
						return self.GetResultObject(self.HTTPGet(self.request.url + '?' + urlencode(self.request.options._getParameters(self.applicationKey, sig, callBackURL, 'url', self.request.data))), "ID")
				else:
						return self.GetResultObject(self.HTTPPost(self.request.url, self.request.options._getParameters(self.applicationKey, sig, callBackURL, 'html', quote(self.request.data))), "ID")

		#
		# Calls the GrabzIt web service to take the screenshot and saves it to the target path provided. if no target path is provided
		# it returns the screenshot byte data.
		#
		# WARNING this method is synchronous so will cause a application to pause while the result is processed.
		#
		# This function returns the true if it is successful saved to a file, or if it is not saving to a file byte data is returned,
		# otherwise the method throws an exception.
		#
		def SaveTo(self, saveToFile = ''):
				id = self.Save()

				if (id == None or id == ""):
						return False

				#Wait for it to be possibly ready
				sleep((3000 + self.request.options.delay) / 1000)

				#Wait for it to be ready.
				while(1):
						status = self.GetStatus(id)
						if not(status.Cached) and not(status.Processing):
								raise GrabzItException.GrabzItException("The capture did not complete with the error: " + status.Message, GrabzItException.GrabzItException.RENDERING_ERROR)
								break
						elif status.Cached:
								result = self.GetResult(id)
								if result == None:
										raise GrabzItException.GrabzItException("The capture could not be found on GrabzIt.", GrabzItException.GrabzItException.RENDERING_MISSING_SCREENSHOT)
										break

								if (saveToFile == None or saveToFile == ""):
										return result
										
								fo = open(saveToFile, "wb")
								fo.write(result)				
								fo.close()
						
								break

						sleep(3)					
				return True
		
		#
		# This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet.
		#
		# id - The unique identifier of the screenshot, returned by the callback handler or the Save method
		#
		# This function returns the screenshot
		#
		def GetResult(self, id):
				if (id == "" or id == None):
						return None
				
				result = io.BytesIO(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "getfile.ashx?id=" + id)).getvalue()
				
				if result == None or len(result) == 0:
						return None
				
				return result							
		
		#
		# Get the current status of a GrabzIt screenshot
		#
		# id - The id of the screenshot
		#
		# This function returns a Status object representing the screenshot
		#
		def GetStatus(self, id):
				
				if (id == "" or id == None):
						return None
				
				result = self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "getstatus.ashx?id=" + id)	 
				
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
		# Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
		#
		# domain - The domain to return cookies for.
		#
		# This function returns an array of cookies
		#
		def GetCookies(self, domain):
				sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(domain))
				qs = {"key":self.applicationKey, "domain":domain}
				encoded_qs = urlencode(qs)
				
				encoded_qs += "&sig="+sig

				dom = minidom.parseString(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "getcookies.ashx?" + encoded_qs))

				self.CheckForException(dom)
						
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
		# Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
		# cookie is overridden.
		#
		# This can be useful if a websites functionality is controlled by cookies.
		#
		# name - The name of the cookie to set.
		# domain - The domain of the website to set the cookie for.
		# value - The value of the cookie.
		# path - The website path the cookie relates to.
		# httponly - Is the cookie only used on HTTP
		# expires - When the cookie expires. Pass a empty string value if it does not expire.
		#
		# This function returns true if the cookie was successfully set.
		#
		def SetCookie(self, name, domain, value = "", path = "/", httponly = False, expires = ""):
				sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(name)+"|"+str(domain)+"|"+str(value)+"|"+str(path)+"|"+str(int(httponly))+"|"+str(expires)+"|0")		

				qs = {"key":self.applicationKey, "domain":domain, "name":name, "value":value, "path":path, "httponly":int(httponly), "expires":expires}

				encoded_qs = urlencode(qs)
				
				encoded_qs += "&sig="+sig;

				return self.IsSuccessful(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "setcookie.ashx?" + encoded_qs))				  

		#
		# Delete a custom cookie or block a global cookie from being used.
		#
		# name - The name of the cookie to delete
		# domain - The website the cookie belongs to
		#
		# This function returns true if the cookie was successfully set.
		#				
		def DeleteCookie(self, name, domain):
				sig =  self.CreateSignature((str(self.applicationSecret)+"|"+str(name)+"|"+str(domain)+"|1"))

				qs = {"key":self.applicationKey, "domain":domain, "name":name, "delete":1}

				encoded_qs = urlencode(qs)
				
				encoded_qs += "&sig="+sig;

				return self.IsSuccessful(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "setcookie.ashx?" + encoded_qs))				  

		#
		# Add a new custom watermark.
		#
		# identifier - The identifier you want to give the custom watermark. It is important that this identifier is unique.
		# path - The absolute path of the watermark on your server. For instance C:/watermark/1.png
		# xpos - The horizontal position you want the screenshot to appear at: Left = 0, Center = 1, Right = 2
		# ypos - The vertical position you want the screenshot to appear at: Top = 0, Middle = 1, Bottom = 2
		#
		# This function returns true if the watermark was successfully set.
		#
		def AddWaterMark(self, identifier, path, xpos, ypos):
				files = []
				try:					
						files.append(['watermark', os.path.basename(path), open(path, 'rb').read()])
				except:
						raise GrabzItException.GrabzItException("File: " + path + " does not exist", GrabzItException.GrabzItException.FILE_NON_EXISTANT_PATH)
				
				sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier)+"|"+str(xpos)+"|"+str(ypos))
				
				fields = {}
				fields['key'] = self.applicationKey
				fields['identifier'] = identifier
				fields['xpos'] = str(xpos)
				fields['ypos'] = str(ypos)
				fields['sig'] = sig
		
				return self.IsSuccessful(self.HTTPPost("/services/addwatermark.ashx", fields, files))
				
		#
		# Delete a custom watermark.
		#
		# identifier - The identifier of the custom watermark you want to delete
		#
		# This function returns true if the watermark was successfully deleted.
		#
		def DeleteWaterMark(self, identifier):
				sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier))

				qs = {"key":self.applicationKey, "identifier":identifier}

				encoded_qs = urlencode(qs)
				
				encoded_qs += "&sig="+sig

				return self.IsSuccessful(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "deletewatermark.ashx?" + encoded_qs));		  

		#
		# Get your uploaded custom watermarks
		#
		# A GrabzItWaterMark array
		#
		def GetWaterMarks(self):
				return self.getWaterMarks()

		#
		# Get your uploaded custom watermark
		#
		# identifier - The identifier of a particular custom watermark you want to view
		#
		# the GrabzItWaterMark with the specified identifier
		#		
		def GetWaterMark(self, identifier):
				watermarks = self.getWaterMarks(identifier)
				if watermarks.Length == 1:
						return watermarks.get(0)
				return None		 
		
		#
		# This method sets if requests to GrabzIt's API should use SSL or not
		#
		# value - true if should use SSL
		#
		def UseSSL(self, value):
			if value:
				self.protocol = "https"
			else:
				self.protocol = "http"
				
		#
		# This method creates a cryptographically secure encryption key to pass to the encryption key parameter.
		#				 
		def CreateEncrpytionKey(self):
			return base64.standard_b64encode(os.urandom(32)).decode('utf-8')
		
		#
		# This method will decrypt a encrypted capture file, using the key you passed to the encryption key parameter.
		#
		# data - the encrypted bytes
		# key - the encryption key		 
		#
		def Decrypt(self, data, key):
			if (data == None or data == ""):
				return None
				
			iv = data[0:16]
			payload = data[16:]
			cipher = AES.AESModeOfOperationCBC(base64.standard_b64decode(key), iv)
			decrypter = BlockFeeder.Decrypter(cipher, BlockFeeder.PADDING_NONE)
			decrypted = decrypter.feed(payload)
			decrypted += decrypter.feed()
			
			return decrypted	  
		
		#
		# This method will decrypt a encrypted capture, using the key you passed to the encryption key parameter.
		#
		# path - the path of the encrypted capture
		# key - the encryption key		  
		#
		def DecryptFile(self, path, key):
			data = self.ReadFile(path)
			fo = open(path, "wb")
			fo.write(self.Decrypt(data, key))				
			fo.close()			  
			
		def getWaterMarks(self, identifier = ""):
				sig = self.CreateSignature(str(self.applicationSecret)+"|"+str(identifier))

				qs = {"key":self.applicationKey, "identifier":identifier}

				encoded_qs = urlencode(qs)
				
				encoded_qs += "&sig="+sig;			   

				dom = minidom.parseString(self.HTTPGet(self.protocol + self.WebServicesBaseURLGet + "getwatermarks.ashx?" + encoded_qs))

				self.CheckForException(dom)
						
				results = []

				waterMarkNodes = dom.getElementsByTagName("WaterMark")
				
				for waterMarkNode in waterMarkNodes:
						identifier = waterMarkNode.getElementsByTagName('Identifier')
						xPosition = waterMarkNode.getElementsByTagName('XPosition')
						yPosition = waterMarkNode.getElementsByTagName('YPosition')
						format = waterMarkNode.getElementsByTagName('Format')
						
						results.append(GrabzItWaterMark.GrabzItWaterMark(self.GetFirstValue(identifier), self.GetFirstValue(xPosition), self.GetFirstValue(yPosition), self.GetFirstValue(format)))																

				return results
				
		def IsSuccessful(self, result):
				return self.GetResultObject(result, "Result") == self.TrueString
		
		def GetResultObject(self, result, resultTagName):
				dom = minidom.parseString(result)
				
				result = ""
								
				nodes = dom.getElementsByTagName(resultTagName)
				
				self.CheckForException(dom)
				
				for node in nodes:
						if node.firstChild == None:
								break
						result = node.firstChild.nodeValue
						break					
										
				return result
 
		def CheckForException(self, dom):
				if dom == None:
						return
				
				message = ""
				code = ""				
				
				messageNodes = dom.getElementsByTagName("Message")
				codeNodes = dom.getElementsByTagName("Code")
				
				for messageNode in messageNodes:
						if messageNode.firstChild == None:
								break
						message = messageNode.firstChild.nodeValue
						break

				for codeNode in codeNodes:
						if codeNode.firstChild == None:
								break
						code = codeNode.firstChild.nodeValue
						break
				
				if len(message) > 0:
						raise GrabzItException.GrabzItException(message, code)			
	   
				
		def GetFirstValue(self, node):
				if node.length > 0 and node[0].firstChild:
						return node[0].firstChild.nodeValue
				return ""
				
		def HTTPPost(self, selector, fields, files = None):
			content_type = ''
			body = ''
			
			if files != None:
					content_type, body = self.EncodeMultipartFormdata(fields, files)
			else:
					content_type = "application/x-www-form-urlencoded"
					body = urlencode(fields)					
					
			h = None
			if self.protocol == "http":
				h = httpClient.HTTPConnection("grabz.it")
			else:
				h = httpClient.HTTPSConnection("grabz.it")
				
			h.putrequest('POST', selector)
			h.putheader('content-type', content_type)
			h.putheader('content-length', str(len(body)))
			h.endheaders()
			
			try:
					h.send(body)
			except TypeError:
					# python 3 needs it to be encoded
					h.send(body.encode())
					
			response = h.getresponse()
			self.CheckResponseHeader(response.status);
			return response.read()

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
				connection = urlopen(url)
				self.CheckResponseHeader(connection.getcode())
				return connection.read()
		
		
		def CheckResponseHeader(self, httpCode):
				if httpCode == 403:
						raise GrabzItException.GrabzItException('Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.', GrabzItException.GrabzItException.NETWORK_DDOS_ATTACK)
				elif httpCode >= 400:
						raise GrabzItException.GrabzItException('A network error occured when connecting to the GrabzIt servers.', GrabzItException.GrabzItException.NETWORK_GENERAL_ERROR)

		def CreateSignature(self, value):
				md5 = hashlib.md5()
				md5.update(value.encode('ascii', 'replace'))
				return md5.hexdigest()
				
		def ReadFile(self, path):
				try:					
						return open(path, 'rb').read()
				except:
						raise GrabzItException.GrabzItException("File: " + path + " does not exist", GrabzItException.GrabzItException.FILE_NON_EXISTANT_PATH)
		
