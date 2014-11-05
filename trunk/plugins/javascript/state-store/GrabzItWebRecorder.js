var GrabzItLZString={_keyStr:"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",_f:String.fromCharCode,compressToBase64:function(e){if(e==null)return"";var t="";var n,r,i,s,o,u,a;var f=0;e=GrabzItLZString.compress(e);while(f<e.length*2){if(f%2==0){n=e.charCodeAt(f/2)>>8;r=e.charCodeAt(f/2)&255;if(f/2+1<e.length)i=e.charCodeAt(f/2+1)>>8;else i=NaN}else{n=e.charCodeAt((f-1)/2)&255;if((f+1)/2<e.length){r=e.charCodeAt((f+1)/2)>>8;i=e.charCodeAt((f+1)/2)&255}else r=i=NaN}f+=3;s=n>>2;o=(n&3)<<4|r>>4;u=(r&15)<<2|i>>6;a=i&63;if(isNaN(r)){u=a=64}else if(isNaN(i)){a=64}t=t+GrabzItLZString._keyStr.charAt(s)+GrabzItLZString._keyStr.charAt(o)+GrabzItLZString._keyStr.charAt(u)+GrabzItLZString._keyStr.charAt(a)}return t},decompressFromBase64:function(e){if(e==null)return"";var t="",n=0,r,i,s,o,u,a,f,l,c=0,h=GrabzItLZString._f;e=e.replace(/[^A-Za-z0-9\+\/\=]/g,"");while(c<e.length){u=GrabzItLZString._keyStr.indexOf(e.charAt(c++));a=GrabzItLZString._keyStr.indexOf(e.charAt(c++));f=GrabzItLZString._keyStr.indexOf(e.charAt(c++));l=GrabzItLZString._keyStr.indexOf(e.charAt(c++));i=u<<2|a>>4;s=(a&15)<<4|f>>2;o=(f&3)<<6|l;if(n%2==0){r=i<<8;if(f!=64){t+=h(r|s)}if(l!=64){r=o<<8}}else{t=t+h(r|i);if(f!=64){r=s<<8}if(l!=64){t+=h(r|o)}}n+=3}return GrabzItLZString.decompress(t)},compressToUTF16:function(e){if(e==null)return"";var t="",n,r,i,s=0,o=GrabzItLZString._f;e=GrabzItLZString.compress(e);for(n=0;n<e.length;n++){r=e.charCodeAt(n);switch(s++){case 0:t+=o((r>>1)+32);i=(r&1)<<14;break;case 1:t+=o(i+(r>>2)+32);i=(r&3)<<13;break;case 2:t+=o(i+(r>>3)+32);i=(r&7)<<12;break;case 3:t+=o(i+(r>>4)+32);i=(r&15)<<11;break;case 4:t+=o(i+(r>>5)+32);i=(r&31)<<10;break;case 5:t+=o(i+(r>>6)+32);i=(r&63)<<9;break;case 6:t+=o(i+(r>>7)+32);i=(r&127)<<8;break;case 7:t+=o(i+(r>>8)+32);i=(r&255)<<7;break;case 8:t+=o(i+(r>>9)+32);i=(r&511)<<6;break;case 9:t+=o(i+(r>>10)+32);i=(r&1023)<<5;break;case 10:t+=o(i+(r>>11)+32);i=(r&2047)<<4;break;case 11:t+=o(i+(r>>12)+32);i=(r&4095)<<3;break;case 12:t+=o(i+(r>>13)+32);i=(r&8191)<<2;break;case 13:t+=o(i+(r>>14)+32);i=(r&16383)<<1;break;case 14:t+=o(i+(r>>15)+32,(r&32767)+32);s=0;break}}return t+o(i+32)},decompressFromUTF16:function(e){if(e==null)return"";var t="",n,r,i=0,s=0,o=GrabzItLZString._f;while(s<e.length){r=e.charCodeAt(s)-32;switch(i++){case 0:n=r<<1;break;case 1:t+=o(n|r>>14);n=(r&16383)<<2;break;case 2:t+=o(n|r>>13);n=(r&8191)<<3;break;case 3:t+=o(n|r>>12);n=(r&4095)<<4;break;case 4:t+=o(n|r>>11);n=(r&2047)<<5;break;case 5:t+=o(n|r>>10);n=(r&1023)<<6;break;case 6:t+=o(n|r>>9);n=(r&511)<<7;break;case 7:t+=o(n|r>>8);n=(r&255)<<8;break;case 8:t+=o(n|r>>7);n=(r&127)<<9;break;case 9:t+=o(n|r>>6);n=(r&63)<<10;break;case 10:t+=o(n|r>>5);n=(r&31)<<11;break;case 11:t+=o(n|r>>4);n=(r&15)<<12;break;case 12:t+=o(n|r>>3);n=(r&7)<<13;break;case 13:t+=o(n|r>>2);n=(r&3)<<14;break;case 14:t+=o(n|r>>1);n=(r&1)<<15;break;case 15:t+=o(n|r);i=0;break}s++}return GrabzItLZString.decompress(t)},compress:function(e){if(e==null)return"";var t,n,r={},i={},s="",o="",u="",a=2,f=3,l=2,c="",h=0,p=0,d,v=GrabzItLZString._f;for(d=0;d<e.length;d+=1){s=e.charAt(d);if(!Object.prototype.hasOwnProperty.call(r,s)){r[s]=f++;i[s]=true}o=u+s;if(Object.prototype.hasOwnProperty.call(r,o)){u=o}else{if(Object.prototype.hasOwnProperty.call(i,u)){if(u.charCodeAt(0)<256){for(t=0;t<l;t++){h=h<<1;if(p==15){p=0;c+=v(h);h=0}else{p++}}n=u.charCodeAt(0);for(t=0;t<8;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}else{n=1;for(t=0;t<l;t++){h=h<<1|n;if(p==15){p=0;c+=v(h);h=0}else{p++}n=0}n=u.charCodeAt(0);for(t=0;t<16;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}delete i[u]}else{n=r[u];for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}r[o]=f++;u=String(s)}}if(u!==""){if(Object.prototype.hasOwnProperty.call(i,u)){if(u.charCodeAt(0)<256){for(t=0;t<l;t++){h=h<<1;if(p==15){p=0;c+=v(h);h=0}else{p++}}n=u.charCodeAt(0);for(t=0;t<8;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}else{n=1;for(t=0;t<l;t++){h=h<<1|n;if(p==15){p=0;c+=v(h);h=0}else{p++}n=0}n=u.charCodeAt(0);for(t=0;t<16;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}delete i[u]}else{n=r[u];for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}}n=2;for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}while(true){h=h<<1;if(p==15){c+=v(h);break}else p++}return c},decompress:function(e){if(e==null)return"";if(e=="")return null;var t=[],n,r=4,i=4,s=3,o="",u="",a,f,l,c,h,p,d,v=GrabzItLZString._f,m={string:e,val:e.charCodeAt(0),position:32768,index:1};for(a=0;a<3;a+=1){t[a]=a}l=0;h=Math.pow(2,2);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}switch(n=l){case 0:l=0;h=Math.pow(2,8);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}d=v(l);break;case 1:l=0;h=Math.pow(2,16);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}d=v(l);break;case 2:return""}t[3]=d;f=u=d;while(true){if(m.index>m.string.length){return""}l=0;h=Math.pow(2,s);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}switch(d=l){case 0:l=0;h=Math.pow(2,8);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}t[i++]=v(l);d=i-1;r--;break;case 1:l=0;h=Math.pow(2,16);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}t[i++]=v(l);d=i-1;r--;break;case 2:return u}if(r==0){r=Math.pow(2,s);s++}if(t[d]){o=t[d]}else{if(d===i){o=f+f.charAt(0)}else{return null}}u+=o;t[i++]=f+o.charAt(0);r--;f=o;if(r==0){r=Math.pow(2,s);s++}}}};
function GrabzItHTMLReducer()
{
	this.expand = function(a, b, that)
	{	
		if (a == null)
		{
			return;
		}
		
		if (that == null)
		{
			that = this;
		}	

		var excludedNodes = new Array();

		if (a.tagName == b.tagName)
		{		
			var aNodeText = that.getNodeText(a);
			var bNodeText = that.getNodeText(b);
			
			if (aNodeText != bNodeText && bNodeText != '' && aNodeText != '')
			{
				excludedNodes.push(b);
				b.innerHTML = a.innerHTML;
			}
			
			if (a.value != b.value && a.value != '')
			{
				b.value = a.value;
			}
			
			if (typeof b.attributes != 'undefined' && typeof a.attributes != 'undefined')
			{		
				var namesToRemove = new Array();
				for(var i = 0;i < a.attributes.length;i++)
				{
					var aAtt = a.attributes[i];
					
					if (aAtt.nodeName == 'grabzit-empty')
					{
						b = that.removeAllTextNodes(b, function(o){excludedNodes.push(o);});
					}
					
					if (aAtt.value == '')
					{
						namesToRemove.push(aAtt.nodeName);
					}
					else
					{
						b.setAttribute(aAtt.nodeName, aAtt.value);
					}
				}
				
				for(var i = 0;i < namesToRemove.length;i++)
				{				
					b.removeAttribute(namesToRemove[i]);
				}				
			}
		}
		else
		{				
			if (b.parentNode == null || !that.containsNode(b.parentNode.childNodes, a))
			{				
				if (a.nodeType != 3)
				{				
					var add = a.cloneNode(true);
					add.setAttribute('grabzit-added','');
					
					var parent = that.getCorrectParent(a, b);					
					parent.appendChild(add);
				}
			}
			else if (a.nodeType != 3)
			{
				b.parentNode.removeChild(b);
			}
			return;
		}	
		
		that.chooseNextNodes(a, b, excludedNodes, that.expand);
	};
	this.getCorrectParent = function(a, b)
	{
		if (a.parentNode.tagName == b.tagName && this.attributesEqual(a.parentNode, b))
		{
			return b;
		}
		
		return this.getCorrectParent(a, b.parentNode);
	};
	this.attributesEqual = function(a, b)
	{
		if (b.attributes.length != a.attributes.length)
		{
			return false;
		}
		
		for(var i = 0;i < a.attributes.length;i++)
		{
			var found = false;
			var aAtt = a.attributes[i];
			for(var j = 0;j < b.attributes.length;j++)
			{
				var bAtt = b.attributes[i];
				
				if (aAtt.nodeName == bAtt.nodeName && aAtt.value == bAtt.value)
				{
					found = true;
				}
			}
			
			if (!found)
			{
				return false;
			}
		}
		
		return true;
	};
	this.chooseNextNodes = function(a, b, excludedNodes, func)
	{
		this.filterEmptyNodes(a);
		this.filterEmptyNodes(b);
		
		var maximum = a.childNodes.length;
		if (maximum < b.childNodes.length)
		{
			maximum = b.childNodes.length;
		}

		var initialA = a;
		var initialB = b;
		var previousA = a;
		var previousB = b;
		for(var i = 0;i < maximum;i++)
		{
			var nextA = a.childNodes[i];
			var nextB = b.childNodes[i];
			
			if (nextB == null || nextB.nodeType != 1)
			{
				nextB = previousB;
			}
			else
			{
				previousB = nextB;
			}
			if (nextA == null)
			{
				nextA = previousA;
			}
			else
			{
				previousA = nextA;
			}

			if (this.containsNode(excludedNodes, nextA))
			{
				continue;
			}
			
			if (nextB.nodeType != 3 && nextB.hasAttribute('grabzit-added'))
			{
				nextB = b;
			}	

			if (initialA === nextA && initialB === nextB)
			{
				return;
			}
			
			func(nextA, nextB, this);
		}
	};
	this.filterEmptyNodes = function(o)
	{
		for(var i = 0;i < o.childNodes.length;i++)
		{
			var node = o.childNodes[i];
			if (node.nodeType == 3 && this.removeBreaks(node.nodeValue) == '')
			{
				o.removeChild(node);
			}
		}
	};
	this.shrink = function(a, b, that)
	{
		if (a == null || a.getAttribute('id') == 'grabzit-shadow' || (b != null && b.getAttribute('id') == 'grabzit-shadow'))
		{
			return;
		}
		
		if (that == null)
		{
			that = this;
		}

		var excludedNodes = new Array();

		if(a.isEqualNode(b) && a.value == b.value)
		{				
			if (typeof b.attributes != 'undefined')
			{
				var names = new Array();
				for(var i = 0;i < b.attributes.length;i++)
				{
					names.push(b.attributes[i].nodeName);
				}
				for(var i = 0;i < names.length;i++)
				{
					b.removeAttribute(names[i]);
				}
			}
			
			that.setNodeValue(a, b);
			b = that.removeAllTextNodes(b, function(o){excludedNodes.push(o);});
		}
		else if (a.tagName == b.tagName)
		{		
			var aNodeText = that.getNodeText(a);
			var bNodeText = that.getNodeText(b);

			if (aNodeText == bNodeText && aNodeText != '' && bNodeText != '')
			{
				b = that.removeAllTextNodes(b, function(o){excludedNodes.push(o);});
			}

			that.setNodeValue(a, b);
			
			if (aNodeText != bNodeText && bNodeText != '')
			{
				if (aNodeText == '')
				{
					//its been emptied
					b = that.removeAllTextNodes(b, function(o){excludedNodes.push(o);});
					b.setAttribute("grabzit-empty", "");				
				}
				else
				{
					excludedNodes.push(b);
					b.innerHTML = a.innerHTML;
				}
			}			

			if (typeof b.attributes != 'undefined' && typeof a.attributes != 'undefined')
			{
				var attributesToRemove = new Array();
				var namesToRemove = new Array();
				
				for(var j = 0;j < b.attributes.length;j++)
				{
					attributesToRemove.push(b.attributes[j].nodeName);
				}

				for(var i = 0;i < a.attributes.length;i++)
				{
					var aAtt = a.attributes[i];
					attributesToRemove.pop(aAtt.nodeName);
					var found = false;

					for(var j = 0;j < b.attributes.length;j++)
					{
						var bAtt = b.attributes[j];
						if (aAtt.nodeName == bAtt.nodeName)
						{				
							found = true;

							if (aAtt.value != bAtt.value)
							{
								b.setAttribute(bAtt.nodeName, bAtt.value);
							}
							else
							{
								namesToRemove.push(bAtt.nodeName);
							}
							break;
						}
					}

					if (!found)
					{
						b.setAttribute(aAtt.nodeName, aAtt.value);
					}
				}

				for (var j = 0;j < attributesToRemove.length;j++)
				{
					b.setAttribute(attributesToRemove[j], "");
				}
				
				for (var j = 0;j < namesToRemove.length;j++)
				{
					b.removeAttribute(namesToRemove[j]);
				}				
			}
		}
		else
		{
			if (b.parentNode == null || !that.containsNode(b.parentNode.childNodes, a))
			{
				if (a.nodeType != 3)
				{
					var add = a.cloneNode(true);
					add.setAttribute('grabzit-added','');
					
					var parent = that.getCorrectParent(a, b);
					parent.appendChild(add);
				}
			}
			else if (a.nodeType != 3)
			{
				var parent = b.parentNode;
				parent.removeChild(b);
			}
			return;
		}
		
		that.chooseNextNodes(a, b, excludedNodes, that.shrink);
	};
	this.setNodeValue = function(a, b)
	{	
		if (a.tagName == 'INPUT')
		{
			if (a.value != b.value)
			{
				b.setAttribute("value", a.value);				
			}
			else if (a.value == b.value)
			{
				b.value = '';
				b.setAttribute("value", "");
			}
		}
		else if (a.tagName == 'TEXTAREA')
		{			
			if (a.value != b.value)
			{
				b.value = a.value;				
			}
			else if (a.value == b.value)
			{
				b.value = '';
				b.setAttribute("grabzit-empty", "");
			}			
		}	
	};
	this.getNodeText = function(o)
	{
		var text = "";
		for (var i = 0; i < o.childNodes.length; i++) {
			var curNode = o.childNodes[i];
			if (curNode.nodeType == 3) {
				text += curNode.nodeValue;
			}
		}
		
		return this.removeBreaks(text);
	};
	this.removeBreaks = function(text)
	{
		return text.replace(/(\r\n|\n|\r)/gm,"");
	};
	this.removeAllTextNodes = function(o, func)
	{
		for (var i = 0; i < o.childNodes.length; i++) {
			var curNode = o.childNodes[i];
			if (curNode.nodeType == 3) {
				func(curNode.cloneNode(false));
				o.removeChild(curNode);
			}
		}
		return o;
	};
	this.containsNode = function(haystack, needle)
	{
		for(var i = 0;i < haystack.length;i++)
		{
			if(needle.isEqualNode(haystack[i]))
			{
				return true;
			}
		}
		return false;
	};
}
function GrabzItWebRecorder()
{
	this.previousChildCount = 0;
	this.decodePointer = 0;
	this.decodeDone = false;
	this.timeoutIds = new Array();
	this.intervalIds = new Array();
	this.HTMLReducer = new GrabzItHTMLReducer();

	this.freeze = function()
	{
		for(var i = 0;i < this.timeoutIds.length; i++)
		{
			clearTimeout(this.timeoutIds[i]);
		}
		for(var j = 0;j < this.intervalIds.length; j++)
		{
			clearInterval(this.intervalIds[j]);
		}		
		throw new Error('Frozen');
	};
	this.setTimeoutId = function(id)
	{
		this.timeoutIds.push(id);
	};	
	this.setIntervalId = function(id)
	{
		this.intervalIds.push(id);
	};		
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
		
		var decompressed  = GrabzItLZString.decompressFromBase64(data);
		var untokenized = this.untokenize(decompressed);			
		var decoded = this.decode(untokenized);
		alert(decoded);
		var diffNode = document.createElement("html");
		diffNode.innerHTML = decoded;
		this.HTMLReducer.expand(diffNode, document.documentElement);
		this.freeze();
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
		
		this.HTMLReducer.shrink(container, shadow);
		
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
		alert(encoded);
		var tokenized = this.tokenize(encoded);
		var compressed = GrabzItLZString.compressToBase64(tokenized);	
		
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
		var shadow = document.getElementById('grabzit-shadow');
		
		if (shadow == null)
		{
			//only create a shadow copy if one doesn't exist. A user may have specified one.
			var doc = document.documentElement.cloneNode(true);
			shadow = document.createElement("div");
			shadow.setAttribute("id", "grabzit-shadow");
			shadow.setAttribute("style", "display:none");
			shadow.appendChild(doc);
			
			document.documentElement.appendChild(shadow);
		}
	};
	this.getQueryStringValue = function(key) {  
	  return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));  
	};
	this.encodeChildren = function(container)
	{
		var encoded = '';	
			
		this.HTMLReducer.filterEmptyNodes(container);	
			
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

			this.HTMLReducer.filterEmptyNodes(node);
			
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

				if (name == 'value')
				{
					//ignore all events and values
					continue;
				}

				if (encoded != '' && encoded[encoded.length-1] != '|')
				{
					encoded += '|';
				}

				this.encodeAttribute(name, value, encoded);
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
	this.decodeTagName = function(part, tagName, decoded, openTags)
	{
		if (part == 'B')
		{
			//stops empty close tags
			tagName = '';
			decoded += '<br/>';
		}
		else
		{
			if (part == 'd')
			{
				tagName = 'div';
			}
			if (part == 'e')
			{
				tagName = 'select';
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
			else if (part == 'I')
			{
				tagName = 'input';
			}
			else if (part == 'm')
			{
				tagName = 'img';
			}
			else if (part == 'l')
			{
				tagName = 'li';
			}
			else if (part == 'L')
			{
				tagName = 'legend';
			}
			else if (part == 'f')
			{
				tagName = 'fieldset';
			}
			else if (part == 'F')
			{
				tagName = 'form';
			}			
			else if (part == 'n')
			{
				tagName = 'span';
			}
			else if (part == 'o')
			{
				tagName = 'option';
			}
			else if (part == 'S')
			{
				tagName = 'script';
			}			
			else if (part == 't')
			{
				tagName = 'textarea';
			}						
			else if (part == 'U')
			{
				tagName = 'ul';
			}
			else
			{
				//Reserved letters: u, p, b, s, a, i, q
				tagName = part;
			}
			
			openTags++;
			decoded += '<' + tagName;	
		}
		
		return {tagName: tagName, decoded: decoded, openTags: openTags};
	}
	this.encodeTagName = function(tagName, encoded)
	{
		tagName = tagName.toLowerCase();

		if (tagName == 'div')
		{
			encoded += 'd';
		}		
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
			encoded += 'm';
		}
		else if (tagName == 'input')
		{
			encoded += 'I';
		}
		else if (tagName == 'li')
		{
			encoded += 'l';
		}		
		else if (tagName == 'legend')
		{
			encoded += 'L';
		}
		else if (tagName == 'option')
		{
			encoded += 'o';
		}			
		else if (tagName == 'select')
		{
			encoded += 'e';
		}		
		else if (tagName == 'span')
		{
			encoded += 'n';
		}
		else if (tagName == 'script')
		{
			encoded += 'S';
		}
		else if (tagName == 'textarea')
		{
			encoded += 't';
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
			encoded += 'U';
		}
		else
		{
			encoded += tagName;
		}
		
		return encoded;
	};
	this.encodeAttribute = function(name, value, encoded)
	{
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
		else if (name == 'disabled')
		{
			encoded += 'd=';
			
			if (value == 'true')
			{
				encoded += 't';
			}
			else if (value == 'false')
			{
				encoded += 'f';
			}
			else
			{
				encoded += encodeURIComponent(value);
			}				
		}		
		else if (name == 'selected')
		{
			encoded += 'e=';
			
			if (value == 'selected')
			{
				encoded += 's';
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
		else if (name == 'src')
		{
			encoded += 's' + '=' + encodeUrl(value);
		}		
		else if (name == 'style')
		{
			encoded += 'S' + '=' + encodeUrl(value);
		}				
		else if (name == 'title')
		{
			encoded += 'T' + '=' + encodeUrl(value);
		}				
		else if (name == 'class')
		{
			encoded += 'c' + '=' + encodeURIComponent(value);
		}
		else if (name == 'alt')
		{
			encoded += 'a' + '=' + encodeURIComponent(value);
		}		
		else
		{
			encoded += name + '=' + encodeURIComponent(value);
		}	
		
		return encoded;
	}
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
		if (attr == 'd')
		{
			if (attrValue == 't')
			{
				return 'true';
			}
			if (attrValue == 'f')
			{
				return 'false';
			}		
		}
		if (attr == 'e')
		{
			if (attrValue == 's')
			{
				return 'selected';
			}
			return attrValue;
		}

		return decodeURIComponent(attrValue);
	};
	this.decodeAttribute = function(attr)
	{
		if (attr == 'a')
		{
			return 'alt';
		}			
		if (attr == 'c')
		{
			return 'class';
		}
		if (attr == 'd')
		{
			return 'disabled';
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
		if (attr == 's')
		{
			return 'src';
		}		
		if (attr == 'S')
		{
			return 'style';
		}				
		if (attr == 't')
		{
			return 'type';
		}
		if (attr == 'T')
		{
			return 'title';
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

				if ((tagName == 'textarea' && attr == 'v') || attr == 'C')
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

				var dec = that.decodeTagName(part, tagName, decoded, openTags);
				
				tagName = dec.tagName;
				decoded = dec.decoded;
				openTags = dec.openTags;
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
			that.record();
			if (old != null)
			{
				old();
			}			
		};	
	}(this)	
}

var GrabzItWebRecorder = new GrabzItWebRecorder();

window.GrabzItSetTimeout = window.setTimeout;
window.setTimeout = function(code, timeout){
	var id = window.GrabzItSetTimeout(code, timeout);
	GrabzItWebRecorder.setTimeoutId(id);
	return id;
};
window.GrabzItSetInterval = window.setInterval;
window.setInterval = function(code, timeout){
	var id = window.GrabzItSetInterval(code, timeout);
	GrabzItWebRecorder.setIntervalId(id);
	return id;
};