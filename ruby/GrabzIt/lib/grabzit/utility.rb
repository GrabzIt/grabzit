module GrabzIt	
	# @!visibility private
	module Utility
		def self.b_to_str(bValue)
			if bValue
				return 1.to_s
			end
			return 0.to_s
		end

		def self.nil_check(param)
			if param == nil
				return ""
			end
			return param
		end

		def self.nil_int_check(param)
			return param.to_i.to_s
		end

		def self.nil_float_check(param)
			val = param.to_f
			if ((val % 1) == 0)
				return val.to_i.to_s
			end
			
			return val.to_s
		end
	end	
end