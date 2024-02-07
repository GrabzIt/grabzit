# The public REST API for https://grabz.it
# @example To include the GrabzIt module do the following
#    require 'grabzit'
module GrabzIt	
	require 'digest/md5'
	require 'net/http'
	require 'rexml/document'
	require 'base64'
	require 'cgi'
	require 'openssl'
	require 'uri'
	require File.join(File.dirname(__FILE__), 'utility')
	require File.join(File.dirname(__FILE__), 'screenshotstatus')
	require File.join(File.dirname(__FILE__), 'cookie')
	require File.join(File.dirname(__FILE__), 'watermark')
	require File.join(File.dirname(__FILE__), 'exception')
	require File.join(File.dirname(__FILE__), 'animationoptions')
	require File.join(File.dirname(__FILE__), 'imageoptions')
	require File.join(File.dirname(__FILE__), 'htmloptions')
	require File.join(File.dirname(__FILE__), 'tableoptions')
	require File.join(File.dirname(__FILE__), 'pdfoptions')
	require File.join(File.dirname(__FILE__), 'docxoptions')
	require File.join(File.dirname(__FILE__), 'proxy')
	
	# This client provides access to the GrabzIt web services
	# This API allows you to take screenshot of websites for free and convert them into images, PDF's and tables.
	# @example Example usage
	#    require 'grabzit'	
	#
	#    grabzItClient = GrabzIt::Client.new("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET")
	#    grabzItClient.url_to_image("http://www.google.com")
	#    grabzItClient.save("http://www.mysite.com/grabzit/handler")
	# @version 3.0
	# @author GrabzIt
	# @see https://grabz.it/api/ruby/ GrabzIt Ruby API
	class Client

		WebServicesBaseURL = "://api.grabz.it/services/"
		private_constant :WebServicesBaseURL	
		TakePicture = "takepicture"	
		private_constant :TakePicture
		TakeVideo = "takevideo"	
		private_constant :TakeVideo	
		TakeTable = "taketable"
		private_constant :TakeTable
		TakePDF = "takepdf"
		private_constant :TakePDF
		TakeDOCX = "takedocx"
		private_constant :TakeDOCX
		TakeHTML = "takehtml"
		private_constant :TakeHTML
		TrueString = "True"
		private_constant :TrueString
		
		# Create a new instance of the Client class in order to access the GrabzIt API.
		#
		# @param applicationKey [String] your application key
		# @param applicationSecret [String] your application secret
		# @see https://grabz.it/login/create/ You can get an application key and secret by registering for free with GrabzIt		
		def initialize(applicationKey, applicationSecret)
			@applicationKey = applicationKey
			@applicationSecret = applicationSecret
			@protocol = 'http'
			@proxy = Proxy.new()
		end

		# This method specifies the URL of the online video that should be converted into a animated GIF
		#
		# @param url [String] The URL of the video to convert into a animated GIF
		# @param options [AnimationOptions, nil] a instance of the AnimationOptions class that defines any special options to use when creating the animated GIF
		# @return [void]		
		def url_to_animation(url, options = nil)
		
			if options == nil
				options = AnimationOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + "takeanimation", false, options, url)
			return nil
		end

		# This method specifies the URL that should be converted into a image screenshot
		#
		# @param url [String] the URL to capture as a screenshot
		# @param options [ImageOptions, nil] a instance of the ImageOptions class that defines any special options to use when creating the screenshot
		# @return [void]		
		def url_to_image(url, options = nil)		

			if options == nil
				options = ImageOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakePicture, false, options, url)
			return nil
		end

		# This method specifies the HTML that should be converted into a image
		#
		# @param html [String] the HTML to convert into a image
		# @param options [ImageOptions, nil] a instance of the ImageOptions class that defines any special options to use when creating the image
		# @return [void]		
		def html_to_image(html, options = nil)		

			if options == nil
				options = ImageOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakePicture, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that should be converted into a image
		#
		# @param path [String] the file path of a HTML file to convert into a image
		# @param options [ImageOptions, nil] a instance of the ImageOptions class that defines any special options to use when creating the image
		# @return [void]		
		def file_to_image(path, options = nil)		
			html_to_image(read_file(path), options)
		end

		# This method specifies the URL that should be converted into a video
		#
		# @param url [String] the URL to capture as a video
		# @param options [VideoOptions, nil] a instance of the VideoOptions class that defines any special options to use when creating a video
		# @return [void]		
		def url_to_video(url, options = nil)		

			if options == nil
				options = VideoOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeVideo, false, options, url)
			return nil
		end

		# This method specifies the HTML that should be converted into a video
		#
		# @param html [String] the HTML to convert into a video
		# @param options [VideoOptions, nil] a instance of the VideoOptions class that defines any special options to use when creating a video
		# @return [void]		
		def html_to_video(html, options = nil)		

			if options == nil
				options = VideoOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeVideo, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that should be converted into a video
		#
		# @param path [String] the file path of a HTML file to convert into a video
		# @param options [VideoOptions, nil] a instance of the VideoOptions class that defines any special options to use when creating a video
		# @return [void]		
		def file_to_video(path, options = nil)		
			html_to_video(read_file(path), options)
		end	

		# This method specifies the URL that should be converted into rendered HTML
		#
		# @param url [String] the URL to capture as rendered HTML
		# @param options [HTMLOptions, nil] a instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML
		# @return [void]		
		def url_to_rendered_html(url, options = nil)		

			if options == nil
				options = HTMLOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeHTML, false, options, url)
			return nil
		end

		# This method specifies the HTML that should be converted into rendered HTML
		#
		# @param html [String] the HTML to convert into rendered HTML
		# @param options [HTMLOptions, nil] a instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML
		# @return [void]		
		def html_to_rendered_html(html, options = nil)		

			if options == nil
				options = HTMLOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeHTML, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that should be converted into rendered HTML
		#
		# @param path [String] the file path of a HTML file to convert into rendered HTML
		# @param options [HTMLOptions, nil] a instance of the HTMLOptions class that defines any special options to use when creating rendered HTML
		# @return [void]		
		def file_to_rendered_html(path, options = nil)		
			html_to_rendered_html(read_file(path), options)
		end
		
		# This method specifies the URL that the HTML tables should be extracted from
		#
		# @param url [String] the URL to extract HTML tables from
		# @param options [TableOptions, nil] a instance of the TableOptions class that defines any special options to use when converting the HTML table		
		# @return [void]
		def url_to_table(url, options = nil)
		
			if options == nil
				options = TableOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeTable, false, options, url)
			return nil			
		end	
		
		# This method specifies the HTML that the HTML tables should be extracted from
		#
		# @param html [String] the HTML to extract HTML tables from
		# @param options [TableOptions, nil] a instance of the TableOptions class that defines any special options to use when converting the HTML table
		# @return [void]		
		def html_to_table(html, options = nil)		

			if options == nil
				options = TableOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeTable, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that the HTML tables should be extracted from
		#
		# @param path [String] the file path of a HTML file to extract HTML tables from
		# @param options [TableOptions, nil] a instance of the TableOptions class that defines any special options to use when converting the HTML table
		# @return [void]		
		def file_to_table(path, options = nil)		
			html_to_table(read_file(path), options)
		end		
		
		# This method specifies the URL that should be converted into a PDF
		#
		# @param url [String] the URL to capture as a PDF
		# @param options [PDFOptions, nil] a instance of the PDFOptions class that defines any special options to use when creating the PDF		
		# @return [void]
		def url_to_pdf(url, options = nil)
		
			if options == nil
				options = PDFOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakePDF, false, options, url)
			return nil			
		end	
		
		# This method specifies the HTML that should be converted into a PDF
		#
		# @param html [String] the HTML to convert into a PDF
		# @param options [PDFOptions, nil] a instance of the PDFOptions class that defines any special options to use when creating the PDF
		# @return [void]		
		def html_to_pdf(html, options = nil)		

			if options == nil
				options = PDFOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakePDF, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that should be converted into a PDF
		#
		# @param path [String] the file path of a HTML file to convert into a PDF
		# @param options [PDFOptions, nil] a instance of the PDFOptions class that defines any special options to use when creating the PDF
		# @return [void]		
		def file_to_pdf(path, options = nil)		
			html_to_pdf(read_file(path), options)
		end		
		
		# This method specifies the URL that should be converted into a DOCX
		#
		# @param url [String] the URL to capture as a DOCX
		# @param options [DOCXOptions, nil] a instance of the DOCXOptions class that defines any special options to use when creating the DOCX		
		# @return [void]
		def url_to_docx(url, options = nil)
		
			if options == nil
				options = DOCXOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeDOCX, false, options, url)
			return nil			
		end	
		
		# This method specifies the HTML that should be converted into a DOCX
		#
		# @param html [String] the HTML to convert into a DOCX
		# @param options [DOCXOptions, nil] a instance of the DOCXOptions class that defines any special options to use when creating the DOCX
		# @return [void]		
		def html_to_docx(html, options = nil)		

			if options == nil
				options = DOCXOptions.new()
			end
		
			@request = Request.new(@protocol + WebServicesBaseURL + TakeDOCX, true, options, html)
			return nil
		end		

		# This method specifies a HTML file that should be converted into a DOCX
		#
		# @param path [String] the file path of a HTML file to convert into a DOCX
		# @param options [DOCXOptions, nil] a instance of the DOCXOptions class that defines any special options to use when creating the DOCX
		# @return [void]		
		def file_to_docx(path, options = nil)		
			html_to_docx(read_file(path), options)
		end			
		
		# Calls the GrabzIt web service to take the screenshot
		#
		# The handler will be passed a URL with the following query string parameters:
		# - message (is any error message associated with the screenshot)
		# - customId (is a custom id you may have specified in the [AnimationOptions], [ImageOptions], [PDFOptions] or [TableOptions] classes)
		# - id (is the unique id of the screenshot which can be used to retrieve the screenshot with the {#get_result} method)
		# - filename (is the filename of the screenshot)
		# - format (is the format of the screenshot)
		# @note This is the recommended method of saving a screenshot
		# @param callBackURL [String, nil] the handler the GrabzIt web service should call after it has completed its work
		# @return [String] the unique identifier of the screenshot. This can be used to get the screenshot with the {#get_result} method
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def save(callBackURL = nil)
			if @request == nil
				  raise GrabzItException.new("No parameters have been set.", GrabzItException::PARAMETER_MISSING_PARAMETERS)
			end
			
			sig = encode(@request.options()._getSignatureString(GrabzIt::Utility.nil_check(@applicationSecret), callBackURL, @request.getTargetUrl()))
			
			data = take(sig, callBackURL)
			
			if data == nil || data == ""
				data = take(sig, callBackURL)
			end

			if data == nil || data == ""
				raise GrabzItException.new("An unknown network error occurred, please try calling this method again.", GrabzItException::NETWORK_GENERAL_ERROR)
			end

			return get_result_value(data, "ID")
		end

		# Calls the GrabzIt web service to take the screenshot and saves it to the target path provided. if no target path is provided
		# it returns the screenshot byte data.
		#
		# @note Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
		# @param saveToFile [String, nil] the file path that the screenshot should saved to. 
		# @example Synchronously save the screenshot to test.jpg
		#   save_to('images/test.jpg')
		# @raise [RuntimeError] if the screenshot cannot be saved a RuntimeError will be raised that will contain an explanation
		# @return [Boolean] returns the true if it is successfully saved to a file, otherwise if a target path is not provided it returns the screenshot's byte data.
		def save_to(saveToFile = nil)
			id = save()

			if id == nil || id == ""
				return false
			end			
			
			#Wait for it to be possibly ready
			sleep((@request.options().startDelay() / 1000) + 3)

			#Wait for it to be ready.
			while true do
				status = get_status(id)

				if !status.cached && !status.processing
					raise GrabzItException.new("The capture did not complete with the error: " + status.message, GrabzItException::RENDERING_ERROR)
					break
				elsif status.cached
					result = get_result(id)
					if !result
						raise GrabzItException.new("The capture could not be found on GrabzIt.", GrabzItException::RENDERING_MISSING_SCREENSHOT)
						break
					end
					
					if saveToFile == nil || saveToFile == ""
						return result
					end					

					screenshot = File.new(saveToFile, "wb")
					screenshot.write(result)
					screenshot.close

					break
				end

				sleep(3)
			end

			return true
		end	

		# Get the current status of a GrabzIt screenshot
		#
		# @param id [String] the id of the screenshot
		# @return [ScreenShotStatus] a object representing the status of the screenshot
		def get_status(id)
			
			if id == nil || id == ""
				return nil
			end			
			
			result = get(@protocol + WebServicesBaseURL + "getstatus?id=" + GrabzIt::Utility.nil_check(id))

			doc = REXML::Document.new(result)

			processing = doc.root.elements["Processing"].text()
			cached = doc.root.elements["Cached"].text()
			expired = doc.root.elements["Expired"].text()
			message = doc.root.elements["Message"].text()

			return ScreenShotStatus.new((processing == TrueString), (cached == TrueString), (expired == TrueString), message)
		end	

		# This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet
		#
		# @param id [String] the id of the screenshot
		# @return [Object] returns the screenshot
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def get_result(id)
			if id == nil || id == ""
				return nil
			end
			
			return get(@protocol + WebServicesBaseURL + "getfile?id=" + GrabzIt::Utility.nil_check(id))
		end

		# Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well
		#
		# @param domain [String] the domain to return cookies for
		# @return [Array<Cookie>] an array of cookies
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def get_cookies(domain)
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(domain))               

			qs = "key="
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(@applicationKey)))
			qs.concat("&domain=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(domain)))
			qs.concat("&sig=")
			qs.concat(sig)

			result = get(@protocol + WebServicesBaseURL + "getcookies?" + qs)

			doc = REXML::Document.new(result)

			check_for_exception(doc)

			cookies = Array.new

			xml_cookies = doc.elements.to_a("//WebResult/Cookies/Cookie")		
			xml_cookies.each do |cookie|
				expires = nil
				if cookie.elements["Expires"] != nil                        
					expires = cookie.elements["Expires"].text
				end                
				grabzItCookie = GrabzIt::Cookie.new(cookie.elements["Name"].text, cookie.elements["Domain"].text, cookie.elements["Value"].text, cookie.elements["Path"].text, (cookie.elements["HttpOnly"].text == TrueString), expires, cookie.elements["Type"].text)
				cookies << grabzItCookie
			end

			return cookies
		end

		# Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
		#
		# cookie is overridden
		# @note This can be useful if a websites functionality is controlled by cookies
		# @param name [String] the name of the cookie to set
		# @param domain [String] the domain of the website to set the cookie for
		# @param value [String, ''] the value of the cookie
		# @param path [String, '/'] the website path the cookie relates to
		# @param httponly [Boolean, false] is the cookie only used on HTTP
		# @param expires [String, ''] when the cookie expires. Pass a nil value if it does not expire	
		# @return [Boolean] returns true if the cookie was successfully set
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def set_cookie(name, domain, value = "", path = "/", httponly = false, expires = "")
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(name)+"|"+GrabzIt::Utility.nil_check(domain)+
			"|"+GrabzIt::Utility.nil_check(value)+"|"+GrabzIt::Utility.nil_check(path)+"|"+GrabzIt::Utility.b_to_str(httponly)+
			"|"+GrabzIt::Utility.nil_check(expires)+"|0")

			qs = "key="
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(@applicationKey)))
			qs.concat("&domain=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(domain)))
			qs.concat("&name=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(name)))
			qs.concat("&value=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(value)))
			qs.concat("&path=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(path)))
			qs.concat("&httponly=")
			qs.concat(GrabzIt::Utility.b_to_str(httponly))
			qs.concat("&expires=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(expires)))
			qs.concat("&sig=")
			qs.concat(sig)

			return (get_result_value(get(@protocol + WebServicesBaseURL + "setcookie?" + qs), "Result") == TrueString)
		end

		# Delete a custom cookie or block a global cookie from being used
		#
		# @param name [String] the name of the cookie to delete
		# @param domain [String] the website the cookie belongs to
		# @return [Boolean] returns true if the cookie was successfully set
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def delete_cookie(name, domain)
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(name)+
			"|"+GrabzIt::Utility.nil_check(domain)+"|1")

			qs = "key="
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(@applicationKey)))
			qs.concat("&domain=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(domain)))
			qs.concat("&name=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(name)))
			qs.concat("&delete=1&sig=")
			qs.concat(sig)

			return (get_result_value(get(@protocol + WebServicesBaseURL + "setcookie?" + qs), "Result") == TrueString)
		end

		# Get your uploaded custom watermark
		#
		# @param identifier [String, nil] the identifier of a particular custom watermark you want to view
		# @return [WaterMark] the watermark with the specified identifier
		def get_watermark(identifier)
			watermarks = find_watermarks(identifier)
			if watermarks.length == 1
				return watermarks[0]
			end
			
			return nil
		end

		# Get your uploaded custom watermarks
		#
		# @return [Array<WaterMark>] an array of uploaded watermarks
		def get_watermarks()
			return find_watermarks(nil)
		end		

		# Add a new custom watermark
		#
		# @param identifier [String] the identifier you want to give the custom watermark. It is important that this identifier is unique.
		# @param path [String] the absolute path of the watermark on your server. For instance C:/watermark/1.png
		# @param xpos [Integer] the horizontal position you want the screenshot to appear at: Left = 0, Center = 1, Right = 2
		# @param ypos [Integer] the vertical position you want the screenshot to appear at: Top = 0, Middle = 1, Bottom = 2
		# @return [Boolean] returns true if the watermark was successfully set
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def add_watermark(identifier, path, xpos, ypos)
			if !File.file?(path)
				raise "File: " + path + " does not exist"
			end
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(identifier)+"|"+GrabzIt::Utility.nil_int_check(xpos)+
			"|"+GrabzIt::Utility.nil_int_check(ypos))

			boundary = '--------------------------'+Time.now.to_f.to_s

			url = @protocol + "://api.grabz.it/services/addwatermark"
			uri = URI.parse(url)

			file = File.open(path, "rb")
			data = file.read

			post_body = Array.new
			post_body << "\r\n--"+boundary+"\r\n"
			post_body << "Content-Disposition: form-data; name=\"watermark\"; filename=\""+File.basename(path)+"\"\r\nContent-Type: image/jpeg\r\n\r\n"
			post_body << data
			post_body << "\r\n--"+boundary+"\r\n"

			post_body << "Content-Disposition: form-data; name=\"key\"\r\n\r\n"
			post_body << GrabzIt::Utility.nil_check(@applicationKey)
			post_body << "\r\n--"+boundary+"\r\n"

			post_body << "Content-Disposition: form-data; name=\"identifier\"\r\n\r\n"
			post_body << GrabzIt::Utility.nil_check(identifier)
			post_body << "\r\n--"+boundary+"\r\n"

			post_body << "Content-Disposition: form-data; name=\"xpos\"\r\n\r\n"
			post_body << GrabzIt::Utility.nil_check(xpos)
			post_body << "\r\n--"+boundary+"\r\n"

			post_body << "Content-Disposition: form-data; name=\"ypos\"\r\n\r\n"
			post_body << GrabzIt::Utility.nil_check(ypos)
			post_body << "\r\n--"+boundary+"\r\n"

			post_body << "Content-Disposition: form-data; name=\"sig\"\r\n\r\n"
			post_body << sig
			post_body << "\r\n--"+boundary+"--\r\n"

			request = Net::HTTP::Post.new(url)
			request.content_type = "multipart/form-data, boundary="+boundary
			request.body = post_body.join
			
			caller = Net::HTTP.new(uri.host, uri.port)
			caller.use_ssl = uri.scheme == 'https'
			response = caller.start {|http| http.request(request)}	
			
			response_check(response)
			
			return (get_result_value(response.body(), "Result") == TrueString)		
		end

		# Delete a custom watermark
		#
		# @param identifier [String] the identifier of the custom watermark you want to delete
		# @return [Boolean] returns true if the watermark was successfully deleted
		# @raise [RuntimeError] if the GrabzIt service reports an error with the request it will be raised as a RuntimeError
		def delete_watermark(identifier)
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(identifier))

			qs = "key="
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(@applicationKey)))
			qs.concat("&identifier=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(identifier)))
			qs.concat("&sig=")
			qs.concat(sig)

			return (get_result_value(get(@protocol + WebServicesBaseURL + "deletewatermark?" + qs), "Result") == TrueString)
		end

		# This method sets if requests to GrabzIt's API should use SSL or not
		#
		# @param value [Boolean] true if should use SSL
		def use_ssl(value)
			if value
				@protocol = 'https'
			else
				@protocol = 'http'
			end
		end
	
		# This method enables a local proxy server to be used for all requests
		#
		# @param value [String] the URL, which can include a port if required, of the proxy. Providing a null will remove any previously set proxy
		def set_local_proxy(value)
			if value
				uri = URI.parse(value)
				@proxy = Proxy.new(uri.host, uri.port, uri.user, uri.password)
			else
				@proxy = Proxy.new()
			end
		end		
		
		# This method creates a cryptographically secure encryption key to pass to the encryption key parameter.
		#
		# @return [String] a encryption key
		def create_encryption_key()
			Base64.strict_encode64(OpenSSL::Random.random_bytes(32));
		end
		
		# This method will decrypt a encrypted capture file, using the key you passed to the encryption key parameter.
		#
		# @param path [String] the path of the encrypted capture
		# @param key [String] the encryption key
		def decrypt_file(path, key)
			data = read_file(path)
			decryptedFile = File.new(path, "wb")
			decryptedFile.write(decrypt(data, key))
			decryptedFile.close
		end

		# This method will decrypt a encrypted capture, using the key you passed to the encryption key parameter.
		#
		# @param path [String] the encrypted bytes
		# @param key [String] the encryption key
		# @return [Array<Byte>] an array of decrypted bytes
		def decrypt(data, key)
			if data == nil
				return nil
			end
			iv = data[0..15]
			payload = data[16..-1]
			cipher = OpenSSL::Cipher::Cipher.new("aes-256-cbc")
			cipher.padding = 0
			cipher.key = Base64.strict_decode64(key);
			cipher.iv = iv
			decrypted = cipher.update(payload);
			decrypted << cipher.final();
			return decrypted
		end
		
		private
		def take(sig, callBackURL)
			if !@request.isPost()
				return get(@request.url() + "?" + URI.encode_www_form(@request.options()._getParameters(@applicationKey, sig, callBackURL, "url", @request.data())))
			end
			
			return post(@request.url(), URI.encode_www_form(@request.options()._getParameters(@applicationKey, sig, callBackURL, "html", CGI.escape(@request.data()))))
		end		

		private
		def find_watermarks(identifier = nil)
			sig = encode(GrabzIt::Utility.nil_check(@applicationSecret)+"|"+GrabzIt::Utility.nil_check(identifier))               

			qs = "key="
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(@applicationKey)))
			qs.concat("&identifier=")
			qs.concat(CGI.escape(GrabzIt::Utility.nil_check(identifier)))
			qs.concat("&sig=")
			qs.concat(sig)

			result = get(@protocol + WebServicesBaseURL + "getwatermarks?" + qs)

			doc = REXML::Document.new(result)

			check_for_exception(doc)

			watermarks = Array.new

			xml_watemarks = doc.elements.to_a("//WebResult/WaterMarks/WaterMark")		
			xml_watemarks.each do |watemark|
				grabzItWaterMark = GrabzIt::WaterMark.new(watemark.elements["Identifier"].text, watemark.elements["XPosition"].text.to_i, watemark.elements["YPosition"].text.to_i, watemark.elements["Format"].text)
				watermarks << grabzItWaterMark
			end

			return watermarks
		end

		private
		def get(url)
			uri = URI.parse(url)
			response = Net::HTTP.start(uri.host, uri.port, @proxy.host(), @proxy.port(), @proxy.username(), @proxy.password(), :use_ssl => uri.scheme == 'https', :read_timeout => 600) { |http| http.get(uri.request_uri) }
			response_check(response)
			return response.body
		end
		
		private
		def post(url, params)
			headers = {'Content-Type' => 'application/x-www-form-urlencoded'}
			uri = URI.parse(url)
			response = Net::HTTP.start(uri.host, uri.port, @proxy.host(), @proxy.port(), @proxy.username(), @proxy.password(), :use_ssl => uri.scheme == 'https', :read_timeout => 600) { |http| http.post(uri.request_uri, params, headers) }
			response_check(response)
			return response.body
		end		

		private
		def response_check(response)
			statusCode = response.code.to_i
			if statusCode == 403
				raise GrabzItException.new(response.body, GrabzItException::NETWORK_DDOS_ATTACK)
			elsif statusCode >= 400
				raise GrabzItException.new("A network error occurred when connecting to GrabzIt.", GrabzItException::NETWORK_GENERAL_ERROR)
			end
		end
		
		private
		def check_for_exception(doc)
			if doc == nil
				return
			end
			
			message = doc.root.elements["Message"].text() rescue nil
			code  = doc.root.elements["Code"].text() rescue nil
			
			if message != nil
				raise GrabzItException.new(message, code)
			end			
		end

		private
		def encode(text)
			return Digest::MD5.hexdigest(text.encode('ascii', **{:invalid => :replace, :undef => :replace, :replace => '?'}))
		end

		private
		def read_file(path)
			if !File.file?(path)
				raise "File: " + path + " does not exist"
			end

			file = File.open(path, "rb")
			return file.read
		end
		
		private 
		def get_result_value(result, field)
			doc = REXML::Document.new(result)
			
			value = doc.root.elements[field].text() rescue nil

			check_for_exception(doc)

			return value
		end
	end
end
