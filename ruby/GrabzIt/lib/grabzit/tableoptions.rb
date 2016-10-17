module GrabzIt
	require File.join(File.dirname(__FILE__), 'baseoptions')
	
	# Represents all of the options available when extracting tabular data
	# @version 3.0
	# @author GrabzIt
	class TableOptions < BaseOptions
		def initialize()
			@tableNumberToInclude = 1
			@format = 'csv'
			@includeHeaderNames = true
			@includeAllTables = false
			@targetElement = nil
			@requestAs = 0
		end
		
		# @return [Integer] the index of the table to be converted
		def tableNumberToInclude
			@tableNumberToInclude
		end
		
		# Set the index of the table to be converted, were all tables in a web page are ordered from the top of the web page to bottom
		#
		# @param value [Integer] the table number
		# @return [void]		
		def tableNumberToInclude(value)
			@tableNumberToInclude = value
		end		
		
		# @return [String] the format of the table should be
		def format
			@format
		end
		
		# Set the format the table should be in: 'csv', 'xlsx' or 'json'
		#
		# @param value [String] the format
		# @return [void]		
		def format(value)
			@format = value
		end		
		
		# @return [Boolean] if the header names are included in the table
		def includeHeaderNames
			@includeHeaderNames
		end
		
		# Set to true to include header names into the table
		#
		# @param value [Boolean] include header names
		# @return [void]		
		def includeHeaderNames(value)
			@includeHeaderNames = value
		end		
		
		# @return [Boolean] if every table on will be extracted with each table appearing in a separate spreadsheet sheet
		def includeAllTables
			@includeAllTables
		end
		
		# Set to true to extract every table on the web page into a separate spreadsheet sheet. Only available with the XLSX and JSON formats
		#
		# @param value [Boolean] include all tables
		# @return [void]		
		def includeAllTables(value)
			@includeAllTables = value
		end		
		
		# @return [String] the id of the only HTML element in the web page that should be used to extract tables from
		def targetElement
			@targetElement
		end
		
		# Set the id of the only HTML element in the web page that should be used to extract tables from
		#
		# @param value [String] target
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
			"|"+GrabzIt::Utility.nil_check(@customId)+"|"+GrabzIt::Utility.nil_int_check(@tableNumberToInclude)+"|"+GrabzIt::Utility.b_to_str(@includeAllTables)+
			"|"+GrabzIt::Utility.b_to_str(@includeHeaderNames)+"|"+GrabzIt::Utility.nil_check(@targetElement)+"|"+GrabzIt::Utility.nil_check(@format)+"|"+
			GrabzIt::Utility.nil_int_check(@requestAs)+"|"+GrabzIt::Utility.nil_check(@country)
		end

		# @!visibility private
		def _getParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue)
			params['includeAllTables'] = GrabzIt::Utility.b_to_str(@includeAllTables)
			params['includeHeaderNames'] = GrabzIt::Utility.b_to_str(@includeHeaderNames)
			params['format'] = GrabzIt::Utility.nil_check(@format)
			params['tableToInclude'] = GrabzIt::Utility.nil_int_check(@tableNumberToInclude)
			params['target'] = GrabzIt::Utility.nil_check(@targetElement)
			params['requestmobileversion'] = GrabzIt::Utility.nil_int_check(@requestAs)

			return params
		end
	end
end