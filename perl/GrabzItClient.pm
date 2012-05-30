#!/usr/bin/perl 

package GrabzItClient;

use Digest::MD5  qw(md5_hex);
use LWP::Simple;
use URI::Escape;
use XML::Simple;

use constant WebServicesBaseURL => "http://localhost:1313/services/";

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
#format - The format the screenshot should be in: jpg, gif, png
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
1;