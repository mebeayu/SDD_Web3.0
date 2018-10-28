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
                 <!-- 2018.8.10新增 begin-->
                    <!-- 没完成的状态 -->
                    <div class="order_invite" v-if='order_invite'  >
                        <ul v-show="data_max_shared_times>0">
                            <li>
                                <p>邀{{data_max_shared_times}}人享优惠: <em>还差 <i>{{methods_get_shared_human_count(item.shared_count)}}</i> 人</em> </p> 
                                <p  class="invite_bottom_p">
                                    <span  class="order_line  order_line_light" v-for="(i,index) of data_max_shared_times-methods_get_shared_human_count(item.shared_count)"  ></span>
                                    <span class="order_line order_line_gray"  v-for="(i,index) of methods_get_shared_human_count(item.shared_count)" ></span>  
                                    
                                </p> 
                            </li>
                            <li>
                                <p  v-show="methods_get_shared_human_count(item.shared_count)>0">{{methods_render_expire_second(item.remain_second)}}</p>
                                <p  v-if="methods_get_shared_human_count(item.shared_count)>0"  class="invite_bottom_p">后失效,快去邀请吧</p>
                                <p  v-else="" class="invite_bottom_p">邀请人数已够,赶紧付款!</p>
                            </li>
                            <li>
                                <button  v-show="methods_get_shared_human_count(item.shared_count)>0" class="invite_go_btn" @click="methods_btn_click_invite_users(item.id)" >去邀请</button>
                            </li>
                        </ul>   
                        <div class="order-goods-btn">
                            <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1"/>
                            <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                        </div>
                    </div>
                  
                    <!-- 已完成的状态 -->
                    <div class="order_invite" v-else>
                        <ul>
                            <li>
                                <p>邀2人享优惠: <strong class="invite_complate">已完成</strong> </p> 
                                <p class="invite_bottom_p"> 
                                    <span  class="order_line  order_line_light" v-for="(i,index) of 3" :key="index"></span>
                                   
                                </p> 
                            </li>

                            <li class="order-goods-btn">
                                <input type="button" value="取消订单" v-on:click="cancelorder(item.id,1)" v-if="item.status==1"/>
                                <input type="button" value="付款" v-if="item.status==1" v-on:click="topay(item.id)"/>
                            </li>
                        </ul>
                    </div>
                <!-- 2018.8.10新增 end-->
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
    /* 2018.8.10新增 begin */
    i{
        font-style: normal;
    }
    .order_invite{
        padding: .3rem 0;
        font-size: .33rem;
    }
    .order_invite ul{
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        -webkit-box-orient: horizontal;
        -webkit-box-direction: normal;
        -ms-flex-direction: row;
        flex-direction: row;
        padding-left: 0.4rem;
        -webkit-box-pack: justify;
        -ms-flex-pack: justify;
        justify-content: space-around;
        -ms-flex-wrap: nowrap;
        flex-wrap: nowrap;
    }
    .invite_go_btn{
        padding: .2rem .4rem;
        color: #fff;
        background: #67c3aa;
        border-radius: .66666667rem;
        font-style: normal;
        min-width: 1.5rem;
        text-align: center;
        /* margin-left: .2rem; */
        white-space: nowrap;
        font-size: .34rem;
        border: none;
    }
    .invite_bottom_p{
        margin-top: .1rem;
         display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        -webkit-box-orient: horizontal;
        -webkit-box-direction: normal;
        -ms-flex-direction: row;
        flex-direction: row;
        -webkit-box-pack: justify;
        -ms-flex-pack: justify;
        justify-content: space-around;
        -ms-flex-wrap: nowrap;
        flex-wrap: nowrap;
    }
    .order_line{
        display: inline-block;
        width: 31%;
        height: .2rem;
        border-radius: .66666667rem;
    }
    .order_line_gray{
        border: 1px solid #65c4aa;
    } 
    .order_line_light{
        background: #65c4aa;
    }
    .invite_complate{
        color: #65c4aa;
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
                alertTilData: ["转卖提交数据中 ...", "一键转卖成功~", "一键转卖出现异常!"],
                order_invite:true,
                currenttime:'-',  //倒计时
                loacalTimer:'',
                need_people:1, //还差多少人
                data_max_shared_times:2//分享次数
            }
        },
        components: {
            viewtitle,
            orderlistcell
        },
        created() {
        	this.data_max_shared_times=window.Order_Shared_people_num;
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
            this.timer();
        },
        methods: {
        	
        	//显示过期时间
        	methods_render_expire_second(remain_second)
        	{
        		if(remain_second<0)
        			return "0时0分0秒";
        		var hour=parseInt(remain_second/3600);
        		var minute= parseInt( ( remain_second-hour*3600)/60);
        		var second=remain_second-hour*3600-minute*60;
        		return `${hour}时${minute}分${second}秒`;
        	},
        	//还差人数
        	methods_get_shared_human_count(shared_count)
        	{
        		return this.data_max_shared_times-shared_count>0?(this.data_max_shared_times-shared_count):0;
        	},
        	
        	//邀请用户
        	methods_btn_click_invite_users(orderid)
        	{
        		var that=this;
	            var ua = navigator.userAgent.toLowerCase();
	            var isWeixin = ua.indexOf('micromessenger') != -1;
	            if(isWeixin){
	                // 微信分享
	                wx.ready(function () {
	                    wx.onMenuShareTimeline({
	                        title: '重磅福利！省兜兜红包天天送', // 分享标题
	                        desc: '', // 分享描述
	                        link: location.origin + "/Event/XxxshareEx?val=" + encodeURIComponent(this.$store.state.shorttoken)+"-1-"+orderid, // 分享链接
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
	                var redirect = location.origin + "/Event/XxxshareEx?val=" + encodeURIComponent(this.$store.state.shorttoken)+"-1-"+orderid;
	                if (ua.indexOf('iphone') > -1) {
	                    redirect = encodeURIComponent(redirect);
	                }
	                window.location.href = "/Event/SharingGuide?title=" + title + "&recommend=" + recommend + "&pic_id=" + pic_id + "&redirect=" + redirect;
	            }
        	},
            //时间戳
            timer:function () {
                const _this=this;
                window.setInterval(()=>{
                _this.formDate( _this.loacalTimer);
                    _this.loacalTimer--;
                    if (_this.loacalTimer < 0) {
                        if (_this.timer != null) {
                            clearInterval(_this.timer);
                        }
                    }
                },1000);
            },
            
            formDate(times){
                var day = 0,
                     hour = 0,
                     minute = 0,
                     second = 0;//时间默认值
                if (times > 0) {
                    day = Math.floor(times / (60 * 60 * 24));
                    hour = Math.floor(times / (60 * 60)) - (day * 24);
                    minute = Math.floor(times / 60) - (day * 24 * 60) - (hour * 60);
                    second = Math.floor(times) - (day * 24 * 60 * 60) - (hour * 60 * 60) - (minute * 60);
                }
                // 以两位数显示时间的各部分
                function FormatDatePart(value) {
                    if (value <= 9) {
                        return '0' + value;
                    }
                    return value;
                }
                this.currenttime = FormatDatePart(hour) + "时" + FormatDatePart(minute) + "分" + FormatDatePart(second) + "秒";
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