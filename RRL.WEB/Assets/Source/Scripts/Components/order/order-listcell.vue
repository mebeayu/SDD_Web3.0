<!-->订单列表<-->
<template>
<div v-finger:swipe="swipe">
    <div class="order-listcell" v-for="item in this.$store.state.orderlist.all"
            v-if="$store.state.orderstatus==0" style="margin-bottom:10px;">
        <div class="OL-header">
            <div class="OL-header-left">
                <img src="/Assets/Source/img/商家@3x.png" />
                <p>{{item.shop_name}}</p>
                <img src="/Assets/Source/img/展开@3x.png" class="open" />
            </div>
            <span class="OL-header-right" v-if="item.status==2">
                待确认
            </span>
        </div>
        <div class="trans-info">
            <img src="/Assets/Source/img/购物车@3x.png" />
            <div>
                <p>创建时间：{{item.addtime | timeformat}}</p>
            </div>
        </div>
        <div class="order-goods-pic">
            <ul>
                <li v-for="gl in item.goods_list">
                    <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                </li>
                <li>
                    <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多</router-link>
                </li>
            </ul>
        </div>
        <div class="order-goods-info">
            <p>
                <span>共计：{{item.goods_count}}件商品</span>
                <span>实付款：￥{{item.money}}</span>
            </p>
            <p>
                奖励金币：{{item.return_money}}
            </p>
        </div>
        <div class="order-goods-btn">
            <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1" />
            <input type="button" value="再来一单" v-on:click="buyagain" v-else />
            <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id,2)" />
            <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)" />
        </div>
    </div>
    <div class="order-listcell" v-for="item in this.$store.state.orderlist.notpay" v-if="$store.state.orderstatus==1">
        <div class="OL-header">
            <div class="OL-header-left">
                <img src="/Assets/Source/img/商家@3x.png" />
                <p>{{item.shop_name}}</p>
                <img src="/Assets/Source/img/展开@3x.png" class="open" />
            </div>
            <span class="OL-header-right" v-if="item.status==2">
                待确认
            </span>
        </div>
        <div class="trans-info">
            <img src="/Assets/Source/img/购物车@3x.png" />
            <div>
                <p>创建时间：{{item.addtime | timeformat}}</p>
                
            </div>
        </div>
        <div class="order-goods-pic">
            <ul>
                <li v-for="gl in item.goods_list">
                    <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                </li>
                <li>
                    <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多</router-link>
                </li>
            </ul>
        </div>
        <div class="order-goods-info">
            <p>
                <span>共计：{{item.goods_count}}件商品</span>
                <span>实付款：￥{{item.money}}</span>
            </p>
            <p>
                奖励金币：{{item.return_money}}
            </p>
        </div>
        <div class="order-goods-btn">
            <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1" />
            <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)" />
        </div>
    </div>
    <div class="order-listcell" v-for="item in this.$store.state.orderlist.notrecived" v-if="$store.state.orderstatus==2">
        <div class="OL-header">
            <div class="OL-header-left">
                <img src="/Assets/Source/img/商家@3x.png" />
                <p>{{item.shop_name}}</p>
                <img src="/Assets/Source/img/展开@3x.png" class="open" />
            </div>
            <span class="OL-header-right" v-if="item.status==2">
                待确认
            </span>
        </div>
        <div class="trans-info">
            <img src="/Assets/Source/img/购物车@3x.png" />
            <div>
                <p>创建时间：{{item.addtime | timeformat}}</p>
                
            </div>
        </div>
        <div class="order-goods-pic">
            <ul>
                <li v-for="gl in item.goods_list">
                    <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                </li>
                <li>
                    <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多</router-link>
                </li>
            </ul>
        </div>
        <div class="order-goods-info">
            <p>
                <span>共计：{{item.goods_count}}件商品</span>
                <span>实付款：￥{{item.money}}</span>
            </p>
            <p>
                奖励金币：{{item.return_money}}
            </p>
        </div>
        <div class="order-goods-btn">
            <input type="button" value="再来一单" v-on:click="buyagain" />
            <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id,2)" />
        </div>

    </div>
    <div class="order-listcell" v-for="item in this.$store.state.orderlist.notcommend" v-if="$store.state.orderstatus==3">
        <div class="OL-header">
            <div class="OL-header-left">
                <img src="/Assets/Source/img/商家@3x.png" />
                <p>{{item.shop_name}}</p>
                <img src="/Assets/Source/img/展开@3x.png" class="open" />
            </div>
            <span class="OL-header-right" v-if="item.status==2">
                待确认
            </span>
        </div>
        <div class="trans-info">
            <img src="/Assets/Source/img/购物车@3x.png" />
            <div>
                <p>创建时间：{{item.addtime | timeformat}}</p>
                
            </div>
        </div>
        <div class="order-goods-pic">
            <ul>
                <li v-for="gl in item.goods_list">
                    <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                </li>
                <li>
                    <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多</router-link>
                </li>
            </ul>
        </div>
        <div class="order-goods-info">
            <p>
                <span>共计：{{item.goods_count}}件商品</span>
                <span>实付款：￥{{item.money}}</span>
            </p>
            <p>
                奖励金币：{{item.return_money}}
            </p>
        </div>
        <div class="order-goods-btn">
            <input type="button" value="再来一单" v-on:click="buyagain" />
            <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id)" />
            <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)" />
        </div>
    </div>
    <div class="order-listcell" v-for="item in this.$store.state.orderlist.return" v-if="$store.state.orderstatus==4">
        <div class="OL-header">
            <div class="OL-header-left">
                <img src="/Assets/Source/img/商家@3x.png" />
                <p>{{item.shop_name}}</p>
                <img src="/Assets/Source/img/展开@3x.png" class="open" />
            </div>
            <span class="OL-header-right" v-if="item.status==2">
                待确认
            </span>
        </div>
        <div class="trans-info">
            <img src="/Assets/Source/img/购物车@3x.png" />
            <div>
                <p>创建时间：{{item.addtime | timeformat}}</p>
                
            </div>
        </div>
        <div class="order-goods-pic">
            <ul>
                <li v-for="gl in item.goods_list">
                    <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                </li>
                <li>
                    <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多</router-link>
                </li>
            </ul>
        </div>
        <div class="order-goods-info">
            <p>
                <span>共计：{{item.goods_count}}件商品</span>
                <span>实付款：￥{{item.money}}</span>
            </p>
            <p>
               奖励金币：{{item.return_money}}
            </p>
        </div>
        <div class="order-goods-btn">
            <input type="button" value="联系客服" v-on:click="kefu" />
            <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id)" />
            <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)" />
        </div>
    </div>
            <div class="shadow1" v-if="showalert==true">
                <div class="tiplogout">
                    <p v-if='status==1'>确认取消订单?</p>
                    <p v-if='status==2'>确认收货？</p>
                    <div>
                        <div v-on:click='opera(false)'>取消</div>
                        <div v-on:click='opera(true,status)'>确定</div>
                    </div>
                </div>
        </div>
</div>
</template>
<script>
    let orderlistcell={
        data(){
            return{
                showalert:false,
                id:'',
                status:1
            }
         },
        methods:{
            //上划分页
            swipe(e){
                if(e.direction=='Up'){
                    this.$store.commit('changeorderlistpage')
                    this.$store.dispatch('getorderlist')
                }
            },
            //确认收货弹出框
            suregetgoods(id){
                // console.log(id)
                // this.$store.dispatch('suregetgoods',id)
                // location.reload()
                this.showalert=true
                this.id=id
                this.status = 2;
            },
            //再来一单
            buyagain(){
                this.$router.push('/index')
            },
            //付款操作
            topay(id){
                this.$store.dispatch('setPreOrderList',id)
                this.$store.commit('setorderid',id)
                this.$router.push('/makesureorder/:shopcart')
            },
            //取消和确认订单操作
            opera(e,from){
                if(e==false){
                    this.showalert=false
                }else{
                    if(from==1){
                        this.$store.dispatch('cancelorder',this.id)
                        ///location.reload()
                    }else if(from==2){
                        this.$store.dispatch('suregetgoods',this.id).then(() => {
                            this.showalert=false;
                            this.$store.commit('setorderlist',[]);
                            this.$store.dispatch('getorderlist');
                        })
                        ///location.reload()
                    }
                }
            },
            //取消订单弹出框
            cancelorder(id){
                this.showalert=true
                this.id=id
                this.status = 1;

            },
            //联系客服弹出框
            kefu(){
                alert('详情请联系客服：400-990-7881')
            }
        },
        filters:{
            timeformat(time) {
                return time.replace(/\//g,'-')
            }
        }
    }
    export {orderlistcell as default }
</script>