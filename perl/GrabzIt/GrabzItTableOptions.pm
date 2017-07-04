#!/usr/bin/perl 

package GrabzItTableOptions;

use GrabzIt::GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    
    $self->{"tableNumberToInclude"} = 1;
    $self->{"format"} = "csv";
    $self->{"includeHeaderNames"} = 1;
    $self->{"includeAllTables"} = 1;
    $self->{"targetElement"} = '';
    $self->{"requestAs"} = 0;
    
    bless $self, $class;

    return $self;    
}

#
# The index of the table to be converted, were all tables in a web page are ordered from the top of the web page to bottom
#
sub tableNumberToInclude
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"tableNumberToInclude"} = shift;
    }
    return $self->{"tableNumberToInclude"};
}

#
# The format the table should be in: csv, xlsx or json
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
# True to include header names into the table
#
sub includeHeaderNames
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeHeaderNames"} = shift;
    }
    return $self->{"includeHeaderNames"};
}

#
# True to extract every table on the web page into a separate spreadsheet sheet. Only available with the XLSX and JSON formats
#
sub includeAllTables
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeAllTables"} = shift;
    }
    return $self->{"includeAllTables"};
}

#
# The id of the only HTML element in the web page that should be used to extract tables from
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
    "|".$self->customId()."|".$self->tableNumberToInclude() ."|".$self->includeAllTables()."|".$self->includeHeaderNames()."|".$self->targetElement()."|".
    $self->format()."|".$self->requestAs()."|".$self->country()."|".$self->exportURL();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'includeAllTables'} = $self->includeAllTables();
    $params->{'includeHeaderNames'} = $self->includeHeaderNames();
    $params->{'format'} = $self->format();
    $params->{'tableToInclude'} = $self->tableNumberToInclude();
    $params->{'target'} = $self->targetElement();
    $params->{'requestmobileversion'} = $self->requestAs();    

    return $params;
}
1;