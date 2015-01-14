var Eotu = {
	AjaxGet: function (url) {
		this.Call('AjaxGet', {url: url});
	},
	ShowMessage: function (title, message) {
		this.Call('ShowMessage', {title: title, message: message});
	},
	SetWindowTitle: function (title) {
		this.Call('SetWindowTitle', {title: title});
	},
	SetWindowSize: function (width, height) {
		this.Call('SetWindowSize', {width: width, height: height});
	},
	Call: function (funName, json) {
		var event = new MessageEvent(funName, {
			'view': window,
			'bubbles': false,
			'cancelable': false,
			'data': JSON.stringify(json)
		});
		document.dispatchEvent(event);
	},
	Success: function (json) {
		alert(json);
	},
	Error: function (json) {
		alert(json);
	}
};

$(document).ready(function () {
	Eotu.SetWindowTitle('登录');
	Eotu.SetWindowSize($(document).outerWidth(true), $(document).outerHeight(true) + 80);
	//Eotu.AjaxGet("http://dev.eotu.com:81/api/front/index/hotelTypes");
});