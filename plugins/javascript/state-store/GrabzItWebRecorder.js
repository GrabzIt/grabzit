function GrabzItWebRecorder()
{
	this.previousChildCount = 0;
	this.decodePointer = 0;
	this.decodeDone = false;

	this.tokenize = function(encoded)
	{
		var parts = encoded.split('|');
		var tokenized = new Array();
		
		for(var i = 0; i < parts.length; i++)
		{
			if (tokenized[i] != null)
			{
				continue;
			}
			var found = false;
			var subject = parts[i];
			
			if (subject.length <= 1)
			{
				tokenized[i] = subject;
				continue;
			}
			
			for (var j = i + 1; j < parts.length; j++)
			{
				var compare = parts[j];
				
				if (compare == subject)
				{
					tokenized[j] = i;
				}
			}
			if (!found)
			{
				tokenized[i] = subject;
			}
		}
		
		return tokenized.join('|');
	};
	this.untokenize = function(encoded)
	{
		var parts = encoded.split('|');
		var untokenized = new Array();
		
		for(var i = 0; i < parts.length; i++)
		{
			var subject = parts[i];
			var iSubject = parseInt(subject);
			if (iSubject == subject)
			{
				untokenized[i] = parts[iSubject];
			}
			else
			{
				untokenized[i] = subject;
			}
		}
		
		return untokenized.join('|');
	};
	this.record = function()
	{
		var data = this.getQueryStringValue("grabzit");
		if (data == '')
		{
			this.shadowCopy();
			return;
		}	
		
		var decompressed  = LZString.decompressFromBase64(data);
		var untokenized = this.untokenize(decompressed);			
		var decoded = this.decode(untokenized);
		var diffNode = document.createElement("html");
		diffNode.innerHTML = decoded;
		HTMLReducer.expand(diffNode, document.documentElement);
	};
	this.appendURL = function(url)
	{
		if (url == null)
		{
			url = location.href;
		}
		var state = this.getState();
		if (url.indexOf('?') == -1)
		{
			url += '?';		
		}
		else
		{
			url += '&';
		}
		return url + 'grabzit=' + state;
	};
	this.getState = function()
	{
		var container = document.documentElement;
		var shadow = document.getElementById('grabzit-shadow');
		
		if (shadow == null)
		{
			//if its larger than this it could cause issues
			alert('The inital state of the web page has not been recorded. Please check that no JavaScript code is overwriting the onload event.');
			
			return '';	
		}
		
		shadow = shadow.childNodes[0];
		
		HTMLReducer.shrink(container, shadow);
		
		var encoded = '';
		encoded += this.encodeTagName(container.tagName, encoded);
		var childCount = 0;
		
		for(var i = 0;i<container.childNodes.length;i++)
		{
			if (container.childNodes[i].id != "grabzit-shadow")
			{
				childCount++;
			}
		}
		
		encoded += ':' + childCount;
		
		if (childCount > 0)
		{
			encoded += '|';
		}	
		
		encoded += this.encodeChildren(shadow);
		var tokenized = this.tokenize(encoded);
		var compressed = LZString.compressToBase64(tokenized);	
		
		if (compressed.length > 1024)
		{
			//if its larger than this it could cause issues
			alert('The size of the web data you are trying to send to GrabzIt is too large. Try to reduce this by specifiying a id of a element that more tightly wraps the data you are interested in recording.');
			
			return '';
		}	
		return compressed;
	};
	this.shadowCopy = function()
	{
		var doc = document.documentElement.cloneNode(true);
		var shadow = document.createElement("div");
		shadow.setAttribute("id", "grabzit-shadow");
		shadow.setAttribute("style", "display:none");
		shadow.appendChild(doc);
		
		document.documentElement.appendChild(shadow);
	};
	this.getQueryStringValue = function(key) {  
	  return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));  
	};
	this.encodeChildren = function(container)
	{
		var encoded = '';	
			
		HTMLReducer.filterEmptyNodes(container);	
			
		for(var i = 0;i < container.childNodes.length;i++)
		{
			var node = container.childNodes[i];
			var tagName = node.tagName;
			var innerText = '';

			if (tagName === undefined || node.id == "grabzit-shadow")
			{
				continue;
			}

			if (encoded != '' && encoded[encoded.length-1] != '|')
			{
				encoded += '|';
			}

			encoded = this.encodeTagName(tagName, encoded);

			HTMLReducer.filterEmptyNodes(node);
			
			if (node.childNodes.length > 0)
			{
				encoded += ':' + node.childNodes.length;
			}

			encoded += '|';

			if (node.value != null && node.value != '')
			{
				encoded += 'v=' + encodeURIComponent(node.value) + '|';
			}

			if (node.firstChild != null && node.firstChild.nodeValue != null && node.firstChild.nodeValue.replace(/(\r\n|\n|\r)/gm,"") != '')
			{
				encoded += 'C=' + encodeURIComponent(node.firstChild.nodeValue) + '|';
			}

			for(var j = 0;j < node.attributes.length;j++)
			{
				var attribute = node.attributes[j];
				var name = attribute.name;
				var value = attribute.value;

				if (name == 'onclick' || name == 'value')
				{
					//ignore all events and values
					continue;
				}

				if (encoded != '' && encoded[encoded.length-1] != '|')
				{
					encoded += '|';
				}

				if (name == 'type')
				{
					encoded += 't=';

					if (value == 'text')
					{
						encoded += 't';
					}
					else if (value == 'button')
					{
						encoded += 'b';
					}
					else
					{
						encoded += encodeURIComponent(value);
					}
				}
				else if (name == 'method')
				{
					encoded += 'm=';
					
					if (value == 'get')
					{
						encoded += 'g';
					}
					else if (value == 'post')
					{
						encoded += 'p';
					}
					else
					{
						encoded += encodeURIComponent(value);
					}				
				}
				else if (name == 'name')
				{
					encoded += 'n' + '=' + encodeURIComponent(value);
				}
				else if (name == 'id')
				{
					encoded += 'i' + '=' + encodeURIComponent(value);
				}
				else if (name == 'href')
				{
					encoded += 'h' + '=' + encodeUrl(value);
				}
				else if (name == 'class')
				{
					encoded += 'c' + '=' + encodeURIComponent(value);
				}
				else
				{
					encoded += name + '=' + encodeURIComponent(value);
				}
			}

			if (node.childNodes.length > 0)
			{
				if (encoded != '' && encoded[encoded.length-1] != '|')
				{
					encoded += '|';
				}
				encoded += this.encodeChildren(node);
			}
		}
		return encoded;
	};
	this.encodeTagName = function(tagName, encoded)
	{
		tagName = tagName.toLowerCase();
		
		if (tagName == 'body')
		{
			encoded += 'y';
		}		
		else if (tagName == 'br')
		{
			encoded += 'B';
		}
		else if (tagName == 'head')
		{
			encoded += 'h';
		}		
		else if (tagName == 'html')
		{
			encoded += 'H';
		}			
		else if (tagName == 'img')
		{
			encoded += 'I';
		}
		else if (tagName == 'input')
		{
			encoded += 'i';
		}
		else if (tagName == 'li')
		{
			encoded += 'l';
		}
		else if (tagName == 'span')
		{
			encoded += 's';
		}
		else if (tagName == 'script')
		{
			encoded += 'S';
		}					
		else if (tagName == 'fieldset')
		{
			encoded += 'f';
		}
		else if (tagName == 'form')
		{
			encoded += 'F';
		}	
		else if (tagName == 'ul')
		{
			encoded += 'u';
		}
		else
		{
			encoded += tagName;
		}
		
		return encoded;
	};
	this.decodeAttributeValue = function(attr, attrValue)
	{
		if (attr == 't')
		{
			if (attrValue == 't')
			{
				return 'text';
			}
			if (attrValue == 'b')
			{
				return 'button';
			}
		}
		if (attr == 'm')
		{
			if (attrValue == 'g')
			{
				return 'get';
			}
			if (attrValue == 'p')
			{
				return 'post';
			}
		}	
		if (attr == 'h')
		{
			return this.decodeUrl(attrValue);
		}

		return decodeURIComponent(attrValue);
	};
	this.decodeAttribute = function(attr)
	{
		if (attr == 'c')
		{
			return 'class';
		}
		if (attr == 't')
		{
			return 'type';
		}
		if (attr == 'v')
		{
			return 'value';
		}
		if (attr == 'm')
		{
			return 'method';
		}	
		if (attr == 'n')
		{
			return 'name';
		}
		if (attr == 'h')
		{
			return 'href';
		}
		if (attr == 'i')
		{
			return 'id';
		}

		return attr;
	};
	this.endTag = function(html, tagName)
	{
		if (html != '' && html[html.length-1] != '>')
		{
			html += '>';
		}
		return html;
	};
	this.closeTag = function(html, tagName)
	{
		if (html != '' && tagName != '')
		{
			html += '</' + tagName + '>';
		}
		return html;
	};
	this.encodeUrl = function(url)
	{
		if (document.location.hostname)
		{
			if (url.indexOf('http://'+document.location.hostname) == 0)
			{
				return '^1' + url.substring(('http://'+document.location.hostname).length, url.length);
			}
			if (url.indexOf('https://'+document.location.hostname) == 0)
			{
				return '^2' + url.substring(('https://'+document.location.hostname).length, url.length);
			}	
			if (url.indexOf(document.location.hostname) == 0)
			{
				return '^3' + url.substring(document.location.hostname.length, url.length);
			}	
		}	
		if (url.indexOf('http://www.') == 0)
		{
			return '^4' + url.substring(11, url.length);
		}
		if (url.indexOf('https://www.') == 0)
		{
			return '^5' + url.substring(12, url.length);
		}	
		if (url.indexOf('www.') == 0)
		{
			return '^6' + url.substring(4, url.length);
		}
		//Also do the current domain
		return url;
	};
	this.decodeUrl = function(url)
	{
		if (document.location.hostname)
		{
			if (url.indexOf('^1') == 0)
			{
				return 'http://'+document.location.hostname + url.substring(2, url.length);
			}
			if (url.indexOf('^2') == 0)
			{
				return 'https://'+document.location.hostname + url.substring(2, url.length);
			}	
			if (url.indexOf('^3') == 0)
			{
				return document.location.hostname + url.substring(11, url.length);
			}
		}	
		if (url.indexOf('^4') == 0)
		{
			return 'http://www.' + url.substring(2, url.length);
		}
		if (url.indexOf('^5') == 0)
		{
			return 'https://www.' + url.substring(2, url.length);
		}	
		if (url.indexOf('^6') == 0)
		{
			return 'www.' + url.substring(2, url.length);
		}
		//Also do the current domain
		return url;
	};
	this.decode = function(encoded)
	{
		previousChildCount = 0;
		this.decodePointer = 0;
		var parts = encoded.split('|');
		return this.decodeChildren(parts, 1, false);
	};
	this.decodeChildren = function(parts, limit, diving, that)
	{
		if (that == null)
		{
			that = this;
		}
		
		var decoded = '';
		var tagName = '';
		var tagContent = '';
		var childCount = 0;
		var decodeOffset = 0;
		var currentLoopCount=0;
		var openTags = 0;
		var waiting = false;

		if (diving)
		{
			that.decodePointer--;
		}

		while(that.decodePointer < parts.length)
		{
			currentLoopCount++;
			var part = parts[that.decodePointer];
			that.decodePointer++;
			var index = part.indexOf('=');
			if (index > -1)
			{
				if (decoded != '' && decoded[decoded.length-1] != ' ')
				{
					decoded += ' ';
				}

				var attr = part.substring(0, index);

				if (attr == 'C')
				{
					//it's content!
					tagContent = decodeURIComponent(part.substring(index+1, part.length));
					continue;
				}

				decoded += that.decodeAttribute(attr);
				decoded += '="';
				decoded += that.decodeAttributeValue(attr, part.substring(index+1, part.length));
				decoded += '"';
			}
			else if (part != '')
			{
				var countIndex = part.indexOf(':');
				if (countIndex > -1)
				{
					childCount = parseInt(part.substring(countIndex + 1, part.length));
					part = part.substring(0, countIndex);
				}
				else
				{
					childCount = 0;
				}

				decoded = that.endTag(decoded, tagName);

				if (!diving || currentLoopCount > 1)
				{
					decoded += tagContent;
					tagContent = '';				
					
					if (that.previousChildCount > 0)
					{
						var lastCount = that.previousChildCount;
						that.previousChildCount = childCount;
						decoded += that.decodeChildren(parts, lastCount, true, that);
						
						if (openTags > 0)
						{
							openTags--;
							decoded = that.closeTag(decoded, tagName);
						}		

						if (decodeOffset >= limit)
						{
							that.previousChildCount = 0;
							return decoded;
						}
						
						continue;
					}
					else
					{
						that.previousChildCount = childCount;
					}

					if (openTags > 0)
					{
						openTags--;
						decoded = that.closeTag(decoded, tagName);
					}
					
					if (decodeOffset >= limit)
					{		
						that.previousChildCount = 0;				
						that.decodePointer--;
						return decoded;
					}

					tagName = '';
				}
				
				decodeOffset++;			

				if (part == 'B')
				{
					//stops empty close tags
					tagName = '';
					decoded += '<br/>';
					continue;
				}

				if (part == 'y')
				{
					tagName = 'body';
				}			
				else if (part == 'h')
				{
					tagName = 'head';
				}
				else if (part == 'H')
				{
					tagName = 'html';
				}					
				else if (part == 'i')
				{
					tagName = 'input';
				}
				else if (part == 'I')
				{
					tagName = 'img';
				}
				else if (part == 'l')
				{
					tagName = 'li';
				}
				else if (part == 'f')
				{
					tagName = 'fieldset';
				}
				else if (part == 'F')
				{
					tagName = 'form';
				}			
				else if (part == 's')
				{
					tagName = 'span';
				}
				else if (part == 'S')
				{
					tagName = 'script';
				}			
				else if (part == 'u')
				{
					tagName = 'ul';
				}
				else
				{
					tagName = part;
				}
				
				openTags++;
				decoded += '<' + tagName;
			}
		}

		if (!that.decodeDone && that.decodePointer == parts.length)
		{
			that.decodeDone = true;
			decoded = that.endTag(decoded, tagName);
			
			openTags--;
			decoded = that.closeTag(decoded, tagName);
		}
		return decoded;
	};	
	var __construct = function(that) 
	{
		var old = window.onload;
		window.onload = function()
		{
			if (old != null)
			{
				old();
			}
			that.record();
		};	
	}(this)	
}

var GrabzItWebRecorder = new GrabzItWebRecorder();