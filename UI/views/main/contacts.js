define([
	'ember',
	'text!templates/main/contacts.html',
	'eotu'
], function (Ember, mainContactsTemplate, Eotu) {

	Ember.TEMPLATES.mainContactsTemplate = Ember.Handlebars.compile(mainContactsTemplate);

	return Ember.View.extend({
		contacts: Ember.A([
			Ember.Object.create({fullname: 'fullname', phone: 'phone'}),
			Ember.Object.create({fullname: 'fullname', phone: 'phone'})
		]),
		templateName: 'mainContactsTemplate',
		didInsertElement: function () {
			Eotu.PlaySound('ui/sound/login.wav', true);
			Eotu.SetWindowSize(760, $(document).outerHeight(true));
		}
	});
});
