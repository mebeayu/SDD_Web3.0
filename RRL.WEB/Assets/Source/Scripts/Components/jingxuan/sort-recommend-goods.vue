<!-->精选页主推商品<-->
<template>
    <router-link class="sort-goodscell"
                :to="{name:'goodsdetail',params: { goodsid: this.$store.state.sorthotgoods.id,username:'null' }}"
                tag="li">
        <img v-bind:src="$store.state.imgs+$store.state.sorthotgoods.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" class="recommend-img" />
        <div class="SG-right">
            <div class="SG-right-top">
                <div>
                    <img src="/Assets/Source/img/火热@3x.png" />
                    <img src="/Assets/Source/img/新品@3x.png" />
                </div>
                <p class="SG-goods-name">{{$store.state.sorthotgoods.name}}</p>
                <p class="SG-goods-intro">{{$store.state.sorthotgoods.propaganda}}</p>
            </div>
            <div class="SG-right-bottom">
                <p>
                    <span class="SG-goods-price">￥{{$store.state.sorthotgoods.price}}</span>
                    <span class="SG-goods-origin">原价￥{{$store.state.sorthotgoods.local_price}}</span>
                </p>
                <p class="SG-goods-return">奖励￥{{$store.state.sorthotgoods.price*$store.state.sorthotgoods.return_money_rate}}</p>
            </div>
            <img class="put-in-cart" src="/Assets/Source/img/购物车2@3x.png" v-on:click.stop="addgoods($store.state.sorthotgoods.id)" />
        </div>
    </router-link>
</template>
<script>
    import alertbox from '../alertbox/alertbox'
    let recommendgoods={
        methods:{
            //添加商品至购物车
            addgoods(e){
                this.$store.commit('setgoodsid',e)
                this.$store.dispatch('addgoods',1)
                this.$store.commit('changealertshow',true)
            }
        }
    }

    export {recommendgoods as default}
</script>