class ImagesController < ApplicationController
  def index
  	@images = Array.new
  	Dir.foreach("public/screenshots") do |f|
  		if f != "." && f != ".." && f != "keep.txt"
			@images << "../../screenshots/"+f  
		end
	end
  end
end
