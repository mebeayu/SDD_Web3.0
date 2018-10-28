<template>
    <div style="background:#f5f5f5" >
    <goodsheader></goodsheader>
<div class="goods-detail-rule">
        <div class="goods-detail-name">
            <div class="GD-name-left">
                <p class="GD-name">
                    {{this.$store.state.goodsdetail.name}}
                </p>
                <p class="GD-intro">
                    {{this.$store.state.goodsdetail.propaganda}}
                </p>
                <p class="GD-price" >
                    兑换价：
                    <span>￥{{this.$store.state.goodsdetail.price}} + {{this.$store.state.goodsdetail.discount}}金币</span>
                </p>
                <p class="GD-return" >
                    常规价：￥{{this.$store.state.goodsdetail.local_price}}
                </p>
            </div>
            <div class="GD-name-right">
                <!--div v-if="this.$store.state.goodsfav==false">
                    <img src="/Assets/Source/img/收藏@3x.png" v-on:click="addfavgoods" />
                    <p>收藏</p>
                </div>
                <div v-else>
                    <img src="/Assets/Source/img/已收藏@3x.png" v-on:click="removefavgoods" />
                    <p>已收藏</p>
                </div-->
                <div v-on:click='share'>
                    <img src="/Assets/Source/img/分享@3x.png" />
                    <p>分享</p>
                </div>
            </div>
        </div>
        <div class="GD-getway">
            <div class="GD-getway-left">
                领取方式:<span v-if="this.$store.state.goodsdetail.postage==0">快递包邮</span>
                <span v-else>运费：{{this.$store.state.goodsdetail.postage}}</span>
            </div>
            <div class="GD-getway-right">
                <p v-if="this.$store.state.goodsdetail.sell_count_visible==true" style="display:inline-block">
                已售:<span >{{this.$store.state.goodsdetail.sell_count}}</span>
                </p>
                <p v-if="this.$store.state.goodsdetail.inv_count_visible==true" style="display:inline-block">
                剩余：<span v-if="this.$store.state.goodsdetail.inv_count!=-1">{{this.$store.state.goodsdetail.inv_count}}</span><span v-else>999</span>
                </p>
            </div>
        </div>
        <ul class="GD-rule">
            <li v-if="this.$store.state.goodsdetail.is_real==true">
                <img src="/Assets/Source/img/支持icon@3x.png" />
                正品保证
            </li>
            <li>
                <img src="/Assets/Source/img/支持icon@3x.png" />
                返{{this.$store.state.goodsdetail.return_money_rate*100}}%
            </li>
            <li v-if="this.$store.state.goodsdetail.is_quick_refund==true">
                <img src="/Assets/Source/img/支持icon@3x.png" />
                急速退款
            </li>
            <li>
                <img src="/Assets/Source/img/支持icon@3x.png" />
                7日可达
            </li>
            <li v-if='this.$store.state.goodsdetail.is_unuse_refund==true'>
                <img src="/Assets/Source/img/支持icon@3x.png" />
                未使用可退换
            </li>
            <li>
                <img src="/Assets/Source/img/支持icon@3x.png" />
                包装损坏可退换
            </li>
            <li v-if="this.$store.state.goodsdetail.is_can_not_refund==true">
                <img src="/Assets/Source/img/不支持icon@3x .png" />
                不支持退换
            </li>
        </ul>
    </div>    <!--selfshop v-bind:goodsdetail="goodsdetail"></selfshop-->
    <goodsinfo v-bind:style="{marginBottom:this.$store.state.bottomHeight+'px'}" style="background:#fff"></goodsinfo>
    <div class="goods-detail-buytool" id="goodsbottom">
        <div>
            <div v-if="this.$store.state.goodsfav==false||typeof(this.$store.state.goodsfav) == 'undefined'">
                <img src="/Assets/Source/img/收藏@3x.png" v-on:click="addfavgoods" />
                <p>收藏</p>
            </div>
            <div v-else>
                <img src="/Assets/Source/img/collection.png" v-on:click="removefavgoods" />
                <p style="color:#ba3027">已收藏</p>
            </div>
            <input type="button" value="立即兑换" v-on:click="buy" style='color:#fff;background:#ba3027'/>
        </div>
    </div>    <alertbox></alertbox>
    <kefubox></kefubox>
</div>   
</template>

<script>
    import goodsheader from './goodsdetail/goods-detail-header';
    import goodsinfo from './goodsdetail/goods-dtail-info';
    import selfshop from './goodsdetail/selfshop';
    import alertbox from './alertbox/alertbox';
    import kefubox from './alertbox/kefubox'

    let coingoods = {
        data(){
            return {
                path:'',
                sharemsg:{},
                sharetimeline:{}
            }
        },
        components:{
            goodsheader,
            goodsinfo,
            selfshop,
            alertbox,
            kefubox
        },
        created(){
            this.$store.commit('setgoodsdetail',{})
            //设置商品id
            this.$store.commit('setgoodsid',this.$route.params.goodsid)
            //获取商品详情
            this.$store.dispatch('setgoodsdetail')
            //设置轮播图下标为0
            this.$store.commit('setimgindex')
            let urlarray=window.location.href.split('#')
            let url = urlarray[0].split('?')[0]
            let urllast = urlarray[1]
            this.path=url+'#'+urllast
            //获取微信分享配置
            this.$store.dispatch('getwxshare',url)
            sessionStorage.setItem('recommand',this.$route.params.username)
            
        },
        computed:{
            //获取微信分享配置
            getconfig(){
                return this.$store.state.wxshare
            },
            //获取用户信息
            setuserinfo(){
                return this.$store.state.useinfo
            },
            //获取商品详情
            getdetail(){
                return this.$store.state.goodsdetail
            }
        },
        watch:{
            getconfig(){
                wx.config(this.$store.state.wxshare)
            },
            // setuserinfo(){
            //     let name=this.$store.state.useinfo.username
            //     let reg=/null/g
            //     this.path=window.location.href.replace(reg,name)
            //     this.$store.commit('sharepath',this.path)
            // },
            getdetail(){
                //配置分享内容
                if(this.$store.state.goodsdetail.name != undefined){
                    let name=sessionStorage.getItem('myid')
                    let reg=/null/g
                    this.path=this.path.replace(reg,name)
                    this.$store.commit('sharepath',this.path)
                    this.sharemsg={
                        title: this.$store.state.goodsdetail.name, // 分享标题
                        desc: this.$store.state.goodsdetail.propaganda, // 分享描述
                        link: this.$store.state.sharepath, // 分享链接
                        imgUrl: 'https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/'+this.$store.state.goodsdetail.pic_id+'.jpg', // 分享图标
                        type: '', // 分享类型,music、video或link，不填默认为link
                        success: function() {
                            alert('分享成功')
                        },
                        cancel: function() {
                            alert('分享取消')
                        }
                    }
                    this.sharetimeline={
                        title: this.$store.state.goodsdetail.name, // 分享标题
                        link:this.$store.state.sharepath,
                        imgUrl: 'https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/'+this.$store.state.goodsdetail.pic_id+'.jpg', // 分享图标
                        success: function() {
                            alert('分享成功')// 用户确认分享后执行的回调函数
                        },
                        cancel: function() {
                            alert('分享取消')// 用户取消分享后执行的回调函数
                        }
                    }
                    let that=this
                    wx.ready(function() {
                        wx.onMenuShareAppMessage(that.sharemsg);
                        wx.onMenuShareTimeline(that.sharetimeline);
                    });
                }
            }
        },
        mounted(){
            //获取底部导航高度
            this.$store.commit('toolheight')            
        },
        methods:{
            //从商品页创建订单
            buy(){
                if(this.$store.state.shorttoken.length === 0){
                    this.$router.push('/login_new')
                }else{
                    if (this.$store.state.goodsdetail.discount > this.$store.state.goodsdetail.plate_to_return_money){
                        alert('金币不足，无法抵扣')
                    }else {
                        let goods=[this.$store.state.goodsdetail]
                        this.$store.commit('setcartitem',goods)
                        this.$store.dispatch('makeorderfromgoods')                            
                        this.$router.push('/makesureorder/:goodsdetail'+this.$store.state.orderid)
                    }
                    
                    
                    //this.$router.push('/makesureorder/:goodsdetail')
                }
            },
            //添加商品收藏
            addfavgoods(){
                //console.log(typeof(this.$store.state.longtoken)=='undefined')
                if(this.$store.state.shorttoken.length === 0){
                    confirm('尚未登录，是否登录？', () => {
                        this.$router.push('/login_new')
                    })
                }else{
                    this.$store.state.goodsfav=true
                    this.$store.dispatch('addgoodsfav')
                }

            },
            //移除商品收藏
            removefavgoods(){
                this.$store.state.goodsfav=false
                this.$store.dispatch('removegoodsfav')
            },
            share(){
                alert('请点击右上角分享')
            }
        }
    }

    export default coingoods
</script>