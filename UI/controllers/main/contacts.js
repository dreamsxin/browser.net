define([
	'ember',
	'eotu'
], function (Ember, Eotu) {
	return Ember.ObjectController.extend({
		contact: null,
		messages: Ember.A([]),
		actions: {
			select: function (contact) {
				Eotu.Contacts.forEach(function (item) {
					item.set("isSelected", false);
				});
				contact.set("isSelected", true);
				this.set('contact', contact);
				this.messages.pushObject(Ember.Object.extend({from:'郑依丽', message:'测试', date:(new Date()).toLocaleString()}).create());
			}
		}
	});
});
