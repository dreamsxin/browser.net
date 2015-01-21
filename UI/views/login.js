define([
	'ember',
	'text!templates/login.html',
	'eotu'
], function (Ember, loginTemplate, Eotu) {

	Ember.TEMPLATES.loginTemplate = Ember.Handlebars.compile(loginTemplate);

	return Ember.View.extend({
		templateName: 'loginTemplate',
		didInsertElement: function () {
			Eotu.PlaySound('ui/sound/login.wav', true);
			Eotu.SetWindowSize(300, $(document).outerHeight(true));
		}
	});
});
