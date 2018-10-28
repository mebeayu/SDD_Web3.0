<!--订单创建页-->
<template>
    <div class="order-creationbar">
        <div class="order-creation-goback" @click="goback">
            <img src="/Assets/Source/img/order-creation-goback.gif" alt="">
        </div>
        <viewtitle></viewtitle>
        <add id='first-item'></add>
        <!--订单列表展示开始-->
        <div class="wj-order-listbar">
            <ul>
                <li v-for="item in OrderList">
                    <div class="wj-order-list">
                        <img class="wj-order-list-pic"
                             v-bind:src="$store.state.imgs+item.goods_pic_id+$store.state.img_suffix"
                             :onerror="$store.state.defaultimg">
                        <p class="wj-order-list-til">{{ item.goods_name || item.shop_name }}</p>
                        <p class="wj-order-list-spec" v-if="item.specification">颜色:白，尺寸：{{ item.specification }}</p>
                        <p class="wj-order-list-price"><span>￥{{ (item.money).toFixed(2)}}</span><span>
                                <input type="button"
                                       @click="cartBtnNum(item,-1,$event)" value="-" class="wj-order-list-add"
                                       :class="item.goods_count===1?'wj-order-list-disable':''"/>
                                <input type="button"
                                       v-model.number="item.goods_count"
                                       class="wj-order-list-num"
                                       readonly="true"/>
                                <input type="button"
                                       @click="cartBtnNum(item,1,$event)"
                                       value="+"
                                       class="wj-order-list-reduce"
                                       :class="item.goods_count===99?'wj-order-list-disable':''"
                                />
                            </span></p>
                    </div>
                    <p class="wj-order-list-fare">运费：<span>{{ item.postage | formatPostage }}</span></p>
                    <div class='chosecoupons' @click='chosecoupons(item)'>
                        <span>请点击选择优惠券：<span v-if="item.user_coupons_countmoney!==0">{{ "已优惠" + item.user_coupons_countmoney + "元" }}</span></span>
                    </div>
                    <div v-if="0" class='chosecoupons' @click="clickPocketCardFn(1)">
                        <span>请点击选择兜兜卡：<span v-if="item.user_coupons_countmoney!==0">{{ "已优惠" + item.user_coupons_countmoney + "元" }}</span></span>
                    </div>
                    <div class='buy-card' @click="item.showcard = false" v-if='item.showcard'>
                        <div class='bc-tips'>
                            <span class="select-coupon">请选择以下优惠券：</span>
                            <span class="empty-select-coupon" @click="emptySelectCoupon(item)">清空已选优惠券</span>
                        </div>
                        <!--$store.state.makeorder.list-->
                        <ul class='card-list' v-if='list.length != 0'>
                            <li v-for='(childitem, index) in list'
                                @click='chosecard(childitem,item)'
                                v-bind:class='index == choseindex? "lion": ""'>
                                <div class='flexbox'>
                                    <div class='cl-left'>
                                        <i class='moneyicon'></i><span>{{childitem.countmoney}}</span>
                                    </div>
                                    <div class='cl-right'>
                                        <p style='margin-top:14px;'>
                                            {{childitem.name}}
                                        </p>
                                    </div>
                                </div>
                                <div class='cardtime'>过期时间:{{childitem.endtimestr}}</div>
                            </li>
                        </ul>
                        <div v-else style='color:#fff;padding:.5rem;'>暂无可用优惠券</div>
                        <div class="closebtn" @click="item.showcard = false"></div>
                    </div>

                    <div class="wj-order-list-buy-msg">
                        <div class="wj-order-list-buy-border">
                            <div class="wj-order-list-buy-icon"><img
                                    src="/Assets/Source/img/order-creation-bug-msg.png" alt=""></div>
                            <div class="wj-order-list-buy-opt">
                                买家留言<br/>
                                <p>选填：<textarea @blur="buyerMsg(item,$event)">{{ item.msgLeaveVal }}</textarea></p>
                            </div>
                        </div>
                    </div>
                    <p class="wj-order-list-buy-total"><span>小计：</span>￥{{ item.all_pay_cash.toFixed(2) }}
                    </p>
                </li>
            </ul>
        </div>
        <!--订单列表展示结束-->

        <!--请选择兜兜卡弹出框区域开始-->
        <div v-show="pocketCardIfShow"
             class="dialog-pocket-cards-bg">
            <div class="yes-pocket-card dialog-cont-top">
                <ul class="pocket-card-btn-list">
                    <li>
                        <div class="cards">
                            <label class="pocket-card-l">
                                <input type="radio" name="pocket-cards" :checked="pocketcardChecked" class="mgr mgr-primary"/>
                            </label>
                            <div class="pocket-card-r">
                                <img src="/Assets/Source/img/pocket-cards-100@2x.png" class="big" alt=""/>
                                <p class="cards-num">
                                    <span>100</span>
                                    <img src="/Assets/Source/img/wj-pocket-card-rmb-@3x.png" alt=""/>
                                </p>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="cards">
                            <label class="pocket-card-l">
                                <input type="radio" name="pocket-cards" :checked="pocketcardChecked" class="mgr mgr-primary"/>
                            </label>
                            <div class="pocket-card-r">
                                <img src="/Assets/Source/img/pocket-cards-100@2x.png" class="big" alt=""/>
                                <p class="cards-num">
                                    <span>100</span>
                                    <img src="/Assets/Source/img/wj-pocket-card-rmb-@3x.png" alt=""/>
                                </p>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="dialog-cont-bot">
                <button @click="clickPocketCardFn">取消</button>
                <button @click="clickPocketCardFn(3)">确定</button>
            </div>
        </div>
        <!--请选择兜兜卡弹出框区域结束-->

        <!--确认支付开始-->
        <div class="wj-order-list-payment" >
            <div class="wj-order-list-voucher-pur" @click="usableBean" v-show="methods_platform_hold_money()<1000000||data_is_zero_area_goods_type">
                <div class="check_box">
                    <input type="checkbox" disabled="disabled" readonly="readonly" class="mgc mgc-danger" ref="isBeanBtn" :checked="isCheckedBean"
                           v-model="isCheckedBean"/>
                </div>
                <span><img src="/Assets/Source/img/order-creation-gold-bean.png">消费金豆{{ computedBean }} (可用{{ useComputedBean() }}+<del>{{methods_platform_hold_money()}}</del>金豆)</span>
            </div>
            <div class="wj-order-list-voucher-pur"  v-show="!(methods_platform_hold_money()<1000000||data_is_zero_area_goods_type)">
                <div  class="red-package-time">您只能现金购物或者在0门槛金豆区购物
                    <a href="javascript:;" class="jdDetail" @click='showJdDetail'> 详情&gt;</a>
                </div>
            </div>

            <div class="wj-order-list-paymentbar">
                <span class="wj-order-list-payment-price">￥{{ computedTotal }}</span>
                <button ref="concessionBtn" class="wj-order-list-concession-btn" @click="makeorder">确认支付</button>
            </div>

        </div>
        <!--确认支付结束-->

        <!--消息提醒框开始-->
        <div v-if="alertFlag">
            <div class="msg-dialog"></div>
            <div class="msg-remind-box">
                <img :src="'/Assets/Source/img/'+ alertImg"/>
                <p>{{ alertTil }}</p>
            </div>
        </div>
        <!--消息提醒框结束-->
		<div class="pay_jd_rule_box" v-show="showRule">
		          <div class="pay_jd_rule">
		            <p class="pay_jd_rule_explain">您的累计买劵金额为:<span>{{data_pay_coupons_total_money}}元</span></p>
		           
		            <h6 >规则说明</h6>
		            <table>
		                <thead>
		                  <tr>
		                      <td>累计买劵金额</td>
		                      <td>购买门槛</td>
		                  </tr>
		                </thead>
		                <tbody>
		
		                    <tr v-for="(item, index) in data_UserHoldMoneyConfig" :key="index" >
		                      <td>{{item.buy_total_h_money_remark}}</td>
		                      <td v-if="index==0">可在<a href="/Event/DetonatingWeek">0门槛金豆区</a>使用</td>
		                      <td v-else>{{(parseFloat( item.platform_hold_money/10000)+'W金豆')}}</td>
		                    </tr>
		                     
		                </tbody>
		            </table>
		            <p class="pay_jd_rule_explain" style="margin-top:.2rem;">温馨提示:规则会根据市场变换而优化调整,一切解释权归省兜兜所有</p>
		            <div class="pay_jd_rule_btns">
		              <div class="btn surebtn" @click="hideBtn">确定</div>
		              <div class="btn cancelbtn" @click="hideBtn">取消</div>
		            </div>
		          </div>
		        </div>
    </div>
</template>

<style scoped>
	
	/* 修改 */
    .jdDetail{
          color: #f4cb57;
        text-decoration: underline;
        font-size: .35rem;
    }
	 .pay_jd_rule_btns{
       display: flex;
        justify-content: space-between;
        margin-top: .5rem;
        text-align: center;
     }
    .pay_jd_rule_btns .btn{
      padding: .26666667rem .4rem;
      line-height: .53333333rem;
      border: .02666667rem solid #dadada;
      border-radius: .2rem;
      width: 27%;
    }
    .pay_jd_rule_btns .surebtn{
      background: #bb1a1a;
      color: #fff;
    }
     .pay_jd_rule_btns .cancelbtn{
      background: #fff;
      color: #555;
    }
    .pay_jd_rule_box{
         width: 100%;
        height: 100%;
        background:rgba(0,0,0,.5);
        position: absolute;
        top: 0;
        z-index: 10;
        display: flex;
        justify-content: center;
        align-items: center
    }
    .pay_jd_rule_explain span{
      color: #f60;
    }
    .pay_jd_rule{
      display: inline-block;
      background: #fff;
      border-radius: 2%;
      position: absolute;
      color: #333;
      padding: .1rem;
    }
    .pay_jd_rule h6{
      line-height: 1rem;
    }
    table {
    width: 100%;
     border-collapse:collapse;
     margin: 0 auto;
     text-align: center;
    }
    table,tr, td {
      border: 1px solid #999;
    }
    thead tr{
      background: #ccc;
    }
    td {
      padding:0 .1rem;
      line-height: 1rem;
    }
    .red-package-time {
         color: #666;
         text-align: right;
         font-size: .38rem;
    }
    .jdDetail{
        color: #f4cb57;
        text-decoration: underline;
        font-size: .35rem;
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

    .order-creation-goback {
        position: fixed;
        top: 0;
        right: 0;
        z-index: 9;
        width: 1.5rem;
        height: 1.5rem;
        text-align: center;
    }

    .order-creation-goback img {
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        width: .52rem;
        height: .44rem;
    }

    .wj-order-list {
        padding: .26666666667rem;
        background: #fafafa;
        overflow: hidden;
    }

    .wj-order-listbar li, .wj-order-list-voucher-total {
        border-top: .4rem solid #f0f0f0;
    }

    .wj-order-listbar li:last-child {
        border-bottom: .4rem solid #f0f0f0;
    }

    .wj-order-list-voucher-total {
        padding-bottom: 1.826666666667rem;
    }

    .wj-order-list-til {
        font-size: .4rem;
        color: #2b2b2b;
        white-space: normal;
        height: 1rem;
        padding-bottom: .2rem;
    }

    .wj-order-list-buy-icon {
        float: left;
        margin: .32rem .24rem 0 0;
    }

    .wj-order-list-buy-icon img {
        width: .37333333333333335rem;
        height: .36rem;
    }

    img.wj-order-list-pic {
        float: left;
        width: 2rem;
        height: 2rem;
        margin-right: .373333333333rem;
    }

    .wj-order-list-spec {
        font-size: .38rem;
        color: #666;
    }

    .wj-order-list-buy-msg {
        padding: 0 .26666666666rem;
    }

    .wj-order-list-buy-border {
        padding: .266666666666rem 0;
        border: .0133333333333rem solid #dadada;
        border-left: none;
        border-right: none;
    }

    .wj-order-list-buy-opt p textarea {
        float: right;
        width: 84%;
        height: auto;
        line-height: .3rem;
        font-family: "Microsoft yahei";
        color: #666;
        font-size: .24rem;
        border: none;
        outline: none;
    }

    .chosecoupons {
        position: relative;
        bottom: 0;
        -webkit-box-sizing: border-box;
        box-sizing: border-box;
        color: #666;
        font-size: .34rem;
        text-align: left;
        border-top: 1px solid #dadada;
        padding: .32rem .25rem;
        background: #fff;
        overflow: hidden;
    }

    .chosecoupons::after {
        position: absolute;
        content: ">";
        right: .25rem;
        color: #a5a1a1;
        font-size: .4rem;
    }

    .wj-order-list-payment-price {
        float: left;
        width: 2.33333333333rem;
        color: #ba3027;
        font-size: .4rem;
        font-weight: bold;
        padding-left: .26666666666rem;
        overflow: hidden;
    }

    .wj-order-list-concession {
        position: relative;
        float: left;
        color: #999999;
        font-size: .38rem;
        padding-left: .16rem;
    }

    .wj-order-list-concession::before {
        position: absolute;
        top: 50%;
        left: 0;
        content: "";
        width: .0133333333333rem;
        height: .426666666rem;
        line-height: .426666666rem;
        background: #cecece;
        vertical-align: bottom;
        transform: translateY(-50%);
    }

    .wj-order-list-price {
        padding-top: .0533333333333rem;
        overflow: hidden;
    }

    .wj-order-list-price span:nth-of-type(1) {
        float: left;
        color: #ba3027;
        font-size: .4rem;
        font-weight: bold;
    }

    .wj-order-list-price span:nth-of-type(2) {
        float: right;
        font-size: .38rem;
        color: #666;
        height: .5066666666666rem;
        line-height: .5066666666666rem;
        border: 1px solid #dadada;
        border-radius: .0533333333333rem;
        text-align: center;
        overflow: hidden;
    }

    .wj-order-list-add, .wj-order-list-reduce, .wj-order-list-num {
        font-style: normal;
        float: left;
        font-size: .373333333333rem;
        padding: 0 .2rem;
        color: #555;
        height: 100%;
        background: #fff;
    }

    .wj-order-list-num {
        padding: 0 .26666666666rem;
        border: 1px solid #dadada;
        border-top: none;
        border-bottom: none;
    }

    .wj-order-list-disable {
        background: #eee;
        color: #b7b7b7;
    }

    .wj-order-list-fare {
        color: #666;
        font-size: .38rem;
        padding: .266666666666rem;
    }

    .wj-order-list-fare span {
        float: right;
    }

    .wj-order-list-buy-opt {
        font-size: .373333333333rem;
    }

    .wj-order-list-buy-opt p {
        color: #666666;
        font-size: .38rem;
        white-space: normal;
    }

    /*单选框*/
    .mgc {
        position: relative;
        width: .4rem;
        height: .4rem;
        background-clip: border-box;
        -webkit-appearance: none;
        -moz-appearance: none;
        appearance: none;
        margin: -.15px .008rem 0 0;
        vertical-align: text-bottom;
        border-radius: .04rem;
        -webkit-transition: background-color .25s;
        transition: background-color .25s;
        background-color: #fff;
        border: .0133333333333rem solid #d7d7d7;
        outline: none;
    }

    .mgc:checked:after {
        content: '';
        display: block;
        height: .1066666666666rem;
        width: .186666666666rem;
        border: 0 solid #333;
        border-width: 0 0 .0266666666666rem .0266666666666rem;
        -webkit-transform: rotate(-45deg);
        transform: rotate(-45deg);
        position: absolute;
        top: .066666666666rem;
        left: .066666666666rem;
    }

    .mgc:disabled {
        opacity: .65
    }

    .mgc:focus {
        outline: none;
        box-shadow: inset 0 .0133333333333rem .0133333333333rem rgba(255, 255, 255, 0.075), 0 0 .0266666666666rem #38a7ff
    }

    .mgc:checked {
        background-color: #ba3027;
        border-color: #ba3027;
    }

    .mgc:checked:after {
        border-color: #414141
    }

    .mgc-danger {
        background-color: #fff;
        border: .0133333333333rem solid #d7d7d7;
    }

    .mgc-danger:checked {
        background-color: #cf3b3a;
        border-color: #cf3b3a;
    }

    .mgc-danger:checked:after {
        border-color: #fff
    }

    .wj-order-list-buy-total {
        color: #ba3027;
        font-size: .4rem;
        text-align: right;
        padding: .373333333333rem .266666666666rem .373333333333rem 0;
    }

    .wj-order-list-buy-total span {
        font-size: .38rem;
        color: #666666;
    }

    .wj-order-list-voucher-total p {
        padding: .2rem .33333333333rem .533333333333rem 0;
        text-align: right;
    }

    .wj-order-list-voucher {
        padding: .2rem;
        overflow: hidden;
    }

    .wj-order-list-paymentbar {
        height: .933333333333rem;
        line-height: .933333333333rem;
        margin: .2066666666666rem 0;
        overflow: hidden;
    }

    .wj-order-list-voucher span:nth-of-type(1) {
        float: left;
        font-size: .38rem;
        color: #333;
    }

    .wj-order-list-voucher span:nth-of-type(2) {
        float: right;
        font-size: .38rem;
        color: #ba3027;
    }

    .wj-order-list-voucher-pur {
        position: relative;
        border-top: .0133333333333rem solid rgb(212, 212, 212);
        padding: .266666666666rem;
        overflow: hidden;
    }

    .wj-order-list-voucher-pur img {
        display: inline-block;
        width: .3333333333333333rem;
        height: .3333333333333333rem;
        margin-right: .0533333333333rem;
        vertical-align: middle;
    }

    .wj-order-list-voucher em {
        font: normal .38rem/0 serif;
        color: #666;
        padding-left: .0533333333333rem;
    }

    .wj-order-list-voucher-pur span {
        font-size: .38rem;
        color: #ba3027;
        float: right;
    }

    .wj-order-list-voucher-pur .check_box {
        float: left;
        margin-right: .2rem;
    }

    .wj-order-list-payment {
        background: #fff;
        height: auto;
        position: fixed;
        left: 0;
        bottom: 0;
        width: 100%;
        overflow: hidden;
    }

    .wj-order-list-concession-btn {
        border: none;
        background: #ba3027;
        float: right;
        color: #fff;
        width: 2.26666666666rem;
        border-radius: .10666666666666rem;
        height: .906666666666rem;
        font-size: .373333333333rem;
        position: absolute;
        bottom: .2rem;
        right: .266666666666rem;
    }

    .wj-order-listbar {
        padding-bottom: 2.5rem;
    }

    .bc-tips {
        position: relative;
    }

    .select-coupon {
        color: #ccc;
    }

    .empty-select-coupon {
        position: absolute;
        background: #fff;
        color: #f00;
        border-radius: .1rem;
        text-indent: 0;
        padding: .1rem .2rem;
        right: .4rem;
        top: -.1rem;
    }

    .closebtn {
        position: fixed;
        top: 0.26666667rem;
        left: 0.26666667rem;
        width: 0.8rem;
        height: 0.8rem;
        background: url(/Assets/Source/img/clearwords.png) #fff no-repeat center;
        background-size: 60% 60%;
        border-radius: 50%;
        -webkit-filter: brightness(1.5);
    }

    .buy-card {
        background: rgba(0, 0, 0, 0.6);
        min-height: 100%;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        z-index: 111;
        right: 0;
        bottom: 0;
    }
</style>

<script>
    //引入 返回 地址 订单详情显示
    import viewtitle from './title';
    import add from './makeorder/address'
    import ordershowgoods from './makeorder/ordershowgood'

    import $ from 'jquery';

    let makesureorder = {
        data() {
            return {
                discount: 0,
                // usedis:false,
                lastmoney: 0,
                totalmoney: 0,
                list: [],
                num: 0,
                return_money: 0,
                choseindex: 0,
                chosedcard: {},
                OrderList: [],
                btnFlag: false,
                isCheckedBean: false,
                isFlagBean: true,
                clickFlag: true,
                discountPriceData: null,
                alertFlag: false,
                alertImg: null,
                alertTil: null,
                alertImgData: ["status_loading.gif", "sign-check-icon.png", "sign-error-icon.png"],
                alertTilData: ["数据加载中 ...", "数据加载成功", "数据加载异常"],
                // 兜兜卡
                pocketCardIfShow: false, // 控制请选择兜兜卡弹出框区域是否显示
                pocketcardChecked: false,
                platform_hold_money:100000,// 平台持有的金豆数量
                data_goods_type:{},//商品类型
                data_zero_hold_goods_type:895,//0门槛商品类型是895
                data_is_zero_area_goods_type:false,// 默认都不是0门槛专区
                showRule:false,
                data_pay_coupons_total_money:0,// 购券花费
                data_hold_money:0,
                data_UserHoldMoneyConfig:[]
            }
        },
        filters: {
            // 格式化运费
            formatPostage(val) {
                if (val === 0) {
                    return "包邮";
                }
                return "￥" + val.toFixed(2);
            }
        },
        components: {
            viewtitle,
            add,
            ordershowgoods,
        },
        computed: {
            // 计算可消费金豆数量
            computedBean() {
                let total = 0;
                this.OrderList.forEach((item) => {
                    total += item.need_pay_beans;
                });
                return total;
            },
            computed_platform_hold_money(){
            	return parseInt(this.platform_hold_money);
            },
            // 计算总价
            computedTotal() {
                let total = 0;
                if (this.isCheckedBean) {
                    this.OrderList.forEach((item) => {
                        total += item.need_pay_money;
                    });
                } else {
                    this.OrderList.forEach((item) => {
                        total += item.all_pay_cash;
                    });
                }
                return total.toFixed(2);
            },
            computed_get_hold_money(){
        		return parseInt(this.data_hold_money/10000);
        	}
        },
        created() {
        	this.method_ajax_GetUserHoldMoney();
            //console.log(this.$route.params.from,this.$store.state.goodsid)
            this.method_ajax_getUserHoldMoneyConfig();


            // // 计算可用金豆
            // this.useComputedBean();

            // this.computedTotal();
        },
        methods: {
        	method_get_hold_money()
        	{
        		return parseInt(this.data_hold_money/10000);
        	},
            method_ajax_getUserHoldMoneyConfig()
        	{
        		var this_vm=this;
        		$.ajax({
                    url: "/WebApi/UserManager/GetUserHoldMoneyConfig?token="+this_vm.$store.state.shorttoken,
                    type: "get",
                    complete: function(xhr, textStatus) {
                        var res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            this_vm.data_UserHoldMoneyConfig=res.data;
                            this_vm.data_pay_coupons_total_money=res.add_data.pay_coupons_total_money;
                            this_vm.data_hold_money=res.add_data.hold_money;
                        }
                    }
                });
        	},
        	
            showJdDetail(){
                this.showRule=true;
            },
            hideBtn(){
                this.showRule=false;
            },
        	methods_init_orderlist()
        	{
        		this.$store.dispatch('setPreOrderList', this.$route.params.orderList).then(() => {
                const len = this.$store.state.PreOrderList.length;
               //debugger;
                this.OrderList = this.$store.state.PreOrderList;
                // 发送可用金豆请求
                //this.useBean();
				this.data_goods_type={};
                for (let i = 0; i < len; i++) {
                    this.discount = parseFloat(this.discount) + parseFloat(this.$store.state.PreOrderList[i].discount);
                    this.discount = this.discount.toFixed(2);
                    this.totalmoney += this.$store.state.PreOrderList[i]['money'];
                    this.return_money += this.$store.state.PreOrderList[i]['return_money'];
                    this.data_goods_type[this.$store.state.PreOrderList[i]['goods_type']]="1";
                }
                this.motheds_set_is_zero_area();
                this.useBean();
                this.totalmoney = this.totalmoney.toFixed(2)
                this.return_money = this.return_money.toFixed(2)
                sessionStorage.setItem('totalcash', this.totalmoney)
                this.$store.commit('SET_JINBIDISCOUNT', this.discount)
            });

            if(this.isWeiXin()){
                this.isWeiXinBeansPay();
            }
        	},
        	
        	methods_platform_hold_money(){
        		
            	return parseInt(this.platform_hold_money);
            },
        	//
        	method_ajax_GetUserHoldMoney()
        	{
        		var this_vm=this;
        		$.ajax({
                    url: "/WebApi/UserManager/GetUserHoldMoney?token="+this_vm.$store.state.shorttoken,
                    type: "get",
                    complete: function(xhr, textStatus) {
                        var res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            this_vm.platform_hold_money=res.data.hold_money;
                            this_vm.methods_init_orderlist();
                        }
                    }
                });
        	},
            // 是否显示兜兜卡弹框区域
            clickPocketCardFn(val) {
                // val有传值的情况
                // 点击请选择按钮
                if (val === 1) {
                    this.pocketCardIfShow = true;
                    return;
                }
                // 点击确认按钮
                if (val === 3) {
                }
                // 点击取消按钮
                this.pocketCardIfShow = false;
            },
            //判断是否是微信浏览器的函数
            isWeiXin() {
                let ua = window.navigator.userAgent.toLowerCase();
                //通过正则表达式匹配ua中是否含有MicroMessenger字符串
                if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                    return true;
                } else {
                    return false;
                }
            },
            // 设置微信信息
            setWeixinMsg() {
                // 如果是微信浏览器
                if (this.isWeiXin()) {
                    confirm("请先下载省兜兜APP，再使用金豆进行支付~", function () {
                        window.location.href = "https://e-shop.rrlsz.com.cn/ProductForGame";
                    });
                    return;
                }
            },
             
            //
            // 计算可用金豆
            useComputedBean() {
            	let beans = localStorage.getItem("hMoney")-parseInt(this.platform_hold_money); 
                return beans.toFixed(2); 
            },
            // 是微信端的浏览器默认发送 0
            isWeiXinBeansPay(){
                const _this = this;
                const OrderList = this.$route.params.orderList;
                const isNum = 0;
                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderIsBeansPayV3?orderlist=' + encodeURIComponent(OrderList) + '&is_beans_pay=' + isNum + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    success: function (data) {
                        if (data.status !== 0) {
                            alert(data.message);
                            return;
                        }
                    }
                });
            },
            //是否是零元专区
            motheds_set_is_zero_area()
            {
            	var is_zero_area_goods_type=false;
				var scount=0;
				for(var s in this.data_goods_type)
				{
					scount++;
				}
				
				if(scount==1 &&this.data_goods_type[this.data_zero_hold_goods_type])
				{
					is_zero_area_goods_type=true;
				}
				//debugger;
				this.data_is_zero_area_goods_type= is_zero_area_goods_type;
				if(this.data_is_zero_area_goods_type)
				{
					this.platform_hold_money=0;
				}
            },
            
            // 调用金豆是否选中可用的接口
            beanIsCheckedApi(id, isNum) {
                this.setWeixinMsg();
                // 判断是否是微信浏览器
                if (this.isWeiXin()) return;
				var is_zero_area_goods_type=this.data_is_zero_area_goods_type;
				 
				if(!is_zero_area_goods_type)
				{
					if(this.methods_platform_hold_money()>=1000000)
					{
						alert("您只能现金购物或者在0门槛金豆区购物");
						return;
					}
				}
				 
                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderIsBeansPayV3?orderlist=' + id + '&is_beans_pay=' + isNum + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    success: function (data) {
                        if (data.status !== 0) {
                            alert(data.message);
                            return;
                        }
                    }
                });
            },
            // 返回首页
            goback() {
                this.$router.push("https://e-shop.rrlsz.com.cn/#/index");
            },
            //订单创建完成
            makeorder() {
                //收货地址判定
                let orderIdListAry = this.$route.params.orderList;
                if (this.$store.state.receive_info && this.$store.state.receive_info.id) {
                    this.$store.dispatch('sendrecivedid').then(() => {
                        let fincash = this.computedTotal;
                        window.location.href = '/payway?list=' + encodeURIComponent(orderIdListAry) + '&token=' + encodeURIComponent(this.$store.state.shorttoken) + '&type=1,2,3' + '&cash=' + fincash + '&wallet=' + this.$store.state.userdiscount + '&jd=' + this.$store.state.goldbean;
                    });
                } else {
                    alert('请填写收货地址')
                }
            },
            //使用折扣判定
            usediscount() {
                this.usedis = !this.usedis
            },
            chosecoupons(item) {
                this.$store.dispatch('getlist').then(() => {
                    if (typeof item.showcard === "undefined") {
                        this.$set(item, "showcard", true);
                    }
                    this.$set(item, "showcard", true);
                    this.list = this.$store.state.makeorder.list
                })
            },
            // 清空优惠券
            emptySelectCoupon(item) {
                this.updateCommodiInfo(item.id, item.goods_id, item.goods_count, 0);
                // 隐藏前端界面展示的优惠券 === 后端没有清空
                item.user_coupons_countmoney = 0;
            },
            chosecard(childitem, itemOld) {

                let orderId = itemOld.id,
                    goodsId = itemOld.goods_id,
                    userCouponsId = childitem.id,
                    _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderGoodsAttrV3?order_id=' + orderId + '&goods_id=' + goodsId + '&token=' + encodeURIComponent(_this.$store.state.shorttoken) + '&user_coupons_id=' + userCouponsId,
                    type: "post",
                    success: function (data) {
                        if (data.status !== 0) {
                            alert(data.message);
                            return;
                        }
                        // 过滤到以前的老数据，更新现在的新数据
                        let arrTemp = [],
                            newOrderList = data.data;
                        newOrderList.forEach(item => arrTemp.push(item.id));
                        _this.OrderList.forEach((item, index) => {
                            arrTemp.forEach((item2, index2) => {
                                if (item.id === item2) {
                                    _this.OrderList.splice(index, 1, ...newOrderList);
                                }
                            })
                        });

                    }
                })

            },

            // 更新商品数量或者商品备注信息
            updateCommodiInfo(orderId, goodsId, goodsCount, userCouponsId) {

                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderGoodsAttrV3?order_id=' + orderId + '&goods_id=' + goodsId + '&token=' + encodeURIComponent(_this.$store.state.shorttoken) + '&user_coupons_id=' + userCouponsId + '&goods_count=' + goodsCount,
                    type: "post",
                    complete: (XMLHttpRequest, textStatus) => {
                        // console.log(XMLHttpRequest, textStatus);
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                this.$refs.concessionBtn.disable = false;
                                _this.useBean();
                                clearTimeout(timer);
                            }, 1000);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        // 成功 console.log(XMLHttpRequest.responseJSON.data);
                        // 过滤到以前的老数据，更新现在的新数据
                        let arrTemp = [],
                            newOrderList = XMLHttpRequest.responseJSON.data;
                        newOrderList.forEach(item => arrTemp.push(item.id));
                        _this.OrderList.forEach((item, index) => {
                            arrTemp.forEach((item2, index2) => {
                                if (item.id === item2) {
                                    _this.OrderList.splice(index, 1, ...newOrderList);
                                    //_this.useBean();
                                }
                            })
                        });
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            this.$refs.concessionBtn.disable = false;
                            _this.useBean();
                            clearTimeout(timer);
                        }, 1000);

                    }
                })
            },

            // 更新留言信息
            buyerMsg(item, $event) {
                let orderId = item.id,
                    goodsId = item.goods_id,
                    msg = $event.target.value,
                    _this = this;

                // 显示数据加载中
                this.alertImg = this.alertImgData[0];
                this.alertTil = this.alertTilData[0];
                this.alertFlag = true;

                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderGoodsAttrV3?order_id=' + orderId + '&goods_id=' + goodsId + '&token=' + encodeURIComponent(_this.$store.state.shorttoken) + '&msg_leave_word=' + encodeURIComponent(msg),
                    type: "post",
                    complete: (XMLHttpRequest, textStatus) => {
                        // console.log(XMLHttpRequest, textStatus);
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                this.$refs.concessionBtn.disable = false;
                                clearTimeout(timer);
                            }, 1000);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        // 成功 console.log(XMLHttpRequest.responseJSON.data);
                        // 过滤到以前的老数据，更新现在的新数据
                        let arrTemp = [],
                            newOrderList = XMLHttpRequest.responseJSON.data;
                        newOrderList.forEach(item => arrTemp.push(item.id));
                        _this.OrderList.forEach((item, index) => {
                            arrTemp.forEach((item2, index2) => {
                                if (item.id === item2) {
                                    _this.OrderList.splice(index, 1, ...newOrderList);
                                }
                            })
                        });
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            this.$refs.concessionBtn.disable = false;
                            clearTimeout(timer);
                        }, 1000);

                    }

                })
            }
            ,

            // 添加，减少数量
            cartBtnNum(item, num, $event) {
                let order_id = item.id,
                    goods_id = item.goods_id,
                    userCouponsId = item.user_coupons_id;

                // 加
                if (num > 0) {
                    // 显示数据加载中
                    this.alertImg = this.alertImgData[0];
                    this.alertTil = this.alertTilData[0];
                    this.alertFlag = true;
                    this.$refs.concessionBtn.disable = true;
                    if (item.goods_count === 99) return;
                    item.goods_count++;
                    this.updateCommodiInfo(order_id, goods_id, item.goods_count, userCouponsId);
                    // this.useBean();
                    return;
                }
                // 减
                if (item.goods_count === 1) return;
                // 显示数据加载中
                this.alertImg = this.alertImgData[0];
                this.alertTil = this.alertTilData[0];
                this.alertFlag = true;
                this.$refs.concessionBtn.disable = true;
                item.goods_count--;
                if (typeof item.msgLeaveVal === "undefined") {
                    this.$set(item, "msgLeaveVal", "");
                }
                this.updateCommodiInfo(order_id, goods_id, item.goods_count, userCouponsId);
                // this.useBean();
            },

            // 点击是否选中可用金豆
            clickUseBean() {
                this.setWeixinMsg();
                // 判断是否是微信浏览器
                if (this.isWeiXin()) return;

                let isNum = 0, arrId = [], _this = this;
                this.OrderList.forEach(item => {
                    isNum += item.need_pay_beans;
                    arrId.push(item.id);
                });
                arrId.length > 1 ? arrId.join(",") : arrId.join("");
                let hMoney = localStorage.getItem("hMoney")-parseInt(this.platform_hold_money);
                this.useComputedBean();
                let beanFlag = null;
                // 金豆够
                if (hMoney > isNum) {
                    this.isFlagBean = true;
                    this.$refs.isBeanBtn.disabled = false;
                    if (_this.isCheckedBean) {
                        _this.isCheckedBean = false;
                        beanFlag = 0;
                    } else {
                        _this.isCheckedBean = true;
                        beanFlag = 1;
                    }
                } else {
                    // 不够
                    _this.alertFlag = false;
                    this.isCheckedBean = false;
                    this.isFlagBean = false;
                    beanFlag = 0;
                    this.$refs.isBeanBtn.disabled = true;
                }

                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderIsBeansPayV3?orderlist=' + arrId + '&is_beans_pay=' + beanFlag + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest, textStatus) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                this.$refs.concessionBtn.disable = false;
                                clearTimeout(timer);
                            }, 1000);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            this.$refs.concessionBtn.disable = false;
                            clearTimeout(timer);
                        }, 1000);

                    }
                });
            },

            // 可用金豆
            useBean() {
                // this.setWeixinMsg();
                // 判断是否是微信浏览器
                if (this.isWeiXin()) return;

                let isNum = 0, arrId = [], _this = this;
                this.OrderList.forEach(item => {
                    isNum += item.need_pay_beans;
                    arrId.push(item.id);
                });
                arrId.length > 1 ? arrId.join(",") : arrId.join("");
                let hMoney = localStorage.getItem("hMoney")-parseInt(this.platform_hold_money);
                this.useComputedBean();
                let beanFlag = null;
                // 金豆够
                var is_show_check_div=(this.methods_platform_hold_money()<1000000||this.data_is_zero_area_goods_type);
                if (hMoney > isNum && is_show_check_div) {
                    this.isFlagBean = true;
                    this.$refs.isBeanBtn.disabled = false;
                    _this.isCheckedBean = true;
                    beanFlag = 1;
                } else {
                    // 不够
                    _this.alertFlag = false;
                    this.isCheckedBean = false;
                    this.isFlagBean = false;
                    beanFlag = 0;
                    this.$refs.isBeanBtn.disabled = true;
                }

                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderIsBeansPayV3?orderlist=' + arrId + '&is_beans_pay=' + beanFlag + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest, textStatus) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                this.$refs.concessionBtn.disable = false;
                                clearTimeout(timer);
                            }, 1000);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            this.$refs.concessionBtn.disable = false;
                            clearTimeout(timer);
                        }, 1000);

                    }
                });
            },

            // 是否选中可用金豆
            usableBean() {
                this.setWeixinMsg();
                // 判断是否是微信浏览器
                if (this.isWeiXin()) return;

                if (!this.isFlagBean) {
                    this.clickUseBean();
                    alert("金豆未达到门槛" + this.methods_platform_hold_money() + "或未达到商品购买金豆数量，无法用金豆购买，可直接用现金购买哦。");
                    return;
                }

                // 显示数据加载中
                this.alertImg = this.alertImgData[0];
                this.alertTil = this.alertTilData[0];
                this.alertFlag = true;
                this.$refs.concessionBtn.disable = true;

                this.clickUseBean();
            },

            // 可用代金券
            usableVoucher(item, $event) {
                let arrId = [], _this = this;
                arrId.length > 1 ? arrId.join(",") : arrId.join("");
                $.ajax({
                    url: '/WebApi/TradeManager/GetAvailableCoupons?order_list=' + item.id + '&token=' + encodeURIComponent(_this.$store.state.shorttoken) + '&page_index=' + 1 + '&page_size=' + 1000,
                    type: "get",
                    success: function (data) {
                        if (data.status !== 0) {
                            alert(data.message);
                            return;
                        }
                        _this.list = data.data;
                    }
                });
            }
        }
    }

    export default makesureorder
</script>