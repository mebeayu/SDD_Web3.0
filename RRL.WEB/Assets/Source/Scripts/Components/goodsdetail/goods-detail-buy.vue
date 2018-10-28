<!-->商品详情下属底部操作条 belong to 商品详情<-->
<template>
    <div class="goods-detail-buytool" id="goodsbottom">
        <div>
            <div>
                <img src="/Assets/Source/img/收藏商家@3x.png" v-on:click="toindex()" />
                <p>首页</p>
            </div>
            <div v-if="goodsdetail.goodsfav==false||typeof(goodsdetail.goodsfav) == 'undefined'">
                <img src="/Assets/Source/img/收藏@3x.png" v-on:click="addfavgoods" />
                <p>收藏</p>
            </div>
            <div v-else>
                <img src="/Assets/Source/img/collection.png" v-on:click="removefavgoods" />
                <p style="color:#ba3027">已收藏</p>
            </div>
            <input type="button" value="加入购物车" v-on:click="addgoodsinshop" v-if='goodsdetail.discount == 0'
            style='background:#fff;color:#000;border-left:1px solid #d4d4d4'/>
            <input type="button" value="立即购买" v-on:click="buy" v-if='goodsdetail.discount == 0'/>
            <input type="button" value="立即兑换" v-on:click="buy" v-else/>
        </div>
    </div>
</template>
<script>
  import $ from 'jquery';

    let goodsbuy={
        props:
            ['goodsdetail']
        ,
        methods:{
            //添加商品至购物车
            addgoodsinshop(){
                if(this.$store.state.shorttoken.length === 0){
                    confirm('尚未登录,是否登录？',() => {
                        this.$router.push({path:'/login'})
                    })
                }else{
                    if(this.goodsdetail.inv_count > 0) {
                        this.$store.commit('changealertshow',true)
                        this.$store.dispatch('addgoods',{num:1, id:this.$store.state.goodsid})
                    }else {
                        alert('商品库存不足')
                    }
                    
                }
            },
            //从商品页创建订单
            buy(){
            	var allParam = window.UI.Page.getAllparam();
                var reference=allParam.reference;
                if(this.$store.state.shorttoken.length === 0){
                    confirm('尚未登录,是否登录？',() => {
                        this.$router.push({path:'/login',query:{redirect: ("/#/goodsdetail/" + this.$store.state.goodsid + "/null")}});
                    })
                }else{
                    if(this.goodsdetail.discount==0){
                        if(this.goodsdetail.inv_count > 0) {
                            let goods=[this.goodsdetail]
                            this.$store.commit('setcartitem',goods)
                            this.$store.dispatch('makeorderfromgoods').then(() => {
                                this.$router.push('/makesureorder/:goodsdetail/'+this.$store.state.orderid);
                            })
                        } else {
                            alert('商品库存不足')
                        }
                        
                    }else {
                        if (this.goodsdetail.discount > this.goodsdetail.plate_to_return_money){
                            alert('金币不足，无法抵扣')
                        }else {
                            if(this.goodsdetail.inv_count > 0) {
                                let goods=[this.goodsdetail]
                                this.$store.commit('setcartitem',goods)
                                this.$store.dispatch('makeorderfromgoods').then(() => {
                                  this.$router.push('/makesureorder/:goodsdetail/'+this.$store.state.orderid);
                                })
                            } else {
                                alert('商品库存不足')
                            }
                        }
                    }
                    
                    //this.$router.push('/makesureorder/:goodsdetail')
                }
            },
            //添加商品收藏
            addfavgoods(){
                //console.log(typeof(this.$store.state.longtoken)=='undefined')
                if(this.$store.state.shorttoken.length === 0){
                    confirm('尚未登录,是否登录？',() => {
                        this.$router.push({path:'/login'})
                    })
                }else{
                    this.goodsdetail.goodsfav=true
                    this.$store.dispatch('addgoodsfav')
                }

            },
            //移除商品收藏
            removefavgoods(){
                this.goodsdetail.goodsfav=false
                this.$store.dispatch('removegoodsfav')
            },
            toindex(){
                this.$router.push('/index')
            }
        }
    }

    export default goodsbuy
</script>