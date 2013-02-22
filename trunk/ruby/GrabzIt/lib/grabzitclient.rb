require 'digest/md5'
require 'net/http'
require 'rexml/document'
require 'cgi'
require File.join(File.dirname(__FILE__), 'screenshotstatus')
require File.join(File.dirname(__FILE__), 'grabzitcookie')

class GrabzItClient

	WebServicesBaseURL = "http://grabz.it/services/"
	TrueString = "True"


	def initialize(applicationKey, applicationSecret)
		@applicationKey = applicationKey
		@applicationSecret = applicationSecret
	end


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
	#format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
	#delay - The number of milliseconds to wait before taking the screenshot
	#targetElement - The id of the only HTML element in the web page to turn into a screenshot
	#
	#This function returns the unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.
	#
	def take_picture(url, callback = nil, customId = nil, browserWidth = nil, browserHeight = nil, width = nil, height = nil, format = nil, delay = nil, targetElement = nil)
		qs = "key=" + CGI.escape(@applicationKey) + "&url=" + CGI.escape(url) + "&width=" + nil_check(width) + "&height=" + nil_check(height) + "&format=" + nil_check(format) + "&bwidth=" + nil_check(browserWidth) + "&bheight=" + nil_check(browserHeight) + "&callback=" + CGI.escape(nil_check(callback)) + "&customid=" + CGI.escape(nil_check(customId)) + "&delay=" + nil_check(delay) + "&target=" + CGI.escape(nil_check(targetElement))
		sig =  Digest::MD5.hexdigest(@applicationSecret+"|"+url+"|"+nil_check(callback)+"|"+nil_check(format)+"|"+nil_check(height)+"|"+nil_check(width)+"|"+nil_check(browserHeight)+"|"+nil_check(browserWidth)+"|"+nil_check(customId)+"|"+nil_check(delay)+"|"+nil_check(targetElement))
		qs = qs + "&sig=" + sig

		result = get(WebServicesBaseURL + "takepicture.ashx?" + qs)

		doc = REXML::Document.new(result)

		message = doc.root.elements["Message"].text()
		id = doc.root.elements["ID"].text()

		if message != nil
			raise message
		end

		return id        
	end   

	#
	#This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.
	#
	#url - The URL that the screenshot should be made of
	#saveToFile - The file path that the screenshot should saved to: e.g. images/test.jpg
	#browserWidth - The width of the browser in pixels
	#browserHeight - The height of the browser in pixels
	#outputHeight - The height of the resulting thumbnail in pixels
	#outputWidth - The width of the resulting thumbnail in pixels
	#format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
	#delay - The number of milliseconds to wait before taking the screenshot
	#targetElement - The id of the only HTML element in the web page to turn into a screenshot
	#
	#This function returns the true if it is successfull otherwise it throws an exception.
	#
	def save_picture(url, saveToFile, browserWidth = nil, browserHeight = nil, width = nil, height = nil, format = nil, delay = nil, targetElement = nil)
		id = take_picture(url, nil, nil, browserWidth, browserHeight, width, height, format, delay, targetElement)

		#Wait for it to be ready.
		while true do
			status = get_status(id)

			if !status.cached && !status.processing
				raise "The screenshot did not complete with the error: " + status.Message
				break;
			elsif status.cached
				result = get_picture(id)
				if !result
					raise "The screenshot image could not be found on GrabzIt."
					break
				end
				
				screenshot = File.new(saveToFile, "wb")
				screenshot.write(result)
				screenshot.close				

				break
			end

			sleep(1)
		end

		return true
	end
	
	#
	#Get the current status of a GrabzIt screenshot
	#
	#id - The id of the screenshot
	#
	#This function returns a Status object representing the screenshot
	#
        def get_status(id)
                result = get(WebServicesBaseURL + "getstatus.ashx?id=" + id)

		doc = REXML::Document.new(result)

		processing = doc.root.elements["Processing"].text()
		cached = doc.root.elements["Cached"].text()
		expired = doc.root.elements["Expired"].text()
		message = doc.root.elements["Message"].text()

                status = ScreenShotStatus.new()
                status.processing = (processing == TrueString)
                status.cached = (cached == TrueString)
                status.expired = (expired == TrueString)
                status.message = message

                return status
        end	

	#
	#This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.
	#
	#id - The unique identifier of the screenshot, returned by the callback handler or the TakePicture method
	#
	#This function returns the screenshot
	#
	def get_picture(id)
		result = get(WebServicesBaseURL + "getpicture.ashx?id=" + id)

		if result == nil
			return nil
		end

		return result
	end
	
        #
        #Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
	#
        #domain - The domain to return cookies for.
	#
        #This function returns an array of cookies
        #
        def get_cookies(domain)
                sig =  Digest::MD5.hexdigest(@applicationSecret+"|"+domain)               

                qs = "key=" +CGI.escape(@applicationKey)+"&domain="+CGI.escape(domain)+"&sig="+sig

                result = get(WebServicesBaseURL + "getcookies.ashx?" + qs)

                doc = REXML::Document.new(result)
                
                message = doc.root.elements["Message"].text()

                if message != nil
                        raise message
                end

                cookies = Array.new

		xml_cookies = doc.elements.to_a("//WebResult/Cookies/Cookie")		
                xml_cookies.each do |cookie|                	
                        grabzItCookie = GrabzItCookie.new()
                        grabzItCookie.name = cookie.elements["Name"].text
                        grabzItCookie.value = cookie.elements["Value"].text
                        grabzItCookie.domain = cookie.elements["Domain"].text
                        grabzItCookie.path = cookie.elements["Path"].text
                        grabzItCookie.httpOnly = (cookie.elements["HttpOnly"].text == TrueString)
                        if cookie.elements["Expires"] != nil                        
                        	grabzItCookie.expires = cookie.elements["Expires"].text
                        end
                        grabzItCookie.type = cookie.elements["Type"].text

                        cookies << grabzItCookie
                end

                return cookies
        end

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
        def set_cookie(name, domain, value = "", path = "/", httponly = false, expires = "")
                sig =  Digest::MD5.hexdigest(@applicationSecret+"|"+name+"|"+domain+"|"+nil_check(value)+"|"+nil_check(path)+"|"+b_to_int(httponly).to_s+"|"+nil_check(expires)+"|0")

                qs = "key=" +CGI.escape(@applicationKey)+"&domain="+CGI.escape(domain)+"&name="+CGI.escape(name)+"&value="+CGI.escape(nil_check(value))+"&path="+CGI.escape(nil_check(path))+"&httponly="+b_to_int(httponly).to_s+"&expires="+nil_check(expires)+"&sig="+sig

                result = get(WebServicesBaseURL + "setcookie.ashx?" + qs)

                doc = REXML::Document.new(result)
                
                message = doc.root.elements["Message"].text()
                resultVal = doc.root.elements["Result"].text()

                if message != nil
                        raise message
                end

                return (resultVal == TrueString)
        end

        #
        #Delete a custom cookie or block a global cookie from being used.
	#
        #name - The name of the cookie to delete
        #domain - The website the cookie belongs to
	#
        #This function returns true if the cookie was successfully set.
        #
        def delete_cookie(name, domain)
                sig =  Digest::MD5.hexdigest(@applicationSecret+"|"+name+"|"+domain+"|1")

                qs = "key=" + CGI.escape(@applicationKey)+"&domain="+CGI.escape(domain)+"&name="+CGI.escape(name)+"&delete=1&sig="+sig

                result = get(WebServicesBaseURL + "setcookie.ashx?" + qs)

                doc = REXML::Document.new(result)
                
                message = doc.root.elements["Message"].text()
                resultVal = doc.root.elements["Result"].text()

                if message != nil
                        raise message
                end

                return (resultVal == TrueString)
        end
	

	private
	def get(url)
		Net::HTTP.get(URI.parse(url))
	end
	
	private
	def b_to_int(bValue)
		if bValue
			return 1
        	end
        	return 1
	end
	
	private
	def nil_check(param)
		if param == nil
			return ""
        	end
        	return param
	end	
end