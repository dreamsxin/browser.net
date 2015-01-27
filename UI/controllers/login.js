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
				Eotu.addEvent('Debug', function (data) {
					alert(data);
				});
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
						var message = JSON.parse(data);
						Eotu.console.log(message);
						Eotu.console.log(Eotu.Onlines);
						var found = false;
						Eotu.Onlines.forEach(function (item) {
							if (message.from === item.uid) {
								found = true;
								message.from = item.fullname;
								Eotu.Messages.pushObject(Ember.Object.extend(message).create());
							}
						});
						if (!found) {
							Eotu.Messages.pushObject(Ember.Object.extend(message).create());
						}
						Ember.run.later((function () {
							if ($('#message_list').length > 0) {
								$('#message_list').scrollTop($('#message_list')[0].scrollHeight + 60);
							}
						}), 100);
					},
					message: function (type, data) {
						alert('消息' + type + data);
					},
					onlineNotify: function (data) {
						var items = JSON.parse(data);
						if (items) {
							Eotu.console.log("online", items);
							for (var i = 0; i < items.length; i++) {
								Eotu.Onlines.forEach(function (item) {
									if (item.guid === items[i].guid) {
										Eotu.Onlines.removeObject(item);
								Eotu.console.log("删除");
									}
								});
								items[i]["online"] = items[i].presence > 0 ? true : false;
								Eotu.Onlines.pushObject(Ember.Object.extend(items[i]).create());
								
							}
							Eotu.console.log(Eotu.Onlines);
						}
					},
					notify: function (type, data) {
						alert('提醒' + type + data);
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
