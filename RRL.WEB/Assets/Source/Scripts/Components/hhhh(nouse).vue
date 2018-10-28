<!-->新人有礼页面<-->
<template>
    <div>
        <div class="closehhh">
            <p v-on:click="back">
                <img src="/Assets/Source/img/back.png" />
                关闭
            </p>
               省兜兜商城
        </div>
        <div class="hhh-regbox">
            <p style="padding:0 15px;box-sizing:border-box;height:auto">
                <img src='/Assets/Source/img/新人有礼.png' style="width:100%;border-top:0;border-left:0;border-right:0;padding-bottom:15px;" />
            </p>
            <img src='/Assets/Source/img/优惠卷.png' />
            <div>
                <input style="width:100%;height:100%;text-align:center"
                        placeholder="请输入您的手机号"
                        v-model="username"
                        v-on:blur="getphone" />
            </div>
            <div class="hh-getsms">
                <input placeholder="请输入验证码" v-model ="userpassword" />
                <div v-on:click="getsms" v-if="hassend==false">
                    验证码
                </div>
                <div class="hassend" v-else>已发送({{time}}s)</div>
            </div>
            <div>
                <p style="width:100%;height:100%;text-align:center;background:#ba3027;color:#fff;line-height:36px;border-radius:8px;"
                    v-on:click="logincli">
                        领取礼包并注册
                </p>
            </div>
            <div class="read">
                <input id='read' type="checkbox" v-on:click="isread" v-model="aaa" />
                <router-link tag="label" to="/gamerule">
                        已阅读并同意活动规则
                </router-link>
            </div>
            <div class="erromsg" style="font-size:18px;font-weight:600">
                {{erro||this.$store.state.loginError}}
            </div>
        </div>
        <div class="hhh-sale">
            <span></span>
            <span>品质精选</span>
            <span></span>
        </div>
        <div class="sale-goods-box">
            <ul class="sale-goods" style="justify-content: space-between ;width:100%;">
                <li>
                    <router-link :to="{name:'goodsdetail',params: { goodsid: this.$store.state.mainrecommend[0].id }}">
                        <img v-bind:src="$store.state.imgs+this.$store.state.mainrecommend[0].pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" />
                        <p class="sale-goods-name">{{this.$store.state.mainrecommend[0].name}}</p>
                        <p class="sale-goods-intro">{{this.$store.state.mainrecommend[0].propaganda}}</p>
                        <p class="sale-goods-price">￥{{this.$store.state.mainrecommend[0].price}}</p>
                        <p class="sale-goods-return">奖励￥{{this.$store.state.mainrecommend[0].price*this.$store.state.mainrecommend[0].return_money_rate}}</p>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{name:'goodsdetail',params: { goodsid: this.$store.state.mainrecommend[1].id }}">
                        <img v-bind:src="$store.state.imgs+this.$store.state.mainrecommend[1].pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" />
                        <p class="sale-goods-name">{{this.$store.state.mainrecommend[1].name}}</p>
                        <p class="sale-goods-intro">{{this.$store.state.mainrecommend[1].propaganda}}</p>
                        <p class="sale-goods-price">￥{{this.$store.state.mainrecommend[1].price}}</p>
                        <p class="sale-goods-return">奖励￥{{this.$store.state.mainrecommend[1].price*this.$store.state.mainrecommend[1].return_money_rate}}</p>
                    </router-link>
                </li>
                <li>
                    <router-link :to="{name:'goodsdetail',params: { goodsid: this.$store.state.mainrecommend[2].id }}">
                        <img v-bind:src="$store.state.imgs+this.$store.state.mainrecommend[2].pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" />
                        <p class="sale-goods-name">{{this.$store.state.mainrecommend[2].name}}</p>
                        <p class="sale-goods-intro">{{this.$store.state.mainrecommend[2].propaganda}}</p>
                        <p class="sale-goods-price">￥{{this.$store.state.mainrecommend[2].price}}</p>
                        <p class="sale-goods-return">奖励￥{{this.$store.state.mainrecommend[2].price*this.$store.state.mainrecommend[2].return_money_rate}}</p>
                    </router-link>
                </li>
            </ul>
        </div>
        <div class="hhh-intro">
            <div>
                <div>
                    <header>【活动介绍】</header>
                    <p>1.输入手机号注册，即可领取50元奖励金</p>
                    <p>2.此奖励金每个账户ID限领一次</p>
                    <p>3.此50元奖励金仅限于在【省兜兜】平台使用或提现，不得做其他用途</p>
                    <p>4.使用规则遵循省兜兜平台规则</p>
                    <p>5.活动最终解释权归省兜兜所有</p>
                </div>
                <div>
                    <header>【活动介绍】</header>
                    <p>
                        省兜兜以提供本地优质商品和服务为核心，更推出消费奖励新模式，让消费者用更少的钱享受到更高的生活品质，为消费者打造一个低价买正品、消费有奖励的购物平台。
                    </p>
                </div>
            </div>
            <!--p>人人利网络科技（深圳）有限公司</p-->
        </div>
    </div>
</template>

<script>
    let hhhh={
        data(){
            return {
                username:'',
                userpassword:'',
                reg:/^1[0-9]{10}$/,
                erro:'',
                time:30,
                hassend:false,
                hasread:true,
                aaa:true,
                sh:{},
                recommand:'',
                sharetimeline:'',
                sharetimeline:''
            }
        },
        created(){
            //判定是否有商品列表
            if(this.$store.state.mainrecommend.length==0){
                this.$store.dispatch('setmaingoods')
            }
            let url=window.location.href
            url=url.split('#')
            url=url[0]
            //this.$store.dispatch('getwxshare')
            //获取微信分享配置
            this.$store.dispatch('getwxshare',url)
            if(this.$store.state.longtoken==undefined){
                this.path='https://e-shop.rrlsz.com.cn/#/hhhh/:'
                this.$store.commit('sharepath',this.path)
            }else{
                //获取用户信息
                this.$store.dispatch('getuserinfo')
            }
            //将推荐人信息存入缓存
            sessionStorage.setItem('recommand',this.$route.params.username)
            if(this.$route.params.username!=':'){
                sessionStorage.setItem('recommand',this.$route.params.username)
            }
            //获取推荐人信息
            this.recommand=sessionStorage.getItem("recommand")
        },
        computed:{
            getreg(){
                //获取shorttoken
                return this.$store.state.shorttoken
            },
            getconfig(){
                //获取分享配置
                return this.$store.state.wxshare
            },
            setuserinfo(){
                //获取用户信息
                return this.$store.state.useinfo
            }
        },
        watch:{
            getreg(){
                if(this.$store.state.loginError.length == 0){
                    this.$router.push('/ffff/:hhhh')
                }
            },
            getconfig(){
                wx.config(this.$store.state.wxshare)
            },
            setuserinfo(){
                let that=this
                let name=this.$store.state.useinfo.username
                this.path='https://e-shop.rrlsz.com.cn/#/hhhh/'+name
                this.$store.commit('sharepath',this.path)
                this.sharemsg={
                        title: '【省兜兜】新用户送50元现金红包', // 分享标题
                        desc: '低价购正品，购物有奖励，多省一点，奖给生活。', // 分享描述
                        link: that.$store.state.sharepath, // 分享链接
                        imgUrl: that.$store.state.sharelogo, // 分享图标
                        type: '', // 分享类型,music、video或link，不填默认为link
                        success: function() {
                            alert('分享成功')
                        },
                        cancel: function() {
                            alert('分享取消')
                        }
                    },
                this.sharetimeline={
                        title: '【省兜兜】新用户送50元现金红包', // 分享标题
                        link:that.$store.state.sharepath,
                        imgUrl: that.$store.state.sharelogo, // 分享图标
                        success: function() {
                            alert('分享成功')// 用户确认分享后执行的回调函数
                        },
                        cancel: function() {
                            alert('分享取消')// 用户取消分享后执行的回调函数
                        }
                    }
                }
        },
        methods:{
            //用户注册
            logincli(){
                var that=this
                this.$store.commit('setuser_info',{name:that.username,password:that.userpassword,rec:that.recommand})
                if(this.aaa == true){
                    this.erro=''
                    this.$store.dispatch('toregist')
                }else{
                    this.erro="请阅读并同意活动规则"
                }
            },
            back(){
                this.$router.push('/index')
            },
            //获取验证码
            getsms(){
                var that=this
                if(this.erro==''&&this.reg.test(this.username)){
                    this.$store.dispatch('sendsms',that.username)
                    this.hassend=true
                    this.sh=setInterval(function(){
                        that.time--
                        console.log(that.time)
                        if(that.time==0){
                            that.hassend=false
                            clearInterval(that.sh)
                            that.time=30
                        }
                    },1000)
                }else{
                    this.erro="请输入正确的手机号码"
                }
            },
            //手机号及合法性判定
            getphone(){
                if(this.reg.test(this.username)){
                    this.erro=''
                }else{
                    this.erro="请输入正确的手机号码"
                }
            },
            //是否勾选同意规则
            isread(){
                this.aaa=!this.aaa
            },
            //添加商品至购物车
            addgoods(e){
                this.$store.commit('setgoodsid',e)
                this.$store.dispatch('addgoods',1)
                this.$store.commit('changealertshow',true)
            }
        },
        beforeDestroy(){
            //清除验证码计时器
            clearInterval(this.sh)
        },
        mounted(){
            //微信配置
            let that=this
            wx.ready(function() {
                wx.onMenuShareAppMessage(that.sharemsg);
                wx.onMenuShareTimeline(that.sharetimeline);
            });
        }
    }

    export default hhhh
</script>