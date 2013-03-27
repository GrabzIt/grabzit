# This class represents the custom watermarks stored in GrabzIt
# @version 2.0
# @author GrabzIt
class GrabzItWaterMark
	# @api private
	def initialize(identifier = '', xPosition = 0, yPosition = 0, format = '')
		@Identifier = identifier
		@XPosition = xPosition
		@YPosition = yPosition
		@Format = format
	end
	# @return [String] the identifier of the watermark
	def identifier
		@Identifier
	end
	# @return [Integer] the horizontal postion of the watermark. 0 = Left, 1 = Center, 2 = Right
	def xPosition
		@XPosition
	end  
	# @return [Integer] the vertical postion of the watermark. 0 = Top, 1 = Middle, 2 = Bottom	
	def yPosition
		@YPosition
	end
	# @return [String] the format of the watermark	
	def format
		@Format
	end  
end
