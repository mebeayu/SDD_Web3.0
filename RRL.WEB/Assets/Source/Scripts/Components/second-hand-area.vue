<!--二手交易页-->
<template>
    <div>
        <div class="back-page-btn">
            <div @click="back">
                <img src="/Assets/Source/img/返回@3x.png"/>
                <!--<img src="/Assets/Source/img/second-hand-area-left.png"/>-->
            </div>
            <p>二手交易我要卖</p>
        </div>

        <!--头部切换导航开始-->
        <!--<div class="second-hand-area-nav">
            <ul class="second-hand-area-list">
                <li :class="{cur:wantBuy}" @click="ifWantBuySold(1,$event)">我要买</li>
                <li :class="{cur:wantSold}" @click="ifWantBuySold(2,$event)">我要卖</li>
            </ul>
        </div>-->
        <!--头部切换导航结束-->

        <!--主要内容展示区开始-->
        <div class="second-hand-area-cont" style="padding-top: 1.1rem;">
            <!--我要卖页面开始-->
            <div class="want-sold">
                <!--=======================-->
                <div class="order-listcell" v-for="item in allOrderList">
                    <div class="OL-header">
                        <div class="OL-header-left">
                            <img src="/Assets/Source/img/商家@3x.png"/>
                            <p>{{item.shop_name}}</p>
                            <img src="/Assets/Source/img/展开@3x.png" class="open"/>
                        </div>
                        <span class="OL-header-right" v-if="item.status==2">
                            待确认
                        </span>
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
                                <router-link :to="{name:'orderdetail',params: { orderid: item.id }}" tag="span">查看更多
                                </router-link>
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
                        <input type="button" value="再来一单" v-on:click="buyagain"/>
                        <input type="button" value="确认收货"
                               v-if="item.status==2"
                               @click="suregetgoods(item.id,2)"/>
                        <input type="button" :class="[item.is_can_resell===false?'key-no-resell':'']" value="一键转卖"
                               @click="ifKeyResell(item)"/>
                        <img @click="showTilResell" v-if="item.status==2" class="wj-tips-img"
                             src="/Assets/Source/img/提示@3x.png" alt="">
                    </div>

                </div>
                <!--=======================-->

                <!--点击加载更多数据开始-->
                <div class="click-more-data">
                    <button @click="moreData" :disabled="buttonFlag">{{ buttonVal }}</button>
                </div>
                <!--点击加载更多数据结束-->
            </div>
            <!--我要卖页面结束-->

        </div>
        <!--主要内容展示区结束-->

        <!--消息提醒框开始-->
        <div v-if="alertFlag">
            <div class="msg-dialog"></div>
            <div class="msg-remind-box">
                <img :src="'/Assets/Source/img/'+ alertImg"/>
                <p>{{ alertTil }}</p>
            </div>
        </div>
        <!--消息提醒框结束-->

        <!--确认是否收货开始-->
        <div class="shadow1" v-if="showalert==true">
            <div class="tiplogout">
                <p v-if='alertstatus==1'>确认取消订单?</p>
                <p v-if='alertstatus==2'>确认收货？</p>
                <div>
                    <div @click='opera(false)'>取消</div>
                    <div @click='opera(true,alertstatus)'>确定</div>
                </div>
            </div>
        </div>
        <!--确认是否收货结束-->

    </div>
</template>

<style scoped>
    /* 我要买的数据加载提示 */
    .wj-iframe-tips {
        padding: .4rem 0;
        font-size: .37333333333333335rem;
        color: #adabab;
        text-align: center;
    }

    /*点击加载更多*/
    .click-more-data {
        padding: .26666666666666666rem;
    }

    .click-more-data button {
        display: block;
        width: 100%;
        height: 100%;
        border: .013333333333333334rem solid #e3e4e8;
        border-radius: .05333333333333334rem;
        padding: .26666666666666666rem 0;
        font-size: .37333333333333335rem;
        color: #adabab;
        background: #efefef;
        outline: none;
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

    .courier-number {
        color: #2d2d2d;
        font-size: 28px;
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

    .want-buy {
        position: absolute;
        left: 0;
        top: 2.2rem;
        bottom: 0;
        width: 100%;
        overflow: hidden;
    }

    .want-buy iframe {
        display: block;
        width: 100%;
        height: 100%;
        overflow: hidden;
    }

    .second-hand-area-list {
        display: flex;
        flex-direction: row;
        justify-content: center;
        padding-top: 1.1066666666666667rem;
    }

    .second-hand-area-list li {
        display: flex;
        flex: 1;
        justify-content: center;
        height: 1.0666666666666667rem;
        line-height: 1.0666666666666667rem;
        font-size: .37333333333333335rem;
        color: #7a7b87;
    }

    .second-hand-area-list li.cur {
        color: #65c4aa;
        border-bottom: .04rem solid #65c4aa;
    }

    .back-page-btn {
        position: fixed;
        left: 0;
        top: 0;
        width: 100%;
        height: 1.1066666666666667rem;
        border-bottom: .013333333333333334rem solid #e3e4e8;
        padding: 0;
        z-index: 9;
    }

    .back-page-btn p {
        font-size: .4266666666666667rem;
        color: #2d2d34;
        font-weight: 500;
        line-height: 1.1066666666666667rem;
    }

    .back-page-btn div {
        position: absolute;
        left: 0;
        top: 0;
        width: 1.0133333333333334rem;
        height: 100%;
    }

    .back-page-btn div img {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate3d(-50%, -50%, 0);
        width: .21333333333333335rem;
        height: .37333333333333335rem;
    }
</style>

<script>
    import $ from 'jquery';

    let secondhandarea = {
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
                alertTilData: ["转卖提交数据中 ...", "一键转卖成功~", "一键转卖出现异常!"],
                buttonFlag: false,
                buttonVal: "点击加载更多数据",
                allOrderListFlag: false,
                pageStart: 1,
                pageSize: 8,
                allOrderList: [], // 所有待收货和待评价的列表
                iframeSrc: "",
                iframeTips: "数据加载中 ......"
            }
        },
        created() {
            // 加载数据
            this.getAllOrderList(this.pageStart, this.pageSize);
        },
        methods: {
            moreData() {
                if (this.allOrderListFlag) return;
                this.buttonFlag = true;
                this.buttonVal = "数据加载中...";
                this.getAllOrderList(this.pageStart += 1, this.pageSize += 8);
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
                // this.$router.go(-1);
                var host=location.host;
                if(host.indexOf("localhost")>-1)
                {
                	window.location.href="/#/index";
                }else
                { 
                	window.location.href = "https://"+host+"/#/index";
                }
            },
            //上划分页
            swipe(e) {
                if (e.direction == 'Up') {
                    this.page[this.status]++;
                    //this.getorderlist()
                }
            },
            //确认收货弹出框
            suregetgoods(id) {
                this.showalert = true;
                this.id = id;
                this.alertstatus = 2;
            },
            //再来一单
            buyagain() {
                this.$router.push('/index')
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
                            this.getAllOrderList(this.pageStart, this.pageSize);
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
                alert('详情请联系客服：400-990-7881')
            },
            // 获取订单状态为待收货和待评价的列表数据
            getAllOrderList(page, pagesize) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/OrderList?page=' + page + '&pagesize=' + pagesize + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: 'get',
                    complete: (xhr, textStatus) => {
                        let resJson = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            _this.buttonFlag = false;
                            _this.buttonVal = "点击加载更多数据";
                            alert(resJson.message);
                            return;
                        }
                        // 异常
                        if (resJson.status === 98) {
                            _this.buttonFlag = false;
                            _this.buttonVal = resJson.message;
                            //alert(resJson.message);
                            return;
                        }
                        // 成功
                        if (xhr.status === 200 && resJson.status === 0) {
                            let res = xhr.responseJSON.data;
                            if (res.length <= 0) {
                                _this.buttonFlag = false;
                                _this.buttonVal = "数据已经全部加载完成了~";
                                _this.allOrderListFlag = true;
                                return;
                            }

                            let flag = res.every(item => item.status !== 2 && item.status !== 3);
                            if (flag) {
                                _this.buttonFlag = false;
                                _this.buttonVal = "暂无数据 ~";
                                _this.allOrderListFlag = true;
                                return;
                            }

                            res.forEach(item => {
                                if (item.status === 2 || item.status === 3) {
                                    _this.allOrderList = [..._this.allOrderList, item];
                                    _this.buttonFlag = false;
                                    _this.buttonVal = "点击加载更多数据";
                                }

                            });
                        }
                    }
                });
            },
        },
        filters: {
            timeformat(time) {
                return time.replace(/\//g, '-')
            }
        }

    }
    export {secondhandarea as default}
</script>