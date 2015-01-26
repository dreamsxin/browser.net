define([
	'ember',
	'eotu',
	'jquery'
], function (Ember, Eotu, $) {
	return Ember.ObjectController.extend({
		contact: null,
		messages: null,
		actions: {
			select: function (contact) {
				Eotu.Contacts.forEach(function (item) {
					item.set("isSelected", false);
				});
				contact.set("isSelected", true);
				this.set('contact', contact);
				this.set('messages', Eotu.Messages);
				if ($('#message_list').length > 0) {
					$('#message_list').scrollTop($('#message_list').scrollHeight + 60);
				}
			},
			send: function (contact) {
				Eotu.Onlines.forEach(function (item) {
					Eotu.console.log(contact.fullname, item.fullname);
					if (contact.fullname === item.fullname) {
						Eotu.Messages.pushObject(Ember.Object.extend({to: item.fullname, message: $('#message_body').val()}).create());
						Eotu.SendMessage(item.uid, $('#message_body').val());
						$('#message_body').val('');
						return;
					}
				});
			}
		}
	});
});
