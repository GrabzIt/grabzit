class ScreenShotStatus
	def initialize
		@Processing = false
		@Cached = false
		@Expired = false
		@Message = ''
	end
	def processing
		@Processing
	end
	def processing=(value)
		@Processing = value
	end  
	def cached
		@Cached
	end
	def cached=(value)
		@Cached = value
	end  
	def expired
		@Expired
	end
	def expired=(value)
		@Expired = value
	end  
	def message
		@Message
	end
	def message=(value)
		@Message = value
	end  	
end
