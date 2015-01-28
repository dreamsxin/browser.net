define([
	'ember',
	'eotu',
	'jquery'
], function (Ember, Eotu, $) {
	return Ember.ObjectController.extend({
		contact: null,
		actions: {
			select: function (contact) {
				Eotu.Onlines.forEach(function (item) {
					item.set("isSelected", false);
				});
				contact.set("isSelected", true);
				this.set('contact', contact);
			},
			send: function (contact) {
				if (!contact) {
					alert("请选择用户");
				}
				Eotu.console.log("给" + contact.fullname + " 发送消息");
				var found = false;
				Eotu.Onlines.forEach(function (item) {
					if (contact.guid === item.guid) {
						found = true;
						Eotu.Messages.pushObject(Ember.Object.extend({from: Eotu.Profile.fullname, to: item.fullname, message: $('#message_body').val()}).create());
						Ember.run.later((function () {
							if ($('#message_list').length > 0) {
								$('#message_list').scrollTop($('#message_list')[0].scrollHeight + 60);
							}
						}), 100);
						Eotu.SendMessage(item.uid, $('#message_body').val());
						$('#message_body').val('');
						return;
					}
				});
				if (!found) {
					Eotu.console.log(contact.fullname + " 不在线");
				}
			}
		}
	});
});
