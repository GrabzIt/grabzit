module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating an video
	# @version 3.0
	# @author GrabzIt
	class VideoOptions < BaseOptions
		def initialize()
			super()
			@browserWidth = nil
			@browserHeight = nil
			@waitForElement = nil
			@requestAs = 0
			@customWaterMarkId = nil
			@noAds = false
			@noCookieNotifications = false
			@address = nil
			@clickElement = nil
			@start = 0
			@duration = 10
			@framesPerSecond = 0
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
		
		# @return [Integer] the starting time of the web page that should be converted into a video
		def start
			@start
		end
		
		# Set the starting time of the web page that should be converted into a video
		#
		# @param value [Integer] the second to start at
		# @return [void]		
		def start=(value)
			@start = value
		end			

		# @return [Integer] the length in seconds of the web page that should be converted into a video
		def duration
			@duration
		end
		
		# Set the length in seconds of the web page that should be converted into a video
		#
		# @param value [Integer] the number of seconds
		# @return [void]		
		def duration=(value)
			@duration = value
		end		
		
		# @return [Float] the number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10
		def framesPerSecond
			@framesPerSecond
		end
		
		# Set the number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10
		#
		# @param value [Float] the number of frames per second
		# @return [void]		
		def framesPerSecond=(value)
			@framesPerSecond = value
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
			
			items.push(GrabzIt::Utility.nil_check(callBackURL),
			GrabzIt::Utility.nil_int_check(@browserHeight),
			GrabzIt::Utility.nil_int_check(@browserWidth),
			GrabzIt::Utility.nil_check(@customId),
			GrabzIt::Utility.nil_check(@customWaterMarkId),
			GrabzIt::Utility.nil_int_check(@start),
			GrabzIt::Utility.nil_int_check(@requestAs),
			GrabzIt::Utility.nil_check(@country),
			GrabzIt::Utility.nil_check(@exportURL),	
			GrabzIt::Utility.nil_check(@waitForElement),
			GrabzIt::Utility.nil_check(@encryptionKey),
			GrabzIt::Utility.b_to_str(@noAds),
			GrabzIt::Utility.nil_check(@post),
			GrabzIt::Utility.nil_check(@proxy),
			GrabzIt::Utility.nil_check(@address),
			GrabzIt::Utility.b_to_str(@noCookieNotifications),
			GrabzIt::Utility.nil_check(@clickElement),
			GrabzIt::Utility.nil_float_check(@framesPerSecond),
			GrabzIt::Utility.nil_int_check(@duration))
			
			return items.join("|")
		end
	
		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)		
			params['bwidth'] = GrabzIt::Utility.nil_int_check(@browserWidth)
			params['customwatermarkid'] = GrabzIt::Utility.nil_check(@customWaterMarkId)
			params['bheight'] = GrabzIt::Utility.nil_int_check(@browserHeight)
			params['waitfor'] = GrabzIt::Utility.nil_check(@waitForElement)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['noads'] = GrabzIt::Utility.b_to_str(@noAds)
			params['post'] = GrabzIt::Utility.nil_check(@post)
			params['address'] = GrabzIt::Utility.nil_check(@address)
			params['nonotify'] = GrabzIt::Utility.b_to_str(@noCookieNotifications)
			params['click'] = GrabzIt::Utility.nil_check(@clickElement)
			params['start'] = GrabzIt::Utility.nil_int_check(@start)
			params['fps'] = GrabzIt::Utility.nil_float_check(@framesPerSecond)
			params['duration'] = GrabzIt::Utility.nil_int_check(@duration)

			return params
		end
	end
end