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
		handlebars: 'handlebars-v2.0.0',
		ember: 'ember'
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
		'jquery',
		'eotu'
	], function ($, Eotu) {
		$(document).ready(function () {
			Eotu.SetWindowTitle('聊天');
			
			Eotu.SetWindowSize(600, $(document).outerHeight(true));
		});
	});
});