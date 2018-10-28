<!-->购物车下属猜你喜欢商品列表cell  belong to 购物车<-->
<template>
    <div class="goods-list-content">
        <router-link :to="{name:'goodsdetail',params: { goodsid: item.id,username:'null' }}"
                     class="goods-cell"
                     v-for="item in this.$store.state.mainrecommend"
                     tag="div">
            <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg"/>
            <p class="goods-name">
                {{item.name}}
            </p>
            <div>
                <span class="price">￥{{item.price}}</span>
                <img class="addBTN" src="/Assets/Source/img/加入购物车@3x.png" v-on:click.stop="addsopcart(item.id)"/>
            </div>
        </router-link>
    </div>
</template>

<script>
  import $ from "jquery";

  let goodscell = {
    methods: {
      //添加商品至购物车
      addsopcart(id) {
        //添加商品至购物车
        if (this.$store.state.shorttoken.length === 0) {
          confirm('尚未登录,是否登录？', () => {
            this.$router.push({path: '/login'});
          })
        } else {
          this.$store.commit('changealertshow', true)
          this.$store.dispatch('addgoods', {num: 1, id: id});
          location.reload();
        }
      }
    }
  }
  export {goodscell as default}
</script>