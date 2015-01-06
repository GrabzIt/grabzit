function GrabzIt(key)
{
	return new (function(key)
	{	
		this.key = key;
		
		this.Create = function(url, options)
		{
			var protocol = '//';
			if (window.location.protocol != 'https' && window.location.protocol != 'http')
			{
				protocol = 'http://';
			}

			var grabzItUrl = protocol + 'api.grabz.it/services/javascript.ashx?key='+encodeURIComponent(this.key)+'&url=' + encodeURIComponent(url);

			for(var k in options)
			{
				if (k != 'format' && k != 'cache' && k != 'customWaterMarkId' && k != 'quality'
				&& k != 'country' && k != 'filename' && k != 'errorid' && k != 'errorclass' &&
				k != 'onfinish' && k != 'onerror' && k != 'delay' && k != 'bwidth' && k != 'bheight' &&
				k != 'height' && k != 'width' && k != 'target' && k != 'requestas' && k != 'download' && k != 'suppresserrors' && k != 'displayid' && k != 'displayclass' && k != 'background' && k != 'pagesize' && k != 'orientation' && k != 'includelinks' && k != 'includeoutline' && k != 'title' && k != 'coverurl' && k != 'mtop' && k != 'mleft' && k != 'mbottom' && k != 'mright' && k != 'tabletoinclude' && k != 'includeheadernames' && k != 'includealltables' && k != 'start' && k != 'duration' && k != 'speed' && k != 'fps' && k != 'repeat' && k != 'reverse')
				{
					throw "Option " + k + " not recognized!";
				}
				
				var v = options[k];
				if (v != null)
                		{
					grabzItUrl += '&' + k + '=' + encodeURIComponent(v);
				}
			}

			var scriptNode = document.createElement('script');
			scriptNode.src = grabzItUrl;

			return scriptNode;
		};		

		this.AddTo = function(container, url, options)
		{
			var elem = null;
			if (typeof container == 'string' || container instanceof String)
			{
				elem = document.getElementById(container);
				if (elem == null)
				{
					throw "An element with the id " + container + " was not found";
				}
			}
			else if (container.nodeType === 1)
			{
				elem = container;
			}

			if (elem == null)
			{
				throw "No valid element was provided to attach the screenshot to";
			}

			elem.appendChild(this.Create(url, options));
		}
	})(key);
}