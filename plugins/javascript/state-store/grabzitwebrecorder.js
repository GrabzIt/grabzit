var GrabzItLZString={_keyStr:"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",_f:String.fromCharCode,compressToBase64:function(e){if(e==null)return"";var t="";var n,r,i,s,o,u,a;var f=0;e=GrabzItLZString.compress(e);while(f<e.length*2){if(f%2==0){n=e.charCodeAt(f/2)>>8;r=e.charCodeAt(f/2)&255;if(f/2+1<e.length)i=e.charCodeAt(f/2+1)>>8;else i=NaN}else{n=e.charCodeAt((f-1)/2)&255;if((f+1)/2<e.length){r=e.charCodeAt((f+1)/2)>>8;i=e.charCodeAt((f+1)/2)&255}else r=i=NaN}f+=3;s=n>>2;o=(n&3)<<4|r>>4;u=(r&15)<<2|i>>6;a=i&63;if(isNaN(r)){u=a=64}else if(isNaN(i)){a=64}t=t+GrabzItLZString._keyStr.charAt(s)+GrabzItLZString._keyStr.charAt(o)+GrabzItLZString._keyStr.charAt(u)+GrabzItLZString._keyStr.charAt(a)}return t},decompressFromBase64:function(e){if(e==null)return"";var t="",n=0,r,i,s,o,u,a,f,l,c=0,h=GrabzItLZString._f;e=e.replace(/[^A-Za-z0-9\+\/\=]/g,"");while(c<e.length){u=GrabzItLZString._keyStr.indexOf(e.charAt(c++));a=GrabzItLZString._keyStr.indexOf(e.charAt(c++));f=GrabzItLZString._keyStr.indexOf(e.charAt(c++));l=GrabzItLZString._keyStr.indexOf(e.charAt(c++));i=u<<2|a>>4;s=(a&15)<<4|f>>2;o=(f&3)<<6|l;if(n%2==0){r=i<<8;if(f!=64){t+=h(r|s)}if(l!=64){r=o<<8}}else{t=t+h(r|i);if(f!=64){r=s<<8}if(l!=64){t+=h(r|o)}}n+=3}return GrabzItLZString.decompress(t)},compressToUTF16:function(e){if(e==null)return"";var t="",n,r,i,s=0,o=GrabzItLZString._f;e=GrabzItLZString.compress(e);for(n=0;n<e.length;n++){r=e.charCodeAt(n);switch(s++){case 0:t+=o((r>>1)+32);i=(r&1)<<14;break;case 1:t+=o(i+(r>>2)+32);i=(r&3)<<13;break;case 2:t+=o(i+(r>>3)+32);i=(r&7)<<12;break;case 3:t+=o(i+(r>>4)+32);i=(r&15)<<11;break;case 4:t+=o(i+(r>>5)+32);i=(r&31)<<10;break;case 5:t+=o(i+(r>>6)+32);i=(r&63)<<9;break;case 6:t+=o(i+(r>>7)+32);i=(r&127)<<8;break;case 7:t+=o(i+(r>>8)+32);i=(r&255)<<7;break;case 8:t+=o(i+(r>>9)+32);i=(r&511)<<6;break;case 9:t+=o(i+(r>>10)+32);i=(r&1023)<<5;break;case 10:t+=o(i+(r>>11)+32);i=(r&2047)<<4;break;case 11:t+=o(i+(r>>12)+32);i=(r&4095)<<3;break;case 12:t+=o(i+(r>>13)+32);i=(r&8191)<<2;break;case 13:t+=o(i+(r>>14)+32);i=(r&16383)<<1;break;case 14:t+=o(i+(r>>15)+32,(r&32767)+32);s=0;break}}return t+o(i+32)},decompressFromUTF16:function(e){if(e==null)return"";var t="",n,r,i=0,s=0,o=GrabzItLZString._f;while(s<e.length){r=e.charCodeAt(s)-32;switch(i++){case 0:n=r<<1;break;case 1:t+=o(n|r>>14);n=(r&16383)<<2;break;case 2:t+=o(n|r>>13);n=(r&8191)<<3;break;case 3:t+=o(n|r>>12);n=(r&4095)<<4;break;case 4:t+=o(n|r>>11);n=(r&2047)<<5;break;case 5:t+=o(n|r>>10);n=(r&1023)<<6;break;case 6:t+=o(n|r>>9);n=(r&511)<<7;break;case 7:t+=o(n|r>>8);n=(r&255)<<8;break;case 8:t+=o(n|r>>7);n=(r&127)<<9;break;case 9:t+=o(n|r>>6);n=(r&63)<<10;break;case 10:t+=o(n|r>>5);n=(r&31)<<11;break;case 11:t+=o(n|r>>4);n=(r&15)<<12;break;case 12:t+=o(n|r>>3);n=(r&7)<<13;break;case 13:t+=o(n|r>>2);n=(r&3)<<14;break;case 14:t+=o(n|r>>1);n=(r&1)<<15;break;case 15:t+=o(n|r);i=0;break}s++}return GrabzItLZString.decompress(t)},compress:function(e){if(e==null)return"";var t,n,r={},i={},s="",o="",u="",a=2,f=3,l=2,c="",h=0,p=0,d,v=GrabzItLZString._f;for(d=0;d<e.length;d+=1){s=e.charAt(d);if(!Object.prototype.hasOwnProperty.call(r,s)){r[s]=f++;i[s]=true}o=u+s;if(Object.prototype.hasOwnProperty.call(r,o)){u=o}else{if(Object.prototype.hasOwnProperty.call(i,u)){if(u.charCodeAt(0)<256){for(t=0;t<l;t++){h=h<<1;if(p==15){p=0;c+=v(h);h=0}else{p++}}n=u.charCodeAt(0);for(t=0;t<8;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}else{n=1;for(t=0;t<l;t++){h=h<<1|n;if(p==15){p=0;c+=v(h);h=0}else{p++}n=0}n=u.charCodeAt(0);for(t=0;t<16;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}delete i[u]}else{n=r[u];for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}r[o]=f++;u=String(s)}}if(u!==""){if(Object.prototype.hasOwnProperty.call(i,u)){if(u.charCodeAt(0)<256){for(t=0;t<l;t++){h=h<<1;if(p==15){p=0;c+=v(h);h=0}else{p++}}n=u.charCodeAt(0);for(t=0;t<8;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}else{n=1;for(t=0;t<l;t++){h=h<<1|n;if(p==15){p=0;c+=v(h);h=0}else{p++}n=0}n=u.charCodeAt(0);for(t=0;t<16;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}delete i[u]}else{n=r[u];for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}}a--;if(a==0){a=Math.pow(2,l);l++}}n=2;for(t=0;t<l;t++){h=h<<1|n&1;if(p==15){p=0;c+=v(h);h=0}else{p++}n=n>>1}while(true){h=h<<1;if(p==15){c+=v(h);break}else p++}return c},decompress:function(e){if(e==null)return"";if(e=="")return null;var t=[],n,r=4,i=4,s=3,o="",u="",a,f,l,c,h,p,d,v=GrabzItLZString._f,m={string:e,val:e.charCodeAt(0),position:32768,index:1};for(a=0;a<3;a+=1){t[a]=a}l=0;h=Math.pow(2,2);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}switch(n=l){case 0:l=0;h=Math.pow(2,8);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}d=v(l);break;case 1:l=0;h=Math.pow(2,16);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}d=v(l);break;case 2:return""}t[3]=d;f=u=d;while(true){if(m.index>m.string.length){return""}l=0;h=Math.pow(2,s);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}switch(d=l){case 0:l=0;h=Math.pow(2,8);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}t[i++]=v(l);d=i-1;r--;break;case 1:l=0;h=Math.pow(2,16);p=1;while(p!=h){c=m.val&m.position;m.position>>=1;if(m.position==0){m.position=32768;m.val=m.string.charCodeAt(m.index++)}l|=(c>0?1:0)*p;p<<=1}t[i++]=v(l);d=i-1;r--;break;case 2:return u}if(r==0){r=Math.pow(2,s);s++}if(t[d]){o=t[d]}else{if(d===i){o=f+f.charAt(0)}else{return null}}u+=o;t[i++]=f+o.charAt(0);r--;f=o;if(r==0){r=Math.pow(2,s);s++}}}};

function GrabzIt(key)
{
	return new (function(key)
	{
		this.key = key;
		this.data = null;
		this.dataKey = null;
		this.options = null;
		this.post = false;
		this.elem = null;
		this.protocol = null;

		this.ConvertURL = function(url, options)
		{
			this.data = url;
			this.dataKey = 'url';
			this.options = this._cleanOptions(options);
			this.post = false;

			return this;
		};

		this.ConvertHTML = function(html, options)
		{
			this.data = encodeURIComponent(html);
			this.dataKey = 'html';
			this.options = this._cleanOptions(options);
			this.post = true;

			return this;
		};
		
		this.UseSSL = function()
		{
			this.protocol = 'https://';
			
			return this;
		};
		
		this._cleanOptions = function(opts)
		{
			if (opts == null)
			{
				return {};
			}
			
			var results = {};
			
			for(var k in opts)
			{
				if (k == null)
				{
					continue;
				}
				
				results[k.toLowerCase()] = opts[k];
			}
			
			return results;
		}
		
		this._createXHTTP = function()
		{
			if (window.XMLHttpRequest)
			{
				return new XMLHttpRequest();
			}
			return new ActiveXObject("Microsoft.XMLHTTP");
		};

		this._post = function(qs)
		{
			var xhttp = this._createXHTTP();

			var that = this;

			xhttp.onreadystatechange = function()
			{
				if (this.readyState == 4 && this.status == 200)
				{
					that.elem.appendChild(that._handlePost(JSON.parse(this.responseText)));
				}
			};

			xhttp.open("POST", this._getBaseWebServiceUrl(), true);
			xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			xhttp.send(qs);
		};
		
		this._getRootURL = function()
		{
			if (this.protocol == null)
			{
				this.protocol = '//';
				if (window.location.protocol != 'https:' && window.location.protocol != 'http:')
				{
					this.protocol = 'http://';
				}
			}

			return this.protocol + 'api.grabz.it/services/';
		};

		this._getBaseWebServiceUrl = function()
		{
			return this._getRootURL() + 'javascript.ashx';
		};

		this._createQueryString = function(sKey, sValue)
		{
			var qs = 'key='+encodeURIComponent(this.key)+'&'+sKey+'=' + encodeURIComponent(sValue);

			for(var k in this.options)
			{
				if (k != 'format' && k != 'cache' && k != 'customwatermarkid' && k != 'quality'
				&& k != 'country' && k != 'filename' && k != 'errorid' && k != 'errorclass' &&
				k != 'onfinish' && k != 'onerror' && k != 'delay' && k != 'bwidth' && k != 'bheight' &&
				k != 'height' && k != 'width' && k != 'target' && k != 'requestas' && k != 'download' && k != 'suppresserrors' && k != 'displayid' && k != 'displayclass' && k != 'background' && k != 'pagesize' && k != 'orientation' && k != 'includelinks' && k != 'includeoutline' && k != 'title' && k != 'coverurl' && k != 'mtop' && k != 'mleft' && k != 'mbottom' && k != 'mright' && k != 'tabletoinclude' && k != 'includeheadernames' && k != 'includealltables' && k != 'start' && k != 'duration' && k != 'speed' && k != 'fps' && k != 'repeat' && k != 'reverse' &&
				k != 'templateid' && k != 'noresult' && k != 'hide' && k != 'includeimages' && k != 'export' && k != 'waitfor')
				{
					throw "Option " + k + " not recognized!";
				}

				var v = this.options[k];
				if (v != null)
                {
					qs += '&' + k + '=' + encodeURIComponent(v);
				}
			}

			return qs;
		};

		this._createScriptNode = function(sUrl)
		{
			var scriptNode = document.createElement('script');
			scriptNode.src = sUrl;

			return scriptNode;
		};

		this._handlePost = function(obj)
		{
			if (obj != null)
			{
				if (obj.ID == null || obj.ID == '')
				{
					throw obj.Message;
				}
				return this._createScriptNode(this._getBaseWebServiceUrl() + '?' + this._createQueryString('id', obj.ID));
			}
		};
		
		this.DataURI = function(callback)
		{
			var onFinishName = null;
			if (this.options['onfinish'] != null)
			{
				onFinishName = this.options['onfinish'];
			}
			
			var functionName = 'grabzItCallback' + Math.floor(Math.random() * (1000000000+1));
			
			this.options['onfinish'] = functionName;
			this.options['noresult'] = 1;
			
			var that = this;			
			window[functionName] = function (id) 
			{					
				var xhttp = that._createXHTTP();

				xhttp.onreadystatechange = function()
				{
					if (this.readyState == 4 && this.status == 200)
					{
					    var reader = new FileReader();
					    reader.onload = function(event) 
					    {
					    	if (callback != null)
					    	{
					    		callback(event.target.result);
					    	}
					    	if (onFinishName != null)
					    	{
					    		var finishFunc = new Function(onFinishName + "('" + id + "')");
					    		finishFunc();
					    	}
					    }
					    reader.readAsDataURL(this.response);
					}
				};

				xhttp.open("GET", that._getRootURL() + 'getjspicture.ashx?id='+id, true);
				xhttp.responseType = "blob";
				xhttp.send();			
			}
			
			this.Create();
		};

		this.Create = function()
		{
			var defaultNode = document.documentElement;
			if (document.body != null)
			{
				defaultNode = document.body;
			}
			this.AddTo(defaultNode);
		};

		this.AddTo = function(container)
		{
			if (typeof container == 'string' || container instanceof String)
			{
				this.elem = document.getElementById(container);
				if (this.elem == null)
				{
					throw "An element with the id " + container + " was not found";
				}
			}
			else if (container.nodeType === 1)
			{
				this.elem = container;
			}

			if (this.elem == null)
			{
				throw "No valid element was provided to attach the capture to";
			}
			
			if (this.options['download'] != '1')
			{
				delete this.options['download'];
			}

			if (this.post)
			{
				this._post(this._createQueryString(this.dataKey, this.data));
				return;
			}

			this.elem.appendChild(this._createScriptNode(this._getBaseWebServiceUrl() + '?' + this._createQueryString(this.dataKey, this.data)));
		};
	})(key);
}

function GrabzItWebRecorder()
{
	this.timeoutIds = new Array();
	this.intervalIds = new Array();
	this.targetId = "";

	this.record = function(targetId)
	{
		this.targetId = targetId;
	};
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
	this.load = function()
	{
		if (this.targetId == null || this.targetId == '')
		{
			alert('Please use the record() method to specify a target element to record.');
		}

		var data = this.getQueryStringValue();
		if (data == '')
		{
			return;
		}

		var decompressed  = GrabzItLZString.decompressFromBase64(data);
		var target = document.getElementById(this.targetId);

		if (target == null)
		{
			//if its larger than this it could cause issues
			alert('The target element can not be found.');

			return '';
		}

		target.innerHTML = decompressed;

		this.freeze();
	};
	this.AppendURL = function(url)
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
	this.AddTo = function(key, container, options)
	{
		GrabzIt(key).ConvertURL(this.AppendURL(), options).AddTo(container);
	};
	this.Create = function(key, options)
	{
		return GrabzIt(key).Create(this.AppendURL(), options);
	}
	this.getQueryStringValue = function() {
	  return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape("grabzit").replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
	};
	this.isRecording = function() {
		  return this.getQueryStringValue() != '';
	};
	this.getState = function()
	{
		var target = document.getElementById(this.targetId);

		if (target == null)
		{
			//if its larger than this it could cause issues
			alert('The target element can not be found.');

			return '';
		}

		this.formatNode(target);

		var cloned = target.cloneNode(true);
		this.minifyNode(cloned);
		var html = cloned.innerHTML.replace(/\>(\r\n|\n|\r)/gm,">");
		var compressed = GrabzItLZString.compressToBase64(html);

		if (compressed.length > 2048)
		{
			//if its larger than this it could cause issues
			alert('The size of the web data you are trying to send to GrabzIt is too large. Try to reduce this by specifiying a id of a element that more tightly wraps the data you are interested in recording.');

			return '';
		}
		return compressed;
	};
	this.minifyNode = function(node)
	{
		if (node === undefined)
		{
			return;
		}
		if (node.tagName && node.tagName.toLowerCase() == "a")
		{
			var href = node.getAttribute("href");
			if (href != null && href != '')
			{
				node.setAttribute("href", "#");
			}
		}
		if (node.tagName && node.tagName.toLowerCase() == "img")
		{
			var src = node.getAttribute("src");
			if (src != null)
			{
				node.setAttribute("src", src.replace(/^(https?):\/\//, '//'));
			}
		}
		if (node.nodeType == 1)
		{
			var val = node.getAttribute("value");
			if (val == null || val == '' || (node.tagName != null && node.tagName.toLowerCase() == "input" && (node.getAttribute("type") != null && (node.getAttribute("type").toLowerCase() == "checkbox" || node.getAttribute("type").toLowerCase() == "radio"))))
			{
				node.removeAttribute("value");
			}

			if(node.tagName != null && node.tagName.toLowerCase() == "select")
			{
				for(var i = 0;i < node.childNodes.length;i++)
				{
					if(node.childNodes[i].tagName && node.childNodes[i].getAttribute("selected") == null)
					{
						node.removeChild(node.childNodes[i]);
					}
					if(node.childNodes[i].tagName && node.childNodes[i].tagName.toLowerCase() == "option")
					{
						node.childNodes[i].removeAttribute("selected");
					}
				}
			}

			node.removeAttribute("alt");
			node.removeAttribute("title");
			node.removeAttribute("onclick");
			node.removeAttribute("onchange");
			node.removeAttribute("onload");
			node.removeAttribute("onafterprint");
			node.removeAttribute("onbeforeprint");
			node.removeAttribute("onbeforeunload");
			node.removeAttribute("onerror");
			node.removeAttribute("onhashchange");
			node.removeAttribute("onmessage");
			node.removeAttribute("onoffline");
			node.removeAttribute("online");
			node.removeAttribute("onpagehide");
			node.removeAttribute("onpageshow");
			node.removeAttribute("onpopstate");
			node.removeAttribute("onresize");
			node.removeAttribute("onstorage");
			node.removeAttribute("onunload");
			node.removeAttribute("onblur");
			node.removeAttribute("oncontextmenu");
			node.removeAttribute("onfocus");
			node.removeAttribute("oninput");
			node.removeAttribute("oninvalid");
			node.removeAttribute("onreset");
			node.removeAttribute("onsearch");
			node.removeAttribute("onselect");
			node.removeAttribute("onkeydown");
			node.removeAttribute("onkeypress");
			node.removeAttribute("onkeyup");
			node.removeAttribute("ondblclick");
			node.removeAttribute("ondrag");
			node.removeAttribute("ondragend");
			node.removeAttribute("ondragenter");
			node.removeAttribute("ondragstart");
			node.removeAttribute("ondrop");
			node.removeAttribute("onmousedown");
			node.removeAttribute("onmousemove");
			node.removeAttribute("onmouseout");
			node.removeAttribute("onmouseover");
			node.removeAttribute("onmouseup");
			node.removeAttribute("onmousewheel");
			node.removeAttribute("onscroll");
			node.removeAttribute("onwheel");
		}

		for(var i = 0;i < node.childNodes.length;i++)
		{
			this.minifyNode(node.childNodes[i]);
		}
	}
	this.formatNode = function(node)
	{
		if (node === undefined)
		{
			return;
		}
		if (node.value != null)
		{
			if(node.tagName && node.tagName.toLowerCase() == "textarea")
			{
				node.innerHTML = node.value;
			}
			else if(node.tagName && node.tagName.toLowerCase() == "select")
			{
				for(var i = 0;i < node.childNodes.length;i++)
				{
					if(node.childNodes[i].tagName && node.childNodes[i].tagName.toLowerCase() == "option")
					{
						node.childNodes[i].removeAttribute("selected");
						if (node.childNodes[i].value == node.value)
						{
							node.childNodes[i].setAttribute("selected", "selected");
						}
					}
				}
			}
			else if (node.value != null && node.value != '')
			{
				node.setAttribute("value", node.value);
			}
		}
		if (node.checked)
		{
			node.setAttribute("checked", "checked");
		}
		else if (node.tagName && node.tagName.toLowerCase() == "input")
		{
			node.removeAttribute("checked");
		}
		for(var i = 0;i < node.childNodes.length;i++)
		{
			this.formatNode(node.childNodes[i]);
		}
	}
	var __construct = function(that)
	{
		var old = window.onload;
		window.onload = function()
		{
			that.load();
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