module GrabzIt
	# This class represents the screenshot status
	# @version 3.0
	# @author GrabzIt
	class ScreenShotStatus
		# @!visibility private
		def initialize(processing = false, cached = false, expired = false, message = '')
			@Processing = processing
			@Cached = cached
			@Expired = expired
			@Message = message
		end
		# @return [Boolean] if true the screenshot is still being processed
		def processing
			@Processing
		end
		# @return [Boolean] if true the screenshot has been cached
		def cached
			@Cached
		end
		# @return [Boolean] if true the screenshot has expired
		def expired
			@Expired
		end
		# @return [String] returns any error messages associated with the screenshot
		def message
			@Message
		end
	end
end
