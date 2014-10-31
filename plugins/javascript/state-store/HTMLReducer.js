function HTMLReducer()
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
			
			if (typeof b.attributes != 'undefined' && typeof a.attributes != 'undefined')
			{		
				for(var i = 0;i < a.attributes.length;i++)
				{
					var aAtt = a.attributes[i];
					
					if (aAtt.nodeName == 'grabzit-empty')
					{
						b = that.removeAllTextNodes(b, function(o){excludedNodes.push(o);});
					}
					
					if (aAtt.value == '')
					{
						b.removeAttribute(aAtt.nodeName);
					}
					else
					{
						b.setAttribute(aAtt.nodeName, aAtt.value);
					}
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
								b.removeAttribute(bAtt.nodeName);
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
			else if (a.value == b.value && b.value != '')
			{
				b.setAttribute("value", "");
			}
		}
		else if (a.tagName == 'TEXTAREA')
		{
			if (a.value != b.value)
			{
				b.value = a.value;				
			}
			else if (a.value == b.value && b.value != '')
			{
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

var HTMLReducer = new HTMLReducer();