#!/usr/bin/perl 

package GrabzItClient;

use Digest::MD5  qw(md5_hex);
use Encode;
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
use GrabzIt::GrabzItRequest;
use GrabzIt::GrabzItAnimationOptions;
use GrabzIt::GrabzItImageOptions;
use GrabzIt::GrabzItTableOptions;
use GrabzIt::GrabzItPDFOptions;
use GrabzIt::GrabzItDOCXOptions;

use constant WebServicesBaseURL_GET => "http://api.grabz.it/services/";
use constant WebServicesBaseURL_POST => "http://grabz.it/services/";
use constant TakePicture => "takepicture.ashx";
use constant TakeTable => "taketable.ashx";
use constant TakePDF => "takepdf.ashx";
use constant TakeDOCX => "takedocx.ashx";
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
# This method specifies the URL of the online video that should be converted into a animated GIF
#
# url - The URL to convert into a animated GIF
# options - A instance of the GrabzItAnimationOptions class that defines any special options to use when creating the animated GIF
#
sub URLToAnimation($;$)
{
	my ($self, $url, $options) = @_;
	$options ||= GrabzItAnimationOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_GET . "takeanimation.ashx", 0, $options, $url);
}

#
# This method specifies the URL that should be converted into a image screenshot.
#
# url - The URL to capture as a screenshot.
# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the screenshot
#
sub URLToImage($;$)
{
	my ($self, $url, $options) = @_;
	$options ||= GrabzItImageOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_GET . TakePicture, 0, $options, $url);
}

#
# This method specifies the HTML that should be converted into a image.
#
# html - The HTML to convert into a image.
# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image
#
sub HTMLToImage($;$)
{
	my ($self, $html, $options) = @_;
	$options ||= GrabzItImageOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_POST . TakePicture, 1, $options, $html);
}

#
# This method specifies a HTML file that should be converted into a image.
#
# path - The file path of a HTML file to convert into a image.
# options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image.
#
sub FileToImage($;$)
{
	my ($self, $path, $options) = @_;
    $self->HTMLToImage($self->_readHTMLFile($path), $options);
}

#
# This method specifies the URL that the HTML tables should be extracted from.
#
# url - The URL to extract HTML tables from.
# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table
#
sub URLToTable($;$)
{
	my ($self, $url, $options) = @_;
	$options ||= GrabzItTableOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_GET . TakeTable, 0, $options, $url);
}

#
# This method specifies the HTML that the HTML tables should be extracted from.
#
# html - The HTML to extract HTML tables from.
# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table.
#
sub HTMLToTable($;$)
{
	my ($self, $html, $options) = @_;
	$options ||= GrabzItTableOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_POST . TakeTable, 1, $options, $html);
}

#
# This method specifies a HTML file that the HTML tables should be extracted from.
#
# path - The file path of a HTML file to extract HTML tables from.
# options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table
#
sub FileToTable($;$)
{
	my ($self, $path, $options) = @_;
    $self->HTMLToTable($self->_readHTMLFile($path), $options);
}

#
# This method specifies the URL that should be converted into a PDF
#
# url - The URL to capture as a PDF
# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF
#
sub URLToPDF($;$)
{
	my ($self, $url, $options) = @_;
	$options ||= GrabzItPDFOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_GET . TakePDF, 0, $options, $url);
}

#
# This method specifies the HTML that should be converted into a PDF.
#
# html - The HTML to convert into a PDF.
# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF
#
sub HTMLToPDF($;$)
{
	my ($self, $html, $options) = @_;
	$options ||= GrabzItPDFOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_POST . TakePDF, 1, $options, $html);
}

#
# This method specifies the HTML that should be converted into a PDF.
#
# html - The HTML to convert into a PDF.
# options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF.
#
sub FileToPDF($;$)
{
	my ($self, $path, $options) = @_;
    $self->HTMLToPDF($self->_readHTMLFile($path), $options);
}

#
# This method specifies the URL that should be converted into a DOCX
#
# url - The URL to capture as a DOCX
# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX
#
sub URLToDOCX($;$)
{
	my ($self, $url, $options) = @_;
	$options ||= GrabzItDOCXOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_GET . TakeDOCX, 0, $options, $url);
}

#
# This method specifies the HTML that should be converted into a DOCX.
#
# html - The HTML to convert into a DOCX.
# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX
#
sub HTMLToDOCX($;$)
{
	my ($self, $html, $options) = @_;
	$options ||= GrabzItDOCXOptions->new();    
	$self->{request} = GrabzItRequest->new(GrabzItClient::WebServicesBaseURL_POST . TakeDOCX, 1, $options, $html);
}

#
# This method specifies the HTML that should be converted into a DOCX.
#
# html - The HTML to convert into a DOCX.
# options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX.
#
sub FileToDOCX($;$)
{
	my ($self, $path, $options) = @_;
    $self->HTMLToDOCX($self->_readHTMLFile($path), $options);
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
	
	if (length $self->{request} == 0)
	{
		 die "No parameters have been set.";
	}
	
	$sig =  $self->_encode($self->{request}->{options}->_getSignatureString($self->{_applicationSecret}, $callBackURL, $self->{request}->_getTargetUrl()));
	
    if ($self->{request}->{"isPost"} == 1)
    {
        return $self->_getResultValue($self->_post($self->{request}->{url}, $self->{request}->{options}->_getParameters($self->{_applicationKey}, $sig, $callBackURL, 'html', uri_escape($self->{request}->{data}))), 'ID');
    }
    
    my $uri = URI->new($self->{request}->{url});
    $uri->query_form($self->{request}->{options}->_getParameters($self->{_applicationKey}, $sig, $callBackURL, 'url', $self->{request}->{data})); 
    
	return $self->_getResultValue($self->_get($uri), 'ID');   
}

#
#Calls the GrabzIt web service to take the screenshot and saves it to the target path provided. if no target path is provided
#it returns the screenshot byte data.
#
#WARNING this method is synchronous so will cause a application to pause while the result is processed.
#
#This function returns the true if it is successful saved to a file, or if it is not saving to a file byte data is returned,
#otherwise the method throws an exception.
#
sub SaveTo($)
{
	my ($self, $saveToFile) = @_;
	
	$saveToFile ||= '';
	
	my $id = $self->Save();

	if ($id eq '')
	{
	    return 0;
	}
	
	#Wait for it to be possibly ready
	sleep(int((3000 + $self->{request}->{options}->{"delay"}) / 1000));
	
	#Wait for it to be ready.
	while(1)
	{
		$status = $self->GetStatus($id);

		if (!$status->getCached() && !$status->getProcessing())
		{
			die "The capture did not complete with the error: " . $status->getMessage()."\n";
			last;
		}
		elsif ($status->getCached())
		{
			$result = $self->GetResult($id);
			if (!$result)
			{
				die "The capture could not be found on GrabzIt.\n";
				last;
			}
			
			if ($saveToFile eq '')
			{
				return $result;
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
        
	$id ||= '';
	
	if ($id eq '')
	{
	    return;
	}	
	
        my $url = GrabzItClient::WebServicesBaseURL_GET . "getfile.ashx?id=" . $id;
	
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
	
	$id ||= '';
	
	if ($id eq '')
	{
	    return;
	}
	
	my $url = GrabzItClient::WebServicesBaseURL_GET . "getstatus.ashx?id=" . $id;
	
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
	
	$sig =  $self->_encode($self->{_applicationSecret}."|".$domain);
	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL_GET . "getcookies.ashx?" . $qs;

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
#expires - When the cookie expires. Pass a empty ('') value if it does not expire.
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
	
	$sig =  $self->_encode($self->{_applicationSecret}."|".$name."|".$domain."|".$value."|".$path."|".$httponly."|".$expires."|0");

	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&name=".uri_escape($name)."&value=".uri_escape($value)."&path=".uri_escape($path)."&httponly=".$httponly."&expires=".uri_escape($expires)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL_GET . "setcookie.ashx?" . $qs;

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
	
	$sig =  $self->_encode($self->{_applicationSecret}."|".$name."|".$domain."|1");

	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&name=".uri_escape($name)."&delete=1&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL_GET . "setcookie.ashx?" . $qs;

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

        $sig = $self->_encode($self->{_applicationSecret}."|".$identifier."|".$xpos."|".$ypos);	
	
	open FILE, $path or die "Couldn't open file: $!";
	binmode FILE;
	local $/;
	$data = <FILE>;
	close FILE;	
	
	$ua = LWP::UserAgent->new();  
		
	$req = POST 'http://grabz.it/services/addwatermark.ashx', 
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
	
	$sig =  $self->_encode($self->{_applicationSecret}."|".$identifier);
	
	$qs = "key=" .uri_escape($self->{_applicationKey})."&identifier=".uri_escape($identifier)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL_GET . "getwatermarks.ashx?" . $qs;

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

	$sig =  $self->_encode($self->{_applicationSecret}."|".$identifier);
	$qs = "key=" .uri_escape($self->{_applicationKey})."&identifier=".uri_escape($identifier)."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL_GET . "deletewatermark.ashx?" . $qs;

	return ($self->_getResultValue($self->_get($url), 'Result') eq GrabzItClient::TrueString);
}

sub _encode($)
{
    my ($self, $text) = @_;
    $text = decode_utf8($text);
    $text =~ s/[^\x00-\x7F]/?/g;
    return md5_hex($text);
}

sub _get($)
{
    my ($self, $url) = @_;
    $ua = LWP::UserAgent->new();
    
    return $self->_handleResponse($ua->get($url));
}

sub _post($$)
{
    my ($self, $url, $params) = @_;    
    $ua = LWP::UserAgent->new();
    
    return $self->_handleResponse($ua->post($url, $params));
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

sub _readHTMLFile($)
{
    my ($self, $path) = @_;
    
    unless (-e $path)
	{
		die "File: " . $path . " does not exist\n";
	}
    
    open FILE, $path or die "Couldn't open file: $!";
	binmode FILE;
	local $/;
	$data = <FILE>;
	close FILE;

    return $data;
}
1;