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
						Eotu.Login('dreamsxin@qq.com', 'aaa111');
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
						alert('文本消息' + data);
					},
					message: function (type, data) {
						alert('消息'+type+data);
					},
					onlineNotify: function (data) {
						alert('在线提醒' + data);
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
