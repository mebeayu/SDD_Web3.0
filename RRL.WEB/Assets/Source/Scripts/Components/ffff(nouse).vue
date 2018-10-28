<!-->新人有礼领取成功后的页面<-->
<template>
    <div>
        <div class="closehhh">
            <p v-on:click="back">
                <img src="/Assets/Source/img/back.png" />
                关闭
            </p>
               省兜兜商城
        </div>
        <div style="text-align=center;margin-top:63px;" class="rulerule">
            <header v-if="this.$store.state.loginError!='用户已经存在'">
                {{thewords}}
            </header>
            <header v-else>
                您已经领取过了~
            </header>
            <p>
                去省兜兜商城逛一哈~
            </p>
        </div>
        <div class="grbtn">
            <input type="button" value="去商城逛逛" style="background:#ba3027;color:#fff;
            font-size:14px;" v-on:click="toshop">
            <input type="button" value="分享" style="background:#fff;color:#000;
            font-size:14px;border:1px solid #ba3027;box-sizing:border-box" v-on:click="share">
        </div>
        <div class="hhh-intro">
            <div>
                <div>
                    <header>使用介绍：</header>
                    <p>1. 此50元奖励金可在【省兜兜】平台使用或提现；</p>
                    <p>2.领取成功后，系统会自动将50元划入您省兜兜账户中的“待返奖励”；</p>
                    <p>3.“待返金额”中的50元会根据日平台收益，每日将50元中相应的金额逐渐返到“我的钱包”，直到返完为止；</p>
                    <p>4.“我的钱包”的金额可在平台购物，也可提现到您的银行卡；</p>
                    <p>5.红包余额查看和提现可到微信公众号【省兜兜】和APP【省兜兜】中登录查看即可。可扫描或识别下方二维码关注下载。</p>
                </div>
            </div>
            <p style="display:flex;justify-content:center;margin-top:20px;margin-bottom:20px;width:100%;">
                    <img style="width:100px;height:100px;" src="/Assets/Source/img/省兜兜微信服务号.jpg" />
            </p>
            <!--p>人人利网络科技（深圳）有限公司</p-->
        </div>
    </div>
</template>

<script>
    let ffff={
        data(){
            return {
                thewords:'',
                sharetimeline:'',
                sharetimeline:''
            }
        },
        methods:{
            back(){
                this.$router.push('/index')
            },
            //转跳去精选
            toshop(){
                this.$router.push('/goodssort')
            },
            //分享提示s
            share(){
                alert('请点击右上角分享')
                // let that=this
                // wx.ready(function() {
                //     wx.onMenuShareAppMessage({
                //         title: '【省兜兜】新用户送50元红包！', // 分享标题
                //         desc: '低价购正品，购物有奖励，多省一点，奖给生活。新用户注册即可领50元现金红包', // 分享描述
                //         link: that.$store.state.sharepath, // 分享链接
                //         imgUrl: that.$store.state.sharelogo, // 分享图标
                //         type: '', // 分享类型,music、video或link，不填默认为link
                //         success: function() {
                //             alert('分享成功')
                //         },
                //         cancel: function() {
                //             alert('分享取消')
                //         }
                //     });
                //     wx.onMenuShareTimeline({
                //         title: '【省兜兜】新用户送50元红包！', // 分享标题
                //         link:that.$store.state.sharepath,
                //         imgUrl: that.$store.state.sharelogo, // 分享图标
                //         success: function() {
                //             alert('分享成功')// 用户确认分享后执行的回调函数
                //         },
                //         cancel: function() {
                //             alert('分享取消')// 用户取消分享后执行的回调函数
                //         }
                //     });
                // });
            }
        },
        created(){
            //判定是否登录显示
            if(this.$route.params.from==':index'){
                this.thewords='您已领取过了'
            }else{
                console.log(this.$route.params.from)
                this.thewords='领取成功'
            }
            let url=window.location.href
            url=url.split('#')
            url=url[0]
            //this.$store.dispatch('getwxshare')
            this.$store.dispatch('getwxshare',url)
            if(this.$store.state.longtoken==undefined){
                this.path='https://e-shop.rrlsz.com.cn/#/hhhh/:'
                this.$store.commit('sharepath',this.path)
            }else{
                //this.$store.dispatch('getuserinfo')
            }
            sessionStorage.setItem('recommand',this.$route.params.username)
        },
        computed:{
            //获取分享config
            getconfig(){
                return this.$store.state.wxshare
            },
            //获取用户信息
            setuserinfo(){
                return this.$store.state.useinfo
            }
        },
        watch:{
            getconfig(){
                wx.config(this.$store.state.wxshare)
            },
            // setuserinfo(){
            //     let that=this
            //     let name=this.$store.state.useinfo.username
            //     this.path='https://e-shop.rrlsz.com.cn/#/hhhh/'+name
            //     this.$store.commit('sharepath',this.path)
            //     this.sharemsg={
            //             title: '【省兜兜】新用户送50元现金红包', // 分享标题
            //             desc: '低价购正品，购物有奖励，多省一点，奖给生活。', // 分享描述
            //             link: that.$store.state.sharepath, // 分享链接
            //             imgUrl: that.$store.state.sharelogo, // 分享图标
            //             type: '', // 分享类型,music、video或link，不填默认为link
            //             success: function() {
            //                 alert('分享成功')
            //             },
            //             cancel: function() {
            //                 alert('分享取消')
            //             }
            //         },
            //     this.sharetimeline={
            //             title: '【省兜兜】新用户送50元现金红包', // 分享标题
            //             link:that.$store.state.sharepath,
            //             imgUrl: that.$store.state.sharelogo, // 分享图标
            //             success: function() {
            //                 alert('分享成功')// 用户确认分享后执行的回调函数
            //             },
            //             cancel: function() {
            //                 alert('分享取消')// 用户取消分享后执行的回调函数
            //             }
            //         }
            //     }
        },
        mounted(){
            //分享配置
            let that=this
            let name=sessionStorage.getItem('myid')
            this.path='https://e-shop.rrlsz.com.cn/#/hhhh/'+name
            this.$store.commit('sharepath',this.path)
            wx.ready(function() {
                wx.onMenuShareAppMessage({
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
                });
                wx.onMenuShareTimeline({
                        title: '【省兜兜】新用户送50元现金红包', // 分享标题
                        link:that.$store.state.sharepath,
                        imgUrl: that.$store.state.sharelogo, // 分享图标
                        success: function() {
                            alert('分享成功')// 用户确认分享后执行的回调函数
                        },
                        cancel: function() {
                            alert('分享取消')// 用户取消分享后执行的回调函数
                        }
                });
            });
        }
    }

    export default ffff
</script>   