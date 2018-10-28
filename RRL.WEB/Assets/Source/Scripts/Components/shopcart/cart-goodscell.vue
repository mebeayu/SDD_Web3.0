<!-->购物车内商品列表cell<-->
<template>
    <ul class="shoppping-cart">
        <li class="shop-name">
                <div class="shop-name-left">
                    <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" />
                    <img src="/Assets/Source/img/商家LOGO@3x .png" class="shoplogo" />
                    <span>省兜兜商城</span>
                </div>
                <input type="button" class="shop-name-right" value="编辑" v-on:click ='changeedit()'/>
            </li>
        <li v-for="(item,index) in this.$store.state.shopcart.shopcartlist" class="cart-goodscell">
            <!--input v-bind:id="'check'+index" type="checkbox" style="display:none;" v-model="item.toggle" />
            <label v-bind:for="'check'+index" v-if='$store.state.editstatus==0' v-on:click.stop="test(item.id,index,item)">
                    <img src="/Assets/Source/img/选择@3x.png" class="choseBTN" v-if="item.toggle==true" />
                    <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" v-else />
                </label-->
            <img src="/Assets/Source/img/选择@3x.png" class="choseBTN" v-show="item.toggle==true" v-on:click.stop = 'choseornot(item.id,true,item)'/>
            <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" v-show = "item.toggle==false" v-on:click.stop = 'choseornot(item.id,false,item)' />
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
                <img src="/Assets/Source/img/减少@3x.png" v-on:click="changenum(-1,item.goods_id,index)">
                <input v-model="item.goods_count" />
                <img src="/Assets/Source/img/增加@3x.png" v-on:click="changenum(1,item.goods_id,index)">
                <div v-on:click="delgoods(item.id)" class="button">
                    <p>删除</p>
                </div>
            </div>
        </li>
    </ul>
</template>
<script>
    let shopcartcell = {
        data() {
            return {
                haschose: false,
                isedit:false,
                choseditem:[],
            }
        },
        created() {
            //清楚购物车已选商品价格总和
            this.$store.commit('clearcartmonry')
        },
        methods: {
            //编辑购物车内商品数量
            changenum(status, id, index) {
                this.$store.commit('setgoodsid', id)
                this.$store.state.shopcart.shopcartlist[index].goods_count += status
                if (this.$store.state.shopcart.shopcartlist[index].goods_count < 1) {
                    this.$store.state.shopcart.shopcartlist[index].goods_count = 1
                } else {
                    this.$store.dispatch('addgoods', status)
                };
            },
            choseornot(id,status, item) {
                if(status == false) {
                    this.choseditem.push(item)
                    item.toggle = true
                } else {
                    for(let i = 0, len = this.choseditem.length; i < len; i++) {
                        this.choseditem[i]['id'] == id ? this.choseditem.splice(i,1) : '';
                    }
                    item.toggle = false
                }
            },
            //购物车内选中和取消选中商品操作
            // test(id, index, item) {
            //     console.log(12456)
            //     //console.log(item.toggle)
            //     if (item.toggle == false) {
            //         this.$store.commit('addcartlist', id)
            //         var money = this.$store.state.shopcart.shopcartlist[index].price * this.$store.state.shopcart.shopcartlist[index].goods_count
            //     } else {
            //         this.$store.commit('removecartlist', id)
            //         var money = -this.$store.state.shopcart.shopcartlist[index].price * this.$store.state.shopcart.shopcartlist[index].goods_count
            //     }
            //     //item.toggle=!item.toggle
            //     this.$store.commit('changecartmonry', money)
            //     this.haschose = false
            //     try {
            //         for (var i = 0; i < this.$store.state.shopcart.shopcartlist.length; i++) {
            //             if (this.$store.state.shopcart.shopcartlist[i].toggle == true) {
            //                 this.haschose = true
            //                 break
            //             }
            //         }
            //     } catch (e) {
            //     }
            //     this.$emit('chose', this.haschose)

            // },
            //删除购物车内商品
            delgoods(e) {
                this.$store.dispatch('delgoods', e)
                location.reload()
            },
            changeedit() {
                this.isedit = !this.isedit
            }
        }
    }
    export default shopcartcell;
</script>