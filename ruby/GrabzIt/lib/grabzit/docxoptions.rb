module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating a DOCX
	# @version 3.1
	# @author GrabzIt
	class DOCXOptions < BaseOptions
		def initialize()
			super()
			@includeBackground = true
			@pagesize = 'A4'
			@orientation = 'Portrait'
			@includeLinks = true
			@includeImages = true
			@title = nil
			@marginTop = 10
			@marginLeft = 10
			@marginBottom = 10
			@marginRight = 10
			@requestAs = 0
			@quality = -1
			@hideElement = nil
			@waitForElement = nil
			@noAds = false
			@browserWidth = nil
			@templateVariables = nil
			@width = nil
			@height = nil
			@templateId = nil
			@targetElement = nil
			@mergeId = nil
			@noCookieNotifications = false
			@address = nil
			@password = nil
		end
		
		# @return [Boolean] true if the background images of the web page should be included in the DOCX
		def includeBackground
			@includeBackground
		end
		
		# Set to true if the background images of the web page should be included in the DOCX
		#
		# @param value [Boolean] include background images
		# @return [void]		
		def includeBackground=(value)
			@includeBackground = value
		end

		# @return [String] the page size of the DOCX to be returned
		def pagesize
			@pagesize
		end
		
		# Set the page size of the DOCX to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
		#
		# @param value [String] page size
		# @return [void]		
		def pagesize=(value)
			value = GrabzIt::Utility.nil_check(value).upcase
			@pagesize = value
		end		
		
		# @return [String] the orientation of the DOCX to be returned
		def orientation
			@orientation
		end
		
		# Set the orientation of the DOCX to be returned: 'Landscape' or 'Portrait'
		#
		# @param value [String] page orientation
		# @return [void]		
		def orientation=(value)
			value = GrabzIt::Utility.nil_check(value).capitalize
			@orientation = value
		end		
		
		# @return [Boolean] true if the links should be included in the DOCX
		def includeLinks
			@includeLinks
		end
		
		# Set to true if links should be included in the DOCX
		#
		# @param value [Boolean] include links
		# @return [void]		
		def includeLinks=(value)
			@includeLinks = value
		end		
		
		# @return [Boolean] true if web page images should be included
		def includeImages
			@includeImages
		end
		
		# Set to true if web page images should be included
		#
		# @param value [Boolean] include images
		# @return [void]		
		def includeImages=(value)
			@includeImages = value
		end	
		
		# @return [String] a title for the DOCX document
		def title
			@title
		end
		
		# Set a title for the DOCX document
		#
		# @param value [String] DOCX title
		# @return [void]		
		def title=(value)
			@title = value
		end	

		# @return [Integer] the margin that should appear at the top of the DOCX document page
		def marginTop
			@marginTop
		end
		
		# Set the margin that should appear at the top of the DOCX document page
		#
		# @param value [Integer] margin top
		# @return [void]		
		def marginTop=(value)
			@marginTop = value
		end		
		
		# @return [Integer] the margin that should appear at the left of the DOCX document page
		def marginLeft
			@marginLeft
		end
		
		# Set the margin that should appear at the left of the DOCX document page
		#
		# @param value [Integer] margin left
		# @return [void]		
		def marginLeft=(value)
			@marginLeft = value
		end	

		# @return [Integer] the margin that should appear at the bottom of the DOCX document page
		def marginBottom
			@marginBottom
		end
		
		# Set the margin that should appear at the bottom of the DOCX document page
		#
		# @param value [Integer] margin bottom
		# @return [void]		
		def marginBottom=(value)
			@marginBottom = value
		end			
		
		# @return [Integer] the margin that should appear at the right of the DOCX document
		def marginRight
			@marginRight
		end
		
		# Set the margin that should appear at the right of the DOCX document
		#
		# @param value [Integer] margin right
		# @return [void]		
		def marginRight=(value)
			@marginRight = value
		end		
		
		# @return [Integer] the width of the browser in pixels
		def browserWidth
			@browserWidth
		end
		
		# Set the width of the browser in pixels
		#
		# @param value [Integer] the browser width
		# @return [void]		
		def browserWidth=(value)
			@browserWidth = value
		end	
		
		# @return [Integer] get the page width of the resulting DOCX in mm.
		def pageWidth
			@width
		end
		
		# Set the page width of the resulting DOCX in mm
		#
		# @param value [Integer] the width
		# @return [void]		
		def pageWidth=(value)
			@width = value
		end			
		
		# @return [Integer] get the page height of the resulting DOCX in mm
		def pageHeight
			@height
		end
		
		# Set the page height of the resulting DOCX in mm
		#
		# @param value [Integer] the height
		# @return [void]		
		def pageHeight=(value)
			@height = value
		end		

		# @return [Integer] the number of milliseconds to wait before creating the capture
		def delay
			@delay
		end
		
		# Set the number of milliseconds to wait before creating the capture
		#
		# @param value [Integer] delay
		# @return [void]		
		def delay=(value)
			@delay = value
		end	

		# @return [Integer] get which user agent type should be used
		def requestAs
			@requestAs
		end
		
		# Set which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
		#
		# @param value [Integer] the browser type
		# @return [void]		
		def requestAs=(value)
			@requestAs = value
		end
		
		# @return [String] the template ID that specifies the header and footer of the DOCX document
		def templateId
			@templateId
		end
		
		# Set a template ID that specifies the header and footer of the DOCX document
		#
		# @param value [String] template id
		# @return [void]		
		def templateId=(value)
			@templateId = value
		end		
		
		# @return [Integer] the quality of the DOCX.
		def quality
			@quality
		end
		
		# Set the quality of the DOCX where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
		#
		# @param value [Integer] quality
		# @return [void]		
		def quality=(value)
			@quality = value
		end
		
		# @return [String] get the CSS selector of the only HTML element in the web page to capture
		def targetElement
			@targetElement
		end
		
		# Set the CSS selector of the only HTML element in the web page to capture
		#
		# @param value [String] the target element
		# @return [void]		
		def targetElement=(value)
			@targetElement = value
		end
		
		# @return [String] get the CSS selector(s) of the one or more HTML elements in the web page to hide
		def hideElement
			@hideElement
		end
		
		# Set the CSS selector(s) of the one or more HTML elements in the web page to hide
		#
		# @param value [String] the element(s) to hide
		# @return [void]		
		def hideElement=(value)
			@hideElement = value
		end
		
		# @return [String] get the CSS selector of the HTML element in the web page that must be visible before the capture is performed
		def waitForElement
			@waitForElement
		end
		
		# Set the CSS selector of the HTML element in the web page that must be visible before the capture is performed
		#
		# @param value [String] the element to wait for
		# @return [void]		
		def waitForElement=(value)
			@waitForElement = value
		end

		# @return [String] get the ID of a capture that should be merged at the beginning of the new DOCX document
		def mergeId
			@mergeId
		end
		
		# Set the ID of a capture that should be merged at the beginning of the new DOCX document
		#
		# @param value [String] the merge ID
		# @return [void]		
		def mergeId=(value)
			@mergeId = value
		end
		
		# @return [Boolean] get if adverts should be automatically hidden
		def noAds
			@noAds
		end
		
		# Set to true if adverts should be automatically hidden
		#
		# @param value [Boolean] hide adverts
		# @return [void]		
		def noAds=(value)
			@noAds = value
		end		
		

		# @return [Boolean] get if cookie notifications should be automatically hidden
		def noCookieNotifications
			@noCookieNotifications
		end
		
		# Set to true if cookie notifications should be automatically hidden
		#
		# @param value [Boolean] hide cookie notifications
		# @return [void]		
		def noCookieNotifications=(value)
			@noCookieNotifications = value
		end

		# @return [String] get the URL to execute the HTML code in
		def address
			@address
		end
		
		# Set the URL to execute the HTML code in
		#
		# @param value [String] the address
		# @return [void]		
		def address=(value)
			@address = value
		end	
		
		# @return [String] get the password that protects the DOCX document
		def password
			@password
		end
		
		# Set the password that protects the DOCX document
		#
		# @param value [String] the password
		# @return [void]		
		def password=(value)
			@password = value
		end

		# Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
		# GrabzIt to perform a HTTP post.
		#
		# @param name [String] the name of the HTTP Post parameter
		# @param value [String] the value of the HTTP Post parameter
		def add_post_parameter(name, value)
			@post = appendParameter(@post, name, value)
		end
		
		# Define a custom template parameter and value, this method can be called multiple times to add multiple parameters.
		#
		# @param name [String] the name of the template parameter
		# @param value [String] the value of the template parameter
		def add_template_parameter(name, value)
			@templateVariables = appendParameter(@templateVariables, name, value)
		end
		
		# @!visibility private
		def _getSignatureString(applicationSecret, callBackURL, url = nil)
			items = [applicationSecret]
			
			if(url != nil)
				items.push(GrabzIt::Utility.nil_check(url))
			end
			
			items.push(GrabzIt::Utility.nil_check(callBackURL),GrabzIt::Utility.nil_check(@customId),GrabzIt::Utility.b_to_str(@includeBackground),@pagesize ,@orientation,GrabzIt::Utility.b_to_str(@includeImages),GrabzIt::Utility.b_to_str(@includeLinks),GrabzIt::Utility.nil_check(@title),GrabzIt::Utility.nil_int_check(@marginTop),GrabzIt::Utility.nil_int_check(@marginLeft),GrabzIt::Utility.nil_int_check(@marginBottom),GrabzIt::Utility.nil_int_check(@marginRight),GrabzIt::Utility.nil_int_check(@delay),GrabzIt::Utility.nil_int_check(@requestAs),GrabzIt::Utility.nil_check(@country),GrabzIt::Utility.nil_int_check(@quality),GrabzIt::Utility.nil_check(@hideElement),GrabzIt::Utility.nil_check(@exportURL),GrabzIt::Utility.nil_check(@waitForElement),GrabzIt::Utility.nil_check(@encryptionKey),GrabzIt::Utility.b_to_str(@noAds),GrabzIt::Utility.nil_check(@post),GrabzIt::Utility.nil_check(@targetElement),GrabzIt::Utility.nil_check(@templateId),GrabzIt::Utility.nil_check(@templateVariables),GrabzIt::Utility.nil_int_check(@height),GrabzIt::Utility.nil_int_check(@width),GrabzIt::Utility.nil_int_check(@browserWidth),GrabzIt::Utility.nil_check(@proxy),GrabzIt::Utility.nil_check(@mergeId),GrabzIt::Utility.nil_check(@address),GrabzIt::Utility.b_to_str(@noCookieNotifications),GrabzIt::Utility.nil_check(@password))
			return items.join("|")
		end
		
		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)		
			params['background'] = GrabzIt::Utility.b_to_str(@includeBackground)
			params['pagesize'] = @pagesize
			params['orientation'] = @orientation
			params['includelinks'] = GrabzIt::Utility.b_to_str(@includeLinks)
			params['includeimages'] = GrabzIt::Utility.b_to_str(@includeImages)
			params['title'] = GrabzIt::Utility.nil_check(@title)
			params['mleft'] = GrabzIt::Utility.nil_int_check(@marginLeft)
			params['mright'] = GrabzIt::Utility.nil_int_check(@marginRight)
			params['mtop'] = GrabzIt::Utility.nil_int_check(@marginTop)
			params['mbottom'] = GrabzIt::Utility.nil_int_check(@marginBottom)
			params['delay'] = GrabzIt::Utility.nil_int_check(@delay)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['quality'] = GrabzIt::Utility.nil_int_check(@quality)
			params['hide'] = GrabzIt::Utility.nil_check(@hideElement)
			params['waitfor'] = GrabzIt::Utility.nil_check(@waitForElement)
			params['noads'] = GrabzIt::Utility.b_to_str(@noAds)
			params['post'] = GrabzIt::Utility.nil_check(@post)
			params['templateid'] = GrabzIt::Utility.nil_check(@templateId)
			params['target'] = GrabzIt::Utility.nil_check(@targetElement)
			params['bwidth'] = GrabzIt::Utility.nil_int_check(@browserWidth)
			params['width'] = GrabzIt::Utility.nil_int_check(@width)
			params['height'] = GrabzIt::Utility.nil_int_check(@height)
			params['tvars'] = GrabzIt::Utility.nil_check(@templateVariables)
			params['mergeid'] = GrabzIt::Utility.nil_check(@mergeId)
			params['address'] = GrabzIt::Utility.nil_check(@address)
			params['nonotify'] = GrabzIt::Utility.b_to_str(@noCookieNotifications)
			params['password'] = GrabzIt::Utility.nil_check(@password)
			
			return params;
		end		
	end
end