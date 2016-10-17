module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating an image
	# @version 3.0
	# @author GrabzIt
	class ImageOptions < BaseOptions
		def initialize()
			@browserWidth = nil
			@browserHeight = nil
			@width = nil
			@height = nil
			@format = nil
			@targetElement = nil
			@requestAs = 0
			@customWaterMarkId = nil
			@quality = -1
		end
		
		# @return [Integer] the width of the browser in pixels
		def browserWidth
			@browserWidth
		end
		
		# Set the width of the browser in pixels
		#
		# @param value [Integer] the browser width
		# @return [void]		
		def browserWidth(value)
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
		def browserHeight(value)
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
		def width(value)
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
		def height(value)
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
		def format(value)
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
		def delay(value)
			@delay = value
		end

		# @return [String] get the id of the only HTML element in the web page to turn into a screenshot
		def targetElement
			@targetElement
		end
		
		# Set the id of the only HTML element in the web page to turn into a screenshot
		#
		# @param value [String] the target element
		# @return [void]		
		def targetElement(value)
			@targetElement = value
		end		
		
		# @return [Integer] get which user agent type should be used
		def requestAs
			@requestAs
		end
		
		# Set which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
		#
		# @param value [Integer] the browser type
		# @return [void]		
		def requestAs(value)
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
		def customWaterMarkId(value)
			@customWaterMarkId = value
		end					
		
		# @return [Integer] the quality of the screenshot.
		def quality
			@quality
		end
		
		# Set the quality of the screenshot where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
		#
		# @param value [Integer] the custom identifier
		# @return [void]		
		def quality(value)
			@quality = value
		end	

		# @!visibility private
		def _getSignatureString(applicationSecret, callBackURL, url = nil)
			urlParam = ''
			if (url != nil)
				urlParam = GrabzIt::Utility.nil_check(url)+"|"
			end		
			
			callBackURLParam = ''
			if (callBackURL != nil)
				callBackURLParam = GrabzIt::Utility.nil_check(callBackURL)
			end					

			return applicationSecret+"|"+ urlParam + callBackURLParam +
			"|"+GrabzIt::Utility.nil_check(@format)+"|"+GrabzIt::Utility.nil_int_check(@height)+"|"+GrabzIt::Utility.nil_int_check(@width)+"|"+GrabzIt::Utility.nil_int_check(@browserHeight)+
			"|"+GrabzIt::Utility.nil_int_check(@browserWidth)+"|"+GrabzIt::Utility.nil_check(@customId)+"|"+GrabzIt::Utility.nil_int_check(@delay)+"|"+GrabzIt::Utility.nil_check(@targetElement)+
			"|"+GrabzIt::Utility.nil_check(@customWaterMarkId)+"|"+GrabzIt::Utility.nil_int_check(@requestAs)+"|"+GrabzIt::Utility.nil_check(@country)+"|"+GrabzIt::Utility.nil_int_check(@quality)	  
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
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['quality'] = GrabzIt::Utility.nil_int_check(@quality)
			
			return params		
		end
	end
end