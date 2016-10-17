module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when creating an animated GIF
	# @version 3.0
	# @author GrabzIt
	class AnimationOptions < BaseOptions
		def initialize()
			@width = 0
			@height = 0
			@start = 0
			@duration = 0
			@speed = 0
			@framesPerSecond = 0
			@repeat = 0
			@reverse = false
			@customWaterMarkId = nil
			@quality = -1
		end
		
		# @return [Integer] the width of the resulting animated GIF in pixels
		def width
			@width
		end
		
		# Set the width of the resulting animated GIF in pixels
		#
		# @param value [Integer] the width
		# @return [void]		
		def width(value)
			@width = value
		end	

		# @return [Integer] the height of the resulting animated GIF in pixels
		def height
			@height
		end
		
		# Set the height of the resulting animated GIF in pixels
		#
		# @param value [Integer] the height
		# @return [void]		
		def height(value)
			@height = value
		end		

		# @return [Integer] the starting position of the video that should be converted into an animated GIF
		def start
			@start
		end
		
		# Set the starting position of the video that should be converted into an animated GIF
		#
		# @param value [Integer] the second to start at
		# @return [void]		
		def start(value)
			@start = value
		end		
		
		# @return [Integer] the length in seconds of the video that should be converted into a animated GIF
		def duration
			@duration
		end
		
		# Set the length in seconds of the video that should be converted into a animated GIF
		#
		# @param value [Integer] the number of seconds
		# @return [void]		
		def duration(value)
			@duration = value
		end				
		
		# @return [Float] the speed of the animated GIF
		def speed
			@speed
		end
		
		# Set the speed of the animated GIF from 0.2 to 10 times the original speed
		#
		# @param value [Float] the speed
		# @return [void]		
		def speed(value)
			@speed = value
		end					
		
		# @return [Float] the number of frames per second that should be captured from the video
		def framesPerSecond
			@framesPerSecond
		end
		
		# Set the number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60
		#
		# @param value [Float] the number of frames per second
		# @return [void]		
		def framesPerSecond(value)
			@framesPerSecond = value
		end			
		
		# @return [Integer] the number of times to loop the animated GIF
		def repeat
			@repeat
		end
		
		# Set the number of times to loop the animated GIF. If 0 it will loop forever
		#
		# @param value [Integer] the number of times to loop
		# @return [void]		
		def repeat(value)
			@repeat = value
		end				
		
		# @return [Boolean] if the frames of the animated GIF should be reversed
		def reverse
			@reverse
		end
		
		# Set to true if the frames of the animated GIF should be reversed
		#
		# @param value [Boolean] reverse value
		# @return [void]		
		def reverse(value)
			@reverse = value
		end					
		
		# @return [String] the custom watermark id
		def customWaterMarkId
			@customWaterMarkId
		end
		
		# Set a custom watermark to add to the animated GIF
		#
		# @param value [String] custom watermark id
		# @return [void]		
		def customWaterMarkId(value)
			@customWaterMarkId = value
		end					
		
		# @return [Integer] the quality of the animated GIF
		def quality
			@quality
		end
		
		# Set the quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
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
			"|"+GrabzIt::Utility.nil_int_check(@height)+"|"+GrabzIt::Utility.nil_int_check(@width)+"|"+GrabzIt::Utility.nil_check(@customId)+"|"+
			GrabzIt::Utility.nil_float_check(@framesPerSecond)+"|"+GrabzIt::Utility.nil_float_check(@speed)+"|"+GrabzIt::Utility.nil_int_check(@duration)+
			"|"+GrabzIt::Utility.nil_int_check(@repeat)+"|"+GrabzIt::Utility.b_to_str(@reverse)+"|"+GrabzIt::Utility.nil_int_check(@start)+
			"|"+GrabzIt::Utility.nil_check(@customWaterMarkId)+"|"+GrabzIt::Utility.nil_check(@country)+"|"+GrabzIt::Utility.nil_int_check(@quality)	  
		end		

		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)	
			params['width'] = GrabzIt::Utility.nil_int_check(@width)
			params['height'] = GrabzIt::Utility.nil_int_check(@height)
			params['duration'] = GrabzIt::Utility.nil_int_check(@duration)
			params['speed'] = GrabzIt::Utility.nil_float_check(@speed)
			params['customwatermarkid'] = GrabzIt::Utility.nil_check(@customWaterMarkId)
			params['start'] = GrabzIt::Utility.nil_int_check(@start)
			params['fps'] = GrabzIt::Utility.nil_float_check(@framesPerSecond)
			params['repeat'] = GrabzIt::Utility.nil_int_check(@repeat)
			params['reverse'] = GrabzIt::Utility.b_to_str(@reverse)		
			params['quality'] = GrabzIt::Utility.nil_int_check(@quality)			
			
			return params
		end
	end
end