require 'grabzitclient'

class HomeController < ActionController::Base
  def index
  end
  
  def processScreenshot
  	url = params[:url]
  	grabzItClient = GrabzItClient.new("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET")
  	grabzItClient.take_picture(url, "URL OF YOUR GrabzIt Handler FILE (http://www.example.com/handler/index)")
  	
  	redirect_to root_path
  end
end
