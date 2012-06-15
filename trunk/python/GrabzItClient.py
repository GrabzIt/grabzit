import md5
import urllib
import cStringIO
from xml.dom import minidom
import GrabzItCookie
import ScreenShotStatus

class GrabzItClient:

	WebServicesBaseURL = "http://grabz.it/services/"
	TrueString = "True"

	def __init__(self, applicationKey, applicationSecret):
		self.applicationKey = applicationKey
		self.applicationSecret = applicationSecret
		
		
	#
	#This method calls the GrabzIt web service to take the screenshot.
	#
	#url - The URL that the screenshot should be made of
	#callback - The handler the GrabzIt web service should call after it has completed its work
	#browserWidth - The width of the browser in pixels
	#browserHeight - The height of the browser in pixels
	#outputHeight - The height of the resulting thumbnail in pixels
	#outputWidth - The width of the resulting thumbnail in pixels
	#customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
	#format - The format the screenshot should be in: bmp, gif, jpg, png
	#delay - The number of milliseconds to wait before taking the screenshot
	#
	#This function returns the unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.
	#		
	def TakePicture(self, url, callback = '', customId = '', browserWidth = 0, browserHeight = 0, width = 0, height = 0, format = '', delay = 0):	
		qs = {"key":self.applicationKey, "url":url, "width":width,"height":height,"format":format,"bwidth":browserWidth,"bheight":browserHeight,"callback":callback,"customid":customId,"delay":delay}
		encoded_qs = urllib.urlencode(qs)
		sig =  md5.new(self.applicationSecret+"|"+url+"|"+callback+"|"+format+"|"+str(height)+"|"+str(width)+"|"+str(browserHeight)+"|"+str(browserWidth)+"|"+customId+"|"+str(delay)).hexdigest()
		
		encoded_qs += "&sig="+sig;

		result = urllib.urlopen(self.WebServicesBaseURL + "takepicture.ashx?" + encoded_qs)
		
		dom = minidom.parse(result)
		
		message = ""
		id = ""
		
		messageNodes = dom.getElementsByTagName("Message")
		idNodes = dom.getElementsByTagName("ID")
		
		for messageNode in messageNodes:
			if messageNode.firstChild == None:
				break
			message = messageNode.firstChild.nodeValue
			break

		for idNode in idNodes:
			if idNode.firstChild == None:
				break
			id = idNode.firstChild.nodeValue
			break			
		
		if len(message) > 0:
			raise Exception(message)
			
		return id
		
		
	#
	#Get the current status of a GrabzIt screenshot
	#
	#id - The id of the screenshot
	#
	#This function returns a Status object representing the screenshot
	#
	def GetStatus(self, id):

		result = urllib.urlopen(self.WebServicesBaseURL + "getstatus.ashx?id=" + id)	
		
		dom = minidom.parse(result)
		
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
	#This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.
	#
	#id - The unique identifier of the screenshot, returned by the callback handler or the TakePicture method
	#
	#This function returns the screenshot
	#		
	def GetPicture(self, id):
		result = cStringIO.StringIO(urllib.urlopen(self.WebServicesBaseURL + "getpicture.ashx?id=" + id).read()).getvalue()
		
		if result == "":
			return None
		
		return result

	#
	#Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
	#
	#domain - The domain to return cookies for.
	#
	#This function returns an array of cookies
	#
	def GetCookies(self, domain):
		sig =  md5.new(self.applicationSecret+"|"+domain).hexdigest()
		qs = {"key":self.applicationKey, "domain":domain}
		encoded_qs = urllib.urlencode(qs)
		
		encoded_qs += "&sig="+sig;

		result = urllib.urlopen(self.WebServicesBaseURL + "getcookies.ashx?" + encoded_qs)

		dom = minidom.parse(result)

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
		sig =  md5.new(self.applicationSecret+"|"+name+"|"+domain+"|"+value+"|"+path+"|"+str(int(httponly))+"|"+expires+"|0").hexdigest()	

		qs = {"key":self.applicationKey, "domain":domain, "name":name, "value":value, "path":path, "httponly":int(httponly), "expires":expires}

		encoded_qs = urllib.urlencode(qs)
		
		encoded_qs += "&sig="+sig;

		result = urllib.urlopen(self.WebServicesBaseURL + "setcookie.ashx?" + encoded_qs)
		
		dom = minidom.parse(result)
		
		message = ""
		result = ""
		
		messageNodes = dom.getElementsByTagName("Message")
		
		for messageNode in messageNodes:
			if messageNode.firstChild == None:
				break
			message = messageNode.firstChild.nodeValue
			break		
			
		resultNodes = dom.getElementsByTagName("Result")
		
		for resultNode in resultNodes:
			if resultNode.firstChild == None:
				break
			result = resultNode.firstChild.nodeValue
			break					

		if len(message) > 0:
			raise Exception(message)		

		return (result == self.TrueString);
		

	#
	#Delete a custom cookie or block a global cookie from being used.
	#
	#name - The name of the cookie to delete
	#domain - The website the cookie belongs to
	#
	#This function returns true if the cookie was successfully set.
	#		
	def DeleteCookie(self, name, domain):
		sig =  md5.new(self.applicationSecret+"|"+name+"|"+domain+"|1").hexdigest()

		qs = {"key":self.applicationKey, "domain":domain, "name":name, "delete":1}

		encoded_qs = urllib.urlencode(qs)
		
		encoded_qs += "&sig="+sig;

		result = urllib.urlopen(self.WebServicesBaseURL + "setcookie.ashx?" + encoded_qs)
		
		dom = minidom.parse(result)
		
		message = ""
		result = ""
		
		messageNodes = dom.getElementsByTagName("Message")
		
		for messageNode in messageNodes:
			if messageNode.firstChild == None:
				break
			message = messageNode.firstChild.nodeValue
			break		
			
		resultNodes = dom.getElementsByTagName("Result")
		
		for resultNode in resultNodes:
			if resultNode.firstChild == None:
				break
			result = resultNode.firstChild.nodeValue
			break					

		if len(message) > 0:
			raise Exception(message)		

		return (result == self.TrueString);	
		
		
	def GetFirstValue(self, node):
		if node.length > 0:
			return node[0].firstChild.nodeValue
		return ""