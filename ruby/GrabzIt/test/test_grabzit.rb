require 'test/unit'
require 'grabzit'

class GrabzItTest < Test::Unit::TestCase
	Cookie_Name = "test_cookie"
	Cookie_Domain = ".example.com"
	WaterMark_Identifier = "test_watermark"
	WaterMark_Path = "test/test.png"
	Screenshot_Path = "test/tmp.jpg"

	def setup
		@applicationKey = "c3VwcG9ydEBncmFiei5pdA=="
		@applicationSecret = "AD8/aT8/Pz8/Tz8/PwJ3Pz9sVSs/Pz8/Pz9DOzJodoi="
		#Set to true if the account is subscribed
		@isSubscribedAccount = true
	end
	
	def test_application_key
		assert_not_nil(@applicationKey, "Please set your application key variable in the setup method. You can get a application key for free from: http://grabz.it")
	end

	def test_application_secret
		assert_not_nil(@applicationSecret, "Please set your application secret variable in the setup method. You can get a application key for free from: http://grabz.it")
	end

	def test_html_to_image
		assert_nothing_raised "An error occured when trying to take convert HTML to a image" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.html_to_image("<h1>Hello world</h1>");
			assert_not_equal(false, grabzItClient.save(), "HTML not converted")
		end		
	end

	def test_html_to_pdf
		assert_nothing_raised "An error occured when trying to take convert HTML to a pdf" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.html_to_pdf("<h1>Hello world</h1>");
			assert_not_equal(false, grabzItClient.save(), "HTML not converted")
		end		
	end
	
	def test_save_to_bytes
		assert_nothing_raised "An error occured when trying to take a image screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_image("http://www.google.com")
			val = grabzItClient.save_to();
			assert_not_nil(val, "Screenshot not returned as bytes")
			assert_not_equal(false, val, "An error occured attempting to save data to byte variable")
		end	
	end	
	
	def test_save_table
		assert_nothing_raised "An error occured when trying to extract a HTML table" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_table("http://grabz.it/api/php/advanced/errorhandling.aspx")
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot using url_to_table method")
		end	
	end	
	
	def test_save_animation
		assert_nothing_raised "An error occured when trying to take a animation" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_animation("http://www.youtube.com");
			assert_not_equal(false, grabzItClient.save(), "Animation not taken")
		end		
	end	
	
	def test_status
		assert_nothing_raised "An error occured when trying to test the status method" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_image("http://www.google.com")
			id = grabzItClient.save()
			status = grabzItClient.get_status(id)
			
			assert_equal(true, (status.processing || status.cached), "Failed to get correct screenshot status!")
		end	
	end	
	
	def test_add_cookie
		assert_nothing_raised "An error occured when trying to add a cookie" do		
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			
			if @isSubscribedAccount
				grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)
			elsif
				assert_raise GrabzIt::GrabzItException, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)				  
				end		
				return
			end
			
			sleep(4)

			assert(find_cookie(grabzItClient), "Set cookie has not been found!")
		end	
	end	
	
	def test_delete_cookie
		assert_nothing_raised "An error occured when trying to add a cookie" do	
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			if @isSubscribedAccount
				grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)
			elsif
				assert_raise GrabzIt::GrabzItException, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)				  
				end		
				return
			end			
			
			sleep(4)
			
			assert_equal(true, find_cookie(grabzItClient), "Test cookie not found!")
			
			grabzItClient.delete_cookie(Cookie_Name, Cookie_Domain)

			sleep(4)

			assert_equal(false, find_cookie(grabzItClient), "Failed to delete cookie!")
		end	
	end		

	def test_delete_watermark
		assert_nothing_raised "An error occured when trying to add a watermark" do	
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			begin
				grabzItClient.delete_watermark(WaterMark_Identifier)
			rescue
			end
			
			if @isSubscribedAccount
				grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)
			elsif
				assert_raise GrabzIt::GrabzItException, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)				  
				end		
				return
			end			
			
			sleep(5)
			
			assert_equal(true, find_watermark(grabzItClient), "Test watermark not found!")
			
			grabzItClient.delete_watermark(WaterMark_Identifier)

			sleep(5)

			assert_equal(false, find_watermark(grabzItClient), "Failed to delete watermark!")
		end	
	end
	
	def test_add_watermark
		assert_nothing_raised "An error occured when trying to add a watermark" do		
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			begin
				grabzItClient.delete_watermark(WaterMark_Identifier)
			rescue
			end	
			if @isSubscribedAccount
				grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)
			elsif
				assert_raise GrabzIt::GrabzItException, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)				  
				end		
				return
			end
			
			sleep(4)

			assert(find_watermark(grabzItClient), "Set watermark has not been found!")
		end	
	end
	
	#def test_Xtreme_DDOS
	#	assert_raise RuntimeError, "An error occured when detecting potential DDOS attack." do
	#		grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
	#		for i in 0..200
	#			grabzItClient.get_status("ccxz")
	#		end
	#	end
	#end
	
	def find_cookie(grabzItClient)
		cookies = grabzItClient.get_cookies(Cookie_Domain)
		
		if cookies == nil
			return false
		end
		
		cookies.each { |cookie| 
			if cookie.name == Cookie_Name 
				return true
			end 
		} 	
		
		return false
	end
	
	def find_watermark(grabzItClient)
		watermark = grabzItClient.get_watermark(WaterMark_Identifier)
		
		if watermark == nil
			return false
		end
		
		return true
	end	
	
	def test_take_pdf
		assert_nothing_raised "An error occured when trying to take a pdf screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_pdf("http://www.google.com")
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot using url_to_pdf method")
		end	
	end	
	
	def test_take_image_options
		assert_nothing_raised "An error occured when trying to take a image screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			options = GrabzIt::ImageOptions.new()
			options.customWaterMarkId("GrabzIt_Browser")
			grabzItClient.url_to_image("http://www.google.com", options)
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot using set_image_options method")
		end	
	end
	
	def test_take_unicode_url_image
		assert_nothing_raised "An error occured when trying to take a image screenshot with a unicode URL" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.url_to_image("\u0068\u0074\u0074\u0070\u003a\u002f\u002f\u0066\u006f\u0072\u0075\u006d\u002e\u0074\u0068\u0061\u0069\u007a\u0061\u002e\u0063\u006f\u006d\u002f\u0074\u0065\u0063\u0068\u006e\u006f\u006c\u006f\u0067\u0079\u002f\u004e\u0065\u0064\u0061\u0061\u002d\u0e08\u0e31\u0e1a\u0e21\u0e37\u0e2d\u002d\u0048\u0079\u0074\u0065\u0072\u0061\u002d\u0e43\u0e2b\u0e49\u0e1a\u0e23\u0e34\u0e01\u0e32\u0e23\u0e23\u0e30\u0e1a\u0e1a\u0e27\u0e34\u0e17\u0e22\u0e38\u0e21\u0e32\u0e15\u0e23\u0e10\u0e32\u0e19\u002d\u0054\u0045\u0054\u0052\u0041\u002d\u0e23\u0e30\u0e14\u0e31\u0e1a\u0e40\u0e27\u0e34\u0e25\u0e14\u0e4c\u0e04\u0e25\u0e32\u0e2a\u0e2a\u0e33\u0e2b\u0e23\u0e31\u0e1a\u0e01\u0e32\u002f\u0036\u0033\u0037\u0037\u0035\u0037\u002f".encode('utf-8'))
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot or a unicode URL")
		end	
	end		
end