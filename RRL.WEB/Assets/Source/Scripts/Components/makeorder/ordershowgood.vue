<!-->订单创建页的订单详情cell<-->
<template>
    <div class="detailshow">
            <!--div class="shopsname">
                <div>
                    <img src='/Assets/Source/img/商家@3x.png'>
                    <p>省兜兜官方店</p>
                    <img src='/Assets/Source/img/火热@3x.png' style="width:45px;">
                </div>
                <img src="/Assets/Source/img/展开@3x.png" style="width:10px;">
            </div-->
            <div v-for="hhh in this.$store.state.PreOrderList">
                <div class="ubuygoods" v-for="item in hhh.goods_list">
                    <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg">
                    <div>
                        <p class="name">{{item.goods_name||item.name}}</p>
                        <p class="num">
                            <span>{{item.specification}}</span>
                            <span>x{{item.goods_count||1}}</span>
                        </p>
                        <!--p class="btn">
                            <span>￥{{item.price}}</span>
                            <input type="button" value="退换" v-if="hash!='#/makesureorder'" />
                        </p-->
                    </div>
                </div>
            </div>
            <div class="order-goods-info">
            <p>
                <span>共计：{{num}}件商品</span>
                <span>实付款：￥{{totalmoney}}</span>
            </p>
            <p v-if="hash=='#/order'">
                奖励金币：{{return_money}}
            </p>
            <!--p v-if = '$store.state.PreOrderList[0].money <= 88'>运费：{{$store.state.PreOrderList[0].postage}}元</p>
            <p v-else> 运费：满88包邮</p-->
            <p>合计：{{totalmoney}}元</p>
        </div>
        </div>
</template>

<script>
    let ordershowgoods={
        data(){
            return {
                hash:'',
                goodsnum:0,
                price:0,
                postage:0,
                num:0,
                totalmoney:0,
                return_money:0,
                
            }
        },
        created(){
            //重置订单总价,商品数量，价格，邮费等
            this.$store.commit('settotalcash',0)
            this.goodsnum=0;
            this.price=0;
            this.postage=0;
            this.num=0;
            this.$store.dispatch('setPreOrderList').then(() => {
                this.$store.commit('settotalcash',0)
                this.goodsnum=0;
                this.num=0
                for(var i=0;i<this.$store.state.PreOrderList.length;i++){
                    this.num+=this.$store.state.PreOrderList[i].goods_count;
                    this.totalmoney+=this.$store.state.PreOrderList[i].money;
                    this.return_money += this.$store.state.PreOrderList[i].return_money;
                }
                this.num = this.num.toFixed(0);
                this.totalmoney = parseFloat(this.totalmoney).toFixed(2);
                this.return_money = this.return_money.toFixed(2);
                this.$store.commit('settotalcash',this.totalmoney)
            })
            //计算商品总价数量邮费
            // for(var i=0;i<this.$store.state.PreOrderList.length;i++){
            //         this.num+=this.$store.state.PreOrderList[i].goods_count;
            //     }
            
            // if(this.$store.state.PreOrderList[0]['money'] >= 88) {
            //     this.totalmoney = this.$store.state.PreOrderList[0]['money']
            // } else {
            //    this.totalmoney = (this.$store.state.PreOrderList[0]['money'] + this.$store.state.PreOrderList[0]['postage']).toFixed(2)
            // }
            //console.log('444'+this.$store.state.PreOrderList)
            // this.hash=location.hash
            // console.log(this.$store.state.cartitem)
            // for(var i=0;i<this.$store.state.cartitem.length;i++){
            //     this.goodsnum+=this.$store.state.cartitem[i].goods_count||1
            //     if(this.$store.state.cartitem[i].goods_count){
            //         this.price+=this.$store.state.cartitem[i].goods_count * this.$store.state.cartitem[i].price
            //     }else{
            //         this.price+=this.$store.state.cartitem[i].price
            //     }

            //     this.postage+=this.$store.state.cartitem[i].postage
            // }
            // this.$store.commit('settotalcash',this.price+this.postage)
        },
        computed: {
            //获取订单预览信息
            getPreOrderList() {
                return this.$store.state.PreOrderList;
            }
        },
        watch: {
             //计算商品总价数量邮费
            getPreOrderList() {
                
            }
        },
        //清空订单预览信息
        beforeDestroy(){
            this.$store.commit('setPreOrderList',{})
        }
    }

    export default ordershowgoods
</script>