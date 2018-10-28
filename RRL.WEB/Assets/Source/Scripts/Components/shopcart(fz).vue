<!-->购物车页<-->
<template>
  <div style="background:#f5f5f5">
    <p class="shopcate-header" id="carttop">
      购物车
      <!--span v-on:click='edit(1)' class="editbtn" v-if="this.$store.state.editstatus==0">编辑</span>
                    <span v-on:click='edit(0)' class="editbtn" v-else>完成</span-->
    </p>
    <ul class="shoppping-cart" v-bind:style="{marginTop:this.$store.state.topHeight+'px'}" v-if="this.$store.state.shopcart.shopcartlist.length != 0">
      <li class="shop-name">
        <div class="shop-name-left">
          <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" />
          <img src="/Assets/Source/img/商家LOGO@3x .png" class="shoplogo" />
          <span>省兜兜商城</span>
        </div>
        <input type="button" class="shop-name-right" value="编辑" v-on:click='changeedit(true)' v-if='isedit == false' />
        <input type="button" class="shop-name-right" value="完成" v-on:click='changeedit(false)' v-else />
      </li>
      <li v-for="(item,index) in this.$store.state.shopcart.shopcartlist" class="cart-goodscell">
        <!--input v-bind:id="'check'+index" type="checkbox" style="display:none;" v-model="item.toggle" />
                      <label v-bind:for="'check'+index" v-if='$store.state.editstatus==0' v-on:click.stop="test(item.id,index,item)">
                              <img src="/Assets/Source/img/选择@3x.png" class="choseBTN" v-if="item.toggle==true" />
                              <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" v-else />
                          </label-->
        <img src="/Assets/Source/img/选择@3x.png" class="choseBTN" v-show="item.toggle==true" v-on:click.stop='choseornot(item.id,true,item)' />
        <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" v-show="item.toggle==false" v-on:click.stop='choseornot(item.id,false,item)' />
        <router-link tag='img' v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg" class="cart-goods-img" :to="{name:'goodsdetail',params: { goodsid: item.goods_id,username:'null' }}"></router-link>
        <router-link tag='div' class="cart-goods-info" v-if="!isedit" :to="{name:'goodsdetail',params: { goodsid: item.goods_id ,username:'null'}}">
          <p class="first-line">
            <span>{{item.goods_name}}</span>
            <span>x{{item.goods_count}}</span>
          </p>
          <p class="second-line">
            {{item.propaganda}}
          </p>
          <p class="third-line">
            ￥{{item.price}}
          </p>
        </router-link>
        <div class="cart-edit" v-else>
          <img src="/Assets/Source/img/减少@3x.png" v-on:click="changenum(-1,item,index)">
          <input v-model="item.goods_count" />
          <img src="/Assets/Source/img/增加@3x.png" v-on:click="changenum(1,item,index)">
          <div v-on:click="delgoods(item.id)" class="button">
            <p>删除</p>
          </div>
        </div>
      </li>
    </ul>
    <emptycart v-bind:style="{marginTop:this.$store.state.topHeight+'px'}" v-if="this.$store.state.shopcart.shopcartlist.length==0"></emptycart>
    <ul class="guess-u-like" id ='last-item' v-bind:style="{marginBottom:47+this.$store.state.bottomHeight+'px'}">
      <li>猜你喜欢</li>
      <goodscell></goodscell>
    </ul>
    <div id="buy-tool" v-bind:style="{bottom:this.$store.state.bottomHeight+'px'}" v-if='choseditem.length != 0'>
      <div class="buy-tool">
        <div class="buy-tool-left">
          <img src="/Assets/Source/img/未选择@3x.png" />
          <p>已选（{{choseditem.length}}）</p>
        </div>
        <div class="buy-tool-right">
          <p class="price">￥{{settlecount}}</p>
          <input type="button" value="下单" v-on:click="tomakeorder" />
        </div>
      </div>
    </div>
    <bottom></bottom>
    <alertbox></alertbox>
  </div>
</template>
<script>
  // import shopcartcell from './shopcart/cart-goodscell';
  import goodscell from './shopcart/goods-cell';
  // import buytool from './shopcart/buy-tool';
  import bottom from './bottom';
  import emptycart from './shopcart/empty-cart';
  import alertbox from './alertbox/alertbox'
  let shopcart = {
    data() {
      return {
        haschose: false,
        isedit: false,
        choseditem: [],
        settlecount: 0
      }
    },
    components: {
      // shopcartcell,
      goodscell,
      // buytool,
      bottom,
      emptycart,
      alertbox,
    },
    created() {
      //获取购物车列表
      this.$store.dispatch('setshopcartlist')
    },
    mounted() {
      //获取头高底高设置猜你喜欢
      this.$store.commit('topheight')
      this.$store.commit('bottomheight')
      this.$store.dispatch('setmaingoods')
    },
    methods: {
      //编辑购物车内商品数量
      changenum(status, item, index) {
        this.$store.commit('setgoodsid', item.id)
        if (status > 0) {
          this.$store.state.shopcart.shopcartlist[index].goods_count += status
          this.$store.dispatch('addgoods', {num:1, id:item.goods_id})
          if (item.toggle == true) {
            this.settlecount = parseFloat(this.settlecount) + parseFloat(status * this.$store.state.shopcart.shopcartlist[index].price);
            this.settlecount = this.settlecount.toFixed(2)
          }
        } else {
          if (this.$store.state.shopcart.shopcartlist[index].goods_count > 1) {
            this.$store.state.shopcart.shopcartlist[index].goods_count += status
            this.$store.dispatch('addgoods', {num:-1, id:item.goods_id})
            if (item.toggle == true) {
              this.settlecount = parseFloat(this.settlecount) + parseFloat(status * this.$store.state.shopcart.shopcartlist[index].price);
              this.settlecount = this.settlecount.toFixed(2)
            }
          }
        }
        // this.$store.state.shopcart.shopcartlist[index].goods_count += status
        // if (this.$store.state.shopcart.shopcartlist[index].goods_count < 1) {
        //   this.$store.state.shopcart.shopcartlist[index].goods_count = 1
        // } else {
        //   this.$store.dispatch('addgoods', status)
        // };
        // if( item.toggle == true ) {
        //   this.settlecount = this.settlecount + status *
        // }
      },
      choseornot(id, status, item) {
        if (status == false) {
          this.choseditem.push(item)
          item.toggle = true
          this.settlecount = parseFloat(this.settlecount) + parseFloat(item.goods_count * item.price);
          this.settlecount = this.settlecount.toFixed(2);
        } else {
          for (let i = 0, len = this.choseditem.length; i < len; i++) {
            if( this.choseditem[i]['id'] == id ) {
              this.choseditem = this.choseditem.splice(i, 1);
              break;
            }
          }
          this.settlecount = parseFloat(this.settlecount) - parseFloat(item.goods_count * item.price);
          this.settlecount = this.settlecount.toFixed(2);
          item.toggle = false
        }
      },
      //删除购物车内商品
      delgoods(e) {
        // let newlist = this.$store.state.shopcart.shopcartlist;
        // for(let i =0, len = newlist.length; i< len; i++) {
        //   if( newlist[i]['id'] == e ){
        //     this.$store.state.shopcart.shopcartlist = this.$store.state.shopcart.shopcartlist.splice(e, 1)
        //     // this.$store.commit('SETSHOPCARTLIST', newlist)
        //     break;
        //   }
        // }
        // console.log(his.$store.state.shopcart.shopcartlist)
        this.$store.dispatch('delgoods', e)
        location.reload()
      },
      changeedit(sta) {
        this.isedit = sta;
      },
      //从购物创建订单
      tomakeorder() {
        //console.log(this.$store.state.cartlist)
        let list = []
        for (let i = 0, len = this.choseditem.length; i < len; i++) {
          list.push(this.choseditem[i].id)
        }
        this.$store.dispatch('makeorderfromcart', list).then(() => {
            this.$router.push('/makesureorder/:shopcart')
        })
      }
    },
  }
  export {
    shopcart as
    default
  }
</script>