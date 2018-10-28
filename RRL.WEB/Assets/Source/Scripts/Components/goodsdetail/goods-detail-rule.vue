<!-->商品详情页具体显示<-->
<template>
    <div class="goods-detail-rule">
        <div class="goods-detail-name">
            <div class="GD-name-left">
                <p class="GD-name">
                    {{this.$store.state.goodsdetail.goodsdetail.name}}
                </p>
                <p class="GD-intro">
                    {{this.$store.state.goodsdetail.goodsdetail.propaganda}}
                </p>
                <div class="GD-price" v-if='this.$store.state.goodsdetail.goodsdetail.discount == 0'>
                    <span>￥{{this.$store.state.goodsdetail.goodsdetail.price}}</span>
                    <span>(￥{{ this.$store.state.goodsdetail.goodsdetail.cash_price }} +{{ this.$store.state.goodsdetail.goodsdetail.beans_price }}金豆)</span>
                    <p class="wj-local-price">原价￥{{this.$store.state.goodsdetail.goodsdetail.local_price}}</p>
                </div>
                <p class="GD-price" v-else>
                    兑换价：
                    <span>￥{{this.$store.state.goodsdetail.goodsdetail.price}} + {{this.$store.state.goodsdetail.goodsdetail.discount}}金币</span>
                </p>
                <!-- <p class="GD-return" v-if='this.$store.state.goodsdetail.goodsdetail.discount == 0'>
                    奖励金币{{(this.$store.state.goodsdetail.goodsdetail.price*this.$store.state.goodsdetail.goodsdetail.return_money_rate).toFixed(2)}}
                </p>
                <p class="GD-return" v-else>
                    官方指导价：￥{{this.$store.state.goodsdetail.goodsdetail.local_price}}
                </p> -->
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
                    <img src="/Assets/Source/img/分享@3x.png"/>
                    <p>分享</p>
                </div>
            </div>
        </div>
        <div class="GD-getway">
            <div class="GD-getway-left">
                领取方式:<span v-if="this.$store.state.goodsdetail.goodsdetail.postage==0">快递包邮</span>
                <span v-else>运费：￥{{this.$store.state.goodsdetail.goodsdetail.postage}} {{method_postage_description()}}</span>
            </div>
            <div class="GD-getway-right">
                <p v-if="$store.state.goodsdetail.goodsdetail.sell_count_visible"
                   style="display:inline-block">
                    已售:<span>{{ $store.state.goodsdetail.goodsdetail.sell_count }}</span>
                </p>
                <p v-if="this.$store.state.goodsdetail.goodsdetail.inv_count_visible==true"
                   style="display:inline-block">
                    剩余：<span v-if="this.$store.state.goodsdetail.goodsdetail.inv_count!=-1">{{this.$store.state.goodsdetail.goodsdetail.inv_count}}</span><span v-else>999</span>
                </p>
            </div>
        </div>
        <ul class="GD-rule">
            <li v-if="this.$store.state.goodsdetail.goodsdetail.is_real==true">
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                正品保证
            </li>
            <!-- <li>
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                返{{this.$store.state.goodsdetail.goodsdetail.return_money_rate*100}}%
            </li> -->
            <li v-if="this.$store.state.goodsdetail.goodsdetail.is_quick_refund==true">
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                急速退款
            </li>
            <li>
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                7日可达
            </li>
            <li v-if='this.$store.state.goodsdetail.goodsdetail.is_unuse_refund==true'>
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                未使用可退换
            </li>
            <li>
                <img src="/Assets/Source/img/支持icon@3x.png"/>
                包装损坏可退换
            </li>
            <li v-if="this.$store.state.goodsdetail.goodsdetail.is_can_not_refund==true">
                <img src="/Assets/Source/img/不支持icon@3x .png"/>
                不支持退换
            </li>
            <li v-if="this.$store.state.goodsdetail.goodsdetail.is_can_use_coupons
==true">
                <img style="width:.42rem;height:.42rem;" src="/Assets/Source/img/wj_goodsdetail_use_coupons.png"/>
                优惠券可用
            </li>
        </ul>
    </div>
</template>
<style scoped>
    p.wj-local-price {
        font-size: .28rem;
        color: #666;
        text-decoration: line-through;
    }
    .GD-rule li:last-child{
        margin-right: 0;
    }
</style>
<script>
    let goodsrule = {
        props: ["goodsdetail"],
        methods: {
            //分享弹出框
            shareGoods(){
                this.bus.$emit('showShareDiloag');
            },
             share() {
            	
             	var username=localStorage.getItem('username');
             	if(!username)
             	{
             		username="";
             	} 
            	 
                 const ua = window.navigator.userAgent.toLowerCase();
                 const goodsdetail = this.goodsdetail;
                 if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                     alert('请点击顶部导航栏的右上角分享');
                 } else {
                 	var redirect=('https://e-shop.rrlsz.com.cn/'+ location.hash+'?reference='+username);
		          	if (ua.indexOf('iphone') > -1) {
                         redirect = encodeURIComponent(redirect);
                     }
                     location.href = `/Event/SharingGuide?title=${encodeURIComponent(goodsdetail.name)}&recommend=${encodeURIComponent(goodsdetail.propaganda)}&pic_id=${encodeURIComponent(goodsdetail.pic_id)}&redirect=${redirect}`;
                 }

                 //this.islogin();
                 /*if (this.$store.state.defaultshare != true) {
                   alert('请点击右上角分享')
                 } else if (this.$store.state.mutiple != true) {
                   this.$router.push('/makeshareimg/0')
                 } else {
                   this.$router.push('/sharepage')
                 }*/
             },
            islogin() {
                if (this.$store.state.shorttoken.length == 0) {
                    alert('请注意，您尚未登录，分享将没有推荐奖励')
                }
            },
            method_postage_description()
            {
            	var postage_price=this.$store.state.goodsdetail.goodsdetail.postage_free_total_price;
            	if(postage_price<=1000)
            	{
            		return '（满￥'+this.$store.state.goodsdetail.goodsdetail.postage_free_total_price+'包邮）';
            	}else
            	{
            		return '（不包邮）';
            	}
            	
            }

        },
        created() {
            this.$store.dispatch('jugimg')
        }
    }
    export {goodsrule as default}
</script>