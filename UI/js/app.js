define([
	'ember',
	'controllers/login',
	'views/login'
], function (Ember, LoginController, LoginView) {
	return Ember.Application.extend({
		name: "App",
		LoginController: LoginController,
		LoginView: LoginView,
		MainContactsView: Ember.View.extend({
			didInsertElement: function () {
				Eotu.PlaySound('ui/sound/login.wav', true);
				Eotu.SetWindowSize(760, $(document).outerHeight(true));
			}
		}),
		ApplicationView: Ember.View.extend({
			templateName: 'application'
		}),
		ready: function () {
		}
	});
});