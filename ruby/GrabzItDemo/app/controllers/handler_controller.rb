require 'grabzit'

class HandlerController < ApplicationController
  def index
	  message = params[:message]
	  customId = params[:customid]
	  id = params[:id]
	  filename = params[:filename]
	  format = params[:format]
	  targetError = params[:targeterror]
	  
  	  app_config = YAML.load_file("#{Rails.root}/config/config.yml")[Rails.env]
  	  grabzItClient = GrabzIt::Client.new(app_config['application_key'], app_config['application_secret'])
	  result = grabzItClient.get_result(id)
	  
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
