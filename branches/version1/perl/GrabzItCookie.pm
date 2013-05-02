#!/usr/bin/perl

package GrabzItCookie;

sub new
{
    my $class = shift;
    my $self = {
        _name => shift,
        _value => shift,
        _domain      => shift,
        _path      => shift,
        _httponly      => shift,
        _expires      => shift,
        _type      => shift,
    };
    bless $self, $class;
    return $self;
}

sub getName {
    my ($self) = @_;	
    
    return $self->{_name};
}

sub getValue {
    my ($self) = @_;	
    
    return $self->{_value};
}

sub getDomain {
    my ($self) = @_;	
    
    return $self->{_domain};
}

sub getPath {
    my ($self) = @_;	
    
    return $self->{_path};
}

sub getHttpOnly {
    my ($self) = @_;	
    
    return $self->{_httponly};
}

sub getExpires {
    my ($self) = @_;	
    
    return $self->{_expires};
}

sub getType {
    my ($self) = @_;	
    
    return $self->{_type};
}
1;