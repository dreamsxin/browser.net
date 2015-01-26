define([
	'ember',
	'text!templates/main/contacts.html',
	'eotu'
], function (Ember, mainContactsTemplate, Eotu) {

	Ember.TEMPLATES.mainContactsTemplate = Ember.Handlebars.compile(mainContactsTemplate);

	return Ember.View.extend({
		contacts: Ember.A([]),
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
			$.ajax({
				url: 'http://www.eotu.com:81/api/local/contact/list/' + Eotu.get('Profile.user_id'),
				success: function (obj) {
					if (obj.status === 'ok') {
						var items = obj.data;
						for (var i = 0; i < items.length; i++) {
							items[i]["isSelected"] = false;
							_this.contacts.pushObject(Ember.Object.extend(items[i]).create());
						}
						Eotu.set('Contacts', _this.contacts);
					} else {
						alert(obj.message);
					}
				},
				dataType: 'json'
			});
		}
	});
});
