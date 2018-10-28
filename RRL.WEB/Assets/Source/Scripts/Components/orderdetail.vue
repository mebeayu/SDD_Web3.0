<!-->订单详情页<-->
<template>
    <div>
        <viewtitle></viewtitle>
        <div class="orderdetailcontent" style='background:#f5f5f5;padding-top:1.5rem;'>
            <!--div style='background:#fff;margin-top:55px;' class='orderusetime'>
                            <img src='/Assets/Source/img/时间@3x.png'/>
                            <P v-if='this.$store.state.orderdetail.status==2'>使用有效期：<span>截止{{outtime}}</span><span>无需预约</span></p>
                            <p v-if='this.$store.state.orderdetail.status==3'>使用日期:<span>{{addtime}}</span></p>
                        </div-->
            <div class="ordertimelast" style="">
                <img src="/Assets/Source/img/时间@3x.png">
                <p>
                    订单创建时间:
                    <span>
                                    {{this.$store.state.orderdetail.addtime | timeformat}}
                                </span>
                </p>
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
                <div v-for="item in this.$store.state.orderdetail.goods_list">
                    <div class="ubuygoods" v-on:click='togoods(item.goods_id)'>
                        <img v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix" :onerror="$store.state.defaultimg">
                        <div>
                            <p class="name">{{item.name}}</p>
                            <p class="num">
                                <span>净含量:{{item.specification}}</span>
                                <span>x{{item.goods_count}}</span>
                            </p>
                            <p class="btn">
                                <span>￥{{item.price}}</span>
                                <span>
                                    <input type="button" :class="[item.is_can_resell===false?'key-no-resell':'']"
                                           value="一键转卖" @click.stop="ifKeyResell(item)"/>
                                    <!--<input type="button" value="一键转卖" v-on:click.stop='tojuju()'
                                           style='color:#ba3027;border-color:#ba3027'
                                           v-if="$store.state.orderdetail.status==3"/>-->
                                    <input type="button" value="退换" v-on:click.stop='alert($store.state.orderdetail.id)'
                                           v-if="$store.state.orderdetail.status==2"/>
                                </span>
                            </p>
                        </div>
                    </div>

                    <!--新增物流信息部分-->
                    <p class="courier-number" v-if="$store.state.orderdetail.track_num.length!=0">快递单号：{{$store.state.orderdetail.track_num}}</p>
                    <div class="logistics" v-if="$store.state.orderdetail.status==2" style='background:#fff;'
                         v-on:click='totransdetail'>
                         
                        <div class="logistics-left">
                            <img src='/Assets/Source/img/物流@3x.png'>
                            <div>
                                <p>
                                    <span style="margin-right:10px;">{{trackcom}}</span>
                                    <span style='white-space:normal'>{{context}}</span>
                                </p>
                                <p v-if='context !== "暂无物流信息"'>
                                    <span style="margin-right:10px;">{{time[0]}}</span>
                                    <span>{{time[1]}}</span>
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="order-goods-info" style='background:#fff;'>
                    <p>
                        <span>共计：{{this.$store.state.orderdetail.goods_count}}件商品</span>
                        <span>实付款：{{this.$store.state.orderdetail.real_pay_text}}</span>
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
                <input type="button" value="取消订单" v-on:click="cancelorder()"
                       v-if="this.$store.state.orderdetail.status==1"/>
                <input type="button" value="联系客服" v-on:click="kefu" v-if="this.$store.state.orderdetail.status==4"/>
                <input type="button" value="再来一单" v-on:click="buyagain"
                       v-if='this.$store.state.orderdetail.status==2||this.$store.state.orderdetail.status==3'/>
                <input type="button" value="付款" v-if="this.$store.state.orderdetail.status==1"
                       v-on:click="topay($store.state.orderdetail.id)"/>
                <input type="button" value="确认收货" v-if="this.$store.state.orderdetail.status==2"
                       v-on:click="suregetorders()"/>
                <input type="button" value="转卖详情" v-if="$store.state.orderdetail.is_can_resell"
                       @click="zhuanmai()"/>
                <!--<input type="button" value="转卖详情" v-if="this.$store.state.orderdetail.status==3"
                       v-on:click="zhuanmai()"/>-->
            </div>
            <div class="shadow1" v-if="showalert==true">
                <div class="tiplogout">
                    <p v-if="this.$store.state.orderdetail.status==1">确认取消订单?</p>
                    <p v-if="this.$store.state.orderdetail.status==2">确认收货？</p>
                    <div>
                        <div v-on:click='opera(false)'>取消</div>
                        <div v-on:click='opera(true,$store.state.orderdetail.id,$store.state.orderdetail.status)'>确定
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--orderlistcell></orderlistcell-->

        <!--消息提醒框开始-->
        <div v-if="alertFlag">
            <div class="msg-dialog"></div>
            <div class="msg-remind-box">
                <img :src="'/Assets/Source/img/'+ alertImg"/>
                <p>{{ alertTil }}</p>
            </div>
        </div>
        <!--消息提醒框结束-->
    </div>
</template>

<style scoped>
    .courier-number{
        margin-top: .4rem;
        margin-left: .4rem;
        font-size: .35rem;
        color: #333;
    }
    /*弹出框*/
   
    .msg-dialog {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        width: 100%;
        height: 100%;
        background: transparent;
    }

    .msg-remind-box {
        position: fixed;
        background: rgba(0, 0, 0, .7);
        color: #eee;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        border-radius: .1rem;
        text-align: center;
        font-size: .4rem;
        padding: .5rem .8rem .8rem;
        box-shadow: 0.1rem 0.1rem 0.2rem rgba(0, 0, 0, .2);
    }

    .msg-remind-box img {
        width: .8rem;
        height: .8rem;
    }

    .msg-remind-box p {
        padding-top: .2rem;
    }
    .key-no-resell {
        border-color: #dcdcdc!important;
        color: #c9c5c5;
        background-color: #fff;
    }
</style>

<script>
    //引入返回 订单详情cell
    import viewtitle from './title';
    import orderlistcell from './order/getgoodstime';
    import $ from 'jquery';

    //import bus from "../bus.js";

    let orderdetail = {
        components: {
            viewtitle,
            orderlistcell
        },
        data() {
            return {
                trackcom: '',
                time: [],
                context: '',
                showalert: false,
                addtime: '',
                outtime: '',
                transMsg: {},
                alertFlag: false,
                alertImg: null,
                alertTil: null,
                alertImgData: ["status_loading.gif", "sign-check-icon.png", "sign-error-icon.png"],
                alertTilData: ["转卖提交数据中 ...", "一键转卖成功~", "一键转卖出现异常!"]
            }
        },
        created() {
            //获取订单id及订单详情
            let id = this.$route.params.orderid;
            this.$store.dispatch('setorderdetail', id);
        },
        computed: {
            //获取订单详情
            getdetail() {
                localStorage.setItem("transdetailMsg", JSON.stringify(this.$store.state.orderdetail));
                return this.$store.state.orderdetail;
            },
            //获取物流信息
            gettransinfo() {
                localStorage.setItem("transdetailMsg", JSON.stringify(this.$store.state.transinfo));
                return this.$store.state.transinfo
            },
            //获取物流字典
            gettrackcom() {
                return this.$store.state.trackcom
            }
        },
        watch: {
            getdetail() {
                const that = this;
                if (this.$store.state.orderdetail.track_com.length != 0) {
                    //获取订单详情后再获取物流信息
                    this.$store.dispatch('settransinfo').then(() => {
                        try {
                            that.context = that.$store.state.transinfo[0].context || '暂无物流信息';
                            that.transMsg = that.$store.state.transinfo;
                            localStorage.setItem("transdetailMsg", JSON.stringify(that.$store.state.transinfo));
                        } catch (e) {
                        }
                    })
                } else {
                    this.context = '暂无物流信息'
                }
                this.addtime = this.$store.state.orderdetail.addtime.replace(/\//g, '-')
                this.outtime = this.$store.state.orderdetail.addtime.split(' ')
                this.outtime = this.outtime[0].replace(/\//g, '-')
            },
            gettransinfo() {
                //获取物流信息后获取物流字典
                this.$store.dispatch('settrackcom')
            },
            gettrackcom() {
                //讲物流信息匹配字典
                for (var i = 0; i < this.$store.state.trackcom.length; i++) {
                    if (this.$store.state.com == this.$store.state.trackcom[i].code) {
                        this.trackcom = this.$store.state.trackcom[i].name
                        break
                    }
                }
                try {
                    this.time = this.$store.state.transinfo[0].time.split('T')
                } catch (e) {
                }
            }
        },
        methods: {
            //确认收货操作提示
            suregetorders() {
                // console.log(id)
                // this.$store.dispatch('suregetgoods',id)
                // location.reload()
                this.showalert = true
            },
            //付款操作
            topay(id) {
                //获取订单预览信息
                this.$store.dispatch('setPreOrderList', id)
                //设置订单id
                this.$store.commit('setorderid', id)
                //跳转创建订单页
                this.$router.push('/makesureorder/:shopcart/' + id);
            },
            //取消订单操作提示
            cancelorder() {
                this.showalert = true
            },
            //退换货弹出框对应操作
            alert(id) {
                confirm('是否确认退换', () => {
                    this.$store.dispatch('RefundOrder', id)
                    alert('已申请退换货')
                    this.$router.push('/order/4')
                })
            },
            //再来一单操作
            buyagain() {
                this.$router.push('/index')
            },
            //弹出框确认操作（取消订单及确认收货）
            opera(e, id, state) {
                if (e == false) {
                    this.showalert = false
                } else {
                    if (state == 1) {
                        this.$store.dispatch('cancelorder', id)
                        this.$router.go(-1)
                    } else if (state == 2) {
                        this.$store.dispatch('suregetgoods', id)
                        location.reload()
                    }
                }
            },
            //客服弹出框
            kefu() {
                alert('详情请联系客服：400-990-7881')
            },
            togoods(goods_id) {
                this.$router.push('/goodsdetail/' + goods_id + '/null')
            },
            totransdetail() {
                // 数据传递给快递详情
                // localStorage.setItem("transdetailMsg", JSON.stringify(this.transMsg));
                let that = this;
                let track_com
                    = JSON.parse(localStorage.getItem("transdetailMsg")).track_com
                ;
                let track_num
                    = JSON.parse(localStorage.getItem("transdetailMsg")).track_num
                ;
                $.ajax({
                    url: '/WebApi/Public/TrackInfo?TrackCom=' + track_com + '&TrackNum=' + track_num,
                    //url:'WebApi/Public/TrackInfo?TrackCom=shunfeng&TrackNum=236560068552',
                    type: "get",
                    dataType: "json",
                    success: function (data) {
                        localStorage.setItem("transdetailInfo", JSON.stringify(data));
                        that.$router.push('/transdetail');
                    }
                });
            },
            getResellAPI(goodsId, orderId) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/OneKeyReSell?goods_id=' + goodsId + '&order_id=' + orderId + '&token=' + encodeURIComponent(this.$store.state.shorttoken),
                    type: 'get',
                    complete: (xhr, textStatus) => {
                        let resJson = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            _this.alertImg = _this.alertImgData[0];
                            _this.alertTil = _this.alertTilData[0];
                            _this.alertFlag = true;
                            alert(resJson.message);
                            return;
                        }
                        // 请先确认订单!
                        if (xhr.status === 200 && resJson.status === 99) {
                            alert(resJson.message);
                            return;
                        }
                        // 成功
                        _this.alertImg = _this.alertImgData[0];
                        _this.alertTil = _this.alertTilData[0];
                        _this.alertFlag = true;
                        if (xhr.status === 200 && resJson.status === 0) {
                            window.location.href = xhr.responseJSON.data;
                        }
                    }
                })
            },
            // 点击一键转卖是否发布
            ifKeyResell(item) {
                if (item.is_can_resell === false) return;
                let goods_id = null,order_id = null;
                if(!item.goods_id){
                  goods_id = goods_list[0].goods_id;
                  order_id = goods_list[0].order_id;
                }else{
                  goods_id = item.goods_id;
                  order_id = item.order_id;
                }
                confirm("是否确定一键转卖？", () => {
                    this.getResellAPI(goods_id, order_id);
                });
            },
            tojuju() {
                $.ajax({
                    url: '/WebApi/TradeManager/OneKeyReSell?goods_id=' + this.$store.state.orderdetail.goods_list[0].goods_id + '&order_id=' + this.$route.params.orderid + '&token=' + encodeURIComponent(this.$store.state.shorttoken),
                    type: 'get',
                    success: function (res) {
                        console.log(res);
                        if (res.status == 0) {
                            window.location.href = res.data
                        } else {
                            alert(res.message);
                        }
                    }
                })
            },
            zhuanmai() {
                $.ajax({
                    url: '/WebApi/TradeManager/ReSellDetails?token=' + this.$store.state.shorttoken,
                    type: 'get',
                    success: function (res) {
                        if (res.status == 0) {
                            window.location.href = res.data
                        } else {
                            alert(res.message)
                        }
                    }
                })
            }
        },
        filters: {
            timeformat(time) {
                try {
                    return time.replace(/\//g, '-')
                } catch (e) {
                }
            },
            transstatus(sta) {
                switch (sta) {
                    case 200 :
                        return "带发货"
                        break;
                    case 300 :
                        return "已发货"
                        break;
                    case 400 :
                        return "已签收"
                        break;
                }
            }
        }
    }
    export default orderdetail
</script>