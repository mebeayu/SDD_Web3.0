<!--购物车底部下单按钮 belong to 购物车-->
<template>
<div id="buy-tool">
  <div class="buy-tool">
    <div class="buy-tool-left">
      <img src="/Assets/Source/img/未选择@3x.png" />
      {{ this.$store.state }}
      <p>已选（{{this.$store.state.cartlist.length}}）</p>
    </div>
    <div class="buy-tool-right">
      <p class="price">￥{{this.$store.state.cartmoney}}</p>
        <input type="button" value="下单" v-on:click="tomakeorder" />
    </div>
  </div>
</div>
</template>

<script>
  let buytool={
    methods:{
      //从购物创建订单
      tomakeorder(){
        //console.log(this.$store.state.cartlist)
        this.$store.dispatch('makeorderfromcart').then(() => {
          alert(123)
          this.$router.push('/makesureorder/:shopcart')
        })

      }
    }
  }

  export {buytool as default}
</script>