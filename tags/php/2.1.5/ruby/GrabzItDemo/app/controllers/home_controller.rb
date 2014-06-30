require 'grabzit'

class HomeController < ActionController::Base
  def index
  end
  
  def processScreenshot
  	url = params[:url]
  	format = params[:format]
  	
  	app_config = YAML.load_file("#{Rails.root}/config/config.yml")[Rails.env]
  	grabzItClient = GrabzIt::Client.new(app_config['application_key'], app_config['application_secret'])
  	
  	if format == "jpg"
  		grabzItClient.set_image_options(url)
  	else
  		grabzItClient.set_pdf_options(url)
  	end
  	
  	@SuccessMessage = ''
  	@ErrorMessage = ''
  	
  	begin
		grabzItClient.save(app_config['handler'])
		@SuccessMessage = "Processing screenshot."
	rescue RuntimeError => e
		@ErrorMessage = e
	end
	
	render :action => "index.html"
  end
  
  def clearScreenshots
  	dir_path = "public/screenshots/"
  	Dir.foreach(dir_path) {|f| fn = File.join(dir_path, f); File.delete(fn) if f != '.' && f != '..'}
  	
  	redirect_to root_path
  end
end
