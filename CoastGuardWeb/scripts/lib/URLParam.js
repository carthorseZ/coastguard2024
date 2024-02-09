var serviceUrl;
var proxy;
		
jQuery.extend({
/**
* Returns get parameters.
*
			* If the desired param does not exist, null will be returned
			*
			* @example value = $.getURLParam("paramName");
			
ToDo: Fix handling of multiple ?
			*/
			getURLParam: function(strParamName) {
				var strReturn = "";
				var strHref = window.location.href;
				var bFound = false;

				var cmpstring = strParamName + "=";
				var cmplen = cmpstring.length;

				if (strHref.indexOf("?") > -1) {
					var strQueryString = strHref.substr(strHref.indexOf("?") + 1);
					var aQueryString = strQueryString.split("&");
					for (var iParam = 0; iParam < aQueryString.length; iParam++) {
						if (aQueryString[iParam].substr(0, cmplen) == cmpstring) {
							var aParam = aQueryString[iParam].split("=");
							strReturn = aParam[1];
							bFound = true;
							break;
						}

					}
				}
				if (bFound == false) return null;
				return strReturn;
			}
		});