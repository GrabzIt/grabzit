#!/usr/bin/python

import os

try:
	from urllib.parse import urlencode	
	from urllib.request import urlopen
	from urllib.parse import urlparse
	import http.client as httpClient
except ImportError:
	from urllib import urlopen
	from urlparse import urlparse	
	from urllib import urlencode	
	import httplib as httpClient

import hashlib	
import mimetypes
import io
from xml.dom import minidom
from time import sleep
from GrabzIt import GrabzItScrape
from GrabzIt import GrabzItScrapeHistory
from GrabzIt import GrabzItScrapeException

class GrabzItScrapeClient:

        WebServicesBaseURL = "http://api.grabz.it/services/scraper/"
        TrueString = "True"

        def __init__(self, applicationKey, applicationSecret):
                self.applicationKey = applicationKey
                self.applicationSecret = applicationSecret                                                      
        
        #
	#Get all of the users scrapes
	#
	#id - If specified, just returns that scrape that matches the id.
	#
	#This function returns a array of Scrape objects
        #
        def GetScrapes(self, id = ""):
                sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(id))
                qs = {"key":self.applicationKey, "identifier":id}
                encoded_qs = urlencode(qs)
                
                encoded_qs += "&sig="+sig

                dom = minidom.parseString(self.HTTPGet(self.WebServicesBaseURL + "getscrapes.ashx?" + encoded_qs))

                self.CheckForException(dom)
                        
                results = []

                nodes = dom.getElementsByTagName("Scrape")
                
                for node in nodes:
                        identifier = node.getElementsByTagName('Identifier')
                        name = node.getElementsByTagName('Name')
                        status = node.getElementsByTagName('Status')
                        nextRun = node.getElementsByTagName('NextRun')
                        
                        scrapeResults = []
                        resultNodes = node.getElementsByTagName('Result')
                        
                        for resultNode in resultNodes:
                            resultIdentifier = resultNode.getElementsByTagName('Identifier')
                            resultFinished = resultNode.getElementsByTagName('Finished')
                            scrapeResults.append(GrabzItScrapeHistory.GrabzItScrapeHistory(self.GetFirstValue(resultIdentifier), self.GetFirstValue(resultFinished)))
                            
                        results.append(GrabzItScrape.GrabzItScrape(self.GetFirstValue(identifier), self.GetFirstValue(name), self.GetFirstValue(status), self.GetFirstValue(nextRun), scrapeResults))                                                                

                return results
                
        #
        #Sets the status of a scrape. 
        #
        #id - The id of the scrape to set.
        #status - The scrape status to set the scrape to. Options include Start, Stop, Enable and Disable
        #
        #This function returns true if the scrape was successfully set.
        #
        def SetScrapeStatus(self, id, status):
                sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(id)+"|"+str(status))        

                qs = {"key":self.applicationKey, "id":id, "status":status}

                encoded_qs = urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                return self.IsSuccessful(self.HTTPGet(self.WebServicesBaseURL + "setscrapestatus.ashx?" + encoded_qs))                               
                
        #
        #Re-sends the scrape result with the matching scrape id and result id using the export parameters stored in the scrape. 
        #
        #id - The id of the scrape that contains the result to re-send.
        #resultId - The id of the result to re-send.
        #
        #This function returns true if the result was successfully requested to be re-sent.
        #
        def SendResult(self, id, resultId):
                sig =  self.CreateSignature(str(self.applicationSecret)+"|"+str(id)+"|"+str(resultId))        

                qs = {"key":self.applicationKey, "id":id, "spiderId":resultId}

                encoded_qs = urlencode(qs)
                
                encoded_qs += "&sig="+sig;

                return self.IsSuccessful(self.HTTPGet(self.WebServicesBaseURL + "sendscrape.ashx?" + encoded_qs))                
                
        
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
                        raise GrabzItScrapeException.GrabzItScrapeException(message, code)          
       
                
        def GetFirstValue(self, node):
                if node.length > 0 and node[0].firstChild:
                        return node[0].firstChild.nodeValue
                return ""
        
        def HTTPGet(self, url):
                connection = urlopen(url)
                self.CheckResponseHeader(connection.getcode())
                return connection.read()
                
        def CheckResponseHeader(self, httpCode):
                if httpCode == 403:
                        raise GrabzItScrapeException.GrabzItScrapeException('Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.', GrabzItException.GrabzItException.NETWORK_DDOS_ATTACK)
                elif httpCode >= 400:
                        raise GrabzItScrapeException.GrabzItScrapeException('A network error occured when connecting to the GrabzIt servers.', GrabzItException.GrabzItException.NETWORK_GENERAL_ERROR)

        def CreateSignature(self, value):
                md5 = hashlib.md5()
                md5.update(value.encode('ascii', 'replace'))
                return md5.hexdigest()