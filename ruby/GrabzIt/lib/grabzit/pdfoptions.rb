module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating a PDF
	# @version 3.0
	# @author GrabzIt
	class PDFOptions < BaseOptions
		def initialize()
			@includeBackground = true
			@pagesize = 'A4'
			@orientation = 'Portrait'
			@includeLinks = true
			@includeOutline = false
			@title = nil
			@coverURL = nil
			@marginTop = 10
			@marginLeft = 10
			@marginBottom = 10
			@marginRight = 10
			@requestAs = 0
			@templateId = nil
			@customWaterMarkId = nil
			@quality = -1
			@targetElement = nil
			@hideElement = nil
			@waitForElement = nil
		end
		
		# @return [Boolean] true if the background of the web page should be included in the PDF
		def includeBackground
			@includeBackground
		end
		
		# Set to true if the background of the web page should be included in the PDF
		#
		# @param value [Boolean] include background
		# @return [void]		
		def includeBackground(value)
			@includeBackground = value
		end

		# @return [String] the page size of the PDF to be returned
		def pagesize
			@pagesize
		end
		
		# Set the page size of the PDF to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
		#
		# @param value [String] page size
		# @return [void]		
		def pagesize(value)
			value = GrabzIt::Utility.nil_check(value).upcase
			@pagesize = value
		end		
		
		# @return [String] the orientation of the PDF to be returned
		def orientation
			@orientation
		end
		
		# Set the orientation of the PDF to be returned: 'Landscape' or 'Portrait'
		#
		# @param value [String] page orientation
		# @return [void]		
		def orientation(value)
			value = GrabzIt::Utility.nil_check(value).capitalize
			@orientation = value
		end		
		
		# @return [Boolean] true if the links should be included in the PDF
		def includeLinks
			@includeLinks
		end
		
		# Set to true if links should be included in the PDF
		#
		# @param value [Boolean] include links
		# @return [void]		
		def includeLinks(value)
			@includeLinks = value
		end		
		
		# @return [Boolean] true if the PDF outline should be included
		def includeOutline
			@includeOutline
		end
		
		# Set to true if the PDF outline should be included
		#
		# @param value [Boolean] include links
		# @return [void]		
		def includeOutline(value)
			@includeOutline = value
		end			
		
		# @return [String] a title for the PDF document
		def title
			@title
		end
		
		# Set a title for the PDF document
		#
		# @param value [String] PDF title
		# @return [void]		
		def title(value)
			@title = value
		end	

		# @return [String] the URL of a web page that should be used as a cover page for the PDF
		def coverURL
			@coverURL
		end
		
		# Set the URL of a web page that should be used as a cover page for the PDF
		#
		# @param value [String] cover URL
		# @return [void]		
		def coverURL(value)
			@coverURL = value
		end

		# @return [Integer] the margin that should appear at the top of the PDF document page
		def marginTop
			@marginTop
		end
		
		# Set the margin that should appear at the top of the PDF document page
		#
		# @param value [Integer] margin top
		# @return [void]		
		def marginTop(value)
			@marginTop = value
		end		
		
		# @return [Integer] the margin that should appear at the left of the PDF document page
		def marginLeft
			@marginLeft
		end
		
		# Set the margin that should appear at the left of the PDF document page
		#
		# @param value [Integer] margin left
		# @return [void]		
		def marginLeft(value)
			@marginLeft = value
		end	

		# @return [Integer] the margin that should appear at the bottom of the PDF document page
		def marginBottom
			@marginBottom
		end
		
		# Set the margin that should appear at the bottom of the PDF document page
		#
		# @param value [Integer] margin bottom
		# @return [void]		
		def marginBottom(value)
			@marginBottom = value
		end			
		
		# @return [Integer] the margin that should appear at the right of the PDF document
		def marginRight
			@marginRight
		end
		
		# Set the margin that should appear at the right of the PDF document
		#
		# @param value [Integer] margin right
		# @return [void]		
		def marginRight(value)
			@marginRight = value
		end		
		
		# @return [Integer] the number of milliseconds to wait before creating the capture
		def delay
			@delay
		end
		
		# Set the number of milliseconds to wait before creating the capture
		#
		# @param value [Integer] delay
		# @return [void]		
		def delay(value)
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
		def requestAs(value)
			@requestAs = value
		end

		# @return [String] the PDF template ID that specifies the header and footer of the PDF document
		def templateId
			@templateId
		end
		
		# Set a PDF template ID that specifies the header and footer of the PDF document
		#
		# @param value [String] template id
		# @return [void]		
		def templateId(value)
			@templateId = value
		end		
		
		# @return [String] the custom watermark id.
		def customWaterMarkId
			@customWaterMarkId
		end
		
		# Set a custom watermark to add to the PDF.
		#
		# @param value [String] custom watermark id
		# @return [void]		
		def customWaterMarkId(value)
			@customWaterMarkId = value
		end					
		
		# @return [Integer] the quality of the PDF.
		def quality
			@quality
		end
		
		# Set the quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
		#
		# @param value [Integer] quality
		# @return [void]		
		def quality(value)
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
		def targetElement(value)
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
		def hideElement(value)
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
		def waitForElement(value)
			@waitForElement = value
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
			"|"+GrabzIt::Utility.nil_check(@customId)+"|"+GrabzIt::Utility.b_to_str(@includeBackground)+"|"+@pagesize +"|"+@orientation+"|"+
			GrabzIt::Utility.nil_check(@customWaterMarkId)+"|"+GrabzIt::Utility.b_to_str(@includeLinks)+"|"+GrabzIt::Utility.b_to_str(@includeOutline)+"|"+
			GrabzIt::Utility.nil_check(@title)+"|"+GrabzIt::Utility.nil_check(@coverURL)+"|"+GrabzIt::Utility.nil_int_check(@marginTop)+"|"+GrabzIt::Utility.nil_int_check(@marginLeft)+
			"|"+GrabzIt::Utility.nil_int_check(@marginBottom)+"|"+GrabzIt::Utility.nil_int_check(@marginRight)+"|"+GrabzIt::Utility.nil_int_check(@delay)+"|"+
			GrabzIt::Utility.nil_int_check(@requestAs)+"|"+GrabzIt::Utility.nil_check(@country)+"|"+GrabzIt::Utility.nil_int_check(@quality)+"|"+GrabzIt::Utility.nil_check(@templateId)+"|"+GrabzIt::Utility.nil_check(@hideElement)+"|"+GrabzIt::Utility.nil_check(@targetElement)+"|"+GrabzIt::Utility.nil_check(@exportURL)+"|"+GrabzIt::Utility.nil_check(@waitForElement)  
		end
		
		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)		
			params['background'] = GrabzIt::Utility.b_to_str(@includeBackground)
			params['pagesize'] = @pagesize
			params['orientation'] = @orientation
			params['templateid'] = GrabzIt::Utility.nil_check(@templateId)
			params['customwatermarkid'] = GrabzIt::Utility.nil_check(@customWaterMarkId)
			params['includelinks'] = GrabzIt::Utility.b_to_str(@includeLinks)
			params['includeoutline'] = GrabzIt::Utility.b_to_str(@includeOutline)
			params['title'] = GrabzIt::Utility.nil_check(@title)
			params['coverurl'] = GrabzIt::Utility.nil_check(@coverURL)
			params['mleft'] = GrabzIt::Utility.nil_int_check(@marginLeft)
			params['mright'] = GrabzIt::Utility.nil_int_check(@marginRight)
			params['mtop'] = GrabzIt::Utility.nil_int_check(@marginTop)
			params['mbottom'] = GrabzIt::Utility.nil_int_check(@marginBottom)
			params['delay'] = GrabzIt::Utility.nil_int_check(@delay)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)		
			params['quality'] = GrabzIt::Utility.nil_int_check(@quality)
			params['target'] = GrabzIt::Utility.nil_check(@targetElement)
			params['hide'] = GrabzIt::Utility.nil_check(@hideElement)
			params['waitfor'] = GrabzIt::Utility.nil_check(@waitForElement)
			
			return params;
		end		
	end
end