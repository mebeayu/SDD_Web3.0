<!--绑定账号-->
<template>
	<div class="bindAccount-wrap">
		<div class="back-page-btn" id="carttop">
			<img src="/Assets/Source/img/返回@3x.png" @click="back" />
			<p>{{this.$store.state.viewtitle}}</p>
		</div>
		<div class="bindAccount-content">
			<!--<h3 class="account-tips">为保护您的账号安全,请先绑定手机号码</h3>-->
			<h3 class="account-tips">为账号安全,首次授权需要绑定手机号码</h3>
			<div class="account-numbar">
				<input class="username" v-model="username" @blur="erroUn" placeholder="请输入手机号码" />
				<p class="account-get-num" v-if="this.hassend==false" @click="getVerificalCode">获取验证码</p>
				<p class="account-get-num" v-else>验证码已发送({{ time }}s)</p>
			</div>
			<div style="justify-content: space-between">
				<input class="password" @blur="erroCode" v-model="userpassword" placeholder="请输入手机验证码" />
			</div>

			<div @click="toBindAccount" class="bindBtn">

				<p>绑定并登录</p>
			</div>
			<div class="erromsg" style="background:#fff">
				{{ err }}
			</div>
		</div>
	</div>
</template>
<script>
	import $ from 'jquery';
	const BindAccount = {
		name: 'bindAccount-content',
		data() {
			return {
				username: "",
				userpassword: "",
				hassend: false,
				time: '60',
				reg: /^1[0-9]{10}$/,
				err: ''
			}
		},
		methods: {
			// 去除空格
			stringTrim(val) {
				let result = val,
					is_glob = "g";
				result = result.replace(/(^\s+)|(\s+$)/g, "");
				if(is_glob.toLowerCase() == "g") {
					result = result.replace(/\s/g, "");
				}
				return result;
			},
			//校验手机号码
			erroUn() {
				if(this.username.length !== 0 && this.reg.test(this.username)) {
					this.err = "";
					return true;
				}
				this.err = "手机号码错误";
			},
			//校验手机验证码
			erroCode() {
				if(!this.erroUn()) return;
				if(this.userpassword.length !== 0) {
					this.erro = "";
					return true;
				}
				this.err = "验证码错误";
			},
			//点击获取验证码
			getVerificalCode() {
				if(this.reg.test(this.username) && this.username.length !== 0) {
					this.err = "";
					this.sendVerificalCode();
					return;
				}
				this.err = "手机号码错误";
			},
			sendVerificalCode() {
				let that = this;
				$.ajax({
					url: 'WebApi/AuthManager/SendSMS?mobile=' + this.username,
					type: 'get',
					success: function(res) {
						if(res.status === 0) {
							that.hassend = true;
							that.sh = setInterval(function() {
								that.time--;
								if(that.time === 0) {
									that.hassend = false;
									that.time = 60;
									clearInterval(that.sh);
								}
							}, 1000);
							return;
						}
						if(res.status === 94) {
							alert(res.message);
							return;
						}
						alert('发送失败，请稍后重试');
					}
				})
			},
			// 获取url参数
			getUrlParam(paraName) {
				var url = document.location.toString();
				var arrObj = url.split("?");
				if(arrObj.length > 1) {
					var arrPara = arrObj[1].split("&");
					var arr;
					for(var i = 0; i < arrPara.length; i++) {
						arr = arrPara[i].split("=");
						if(arr != null && arr[0] == paraName) {
							return arr[1];
						}
					}
					return "";
				} else {
					return "";
				}
			},
			//绑定并登录
			toBindAccount() {
				var _allParam = window.UI.Page.getAllparam();
				const that = this,
					fromcodeURL = _allParam.fromcode || ""; // 获取url地址栏的fromcode的值
				var _channel_open_id = _allParam.channel_open_id;
				if(this.username.length === 0 || this.reg.test(this.username) !== true) {
					this.err = '手机号码错误';
				} else if(this.userpassword.length === 0) {
					this.err = '验证码错误';
				} else {
					$.ajax({
						url: '/WebApi/UserManager/ResgistWithSMS?UserName=' + that.stringTrim(that.username) + '&SMSCode=' + that.stringTrim(that.userpassword) + '&fromcode=' + that.stringTrim(fromcodeURL) + '&channel=wx&channel_open_id=' + _channel_open_id,
						type: 'get',
						complete: (xhr) => {
							this.err = "";
							// 失败
							if((xhr.readyState !== 4) && (xhr.status !== 200)) {
								alert("请求服务器异常!");
								return;
							}
							let data = xhr.responseJSON;
							if(data.status !== 0) {
								alert(data.message);
								return;
							}
							that.$store.commit('SET_SHORTTOKEN', data.data);

							localStorage.setItem('longtoken', data.additional_data);
							localStorage.setItem('shorttoken', data.data);
							var allParam = window.UI.Page.getAllparam();

							var redirect_to = "/#/my";

							const para = allParam.to || allParam.redirect;
							if(para) {
								//window.location.href = para.replace("#token", that.$store.state.shorttoken);
								redirect_to = para.replace("#token", that.$store.state.shorttoken);
							} else {
								//that.$router.push('/my')
								redirect_to = "/#/my";
							}
							//采集设备信息

							var ua = navigator.userAgent.toLowerCase();
							//alert(ua);
							var isWeixin = ua.indexOf('micromessenger') != -1;

							if(redirect_to.indexOf("http") == -1) {
								redirect_to = window.location.origin + redirect_to;
							}
							//alert(redirect_to);
							var goto_page = "/Device/LoginInfo?token=" + data.data + "&redirect=" + encodeURIComponent(redirect_to) + "&alias=" + that.stringTrim(that.username);
							window.location.href = goto_page;

							return;

							if(!isWeixin) { //不是微信
								if(ua.indexOf('mobile') > -1 || ua.indexOf('mqqbrowser') > -1 || ua.indexOf('iphone') > -1) {

								} else {
									if(redirect_to == "/#/my") {
										that.$router.push('/my');
									} else {
										window.location.href = redirect_to;
									}
								}
							} else {
								//微信
								if(redirect_to == "/#/my") {
									that.$router.push('/my');
								} else {
									window.location.href = redirect_to;
								}
							}

						}
					})
				}
			},
			back() {
				location.href="/#/my";
				/*var allParam = window.UI.Page.getAllparam();
				this.$router.push({
					path: '/login',
					query: allParam
				});
				try {
					this.$router.go(-1);
					window.history.go(-1);
				} catch(e) {
					this.$router.push("/my");
				}*/

			},
			beforeDestroy() {
				//清除验证码计时器
				clearInterval(this.sh);
			}
		}
	}

	export default BindAccount;
</script>
<style scoped>
	.bindAccount-content {
		padding: .453333rem .8rem .453333rem;
		background: #fff;
		margin-top: 1.46666667rem;
	}
	
	.account-tips {
		font-size: .45rem;
		line-height: 2rem;
		color: #2d2d34;
		font-family: '黑体', sans-serif;
	}
	
	.password,
	.username {
		width: 100%;
		height: 1.066667rem;
		line-height: 1.066667rem;
		margin-top: .266667rem;
		border: none;
		background: #fff;
		border-bottom: .013333rem solid #cccbcc;
		font-size: .373333rem;
		color: #b1b1b9
	}
	
	.account-numbar {
		position: relative
	}
	
	.account-get-num {
		position: absolute;
		top: 60%;
		transform: translateY(-50%);
		right: 0;
		padding: .226667rem .186667rem;
		text-align: center;
		color: #fff;
		font-size: .266667rem;
		border-radius: .533333rem;
		background: #65c4aa
	}
	
	.bindBtn {
		margin: .8rem 0 .533333rem;
	}
	
	.bindBtn p {
		width: 100%;
		height: 1.066667rem;
		line-height: 1.066667rem;
		text-align: center;
		background: #65c4aa;
		color: #fff;
		font-size: .426667rem;
		border-radius: .666667rem
	}
	
	.erromsg {
		display: block;
		color: #ba3027;
		line-height: 1rem;
	}
</style>