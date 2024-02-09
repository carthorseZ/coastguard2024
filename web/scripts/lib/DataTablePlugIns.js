var currencyDetect = false;
var htmlDetect = false;
var ipDetect = false;
var htmlNumberDetect = false;
var ukDateDetect = false;
var fileSizeDetect = false;

// Curency Detect
/*
jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		if (!currencyDetect) return null;
		var sValidChars = "0123456789.-,";
		var Char;

	
		for (i = 1; i < sData.length; i++) {
			Char = sData.charAt(i);
			if (sValidChars.indexOf(Char) == -1) {
				return null;
			}
		}

	
		if (sData.charAt(0) == '$' || sData.charAt(0) == '£') {
			return 'currency';
		}
		return null;
	}
)
// HTML Detect

jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		if (!htmlDetect) return null;
		return 'html';
	}
);

	// IP Address
	
jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		if (!ipDetect) return null;
		if (/^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$/.test(sData)) {
			return 'ip-address';
		}
		return null;
	}
);
/*
	// Html Number 
	jQuery.fn.dataTableExt.aTypes.unshift(function (sData) {
		if (!htmlNumberDetect) return false;
		sData = typeof sData.replace == 'function' ?
		sData.replace(/<.*?>/g, "") : sData;
		sData = $.trim(sData);

		var sValidFirstChars = "0123456789-";
		var sValidChars = "0123456789.";
		var Char;
		var bDecimal = false;

		// Check for a valid first char (no period and allow negatives) 
		Char = sData.charAt(0);
		if (sValidFirstChars.indexOf(Char) == -1) {
			return null;
		}

		// Check all the other characters are valid 
		for (var i = 1; i < sData.length; i++) {
			Char = sData.charAt(i);
			if (sValidChars.indexOf(Char) == -1) {
				return null;
			}

			// Only allowed one decimal place... 
			if (Char == ".") {
				if (bDecimal) {
					return null;
				}
				bDecimal = true;
			}
		}

		return 'num-html';
	});
	*/
	/*
	// UK Date
	
	jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		// 
		if (!ukDateDetect) return null;
		if (sData.match(/^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/(19|20|21)\d\d$/)) {
			return 'uk_date';
		}
		return null;
	}
);
/*
*/
jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		// 
		//if (!ukDateDetect) return null;
		
		if (top.IsUkDate(sData)) {
			return 'uk_date';
		}
		return null;
	}
);
/*
	// Week Day

	jQuery.fn.dataTableExt.aTypes.unshift(
  function (sData) {
	  var sValidStrings = 'monday,tuesday,wednesday,thursday,friday,saturday,sunday';

	  if (sValidStrings.indexOf(sData.toLowerCase()) >= 0) {
		  return 'weekdays-sort';
	  }

	  return null;
  }
);
  // File Size
  jQuery.fn.dataTableExt.aTypes.unshift(
	function (sData) {
		if (!fileSizeDetect) return null;
		var sValidChars = "0123456789";
		var Char;


		for (i = 0; i < (sData.length - 3); i++) {
			Char = sData.charAt(i);
			if (sValidChars.indexOf(Char) == -1) {
				return null;
			}
		}


		if (sData.substring(sData.length - 2, sData.length) == "KB"
			|| sData.substring(sData.length - 2, sData.length) == "MB"
			|| sData.substring(sData.length - 2, sData.length) == "GB") {
			return 'file-size';
		}
		return null;
	}
);


  /// Start of Sort Code
  ///
  ///

	// Currency
	/*
jQuery.fn.dataTableExt.oSort['currency-asc'] = function (a, b) {
	
	var x = a == "-" ? 0 : a.replace(/,/g, "");
	var y = b == "-" ? 0 : b.replace(/,/g, "");

	
	x = x.substring(1);
	y = y.substring(1);

	// Parse and return 
	x = parseFloat(x);
	y = parseFloat(y);
	return x - y;
};

jQuery.fn.dataTableExt.oSort['currency-desc'] = function (a, b) {
	// Remove any commas (assumes that if present all strings will have a fixed number of d.p) 
	var x = a == "-" ? 0 : a.replace(/,/g, "");
	var y = b == "-" ? 0 : b.replace(/,/g, "");

// Remove the currency sign 
	x = x.substring(1);
	y = y.substring(1);
// Parse and return 
	x = parseFloat(x);
	y = parseFloat(y);
	return y - x;
};
*/

// UK Date Time
/*
function trim(str) {
	str = str.replace(/^\s+/, '');
	for (var i = str.length - 1; i >= 0; i--) {
		if (/\S/.test(str.charAt(i))) {
			str = str.substring(0, i + 1);
			break;
		}
	}
	return str;
}
/*
jQuery.fn.dataTableExt.oSort['date-euro-asc'] = function (a, b) {
	if (trim(a) != '') {
		var frDatea = trim(a).split(' ');
		var frTimea = frDatea[1].split(':');
		var frDatea2 = frDatea[0].split('/');
		var x = (frDatea2[2] + frDatea2[1] + frDatea2[0] + frTimea[0] + frTimea[1] + frTimea[2]) * 1;
	} else {
		var x = 10000000000000; // = l'an 1000 ...
	}

	if (trim(b) != '') {
		var frDateb = trim(b).split(' ');
		var frTimeb = frDateb[1].split(':');
		frDateb = frDateb[0].split('/');
		var y = (frDateb[2] + frDateb[1] + frDateb[0] + frTimeb[0] + frTimeb[1] + frTimeb[2]) * 1;
	} else {
		var y = 10000000000000;
	}
	var z = ((x < y) ? -1 : ((x > y) ? 1 : 0));
	return z;
};

jQuery.fn.dataTableExt.oSort['date-euro-desc'] = function (a, b) {
	if (trim(a) != '') {
		var frDatea = trim(a).split(' ');
		var frTimea = frDatea[1].split(':');
		var frDatea2 = frDatea[0].split('/');
		var x = (frDatea2[2] + frDatea2[1] + frDatea2[0] + frTimea[0] + frTimea[1] + frTimea[2]) * 1;
	} else {
		var x = 10000000000000;
	}

	if (trim(b) != '') {
		var frDateb = trim(b).split(' ');
		var frTimeb = frDateb[1].split(':');
		frDateb = frDateb[0].split('/');
		var y = (frDateb[2] + frDateb[1] + frDateb[0] + frTimeb[0] + frTimeb[1] + frTimeb[2]) * 1;
	} else {
		var y = 10000000000000;
	}
	var z = ((x < y) ? 1 : ((x > y) ? -1 : 0));
	return z;
};

// UK Date
*/
jQuery.fn.dataTableExt.oSort['uk_date-asc'] = function (a, b) {
	var ukDatea = a.split('/');
	var ukDateb = b.split('/');

	var x = (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
	var y = (ukDateb[2] + ukDateb[1] + ukDateb[0]) * 1;

	return ((x < y) ? -1 : ((x > y) ? 1 : 0));
};


jQuery.fn.dataTableExt.oSort['uk_date-desc'] = function (a, b) {
	var ukDatea = a.split('/');
	var ukDateb = b.split('/');

	var x = (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
	var y = (ukDateb[2] + ukDateb[1] + ukDateb[0]) * 1;

	return ((x < y) ? 1 : ((x > y) ? -1 : 0));
};

/*
// FileSize
jQuery.fn.dataTableExt.oSort['file-size-asc'] = function (a, b) {
	var x = a.substring(0, a.length - 2);
	var y = b.substring(0, b.length - 2);

	var x_unit = (a.substring(a.length - 2, a.length) == "MB" ? 1000 : (a.substring(a.length - 2, a.length) == "GB" ? 1000000 : 1));
	var y_unit = (b.substring(b.length - 2, b.length) == "MB" ? 1000 : (b.substring(b.length - 2, b.length) == "GB" ? 1000000 : 1));

	x = parseInt(x * x_unit);
	y = parseInt(y * y_unit);

	return ((x < y) ? -1 : ((x > y) ? 1 : 0));
};

jQuery.fn.dataTableExt.oSort['file-size-desc'] = function (a, b) {
	var x = a.substring(0, a.length - 2);
	var y = b.substring(0, b.length - 2);

	var x_unit = (a.substring(a.length - 2, a.length) == "MB" ? 1000 : (a.substring(a.length - 2, a.length) == "GB" ? 1000000 : 1));
	var y_unit = (b.substring(b.length - 2, b.length) == "MB" ? 1000 : (b.substring(b.length - 2, b.length) == "GB" ? 1000000 : 1));

	x = parseInt(x * x_unit);
	y = parseInt(y * y_unit);

	return ((x < y) ? 1 : ((x > y) ? -1 : 0));
};

// Formated Number
jQuery.fn.dataTableExt.oSort['formatted-num-asc'] = function (x, y) {
	x = x.replace(/[^\d\-\.\/]/g, '');
	y = y.replace(/[^\d\-\.\/]/g, '');
	if (x.indexOf('/') >= 0) x = eval(x);
	if (y.indexOf('/') >= 0) y = eval(y);
	return x / 1 - y / 1;
}
jQuery.fn.dataTableExt.oSort['formatted-num-desc'] = function (x, y) {
	x = x.replace(/[^\d\-\.\/]/g, '');
	y = y.replace(/[^\d\-\.\/]/g, '');
	if (x.indexOf('/') >= 0) x = eval(x);
	if (y.indexOf('/') >= 0) y = eval(y);
	return y / 1 - x / 1;
}
// IP Addresses
jQuery.fn.dataTableExt.oSort['ip-address-asc'] = function (a, b) {
	var m = a.split("."), x = "";
	var n = b.split("."), y = "";
	for (var i = 0; i < m.length; i++) {
		var item = m[i];
		if (item.length == 1) {
			x += "00" + item;
		} else if (item.length == 2) {
			x += "0" + item;
		} else {
			x += item;
		}
	}
	for (var i = 0; i < n.length; i++) {
		var item = n[i];
		if (item.length == 1) {
			y += "00" + item;
		} else if (item.length == 2) {
			y += "0" + item;
		} else {
			y += item;
		}
	}
	return ((x < y) ? -1 : ((x > y) ? 1 : 0));
};

jQuery.fn.dataTableExt.oSort['ip-address-desc'] = function (a, b) {
	var m = a.split("."), x = "";
	var n = b.split("."), y = "";
	for (var i = 0; i < m.length; i++) {
		var item = m[i];
		if (item.length == 1) {
			x += "00" + item;
		} else if (item.length == 2) {
			x += "0" + item;
		} else {
			x += item;
		}
	}
	for (var i = 0; i < n.length; i++) {
		var item = n[i];
		if (item.length == 1) {
			y += "00" + item;
		} else if (item.length == 2) {
			y += "0" + item;
		} else {
			y += item;
		}
	}
	return ((x < y) ? 1 : ((x > y) ? -1 : 0));
};*/