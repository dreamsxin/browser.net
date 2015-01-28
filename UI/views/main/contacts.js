define([
	'ember',
	'text!templates/main/contacts.html',
	'eotu'
], function (Ember, mainContactsTemplate, Eotu) {

	Ember.TEMPLATES.mainContactsTemplate = Ember.Handlebars.compile(mainContactsTemplate);

	return Ember.View.extend({
		contacts: null,
		messages: null,
		templateName: 'mainContactsTemplate',
		didInsertElement: function () {
			_this = this;
			if (!Eotu.get('Profile.user_id')) {
				Eotu.console.log('Eotu.Profile.user_id not found');
				_this.get('controller').transitionToRoute('login');
				return;
			}
			Eotu.PlaySound('ui/sound/login.wav', true);
			Eotu.SetWindowSize(1140, $(document).outerHeight(true));
			_this.set('contacts',Eotu.Onlines);
			_this.set('messages', Eotu.Messages);
		}
	});
});
