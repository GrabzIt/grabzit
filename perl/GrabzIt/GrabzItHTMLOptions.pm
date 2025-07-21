#!/usr/bin/perl 

package GrabzItHTMLOptions;

use GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    
    $self->{"browserWidth"} = 0;
    $self->{"browserHeight"} = 0;
    $self->{"waitForElement"} = '';    
    $self->{"requestAs"} = 0;
    $self->{"noAds"} = 0;
    $self->{"address"} = '';
    $self->{"noCookieNotifications"} = 0;
    $self->{"clickElement"} = '';
    $self->{"jsCode"} = '';

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
# The JavaScript code that will be execute in the web page before the capture is performed
#
sub jsCode
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"jsCode"} = shift;
    }
    return $self->{"jsCode"};
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
    "|".$self->browserHeight()."|".$self->browserWidth()."|".$self->customId()."|".$self->delay().
    "|".$self->requestAs()."|".$self->country()."|".$self->exportURL()."|".$self->waitForElement().
    "|".$self->encryptionKey()."|".$self->noAds()."|".$self->{"post"}."|".$self->proxy().
    "|".$self->address()."|".$self->noCookieNotifications()."|".$self->clickElement().
    "|".$self->jsCode();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'bwidth'} = $self->browserWidth();
    $params->{'bheight'} = $self->browserHeight();
    $params->{'delay'} = $self->delay();
    $params->{'requestmobileversion'} = $self->requestAs();
    $params->{'waitfor'} = $self->waitForElement();   
    $params->{'noads'} = $self->noAds();
    $params->{'post'} = $self->{"post"};
    $params->{'nonotify'} = $self->noCookieNotifications();
    $params->{'address'} = $self->address();
    $params->{'click'} = $self->clickElement();
    $params->{'jscode'} = $self->jsCode();
    
    return $params;
}
1;