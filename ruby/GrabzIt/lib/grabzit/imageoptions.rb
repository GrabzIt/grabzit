module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating an image
	# @version 3.0
	# @author GrabzIt
	class ImageOptions < BaseOptions
		def initialize()
			super()
			@browserWidth = nil
			@browserHeight = nil
			@width = nil
			@height = nil
			@format = nil
			@targetElement = nil
			@hideElement = nil
			@waitForElement = nil
			@requestAs = 0
			@customWaterMarkId = nil
			@quality = -1
			@transparent = false
			@noAds = false
			@noCookieNotifications = false
			@address = nil
			@hd = false
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
		
		# @return [Integer] get the width of the resulting screenshot in pixels.
		def width
			@width
		end
		
		# Set the width of the resulting screenshot in pixels. Use -1 to not reduce the width of the screenshot
		#
		# @param value [Integer] the width
		# @return [void]		
		def width=(value)
			@width = value
		end			
		
		# @return [Integer] get the height of the resulting screenshot in pixels
		def height
			@height
		end
		
		# Set the height of the resulting screenshot in pixels. Use -1 to not reduce the height of the screenshot
		#
		# @param value [Integer] the height
		# @return [void]		
		def height=(value)
			@height = value
		end

		# @return [String] get the format of the screenshot image
		def format
			@format
		end
		
		# Set the format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
		#
		# @param value [String] the format
		# @return [void]		
		def format=(value)
			@format = value
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
		
		# @return [String] the custom watermark id.
		def customWaterMarkId
			@customWaterMarkId
		end
		
		# Set a custom watermark to add to the screenshot.
		#
		# @param value [String] custom watermark id
		# @return [void]		
		def customWaterMarkId=(value)
			@customWaterMarkId = value
		end					
		
		# @return [Integer] the quality of the screenshot.
		def quality
			@quality
		end
		
		# Set the quality of the screenshot where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
		#
		# @param value [Integer] the quality
		# @return [void]		
		def quality=(value)
			@quality = value
		end	

		# @return [Boolean] true if the image capture should be transparent
		def transparent
			@transparent
		end
		
		# Set to true if the image capture should be transparent. This is only compatible with png and tiff images
		#
		# @param value [Boolean] true if the image should be transparent
		# @return [void]
		def transparent=(value)
			@transparent = value
		end
		
		# @return [Boolean] true if the image capture should be in high definition
		def hd
			@hd
		end
		
		# Set to true if the image capture should be in high definition.
		#
		# @param value [Boolean] true if the image should be high definition
		# @return [void]
		def hd=(value)
			@hd = value
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
			
			items.push(GrabzIt::Utility.nil_check(callBackURL),GrabzIt::Utility.nil_check(@format),GrabzIt::Utility.nil_int_check(@height),GrabzIt::Utility.nil_int_check(@width),GrabzIt::Utility.nil_int_check(@browserHeight),GrabzIt::Utility.nil_int_check(@browserWidth),GrabzIt::Utility.nil_check(@customId),GrabzIt::Utility.nil_int_check(@delay),GrabzIt::Utility.nil_check(@targetElement),GrabzIt::Utility.nil_check(@customWaterMarkId),GrabzIt::Utility.nil_int_check(@requestAs),GrabzIt::Utility.nil_check(@country),GrabzIt::Utility.nil_int_check(@quality),GrabzIt::Utility.nil_check(@hideElement),GrabzIt::Utility.nil_check(@exportURL),GrabzIt::Utility.nil_check(@waitForElement),GrabzIt::Utility.b_to_str(@transparent),GrabzIt::Utility.nil_check(@encryptionKey),GrabzIt::Utility.b_to_str(@noAds),GrabzIt::Utility.nil_check(@post),GrabzIt::Utility.nil_check(@proxy),GrabzIt::Utility.nil_check(@address),GrabzIt::Utility.b_to_str(@noCookieNotifications),GrabzIt::Utility.b_to_str(@hd),GrabzIt::Utility.nil_check(@clickElement),GrabzIt::Utility.nil_check(@jsCode))
			
			return items.join("|")
		end
	
		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)		
			params['width'] = GrabzIt::Utility.nil_int_check(@width)
			params['height'] = GrabzIt::Utility.nil_int_check(@height)
			params['format'] = GrabzIt::Utility.nil_check(@format)
			params['bwidth'] = GrabzIt::Utility.nil_int_check(@browserWidth)
			params['customwatermarkid'] = GrabzIt::Utility.nil_check(@customWaterMarkId)
			params['bheight'] = GrabzIt::Utility.nil_int_check(@browserHeight)
			params['delay'] = GrabzIt::Utility.nil_int_check(@delay)
			params['target'] = GrabzIt::Utility.nil_check(@targetElement)
			params['hide'] = GrabzIt::Utility.nil_check(@hideElement)
			params['waitfor'] = GrabzIt::Utility.nil_check(@waitForElement)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['quality'] = GrabzIt::Utility.nil_int_check(@quality)
			params['transparent'] = GrabzIt::Utility.b_to_str(@transparent)
			params['noads'] = GrabzIt::Utility.b_to_str(@noAds)
			params['post'] = GrabzIt::Utility.nil_check(@post)
			params['address'] = GrabzIt::Utility.nil_check(@address)
			params['nonotify'] = GrabzIt::Utility.b_to_str(@noCookieNotifications)
			params['hd'] = GrabzIt::Utility.b_to_str(@hd)
			params['click'] = GrabzIt::Utility.nil_check(@clickElement)
			params['jscode'] = GrabzIt::Utility.nil_check(@jsCode)
			
			return params
		end
	end
end