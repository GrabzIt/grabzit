require 'URI'
require 'grabzit'

class HomeController < ActionController::Base
  def index
	@UseCallbackHandler = useCallbackHandler(request)
  end
  
  def useCallbackHandler(r)
	app_config = YAML.load_file("#{Rails.root}/config/config.yml")[Rails.env]
	if app_config['handler'] !~ URI::regexp
		return false
	end
	localhost = Regexp.union [/^127\.\d{1,3}\.\d{1,3}\.\d{1,3}$/, /^::1$/, /^0:0:0:0:0:0:0:1(%.*)?$/]
	return localhost !~ r.remote_addr && localhost !~ r.remote_ip	
  end
  
  def processScreenshot
  	url = params[:url]
	html = params[:html]
  	format = params[:format]
	isHtml = params[:convert] == "html"
  	
  	app_config = YAML.load_file("#{Rails.root}/config/config.yml")[Rails.env]
  	grabzItClient = GrabzIt::Client.new(app_config['application_key'], app_config['application_secret'])
  	
  	if format == "jpg"
		if isHtml
			grabzItClient.html_to_image(html)
		else
			grabzItClient.url_to_image(url)
		end
	elsif format == "docx"
		if isHtml
			grabzItClient.html_to_docx(html)
		else
			grabzItClient.url_to_docx(url)
		end		
	elsif format == "csv"
		if isHtml
			grabzItClient.html_to_table(html)
		else
			grabzItClient.url_to_table(url)
		end
  	elsif format == "gif"
	  	grabzItClient.url_to_animation(url)
  	else
		if isHtml
			grabzItClient.html_to_pdf(html)
		else
			grabzItClient.url_to_pdf(url)
		end
  	end
  	
  	@SuccessMessage = ''
  	@ErrorMessage = ''
  	
  	begin
		if useCallbackHandler(request)
			grabzItClient.save(app_config['handler'])
			@SuccessMessage = "Processing..."
			@UseCallbackHandler = true
		else
			grabzItClient.save_to("public/screenshots/"+rand(1...999999).to_s()+"."+format)
			@UseCallbackHandler = false
		end
	rescue RuntimeError, GrabzIt::GrabzItException => e
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
