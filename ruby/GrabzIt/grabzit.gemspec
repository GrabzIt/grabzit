Gem::Specification.new do |s|
  s.name        = 'grabzit'
  s.version     = '3.3.5'
  s.license = 'MIT'
  s.date        = Date.parse('2019-08-11')
  s.summary     = "GrabzIt Ruby Client"
  s.description = "Use GrabzIt to convert HTML or URL's into images, PDF or DOCX documents. These captures have highly customizable options include altering quality, delay, size, browser type, geographic location and much more. Additionally GrabzIt can even convert HTML tables on the web into a CSV or Excel spreadsheet. As well as enabling online video's to be converted into animated GIF's."
  s.authors     = ["GrabzIt"]
  s.require_paths = ["lib"]
  s.email       = 'support@grabz.it'
  s.files       = ["Rakefile", "lib/grabzit.rb", "lib/grabzit/client.rb","lib/grabzit/cookie.rb","lib/grabzit/screenshotstatus.rb", "lib/grabzit/watermark.rb", "lib/grabzit/exception.rb", "lib/grabzit/baseoptions.rb", "lib/grabzit/utility.rb", "lib/grabzit/animationoptions.rb", "lib/grabzit/request.rb", "lib/grabzit/tableoptions.rb", "lib/grabzit/imageoptions.rb", "lib/grabzit/htmloptions.rb", "lib/grabzit/pdfoptions.rb", "lib/grabzit/docxoptions.rb", "lib/grabzit/proxy.rb", "test/test.png", "test/test_grabzit.rb"]
  s.homepage    = 'https://grabz.it/api/ruby'
  s.add_dependency('rake')
end
