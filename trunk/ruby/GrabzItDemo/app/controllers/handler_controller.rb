require 'grabzitclient'

class HandlerController < ApplicationController
  def index
	  message = params[:message]
	  customId = params[:customid]
	  id = params[:id]
	  filename = params[:filename]
	  
	  grabzItClient = GrabzItClient.new("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET")
	  result = grabzItClient.get_picture(id)
	  
	  if result == nil
	          return
	  end

	  #Ensure that the application has the correct rights for this directory.  
	  screenshot = File.new("public/screenshots/"+filename, "wb")
	  screenshot.write(result)
	  screenshot.close  
	  
	  redirect_to root_path
  end
end
