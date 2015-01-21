define([
	'ember',
	'text!templates/login.html'
], function (Ember, loginTemplate) {

	Ember.TEMPLATES.loginTemplate = Ember.Handlebars.compile(loginTemplate);

	return Ember.View.extend({
		templateName: 'loginTemplate'
	});
});
