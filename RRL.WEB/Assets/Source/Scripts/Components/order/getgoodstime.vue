<!-->订单详情页<-->
<template>
    <div class="orderdetailcontent" style='background:#f5f5f5'>
        <!--div style='background:#fff;margin-top:55px;' class='orderusetime'>
            <img src='/Assets/Source/img/时间@3x.png'/>
            <P v-if='this.$store.state.orderdetail.status==2'>使用有效期：<span>截止{{outtime}}</span><span>无需预约</span></p>
            <p v-if='this.$store.state.orderdetail.status==3'>使用日期:<span>{{addtime}}</span></p>
        </div-->
        <div class="ordertimelast" style="">
            <!--img src="/Assets/Source/img/时间@3x.png">
            <p>
                剩余收货时间：
                <span>
                    6天3小时
                </span>
            </p-->
        </div>
        <div class="logistics" v-if="this.$store.state.orderdetail.status==2" style='background:#fff;'>
            <div class="logistics-left">
                <img src='/Assets/Source/img/物流@3x.png'>
                <div>
                    <p>
                        <span style="margin-right:10px;">{{trackcom}}</span>
                        <span>{{this.$store.state.transinfo[0].context || '暂无物流信息'}}</span>
                    </p>
                    <p>
                        <span style="margin-right:10px;">{{time[0]}}</span>
                        <span>{{time[1]}}</span>
                    </p>
                </div>
            </div>
            <a>
                <img src="/Assets/Source/img/展开@3x.png" style="width:10px;">
            </a>
        </div>
        <div class="buyaddress" style='background:#fff;'>
            <img src='/Assets/Source/img/定位@3x.png'>
            <div>
                <p>
                    <span>收货人：{{this.$store.state.orderdetail.receive_name}}</span>
                    <span>{{this.$store.state.orderdetail.receive_mobile}}</span>
                </p>
                <p style="white-space:normal">收货地址：{{this.$store.state.orderdetail.receive_address}}</p>
            </div>
        </div>
        <div class="detailshow" style='background:#fff;'>
            <div class="shopsname">
                <div>
                    <img src='/Assets/Source/img/商家@3x.png'>
                    <p>{{this.$store.state.orderdetail.shop_name}}</p>
                    <img src='/Assets/Source/img/火热@3x.png' style="width:45px;">
                </div>
                <img src="/Assets/Source/img/展开@3x.png" style="width:10px;">
            </div>
            <div class="ubuygoods" v-for="item in this.$store.state.orderdetail.goods_list">
                <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg">
                <div>
                    <p class="name">{{item.name}}</p>
                    <p class="num">
                        <span>净含量:{{item.specification}}</span>
                        <span>x{{item.goods_count}}</span>
                    </p>
                    <p class="btn">
                        <span>￥{{item.price}}</span>
                        <input type="button" value="退换" v-on:click='alert($store.state.orderdetail.id)' v-if="$store.state.orderdetail.status==2" />
                    </p>
                </div>
            </div>
            <div class="order-goods-info" style='background:#fff;'>
                <p>
                    <span>共计：{{this.$store.state.orderdetail.goods_count}}件商品</span>
                    <span>实付款：￥{{this.$store.state.orderdetail.money}}</span>
                </p>
                <p>
                    奖励金币：{{this.$store.state.orderdetail.return_money}}
                </p>
            </div>
        </div>
        <!--div v-if='this.$store.state.orderdetail.status == 2||this.$store.state.orderdetail.status == 3' class='ordercode'>
            订单验证码:{{this.$store.state.orderdetail.check_code}}
            <p>
              (线下到店消费类商品，请凭验证码到店使用)<span v-if='this.$store.state.orderdetail.status == 3'>已使用</span></p>
            
        </div-->
        <div class="order-goods-btn" style='background:#fff;'>
            <input type="button" value="取消订单" v-on:click="cancelorder()" v-if="this.$store.state.orderdetail.status==1" />
            <input type="button" value="联系客服" v-on:click="kefu" v-if="this.$store.state.orderdetail.status==4" />
            <input type="button" value="再来一单" v-on:click="buyagain" v-if='this.$store.state.orderdetail.status==2||this.$store.state.orderdetail.status==3' />
            <input type="button" value="付款" v-if="this.$store.state.orderdetail.status==1" v-on:click="topay($store.state.orderdetail.id)" />
            <input type="button" value="确认收货" v-if="this.$store.state.orderdetail.status==2" v-on:click="suregetorders()" />
        </div>
        
        <div class="shadow1" v-if="showalert==true">
                <div class="tiplogout">
                    <p v-if="this.$store.state.orderdetail.status==1">确认取消订单?</p>
                    <p v-if="this.$store.state.orderdetail.status==2">确认收货？</p>
                    <div>
                        <div v-on:click='opera(false)'>取消</div>
                        <div v-on:click='opera(true,$store.state.orderdetail.id,$store.state.orderdetail.status)'>确定</div>
                    </div>
                </div>
        </div>
        
    </div>
</template>

<script>
    //
    //import ordershowgoods from './ordershowgood';
    //import adress from './address'
    let orderdetail={
        data(){
            return {
                trackcom:'',
                time:[],
                context:'',
                showalert:false,
                addtime:'',
                outtime:''
            }
        },
        created(){
            // this.$store.dispatch('setorderdetail')
        },
        // components:{
        //     ordershowgoods,
        //     adress
        // },
        computed:{
            //获取订单详情
            getdetail(){
                return this.$store.state.orderdetail
            },
            //获取物流信息
            gettransinfo(){
                return this.$store.state.transinfo
            },
            //获取物流字典
            gettrackcom(){
                 return this.$store.state.trackcom
            }
        },
        watch:{
            getdetail(){
                if(this.$store.state.orderdetail.track_com.length!=0){
                    //获取订单详情后再获取物流信息
                    this.$store.dispatch('settransinfo')
                    this.context=this.$store.state.transinfo[0].context || '暂无物流信息'
                }else{
                    this.context='暂无物流信息'
                }
                this.addtime=this.$store.state.orderdetail.addtime.replace(/\//g,'-')
                this.outtime=this.$store.state.orderdetail.addtime.split(' ')
                this.outtime=this.outtime[0].replace(/\//g,'-')
            },
            gettransinfo(){
                //获取物流信息后获取物流字典
                this.$store.dispatch('settrackcom')
            },
            gettrackcom(){
                //讲物流信息匹配字典
                for(var i=0;i<this.$store.state.trackcom.length;i++){
                    if(this.$store.state.com==this.$store.state.trackcom[i].code){
                        this.trackcom=this.$store.state.trackcom[i].name
                        break
                    }
                }
                try{
                    this.time=this.$store.state.transinfo[0].time.split('T')
                } catch (e){
                }
            }
        },
        methods:{
            //确认收货操作提示
            suregetorders(){
                // console.log(id)
                // this.$store.dispatch('suregetgoods',id)
                // location.reload()
                this.showalert=true
            },
            //付款操作
            topay(id){
                //获取订单预览信息
                this.$store.dispatch('setPreOrderList',id)
                //设置订单id
                this.$store.commit('setorderid',id)
                //跳转创建订单页
                this.$router.push('/makesureorder/:shopcart')
            },
            //取消订单操作提示
            cancelorder(){
                this.showalert=true
            },
            //退换货弹出框对应操作
            alert(id){
                confirm('是否确认退换', () => {
                    this.$store.dispatch('RefundOrder',id)
                    alert('已申请退换货')
                    this.$router.push('/order/4')
                })
            },
            //再来一单操作
            buyagain(){
                this.$router.push('/index')
            },
            //弹出框确认操作（取消订单及确认收货）
            opera(e,id,state){
                if(e==false){
                    this.showalert=false
                }else{
                    if(state==1){
                        this.$store.dispatch('cancelorder',id)
                        this.$router.go(-1)
                    }else if(state==2){
                        this.$store.dispatch('suregetgoods',id)
                        // this.$router.go(-1)
                    }

                }
            },
            //客服弹出框
            kefu(){
                alert('详情请联系客服：400-990-7881')
            }
        }
    }
    export default orderdetail
</script>