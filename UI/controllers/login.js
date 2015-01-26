define([
	'ember',
	'eotu'
], function (Ember, Eotu) {
	return Ember.ObjectController.extend({
		form: {
			username: null,
			password: null
		},
		actions: {
			login: function () {
				if (!this.get('form.username') || !this.get('form.password')) {
					alert('请输入账户和密码');
					return;
				}
				var _this = this;
				var json = JSON.stringify(this.get('form'));
				var sockid;
				Eotu.addEvent('Debug', function(data){alert(data);});
				sockid = Eotu.Connect('192.168.1.126', 60005, {
					connected: function () {
						Eotu.console.log('connected');
						Eotu.Login(_this.get('form.username'), _this.get('form.password'));
					},
					authSuccess: function (data) {
						Eotu.console.log('authSuccess');
						Eotu.set('Profile', Ember.Object.extend(JSON.parse(data).data).create());
						Eotu.console.log(data);
						_this.transitionToRoute('/main/contacts');
					},
					authFail: function () {
						Eotu.console.log('authFail');
					},
					textMessage: function (data) {
						var item = JSON.parse(data);
						Eotu.Messages.pushObject(Ember.Object.extend(item).create());
					},
					message: function (type, data) {
						alert('消息'+type+data);
					},
					onlineNotify: function (data) {
						var items = JSON.parse(data);
						Eotu.console.log(items);
						for(var i=0; i<items.length; i++) {
							Eotu.Onlines.pushObject(Ember.Object.extend(items[i]).create());
						}
					},
					notify: function (type, data) {
						alert('提醒'+type+data);
					},
					change: function (code, status) {
						Eotu.console.log(status);
					},
					receive: function (data) {
						Eotu.console.log(data);
					},
					tcp: false
				});
			}
		}
	});
});
