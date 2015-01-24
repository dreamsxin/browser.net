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
				sockid = Eotu.Connect('192.168.1.126', 60005, {
					'connected': function () {
						Eotu.Login('dreamsxin@qq.com', 'aaa111');
					},
					'authSuccess': function (data) {
						alert('登录成功' + data);
						Eotu.set('Profile', Ember.Object.extend(obj.data).create());
						_this.transitionToRoute('/main/contacts');
					},
					'authFail': function () {
						alert('登录失败');
					},
					'change': function (code, status) {
						//alert(status);
					},
					'receive': function (data) {
						alert(data);
					},
					'tcp': false
				});
			}
		}
	});
});
