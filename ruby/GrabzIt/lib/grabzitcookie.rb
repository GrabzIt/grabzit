# This class represents the cookies stored in GrabzIt
# @version 2.0
# @author GrabzIt
class GrabzItCookie
	# @api private
	def initialize(name = '', domain = '', value = '', path = '', httpOnly = false, expires = nil, type = nil)
		@Name = name
		@Value = value
		@Domain = domain
		@Path = path
		@HttpOnly = httpOnly
		@Expires = expires		
		@Type = type	
	end
	# @return [String] the name of the cookie
	def name
		@Name
	end
	# @return [String] the value of the cookie
	def value
		@Value
	end
	# @return [String] the domain of the cookie
	def domain
		@Domain
	end
	# @return [String] the path of the cookie
	def path
		@Path
	end
	# @return [Boolean] is the cookie httponly
	def httpOnly
		@HttpOnly
	end
	# @return [String] the date and time the cookie expires
	def expires
		@Expires
	end  	
	# @return [String] the type of cookie
	def type
		@Type
	end
end
