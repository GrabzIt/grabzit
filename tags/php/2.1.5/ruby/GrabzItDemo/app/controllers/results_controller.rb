class ResultsController < ApplicationController
  def index
  	@screenshots = Array.new
  	Dir.foreach("public/screenshots") do |f|
  		if f != "." && f != ".." && f != "keep.txt"
			@screenshots << "/screenshots/"+f  
		end
	end
	@response = @screenshots
	render :json => @response
  end
end
