<!-->注册页<-->
<template>
    <div style="display: none;">
        我在拼命加载中 ......
    </div>
</template>

<script>
    import $ from 'jquery';
    import viewtitle from './title';

    let regist = {
        data() {
            return {
               /* username: '',
                userpassword: '',
                reg: /^1[0-9]{10}$/,
                erro: '',
                time: 60,
                hassend: false,
                recommand: '',
                showrec: true,*/
            }
        },
        components: {
            viewtitle
        },
        created() {

            this.redirectURL();

            /*//存储及获取推荐人信息
            if (this.$route.params.username.length == 11) {
                sessionStorage.setItem('recommand', this.$route.params.username)
                this.recommand = this.$route.params.username
                this.showrec = false
            } else if (sessionStorage.getItem('recommand')) {
                this.recommand = sessionStorage.getItem("recommand")
                this.showrec = false
            } else {
                this.recommand = ''
                this.showrec = true
            }
            ;*/
        },
        mounted() {
            //获取头高
           // this.$store.commit('topheight')
        },
        methods: {
            // 重定向至首页
            redirectURL() {
                let username = this.$route.params.username,
                    href = "/#/login?reference=" + username;
                window.location.href = href;
            },
            //注册
            /*logincli() {
                const that = this;
                if (!this.reg.test(this.username)) {
                    this.erro = '请输入正确的手机号码'
                } else if (this.userpassword.length == 0) {
                    this.erro = '请输入验证码'
                } else {
                    this.erro = ''
                    $.ajax({
                        url: 'WebApi/UserManager/ResgistWithSMS?UserName=' + this.username + '&SMSCode=' + this.userpassword + '&spreader=' + this.recommand + '&devicecode=&channel=wx',
                        type: 'get',
                        success: function (data) {
                            if (data.status == 0) {
                                that.$store.commit('SET_SHORTTOKEN', data.data)
                                localStorage.setItem('longtoken', data.additional_data)
                                that.$router.push('/my')
                            } else {
                                alert(data.message)
                            }
                        }
                    })
                }
            },
            back() {
                this.$router.push('/my')
            },
            //发送验证码
            getsms() {
                var that = this
                if (this.reg.test(this.username)) {
                    this.sendsms()
                } else {
                    this.erro = "请输入正确的手机号码"
                }
            },
            //发送验证码
            sendsms() {
                const that = this
                $.ajax({
                    url: 'WebApi/AuthManager/SendSMS?mobile=' + this.username,
                    type: "get",
                    success: function (data) {
                        if (data.status == 0) {
                            that.hassend = true
                            that.sh = setInterval(function () {
                                that.time--
                                if (that.time == 0) {
                                    that.hassend = false
                                    that.time = 60
                                    clearInterval(that.sh)
                                }
                            }, 1000)
                        } else {
                            alert('发送失败，请稍后重试')
                        }
                    }
                })
            },
            //手机合法性判定
            // getphone() {
            //     if (this.reg.test(this.username)) {
            //         this.erro = ''
            //     } else {
            //         this.erro = "请输入正确的手机号码"
            //     }
            // },*/
        }
    }
    export default regist
</script>