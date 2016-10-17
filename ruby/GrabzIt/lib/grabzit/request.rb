module GrabzIt
	# @version 3.0
	# @author GrabzIt
	# @!visibility private
	class Request
		def initialize(url, isPost, options, data = nil)
			@url = url
			@isPost = isPost
			@options = options
			@data = data
		end	
		
		def url
			@url
		end
		
		def isPost
			@isPost
		end
		
		def options
			@options
		end
		
		def data
			@data
		end
		
		def getTargetUrl
			if (@isPost)
				return nil
			end
			return @data		
		end
	end
end