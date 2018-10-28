<!-->首页特价精选<-->
<template>
<div class="index-sale">
        <p class="index-sale-title">
            必买推荐
            <img src="/Assets/Source/img/特价@3x.png" />
        </p>
        <div class="sale-goods-box">
            <ul class="sale-goods">
                <li v-for="item in this.$store.state.mainrecommend">
                    <router-link :to="{name:'goodsdetail',params: { goodsid: item.id,username:'null'}}">
                        <!--img src="/Assets/Source/img/默认图像.png" v-if="item.pic_id==0||item.pic_id==undefined"/-->
                        <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" />
                        <p class="sale-goods-name">{{item.name}}</p>
                        <p class="sale-goods-intro">{{item.propaganda}}</p>
                        <p class="sale-goods-price">￥{{item.price}}</p>
                        <p class="sale-goods-return">奖励￥{{item.price*item.return_money_rate}}</p>
                    </router-link>
                </li>
                <router-link to="/goodssort">
                查看更多
                </router-link>
            </ul>
        </div>
    </div>
</template>
<script>
    let indexSale={

    }
    export  {indexSale as default}
</script>