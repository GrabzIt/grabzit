module GrabzIt
	# @version 3.0
	# @author GrabzIt
	class BaseOptions
		# @!visibility private
		def initialize()
			@customId = nil
			@country = nil
			@exportURL = nil
			@encryptionKey = nil
			@delay = nil
			@post = nil
			@proxy = nil
		end	
		
		# @return [String] the custom identifier that you can pass through to the web service.
		def customId
			@customId
		end
		
		# Set a custom identifier to pass to the web service. This will be returned with the callback URL you have specified.
		#
		# @param value [String] the custom identifier
		# @return [void]
		def customId=(value)
			@customId = value
		end		
		
		# @return [String] the country the capture should be created from.
		def country
			@country
		end

		# Set the country the capture should be created from: Default = "", Singapore = "SG", UK = "UK", US = "US".
		#
		# @param value [String] the country to use
		# @return [void]
		def country=(value)
			@country = value
		end
		
		# @return [String] the export URL that should be used to transfer the capture to a third party location.
		def exportURL
			@exportURL
		end

		# Set the export URL that should be used to transfer the capture to a third party location.
		#
		# @param value [String] export URL to use
		# @return [void]
		def exportURL=(value)
			@exportURL = value
		end
		
		# @return [String] the encryption key that will be used to encrypt your capture.
		def encryptionKey
			@encryptionKey
		end

		# Set the encryption key that will be used to encrypt your capture.
		#
		# @param value [String] encryption key to use
		# @return [void]
		def encryptionKey=(value)
			@encryptionKey = value
		end	
		
		# @return [String] the HTTP proxy that should be used to create the capture.
		def proxy
			@proxy
		end

		# Set the HTTP proxy that should be used to create the capture.
		#
		# @param value [String] HTTP proxy to use
		# @return [void]
		def proxy=(value)
			@proxy = value
		end			
		
		# @!visibility private
		def startDelay
			if @delay == nil
				return 0
			end
			return @delay
		end		
		
		protected
		def appendParameter(qs, name, value)
			val = ""
			if name != nil && name != ""
				val = CGI.escape(name)
				val += "="
				if value != nil && value != ""
					val += CGI.escape(value)
				end
			end
		
			if val == ""
				return qs
			end
			if qs == nil
				qs = val
				return qs
			end
			
			qs += "&"
			qs += val
			return qs
		end
		
		protected
		def createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = Hash.new
			params['key'] = GrabzIt::Utility.nil_check(applicationKey)
			params['country'] = GrabzIt::Utility.nil_check(@country)
			params['customid'] = GrabzIt::Utility.nil_check(@customId)
			params['callback'] = GrabzIt::Utility.nil_check(callBackURL)
			params['export'] = GrabzIt::Utility.nil_check(@exportURL)
			params['encryption'] = GrabzIt::Utility.nil_check(@encryptionKey)
			params['proxy'] = GrabzIt::Utility.nil_check(@proxy)
			params['sig'] = sig
			params[dataName] = GrabzIt::Utility.nil_check(dataValue)

			return params
		end
	end
end