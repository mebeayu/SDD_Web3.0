<!--登录页-->
<template>
    <div class="login-wrap-box" v-if="login_wrap_box">
        <div class="login-wrap">
            <div class="rrl-close-btn">
                <img @click="back" src="/Assets/Source/关闭@3x.png"/>
            </div>
            <div class="rrl-login-big">
               
                <div class="rrl-loginbox">
                    <div class="loginbox_padding">
                        <div class="logo">
                        省兜兜
                        </div>

                        <div class="rrl-get-numbar">
                            <img src="/Assets/Source/login_07.png" alt="手机号">
                            <input class="username" v-model="username" @blur="erroUn" placeholder="请输入手机号码"/>
                            <p class="rrl-get-num" @click="getsms" v-if="this.hassend==false">获取验证码</p>
                            <p class="rrl-get-num" v-else>验证码已发送({{ time }}s)</p>
                        </div>
                        <div class="rrl_get_yzm">
                             <img src="/Assets/Source/login_17.png" alt="手机验证码">
                            <input class="password" @blur="erroCode" v-model="userpassword" placeholder="请输入手机验证码"/>
                        </div>
                        <div class="rrl-dync-codebar">
                             <img src="/Assets/Source/login_27.png" alt="动态验证码">
                            <input v-model="dynccode" @blur="erroDsCode" class="rrl-dync-code" placeholder="请输入动态验证码"/>
                            <p @click="getMathRandomCode" class="rrl-dync-code-til">
                                <img :src="codeImg"/>
                            </p>
                        </div>
                        <div @click="tologin" class="logbtn">
                            <p>登录</p>
                        </div>
                        <div class="rrl-reference-tilbar">
                            <p v-if="referenceNum.length===0"><span @click="referenceShow">我有推荐人</span><img
                                    src="/Assets/Source/下一页@3x.png"
                                    @click="referenceShow"
                                    class="rrl-reference-pic"
                            /></p>
                            <p v-if="referenceNum.length!==0">你的推荐人手机号码是{{ referenceNum }}</p>
                            <div class="erromsg" style="background:#fff">
                                {{ erro }}
                            </div>
                        </div>
                        <div class="login-terms" style="display:none">
                            登录即表示同意<span>《用户使用条款》</span>
                        </div>
                    </div>
                    
                </div>
                <div class="rrl-login-method">
                    <div class="rrl-login-method-btn">
                        <img src="/Assets/Source/微信@3x.png" alt="" @click="openTpLogin('wx')"/>
                        <img src="/Assets/Source/qq@3x.png" alt="" style="display:none"/>
                        <img src="/Assets/Source/微博@3x.png" alt="" style="display:none"/>
                    </div>    
                </div>
            </div>



            <!--我有推荐人弹框-->
            <div class="rrl-reference-dialog" v-if="dialogShow">
                <div class="rrl-reference-bg"></div>
                <div class="rrl-reference-cont">
                    <div class="rrl-reference-close" @click="dialogShow = false">
                        <img style="float:right;width:.32rem;height:.32rem;" src="/Assets/Source/输入框关闭@3x.png" alt="">
                    </div>
                    <input v-model="referenceNum" :disabled="referenceNumFlag" class="rrl-reference-txt" type="text"
                        placeholder="请输入推荐人号码"/>
                    <p class="rrl-reference-tips">（一旦确认，无法修改，请谨慎填写）</p>
                    <div class="rrl-reference-btn">
                        <input type="button" value="确认" @click="referenceBtn"/>
                        <input type="button" value="取消" @click="dialogShow = false"/>
                    </div>
                </div>
            </div>
            <!--我有推荐人弹框-->

        </div>
    </div>
    <div  class="login-wrap" v-else>
          请更新到最新版本即可分享。
    </div>
</template>

<style scoped>

    /* 修改 */
     .login-wrap {
        padding: .453333rem .5rem .453333rem;
    }
    .rrl-loginbox{
        background: #fff;
        border-radius: 2%;
        overflow: hidden;
        padding: 0 .5rem;
        margin-top: 45%;
        box-shadow:0px -2px 10px #d6cdcd
    }
    .loginbox_padding{
        padding: .5rem 0;
    }
    .logo {
        text-align: center;
        margin: .133333rem auto .333333rem;
        overflow: hidden;
        color: #65c4aa;
        font-size: .68rem;
        font-weight:bold; 
    }
    /* 修改 */
    * {
        margin: 0;
        padding: 0
    }

    button, input {
        outline: none
    }

    .shadow1 {
        z-index: 9;
    }

    .rrl-close-btn img {
        display: block;
        width: .48rem;
        height: .48rem
    }
    .rrl-login-method-btn {
        -ms-flex-pack: justify;
        justify-content: center;
        height: 1.146667rem;
        padding: 0 .666667rem;
        overflow: hidden;
        margin-top: 1.5rem;
    }

    .rrl-login-method-btn, .rrl-login-method-btn img {
        display: -ms-flexbox;
        display: flex
    }

    .rrl-login-method-btn img {
        width: 1.146667rem;
        height: 1.146667rem;
        border-radius: 50%
    }

    .rrl-dync-codebar, .rrl-get-numbar,.rrl_get_yzm {
        width: 100%;
        height: 1.066667rem;
        line-height: 1.066667rem;
        margin-top: .266667rem;
        border: none;
        background: #fff;
        border-bottom: .013333rem solid #cccbcc;
        font-size: .35rem;
        color: #b1b1b9;
        position: relative;
    }
    .rrl-dync-codebar input, .rrl-get-numbar input,.rrl_get_yzm input{
        width: 88%;
    }
     .rrl-dync-codebar img, .rrl-get-numbar img,.rrl_get_yzm img{
        width: 0.7729211087420041rem;
        height: 0.6263326226012793rem;
        vertical-align: middle;
    }
    .rrl-dync-code-til, .rrl-get-num {
           position: absolute;
            top: 50%;
            -webkit-transform: translateY(-50%);
            transform: translateY(-50%);
            right: 0;
            text-align: center;
            color: #fff;
            font-size: .266667rem;
            border-radius: .533333rem;
            width: 1.8rem;
            height: .8rem;
            line-height: .8rem;
    }

    .rrl-dync-code-til {
        /*padding: .04rem .05rem;*/
        padding: 0;
        width: 1.7066666666666668rem;
        height: .72rem;
        overflow: hidden;
    }

    .rrl-dync-code-til img {
        display: block;
        width: 100%;
        height: 100%;
        overflow: hidden;
    }

    .rrl-get-num {
        background: #65c4aa
    }

    .rrl-dync-code-til {
        border: .013333rem solid #91919c
    }

    .login-terms {
        color: #b1b1b9;
        text-align: center;
        font-size: .32rem
    }

    .login-terms span {
        color: #65c4aa
    }

    .rrl-reference-tilbar {
        position: relative;
        text-align: right;
        font-size: .32rem;
        color: #65c4aa;
        padding-right: .08rem;
    }

    .rrl-reference-tilbar .erromsg {
        position: absolute;
        top: 0;
        left: 0;
        font-size: .32rem;
    }

    .rrl-reference-tilbar p {
        position: relative;
    }

    .rrl-reference-tilbar span {
        position: absolute;
        top: 50%;
        right: .346667rem;
        transform: translateY(-50%);
    }
    .logbtn {
        margin: .8rem 0 .533333rem
    }

    .logbtn p {
        width: 100%;
        height: 1.066667rem;
        line-height: 1.066667rem;
        text-align: center;
        background: #65c4aa;
        color: #fff;
        font-size: .426667rem;
        border-radius: .666667rem
    }
    .login-wrap-box{
        height: 17.777185501066096rem;
        background:url('/Assets/Source/login_bg.jpg') no-repeat center top #fff;
        background-size: 100% 100%;
    }
   
   

    .rrl-reference-bg {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, .2);
        z-index: 10;
    }

    .rrl-reference-cont {
        position: absolute;
        top: 50%;
        left: 50%;
        border-radius: .133333rem;
        padding: .533333rem .666667rem;
        transform: translate(-50%, -50%);
        background: #fff;
        width: 7.413333rem;
        height: 5.333333rem;
        z-index: 20;
    }

    .rrl-reference-close {
        position: relative;
        text-align: right;
        width: 100%;
        height: .32rem;
        padding-bottom: .573333rem;
        font-size: .32rem
    }

    .rrl-reference-txt {
        height: .92rem;
        line-height: .92rem;
        width: 100%;
        border-radius: .5rem;
        background: #fff;
        border: .013333rem solid #cccbcc;
        text-indent: .4rem;
        color: #b1b1b9;
        font-size: .36rem
    }

    .rrl-reference-tips {
        padding: .213333rem 0 .626667rem;
        color: #e94246;
        font-size: .28rem;
        text-align: center
    }

    .rrl-reference-btn {
        padding: 0 .453333rem;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-pack: justify;
        justify-content: space-between
    }

    .rrl-reference-btn input {
        width: 48%;
        height: .933333rem;
        line-height: .933333rem;
        border-radius: .666667rem;
        border: none
    }

    .rrl-reference-btn input:first-of-type {
        color: #fff;
        background: #65c4aa;
        font-size: .413333rem
    }

    .rrl-reference-btn input:nth-of-type(2) {
        color: #5a5b68;
        background: #efefef;
        font-size: .413333rem
    }

    .rrl-close-btn {
        margin-bottom: .13333333333333333rem;
        overflow: hidden
    }

    .rrl-close-btn img {
        float: right
    }

    .rrl-login-big {
        text-align: center
    }

    h1[data-v-694cd902], h2[data-v-694cd902] {
        font-weight: 400
    }

    ul[data-v-694cd902] {
        list-style-type: none;
        padding: 0
    }

    li[data-v-694cd902] {
        display: inline-block;
        margin: 0 .133333rem
    }

    a[data-v-694cd902] {
        color: #42b983
    }

    .rrl-reference-pic {
        width: .24rem;
        height: .4266666666666667rem;
    }
</style>

<script>
    import $ from 'jquery';

    let login = {
        data() {
            return {
                username: "",
                userpassword: "",
                dynccode: "",
                spreader: "",
                codeImg: "VerCode/Picture?t=" + new Date().getTime(),
                reg: /^1[0-9]{10}$/,
                erro: '',
                time: 60,
                hassend: false,
                dialogShow: false,
                referenceNum: "",
                referenceNumFlag: false,
                login_wrap_box:true,
            }
        },
        beforeCreate()
        {
        	var allParam = window.UI.Page.getAllparam();
        	if(allParam.channel_open_id)
        	{
        		this.$router.push({path:'/BindAccount',query:allParam});
        	}
        },
        created() {
            //this.loginFilter();
            // 推荐人赋值
            this.getReference();

            if (this.$route.params.username == 'tobind') {
                this.open = true;
            } else {
                this.open = false;
            }
            this.rec = sessionStorage.getItem('recommand')
        },
        directives: {
            focus: {
                // 自定义指令
                inserted: function (el, binding) {
                    if (binding.value) {
                        el.focus()
                    }
                }
            }
        },
        methods: {
            //判断是否为iphone和微信，不是则跳转到原生app登录
            loginFilter(){
                var redirectUrl=window.UI.Page.getAllparam().redirect; 
                var ua = navigator.userAgent.toLowerCase(); 
                if(!(ua.indexOf('iphone')>-1 || ua.indexOf('ipad')>-1 || ua.indexOf('micromessenger')>-1)){
                //if( !(ua.indexOf('micromessenger')>-1)){
                    this.login_wrap_box=false;
                    if(!redirectUrl){
                        var goto_page = "/Device/UserLogin";
                    }else{
                        var goto_page = "/Device/UserLogin?redirect="   + encodeURIComponent(redirectUrl);
                    }
                    window.location.href = goto_page;
                }
            },
            // 去除空格
            stringTrim(val) {
                let result = val,
                    is_glob = "g";
                result = result.replace(/(^\s+)|(\s+$)/g, "");
                if (is_glob.toLowerCase() == "g") {
                    result = result.replace(/\s/g, "");
                }
                return result;
            },
            openTpLogin(type) {
                location.href = "/Device/ThirdPartyLogin?type=" + type;
            },
            getReference() {
                var allParam = window.UI.Page.getAllparam();
                var referenceNumrrl = allParam.reference;
                if (referenceNumrrl) {
                    this.referenceNum = referenceNumrrl;
                    this.referenceNumFlag = true;
                } else {
                    this.referenceNum = "";
                }
            },
            // 隐藏erro
            erroUn() {
                if (this.username.length !== 0 && this.reg.test(this.username)) {
                    this.erro = "";
                    return true;
                }
                this.erro = "手机号码错误";
            },
            erroCode() {
                if (!this.erroUn()) return;
                if (this.userpassword.length !== 0) {
                    this.erro = "";
                    return true;
                }
                this.erro = "验证码错误";
            },
            erroDsCode() {
                if (!(this.erroUn() && this.erroCode())) return;
                if (this.dynccode.length !== 0) {
                    this.erro = "";
                    return true;
                }
                this.erro = "动态验证码错误";
            },
            // 获取动态验证码
            getMathRandomCode() {
                this.codeImg = "VerCode/Picture?t=" + new Date().getTime();
            },
            referenceShow() {
                if (this.referenceNum) {
                    return false;
                }
                this.dialogShow = true;
            },
            // 获取动态验证码
            randomDyncCode() {
                return Math.random() * 1;
            },
            // 确认
            referenceBtn() {
                this.dialogShow = false;
                if (this.referenceNum.length !== 0 && this.reg.test(this.referenceNum) !== true) {
                    alert("请输入正确的推荐人手机号码！");
                    this.referenceNum = "";
                    return;
                }
                if (this.referenceNum) {
                    this.referenceNumFlag = true;
                }
            },
            // 获取url参数
            getUrlParam(paraName) {
                var url = document.location.toString();
                var arrObj = url.split("?");

                if (arrObj.length > 1) {
                    var arrPara = arrObj[1].split("&");
                    var arr;

                    for (var i = 0; i < arrPara.length; i++) {
                        arr = arrPara[i].split("=");

                        if (arr != null && arr[0] == paraName) {
                            return arr[1];
                        }
                    }
                    return "";
                }
                else {
                    return "";
                }
            },
            //登录
            tologin() {
                const that = this,
                    fromcodeURL = window.UI.Page.getAllparam().fromcode || "";   // 获取url地址栏的fromcode的值
                if (this.username.length === 0 || this.reg.test(this.username) !== true) {
                    this.erro = '手机号码错误';
                } else if (this.userpassword.length === 0) {
                    this.erro = '验证码错误';
                } else if (this.dynccode.length === 0) {
                    this.erro = '动态验证码错误';
                } else {
                    $.ajax({
                        url: '/WebApi/UserManager/ResgistWithSMS?UserName=' + that.stringTrim(that.username) + '&SMSCode=' + that.stringTrim(that.userpassword) + '&spreader=' + that.stringTrim(that.referenceNum) + '&devicecod=&picture_code=' + that.stringTrim(that.dynccode) + '&fromcode=' + that.stringTrim(fromcodeURL) + '&channel=wx',
                        type: 'get',
                        complete: (xhr) => {
                            this.erro = "";
                            // 失败
                            if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                                alert("请求服务器异常!");
                                return;
                            }
                            let data = xhr.responseJSON;

                            if (data.status === 91) {
                                that.referenceNum = "";
                                alert(data.message);
                                return;
                            }
                            if (data.status !== 0) {
                                alert(data.message);
                                return;
                            }

                            that.$store.commit('SET_SHORTTOKEN', data.data);

                            localStorage.setItem('longtoken', data.additional_data);
                            localStorage.setItem('shorttoken', data.data);
                            localStorage.setItem('username', that.stringTrim(that.username));
                            var allParam = window.UI.Page.getAllparam();


                            var redirect_to = "/#/my";


                            // 推荐人   this.referenceNum
                            /*var referenceNumrrl = allParam.reference;
                            if (!referenceNumrrl) {
                            	  var self_href=window.location.href;
                            	  if(/reference=\d{11}/.test(self_href))
                            	  {
                            	  	referenceNumrrl="1111";
                            	  }
                            }
                            if (referenceNumrrl) {
                                //window.location.href = "https://e-shop.rrlsz.com.cn/ProductForGame";
                                //return;
                                redirect_to="https://e-shop.rrlsz.com.cn/ProductForGame";
                            } 
                            */

                            const para = allParam.to || allParam.redirect;
                            if (para) {
                                //window.location.href = para.replace("#token", that.$store.state.shorttoken);
                                redirect_to = para.replace("#token", that.$store.state.shorttoken);
                                if(para.indexOf("gamehall") > -1 && data.add_data.isNewUser == 1){
                                	// 如果是返回到休闲区首页，并且是新用户，则增加新用户参数到返回地址
                                	if(redirect_to.indexOf('?') == -1){
                                		redirect_to += '?isNewUser=1';
                                	}
                                	else {
                                		redirect_to += '&isNewUser=1';
                                	}
                                }
                            } else {
                                //that.$router.push('/my')
                                redirect_to = "/#/my";
                            }
                            //采集设备信息

                            var ua = navigator.userAgent.toLowerCase();
                            //alert(ua);
                            var isWeixin = ua.indexOf('micromessenger') != -1;
                            
                            // lcl 2018-06-13 Insert
                            if(isWeixin){
                            	$.get('/WebApi/Game/SpreaderShareRedpackageForWeChat',{token:that.$store.state.shorttoken, openId:window.GLOBLE_OPEN_ID});
                            }

                            if (redirect_to.indexOf("http") == -1) {
                                redirect_to = window.location.origin + redirect_to;
                            }
                            //alert(redirect_to);
                            var goto_page = "/Device/LoginInfo?token=" + data.data + "&redirect=" + encodeURIComponent(redirect_to) + "&alias=" + that.stringTrim(that.username);
                            window.location.href = goto_page;

                            return;

                            if (!isWeixin) {//不是微信
                                if (ua.indexOf('mobile') > -1 || ua.indexOf('mqqbrowser') > -1 || ua.indexOf('iphone') > -1) {

                                } else {
                                    if (redirect_to == "/#/my") {
                                        that.$router.push('/my');
                                    } else {
                                        window.location.href = redirect_to;
                                    }
                                }
                            } else {
                                //微信
                                if (redirect_to == "/#/my") {
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
                let params = window.UI.Page.getAllparam(),
                    redirectTo = params.redirect || params.to;
                if (redirectTo) {
                    if (location.href.indexOf("http") > -1) {
                        location.href = redirectTo;
                        return;
                    }
                    //location.href=redirectTo;
                    redirectTo = redirectTo.replace("/#/", "/");
                    this.$router.push(redirectTo);
                } else {
                    try {
                        window.history.go(-1);
                        this.$router.go(-1);
                    } catch (e) {
                        window.location.href = "/#/index";
                    }
                }
            },
            //发送验证码
            getsms() {
                if (this.reg.test(this.username)) { /*this.erro.length !== 0 &&*/
                    this.erro = "";
                    this.sendsms();
                    return;
                }
                this.erro = "手机号码错误";
            },
            //发送验证码
            sendsms() {
                const that = this;
                that.getphone(); // 首先判断是不是合法的手机电话号码
                $.ajax({
                    url: 'WebApi/AuthManager/SendSMS?mobile=' + this.username,
                    type: "get",
                    success: function (data) {
                        if (data.status === 0) {
                            that.hassend = true;
                            that.sh = setInterval(function () {
                                that.time--;
                                if (that.time === 0) {
                                    that.hassend = false;
                                    that.time = 60;
                                    clearInterval(that.sh);
                                }
                            }, 1000);
                            return;
                        }
                        if (data.status === 94) {
                            alert(data.message);
                            return;
                        }
                        alert('发送失败，请稍后重试');
                    }
                })
            },
            //手机号及合法性判定
            getphone() {
                if (this.reg.test(this.username)) {
                    this.erro = '';
                    return;
                }
                this.erro = "手机号码错误";
            },
            beforeDestroy() {
                //清除验证码计时器
                clearInterval(this.sh);
            }
        }

    }
    export default login
</script>