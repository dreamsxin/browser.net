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
			Eotu.PlaySound('ui/sound/login.wav', true);
			Eotu.SetWindowSize(760, $(document).outerHeight(true));
			var sockid;
			sockid = Eotu.Connect('192.168.1.108', 81, {
				'connected': function () {
					Eotu.Send("POST /api/local/contact/list/9 HTTP/1.1\r\nHost: www.eotu.com:81\r\nConnection: Close\r\n\r\n");
				},
				'change': function (code, status) {
					//alert(status);
				},
				'receive': function (data) {
					if (data.indexOf("\r\n\r\n") >= 0) {
						data = data.substring(data.indexOf("\r\n\r\n") + 4, data.length);
					}
					if (data.length > 0) {
						var obj = jQuery.parseJSON(data);
						if (obj.status === 'ok') {
							var items = obj.data;
							for(var i=0; i<items.length; i++) {
								_this.contacts.pushObject(items[i]);
							}
						} else {
							alert(obj.message);
						}
					}
				},
				'tcp': true
			});
		}
	});
});
