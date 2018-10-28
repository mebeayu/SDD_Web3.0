<!-->订单列表页<-->
<template>
    <div style="background:#f5f5f5">
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back"/>
            <p>{{this.$store.state.viewtitle}}</p>
        </div>
        <div class="my-order-nav-box" id="first-item" style="background:#fff">
            <ul class="my-order-nav">
                <li v-bind:class="{'at-list':this.status==0}" v-on:click="changestatus('0')">全部</li>
                <li v-bind:class="{'at-list':this.status==1}" v-on:click="changestatus('1')">待付款</li>
                <li v-bind:class="{'at-list':this.status==2}" v-on:click="changestatus('2')">待收货</li>
                <li v-bind:class="{'at-list':this.status==3}" v-on:click="changestatus('3')">待评价</li>
                <li v-bind:class="{'at-list':this.status==4}" v-on:click="changestatus('4')">退换/售后</li>
            </ul>
        </div>
        <div v-finger:swipe="swipe">
            <div class="order-listcell" v-for="item in list.all"
                 v-if="status==0" style="margin-bottom:10px;">
                <div class="OL-header">
                    <div class="OL-header-left">
                        <img src="/Assets/Source/img/商家@3x.png"/>
                        <p>{{item.shop_name}}</p>
                        <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                    </div>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='1'">
                        转卖中
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='99'">
                        转卖完成
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==-3">
                        订单退款
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==-2">
                        订单异常
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==-1">
                        订单取消
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==1">
                        订单待付
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==2">
                        待确认
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==3">
                        订单待评价
                    </span>
                    <span class="OL-header-right" v-if="item.sh_sell_status=='0'&&item.status==4">
                        退换货
                    </span>
                </div>
                <div class="trans-info">
                    <img src="/Assets/Source/img/购物车@3x.png"/>
                    <div>
                        <p>创建时间：{{item.addtime | timeformat}}</p>
                    </div>
                </div>
                <p class="courier-number" v-if="item.track_num.length!==0">快递单号：{{ item.track_num }}</p>
                <div class="order-goods-pic">
                    <ul>
                        <li v-for="gl in item.goods_list">
                            <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix'
                                 :onerror="$store.state.defaultimg"/>
                        </li>
                        <li>
                            <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多
                            </router-link>
                        </li>
                    </ul>
                </div>
                <div class="order-goods-info">
                    <p>
                        <span>共计：{{ item.goods_count}} 件商品</span>
                        <span>实付款：{{ item.real_pay_text }}</span>
                    </p>
                    <!-- <p>
                        奖励金币：{{item.return_money}}
                    </p> -->
                </div>
                <div class="order-goods-btn">
                    <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1"/>
                    <input type="button" value="再来一单" v-on:click="buyagain(item)" v-else/>
                    <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id,2)"/>
                    <input type="button" :class="[item.is_can_resell===false?'key-no-resell':'']" value="一键转卖"
                           @click="ifKeyResell(item)"/>
                    <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                </div>
            </div>
            <div class="order-listcell" v-for="item in list.notpay" v-if="status==1">
                <div class="OL-header">
                    <div class="OL-header-left">
                        <img src="/Assets/Source/img/商家@3x.png"/>
                        <p>{{item.shop_name}}</p>
                        <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                    </div>
                    <span class="OL-header-right" v-if="item.status==1">
                        订单待付
                    </span>
                </div>
                <div class="trans-info">
                    <img src="/Assets/Source/img/购物车@3x.png"/>
                    <div>
                        <p>创建时间：{{item.addtime | timeformat}}</p>

                    </div>
                </div>
                <div class="order-goods-pic">
                    <ul>
                        <li v-for="gl in item.goods_list">
                            <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix'
                                 :onerror="$store.state.defaultimg"/>
                        </li>
                        <li>
                            <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多
                            </router-link>
                        </li>
                    </ul>
                </div>
                <div class="order-goods-info">
                    <p>
                        <span>共计：{{item.goods_count}}件商品</span>
                        <span>实付款：{{item.real_pay_text}}</span>
                    </p>
                    <!-- <p>
                        奖励金币：{{item.return_money}}
                    </p> -->
                </div>
                <div class="order-goods-btn">
                    <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1"/>
                    <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                </div>
            </div>
            <div class="order-listcell" v-for="item in list.notrecived" v-if="status==2">
                <div class="OL-header">
                    <div class="OL-header-left">
                        <img src="/Assets/Source/img/商家@3x.png"/>
                        <p>{{item.shop_name}}</p>
                        <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                    </div>
                    <!--v-if="(item.sh_sell_status==='0'&&item.status===2)||(item.sh_sell_status==='1'&&item.status===2)"-->
                    <span class="OL-header-right" v-if="item.status==2">
                        待确认
                    </span>
                </div>
                <div class="trans-info">
                    <img src="/Assets/Source/img/购物车@3x.png"/>
                    <div>
                        <p>创建时间：{{item.addtime | timeformat}}</p>
                    </div>
                </div>
                <div class="order-goods-pic">
                    <ul>
                        <li v-for="gl in item.goods_list">
                            <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix'
                                 :onerror="$store.state.defaultimg"/>
                        </li>
                        <li>
                            <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多
                            </router-link>
                        </li>
                    </ul>
                </div>
                <div class="order-goods-info">
                    <p>
                        <span>共计：{{item.goods_count}}件商品</span>
                        <span>实付款：{{item.real_pay_text}}</span>
                    </p>
                    <p>
                        奖励金币：{{item.return_money}}
                    </p>
                </div>
                <div class="order-goods-btn">
                    <input type="button" value="再来一单" v-on:click="buyagain(item)"/>
                    <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id,2)"/>
                    <input type="button" :class="[item.is_can_resell===false?'key-no-resell':'']" value="一键转卖"
                           @click="ifKeyResell(item)"/>
                    <!--<input class="key-no-resell" type="button" value="一键转卖"/>-->
                    <img @click="showTilResell" v-if="item.status==2" class="wj-tips-img"
                         src="/Assets/Source/img/提示@3x.png" alt="">
                </div>

            </div>
            <div class="order-listcell" v-for="item in list.notcommend" v-if="status==3">
                <div class="OL-header">
                    <div class="OL-header-left">
                        <img src="/Assets/Source/img/商家@3x.png"/>
                        <p>{{item.shop_name}}</p>
                        <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                    </div>
                    <span class="OL-header-right" v-if="item.status==3">
                        订单待评价
                    </span>
                </div>
                <div class="trans-info">
                    <img src="/Assets/Source/img/购物车@3x.png"/>
                    <div>
                        <p>创建时间：{{item.addtime | timeformat}}</p>

                    </div>
                </div>
                <div class="order-goods-pic">
                    <ul>
                        <li v-for="gl in item.goods_list">
                            <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix'
                                 :onerror="$store.state.defaultimg"/>
                        </li>
                        <li>
                            <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多并一键转卖
                            </router-link>
                        </li>
                    </ul>
                </div>
                <div class="order-goods-info">
                    <p>
                        <span>共计：{{item.goods_count}}件商品</span>
                        <span>实付款：{{item.real_pay_text}}</span>
                    </p>
                    <p>
                        奖励金币：{{item.return_money}}
                    </p>
                </div>
                <div class="order-goods-btn">
                    <input type="button" value="再来一单" v-on:click="buyagain(item)"/>
                    <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id)"/>
                    <input type="button" :class="[item.is_can_resell===false?'key-no-resell':'']" value="一键转卖"
                           @click="ifKeyResell(item)"/>
                    <img @click="showTilResell" class="wj-tips-img" src="/Assets/Source/img/提示@3x.png" alt="">
                    <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                </div>
            </div>
            <div class="order-listcell" v-for="item in list.return" v-if="status==4">
                <div class="OL-header">
                    <div class="OL-header-left">
                        <img src="/Assets/Source/img/商家@3x.png"/>
                        <p>{{item.shop_name}}</p>
                        <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                    </div>
                    <span class="OL-header-right" v-if="item.status==4">
                        退换货
                    </span>
                </div>
                <div class="trans-info">
                    <img src="/Assets/Source/img/购物车@3x.png"/>
                    <div>
                        <p>创建时间：{{item.addtime | timeformat}}</p>

                    </div>
                </div>
                <div class="order-goods-pic">
                    <ul>
                        <li v-for="gl in item.goods_list">
                            <img v-bind:src='$store.state.imgs+gl.pic_id+$store.state.img_suffix'
                                 :onerror="$store.state.defaultimg"/>
                        </li>
                        <li>
                            <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多
                            </router-link>
                        </li>
                    </ul>
                </div>
                <div class="order-goods-info">
                    <p>
                        <span>共计：{{item.goods_count}}件商品</span>
                        <span>实付款：{{item.real_pay_text}}</span>
                    </p>
                    <p>
                        奖励金币：{{item.return_money}}
                    </p>
                </div>
                <div class="order-goods-btn">
                    <input type="button" value="联系客服" v-on:click="kefu"/>
                    <input type="button" value="确认收货" v-if="item.status==2" v-on:click="suregetgoods(item.id)"/>
                    <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                </div>
            </div>
            <div class="shadow1" v-if="showalert==true">
                <div class="tiplogout">
                    <p v-if='alertstatus==1'>确认取消订单?</p>
                    <p v-if='alertstatus==2'>确认收货？</p>
                    <div>
                        <div v-on:click='opera(false)'>取消</div>
                        <div v-on:click='opera(true,alertstatus)'>确定</div>
                    </div>
                </div>
            </div>
        </div>

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

    .courier-number {
        color: #2d2d2d;
        font-size: .37333333333333335rem;
        padding: .4rem;
        border-bottom: 1px solid rgba(0, 0, 0, .08);
    }

    .wj-tips-img {
        width: .5333333333333333rem;
        height: .5333333333333333rem;
        position: relative;
        top: .14rem;
    }

    .order-goods-btn input:nth-child(1) {
        margin-right: 0;
    }

    .key-no-resell {
        border-color: #dcdcdc;
        color: #c9c5c5;
        background-color: #fff;
    }
</style>

<script>
    //引入 返回 订单列表导航 订单列表
    import viewtitle from './title';
    import orderlistcell from './order/order-listcell';
    import $ from 'jquery';

    let order = {
        data() {
            return {
                status: "0",
                page: [1, 1, 1, 1, 1],
                list: {
                    all: [],
                    notpay: [],
                    notrecived: [],
                    notcommend: [],
                    return: []
                },
                showalert: false,
                id: '',
                alertstatus: 1,
                alertFlag: false,
                alertImg: null,
                alertTil: null,
                alertImgData: ["status_loading.gif", "sign-check-icon.png", "sign-error-icon.png"],
                alertTilData: ["转卖提交数据中 ...", "一键转卖成功~", "一键转卖出现异常!"]
            }
        },
        components: {
            viewtitle,
            orderlistcell
        },
        created() {
            this.status = this.$route.params.status.toString();
            console.log(this.status)
            if (GLOBE_SHORT_TOKEN) {
                this.$store.commit('SET_SHORTTOKEN', GLOBE_SHORT_TOKEN)
                this.getorderlist()
            } else {
                if (this.$store.state.shorttoken.length == 0 || this.$store.state.shorttoken == undefined) {
                    if (localStorage.getItem('longtoken')) {
                        const longtoken = localStorage.getItem('longtoken')
                        this.$store.dispatch('getshorttoken', longtoken).then(() => {
                            this.getorderlist()
                        })
                    } else {
                        this.$router.push('/login')
                    }
                }
            }
            this.getorderlist();
        },
        // watch: {
        //     gettoken() {
        //         return this.$store.state.shorttoken;
        //     }
        // },
        // computed:{
        //     gettoken() {
        //         this.getorderlist();
        //     }
        // },
        methods: {
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
                let goods_list = item.goods_list[0];
                confirm("是否确定一键转卖？", () => {
                    this.getResellAPI(goods_list.goods_id, goods_list.order_id);
                });
            },
            // 显示弹框
            showTilResell() {
                confirm("<ul><li>1、付款之后，可以一键转卖，付款时间为0-9点时，当天上午九点之前可以一键转卖；付款时间为9-24点时，次日上午九点之前可以一键转卖。</li><li>2、确认收货之后，可以点击一键转卖。</li><li>3、目前暂免收手续费和折旧费。</li></ul>", () => {
                });
            },
            //返回页面
            back() {
                this.$router.go(-1)
            },
            getorderlist: function () {
                const that = this
                if (this.status == 0) {
                    $.ajax({
                        url: 'WebApi/TradeManager/OrderList',
                        data: {
                            page: this.page[0],
                            pagesize: 5,
                            token: this.$store.state.shorttoken
                        },
                        type: 'get',
                        success: function (res) {
                            if (res.status == 0) {
                                that.setlist(res.data)
                            } else {
                                //context.commit('seterror', res.message)
                                // that.$router.push('/login')
                            }
                        }
                    })
                } else {
                    $.ajax({
                        url: 'WebApi/TradeManager/OrderListByStatus',
                        data: {
                            status: this.status,
                            page: this.page[this.status],
                            pagesize: 5,
                            token: this.$store.state.shorttoken
                        },
                        type: 'get',
                        success: function (res) {
                            if (res.status == 0) {
                                that.setlist(res.data)
                            } else {
                                // context.commit('seterror', res.message)
                                that.$router.push('/login')
                            }
                        }
                    })
                }
            },
            setlist(data) {
                // console.log(1234, this.status)
                switch (this.status) {
                    case "0": {
                        this.list.all = this.list.all.concat(data);
                        console.log(this.list)
                        let hash = {};
                        let arr = this.list.all
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, []);
                        this.list.all = arr;
                        console.log(this.list.all, this.status)
                    }
                        break;
                    case "1": {
                        this.list.notpay = this.list.notpay.concat(data)
                        let hash = {};
                        let arr = this.list.notpay
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, [])
                        this.list.notpay = arr
                    }
                        break;
                    case "2": {
                        this.list.notrecived = this.$store.state.orderlist.notrecived = this.list.notrecived.concat(data)
                        let hash = {};
                        let arr = this.list.notrecived
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, [])
                        this.list.notrecived = arr
                    }
                        break;
                    case "3": {
                        this.list.notcommend = this.list.notcommend.concat(data)
                        let hash = {};
                        let arr = this.list.notcommend
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, [])
                        this.list.notcommend = arr
                    }
                        break;
                    case "4": {
                        this.list.return = this.list.return.concat(data)
                        let hash = {};
                        let arr = this.list.return
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, [])
                        this.list.return = arr
                    }
                        break;
                }
            },
            changestatus(e) {
                this.status = e
                switch (e) {
                    case "0": {
                        this.getorderlist()
                    }
                        break;
                    case "1": {
                        this.getorderlist()

                    }
                        break;
                    case "2": {
                        this.getorderlist()

                    }
                        break;
                    case "3": {
                        this.getorderlist()
                    }
                        break;
                    case "4": {
                        this.getorderlist()
                    }
                        break;
                }
            },
            //上划分页
            swipe(e) {
                if (e.direction == 'Up') {
                    this.page[this.status]++;
                    console.log(this.page)
                    this.getorderlist()
                }
            },
            //确认收货弹出框
            suregetgoods(id) {
                // console.log(id)
                // this.$store.dispatch('suregetgoods',id)
                // location.reload()
                this.showalert = true
                this.id = id
                this.alertstatus = 2;
            },
            //再来一单
            buyagain(item) {
                let goods_id = item.goods_list[0].goods_id;
                this.$router.push('/goodsdetail/'+ goods_id + '/null');
            },
            //付款操作
            topay(id) {
                this.$store.dispatch('setPreOrderList', id);
                this.$store.commit('setorderid', id);
                this.$router.push('/makesureorder/:shopcart/' + id);
            },
            //取消和确认订单操作
            opera(e, from) {
                if (e == false) {
                    this.showalert = false
                } else {
                    if (from == 1) {
                        this.showalert = false;
                        this.$store.dispatch('cancelorder', this.id);
                        location.reload()
                    } else if (from == 2) {
                        this.$store.dispatch('suregetgoods', this.id).then(() => {
                            this.showalert = false;
                            this.list = {
                                all: [],
                                notpay: [],
                                notrecived: [],
                                notcommend: [],
                                return: []
                            }
                            this.getorderlist();
                        })
                        location.reload()
                    }
                }
            },
            //取消订单弹出框
            cancelorder(id) {
                this.showalert = true
                this.id = id
                this.alertstatus = 1;

            },
            //联系客服弹出框
            kefu() {
                // alert('详情请联系客服：400-990-7881')
                let iframe = encodeURIComponent("https://kefu.easemob.com/webim/im.html?configId=8d149d8f-bc70-45f8-9c44-96c6ef6098c9");
                window.location.href = "/Game/CustomerService?iframe=" + iframe + "&redirect=" + encodeURIComponent("/#/order/0");
            },

        },

        filters: {
            timeformat(time) {
                return time.replace(/\//g, '-')
            }
        }
    }

    export {order as default}
</script>