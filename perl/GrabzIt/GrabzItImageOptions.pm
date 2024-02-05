#!/usr/bin/perl 

package GrabzItImageOptions;

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
    $self->{"format"} = '';
    $self->{"targetElement"} = '';
    $self->{"hideElement"} = '';
    $self->{"waitForElement"} = '';    
    $self->{"requestAs"} = 0;
    $self->{"customWaterMarkId"} = '';
    $self->{"quality"} = -1;
    $self->{"transparent"} = 0;
    $self->{"noAds"} = 0;
    $self->{"address"} = '';
    $self->{"noCookieNotifications"} = 0;
    $self->{"hd"} = 0;
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
# The height of the browser in pixels. Use -1 to screenshot the whole web page
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
# The width of the resulting screenshot in pixels. Use -1 to not reduce the width of the screenshot
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
# The height of the resulting screenshot in pixels. Use -1 to not reduce the height of the screenshot
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
# The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
#
sub format
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"format"} = shift;
    }
    return $self->{"format"};
}

#
# The number of milliseconds to wait before creating the capture
#
sub delay
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"delay"} = shift;
    }
    return $self->{"delay"};
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
# The CSS selector of the only HTML element in the web page to capture
#
sub targetElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"targetElement"} = shift;
    }
    return $self->{"targetElement"};
}

#
# The CSS selector(s) of the one or more HTML elements in the web page to hide
#
sub hideElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"hideElement"} = shift;
    }
    return $self->{"hideElement"};
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
# The custom watermark to add to the screenshot
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
# The quality of the screenshot where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
#
sub quality
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"quality"} = shift;
    }
    return $self->{"quality"};
}

#
# True if the image capture should be transparent. This is only compatible with png and tiff images
#
sub transparent
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"transparent"} = shift;
    }
    return $self->{"transparent"};
}

#
# True if the image capture should be in high definition.
#
sub hd
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"hd"} = shift;
    }
    return $self->{"hd"};
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
    "|".$self->format()."|".$self->height()."|".$self->width()."|".$self->browserHeight()."|".$self->browserWidth()."|".$self->customId()."|".$self->delay().
    "|".$self->targetElement()."|".$self->customWaterMarkId()."|".$self->requestAs()."|".$self->country()."|".$self->quality()."|".$self->hideElement()."|".$self->exportURL()."|".$self->waitForElement()."|".$self->transparent()."|".$self->encryptionKey()."|".$self->noAds()."|".$self->{"post"}."|".$self->proxy()."|".$self->address()."|".$self->noCookieNotifications()."|".$self->hd().
    "|".$self->clickElement();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'width'} = $self->width();
    $params->{'height'} = $self->height();
    $params->{'format'} = $self->format();
    $params->{'bwidth'} = $self->browserWidth();
    $params->{'bheight'} = $self->browserHeight();
    $params->{'delay'} = $self->delay();
    $params->{'target'} = $self->targetElement();
    $params->{'hide'} = $self->hideElement();
    $params->{'customwatermarkid'} = $self->customWaterMarkId();
    $params->{'requestmobileversion'} = $self->requestAs();
    $params->{'quality'} = $self->quality();
    $params->{'waitfor'} = $self->waitForElement();   
    $params->{'transparent'} = $self->transparent(); 
    $params->{'noads'} = $self->noAds();
    $params->{'post'} = $self->{"post"};
    $params->{'nonotify'} = $self->noCookieNotifications();
    $params->{'address'} = $self->address();
    $params->{'hd'} = $self->hd();
    $params->{'click'} = $self->clickElement();
    
    return $params;
}
1;