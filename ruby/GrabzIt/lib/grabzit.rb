Dir[File.join(File.dirname(__FILE__), "grabzit", "*rb")].each do |file|
  require file
end