<!-->首页热门商品<-->
<template>
<div class="index-hotgoods">
        <div class="hot-goods-title">
            热门推荐
            <img src="/Assets/Source/img/热门@3x.png" />
        </div>
        <ul class="hot-goods-list">
            <li v-for='item in this.$store.state.hotgoodslist'>
                <router-link :to="{name:'goodsdetail',params: { goodsid: item.id,username:'null' }}">
                <img v-bind:src='$store.state.imgs+item.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                <div class="hotgoods-info">
                    <p class="hot-title">{{item.name}}</p>
                    <p class="hot-intro">{{item.propaganda}}</p>
                    <p>
                        <span class="price">
                            ￥{{item.price}}
                        </span>
                        <span class="origin-price">
                            原价￥{{item.local_price}}
                        </span>
                    </p>
                    <p class="return-price">
                        奖励￥{{item.return_money_rate * item.price}}
                    </p>
                </div>
                <input class="index-shoppingBTN" value="抢购中" />
                </router-link>
            </li>
        </ul>
    </div>
</template>
<script>
    let hotgoods={

    }
    export {hotgoods as default}
</script>