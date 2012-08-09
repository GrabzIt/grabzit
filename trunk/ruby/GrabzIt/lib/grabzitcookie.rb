class GrabzItCookie
	def initialize
		@Name = ''
		@Value = ''
		@Domain = ''
		@Path = ''
		@HttpOnly = false
		@Expires = nil		
		@Type = nil	
	end
	def name
		@Name
	end
	def name=(value)
		@Name = value
	end  
	def value
		@Value
	end
	def value=(value)
		@Value = value
	end  
	def domain
		@Domain
	end
	def domain=(value)
		@Domain = value
	end  
	def path
		@Path
	end
	def path=(value)
		@Path = value
	end  
	def httpOnly
		@HttpOnly
	end
	def httpOnly=(value)
		@HttpOnly = value
	end  
	def expires
		@Expires
	end
	def expires=(value)
		@Expires = value
	end  	
	def type
		@Type
	end
	def type=(value)
		@Type = value
	end  	
end
