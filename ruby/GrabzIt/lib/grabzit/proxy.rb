module GrabzIt
	require 'cgi'
	
	# This class represents the local proxy
	# @version 3.2
	# @author GrabzIt
	class Proxy
		# @!visibility private
		def initialize(host = nil, port = nil, username = nil, password = nil)
			@Host = host
			@Port = port
			@Username = username
			@Password = password
		end
		# @return [String] returns the host of the proxy
		def host
			@Host
		end
		# @return [String] returns the port of the proxy
		def port
			@Port
		end
		# @return [String] returns the username of the proxy
		def username
			if @Username == nil
				return @Username
			end
			return CGI.unescape(@Username)
		end
		# @return [String] [String] returns the password of the proxy
		def password
			if @Password == nil
				return @Password
			end
			return CGI.unescape(@Password)					
		end
	end
end
