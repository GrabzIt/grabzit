#!/usr/bin/perl 

package GrabzItClient;

use Digest::MD5  qw(md5_hex);
use LWP::Simple;
use URI::Escape;
use XML::Simple;
use ScreenShotStatus;
use GrabzItCookie;

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
#This method calls the GrabzIt web service to take the screenshot.
#
#url - The URL that the screenshot should be made of
#callback - The handler the GrabzIt web service should call after it has completed its work
#browserWidth - The width of the browser in pixels
#browserHeight - The height of the browser in pixels
#outputHeight - The height of the resulting thumbnail in pixels
#outputWidth - The width of the resulting thumbnail in pixels
#customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
#format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
#delay - The number of milliseconds to wait before taking the screenshot
#
#This function returns the unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.
#
sub TakePicture($;$$$$$$$$)
{	
	my ($self, $url, $callback, $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay) = @_;	

	$callback ||= "";
	$customId ||= "";
	$browserWidth ||= 0;
	$browserHeight ||= 0;
	$width ||= 0;
	$height ||= 0;
	$format ||= "";
	$delay ||= 0;	
	
	$qs = "key=" .$self->{_applicationKey}."&url=".uri_escape($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&callback=".uri_escape($callback)."&customid=".uri_escape($customId)."&delay=".$delay;
	$sig =  md5_hex($self->{_applicationSecret}."|".$url."|".$callback."|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay);
	$qs .= "&sig=".$sig;
	
	my $url = GrabzItClient::WebServicesBaseURL . "takepicture.ashx?" . $qs;
	
	my $result = get $url;
	die "Could not contact GrabzIt" unless defined $result;
	
	my $xml = XML::Simple->new();
	$data = $xml->XMLin($result);
	
	my $message = $data->{Message};
	if(!$message)
	{
		die $message;
	}

	return $data->{ID};
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
	
	my $result = get $url;	
	die "Could not contact GrabzIt" unless defined $result;

	my $xml = XML::Simple->new();
	$data = $xml->XMLin($result);

	return new ScreenShotStatus(($data->{Processing} eq GrabzItClient::TrueString), ($data->{Cached} eq GrabzItClient::TrueString), ($data->{Expired} eq GrabzItClient::TrueString), $data->{Message});
}

#
#This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.
#
#id - The unique identifier of the screenshot, returned by the callback handler or the TakePicture method
#
#This function returns the screenshot
#
sub GetPicture($)
{
	my ($self, $id) = @_;	
	
	my $url = GrabzItClient::WebServicesBaseURL . "getpicture.ashx?id=" . $id;
	
	my $result = get $url;		

	return $result;
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

	my $result = get $url;	
	die "Could not contact GrabzIt" unless defined $result;

	my $xml = XML::Simple->new();
	$data = $xml->XMLin($result);

	my $message = $data->{Message};
	if(!$message)
	{
		die $message;
	}

	@result = ();

	foreach my $cookie (@{ $data->{Cookies}->{Cookie}})
	{
		$grabzItCookie = new GrabzItCookie($cookie->{Name}, $cookie->{Value}, $cookie->{Domain}, $cookie->{Path}, $cookie->{HttpOnly}, $cookie->{Expires}, $cookie->{Type});		
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
	
	$name ||= "";
	$domain ||= "";
	$value ||= "";
	$path ||= "/";
	$httponly ||= 0;
	$expires ||= "";
	
	$sig =  md5_hex($self->{_applicationSecret}."|".$name."|".$domain."|".$value."|".$path."|".$httponly."|".$expires."|0");

	$qs = "key=" .$self->{_applicationKey}."&domain=".uri_escape($domain)."&name=".uri_escape($name)."&value=".uri_escape($value)."&path=".uri_escape($path)."&httponly=".$httponly."&expires=".$expires."&sig=".$sig;

	my $url = GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs;

	my $result = get $url;	
	die "Could not contact GrabzIt" unless defined $result;

	my $xml = XML::Simple->new();
	$data = $xml->XMLin($result);

	my $message = $data->{Message};
	if(!$message)
	{
		die $message;
	}

	return ($data->{Result} eq GrabzItClient::TrueString);
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

	my $result = get $url;	
	die "Could not contact GrabzIt" unless defined $result;

	my $xml = XML::Simple->new();
	$data = $xml->XMLin($result);

	my $message = $data->{Message};
	if(!$message)
	{
		die $message;
	}

	return ($data->{Result} eq GrabzItClient::TrueString);
}
1;