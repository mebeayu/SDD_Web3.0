<!--订单创建页-->
<template>
    <div class="order-creationbar">
        <div class="order-creation-goback" @click="goback">
            <img src="/Assets/Source/img/order-creation-goback.gif" alt="">
        </div>
        <viewtitle></viewtitle>
        <add id='first-item'></add>
        <!--订单列表展示开始-->
        <div class="sdd-order-listbar">
            <ul>
                <li v-for="item in OrderList">
                    <div class="sdd-order-list">
                        <img class="sdd-order-list-pic"
                             v-bind:src="$store.state.imgs+item.goods_pic_id+$store.state.img_suffix"
                             :onerror="$store.state.defaultimg">
                        <p class="sdd-order-list-til">{{ item.goods_name || item.shop_name }}</p>
                        <p class="sdd-order-list-spec" v-if="item.specification">颜色:白，尺寸：{{ item.specification }}</p>
                        <p class="sdd-order-list-price"><span>￥{{ (item.money).toFixed(2)}}</span><span>
                                <input type="button"
                                       @click="cartBtnNum(item,-1,$event)" value="-" class="sdd-order-list-add"
                                       :class="item.goods_count===1?'sdd-order-list-disable':''"/>
                                <input type="button"
                                       v-model.number="item.goods_count"
                                       class="sdd-order-list-num"
                                       readonly="true"/>
                                <input type="button"
                                       @click="cartBtnNum(item,1,$event)"
                                       value="+"
                                       class="sdd-order-list-reduce"
                                       :class="item.goods_count===99?'sdd-order-list-disable':''"
                                />
                            </span></p>
                    </div>
                    <p class="sdd-order-list-fare">运费：<span>{{ item.postage | formatPostage }}</span></p>
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

                    <div class="sdd-order-list-buy-msg">
                        <div class="sdd-order-list-buy-border">
                            <div class="sdd-order-list-buy-icon"><img
                                    src="/Assets/Source/img/order-creation-bug-msg.png" alt=""></div>
                            <div class="sdd-order-list-buy-opt">
                                买家留言<br/>
                                <p>选填：<textarea @blur="buyerMsg(item,$event)">{{ item.msgLeaveVal }}</textarea></p>
                            </div>
                        </div>
                    </div>
                    <p class="sdd-order-list-buy-total"><span>小计：</span>￥{{ item.all_pay_cash.toFixed(2) }}
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
                                    <img src="/Assets/Source/img/sdd-pocket-card-rmb-@3x.png" alt=""/>
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
                                    <img src="/Assets/Source/img/sdd-pocket-card-rmb-@3x.png" alt=""/>
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
        <div class="sdd-order-list-payment" >
            <!-- 2018.8.10号新增 -->
             <!-- <div class="wj-order-list-voucher-pur" @click="usableBean" v-show="methods_platform_hold_money()<10000||data_is_zero_area_goods_type">
                <div class="check_box">
                    <input type="checkbox" disabled="disabled" readonly="readonly" class="mgc mgc-danger" ref="isBeanBtn" :checked="isCheckedBean"
                           v-model="isCheckedBean"/>
                </div>
                <span><img src="/Assets/Source/img/order-creation-gold-bean.png">消费金豆{{ computedBean }} (可用{{ useComputedBean() }}+<del>{{methods_platform_hold_money()}}</del>金豆)</span>
            </div>
            <div class="wj-order-list-voucher-pur"  v-show="!(methods_platform_hold_money()<10000||data_is_zero_area_goods_type)">
                <div  class="red-package-time">您只能现金购物或者在0门槛金豆区购物
                    <a href="javascript:;" class="jdDetail" @click='showJdDetail'> 详情&gt;</a>
                </div>
            </div> -->
            <div class="order_list_payment_style"  v-show="methods_platform_hold_money()<10000||data_is_zero_area_goods_type">
                <p class="payment_style_title">请选择购买方式  <span> (可用{{ useComputedBean() }}+<del>{{methods_platform_hold_money()}}</del>金豆)</span></p>
                <ul>
                    <li class="payment_style_list" data-status="1" @click="methods_check_payment_style(1)"> 
                        <div  :class="{ 'radio_icon_box': true, 'selected': data_check_status==1}"></div>
                        <p>{{payment_cash}}</p>
                    </li>
                    <li  :class="{'payment_style_list':true,'disable_color':(data_include_status&2)!=2}" data-status="2" @click="methods_check_payment_style(2)" v-show="(methods_platform_hold_money()<10000||data_is_zero_area_goods_type)">  
                       <div  :class="{ 'radio_icon_box': true, 'selected': data_check_status==2}"></div>
                        <p>{{payment_cashJd}}</p>
                    </li>
                    <li  :class="{'payment_style_list':true,'disable_color':(data_include_status&4)!=4}" data-status="4" @click="methods_check_payment_style(4)" v-show="(methods_platform_hold_money()<10000||data_is_zero_area_goods_type)"> 
                        <div :class="{ 'radio_icon_box': true, 'selected': data_check_status==4}"></div>
                        <p  class="payment_style_cashInvite">{{payment_cashInvite}}</p>
                        <button v-show="data_order_shared_times<data_max_shared_times" class="payment_cashInvite_btn" @click="methods_btn_click_invite_users($event)">邀{{data_max_shared_times}}人可享受</button> 
                    </li>
                </ul>
            </div>
            <div class="wj-order-list-voucher-pur"  v-show="!(methods_platform_hold_money()<10000||data_is_zero_area_goods_type)">
                <div  class="red-package-time">您只能现金购物或者在0门槛金豆区购物
                    <a href="javascript:;" class="jdDetail" @click='showJdDetail'> 详情&gt;</a>
                </div>
            </div>
            <div class="sdd-order-list-paymentbar">
                <span class="sdd-order-list-payment-price">合计: <strong> {{ payment_current }}</strong></span>
                <button ref="concessionBtn" class="sdd-order-list-concession-btn" @click="makeorder">确认支付</button>
            </div>
             <!-- 2018.8.10号新增 -->

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
	.disable_color
	{
		background-color: #c9c9c9;
	}
	
	/* 修改 */
    .selected{
        background: url('/Assets/Game/img/order_xz.png') no-repeat center top!important;
        background-size: .4rem .4rem!important;
    }
    .radio_icon_box{
        width:.4rem;
        height:.4rem;
        float: left;
        background: url('/Assets/Game/img/order_wx.png') no-repeat center top;
        background-size: .4rem .4rem;
        margin-top: .25rem;
        margin-right: .2rem;
    }
    /* .radio_icon{
            display:block;
            width:.4rem;
            height:.4rem;
            float: left;
    }
    .radio_icon img{
        width:100%;
        height:100%;
    } */
    .order_list_payment_style{
        padding: 0 .5rem;
        font-size: .35rem;
        padding-right: 0.2rem;
    }
    .payment_style_title{
        margin-bottom: .1rem;
        font-size: .4rem;
    } 
    .order_list_payment_style li{
        height: 1rem;
        line-height: 1rem;
    }
    .payment_style_cashInvite{
            float: left;
    }   
    .payment_cashInvite_btn{
        padding: 0 .4rem;
        color: #fff;
        background: #67c3aa;
        border-radius: .66666667rem;
        font-style: normal;
        min-width: 1.5rem;
        text-align: center;
        white-space: nowrap;
        font-size: .34rem;
        border: none;
        float: right;
        height: .8rem;
        line-height: .8rem;
        margin-top: 0.1rem;
    }
    .jdDetail{
        color: #f4cb57;
        text-decoration: underline;
        font-size: .35rem;
        padding: 0 .5rem;
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

    .sdd-order-list {
        padding: .26666666667rem;
        background: #fafafa;
        overflow: hidden;
    }

    .sdd-order-listbar li, .sdd-order-list-voucher-total {
        border-top: .4rem solid #f0f0f0;
    }

    .sdd-order-listbar li:last-child {
        border-bottom: .4rem solid #f0f0f0;
    }

    .sdd-order-list-voucher-total {
        padding-bottom: 1.826666666667rem;
    }

    .sdd-order-list-til {
        font-size: .4rem;
        color: #2b2b2b;
        white-space: normal;
        height: 1rem;
        padding-bottom: .2rem;
    }

    .sdd-order-list-buy-icon {
        float: left;
        margin: .32rem .24rem 0 0;
    }

    .sdd-order-list-buy-icon img {
        width: .37333333333333335rem;
        height: .36rem;
    }

    img.sdd-order-list-pic {
        float: left;
        width: 2rem;
        height: 2rem;
        margin-right: .373333333333rem;
    }

    .sdd-order-list-spec {
        font-size: .38rem;
        color: #666;
    }

    .sdd-order-list-buy-msg {
        padding: 0 .26666666666rem;
    }

    .sdd-order-list-buy-border {
        padding: .266666666666rem 0;
        border: .0133333333333rem solid #dadada;
        border-left: none;
        border-right: none;
    }

    .sdd-order-list-buy-opt p textarea {
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

    .sdd-order-list-payment-price {
        float: left;
       /* width: 2.33333333333rem;*/
        font-size: .38rem;
        font-weight: bold;
        padding-left: .26666666666rem;
        overflow: hidden;
    }
    .sdd-order-list-payment-price strong{
        color: #ba3026;
        font-size: .4rem;
    }

    .sdd-order-list-concession {
        position: relative;
        float: left;
        color: #999999;
        font-size: .38rem;
        padding-left: .16rem;
    }

    .sdd-order-list-concession::before {
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

    .sdd-order-list-price {
        padding-top: .0533333333333rem;
        overflow: hidden;
    }

    .sdd-order-list-price span:nth-of-type(1) {
        float: left;
        color: #ba3027;
        font-size: .4rem;
        font-weight: bold;
    }

    .sdd-order-list-price span:nth-of-type(2) {
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

    .sdd-order-list-add, .sdd-order-list-reduce, .sdd-order-list-num {
        font-style: normal;
        float: left;
        font-size: .373333333333rem;
        padding: 0 .2rem;
        color: #555;
        height: 100%;
        background: #fff;
    }

    .sdd-order-list-num {
        padding: 0 .26666666666rem;
        border: 1px solid #dadada;
        border-top: none;
        border-bottom: none;
    }

    .sdd-order-list-disable {
        background: #eee;
        color: #b7b7b7;
    }

    .sdd-order-list-fare {
        color: #666;
        font-size: .38rem;
        padding: .266666666666rem;
    }

    .sdd-order-list-fare span {
        float: right;
    }

    .sdd-order-list-buy-opt {
        font-size: .373333333333rem;
    }

    .sdd-order-list-buy-opt p {
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

    .sdd-order-list-buy-total {
        color: #ba3027;
        font-size: .4rem;
        text-align: right;
        padding: .373333333333rem .266666666666rem .373333333333rem 0;
    }

    .sdd-order-list-buy-total span {
        font-size: .38rem;
        color: #666666;
    }

    .sdd-order-list-voucher-total p {
        padding: .2rem .33333333333rem .533333333333rem 0;
        text-align: right;
    }

    .sdd-order-list-voucher {
        padding: .2rem;
        overflow: hidden;
    }

    .sdd-order-list-paymentbar {
        height: .933333333333rem;
        line-height: .933333333333rem;
        margin: .2066666666666rem 0;
        overflow: hidden;
    }

    .sdd-order-list-voucher span:nth-of-type(1) {
        float: left;
        font-size: .38rem;
        color: #333;
    }

    .sdd-order-list-voucher span:nth-of-type(2) {
        float: right;
        font-size: .38rem;
        color: #ba3027;
    }

    .sdd-order-list-voucher-pur {
        position: relative;
        border-top: .0133333333333rem solid rgb(212, 212, 212);
        padding: .266666666666rem;
        overflow: hidden;
    }

    .sdd-order-list-voucher-pur img {
        display: inline-block;
        width: .3333333333333333rem;
        height: .3333333333333333rem;
        margin-right: .0533333333333rem;
        vertical-align: middle;
    }

    .sdd-order-list-voucher em {
        font: normal .38rem/0 serif;
        color: #666;
        padding-left: .0533333333333rem;
    }

    .sdd-order-list-voucher-pur span {
        font-size: .38rem;
        color: #ba3027;
        float: right;
    }

    .sdd-order-list-voucher-pur .check_box {
        float: left;
        margin-right: .2rem;
    }

    .sdd-order-list-payment {
        background: #fff;
        height: auto;
        position: fixed;
        left: 0;
        bottom: 0;
        width: 100%;
        overflow: hidden;
    }

    .sdd-order-list-concession-btn {
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

    .sdd-order-listbar {
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
                platform_hold_money:100000,// 平台持有的金豆数量,门槛
                data_goods_type:{},//商品类型
                data_zero_hold_goods_type:895,//0门槛商品类型是895
                data_is_zero_area_goods_type:false,// 默认都不是0门槛专区
                showRule:false,
                data_pay_coupons_total_money:0,// 购券花费
                data_hold_money:0,
                data_UserHoldMoneyConfig:[],
                //liling
                payment_cash:'-',  //现金
                payment_cashJd:'-',  //现金金豆
                payment_cashInvite:'-', //邀请之后的优惠价
                payment_current:'',//当前选择的
                //radio_checkd:true,  //勾选
                //start--zetee
                Order_Shared_discount_money_rate :'0.29',//钱的比例
        		Order_Shared_discount_beans_rate:'0.71',//豆的比例
        		data_order_shared_times:0,//已经分享了多少次
        		data_max_shared_times:2, //最大的分享次数
        		data_check_status:1,   //选中的支付状态
        		data_include_status:1 //包含的状态
                //end --zetee
                
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
        	
        	this.Order_Shared_discount_money_rate = window.Order_Shared_discount_money_rate;
        	this.Order_Shared_discount_beans_rate = window.Order_Shared_discount_beans_rate;
        	this.data_max_shared_times=window.Order_Shared_people_num;
        	this.method_ajax_GetUserHoldMoney();
            //console.log(this.$route.params.from,this.$store.state.goodsid)
            this.method_ajax_getUserHoldMoneyConfig();


            // // 计算可用金豆
            // this.useComputedBean();

            // this.computedTotal();
        },
        mounted(){
            //this.methods_check_payment_style();
        },
        methods: {
        	
        	//获取最终支付的RMB
        	methods_get_finnal_cash()
        	{
				 if(this.data_check_status==1)
				 {
				 	return this.methods_get_all_pay_cash().toFixed(2);
				 	
				 }else if(this.data_check_status==2)
				 {
				 	return this.methods_get_shared_discount_cash().toFixed(2);
				 }
				 else if(this.data_check_status==4)
				 {
				 	return this.methods_get_need_pay_money().toFixed(2);
				 }
				 return this.methods_get_all_pay_cash().toFixed(2);
        	},
        	//设置3中支付类型
        	methods_set_three_payment_type()
        	{
        		this.payment_cash=`￥${this.methods_get_all_pay_cash().toFixed(2)}`;  //现金
                this.payment_cashJd=`￥${this.methods_get_shared_discount_cash().toFixed(2)}+${this.methods_get_shared_discount_beans().toFixed(0)}金豆`; //现金金豆
                this.payment_cashInvite=`￥${this.methods_get_need_pay_money().toFixed(2)}+${this.methods_get_need_pay_beans().toFixed(0)}金豆`; //邀请之后的优惠价
        	},
        	//邀请用户
        	methods_btn_click_invite_users(event)
        	{
        		event.stopPropagation();
        		var that=this;
	            var ua = navigator.userAgent.toLowerCase();
	            var isWeixin = ua.indexOf('micromessenger') != -1;
	            if(isWeixin){
	                // 微信分享
	                wx.ready(function () {
	                    wx.onMenuShareTimeline({
	                        title: '重磅福利！省兜兜红包天天送', // 分享标题
	                        desc: '', // 分享描述
	                        link: location.origin + "/Event/XxxshareEx?val=" + encodeURIComponent(this.$store.state.shorttoken)+"-1-"+this.methods_get_order_id_list(), // 分享链接
	                        imgUrl: 'https://e-shop.rrlsz.com.cn/Assets/Event/img/lve.jpg', // 分享图标
	                        type: '', // 分享类型,music、video或link，不填默认为link
	                        success: function () {
	                            alert('分享成功')
	                        },
	                        cancel: function () {
	                            alert('分享取消')
	                        }
	                    });
	                });
	            }else{
	                var title = encodeURIComponent("呼朋唤友，奖励无限！");
	                var recommend = encodeURIComponent("友谊小船不翻，红包拿到手软。");
	                var pic_id = "35642";
	                var redirect = location.origin + "/Event/XxxshareEx?val=" + encodeURIComponent(this.$store.state.shorttoken)+"-1-"+this.methods_get_order_id_list();
	                if (ua.indexOf('iphone') > -1) {
	                    redirect = encodeURIComponent(redirect);
	                }
	                window.location.href = "/Event/SharingGuide?title=" + title + "&recommend=" + recommend + "&pic_id=" + pic_id + "&redirect=" + redirect;
	            }
        	},
        	//获取需要支付的总豆数量
        	methods_get_need_pay_beans()
        	{
        		var all_need_pay_beans=0;
        		this.OrderList.forEach(item => {
                    all_need_pay_beans += item.need_pay_beans;
                });
                return all_need_pay_beans;
        	},
        	//获取需要支付的总钱数
        	methods_get_need_pay_money()
        	{
        		var all_need_pay_money=0;
        		this.OrderList.forEach(item => {
                    all_need_pay_money += item.need_pay_money;
                });
                return all_need_pay_money;
        	},
        	//获取需要支付的总钱数
        	methods_get_all_pay_cash()
        	{
        		var all_pay_cash=0;
        		this.OrderList.forEach(item => {
                    all_pay_cash += item.all_pay_cash;
                });
                return all_pay_cash;
        	},
        	//获取订单ids  ,分开
        	methods_get_order_id_list()
        	{
        		let  arrId = [];
                this.OrderList.forEach(item => {
                    arrId.push(item.id);
                });
                return  arrId.length > 1 ? arrId.join(",") : arrId.join("");
        	},
        	//获取用户分享折扣金豆
        	methods_get_shared_discount_beans()
        	{
        		return  parseInt(this.methods_get_all_pay_cash()*this.Order_Shared_discount_beans_rate);//豆的比例
        	},
        	//获取用户分享折扣金钱
        	methods_get_shared_discount_cash()
        	{
        		return  parseInt(this.methods_get_all_pay_cash()*this.Order_Shared_discount_money_rate);//豆的比例
        	},
        	//获取用户拥有的金豆数量
        	methods_get_user_hmoney()
        	{
        		return localStorage.getItem("hMoney");
        	},
        	//门槛
        	method_get_hold_money()
        	{
        		return parseInt(this.data_hold_money/10000);
        	},
        	//获取订单支付的状态 1=现金支付,2=30%金豆支付,4=金豆支付
        	methods_get_pay_order_status()
        	{
        		var status=1;//1=现金支付,2=30%金豆支付,4=金豆支付
        		var after_sub_hold_money=this.methods_get_user_hmoney()-this.methods_platform_hold_money();
        		if(after_sub_hold_money>this.methods_get_shared_discount_beans())//金豆大于折扣后的豆
    			{
    				status+=2;
    			}
        		if(after_sub_hold_money>this.methods_get_need_pay_beans())//金豆大于门槛
        		{
        			status+=4;
        		}
        		this.data_include_status=status;
        		return status;
        	},
        	//获取订单分享次数
        	methods_ajax_get_order_shared_times(){
        		var orderlist=this.methods_get_order_id_list() ;
        		if(!orderlist){
        			return;
        		}
        		var this_vm=this;
        		$.ajax({
                    url: '/WebApi/TradeManager/GetOrderSharedTimes?orderlist=' +orderlist +'&token=' + encodeURIComponent(this.$store.state.shorttoken),
                    type: "get",
                    async:false,
                    complete: (xhr, textStatus) => {
                        var res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            return ;
                        }
                        // 成功
                        if (res.status === 0) {
                            this_vm.data_order_shared_times=res.data;
                        }
                    }
                });
        	},
        	//更新
        	methods_ajax_UpdateOrderIsBeansPayV3(beanFlag,discount_type,unshowloading) {
        		var _this=this;
                $.ajax({
                    url: '/WebApi/TradeManager/UpdateOrderIsBeansPayV3?orderlist=' + this.methods_get_order_id_list() + '&is_beans_pay=' + beanFlag + '&discount_type='+discount_type+'&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest, textStatus) => {
                    	if(/*!unshowloading*/false)
                    	{
                    		_this.alertFlag=true;
                    	}
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
			//初始化界面,获取订单以后
			methods_init_ui_after_load_order()
			{
				var status=this.methods_get_pay_order_status();
				this.methods_set_three_payment_type();
				if(this.isWeiXin())//只能用现金支付
				{
					this.methods_ajax_UpdateOrderIsBeansPayV3('0','0');
					this.data_check_status=1;
				    this.payment_current=this.payment_cash;
				}else
				{
					if((status&4)==4)
					{
						this.methods_ajax_UpdateOrderIsBeansPayV3('1','0');
						this.data_check_status=4;
		        		this.payment_current=this.payment_cashInvite;
					} else if((status&2)==2)
					{
						this.methods_ajax_UpdateOrderIsBeansPayV3('1','1');
						this.data_check_status=2;
		        		this.payment_current=this.payment_cashJd;
					}else
					{
						this.methods_ajax_UpdateOrderIsBeansPayV3('0','0');
						this.data_check_status=1;
						this.payment_current=this.payment_cash;
					}
				}
				 
			},
			//修改订单后,改变UI
			methods_init_ui_after_update_order()
			{
				this.methods_set_three_payment_type();
				var status=this.methods_get_pay_order_status();
				if(this.data_check_status==1)
				{
					this.methods_ajax_UpdateOrderIsBeansPayV3('0','0',true);
					this.payment_current=this.payment_cash;
				}else if(this.data_check_status==2)
				{
					if((status&2)==2){
						this.methods_ajax_UpdateOrderIsBeansPayV3('1','1',true);
						this.payment_current=this.payment_cashJd;
					}else
					{
						this.methods_ajax_UpdateOrderIsBeansPayV3('0','0',true);
						this.data_check_status=1;
						this.payment_current=this.payment_cash;
					}
				}else if(this.data_check_status==4)
				{
					if((status&4)==4){
						this.methods_ajax_UpdateOrderIsBeansPayV3('1','0',true);
						this.payment_current=this.payment_cashInvite;
					}else
					{
						if((status&2)==2){
							this.methods_ajax_UpdateOrderIsBeansPayV3('1','1',true);
							this.data_check_status=2;
							this.payment_current=this.payment_cashJd;
						}else
						{
							this.methods_ajax_UpdateOrderIsBeansPayV3('0','0',true);
							this.data_check_status=1;
							this.payment_current=this.payment_cash;
						}
					}
					  
				}
				 
			},
            //勾选协议
            methods_check_payment_style(status){
            	if($('.payment_style_list[data-status="'+status+'"]').hasClass("disable_color"))
            	{
            			return;
                }
            	if(this.isWeiXin())//只能用现金支付
				{
					//this.methods_ajax_UpdateOrderIsBeansPayV3('0','0');
					this.data_check_status=1;
					this.payment_current=this.payment_cash;
					this.setWeixinMsg();
					return;
				}
            	$('.payment_style_list').find('.radio_icon_box').removeClass("selected");
            	$('.payment_style_list[data-status="'+status+'"]').find('.radio_icon_box').addClass("selected");
            	if(status==1)
            	{
            		this.methods_ajax_UpdateOrderIsBeansPayV3('0','0');
            		this.payment_current=this.payment_cash;
            	}else if(status==2)
            	{
            		this.methods_ajax_UpdateOrderIsBeansPayV3('1','1');
            		this.payment_current=this.payment_cashJd;
            	}else if(status==4)
            	{
            		this.methods_ajax_UpdateOrderIsBeansPayV3('1','0');
            		this.payment_current=this.payment_cashInvite;
            	}
            	this.data_check_status=status;
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
        	//
			methods_init_orderlist()//初始化订单数据
        	{
        		this.$store.dispatch('setPreOrderList', this.$route.params.orderList).then(() => {
	                const len = this.$store.state.PreOrderList.length;
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
	                this.methods_ajax_get_order_shared_times();
	                this.motheds_set_is_zero_area();
	                this.totalmoney = this.totalmoney.toFixed(2);
	                this.return_money = this.return_money.toFixed(2);
	                this.methods_init_ui_after_load_order();
	                sessionStorage.setItem('totalcash', this.totalmoney);
	                this.$store.commit('SET_JINBIDISCOUNT', this.discount);
	            });
	
	            /*if(this.isWeiXin()){
	                this.isWeiXinBeansPay();
	            }*/
        	},
            showJdDetail(){
                this.showRule=true;
            },
            hideBtn(){
                this.showRule=false;
            },
        	
        	
        	methods_platform_hold_money(){
        		
            	return parseInt(this.platform_hold_money);
            },
        	//获取门槛
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
             
            // 计算可用金豆
            useComputedBean() {
            	let beans = localStorage.getItem("hMoney")-parseInt(this.platform_hold_money); 
                return beans.toFixed(2); 
            },
            // 是微信端的浏览器默认发送 0
           /* isWeiXinBeansPay(){
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
            },*/
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
				this.data_is_zero_area_goods_type= is_zero_area_goods_type;
				if(this.data_is_zero_area_goods_type)
				{
					this.platform_hold_money=0;
				}
            },
            
            // 调用金豆是否选中可用的接口
            /*beanIsCheckedApi(id, isNum) {
                this.setWeixinMsg();
                // 判断是否是微信浏览器
                if (this.isWeiXin()) return;
				var is_zero_area_goods_type=this.data_is_zero_area_goods_type;
				 
				if(!is_zero_area_goods_type)
				{
					if(this.methods_platform_hold_money()>=10000)
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
            },*/
            // 返回首页
            goback() {
                this.$router.push("https://e-shop.rrlsz.com.cn/#/index");
            },
            //订单创建完成
            makeorder() {
            	
            	if(this.data_check_status==4)
            	{
            		var sub_shared_time=this.data_max_shared_times-this.data_order_shared_times;
            		if(sub_shared_time>0)
            		{
            			alert(`分享人数还差${sub_shared_time}人,抓紧时间分享吧!`);
            			return;
            		}
            	}
            	
                //收货地址判定
                let orderIdListAry = this.$route.params.orderList;
                if (this.$store.state.receive_info && this.$store.state.receive_info.id) {
                    this.$store.dispatch('sendrecivedid').then(() => {
                        let fincash = this.methods_get_finnal_cash();
                        window.location.href = '/payway?list=' + encodeURIComponent(orderIdListAry) + '&token=' + encodeURIComponent(this.$store.state.shorttoken) + '&type=1,2,3' + '&cash=' + fincash + '&wallet=' + this.$store.state.userdiscount + '&jd=' + this.$store.state.goldbean;
                    });
                } else {
                    alert('请填写收货地址')
                }
            },
            //使用折扣判定
           /* usediscount() {
                this.usedis = !this.usedis
            },*/
            chosecoupons(item) {
            	debugger;
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
                                //_this.useBean();
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
                            //_this.useBean();
                            _this.methods_init_ui_after_update_order();
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
            /*clickUseBean() {
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
                    //this.$refs.isBeanBtn.disabled = false;
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
                    //this.$refs.isBeanBtn.disabled = true;
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
            },*/

            // 可用金豆
            /*useBean() {
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
                var is_show_check_div=(this.methods_platform_hold_money()<10000||this.data_is_zero_area_goods_type);
                if (hMoney > isNum && is_show_check_div) {
                    this.isFlagBean = true;
                    //this.$refs.isBeanBtn.disabled = false;
                    _this.isCheckedBean = true;
                    beanFlag = 1;
                } else {
                    // 不够
                    _this.alertFlag = false;
                    this.isCheckedBean = false;
                    this.isFlagBean = false;
                    beanFlag = 0;
                    //this.$refs.isBeanBtn.disabled = true;
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
            },*/

            // 是否选中可用金豆
            /*usableBean() {
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
            },*/

            // 可用代金券
            /*usableVoucher(item, $event) {
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
            }*/
        }
    }

    export default makesureorder
</script>