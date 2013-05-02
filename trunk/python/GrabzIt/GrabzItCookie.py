#!/usr/bin/python

class GrabzItCookie:

	def __init__(self, name, value, domain, path, httponly, expires, type):
		self.Name = name
		self.Value = value
		self.Domain = domain
		self.Path = path
		self.HttpOnly = httponly
		self.Expires = expires
		self.Type = type