SET /P version=Please enter the version number
cd GrabzIt
gem build grabzit.gemspec

REM THEN DO THIS

gem push grabzit-%version%.gem