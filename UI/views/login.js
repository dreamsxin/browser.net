define([
	'ember',
	'text!templates/login.html',
	'eotu'
], function (Ember, loginTemplate, Eotu) {

	Ember.TEMPLATES.loginTemplate = Ember.Handlebars.compile(loginTemplate);

	return Ember.View.extend({
		templateName: 'loginTemplate',
		didInsertElement: function () {
			Eotu.SetWindowSize(300, $(document).outerHeight(true));
			Eotu.PlaySound('start', true);
		}
	});
});
