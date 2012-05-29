import md5
import urllib
import cStringIO
from xml.dom import minidom

class GrabzItClient:

	WebServicesBaseURL = "http://grabz.it/services/"

	def __init__(self, applicationKey, applicationSecret):
		self.applicationKey = applicationKey
		self.applicationSecret = applicationSecret
		
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
			
		
	def GetPicture(self, id):
		result = cStringIO.StringIO(urllib.urlopen(self.WebServicesBaseURL + "getpicture.ashx?id=" + id).read()).getvalue()
		
		if result == "":
			return None
		
		return result
