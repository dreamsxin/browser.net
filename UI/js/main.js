require.config({
	config: {
		text: {
			onXhr: function (xhr, url) {
				xhr.overrideMimeType('text/plain; charset=utf8');
			}
		}
	},
	baseUrl: 'js',
	paths: {
		controllers: '../controllers',
		views: '../views',
		templates: '../templates',
		jquery: 'jquery-2.1.3',
		bootstrap: 'bootstrap.min',
		handlebars: 'handlebars-v2.0.0',
		ember: 'ember.min'
	},
	shim: {
		ember: {
			deps: ['jquery', 'handlebars'],
			exports: 'Ember'
		},
		bootstrap: {
			deps: ["jquery"],
			exports: "$.fn.popover"
		}
	}
});
define(['handlebars'], function (Handlebars) {
	window.Handlebars = Handlebars;

	require([
		'app',
		'eotu'
	], function (App, Eotu) {
		var eotuApp = App.create();
		eotuApp.Router.map(function () {
			this.route("login", {path: "/"});
			this.route("register");
			this.route('about');
			this.resource('main', function () {
				this.route('contacts', {path: '/:contact_name'});
			});
		});
		
		eotuApp.MainContactsRoute = Ember.Route.extend({
			model: function () {
				return [{fullname: 'fullname', phone: 'phone'}, {fullname: 'fullname', phone: 'phone'}];
			},
			setupController: function (controller, model) {
				controller.set('model', model);
			}
		});

		$(document).ready(function () {
			Eotu.SetWindowTitle('登录');
			Eotu.PlaySound('ui/sound/start.wav', true);
			//Eotu.SetWindowStyle(Eotu.WindowStyle.None);
			//Eotu.AjaxGet("http://dev.eotu.com:81/api/front/index/hotelTypes");

			Eotu.Init();
			if (Eotu.Socket.valid) {
				Eotu.console.log('EotuSock 插件加载成功');
			}
		});
	});
});