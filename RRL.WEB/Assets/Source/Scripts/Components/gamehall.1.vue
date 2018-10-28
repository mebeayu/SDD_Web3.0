<template>
    <div class="wj-gamehall-mainbar">
        <!--头部开始 -->
        <div class="wj-gamehall-topbar">
            <h4>签到</h4>
            <a  @click="share"  href="javascript:;"><img src="/Assets/Game/img/gamehall-share.jpg"/></a>
        </div>
        <!--头部结束-->

        <!--中间主要区域开始-->
        <div class="wj-gamehall-centerbar">
            <!--广告位开始-->

            <div v-if="is_show_game=='1'" id="wj-gamehall-ad" class="wj-gamehall-adbar">
                <div class="wj-center-scrollybar">
                    <div class="wj-top-banner">
                        <van-swipe :autoplay="3000"
                                   :duration="500"
                                   :loop="true"
                                   :show-indicators="true"
                                   :initial-swipe="0"
                        >
                            <van-swipe-item v-for="(item, index) in swiperSlides"
                                            :key="index"
                                            @click.native="goBanner(item.url)"
                            >
                                <img v-lazy="'SWebApi/Public/picture/' + item.pic_id+'.jpg'">
                            </van-swipe-item>
                        </van-swipe>

                    </div>
                </div>
            </div>

            <!--广告位结束-->

            <!--签到区域开始-->
            <div class="wj-gamehall-signbar border-b">
                <!--点击签到开始-->
                <div id="clickSignFn" class="wj-gamehall-click-sign" @click="click_sign_fn">
                    <p>坚持签到，领取大额免费红包</p>
                    <img src="/Assets/Game/img/gamehall-click-sign.jpg" alt="">
                </div>
                <!--点击签到结束-->

                <!--今日已签开始-->
                <div id="btnSignIn"  @click="sign_in" class="wj-gamehall-today-sign cur1" v-if="sign">签到领红包</div>
                <div class="wj-gamehall-today-sign cur1"  v-else >今日已签，明日再来</div>

                <div class="wj-gamehall-checked-list-bg">
                    <p id="lblContinuousSignDays" class="wj-gamehall-continue">连续{{days}}天</p>
                        <ul class="wj-gamehall-checked-list">

                            <template v-for="(item,index) in items">
                                <li>
                                    <i  v-if="item.is_signin=='1'" ></i>
                                    <i  v-else  class="noChecked" ></i>
                                    <span>{{item.money}}万</span>
                                </li>
                                <li class="line cur1"></li>
                            </template>

                        </ul>
                </div>


                <!--总免费红包区域开始-->
                <div class="wj-total-free-red-envelopes-bar">
                    <div class="wj-total-free-red-envelopes-l">
                        <div class="wj-total-free-red-envelopes-pic">
                            <img src="/Assets/Game/img/gamehall-red-envelopes.png" alt=""/>
                            <span>总免费红包</span>
                            <span id="el_h_money_free">{{hbCount}}</span>
                            <span>个</span>
                        </div>
                        <div class="wj-total-free-red-envelopes-time">
                            清零倒计时：<span id="lblCountdown">{{Countdown}}</span>
                        </div>
                    </div>

                    <div class="wj-total-free-red-envelopes-r">
                        <ul>
                            <li id="el_change_card" @click="change_card"><a href="javascript:;"><span>免费红包兑卡券</span><span>（10w起兑）</span></a>
                            </li>
                        </ul>
                    </div>
                </div>
                <!--总免费红包区域结束-->

                <!--今日已签开始-->
            </div>
            <!--签到区域结束-->

            <!--兜兜运动区域开始-->
            <div class="border-b" style="display:none ">
                <h6 class="wj-gamehall-headtop">一步一红包 好货随你挑</h6>
                <div class="wj-dd-motion-list">
                    <ul>
                        <li>
                            <img src="/Assets/Game/img/img@3x-min.png" alt=""/>
                        </li>
                        <li>
                            <div>今日行走步数<span id="lblSportStep">{{step}}</span>步</div>
                            <div>今日免费红包<span id="lblSportMoney">{{stephb}}</span>个</div>
                            <div>(今日22:00结算后将自动计入总免费红包)</div>
                        </li>
                        <li>
                            <img id="imgBtnToSport" @click="imgBtnToSport_click" src="/Assets/Game/img/go@3x-min.png" alt=""/>
                        </li>
                    </ul>
                </div>
            </div>
            <!--兜兜运动区域结束-->

            <!--赠品休闲区开始-->
            <div>
                <div class="wj-gamehall-headtop-new" id="continuousSignTips"  @click="continuous_sign_tips">
                    <h6>赠品休闲区</h6>
                    <img src="/Assets/Game/img/wjgamehall_rules@3x-min.png" alt="">
                </div>

                <div  v-if=' is_show_game="1" ' class="wj-gamehall-game-ad" id="gameAdLeisure" @click="goLeisureGamesPage">
                    <img src="/Assets/Game/img/game@3x-min.png" alt=""/>
                </div>

                <div class="wj-gamehall-gift-leisure-bar wj-gamehall-gift-leisure-bar-new">
                    <!--广告大图区域开始-->
                    <div class="wj-gamehall-tip-img-top">
                        <a href="javascript:;">
                            <img src="/Assets/Game/img/wjgamehall_tip_img@3x-min.png" alt=""/>
                        </a>
                    </div>
                    <!--广告大图区域结束-->

                    <!--签到领小红包区域开始-->
                    <div class="wj-gamehall-sign-in-small-red-bag wj-gamehall-sign-in-small-red-bag-new">
                        <ul>
                            <li>
                                <div><img class="wj-gamehall-leisure-bean-icon"
                                          src="/Assets/Game/img/wjgamehall_gold_bean@3x-min.png" alt=""/>金豆
                                </div>
                                <div id="el_h_money">{{jdCount}}</div>
                                <div>
                                    <input id="buyGoldBean" @click="buy_gold_bean" type="button" value="购券送豆"/>
                                    <input id="goShoppingGameHall"  @click="go_shopping_game_hall" class="goshop" type="button" value="去购物"/>
                                </div>
                            </li>
                            <li>
                                <div><img class="wj-gamehall-leisure-payoff-icon"
                                          src="/Assets/Game/img/wjgamehall_red_payoff@3x-min.png" alt=""/>小红包
                                </div>
                                <div id="el_h_money_pay">{{smallhbCount}}</div>
                                <div id="playGoldBean" @click="goLeisureGamesPage" v-if='is_show_game="1" '>
                                    <img class="play-gold-bean-pic" src="/Assets/Game/img/play_change@3x-min.png" alt=""/>
                                    <!--<input type="button" value="玩一玩变金豆"/>-->
                                </div>
                            </li>
                        </ul>
                    </div>
                    <!--签到领小红包区域结束-->

                    <!--签到得300小红包区域开始-->
                    <div class="first-share-red-collar">
                        <!--<input id="firstShareRedCollar" class="goshop" type="button" @click="share"-->
                               <!--value="推荐得小红包{{firstShareRedCollar}} + {{firstShareJd}}金币"/>-->
                        <button id="firstShareRedCollar" class="goshop" type="button" @click="share">推荐得{{firstShareRedCollar}}小红包 + {{firstShareJd}}个金豆</button>
                    </div>
                    <!--签到得300小红包区域结束-->

                    <!--小红包清零倒计时区域开始-->
                    <div class="wj-gamehall-small-red-envelopes wj-gamehall-small-red-envelopes-new">
                        <div class="wj-gamehall-small-red-envelopes-l">
                            小红包清零倒计时：<span id="smallRedEnvelopesClear">{{currenttime}}</span>
                        </div>
                    </div>
                    <!--小红包清零倒计时区域结束-->
                </div>

            </div>
            <!--赠品休闲区结束-->
        </div>
        <!--中间主要区域结束-->

        <!--分享红包图片区域开始-->
        <div class="hboxfixed" v-show="hbox">
            <div class="shaw"></div>
            <div class="img" id="shareRedCollarImg" @click="share"><img src="/Assets/Source/img/hbox.png"/></div>
        </div>
        <!--分享红包图片区域结束-->

        <!--省兜兜大红包页面开始-->
        <div class='maskxx wj-maskxx'>
            <p class='gethbnum y-color' v-html='gethbnum'></p>
            <img class='closebtnxx' @click="closeRedFree" src='/Assets/Event/img/images/oolin.jpg'/>
        </div>
        <!--省兜兜大红包页面结束-->


        <!--footerbar-->
        <bottom ref="bottomCOM"></bottom>
    </div>

</template>
<script src='https://res.wx.qq.com/open/js/jweixin-1.0.0.js'></script>
<script>
    import  $      from 'jquery'
    import  bottom from './bottom'
    import Vue from 'vue'
    import {Swipe, SwipeItem} from 'vant';
    Vue.use(Swipe).use(SwipeItem);

    var  gamehall= {
        components: {
            bottom
        },
        data(){
            return {
                allList: [],                   // 所有轮播图数据
                swiperSlides: [],              // 顶部主轮播图
                selectList: [],                // 顶部公告
                token: this.$store.state.shorttoken,// 检测是否登陆
                items:[],                     //循环
                hbox:false,
                sign: true,
                Countdown: '-',
                loacalTimer:'',
                days: '0',
                hbCount: '-',
                jdCount: '-',
                smallhbCount: '-',
                firstShareRedCollar: '300',
                firstShareJd: '10',
                stephb:'-',
                step:'-',
                is_show_game:"1",
                currenttime:'-',
                gethbnum:'30'
            }
        },
        created(){
          this.is_show_game=window.is_show_game;
          this.InitSignInData();
          this.GetUserSportInfo();
          this.GetUserInfo();
          this.Banner();
        },
        mounted(){
            this.timer();
            this.loopPage();
        },
        methods:{
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
                this.Countdown= FormatDatePart(day) + "天" + FormatDatePart(hour) + "时" + FormatDatePart(minute) + "分" + FormatDatePart(second) + "秒";
            },
            /**
            * 获取小红包的轮询
            */
            loopPage(){
                    var endtime = new Date().getTime();//用户退出时间
                    var duration = Math.floor((endtime - starttime) / 1000);
                    var rdcode = String(Math.random()).split(".")[1];
                    var starttime = new Date().getTime();
                    var that=this;
                    $.post("/WebApi/Game/SetState",
                    {starttime: starttime,endtime: endtime,duration:duration, rdcode: rdcode, token: this.token},
                    function(res){
                        if(res.data==true){//分时段小红包
                              $.post("/WebApi/Game/RefreshRandom", {token: that.token},function(data){
                                if(data.data !== 0){
                                  
                                    $('.maskxx').css('display', 'block');
                                    that.gethbnum=data.additional_data;
                                    that.GetUserInfo();
                                }       
                              },'json');
                        } ;
                        if(res.additional_data===true){//针对个人发红包
                            $.post("/WebApi/Game/RefreshPer",
                                  {"token":that.token},
                                  function(res){
                                        console.log(res)     
                                        if(res.status == 0){
                                            var elText = "";
                                            if (res.data.FreeMoney.Money > 0) {
                                                elText += res.data.FreeMoney.Msg + "<br />";
                                            }
                                            if (res.data.PayMoney.Money > 0) {
                                                elText += res.data.PayMoney.Msg;
                                            }
                                            $('.maskxx').css('display', 'block');
                                            that.gethbnum=elText;
                                            that.GetUserInfo();
                                        } else {
                                             alert(res.data);
                                        } 
                                 }, 'json');   
                        }   
                    },'json');
                    setTimeout(function(){
                        that.loopPage();
                    }, 2000)     
            },
            closeRedFree:function(){
               $('.maskxx').css('display', 'none');     
            },
            //获取游戏大厅的轮播图数据
            Banner:function(){
                var that=this;
                $.ajax({
                    url: "/Webapi/ConfigManager/MainCarousel",
                    type: "get",
                    success:function (res) {
                        if (res.status === 0) {
                            var dataList = res.data;
                            for (var i = 0; i < dataList.length; i++) {
                                var item = dataList[i];
                                // 判断是游戏的轮播图数据  type: 8
                                 if (item.type == 8) {
                                     that.swiperSlides.push(item);
                                 }
                            }
                        }
                    }
                })
            },
            //点击轮播图跳转指向
            goBanner(url) {
                if (!url) return;
                if (url.indexOf("javascript:") > -1) {
                    eval(url.replace("javascript:", ""));
                    return;
                }
                window.location.href = url.replace("#token", this.token);
            },
            /**
             * 初始化签到数据
             */
            InitSignInData:function () {
                if(!this.token){
                  return;
                }else{
                    var that=this;
                    $.get('/WebApi/game/InitSignInData',{token:this.token},function (res) {
                        if (res.status == 0) {
                            let data=res.data;
                            //连续签到多少天
                            that.days=data.continuousSignDays;
                            // 推荐得小红包数和金豆数
                            that.firstShareRedCollar=data.shareRedpackage;
                            that.firstShareJd=data.shareGoldCoins;
                            that.loacalTimer=data.countdown;
                            if(data.isCanGetRedpackage==='1'){
                                that.sign=true;
                            }else{
                                that.sign=false;
                            }
                            if(that.items.length>0){
                                that.items=[]
                            }
                            var redpackageDay=data.redpackageDayConfig;
                            for(let i in redpackageDay){
                                var redpackageIndex=redpackageDay[i];
                                that.items.push(redpackageIndex);
                            }
                        }
                    })
                }
            },
            /**
             * 获取用户运动信息
             */
             GetUserSportInfo:function() {
                if(!this.token){
                    return;
                }
                let that=this;
                $.get('/WebApi/Sports/UserSportData', { token:that.token }, function (res) {
                    if (res.status == 0) {
                        that.step=res.data.step;
                        that.stephb=res.data.awardMoney;
                    }
                })
            },
            /**
             * 签到领红包
             */
            sign_in:function () {
                let that=this;
                if(!this.token){
                    confirm('尚未登录,是否登录？',function () {
                        that.$router.push({path:'/login?redirect=' + encodeURIComponent("/#/gamehall?token=#token")})
                    })
                }  else{
                    $.get('/WebApi/game/SignInRedpackage',{token:this.token},function (res) {
                        if(res.status==0){
                            that.InitSignInData();
                            that.GetUserInfo();
                        }
                        if(res.status==99){
                            alert(res.message)
                        }
                    })
                }
            },
            /**
             * 跳转至休闲游戏区
             */
            goLeisureGamesPage:function () {
                window.location.href = "/Game/LeisureGameArea?token=" + encodeURIComponent(this.token);
            },
            /**
             * 赠品休闲区小红包使用情况
             */
            continuous_sign_tips:function () {
                confirm("<ul><li>小红包使用规则</li><li>1、赠品休闲区可以获得小红包，每日陆续发放3次；</li><li>2、当日领取小红包将在24:00准时清零，请尽快使用；</li><li>3、小红包可在赠品休闲区内使用转换为金豆，金豆可在购物时抵扣使用；</li><li>4、所有用户红包均为省兜兜平台免费发放给用户的福利，不能通过充值获取也不可用于交易，本活动最终解释权归省兜兜所有。</li></ul>", function () {
                });
            },
            /**
             * 点击购买金豆
             */
            buy_gold_bean:function () {
                window.location.href = "/Game/PeasPay?token=" + encodeURIComponent(this.token) + "&redirect=" + encodeURIComponent(location.href);
            },
            /**
             * 换购物卡
             */
            change_card:function () {
                let  that=this;
                if(!this.token){
                    confirm('尚未登录,是否登录？',function () {
                        that.$router.push({path:'/login'})
                    })
                }else{
                    window.confirm("是否确定把所有免费红包转换成购物券?", function () {
                        $.post("/WebApi/Game/RedPackageToCoupons", {token:that.token}, function (data) {
                            if (data.status == 0) {
                                alert("免费红包转购物券成功");
                                that.GetUserInfo();
                            } else {
                                alert(data.message);
                            }
                        });
                    });
                }
            },
            /**
             * 获取用户信息
             */
            GetUserInfo:function() {
                var  that=this;
                if(!that.token){
                    return;
                }
                $.post("/WebApi/Game/GetUserInfo", {token: this.token},   (res)=> {
                    if(res.status==0){
                        let data=res.data;
                        that.hbCount=data.h_money_free;
                        that.jdCount=data.h_money;
                        that.smallhbCount=data.h_money_pay;
                        window.h_money_pay = data.h_money_pay;
                        that.$refs.bottomCOM.showRedSup(); // call子组件方法
                    }
                });
            },
            /**
             * 点击签到是否显示弹出框文字
             */
            click_sign_fn:function () {
                confirm("<ul><li>免费红包使用规则</li><li>1、省兜兜幸运签到7日一个循环，每日可领取免费红包金额为1万、3万、6万、8万、12万、15万、25万，如出现断签，再次续签后将与断签前保持连续性；</li><li>2、签到期间，每日首次登陆省兜兜均可进行签到领取免费红包，免费红包可以手动兑换卡券，10万免费红包起兑；</li><li>3、幸运签到日截止时，没有兑换的免费红包将在签到日最后一天24：00由系统自动兑换为购物卡券，请及时兑换；</li><li>4、免费红包将不可用于平台小游戏也不可转换为金豆；</li><li>5、所有用户红包均为省兜兜平台免费发放给用户的福利，不能通过充值获取也不可用于交易，本活动最终解释权归省兜兜所有。</li></ul>", function () {
                });
            },
            /**
             * 点击分享领取红包
             */
            share:function () {
                var ua = navigator.userAgent.toLowerCase();
                var isWeixin = ua.indexOf('micromessenger') != -1;
                if(isWeixin){
                    this.hbox=!this.hbox;
                    
                    // 微信分享
                    wx.ready(function () {
                        wx.onMenuShareTimeline({
                            title: '重磅福利！省兜兜红包天天送', // 分享标题
                            desc: '', // 分享描述
                            link: window.location.href, // 分享链接
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
                    var redirect = "https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=" + encodeURIComponent(this.token);
                    if (ua.indexOf('iphone') > -1) {
                        redirect = encodeURIComponent(redirect);
                    }
                    window.location.href = "https://e-shop.rrlsz.com.cn/Event/SharingGuide?title=" + title + "&recommend=" + recommend + "&pic_id=" + pic_id + "&redirect=" + redirect;

                }
            },
            /**
             * 导航到兜兜运动首页
             */
            imgBtnToSport_click:function () {
                if(!this.token){
                    return;
                }
                var isAndroid = window.navigator.appVersion.match(/android/gi);
                if (isAndroid) {
                    alert('Android暂不支持运动数据！');
                    return;
                }

                location.href = "/Sport/Index2?rundata=enabled&token=" + this.token;
            },
            /**
             * 跳转至分类页面
             */
             go_shopping_game_hall:function() {
                    location.href = "/#/sortofgoods";
                }
             }
      };
    export default  gamehall;
</script>
<style scoped>
    * {
  margin: 0;
  padding: 0;
  box-sizing: content-box;
}
img {
  border: none;
  display: block;
  width: 100%;
  height: 100%;
}
ul,
ol,
li {
  list-style: none;
}
a {
  text-decoration: none;
}
.swipslider {
  position: relative;
  overflow: hidden;
  display: block;
  padding-top: 60%;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.swipslider .sw-slides {
  display: block;
  padding: 0;
  list-style: none;
  width: 100%;
  height: 100%;
  white-space: nowrap;
  font-size: 0;
  -webkit-transform: translateX(0);
  -ms-transform: translateX(0);
      transform: translateX(0);
  position: absolute;
  bottom: 0;
}
.swipslider .sw-slide {
  width: 100%;
  height: 100%;
  margin: auto;
  display: inline-block;
  position: relative;
}
.swipslider .sw-slide > img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
      transform: translate(-50%, -50%);
  max-height: 100%;
  max-width: 100%;
  margin-left: auto;
  margin-right: auto;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.swipslider .sw-slide .sw-content {
  width: 100%;
  height: 100%;
  margin-left: 0;
  margin-right: 0;
  font-size: 14px;
}
.sw-next-prev {
  font-family: "Courier New", Courier, monospace;
  height: 50px;
  width: 50px;
  text-align: center;
  vertical-align: middle;
  position: absolute;
  line-height: 50px;
  font-size: 30px;
  font-weight: bolder;
  color: rgba(160, 160, 160, 0.53);
  top: 50%;
  -webkit-transform: translateY(-50%);
  -ms-transform: translateY(-50%);
      transform: translateY(-50%);
  background-color: rgba(255, 255, 255, 0);
  border-radius: 50%;
  text-decoration: none;
  -webkit-transition: all 0.2s ease-out;
          transition: all 0.2s ease-out;
  cursor: pointer;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.sw-next-prev:hover {
  background-color: rgba(255, 255, 255, 0.74);
}
.sw-next-prev:active {
  background-color: rgba(255, 255, 255, 0.5);
}
.sw-prev {
  left: 2%;
}
.sw-prev::after {
  content: '<';
}
.sw-next {
  right: 2%;
  display: none;
}
.sw-next::after {
  content: '>';
}
.sw-bullet {
  position: absolute;
  bottom: 2%;
  list-style: none;
  display: block;
  width: 100%;
  text-align: center;
  padding: 0;
  margin: 0;
}
.sw-bullet li {
  width: 10px;
  height: 10px;
  background-color: rgba(160, 160, 160, 0.53);
  border-radius: 50%;
  display: inline-block;
  cursor: pointer;
  -webkit-transition: all 0.2s ease-out;
          transition: all 0.2s ease-out;
}
.sw-bullet li:hover {
  background-color: rgba(255, 255, 255, 0.74);
}
.sw-bullet li.active {
  background-color: rgba(255, 255, 255, 0.5);
  box-shadow: 0 0 2px rgba(160, 160, 160, 0.53);
}
.sw-bullet li:not(:last-child) {
  margin-right: 5px;
}
.y-center {
  position: absolute;
  top: 50%;
  -webkit-transform: translateY(-50%);
      -ms-transform: translateY(-50%);
          transform: translateY(-50%);
}
.border-b {
  border-bottom: 0.22666667rem solid #f3f4f5;
}
html,
body,
.wj-gamehall-mainbar {
  width: 100%;
  height: 100%;
  font-family: "Microsoft YaHei", "Helvetica Neue", Helvetica, Arial, sans-serif;
  overflow: hidden;
}
.wj-gamehall-mainbar {
  /*max-width: 640px;*/
  margin: 0 auto;
}
.wj-gamehall-centerbar {
  position: absolute;
  top: 1.17333333rem;
  bottom: 1.30666667rem;
  width: 100%;
  overflow-y: scroll;
}
.wj-gamehall-centerbar-new {
  bottom: 0;
}
.wj-gamehall-botnav {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 1.30666667rem;
  background: #fff;
}
.wj-gamehall-topbar {
  position: relative;
  height: 1.17333333rem;
  line-height: 1.17333333rem;
  font-size: 0.4rem;
  border-bottom: 0.01333333rem solid #e0e7eb;
}
.wj-gamehall-topbar h4 {
  font-weight: normal;
  text-align: center;
  color: #2d2d34;
}
.wj-gamehall-topbar a {
  position: absolute;
  top: 50%;
  -webkit-transform: translateY(-50%);
      -ms-transform: translateY(-50%);
          transform: translateY(-50%);
  right: 0.4rem;
  width: 0.53333333rem;
  height: 0.53333333rem;
}
.wj-gamehall-adbar {
  height: 3.46666667rem;
  background: #eee;
  overflow: hidden;
}
.wj-gamehall-click-sign {
  position: relative;
  padding: 0.52rem 0;
  cursor: pointer;
}
.wj-gamehall-click-sign p {
  color: #7a7b87;
  font-size: 0.4rem;
  font-weight: 500;
  text-align: center;
  cursor: pointer;
}
.wj-gamehall-click-sign img {
  width: 0.45333333rem;
  height: 0.45333333rem;
  position: absolute;
  top: 50%;
  -webkit-transform: translateY(-50%);
      -ms-transform: translateY(-50%);
          transform: translateY(-50%);
  right: 0.4rem;
  cursor: pointer;
}
.wj-gamehall-today-sign {
  text-align: center;
  width: 4.66666667rem;
  height: 1.06666667rem;
  line-height: 1.06666667rem;
  border: 0.09333333rem solid #f5f6fa;
  color: #fff;
  background: #d3d8df;
  margin: 0 auto;
  font-size: 0.4rem;
  border-radius: 2rem;
  cursor: pointer;
}
.wj-gamehall-today-sign.cur1 {
  border-color: #fdeaed;
  background: #fb8394;
}
.wj-gamehall-continue {
  text-align: center;
  color: #fb8394;
  font-size: 0.37333333rem;
  padding: 0.14666667rem 0 0.8rem;
  cursor: pointer;
}
.wj-gamehall-checked-list-bg {
  border-bottom: 0.01333333rem solid #dfe5e9;
  background: url("/Assets/Game/img/gamehall-saign-bg.png") no-repeat;
  background-size: cover;
}
.wj-gamehall-checked-list {
  padding: 0 0.4rem;
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  overflow: hidden;
}
.wj-gamehall-checked-list li {
  text-align: center
  /*float: left;
    width: 14.285714285714286%;
    margin: auto;
    text-align: center;*/
}
.wj-gamehall-checked-list li img {
  position: relative;
  width: 0.66666667rem;
  height: 0.66666667rem;
  margin: auto;
  z-index: 2;
}
.wj-gamehall-checked-list li.cur1 span {
  color: #65c4aa;
}
.wj-gamehall-checked-list li.line {
  position: relative;
  top: .3rem;
  z-index: 0;
  width: 50%;
  background: #eaeff6;
  height: 0.05333333rem;
}
.wj-gamehall-checked-list li.line.cur1 {
  background: #c7f3e7;
}
.wj-gamehall-checked-list li:nth-of-type(1)::after {
  display: none;
}
.wj-gamehall-checked-list li i {
  position: relative;
  z-index: 2;
  display: block;
  width: 0.66666667rem;
  height: 0.66666667rem;
  background: url("/Assets/Game/img/gamehall-yes-checked.png") no-repeat;
  background-size: cover;
  overflow: hidden;
}
.wj-gamehall-checked-list li i.noChecked {
  background-image: url("/Assets/Game/img/gamehall-no-checked.png");
}
.wj-gamehall-checked-list li span {
  display: block;
  padding: 0.18666667rem 0 0.64rem;
  color: #96a4b5;
  font-size: 0.28rem;
}
.wj-gamehall-free-red-envelopes {
  padding: 0.4rem 0.4rem 0;
  overflow: hidden;
}
.wj-gamehall-free-red-envelopes img {
  float: left;
  width: 0.29333333rem;
  height: 0.34666667rem;
}
.wj-gamehall-free-red-envelopes span {
  float: left;
  margin-top: -0.04rem;
}
.wj-gamehall-free-red-envelopes span:nth-of-type(1) {
  font-size: 0.29333333rem;
  color: #7a7b87;
  padding: 0 0.13333333rem 0 0.10666667rem;
}
.wj-gamehall-free-red-envelopes span:nth-of-type(2) {
  font-size: 0.29333333rem;
  color: #e50008;
}
.wj-gamehall-card-voucher li {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  padding: 0.26666667rem 0.4rem 0.4rem;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  overflow: hidden;
}
.wj-gamehall-card-voucher li:nth-of-type(1) {
  border-bottom: 0.01333333rem solid #e3e4e8;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l,
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r {
  float: left;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l {
  position: relative;
  width: 3.22666667rem;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l::after {
  position: absolute;
  right: 0;
  top: 0;
  content: "";
  width: 0.01333333rem;
  height: 100%;
  background: #e3e4e8;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l span {
  display: block;
  font-size: 0.30666667rem;
  color: #7a7b87;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l span.time {
  color: #2d2d34;
  font-size: 0.29333333rem;
  padding-top: 0.16rem;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l span img {
  float: left;
  width: 0.32rem;
  height: 0.34666667rem;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l span i {
  float: left;
  margin-top: -0.04rem;
  font-style: normal;
  white-space: nowrap;
  overflow: hidden;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l.imazamox span {
  display: block;
  width: 100%;
  overflow: hidden;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l.imazamox span:nth-of-type(1) {
  color: #fd762d;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l.imazamox span:nth-of-type(1) img {
  padding-right: 0.22666667rem;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-l.imazamox span:nth-of-type(2) {
  padding-left: 0.54666667rem;
  color: #7a7b87;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r {
  padding-left: 0.46666667rem;
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
  -webkit-box-flex: 1;
  -webkit-flex: 1;
      -ms-flex: 1;
          flex: 1;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r.buy-gold-bean {
  cursor: pointer;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r.buy-gold-bean span {
  color: #fff;
  font-size: 0.28rem;
  border-radius: 0.66666667rem;
  white-space: nowrap;
  height: 0.72rem;
  line-height: 0.72rem;
  text-align: center;
  cursor: pointer;
  overflow: hidden;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r.buy-gold-bean span:nth-of-type(1) {
  width: 2.88rem;
  background: #bdd56a
  /*visibility: hidden;*/
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r.buy-gold-bean span:nth-of-type(2) {
  width: 2.45333333rem;
  background: #65c4aa;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r span {
  float: left;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r span:nth-of-type(2) {
  float: right;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r span.free-red-envelp {
  position: relative;
  color: #fff;
  width: 2.66666667rem;
  height: 0.93333333rem;
  text-align: center;
  font-size: 0.28rem;
  overflow: hidden;
  background: #d3d8df;
  border-radius: 0.66666667rem;
  cursor: pointer;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r span.free-red-envelp i {
  position: absolute;
  top: 50%;
  left: 50%;
  width: 100%;
  -webkit-transform: translate(-50%, -50%);
      -ms-transform: translate(-50%, -50%);
          transform: translate(-50%, -50%);
  font-style: normal;
  white-space: nowrap;
  cursor: pointer;
  overflow: hidden;
}
.wj-gamehall-card-voucher li .wj-gamehall-card-voucher-r span.cur1 {
  background: #65c4aa;
}
.wj-gamehall-game-til {
  padding: 0.4rem;
  font-size: 0.34666667rem;
  color: #444;
  font-weight: normal;
  border-bottom: 0.01333333rem solid #e3e4e8;
}
.wj-gamehall-game-til-new {
  font-size: 0.37333333rem;
  color: #2d2d34;
  padding: 0.4rem;
  font-weight: 700;
}
.wj-gamehall-everyone-playing {
  padding: 0 0.4rem;
  margin: 0.4rem 0;
}
.wj-gamehall-everyone-playing .sw-bullet {
  bottom: 0;
}
.wj-gamehall-everyone-playing .sw-bullet li.active {
  background: #65c4aa;
}
.wj-gamehall-everyone-playing .wj-gamehall-everyone-playing-list {
  /*display: flex;
    flex-direction: row;
    align-items: center;
    flex-wrap: wrap;
    justify-content: space-between;*/
  overflow: hidden;
}
.wj-gamehall-everyone-playing .wj-gamehall-everyone-playing-li {
  text-align: center;
  width: 21%;
  float: left;
  margin: 0 2%;
  overflow: hidden
  /*display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    text-align: center;
    width: 20%;
    margin: 0 1%;
    overflow: hidden;*/
}
.wj-gamehall-everyone-playing .wj-gamehall-everyone-playing-li:nth-child(n+5) {
  margin-top: 0.26666667rem;
}
.wj-gamehall-everyone-playing .wj-gamehall-everyone-playing-li p {
  -ms-text-overflow: ellipsis;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: 0.28rem;
  padding-top: 0.2rem;
  color: #555;
  overflow: hidden;
}
.wj-bottom-nav ul {
  -ms-flex-direction: row;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
          flex-direction: row;
}
.wj-bottom-nav li,
.wj-bottom-nav ul {
  display: -ms-flexbox;
  display: -webkit-box;
  display: -webkit-flex;
  display: flex;
}
.wj-bottom-nav li {
  -ms-flex: 1;
  -webkit-box-flex: 1;
  -webkit-flex: 1;
          flex: 1;
  -ms-flex-direction: column;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -webkit-flex-direction: column;
          flex-direction: column;
  -ms-flex-pack: center;
  -webkit-box-pack: center;
  -webkit-justify-content: center;
          justify-content: center;
  -ms-flex-align: center;
  -webkit-box-align: center;
  -webkit-align-items: center;
          align-items: center;
  -ms-flex-line-pack: center;
  -webkit-align-content: center;
          align-content: center;
  height: 1.306667rem;
  border-top: .013333rem solid #dadada;
  box-sizing: border-box;
}
.wj-bottom-nav li.cur1 span {
  color: #42b983;
}
.wj-bottom-nav li img {
  display: block;
  width: .533333rem;
  height: .533333rem;
}
.wj-bottom-nav li img:nth-of-type(2) {
  display: none;
}
.wj-bottom-nav li.cur1 img:nth-of-type(2) {
  display: block;
}
.wj-bottom-nav li.cur1 img:nth-of-type(1) {
  display: none;
}
.wj-bottom-nav li span {
  font-size: .266667rem;
  padding-top: .133333rem;
  color: #9e9fa9;
}
.wj-bottom-navbar {
  bottom: 0;
}
.wj-bottom-navbar {
  position: absolute;
  left: 0;
  width: 100%;
  background: #ffffff;
}
.buy-card {
  display: none;
}
#shadow1 .tiplogout {
  width: 86% !important;
  top: 3rem !important;
  font-size: .34rem!important;
}
#shadow1 .tiplogout ul {
  padding: 0 0.3rem;
}
#shadow1 .tiplogout li {
  text-align: left;
  line-height: .48rem;
  padding-top: 0.2rem;
}
#shadow1 .tiplogout li:first-child {
  padding-top: 0;
  font-size: .38rem;
  font-weight: 600;
  text-align: center;
}
.wj-maskxx {
  display: none;
}
.wj-total-free-red-envelopes-bar {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
  padding: 0.4rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-pic {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-pic img {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  width: 0.29333333rem;
  height: 0.34666667rem;
  position: relative;
  top: 0.02666667rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-pic span {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  color: #7a7b87;
  font-size: 0.32rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-pic span:nth-of-type(1) {
  padding-left: 0.26666667rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-pic span:nth-of-type(2) {
  color: #fb8394;
  font-size: 0.50666667rem;
  font-weight: 700;
  padding: 0 0.16rem 0 0.29333333rem;
  position: relative;
  top: -0.16rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-time {
  color: #b1b1b9;
  font-size: 0.32rem;
  padding-top: 0.10666667rem;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-r li {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  padding: 0.13333333rem 0.4rem;
  border-radius: 0.53333333rem;
  background: #67c3aa;
}
.wj-total-free-red-envelopes-bar .wj-total-free-red-envelopes-r li span {
  display: block;
  font-size: 0.28rem;
  text-align: center;
  color: #fff;
}
.wj-gamehall-headtop {
  font-size: 0.37333333rem;
  color: #2d2d34;
  padding: 0.4rem;
}
.wj-dd-motion-list {
  border-top: 0.01333333rem solid #e3e4e8;
  padding: 0.4rem 0;
}
.wj-dd-motion-list ul {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
  padding: 0 0.4rem;
}
.wj-dd-motion-list ul li {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
}
.wj-dd-motion-list ul li:nth-of-type(1) {
  padding-right: 0.4rem;
}
.wj-dd-motion-list ul li:nth-of-type(1) img {
  display: block;
  width: 1.89333333rem;
  height: 1.89333333rem;
}
.wj-dd-motion-list ul li:nth-of-type(2) {
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -webkit-flex-direction: column;
      -ms-flex-direction: column;
          flex-direction: column;
  width: 5.46666667rem;
}
.wj-dd-motion-list ul li:nth-of-type(2) div {
  font-size: 0.32rem;
  color: #7a7b87;
  -ms-text-overflow: ellipsis;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
.wj-dd-motion-list ul li:nth-of-type(2) div:nth-of-type(1) span {
  padding: 0 0.17333333rem 0 0.26666667rem;
  color: #098a8e;
  font-size: 0.48rem;
  font-weight: 700;
}
.wj-dd-motion-list ul li:nth-of-type(2) div:nth-of-type(2) {
  padding: 0.24rem 0;
}
.wj-dd-motion-list ul li:nth-of-type(2) div:nth-of-type(2) span {
  padding: 0 0.17333333rem 0 0.26666667rem;
  color: #fb8394;
}
.wj-dd-motion-list ul li:nth-of-type(2) div:nth-of-type(3) {
  font-size: 0.28rem;
  color: #b1b1b9;
}
.wj-dd-motion-list ul li:nth-of-type(3) img {
  display: block;
  width: 1.12rem;
  height: 1.12rem;
}
.wj-gamehall-gift-leisure-bar {
  height: 5.86666667rem;
  padding: 0.4rem 0;
  background: #5acaaa;
  background: -webkit-linear-gradient(left top, #41ecab, #5acaaa);
  /* Safari 5.1 - 6.0 */
  /* Opera 11.1 - 12.0 */
  /* Firefox 3.6 - 15 */
  background: -webkit-linear-gradient(top left, #41ecab, #5acaaa);
  background: linear-gradient(to bottom right, #41ecab, #5acaaa)
  /* 标准的语法 */
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  padding: 0.4rem 0.4rem 0.85333333rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul li {
  width: 14.28571429%;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul li span {
  display: block;
  text-align: center;
  color: #79f6cd;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul li span:nth-of-type(1) {
  font-size: 0.24rem;
  height: 0.24rem;
  padding-bottom: 0.4rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul li span:nth-of-type(2) {
  font-size: 0.4rem;
  font-weight: 700;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-gift-leisure-sign ul li.cur1 span {
  color: #fff;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag {
  padding: 0 0.4rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div,
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  font-size: 0.32rem;
  color: #fff;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div:nth-of-type(2),
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div:nth-of-type(2) {
  font-size: 0.58666667rem;
  font-weight: 700;
  padding: 0.33333333rem 0 0.66666667rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div input,
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div input {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  border: .03rem solid #fff;
  border-radius: 0.66666667rem;
  color: #fff;
  background: transparent;
  padding: 0.2rem;
  font-size: 0.28rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div input.goshop,
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div input.goshop {
  color: #27a27f;
  border: none;
  margin-left: 0.13333333rem;
  background: #9ff9e1;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(3) div:nth-of-type(1) {
  width: 2.6rem;
  height: 1.14666667rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(3) div:nth-of-type(2) {
  padding-top: 0.34666667rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(3) button {
  display: block;
  border: none;
  outline: none;
  width: 100%;
  height: 100%;
  background: url("../../../Game/img/Red_envelopes_on@3x-min.png") no-repeat;
  background-size: cover;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(3) input {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  border: .03rem solid #fff;
  border-radius: 0.66666667rem;
  color: #fff;
  background: transparent;
  padding: 0.2rem;
  font-size: 0.28rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(3) input.goshop {
  color: #27a27f;
  border: none;
  background: #9ff9e1;
  padding: .27rem;
}
.wj-gamehall-gift-leisure-bar img.play-gold-bean-pic {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  width: 2.34666667rem;
  height: 0.88rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes {
  padding: 0.66666667rem 0.4rem 0;
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes span,
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes i {
  font-style: normal;
  font-size: 0.28rem;
  color: #fff;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes .wj-gamehall-small-red-envelopes-l {
  color: #79f6cd;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes .wj-gamehall-small-red-envelopes-r {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes .wj-gamehall-small-red-envelopes-r span {
  padding-right: 0.32rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes .wj-gamehall-small-red-envelopes-r img {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  position: relative;
  top: -0.02666667rem;
  width: 0.45333333rem;
  height: 0.45333333rem;
}
.wj-gamehall-game-ad {
  padding: 0.4rem;
}
.wj-gamehall-game-ad img {
  display: block;
  width: 100%;
  height: 100%;
}
.wj-gamehall-topbar a.wj-back {
  position: absolute;
  left: 0;
  top: 0;
  width: 1.17333333rem;
  height: 1.17333333rem;
  -webkit-transform: translateY(0);
      -ms-transform: translateY(0);
          transform: translateY(0);
}
.wj-gamehall-topbar a.wj-back img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
      -ms-transform: translate(-50%, -50%);
          transform: translate(-50%, -50%);
  width: 0.53333333rem;
  height: 0.53333333rem;
}
.wj-gamehall-leisure-total li {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-flex-wrap: nowrap;
      -ms-flex-wrap: nowrap;
          flex-wrap: nowrap;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
  border-top: 0.01333333rem solid #eeeeee;
}
.wj-gamehall-leisure-total li span {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  width: 33.33333333%;
  padding: 0.4rem;
  font-size: 0.32rem;
  color: #666;
}
.wj-gamehall-leisure-total li span .buy-gold-bean-btn {
  padding: 0.13333333rem 0.26666667rem;
  color: #67c3aa;
  border: 0.01333333rem solid #67c3aa;
  border-radius: 0.66666667rem;
  font-style: normal;
}
.wj-gamehall-leisure-list ul {
  padding: 0.4rem;
}
.wj-gamehall-leisure-list ul li {
  padding-bottom: 0.21333333rem;
}
.wj-gamehall-leisure-list ul li:last-child {
  padding-bottom: 0;
}
.wj-gamehall-leisure-list ul li .wj-gamehall-leisure-list-wrap {
  padding: 0.64rem 1.46666667rem 0.74666667rem;
  background: #c4d97c;
  color: #fff;
  font-size: 0.4rem;
  border-radius: 0.05333333rem;
}
.wj-gamehall-leisure-list ul li .wj-gamehall-leisure-list-wrap div {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
  padding-bottom: 0.26666667rem;
}
.wj-gamehall-leisure-list ul li .wj-gamehall-leisure-list-wrap div img {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  width: 0.74666667rem;
  height: 0.74666667rem;
}
.wj-gamehall-leisure-list ul li .wj-gamehall-leisure-list-wrap div p {
  padding-left: 0.16rem;
  font-weight: 600;
}
.wj-gamehall-leisure-list ul li .wj-gamehall-leisure-list-wrap p {
  -ms-text-overflow: ellipsis;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
.wj-gamehall-tip-img-top {
  padding: 0 0.32rem 0.48rem;
}
.wj-gamehall-gift-leisure-bar-new {
  height: auto;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag-new {
  position: relative;
  padding: 0 1.17333333rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag-new::after {
  position: absolute;
  content: "";
  bottom: 0;
  left: 0;
  width: 100%;
  height: 0.02rem;
  background: #80f5ca;
}
.wj-gamehall-headtop-new {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
      -ms-flex-direction: row;
          flex-direction: row;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
      -ms-flex-pack: justify;
          justify-content: space-between;
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
  padding: 0.4rem;
  border-bottom: 0.01333333rem solid #dadada;
}
.wj-gamehall-headtop-new h6 {
  font-size: 0.37333333rem;
  color: #2d2d34;
}
.wj-gamehall-headtop-new img {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  width: 0.45333333rem;
  height: 0.45333333rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-small-red-envelopes-new {
  padding: 0.48rem 0 0 0.4rem;
}
.wj-gamehall-sign-in-small-red-bag-new li {
  width: 50%;
  padding-bottom: 0.53333333rem;
}
.wj-gamehall-sign-in-small-red-bag-new li:nth-of-type(1) {
  position: relative;
}
.wj-gamehall-sign-in-small-red-bag-new li:nth-of-type(1)::after {
  position: absolute;
  top: 0;
  right: 0;
  content: "";
  width: 0.02rem;
  height: 100%;
  background: #7ff5ca;
  background: -webkit-linear-gradient(transparent, #7ff5ca);
  background: linear-gradient(transparent, #7ff5ca);
}
.first-share-red-collar {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: center;
  -webkit-justify-content: center;
      -ms-flex-pack: center;
          justify-content: center;
  padding-top: 0.57333333rem;
}
.first-share-red-collar .goshop {
  display: block;
  width: 4.8rem;
  height: 0.8rem;
  font-size: 0.29333333rem;
  color: #27A27F;
  border-radius: 0.8rem !important;
  background: #fff;
  border: none;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div,
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div {
  -webkit-box-pack: center;
  -webkit-justify-content: center;
      -ms-flex-pack: center;
          justify-content: center;
}
.wj-gamehall-leisure-bean-icon,
.wj-gamehall-leisure-payoff-icon {
  width: 0.29333333rem;
  padding-right: 0.13333333rem;
}
.wj-gamehall-leisure-payoff-icon {
  height: 0.34666667rem;
}
.wj-gamehall-leisure-bean-icon {
  height: 0.30666667rem;
}
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(1) div,
.wj-gamehall-gift-leisure-bar .wj-gamehall-sign-in-small-red-bag ul li:nth-of-type(2) div {
  -webkit-box-align: center;
  -webkit-align-items: center;
      -ms-flex-align: center;
          align-items: center;
}

    .wj-gamehall-checked-list .line:last-child {display: none;}
    
    .maskxx .closebtnxx{
    	 width: 65%;
   		 height: 1.1rem;
    }
    .closebtnoo, .closebtnxx {
	    position: absolute;
	    bottom: 32%;
	    left: 18%;
	}
</style>
<style>

</style>