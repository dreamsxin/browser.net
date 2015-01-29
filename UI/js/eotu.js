define([
	'ember'
], function (Ember) {
	var Eotu = Ember.Object.extend({
		pluginDef: {
			"name": "EotuSocket",
			"mimeType": "application/x-eotusocket",
			"activeXName": "GuoSheng.EotuSocket",
			"guid": "d0da49f0-f6a1-5c4a-8494-6a5f9c79980d",
			"installURL": {
				"win": "EotuSocket.msi"
			}
		},
		pollInterval: 500,
		isPluginInstalled: function () {
			if (window.ActiveXObject) {
				return this._isIEPluginInstalled();
			} else if (navigator.plugins) {
				return this._isNpapiPluginInstalled();
			}
		},
		_isIEPluginInstalled: function () {
			var axname = this.pluginDef.activeXName;
			var plugin = false;
			try {
				plugin = new ActiveXObject(axname);
			} catch (e) {
				return null;
			}

			var version = false;

			if (plugin) {
				try {
					version = plugin.version;
				} catch (e) {
					version = true;
				}
			}
			return version;
		},
		_isNpapiPluginInstalled: function () {
			var mimeType = this.pluginDef.mimeType;
			var name = this.pluginDef.name;

			if (typeof (navigator.plugins[name]) != "undefined") {
				var re = /([0-9.]+)\.dll/;

				var filename = navigator.plugins[name].filename;
				var fnd = re.exec(filename);
				if (fnd === null) {
					return true;
				} else {
					return fnd[1];
				}
			}

			return false;
		},
		Profile: null,
		Contacts: null,
		Onlines: Ember.A([]),
		Messages: Ember.A([]),
		Socket: null,
		SocketId: null,
		console: console || {
			log: function () {
				return false;
			}
		},
		Connect: function (host, port, callback) {
			if (!this.Socket) {
				this.console.log('未初始化 Socket 对象');
				return false;
			}
			this.SocketId = this.Socket.connect(host, port, callback);
			this.console.log("SocketId", this.SocketId);
			return this.SocketId;
		},
		Login: function (username, password) {
			if (!this.Socket) {
				this.console.log('未初始化 Socket 对象');
				return false;
			}

			this.console.log("SocketId", this.SocketId, username, password);
			this.Socket.login(this.SocketId, JSON.stringify({username: username, password: password}));
			return this.SocketId;
		},
		Send: function (data) {
			if (!this.SocketId) {
				this.console.log('未连接到服务器');
				return false;
			}
			return this.Socket.send(this.SocketId, data);
		},
		SendMessage: function (to, message) {
			if (!this.SocketId) {
				this.console.log('未连接到服务器');
				return false;
			}
			this.console.log("SocketId", this.SocketId, to, message);
			return this.Socket.sendMessage(this.SocketId, JSON.stringify({to: to, message: message}));
		},
		addEvent: function (name, func) {
			if (!this.Socket) {
				this.console.log('未初始化 Socket 对象');
				return false;
			}
			if (this.Socket.attachEvent) {
				this.Socket.attachEvent("on" + name, func);
			} else {
				this.Socket.addEventListener(name, func, false);
			}
		},
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
		CreateWindow: function (url, local) {
			this.Call('CreateWindow', {url: url, local: local});
		},
		AjaxGet: function (url) {
			this.Call('AjaxGet', {url: url});
		},
		ShowMessage: function (title, message) {
			this.Call('ShowMessage', {title: title, message: message});
		},
		PlaySound: function (path, local) {
			this.Call('PlaySound', {path: path, local: local});
		},
		SetWindowActivate: function () {
			this.Call('SetWindowActivate', {topmost: false});
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
		Init: function () {
			this.Socket = document.createElement("EMBED");
			this.Socket.setAttribute("type", "application/x-eotusocket");
			this.Socket.setAttribute("style", "width:0px;height:0px;");
			document.body.appendChild(this.Socket);
			return this.Socket;
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
	}).create();
	return Eotu;
});