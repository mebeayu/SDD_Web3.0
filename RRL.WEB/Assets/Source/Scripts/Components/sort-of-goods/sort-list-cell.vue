<template>
    <ul class='sortlist-cell'>
        <li v-for='item in this.$store.state.listofgoods' v-on:click='todetail(item.id)'
            v-if='$store.state.listofgoods.length != 0'>
            <img v-bind:src='$store.state.imgs+item.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg"
                 :load="$store.state.defaultimg"/>
            <p>{{item.name}}</p>
            <p class="wj-new-price" v-if='item.discount == 0'>价格：￥{{item.price}}(￥{{ item.cash_price }} +{{
                item.beans_price | formatBeansPrice }}金豆)</p>
            <p v-else>
                ￥{{item.price }} + {{item.discount }}金币
            </p>
            <!-- <p v-if='item.discount == 0'>奖励金币：{{(item.return_money_rate * item.price).toFixed(2)}}</p>
            <p v-else>
                官方指导价:￥ {{item.local_price }}
            </p> -->
            <img class='tag'/>
        </li>
        <li v-if='this.$store.state.listofgoods.length == 0'>暂无商品</li>
    </ul>
</template>

<style scoped>
    .wj-new-price {
        font-size: .32rem;
    }
</style>

<script>
  let sortlistcell = {
    methods: {
      todetail(id) {
        this.$router.push('/goodsdetail/' + id + '/null')
      }
    }
  }

  export default sortlistcell
</script>