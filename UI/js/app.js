define([
	'ember',
	'controllers/login',
	'controllers/main/contacts',
	'views/login',
	'views/main/contacts',
	'eotu'
], function (Ember, LoginController, MainContactsController, LoginView, MainContactsView, Eotu) {
	return Ember.Application.extend({
		name: "App",
		LoginController: LoginController,
		LoginView: LoginView,
		MainContactsController: MainContactsController,
		MainContactsView: MainContactsView,
		ApplicationView: Ember.View.extend({
			templateName: 'application'
		}),
		ready: function () {
		}
	});
});