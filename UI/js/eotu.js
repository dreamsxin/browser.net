var Eotu = {
	WindowStyle: {
		None: 'None',
		SingleBorderWindow: 'SingleBorderWindow',
		ThreeDBorderWindow: 'ThreeDBorderWindow',
		ToolWindow: 'ToolWindow'
	},
	ResizeMode: {
		CanMinimize: 'CanMinimize',
		CanResize: 'CanResize',
		CanResizeWithGrip: 'CanResizeWithGrip'
	},	
	AjaxGet: function (url) {
		this.Call('AjaxGet', {url: url});
	},
	ShowMessage: function (title, message) {
		this.Call('ShowMessage', {title: title, message: message});
	},
	SetWindowTitle: function (title) {
		this.Call('SetWindowTitle', {title: title});
	},
	SetWindowStyle: function (style) {
		this.Call('SetWindowStyle', {style: style});
	},
	SetResizeMode: function (mode) {
		this.Call('SetResizeMode', {mode: mode});
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
	Eotu.SetWindowSize(300, $(document).outerHeight(true));
	//Eotu.SetWindowStyle(Eotu.WindowStyle.None);
	//Eotu.AjaxGet("http://dev.eotu.com:81/api/front/index/hotelTypes");
	if(document.getElementById('eotusocket').valid){
		alert(document.getElementById('eotusocket').echo("EotuSock 插件加载成功"));
	}
});