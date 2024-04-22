#!/usr/bin/perl 

package GrabzItVideoOptions;

use GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    $self->{"browserWidth"} = 0;
    $self->{"browserHeight"} = 0;
    $self->{"width"} = 0;
    $self->{"height"} = 0;    
    $self->{"start"} = 0;
    $self->{"duration"} = 10;
    $self->{"framesPerSecond"} = 0;
    $self->{"customWaterMarkId"} = '';
    $self->{"waitForElement"} = '';    
    $self->{"requestAs"} = 0;
    $self->{"noAds"} = 0;
    $self->{"address"} = '';
    $self->{"noCookieNotifications"} = 0;
    $self->{"clickElement"} = '';

    bless $self, $class;

    return $self;
}

#
# The width of the browser in pixels
#
sub browserWidth
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"browserWidth"} = shift;
    }
    return $self->{"browserWidth"};
}

#
# The height of the browser in pixels
#
sub browserHeight
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"browserHeight"} = shift;
    }
    return $self->{"browserHeight"};
}

#
# The width of the resulting video in pixels.
#
sub width
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"width"} = shift;
    }
    return $self->{"width"};
}

#
# The height of the resulting video in pixels.
#
sub height
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"height"} = shift;
    }
    return $self->{"height"};
}

#
# The starting time of the web page that should be converted into a video
#
sub start
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"start"} = shift;
    }
    return $self->{"start"};
}

#
# The length in seconds of the web page that should be converted into a video
#
sub duration
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"duration"} = shift;
    }
    return $self->{"duration"};
}

#
# The number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10
#
sub framesPerSecond
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"framesPerSecond"} = shift;
    }
    return $self->{"framesPerSecond"};
}

#
# The custom watermark to add to the video
#
sub customWaterMarkId
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"customWaterMarkId"} = shift;
    }
    return $self->{"customWaterMarkId"};
}

#
# The CSS selector of the HTML element in the web page that must be visible before the capture is performed
#
sub waitForElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"waitForElement"} = shift;
    }
    return $self->{"waitForElement"};
}

#
# The user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
#
sub requestAs
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"requestAs"} = shift;
    }
    return $self->{"requestAs"};
}

#
# True if adverts should be automatically hidden.
#
sub noAds
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"noAds"} = shift;
    }
    return $self->{"noAds"};
}

#
# True if cookie notification should be automatically hidden.
#
sub noCookieNotifications
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"noCookieNotifications"} = shift;
    }
    return $self->{"noCookieNotifications"};
}

#
# The URL to execute the HTML code in.
#
sub address
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"address"} = shift;
    }
    return $self->{"address"};
}

#
# The CSS selector of the HTML element in the web page to click
#
sub clickElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"clickElement"} = shift;
    }
    return $self->{"clickElement"};
}

#
#Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
#GrabzIt to perform a HTTP post.
#
#name - The name of the HTTP Post parameter.
#value - The value of the HTTP Post parameter
#
sub AddPostParameter($$)
{
	my ($self, $name, $value) = @_;
	$self->{"post"} = $self->_appendPostParameter($self->{"post"}, $name, $value);
}

sub _getSignatureString($$;$)
{
    my ($self, $applicationSecret, $callBackURL, $url) = @_;
    
    $url ||= '';
    
    $urlParam = '';
    if ($url ne '')
    {
        $urlParam = $url."|";
    }
    
    $callBackURLParam = '';
    if ($callBackURL ne '')
    {
        $callBackURLParam = $callBackURL;
    }
    
    return $applicationSecret."|". $urlParam . $callBackURLParam .
    "|".$self->browserHeight()."|".$self->browserWidth()."|".$self->customId()."|".$self->customWaterMarkId().
    "|".$self->start()."|".$self->requestAs()."|".$self->country()."|".$self->exportURL()."|".$self->waitForElement().
    "|".$self->encryptionKey()."|".$self->noAds()."|".$self->{"post"}."|".$self->proxy()."|".$self->address()."|".$self->noCookieNotifications().
    "|".$self->clickElement()."|".$self->framesPerSecond()."|".$self->duration()."|".$self->width()."|".$self->height();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'bwidth'} = $self->browserWidth();
    $params->{'bheight'} = $self->browserHeight();
    $params->{'duration'} = $self->duration();
    $params->{'start'} = $self->start();
    $params->{'fps'} = $self->framesPerSecond();
    $params->{'customwatermarkid'} = $self->customWaterMarkId();
    $params->{'requestmobileversion'} = $self->requestAs();
    $params->{'waitfor'} = $self->waitForElement();   
    $params->{'noads'} = $self->noAds();
    $params->{'post'} = $self->{"post"};
    $params->{'nonotify'} = $self->noCookieNotifications();
    $params->{'address'} = $self->address();
    $params->{'click'} = $self->clickElement();
    $params->{'width'} = $self->width();
    $params->{'height'} = $self->height();   

    return $params;
}
1;