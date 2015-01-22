define([
	'ember',
	'eotu'
], function (Ember, Eotu) {
	return Ember.ObjectController.extend({
		contacts: Ember.A([
			Ember.Object.create({fullname: 'fullname', phone: 'phone'}),
			Ember.Object.create({fullname: 'fullname', phone: 'phone'})
		]),
		actions: {
		}
	});
});
