require 'test/unit'
require 'grabzit'

class GrabzItTest < Test::Unit::TestCase
	Cookie_Name = "test_cookie"
	Cookie_Domain = ".example.com"
	WaterMark_Identifier = "test_watermark"
	WaterMark_Path = "test/test.png"
	Screenshot_Path = "test/tmp.jpg"

	def setup
		@applicationKey = ""
		@applicationSecret = ""
		#Set to true if the account is subscribed
		@isSubscribedAccount = false
	end
	
	def test_application_key
		assert_not_nil(@applicationKey, "Please set your application key variable in the setup method. You can get a application key for free from: http://grabz.it")
	end

	def test_application_secret
		assert_not_nil(@applicationSecret, "Please set your application secret variable in the setup method. You can get a application key for free from: http://grabz.it")
	end

	def test_take_picture
		assert_nothing_raised "An error occured when trying to take a picture" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			assert_not_nil(grabzItClient.take_picture("http://www.google.com"), "Failed to take screenshot using depreceated method")
		end	
	end
	
	def test_take_pdf
		assert_nothing_raised "An error occured when trying to take a pdf screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.set_pdf_options("http://www.google.com")
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot using set_pdf_options method")
		end	
	end	
	
	def test_take_image
		assert_nothing_raised "An error occured when trying to take a image screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.set_image_options("http://www.google.com")
			assert_not_nil(grabzItClient.save(), "Failed to take screenshot using set_image_options method")
		end	
	end
	
	def test_save_picture
		if File.file?(Screenshot_Path)
			File.delete(Screenshot_Path)
		end
		assert_nothing_raised "An error occured when trying to take a image screenshot" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			assert_equal(true, grabzItClient.save_picture("http://www.google.com", Screenshot_Path), "Screenshot not saved")
			assert_equal(true, File.file?(Screenshot_Path), "Not saved screenshot file")
		end	
		File.delete(Screenshot_Path)
	end	
	
	def test_status
		assert_nothing_raised "An error occured when trying to test the status method" do
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			grabzItClient.set_image_options("http://www.google.com")
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
				assert_raise RuntimeError, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)				  
				end		
				return
			end			

			assert(find_cookie(grabzItClient), "Set cookie has not been found!")
		end	
	end	
	
	def test_delete_cookie
		assert_nothing_raised "An error occured when trying to add a cookie" do	
			grabzItClient = GrabzIt::Client.new(@applicationKey, @applicationSecret)
			if @isSubscribedAccount
				grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)
			elsif
				assert_raise RuntimeError, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.set_cookie(Cookie_Name, Cookie_Domain)				  
				end		
				return
			end			
			
			assert_equal(true, find_cookie(grabzItClient), "Test cookie not found!")
			
			grabzItClient.delete_cookie(Cookie_Name, Cookie_Domain)

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
				assert_raise RuntimeError, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)				  
				end		
				return
			end			
			
			assert_equal(true, find_watermark(grabzItClient), "Test watermark not found!")
			
			grabzItClient.delete_watermark(WaterMark_Identifier)

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
				assert_raise RuntimeError, "User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method" do
				  grabzItClient.add_watermark(WaterMark_Identifier, WaterMark_Path, 2, 2)				  
				end		
				return
			end			

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
end