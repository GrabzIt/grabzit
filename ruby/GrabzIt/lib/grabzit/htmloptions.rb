module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating rendered HTML
	# @version 3.0
	# @author GrabzIt
	class HTMLOptions < BaseOptions
		def initialize()
			super()
			@browserWidth = nil
			@browserHeight = nil
			@waitForElement = nil
			@requestAs = 0
			@noAds = false
			@noCookieNotifications = false
			@address = nil
			@clickElement = nil
			@jsCode = nil
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
		
		# @return [Integer] the height of the browser in pixels
		def browserHeight
			@browserHeight
		end
		
		# Set the height of the browser in pixels. Use -1 to screenshot the whole web page
		#
		# @param value [Integer] the browser height
		# @return [void]		
		def browserHeight=(value)
			@browserHeight = value
		end

		# @return [Integer] get the number of milliseconds to wait before creating the capture
		def delay
			@delay
		end
		
		# Set the number of milliseconds to wait before creating the capture
		#
		# @param value [Integer] the delay
		# @return [void]		
		def delay=(value)
			@delay = value
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

		# @return [String] get the CSS selector of the HTML element in the web page that must clicked before the capture is performed
		def clickElement
			@clickElement
		end
		
		# Set the CSS selector of the HTML element in the web page that must clicked before the capture is performed
		#
		# @param value [String] the element to click
		# @return [void]		
		def clickElement=(value)
			@clickElement = value
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

		# @return [String] get the JavaScript code that will be execute in the web page before the capture is performed
		def jsCode
			@jsCode
		end
		
		# Set the JavaScript code that will be execute in the web page before the capture is performed
		#
		# @param value [String] the javascript to execute
		# @return [void]		
		def jsCode=(value)
			@jsCode = value
		end		
		
		# Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
		# GrabzIt to perform a HTTP post.
		#
		# @param name [String] the name of the HTTP Post parameter
		# @param value [String] the value of the HTTP Post parameter
		def add_post_parameter(name, value)
			@post = appendParameter(@post, name, value)
		end		
		
		# @!visibility private
		def _getSignatureString(applicationSecret, callBackURL, url = nil)
			items = [applicationSecret]
			
			if(url != nil)
				items.push(GrabzIt::Utility.nil_check(url))
			end
			
			items.push(GrabzIt::Utility.nil_check(callBackURL),GrabzIt::Utility.nil_int_check(@browserHeight),GrabzIt::Utility.nil_int_check(@browserWidth),GrabzIt::Utility.nil_check(@customId),GrabzIt::Utility.nil_int_check(@delay),GrabzIt::Utility.nil_int_check(@requestAs),GrabzIt::Utility.nil_check(@country),GrabzIt::Utility.nil_check(@exportURL),GrabzIt::Utility.nil_check(@waitForElement),GrabzIt::Utility.nil_check(@encryptionKey),GrabzIt::Utility.b_to_str(@noAds),GrabzIt::Utility.nil_check(@post),GrabzIt::Utility.nil_check(@proxy),GrabzIt::Utility.nil_check(@address),GrabzIt::Utility.b_to_str(@noCookieNotifications),GrabzIt::Utility.nil_check(@clickElement),GrabzIt::Utility.nil_check(@jsCode))
			
			return items.join("|")
		end
	
		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)		
			params['bwidth'] = GrabzIt::Utility.nil_int_check(@browserWidth)
			params['bheight'] = GrabzIt::Utility.nil_int_check(@browserHeight)
			params['delay'] = GrabzIt::Utility.nil_int_check(@delay)
			params['waitfor'] = GrabzIt::Utility.nil_check(@waitForElement)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['noads'] = GrabzIt::Utility.b_to_str(@noAds)
			params['post'] = GrabzIt::Utility.nil_check(@post)
			params['address'] = GrabzIt::Utility.nil_check(@address)
			params['nonotify'] = GrabzIt::Utility.b_to_str(@noCookieNotifications)
			params['click'] = GrabzIt::Utility.nil_check(@clickElement)
			params['jscode'] = GrabzIt::Utility.nil_check(@jsCode)
			
			return params
		end
	end
end