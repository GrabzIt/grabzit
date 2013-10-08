#!/usr/bin/perl 

package GrabzItClient;

use Digest::MD5  qw(md5_hex);
use LWP::Simple;
use LWP::UserAgent;
use HTTP::Request::Common qw(POST); 
use URI::Escape;
use XML::Twig;
use File::Spec;
use File::Basename;
use GrabzIt::ScreenShotStatus;
use GrabzIt::GrabzItCookie;
use GrabzIt::GrabzItWaterMark;

use constant WebServicesBaseURL => "http://grabz.it/services/";
use constant TrueString => "True";

sub new
{	
    my $class = shift;       
    
    my $self = {
        _applicationKey => shift,
        _applicationSecret  => shift,
    };

    bless $self, $class;

    return $self;
}

#
#This method sets the parameters required to take a screenshot of a web page.
#
#url - The URL that the screenshot should be made of
#browserWidth - The width of the browser in pixels
#browserHeight - The height of the browser in pixels
#outputHeight - The height of the resulting thumbnail in pixels
#outputWidth - The width of the resulting thumbnail in pixels
#customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
#format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
#delay - The number of milliseconds to wait before taking the screenshot
#targetElement - The id of the only HTML element in the web page to turn into a screenshot
#requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
#customWaterMarkId - add a custom watermark to the image
#country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
#
sub SetImageOptions($;$$$$$$$$$$$)
{
	my ($self, $url, $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement, $requestAs, $customWaterMarkId, $country) = @_;	

	$customId ||= '';
	$browserWidth ||= 0;
	$browserHeight ||= 0;
	$width ||= 0;
	$height ||= 0;
	$format ||= '';
	$delay ||= 0;	
	$targetElement ||= '';
	$requestAs ||= 0;
	$customWaterMarkId ||= '';
	$country ||= '';

	$self->{request} = GrabzItClient::WebServicesBaseURL . "takepicture.ashx?key=" .uri_escape($self->{_applicationKey})."&url=".uri_escape($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&customid=".uri_escape($customId)."&delay=".$delay."&target=".uri_escape($targetElement)."&customwatermarkid=".uri_escape($customWaterMarkId)."&requestmobileversion=".$requestAs."&country=".uri_escape($country)."&callback=";
	$self->{signaturePartOne} = $self->{_applicationSecret}."|".$url."|";
	$self->{signaturePartTwo} = "|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay."|".$targetElement."|".$customWaterMarkId."|".$requestAs."|".$country;
}

#
#This method sets the parameters required to extract all tables from a web page.
#
#url - The URL that the should be used to extract tables
#format - The format the tableshould be in: csv, xlsx
#customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
#includeHeaderNames - If true header names will be included in the table
#includeAllTables - If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX format.
#targetElement - The id of the only HTML element in the web page that should be used to extract tables from
#requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
#country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
#
sub SetTableOptions($;$$$$$$$$)
{
	my ($self, $url, $customId, $tableNumberToInclude, $format, $includeHeaderNames, $includeAllTables, $targetElement, $requestAs, $country) = @_;	
	
	$tableNumberToInclude ||= 1;
	$customId ||= '';
	$format ||= "csv";
	$includeHeaderNames ||= 1;
	$includeAllTables ||= 1;
	$targetElement ||= '';
	$requestAs ||= 0;
	$country ||= '';

	$self->{request} = GrabzItClient::WebServicesBaseURL . "taketable.ashx?key=" .uri_escape($self->{_applicationKey})."&url=".uri_escape($url)."&includeAllTables=".$includeAllTables."&includeHeaderNames=".$includeHeaderNames ."&format=".$format."&tableToInclude=".$tableNumberToInclude."&customid=".uri_escape($customId)."&target=".uri_escape($targetElement)."&requestmobileversion=".$requestAs."&country=".uri_escape($country)."&callback=";
	$self->{signaturePartOne} = $self->{_applicationSecret}."|".$url."|";
	$self->{signaturePartTwo} = "|".$customId."|".$tableNumberToInclude ."|".$includeAllTables."|".$includeHeaderNames."|".$targetElement."|".$format."|".$requestAs."|".$country;
}

#
#This method sets the parameters required to convert a web page into a PDF.
#
#url - The URL that the should be converted into a pdf
#customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
#includeBackground - If true the background of the web page should be included in the screenshot
#pagesize - The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5'.
#orientation - The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
#includeLinks - True if links should be included in the PDF
#includeOutline - True if the PDF outline should be included
#title - Provide a title to the PDF document
#coverURL - The URL of a web page that should be used as a cover page for the PDF
#marginTop - The margin that should appear at the top of the PDF document page
#marginLeft - The margin that should appear at the left of the PDF document page
#marginBottom - The margin that should appear at the bottom of the PDF document page
#marginRight - The margin that should appear at the right of the PDF document
#delay - The number of milliseconds to wait before taking the screenshot
#requestAs - Request the screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
#customWaterMarkId - add a custom watermark to each page of the PDF document
#country - request the screenshot from different countries: Default = "", UK = "UK", US = "US"
#
sub SetPDFOptions($;$$$$$$$$$$$$$$$)
{
	my ($self, $url, $customId, $includeBackground, $pagesize, $orientation, $includeLinks, $includeOutline, $title, $coverURL, $marginTop, $marginLeft, $marginBottom, $marginRight, $delay, $requestAs, $customWaterMarkId, $country) = @_;	
	
	$tableNumberToInclude ||= 1;
	$customId ||= '';
	$includeBackground ||= 1;
	$pagesize ||= "A4";
	$orientation ||= "Portrait";
	$includeLinks ||= 1;
	$includeOutline ||= 0;
	$title ||= '';
	$coverURL ||= '';
	$marginTop ||= 10;
	$marginLeft ||= 10;
	$marginBottom ||= 10;
	$marginRight ||= 10;
	$delay ||= 0;
	$requestAs ||= 0;
	$customWaterMarkId ||= '';
	$country ||= '';
	
	$pagesize = uc($pagesize);
	$orientation = ucfirst($orientation);

	$self->{request} = GrabzItClient::WebServicesBaseURL . "takepdf.ashx?key=" .uri_escape($self->{_applicationKey})."&url=".uri_escape($url)."&background=".$includeBackground ."&pagesize=".$pagesize."&orientation=".$orientation."&customid=".uri_escape($customId)."&customwatermarkid=".uri_escape($customWaterMarkId)."&includelinks=".$includeLinks."&includeoutline=".$includeOutline."&title=".uri_escape($title)."&coverurl=".uri_escape($coverURL)."&mleft=".$marginLeft."&mright=".$marginRight."&mtop=".$marginTop."&mbottom=".$marginBottom."&delay=".$delay."&requestmobileversion=".$requestAs."&country=".uri_escape($country)."&callback=";

	$self->{signaturePartOne} = $self->{_applicationSecret}."|".$url."|";
	$self->{signaturePartTwo} = "|".$customId ."|".$includeBackground ."|".$pagesize ."|".$orientation."|".$customWaterMarkId."|".$includeLinks."|".$includeOutline."|".$title."|".$coverURL."|".$marginTop."|".$marginLeft."|".$marginBottom."|".$marginRight."|".$delay."|".$requestAs."|".$country;
}

#
#This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the GetResult #method.
#
#This is the recommended method of saving a file.
#
sub Save($)
{
	my ($self, $callBackURL) = @_;	
	
	$callBackURL ||= '';	
	
	if ($self->{signaturePartOne} eq '' && $self->{signaturePartTwo} eq '' && $self->{request} eq '')
	{
		 die "No screenshot parameters have been set.";
	}
	
	$sig =  md5_hex($self->{signaturePartOne}.$callBackURL.$self->{signaturePartTwo});
	$self->{request} .= uri_escape($callBackURL)."&sig=".$sig;
	
	return $self->_getResultValue($self->_get($self->{request}), 'ID');
}

#
#This function attempts to Save the result synchronously to a file.
#
#WARNING this method is synchronous so will cause a application to pause while the result is processed.
#
#This function returns the true if it is successful otherwise it throws an exception.
#
sub SaveTo($)
{
	my ($self, $saveToFile) = @_;	
	
	if ($saveToFile eq '')
	{
		 die "No file to save to has been set.";
	}	
	
	my $id = self->Save();

	if ($id eq '')
	{
	    return 0;
	}
	
	#Wait for it to be ready.
	while(1)
	{
		$status = $self->GetStatus($id);

		if (!$status->getCached() && !$status->getProcessing())
		{
			die "The screenshot did not complete with the error: " . $status->getMessage()."\n";
			last;
		}
		elsif ($status->getCached())
		{
			$result = $self->GetResult($id);
			if (!$result)
			{
				die "The screenshot image could not be found on GrabzIt.\n";
				last;
			}
			open FILE, ">".File::Spec->catfile($saveToFile) or die $!."\n"; 
			binmode FILE;
			print FILE $result; 
			close FILE;
			last;
		}

		sleep(3);
	}
	
	return 1;
}

#
#This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet.
#
#id - The unique identifier of the screenshot, returned by the callback handler or the Save method
#
#This function returns the screenshot
#
sub GetResult($)
{
        my ($self, $id) = @_;   
        
        my $url = GrabzItClient::WebServicesBaseURL . "getfile.ashx?id=" . $id;
	
        return $self->_get($url);
}

#
#Get the current status of a GrabzIt screenshot
#
#id - The id of the screenshot
#
#This function returns a Status object representing the screenshot
#
sub GetStatus($)
{
	my ($self, $id) = @_;	
	
	my $url = GrabzItClient::WebServicesBaseURL . "getstatus.ashx?id=" . $id;
	
	my $result = $self->_get($url);	
	die "Could not contact GrabzIt\n" unless defined $result;

	my $xml = XML::Twig->new();
	$xml->parse($result);
	$data = $xml->root;
	
	return ScreenShotStatus->new(($data->first_child_text('Processing') eq GrabzItClient::TrueString), ($data->first_child_text('Cached') eq GrabzItClient::TrueString), ($data->first_child_text('Expired') eq GrabzItClient::TrueString), $data->first_child_text('Message'));
}

#
#Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
#
#domain - The domain to return cookies for.
#
#This function returns an array of cookies
#
sub GetCookies($)
{
	my ($self, $domain) = @_;
	
	$sig =  md5_hex($self->{_applicationSecret}."|".$domain);
	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "getcookies.ashx?" . $qs;

	my $result = $self->_get($url);	
	die "Could not contact GrabzIt\n" unless defined $result;

	my $xml = XML::Twig->new();
	$xml->parse($result);
	$data = $xml->root;

	my $message = $data->first_child_text('Message');
	if($message ne '')
	{
		die $message."\n";
	}

	my @result = ();

	my @cookies = $data->first_child('Cookies')->children('Cookie');
	foreach my $cookie (@cookies)
	{		
		$grabzItCookie = GrabzItCookie->new($cookie->first_child_text('Name'), $cookie->first_child_text('Value'), $cookie->first_child_text('Domain'), $cookie->first_child_text('Path'), $cookie->first_child_text('HttpOnly'), $cookie->first_child_text('Expires'), $cookie->first_child_text('Type'));
		push(@result, $grabzItCookie);
	}
	
	return \@result;
}

#
#Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
#cookie is overridden.
#
#This can be useful if a websites functionality is controlled by cookies.
#
#name - The name of the cookie to set.
#domain - The domain of the website to set the cookie for.
#value - The value of the cookie.
#path - The website path the cookie relates to.
#httponly - Is the cookie only used on HTTP
#expires - When the cookie expires. Pass a null value if it does not expire.
#
#This function returns true if the cookie was successfully set.
#
sub SetCookie($$;$$$$)
{
	my ($self, $name, $domain, $value, $path, $httponly, $expires) = @_;
	
	$name ||= '';
	$domain ||= '';
	$value ||= '';
	$path ||= "/";
	$httponly ||= 0;
	$expires ||= '';
	
	$sig =  md5_hex($self->{_applicationSecret}."|".$name."|".$domain."|".$value."|".$path."|".$httponly."|".$expires."|0");

	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&name=".uri_escape($name)."&value=".uri_escape($value)."&path=".uri_escape($path)."&httponly=".$httponly."&expires=".uri_escape($expires)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs;

	return ($self->_getResultValue($self->_get($url), 'Result') eq GrabzItClient::TrueString);
}

#
#Delete a custom cookie or block a global cookie from being used.
#
#name - The name of the cookie to delete
#domain - The website the cookie belongs to
#
#This function returns true if the cookie was successfully set.
#
sub DeleteCookie($$)
{
	my ($self, $name, $domain) = @_;
	
	$sig =  md5_hex($self->{_applicationSecret}."|".$name."|".$domain."|1");

	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&name=".uri_escape($name)."&delete=1&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs;

	return ($self->_getResultValue($self->_get($url), 'Result') eq GrabzItClient::TrueString);
}

#
#Add a new custom watermark.
#
#identifier - The identifier you want to give the custom watermark. It is important that this identifier is unique.
#path - The absolute path of the watermark on your server. For instance C:/watermark/1.png
#xpos - The horizontal position you want the screenshot to appear at: Left = 0, Center = 1, Right = 2
#ypos - The vertical position you want the screenshot to appear at: Top = 0, Middle = 1, Bottom = 2
#
#This function returns true if the watermark was successfully set.
#
sub AddWaterMark($$$$)
{
	my ($self, $identifier, $path, $xpos, $ypos) = @_;
	
	$xpos ||= 0;
	$ypos ||= 0;	
	
	unless (-e $path)
	{
		die "File: " . $path . " does not exist\n";
	}

        $sig = md5_hex($self->{_applicationSecret}."|".$identifier."|".$xpos."|".$ypos);	
	
	open FILE, $path or die "Couldn't open file: $!";
	binmode FILE;
	local $/;
	$data = <FILE>;
	close FILE;	
	
	$ua = LWP::UserAgent->new();  
		
	$req = POST GrabzItClient::WebServicesBaseURL . 'addwatermark.ashx', 
	       content_type => 'multipart/form-data', 
	       content      => [ 
				 key => $self->{_applicationKey}, 
				 identifier => $identifier,
				 xpos => $xpos,
				 ypos => $ypos,
				 sig => $sig,
				 watermark => [undef, basename($path), 
					  content_type => 'image/jpeg', 
					  content => $data, 
					 ] 
			       ]; 

	return $self->_getResultValue($self->_handleResponse($ua->request($req)), 'Result');
}

#
#Get your uploaded custom watermark
#
#identifier - The identifier of a particular custom watermark you want to view
#
#This function returns an array of GrabzItWaterMark
#
sub GetWaterMark($)
{
    my ($self, $identifier) = @_;
    return $self->_getWaterMarks($identifier);
}

#
#Get your custom watermarks
#
#This function returns an array of GrabzItWaterMark
#
sub GetWaterMarks()
{
    my ($self) = @_;
    return $self->_getWaterMarks("");
}

sub _getWaterMarks($)
{
	my ($self, $identifier) = @_;
	
	$identifier ||= '';
	
	$sig =  md5_hex($self->{_applicationSecret}."|".$identifier);
	
	$qs = "key=" .uri_escape($self->{_applicationKey})."&identifier=".uri_escape($identifier)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "getwatermarks.ashx?" . $qs;

	my $result = $self->_get($url);	
	die "Could not contact GrabzIt\n" unless defined $result;

	my $xml = XML::Twig->new();
	$xml->parse($result);
	$data = $xml->root;

	my $message = $data->first_child_text('Message');
	if($message ne '')
	{
		die $message."\n";
	}

	my @result = ();

	my @watermarks = $data->first_child('WaterMarks')->children('WaterMark');
	foreach my $watermark (@watermarks)
	{		
		$grabzItWaterMark = GrabzItWaterMark->new($watermark->first_child_text('Identifier'), $watermark->first_child_text('XPosition'), $watermark->first_child_text('YPosition'), $watermark->first_child_text('Format'));
		push(@result, $grabzItWaterMark);
	}
	
	return \@result;
}

#
#Delete a custom watermark.
#
#identifier - The identifier of the custom watermark you want to delete
#
#This function returns true if the watermark was successfully deleted.
#
sub DeleteWaterMark($)
{
	my ($self, $identifier) = @_;

	$sig =  md5_hex($self->{_applicationSecret}."|".$identifier);
	$qs = "key=" .uri_escape($self->{_applicationKey})."&identifier=".uri_escape($identifier)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "deletewatermark.ashx?" . $qs;

	return ($self->_getResultValue($self->_get($url), 'Result') eq GrabzItClient::TrueString);
}

#
#DEPRECATED - Use SetImageOptions and Save method instead
#
sub TakePicture($;$$$$$$$$$)
{	
	my ($self, $url, $callback, $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement) = @_;	

	$self->SetImageOptions($url, $callback, $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement);
	return $self->Save($callback);
}

#
#DEPRECATED - Use the SetImageOptions and SaveTo methods instead
#
sub SavePicture($$;$$$$$$$)
{		
	my ($self, $url, $saveToFile, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement) = @_;	
	
	$self->SetImageOptions($url, '', $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement);
	return $self->SaveTo($saveToFile);
}

#
#DEPRECATED - Use GetResult method instead
#
sub GetPicture($)
{
    my ($self, $id) = @_;
	
    return $self->GetResult($id);
}

sub _get($)
{
    my ($self, $url) = @_;
    
    $ua = LWP::UserAgent->new();
    
    return $self->_handleResponse($ua->get($url));
}

sub _handleResponse($)
{
    my ($self, $response) = @_;
    
    if ($response->code == 403)
    {
        die 'Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.';
    }
    elsif ($response->code >= 400)
    {
        die 'A network error occured when connecting to the GrabzIt servers.';
    }
    
    return $response->decoded_content;
}

sub _getResultValue($$)
{	
	my ($self, $result, $elementId) = @_;
	
	die "Could not contact GrabzIt\n" unless defined $result;
	
	my $xml = XML::Twig->new();
	$xml->parse($result);
	$data = $xml->root;
	
	$message = $data->first_child_text('Message');		
	
	if($message ne '')
	{		
		die $message."\n";
	}

	return $data->first_child_text($elementId);
}
1;