<!--精选商品列表cell-->
<template>
    <ul class="sort-goods-ul">
        <router-link class="sort-goodscell"
                     v-for="item in goodslist"
                     :to="{name:'goodsdetail',params: { goodsid: item.id ,username:'null' }}" tag="li">
            <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg"/>
            <div class="SG-right">
                <div class="SG-right-top">
                    <!--<div>
                        <img src="img/火热@3x.png" />
                        <img src="img/新品@3x.png" />
                    </div>-->
                    <p class="SG-goods-name">{{item.name}}</p>
                    <p class="SG-goods-intro">{{item.propaganda}}</p>
                </div>
                <div class="SG-right-bottom">
                    <p v-if='item.discount == 0'>
                        <span class="SG-goods-price">￥{{item.price}}</span>
                        <span class="SG-goods-origin">原价￥{{item.local_price}}</span>
                    </p>
                    <p v-else>
                        <span class="SG-goods-price">兑换价：{{item.price}} + {{item.discount}}金币</span>
                    </p>
                    <p class="SG-goods-return" v-if='item.discount == 0'>
                        奖励￥{{(item.price*item.return_money_rate).toFixed(2)}}</p>
                    <p class="SG-goods-return" v-else>常规价：￥{{item.local_price}}</p>
                </div>
                <img class="put-in-cart" src="/Assets/Source/img/购物车2@3x.png" v-on:click.stop="addgoods(item,$event)"/>
            </div>
        </router-link>
        <alertbox></alertbox>
    </ul>
</template>

<script>
    //引入弹出框
    import alertbox from '../alertbox/alertbox'

    let sortgoods = {
        props: {
            goodslist: {
                type: Array
            }
        },
        components: {
            alertbox
        },
        methods: {
            //添加商品至购物车
            addgoods(item, $event) {
                this.$store.commit('setgoodsid', $event);
                // 添加商品至购物车
                this.$store.dispatch('addgoods', {num: 1, id: item.id});
                this.$store.commit('changealertshow', true);
            }
        }
    }

    export {sortgoods as default}
</script>