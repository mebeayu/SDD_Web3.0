<!-->商品收藏页<-->
<template>
    <div>
        <viewtitle></viewtitle>
        <ul class="sort-goods-ul" style="padding-top:1.5rem;" v-finger:swipe="swipe">
            <router-link class="sort-goodscell"
                    v-for="item in this.$store.state.goodscolist"
                    :to="{name:'goodsdetail',params: { goodsid: item.goods_id,username:'null' }}" tag="li">
                <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" />
                <div class="SG-right">
                    <div class="SG-right-top">
                        <!--<div>
                            <img src="img/火热@3x.png" />
                            <img src="img/新品@3x.png" />
                        </div>-->
                        <p class="SG-goods-name">{{item.goods_name}}</p>
                        <p class="SG-goods-intro">{{item.propaganda}}</p>
                    </div>
                    <div class="SG-right-bottom">
                        <p>
                            <span class="SG-goods-price">￥{{item.price}}</span>
                            <!--span class="SG-goods-origin">原价￥{{item.local_price}}</span-->
                        </p>
                        <!--p class="SG-goods-return">返￥{{item.price*item.return_money_rate}}</p-->
                    </div>
                </div>
            </router-link>
        </ul>
    </div>
</template>

<script>
    import viewtitle from './title';
    let goodscollection={
        components:{
            viewtitle
        },
        created(){
            //获取商品收藏列表
            this.$store.state.goodscolist=[];
            this.$store.dispatch('getgoodscolist')
        },
        methods:{
            back(){
                this.$router.back(-1)
            },
            //页面上划加载更多
            swipe(e){
                if(e.direction=='Up'){
                    //设置分页
                    this.$store.commit('setgoodscopage')
                    this.$store.dispatch('getgoodscolist')
                }
            }
        }
    }

    export default goodscollection
</script>