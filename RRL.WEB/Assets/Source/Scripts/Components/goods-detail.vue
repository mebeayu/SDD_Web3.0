<!-->商品详情页<-->
<template>
<div style="background:#f5f5f5;" >
    <goodsheader v-bind:goodsdetail="goodsdetail" v-bind:goodsbanner='goodsbanner'></goodsheader>
    <goodsrule :goodsdetail='goodsdetail'  style="background:#fff"></goodsrule>
    <!--selfshop v-bind:goodsdetail="goodsdetail"></selfshop-->
    <goodsinfo id = 'last-item' style="background:#fff"></goodsinfo>
    <goodsbuy v-bind:goodsdetail='goodsdetail'></goodsbuy>
    <alertbox></alertbox>
    <kefubox></kefubox>
    <!--<shareframe :goodsdetail='goodsdetail'></shareframe>-->
    <img src='/Assets/Source/img/totop.png' class='goodstotop' 
    v-on:click ='totop'/>
</div>
</template>

<script>
    //引入头部 规则 信息 店铺 购买栏 添加至购物车弹出框 客服弹出框 分享弹出框
    import goodsheader from './goodsdetail/goods-detail-header';
    import goodsrule from './goodsdetail/goods-detail-rule';
    import goodsinfo from './goodsdetail/goods-dtail-info';
    import selfshop from './goodsdetail/selfshop';
    import goodsbuy from './goodsdetail/goods-detail-buy';
    import alertbox from './alertbox/alertbox';
    import kefubox from './alertbox/kefubox';
    import goodsguige from './goodsdetail/goods-detail-guige';
    import pageVisit from "./PublicModules/PageVisit.js";
    import shareframe from "./goodsdetail/goods-detail-frame"

    // import $ from 'jquery';
   
    let goodsdetail={
        data(){
        	//debugger;
            return {
                path:'',
                sharemsg:{},
                sharetimeline:{},
                hash:'',
                goodsdetail:{},
                goodsbanner:[],
                startTime: null,
                currentPageUrl: '',
                currentGoodsId: 0,
                isDialog:false
            }
        },
        components:{
            goodsheader,
            goodsrule,
            goodsinfo,
            selfshop,
            goodsbuy,
            alertbox,
            kefubox,
            goodsguige,
            shareframe
        },
        activated(){
        },
        created(){
            this.$store.commit('SET_GOODSDETAIL',{})
            //设置商品id
            this.$store.commit('setgoodsid',this.$route.params.goodsid)
            //获取商品详情
            this.$store.dispatch('setgoodsdetail')
            .then(() => {
            	
                this.goodsdetail = this.$store.state.goodsdetail.goodsdetail;
                this.goodsbanner = this.$store.state.goodsdetail.goodsbanner
               // console.log(this.$store.state.goodsdetail.goodsdetail)
                let urlarray=window.location.href.split('#')
                let url = urlarray[0].split('?')[0]
                let urllast = urlarray[1]
                this.hash = urllast
                this.path=url+'#'+urllast
                let name=sessionStorage.getItem('myid')
                let reg=/null/g
                this.path=this.path.replace(reg,name)
                this.$store.dispatch('getwxshare',url)
                .then(() => {
                    if(sessionStorage.getItem('myid')) {
                        this.sharemsg={
                            title: this.goodsdetail.name, // 分享标题
                            desc: this.goodsdetail.propaganda, // 分享描述
                            link: 'https://e-shop.rrlsz.com.cn/login?' + 'hash=' + this.hash, // 分享链接
                            imgUrl: 'https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/'+this.goodsdetail.pic_id+'.jpg', // 分享图标
                            type: '', // 分享类型,music、video或link，不填默认为link
                            success: function() {
                                alert('分享成功')
                            },
                            cancel: function() {
                                alert('分享取消')
                            }
                        }
                        this.sharetimeline={
                            title: this.goodsdetail.name, // 分享标题
                            link:'https://e-shop.rrlsz.com.cn/login?' + 'hash=' + this.hash,
                            imgUrl: 'https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/'+this.goodsdetail.pic_id+'.jpg', // 分享图标
                            success: function() {
                                alert('分享成功')// 用户确认分享后执行的回调函数
                            },
                            cancel: function() {
                                alert('分享取消')// 用户取消分享后执行的回调函数
                            }
                        }
                        wx.ready(function() {
                            wx.onMenuShareAppMessage(this.sharemsg);
                            wx.onMenuShareTimeline(this.sharetimeline);
                        });
                    }
                })
            })
            //设置轮播图下标为0
            this.$store.commit('setimgindex')
            
            //获取微信分享配置
            
            if( this.$route.params.username != 'null' ) {
                sessionStorage.setItem('recommand',this.$route.params.username)
            }
            
            this.startTime = new Date().getTime();
            this.currentPageUrl = location.href;
            this.currentGoodsId = this.$route.params.goodsid;
        },
        // computed:{
        //     //获取微信分享配置
        //     getconfig(){
        //         return this.$store.state.wxshare
        //     },
        //     //获取用户信息
        //     setuserinfo(){
        //         return this.$store.state.useinfo
        //     },
        //     //获取商品详情
        //     getdetail(){
        //         return this.$store.state.goodsdetail
        //     }
        // },
        // watch:{
        //     getconfig(){
        //         wx.config(this.$store.state.wxshare)
        //     },
        //     // setuserinfo(){
        //     //     let name=this.$store.state.useinfo.username
        //     //     let reg=/null/g
        //     //     this.path=window.location.href.replace(reg,name)
        //     //     this.$store.commit('sharepath',this.path)
        //     // },
        //     getdetail(){
        //         //配置分享内容
        //         if(this.$store.state.goodsdetail.name != undefined){
        //             let name=sessionStorage.getItem('myid')
        //             let reg=/null/g
        //             this.path=this.path.replace(reg,name)
        //             this.hash = this.hash.replace(reg, name)
        //             this.$store.commit('sharepath',this.path)
        //             this.sharemsg={
        //                 title: this.$store.state.goodsdetail.name, // 分享标题
        //                 desc: this.$store.state.goodsdetail.propaganda, // 分享描述
        //                 link: 'https://e-shop.rrlsz.com.cn/login?' + 'hash=' + this.hash, // 分享链接
        //                 imgUrl: 'https://e-shop.rrlsz.com.cn/WebApi/Public/picture/'+this.$store.state.goodsdetail.pic_id, // 分享图标
        //                 type: '', // 分享类型,music、video或link，不填默认为link
        //                 success: function() {
        //                     alert('分享成功')
        //                 },
        //                 cancel: function() {
        //                     alert('分享取消')
        //                 }
        //             }
        //             this.sharetimeline={
        //                 title: this.$store.state.goodsdetail.name, // 分享标题
        //                 link:this.$store.state.sharepath,
        //                 imgUrl: 'https://e-shop.rrlsz.com.cn/WebApi/Public/picture/'+this.$store.state.goodsdetail.pic_id, // 分享图标
        //                 success: function() {
        //                     alert('分享成功')// 用户确认分享后执行的回调函数
        //                 },
        //                 cancel: function() {
        //                     alert('分享取消')// 用户取消分享后执行的回调函数
        //                 }
        //             }
        //             let that=this
        //             wx.ready(function() {
        //                 wx.onMenuShareAppMessage(that.sharemsg);
        //                 wx.onMenuShareTimeline(that.sharetimeline);
        //             });
        //         }
        //     }
        // },
        mounted(){
            //获取底部导航高度
            //this.$store.commit('toolheight')
        },
        destroyed(){
        	try{
	    		this.logLeaveTime();
	    	}catch(e){}
        },
        methods:{
            totop() {
                const x = document.body.scrollTop||document.documentElement.scrollTop
                window.scrollTo(0, 0);
            },
            logLeaveTime(){
            	let visitEndTime = new Date().getTime();
            	let data = {
			        token: localStorage.getItem("shorttoken"),
			        visitTime: this.startTime,
			        leaveTime: visitEndTime,
			        visitInterval: Math.round((visitEndTime - this.startTime) / 1000), // 转换成秒
			        pageType: 'detail',
			        goodsId: this.currentGoodsId,
			        visitPath: this.currentPageUrl,
			        deep: 3
		        };
		        pageVisit.logVisitData(data);
            }
        }
    }
    export {goodsdetail as default}
</script>