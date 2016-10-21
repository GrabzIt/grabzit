#!/usr/bin/perl 

package GrabzItRequest;

sub new
{
    my $class = shift;       
    
    my $self = {
        url => shift,
        isPost  => shift,
        options  => shift,
        data  => shift,
    };
    
    $self->{"data"} ||= '';
    
    bless $self, $class;

    return $self;
}

sub _getTargetUrl()
{
    my ($self) = @_;
    
    if ($self->{"isPost"} == 1)
    {
        return '';
    }
    
    return $self->{"data"};
}
1;