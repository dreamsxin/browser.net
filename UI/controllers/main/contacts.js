define([
	'ember',
	'eotu'
], function (Ember, Eotu) {
	return Ember.ObjectController.extend({
		actions: {
			select: function (contact) {
				Eotu.Contacts.forEach(function (item) {
					item.set("isSelected", false);
				});
				contact.set("isSelected", true);
			}
		}
	});
});
