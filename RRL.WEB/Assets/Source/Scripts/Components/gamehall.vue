<style lang="scss" scoped>
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
em,strong,i{
  font-style: normal;
}
.p {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
}
.clearfix{
  zoom: 1;
}
.clearfix:after{
  content: "";
  clear: both;
  display: block;
  z-index: 999;
}
html,
body,
.gamehall-mainbar {
  width: 100%;
  height: 100%;
  font-family: "Microsoft YaHei", "Helvetica Neue", Helvetica, Arial, sans-serif;
  overflow: hidden;
  list-style-type: none;
  background: plum;
}

.gamehall-mainbar {
  margin: 0 auto;
}

.border_t{
  border-top: 0.22666667rem solid #f3f4f5;
}
.center_box,.total_v_red_packet{
  padding:0.3rem;color: #7a7b87;
}

.JD_packet_count{
  border-top: 0.01333333rem solid #e0e7eb;
}
.red_packet_count,.JD_packet_count,.v_packet_count{
    display: -webkit-box;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
    -webkit-justify-content: space-between;
    justify-content: space-between;
    -webkit-box-align: center;
    -webkit-align-items: center;
    -ms-flex-align: center;
    align-items: center;
    height: 1.5rem;
}
.red_packet_count span,.JD_packet_count span,.v_packet_count span{
    display: block;
}
.packet_left{
    width:2rem;
    font-size: 0.34666667rem;
    color: #444;
}
.packet_middle{
   width:2.5rem;
   color: #fb8394;
}
.buy-gold-bean-btn{
    padding: 0.13333333rem .2rem;
    color: #67c3aa;
    border: 0.01333333rem solid #67c3aa;
    border-radius: 0.66666667rem;
    font-style: normal;
    min-width: 1.5rem;
    text-align: center;
    margin-left: .2rem;
    white-space: nowrap;
    font-size: .34rem;
    display: none;
}
.red_packet_task li{
    border: 1px solid #e6e1a8;
    background: #f6f4df;
    padding: .25rem .1rem;
    font-size: .3rem;
    text-align: center;
    margin: .2rem 0;
    border-radius:.3rem; 
}
.red_packet_task li a{
  color: #d9b348;
}
.red_packet_task li strong{
  margin-left: .2rem;
  font-weight: normal;
  color: #b08919;
}
.right_flex{
    width: 2.8518123667377395rem;
}
.buyGoldBean{
    width: 2.8518123667377395rem;
    height: 1.2659914712153517rem;
}



.gamehall-adbar {
  height: 2.5rem;
  background: #eee;
  overflow: hidden;
}
.gamehall-adbar >>>.van-swipe__indicators{
  top: 2rem!important;
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
.game_item{
  margin-top: .2rem;
}
.game_item ul{
  padding: .15rem;
} 
.game_item ul li{
  float: left;
  width: 50%;
  margin-bottom:.22666667rem;
}
.game_item ul li .game_list{
  width:4.5rem;
  height: 2.5rem;
  margin: 0 auto;
  border-radius:.3rem; 
  position: relative;
  padding: .1rem;
   display: -webkit-box;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
    -webkit-justify-content: space-around;
    justify-content: space-around;
    -webkit-box-align: center;
    -webkit-align-items: center;
    -ms-flex-align: center;
    align-items: center;
}
.game_hot{
    width: 1.2rem;
    height: .6rem;
    text-align: center;
    line-height: .6rem;
    border-radius:.3rem 0;
    position: absolute;
    top: 0;
    left: 0;
    background: #f6f4df;
    color: #fa233d;
    font-size: .28rem;
}
.time_state_show{
    position: absolute;
    top: 0;
    right: 0;
    bottom: .1rem;
    width: 57%;
    height: .7rem;
    background: rgba(0,0,0,.2);
    color: #ffcb52;
    line-height: .7rem;
    text-align: center;
    border-radius: 0 .3rem 0 .3rem;
}
.game_logo{
  display: block;
  float: left;
  width:1.6rem;height:1.6rem;
}
.game_explain{
  float: left;
  color:#fff;
  h5{
    font-size: .4rem
  }
  p{
    font-size: .3rem;
    width: 2.5rem;
    // white-space: wrap!important;
  }
}
.gamehall-cooperatbar{
      margin-bottom: 2rem;
}
.gamehall-cooperatbar h5{
  padding: .3rem;
}
.gamehall-game-til-new {
  font-size: 0.37333333rem;
  color: #2d2d34;
  padding: 0.4rem;
  font-weight: 700;
}
.gamehall-game-til {
    padding: 0.4rem;
    font-size: 0.34666667rem;
    color: #444;
    font-weight: normal;
    border-bottom: 0.01333333rem solid #e3e4e8;
}
.wj-bottom-navbar{
  position: fixed!important;
}
.gethbnum {
    position: absolute;
    top: 39%;
    color: #fff;
    font-size: .66666667rem;
    text-align: center;
    width: 100%;
}
.maskxx .closebtnxx{
    width: 65%;
    height: 1.1rem;
}
.closebtnoo,.closebtnxx {
    position: absolute;
    bottom: 32%;
    left: 18%;
}
</style>
<style lang='scss' scoped>
@function rem($px){
    $rem: 75px;
    @return ($px/$rem) + rem;
}
@mixin flex {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -webkit-box-pack: justify;
    -ms-flex-pack: justify;
    flex-wrap: wrap;
    justify-content: space-between;
    -webkit-box-align: center;
    -ms-flex-align: center;
    align-items: center;
}
 @mixin rounded-corners($border){
    -moz-border-radius: $border;
    -webkit-border-radius:$border;
    border-radius:$border;
}
.getPacket-topbar {
    position: fixed;
    top: 0;
    left: 0;
    background: #fff;
    z-index: 888;
    width: 100%;
    border-bottom: 1px solid #f3f4f5;
}
.getPacket_topbar_icon{
    @include flex;
    padding:.05rem .15rem;
}
.getPacket_topbar_icon .buyGoldBean{
    width: rem(214px);
    height: rem(95px);

}
.getPacket_topbar_icon .getEveryDayHb{
    width: rem(240px);
    height: rem(70px);
    background:url('/Assets/Game/img/l5.png') no-repeat center top;
    background-size:100% 100%;
    color:#fff;
    line-height:rem(70px);
    text-align:center;
}
.getPacket_topbar_icon .lhbTip{
    width: rem(36px);
    height: rem(36px);

}
.getPacket-topbar h4 {
  font-weight: normal;
  text-align: center;
  color: #2d2d34;
}
.getPacket-topbar a {
  position: absolute;
  top: 50%;
  -webkit-transform: translateY(-50%);
      -ms-transform: translateY(-50%);
          transform: translateY(-50%);
  right: 0.4rem;
  width: 0.53333333rem;
  height: 0.53333333rem;
}
.gamehall-everyone-playing {
  padding: 0 0.4rem;
  margin: 0.4rem 0;

}
.gamehall-everyone-playing {
  padding: 0 0.4rem;
  margin: 0.4rem 0;
}
.gamehall-everyone-playing .sw-bullet {
  bottom: 0;
}
.gamehall-everyone-playing .sw-bullet li.active {
  background: #65c4aa;
}
.gamehall-everyone-playing .gamehall-everyone-playing-list {
  /*display: flex;
    flex-direction: row;
    align-items: center;
    flex-wrap: wrap;
    justify-content: space-between;*/
  overflow: hidden;
}
.gamehall-everyone-playing .gamehall-everyone-playing-li {
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
.gamehall-everyone-playing .gamehall-everyone-playing-li:nth-child(n+5) {
  margin-top: 0.26666667rem;
}
.gamehall-everyone-playing .gamehall-everyone-playing-li p {
  -ms-text-overflow: ellipsis;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: 0.28rem;
  padding-top: 0.2rem;
  color: #555;
  overflow: hidden;
}
.getPacket_mainbar{
    position: relative;
}
// .enter_get_red_packet{
//     padding:.4rem .3rem;
//     display: flex;
//     justify-content: space-between;
//     align-items: center;
//     color: #444;
//     font-size: .35rem;
// }
// .enter_get_red_packet span{
//     font-family: "黑体",sans-serif;
//     font-weight: bold;
// }
/* 7.26修改 */
.get_rp_count_list {
    border-top: 1px solid #f3f4f5;
    position: fixed;
    bottom: 1.3rem;
    left: 0;
    width: 100%;
    background: #fff;
    padding: .05rem 0;
}
.get_rp_count_list ul{
    display: flex;
    justify-content: space-around;
    align-items: center;
}
.list_red_packet p{
    line-height: .5rem;
}
.list_red_packet img{
    float: left;
    margin-right: .2rem;
    width: rem(24.6px);
    height: rem(30px);
}

.list_red_packet span{
    color: #fa6565;
    font-size:.4rem;
}
.total_v_red_packet p{
    line-height: .5rem;
    img{
        float: left;
        margin-right: .2rem;
        width: 0.328rem;
        height: 0.4rem;
    }
} 
.total_v_red_packet span{
    color: #fa6565;
    font-size:.4rem;     
}
.list_JD_packet{
    width: 28%;
    text-align: center;
    img{
        float: left;
        margin-left: .5rem;
        width: 0.6130063965884861rem;
        height: 0.7329424307036246rem;
    }
}
.list_JD_packet span{
    color: #c6a213;
    font-size:.45rem;
    line-height: .78rem;
}
.get_rp_ad{
    height: 1.2rem;
    display: flex;
    justify-content: space-around;
    align-items: center;
    padding: 0 .2rem;
    
    margin-top: 1.4rem;
}
.right_ad{
    width: 80%;
}
.right_ad img{
    width: 1.7324093816631128rem;
    height: 0.5330490405117271rem;
    float: left;
    margin-top: .35rem;
}
.right_flex{
    width: 30%;
}
.banner_word{
    width: 70%;
    height: 1.2rem;
    line-height: 1.2rem;
    overflow: hidden;
    position: relative;
    text-align: center;
}
.banner_word ul{
    position: absolute;
    top:0;
    left: 0;
    width:100%;
    font-size: .35rem;
}
.gameall_recommend{
    margin-bottom: 4rem;
}
.handleMore{
    text-align: center;
    line-height: 1rem;
    color: #777;
}
.gameall_recom-cell{
    display: -webkit-box;
    display: flex;
    -webkit-box-orient: horizontal;
    -webkit-box-direction: normal;
    flex-direction: row;
    -ms-flex-wrap: wrap;
    flex-wrap: wrap;
}
.gameall_recom-cell li {
    display: -webkit-box;
    display: flex;
    width: 33.3333333333%;
    -ms-flex-pack: justify;
    -webkit-box-pack: justify;
    justify-content: space-between;
    -ms-flex-direction: column;
    -webkit-box-orient: vertical;
    -webkit-box-direction: normal;
    flex-direction: column;
}
.gamehall_recommend_big{
    width: 3.333333rem;
    height: 3.333333rem;
    overflow: hidden;
}
.gamehall_recommend_big img{
    display: block;
    width: 100%;
    height: 100%;
}
.gamehall_recommend_head{
    font-size: .35rem;
    color: #303037;
    text-overflow: ellipsis;
    display: -webkit-box;
    white-space: normal;
    -webkit-line-clamp: 2;
    line-height: 1rem;
    // overflow: hidden;
    margin: 0 .2rem;
    height: .8rem;
}
.gameshall_recommend_price{
    font-size: .3rem;
    color: #e50008;
    padding-bottom: .24rem;
    margin: 0 .2rem;
}
/* <!-- 7.28新增购物卖钱 --> */
.resell_box{
    width: 2.46667rem;
    height: 1.06667rem;
}
.resell_shadow{
    width:100%;
    min-height: 100%;
    position: fixed;
    top:0;
    left: 0;
    background:rgba(0,0,0,.3);
    z-index: 9999;
}
.resell_loading{
    @include rounded-corners(.5rem);
    width: 80%;
    background: #fff;
    border-radius: 0.3rem;
    height: 2rem;
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    margin: auto;
    text-align: center;
    padding: 1rem 0;
}
.resell_loading img{
    width:rem(200px);
    height:rem(80px);
    margin:0 auto;
}
.resell_loading p{
    line-height: 1rem;
    font-size: 0.4rem;
    color:#333;
}
.resell_js{
    width:100%;
    height: 75%;
    position: absolute;
    bottom:0;
    left: 0;
    background: #fff;
    color:#333;
    font-size:.35rem;
}

.resell_js_result{
    width: 86%;
    background: #fff;
    position: absolute;
    top: -1.5rem;
    left: 7%;
    box-shadow: 0 0px 10px #999;
    @include rounded-corners(.2rem);
}
.into_result_detail{
    width:rem(100px);
    height: rem(60px);
    background:url('/Assets/Game/img/resalt_detail.png') no-repeat center top;
    background-size:100% 100%;
    position: absolute;
    top: .5rem;
    left:0;
    color: #fff;
    text-align: center;
    line-height:.8rem;
}
.quit_result_detail{
    width:rem(30px);
    height: rem(30px);
    position: absolute;
    top: .2rem;
    right:.2rem;
}
.resell_js_result dl{
    width: 90%;
    padding: .3rem 0;
    margin: 0 auto;
    text-align: center;
    .js_title{
        font-weight: bold;
        line-height: .8rem;
    }
    .js_con{
        color: #777;
        span{
            color: #65c4aa;
        }
        em{
            font-size: .5rem;
            color: #555;
            font-weight: bold;
            margin-left: .2rem;
        }
    }
    .js_count{
        @include flex;
        justify-content:space-around!important;
        margin-top: .5rem;
        color:#777;
        p{
          line-height: .8rem;
        }
        .result_left{
            em{
                color: #abc659;
            }
        }
        .result_right{
             em{
                color: #f4cb57;
            }
        }
    }
   
}

.resell_js_form{
    margin-top: 2.5rem;
    padding: .5rem;
    .form_title{
        line-height: 1rem;
        margin-top: .5rem;
        i{
            color: #f60;
            margin: 0 .1rem;
        }
        span{
            color:#877700;
            font-size: .32rem;
        }
    }
    input[type='text']{
        display: block;
        border: 1px solid #aaa;
        padding: .2rem;
        margin-bottom: .5rem;
        width: 95%; @include rounded-corners(.2rem);
    }
    .form_tip{
        color:#f60;
    }
    button{
        display: block;
        margin: .5rem auto;
        border: none;
        padding: .4rem 0;
        width: 90%;
        border-radius: 1rem;
        color: #fff;
        font-size: .4rem;
    }
    .btnGray{
        background: #e3e4e8;
    }
    .btnBlue{
        background: #65c4aa;
    }
    .form_xy{
        .radio_icon{
            display:block;
            width:.4rem;
            height:.4rem;
            margin: .05rem .2rem 0 .5rem;
            float: left;
            img{
                width:100%;
                height:100%;
            }
        }
    }
    
}
.return_result_js{
    width:rem(100px);
    height: rem(60px);
    background:url('/Assets/Game/img/return_js.png') no-repeat center top;
    background-size:100% 100%;
    position: absolute;
    top: .5rem;
    right:0;
    color: #fff;
    text-align: center;
    line-height:.8rem;
    z-index:888;
}
.resell_detail{
    width: 90%;
    margin: .1rem auto;
    position: relative;
   .detail_title{
        font-size: .4rem;
        font-weight: bold;
        line-height: 1.5rem;
        text-align: center;
   }
   .detail_con{
       @include flex;
       color:#777;
       div{
           width:50%;    
           text-align: center;
       }
       p{
          line-height:1rem;
       }
       .resell_detail_left em{
           color: #7a7b87;font-weight: bold;    font-size: .46rem;
       }
       .resell_detail_right em{
           color: #2d2d34;font-weight: bold;    font-size: .46rem;
       }
   }
   .detail_tip{
           margin-top: .3rem;
        border: 1px solid #f4cb57;
        background: #fffaed;
        padding: .25rem .1rem;
        color: #d9b95e;
        font-size: .3rem;
        text-align: center;
        border-radius:.2rem; 
        em{
          color:#9e7e24;
        }
    }
    .detail_voucher{
        margin-top: .5rem;
        h3{
            color: #222;
        }
        .voucher { 
            margin: .3rem 0;
            @include flex;
            dd{
                width:rem(330px);
                height: rem(160px);
                position: relative;
                .voucher_explain{
                    width: 100%;
                        line-height: .75rem;
                        text-align: center;
                    color:#fff;
                    font-weight:bold;
                    background:rgba(0,0,0,.2);
                    position: absolute;
                    left:0;
                    bottom:0;
                }
            }
            .voucher_100{
                 background:url('/Assets/Game/img/100djq.png');
                  background-size:100% 100%;
            }
            .voucher_10{
                 background:url('/Assets/Game/img/10djq.png'); 
                 background-size:100% 100%;
            }
        }
    }
    .detaile_item{
        margin-top: .5rem;
        box-shadow: 0 0px 10px #999;
        @include rounded-corners(.1rem);
        padding:.3rem 0;

        .card_count{
            width:90%;
            background: #f3f4f5;
            margin: 0 auto;
            padding: .2rem 0;
            @include rounded-corners(.1rem);
            font-weight: bold;
            li{
                text-align: center;
                line-height: .6rem;
            }
            em{
                color:#cfa326; font-weight: bold;
            }
        }
        p{
            width:90%;
            margin:.1rem auto 0;
            line-height: .6rem;
            padding: 0 .3rem;
            white-space: normal;
        }
        .detaile_list{
            color:#777;
            span{
                font-weight: bold;    color: #666
            }
            strong{
                 color:#f60;
            }
        }
    }
}
.resell_xy{
    width: 90%;
    margin: 0 auto;
    position: relative;
    padding:.4rem 0;
    height: 11.6rem;
    overflow: auto;
    .resell_xy_tip{
        color: #000;
    }
    p{
        white-space: normal!important;    margin-bottom: .2rem;
        color: #666;
        font-size: .35rem;
    }
    h3{
        font-weight: bold;
        line-height: 1rem;
    }
}
.resell_commit{
    width:100%;
    height: 28%;
    position: absolute;
    bottom:0;
    left: 0;
    background: #fff;
    color:#333;
    font-size:.35rem;
    text-align:center;
    .rc_con{
        margin: .7rem 0;
        p{
            font-size: .4rem;
            font-weight:bold;
            line-height: .8rem;
        }
    }   
    .rc_btn{
        display: block;
        margin: .5rem auto;
        border: none;
        padding: .35rem 0;
        width: 70%;
        border-radius: 1rem;
        color: #fff;
        font-size: .4rem;
    }
    .rc_success_btn{
        background:#65c4aa;
    }
    .rc_fail_btn{
        background:#f4cb57;
    }
}
// 2018.8.20
.luckyRedPacket{
    width: 100%;
    height: 100%;
    background:rgba(0,0,0,.5);
    position: fixed;
    top: 0;
    z-index: 99999;
}
.luckyRedPacket_box{
    width:rem(590px);
    height:rem(800px);
    position: absolute;
    top: 0;
    left:0;
    bottom: 0;
    right: 0;
    margin: auto;
    z-index: 11;
}
.closeBtn_lp{
    width: 1rem;
    height: 1rem;
    position: absolute;
    top: 0;
    right: 0;
}
.luckyRedPacket_con{
    position:absolute;
    bottom: 2.5rem;
    width: 100%;
    text-align: center;
    font-size: .38rem;
    color: #efeb62;
    font-family: '黑体',sans-serif;
}
.luckyRedPacket_con h3{
    line-height: 1.5rem;
    font-size: .8rem;
    font-weight: bold;
}
.luckyRedPacket_con p{
    line-height: .7rem;
    font-size: .5rem;
}
.luckyRedPacket_con p span{
    color:#f8fec2;
}
.luckyRedPacket_btn{
    width: rem(402px);
    height: rem(93px);
    position: absolute;
    bottom:  rem(63px);
    left:  rem(96px);
    background: none;
    border: none;
}
</style>
<template>
    <div class="getPacket_mainbar">
        <!--头部开始 -->
        <div class="getPacket-topbar">
            <ul class="getPacket_topbar_icon">
                <li class="buyGoldBean" @click='buy_gold_bean'><img src="/Assets/Game/img/lhb_gjsd.png"/></li>
                <!--<li class="getEveryDayHb"  @click="enterGetRedPacket">天天领50红包</li>-->
                <li class="lhbTip"   @click="lhbTip"><img src="/Assets/Game/img/lhb_tip.png"/></li>
            </ul>
        </div>

        <!-- 7.28新增购物卖钱 -->
        <div class="resell_shadow"  v-show="resell_loading">
            <div class="resell_loading" v-if='resell_con'>
                <img src="/Assets/Game/img/jsq.gif" alt="loading">
                <p>兜兜正在努力计算中...</p>
            </div>
            <div v-else>
                <div  v-if="resell_js_result === 'A'">  
                    <div class="resell_js">
                        <div class="resell_js_result">
                            <div class="into_result_detail"  @click="into_result_detail">
                                详情&gt;
                            </div>
                            <div class="quit_result_detail"  @click="resell_loading=false">
                                <img src="/Assets/Game/img/close.png" alt="关闭">
                            </div>
                            <dl>
                                <dt class="js_title">计算结果</dt>
                                <dd  v-if="result_Card100>0" class="js_con">您当前金豆可换购价值<span>100元</span>购物券<em>{{result_Card100}}</em> 张</dd>
                                <dd  v-if="result_Card10>0"  class="js_con">您当前金豆可换购价值<span>10元</span>购物券<em>{{result_Card10}}</em> 张</dd>
                                <dd  v-else  class="js_con">余额不足无法换购</dd>
                                
                                <dd class="js_count"> 
                                    <div class="result_left"><em>{{actualGetCash}}元</em><p>卖钱后可得</p></div>
                                    <div class="result_right"><em>{{remainingGoldBeans}}</em><p>卖钱后剩余金豆数</p></div>
                                </dd>
                            </dl>
                        </div>
                        <div class="resell_js_form">
                            <p class="form_title">请填写支付宝账号 <i>*</i> <span>(请认真填写并核实,以免无法到账)</span></p>
                            <input type="text"  id="resell_name"    @keyup="initSubmitBtn"    v-model="r_name" placeholder="请填写您的真实姓名">
                            <input type="text" id="resell_num"      v-model="r_num"  @keyup="initSubmitBtn"  placeholder="请输入支付宝账号">
                            <input type="text" id="resell_confirm_num"    v-model="r_confirm_num" @keyup="initSubmitBtn" placeholder="请再次输入支付宝账号">
                            <p class="form_tip" >{{form_tip}}</p>

                            <button  v-if="isShow"  class="btnGray"  disabled>确认卖钱</button>
                            <button  v-else  id="submit_btn"  class="btnBlue"  @click='resell_submit' >确认卖钱</button>
                        
                            <div class="form_xy"> 
                                <span @click="radio_no_checkd" v-if="radio_checkd" class="radio_icon" ><img src="/Assets/Game/img/ra_wx.png" alt="未选中"></span> 
                                <span @click="radio_check"  v-else class="radio_icon" ><img src="/Assets/Game/img/radio_xz.png" alt="选中"></span>
                                确认则表示您同意
                                <a @click="into_resell_xy"   href="javascript:;">《授权委托寄售协议》</a>
                            </div>
                        </div>
                    </div>
                </div>

                <div v-else-if="resell_js_result === 'B'">
                    <div class="resell_js">
                        <div class="return_result_js"  @click="return_result_js">返回&gt;</div>
                        <ul  class="resell_detail">
                            <li class="detail_title">详情</li> 
                            <li class="detail_con">
                                <div class="resell_detail_left"><em>{{rdLeft}}</em> <p>当前总金豆</p> </div>
                                <div class="resell_detail_right"><em>{{rdRight}}</em><p>可购物金豆</p></div>
                            </li>   
                            <li class="detail_tip">TIP: 金豆购物门槛为<em>{{jdmk}}W</em>,可使用部分<em>大于{{jdmk}}W</em>的部分</li>
                            <li class="detail_voucher">
                                <h3>商品价格</h3>
                                <dl class="voucher">
                                    <dd class="voucher_100"><p class="voucher_explain">￥2元+100金豆</p></dd>
                                    <dd class="voucher_10"><p class="voucher_explain">￥0.2元+10金豆</p></dd>
                                </dl>
                            </li>
                            <li class="detaile_item">
                                <ul  class="card_count">
                                    <li  v-if="result_Card100>0"> 本次可购买 <em>{{result_Card100}}</em> 张100元购物卡,价值 <em>{{result_Card100*100}}元</em>  </li>
                                    <li  v-if="result_Card10>0"> 本次可购买 <em>{{result_Card10}}</em> 张10元购物卡,价值 <em>{{result_Card10*10}}元</em>  </li>
                                    <li  v-else > 余额不足无法换购 </li>
                                 </ul>
                                <p class="detaile_list">
                                    <span>消费</span>  <strong>{{platformFeeTotal}}元+{{actualPayGoldBeans}}金豆</strong> (直接扣除,无需另支付)
                                </p>
                                <p class="detaile_list">
                                    <span>结算</span>
                                    卖钱后,可得 <strong>{{cardCashTotal}}-{{platformFeeTotal}}={{actualGetCash}}元</strong> ,剩余<strong>{{remainingGoldBeans}}</strong>金豆
                                </p>
                            </li>
                        </ul>
                    </div>
                </div>

                <div v-else-if="resell_js_result === 'C'">
                    <div class="resell_js">
                        <div class="return_result_js"  @click="return_result_js">返回&gt;</div>
                        <div class="resell_xy">
                            <div class="resell_xy_tip"> 同意购物卖钱，系统将默认扣除指定金豆，并购买指定商品形成订单，同时一键寄售到第三方交易平台，遵循以下相关寄售协议，请仔细阅读并知晓。</div>
                            
                            <h3> 授权委托寄售协议</h3>
                                <p>1、委托人授权“省兜兜”将委托人在该平台购买并“一键转卖”的商品进行寄售；</p>                   
                                <p>2、委托人授权“省兜兜”将转卖商品指定由“聚e聚”平台进行寄售；</p>
                                <p>3、委托人授权“省兜兜”将转卖商品通过邮寄或其他形式交由“聚e聚”完成后续相关销售流程；</p>
                                <p>4、委托人授权“省兜兜”将委托人转卖商品信息，包括但不仅限于订单编号、商品名称</p>
                                <p>购买金额、优惠信息、购买人信息等，提交由“聚e聚”进行信息有效性验证；</p>
                                <p>5、委托人知悉“一键转卖”商品不参与退款、退换货等售后行为，一经售出，将放弃对该商品之使用权；</p>
                                <p>6、本服务协议双方为省兜兜与省兜兜用户，本服务协议具有合同效力。</p>
                            

                            <h3> 寄售协议</h3>
                                <p>1、省兜兜用户授权“聚e聚”平台对其指定的“一键转卖”商品进行寄售；</p>
                                <p>2、省兜兜用户授权寄售商品，统一由省兜兜将所寄售商品交由“聚e聚”平台进行后续销售处理；</p>
                                <p>3、省兜兜用户“一键转卖”的商品即表示同意该价为最终销售价格，不以市场行情波动进行调整；</p>
                                <p>4、省兜兜用户在我司聚e聚平台寄售成功后，将收取交易额的3%作为平台服务手续费；</p>
                                <p>5、本网站有权验证您所转卖之寄售信息真实有效性，并有权无条件拒绝寄售；</p>
                                <p>6、本网站委托“省兜兜”收集用户收款账户信息，用于业务结算，并承诺对用户信息进行保密；</p>
                                <p>7、商品寄售于“聚e聚”平台之后，即视为主动放弃取消该商品寄售权利；</p>
                                <p>8、寄售于“聚e聚”平台之物品销售成功之后，即表示该物品之使用者已放弃该物品之使用权，今后不得以任何理由及方式索回该物品。 </p>
                                <p>9、本服务协议双方为“聚e聚”平台与省兜兜用户用户，本服务协议具有合同效力。</p>
                        </div>
                    </div>
                </div>

                <div v-else-if="resell_js_result === 'D'">
                    <div class="resell_commit">
                        <div class="rc_con">
                             <p>卖钱成功,请等待收款</p>
                            <span>(预计2小时到账)</span> 
                        </div>
                        <button    class="rc_btn rc_success_btn"  @click='resell_commit' >好的</button>
                    </div>
                </div>

                <div v-else-if="resell_js_result === 'E'">
                    <div class="resell_commit">
                        <div class="rc_con">
                            <p>卖钱失败,请稍后再试</p>
                            <span>{{err_message}}</span> 
                        </div>
                        <button    class="rc_btn rc_fail_btn"  @click='resell_commit' >好的</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- 7.28新增购物卖钱 -->

        <div class="get_rp_ad">
            <div class="right_ad">
                <img src="/Assets/Game/img/lhb_rx_05.png" alt="红包规则">
                <div class="banner_word">
                    <ul :style="{top}">
                        <li v-for="ad of ads" :key="ad.id" ref='adlist' >{{ad.word}}</li>
                    </ul>
                </div>
            </div>
            <div class="right_flex" @click='share'>
                <i class="buy-gold-bean-btn">推荐得{{firstShareRedCollar}}红包</i>
            </div>
        </div>

        <!--兜兜运动区域开始-->
        <div class="border-b" style="display:none ">
            <h6 class="gamehall-headtop">十步一红包 好货随你挑</h6>
            <div class="dd-motion-list">
                <ul>
                    <li>
                        <img src="/Assets/Game/img/img@3x-min.png" alt=""/>
                    </li>
                    <li>
                        <div>今日行走步数<span id="lblSportStep">{{step}}</span>步</div>
                        <div>今日红包<span id="lblSportMoney">{{stephb}}</span>个</div>
                        <div>(今日22:00结算后将自动计入总红包)</div>
                    </li>
                    <li>
                        <img id="imgBtnToSport" @click="imgBtnToSport_click" src="/Assets/Game/img/go@3x-min.png" alt=""/>
                    </li>
                </ul>
            </div>
        </div>
        <!--兜兜运动区域结束-->

        <!--小游戏区域开始-->  
        <div class="game_item border_t">
            <ul class="clearfix">
                <!-- <li class="gameList" >
                    <div  class="game_list"  :style="{backgroundImage:'url(' + game.img_url + ')'}"> 
                        <span class="game_hot" v-if='index===0'>最新</span>  

                        <img class="game_logo" :src="game.logo_url" :alt="game.name"/>
                        <div class="game_explain">
                        <h5>{{game.name}}</h5>
                        <p>{{game.simple_remark}}</p>
                        </div>
                    </div>
                </li> -->

              <li v-for="(game,index) in games"  :key="index"  class="gameList"  @click="call_beans_playing(game.action_js)" >
                <div  class="game_list"  :style="{backgroundImage:'url(' + game.img_url + ')',backgroundSize:'100% 100%'}">
                    <span class="game_hot" v-if='index===0'>最新</span> 
                    <span class="game_hot" v-if='index===1'>最热</span>  

                    <p class="time_state_show" v-if="game.type==='101'">{{bdx}}</p>
                    <p class="time_state_show" v-if="game.type==='102'">{{bsx}}</p>
                    <p class="time_state_show" v-if="game.type==='103'">游戏进行中</p>

                    <img class="game_logo" :src="game.logo_url" :alt="game.name"/>  
                    <div class="game_explain">
                      <h5>{{game.name}}</h5>
                      <p>{{game.simple_remark}}</p>
                    </div>
                </div>
              </li>

            <li class="gameList" >
                <div  class="game_list"  style="background:url('/Assets/Game/img/qd_05.png');background-size:100%,100%;padding:0"> 
                </div>
            </li>


            </ul>
        </div>
        <!--小游戏区域结束-->  

        <!--休闲游戏区域开始-->  
        <div class="gamehall-cooperatbar border_t">
            <h5 class="gamehall-game-til gamehall-game-til-new">休闲游戏</h5>
            <div class="top-banner gamehall-everyone-playing">
                <van-swipe  
                            :duration="500" 
                            :loop="true"
                            :show-indicators="true"
                            :initial-swipe="0"
                >
                    <van-swipe-item v-for="(page,index) of pages" :key="index">
                        <div v-for="(item,index) of page" :key="index" class="gamehall-everyone-playing-li"  @click="iframeVal(item.action_js)">
                            <img :src="item.img_url" :alt="item.name">
                            <p>{{item.name}}</p>
                        </div>
                    </van-swipe-item>
                </van-swipe>
              </div>
        </div>
        <!--休闲游戏区域结束-->  

        <!-- 小兜推荐开始 -->
        <div class="gameall_recommend border_t">
            <h5 class="gamehall-game-til gamehall-game-til-new">兜兜优惠购</h5>
            <ul class='gameall_recom-cell'>
                <li v-for='item in xdList' 
                    :key="item.id"  
                    @click='todetail(item.id)'  
                    v-if='xdList.length!= 0'>
                    <div  class="gamehall_recommend_big">
                        <img :src='$store.state.imgs+ item.pic_id+ $store.state.img_suffix' 
                             :onerror="$store.state.defaultimg"
                            :load="$store.state.defaultimg"/>
                    </div> 
                    <p  class="gamehall_recommend_head">{{item.name}}</p>
                    <p class="gameshall_recommend_price" v-if='item.discount == 0'>
                        价格：￥{{item.price}}
                        <br/>
                        (￥{{ item.cash_price }} +{{  item.beans_price | formatBeansPrice }}金豆)
                    </p>
                    <p class="gameshall_recommend_price"  v-else>
                        ￥{{item.price }} + {{item.discount}}金币
                    </p>
                </li>
            </ul>
            <div class="handleMore" @click="getXdMore" v-if='xdList.length >5'>点击查看更多</div>
            <div class="handleMore" v-if='xdList.length === 0'>暂无商品</div>
        </div>
        <!-- 小兜推荐结束 -->

        <!--分享红包图片区域开始-->
        <div class="hboxfixed" v-show="hbox">
            <div class="shaw"></div>
            <div class="img"  @click="this.hbox=false"><img src="/Assets/Source/img/hbox.png"/></div>
        </div>
        <!--分享红包图片区域结束-->

        <!--省兜兜大红包页面开始-->
        <div class='maskxx'>
            <p class='gethbnum y-color' v-html='gethbnum'></p>
            <img class='closebtnxx' @click="closeRedFree" src='/Assets/Event/img/images/oolin.jpg'/>
        </div>
        <!--省兜兜大红包页面结束-->

        <!--2018.8.20页面开始-->
        <div class="luckyRedPacket" v-show="luckyRedPacket">
            <div class="luckyRedPacket_box">
                <img  id="luckyRedPacket" src="/Assets/Game/img/dialog_05_v2.png" alt="幸运红包">
                <div  id="closeBtn_lp" class="closeBtn_lp" @click="closeBtn_lp" ></div>
                <div class="luckyRedPacket_con">
                    <h3>仅需 {{luckyMoney}}元</h3>
                    <p><span>即可获得</span>{{redpacket}}个V红包</p>
                    <p><span>+</span></p>
                    <p>{{couponsMoney}}元购物券</p>
                </div>
                <button class="luckyRedPacket_btn" @click="luckyRedPacket_btn()" ></button>
            </div>
        </div>
        <!--2018.8.20页面结束-->

    
        <div class="get_rp_count_list">
           <ul>
                <!-- <li class="buyGoldBean" @click='buy_gold_bean'> --> 
                <li class="total_v_red_packet"> 
                    <p>
                       <img src="/Assets/Game/img/a_07.png" alt="V红包">
                       V红包
                    </p>
                    <span>{{vRedpacket}}</span>
                </li> 
                <li class="list_red_packet">
                   <p>
                       <img src="/Assets/Game/img/a_09.png" alt="红包">
                       红包
                    </p>
                   <span>{{redPacket}}</span>
                </li>
                <li class="list_JD_packet">
                   <img src="/Assets/Game/img/lhb_icon_09.png" alt="金豆">
                   <span>{{JDPacket}}</span>
                </li>
                <li class="resell_box" @click='Resell'>
                    <img src="/Assets/Game/img/resell.gif" alt="马上卖钱">
                </li>
           </ul>
        </div>
    
        <!--footerbar-->
        <bottom ref="bottomCOM"></bottom>
    </div>
</template>
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
                sHref:false,
                token:localStorage.getItem("shorttoken"),// 检测是否登陆
                hbox:false,   //分享按钮
                redPacket:'-',  //红包
                JDPacket:'-',   //金豆
                vRedpacket:'-',  //V红包
                firstShareRedCollar: '300',  //推荐的300红包
                //firstShareJd: '10',
                allList: [],                   // 所有轮播图数据
                swiperSlides: [],              // 顶部主轮播图
                selectList: [],                // 顶部公告games
                games:[],                   //游戏列表
                stephb:'-',               //兜兜运动
                step:'-',                  //兜兜运动
                pageArr:[],                 //休闲游戏列表
                ads:[
                    {id:1,word:' 账户余额（红包+金豆）越高，'},{id:2,word:'每日领取的红包金额越大。'}
                ],
                resell_loading:false,   //转卖loading
                resell_con:true,
                result_Card100:0,      //面值为100元的卡数量
                result_Card10:0,        //面值为10元的卡数量
                resell_js_result:'A',  //控制显示
                rdLeft:0,        //当前总金豆
                rdRight:0,       //可购物金豆
                jdmk:"-",  //金豆门槛
                resell_xy:false,       //协议
                actualGetCash:0,        //卖钱可得
                remainingGoldBeans:0,          // 卖钱后剩余金豆
                actualPayGoldBeans:0,   //实际用于购卡支付的金豆数额
                cardCashTotal:0,        //所有购买的卡的现金（人民币）总额
                platformFeeTotal:0,    //平台收取的手续费总额
                r_name:'',          //校验真实姓名
                r_num:'',             //校验手机号
                r_confirm_num:'',   //确认账号
                form_tip:'',   //校验提示
                isShow:true,    //提交按钮
                radio_checkd:true,  //勾选
                id:0,
                showA:false,  //A类用户才显示购物卖钱的入口
                isCanBuy:1,
                xdList:[],
                err_message:'',
                bdx:'',
                bsx:'',
                luckyRedPacket:false,
                luckyMoney:'',
                redpacket:'',
                couponsMoney:'',

                payMoney:100,
                enter_zp:false,
                sign: true,
                is_show_game:"1",
                currenttime:'-',
                gethbnum:'30',
                isA:true,
                disable:true,
                lblPlayTimes:0,
                play_total_times:window.m_play_total_times,
                data_timeout_id:0,
                platform_hold_money:window.platform_hold_money,
                data_pay_coupons_total_money:0,// 购券花费
                data_hold_money:0,
                data_UserHoldMoneyConfig:[],
                customAlert:false,
                customAlertSure:false,
                is_news_user:window.global_is_news_user
            }
        },
        created(){
          this.is_show_game=window.is_show_game;
          // this.InitSignInData();
          this.GetUserSportInfo();//获取兜兜运动信息
          this.GetUserInfo();   //获取用户信息
          this.Banner();        //获取游戏大厅的轮播图数据

          this.method_ajax_getUserHoldMoneyConfig();
          this.xdRecommond();   //小兜推荐
          this.loopPage();
          this.getTimeStateBsx(); //爆双喜
          this.getTimeStateBdx(); //爆多喜
          this.IfHasTimeIntervalRedpaket(); // 是否可以领取时段充值红包
        },
        destroyed(){
        	if(this.data_timeout_id>0){
                window.clearTimeout( this.data_timeout_id);
            }
        },
        mounted(){
          this.GetGameList();   //获取游戏列表
          this.GetOtherGameList();   //获取休闲游戏列表
          setInterval(this.adScroll,2000); //广告轮播
					//this.loopPage();
        },
        activated () {
           this.token=localStorage.getItem("shorttoken");
           this.GetUserInfo();
           //this.loopPage();
           this.Shref();
        },
		deactivated(){
			if(this.data_timeout_id>0){
				window.clearTimeout( this.data_timeout_id);
			}
        },
		computed:{
			computed_get_hold_money() {
                return parseInt(this.data_hold_money/10000);
            },
            pages() {
                const pages = []
                this.pageArr.forEach((item, index) => {
                    const page = Math.floor(index / 8)
                    if (!pages[page]) {
                    pages[page] = []
                    }
                    pages[page].push(item);
                })
                return pages
            },
            top: function() {
                return -this.id * 1.2 + 'rem'
            }

        },

    methods:{
        //是否可以领取时段充值红包
        IfHasTimeIntervalRedpaket(){
            var _this=this;
            $.ajax({
                url:'/WebApi/game/IfHasTimeIntervalRedpaket',
                type:'get',
                data:{token:_this.token},
                success:function(res){
                    if(res.status===0){
                        let data=res.data;
                        if(data.isHasRedpacket===1){
                            _this.luckyRedPacket=true;
                            _this.luckyMoney=data.payMoney;
                            _this.redpacket=data.redpacket;
                            _this.couponsMoney=data.couponsMoney;
                        }
                    }
                }
            })
        },
        luckyRedPacket_btn(){
            this.luckyRedPacket=false;
            window.location.href='/game/ReceiveTimeIntervalRedpaket?token='+encodeURIComponent(this.token)+ "&redirect="+encodeURIComponent(location.href);
        },
        closeBtn_lp(){
            this.luckyRedPacket=false;
        },
        //获取爆多喜和爆双喜的开场状态
        getTimeStateBsx(){
            var _this=this;
             $.get("/WebApi/game/GameStatusForBalloon2",(res)=> {
                _this.bsx=res;
             })
        },
        getTimeStateBdx(){
            var _this=this;
             $.get("/WebApi/game/GameStatusForBalloonMore",(res)=> {
                _this.bdx=res;
             })
        },
        //跳转到更多页面
        getXdMore(){
            this.$router.push('/goodslistthird/' + window.global_dictionary.GoldBeanGoodsGroupId)
        },
        todetail(id) {
            this.$router.push('/goodsdetail/' + id + '/null')
        },
        //小兜推荐
        xdRecommond(){
            var that=this;
            $.ajax({
                url: '/SWebApi/ShopManager/GoodsListByType/' + window.global_dictionary.GoldBeanGoodsGroupId + '/' + this.$store.state.listofgoodspage + '.json?' + '&PageSize=6',
                type: 'get',
                dataType:'json',
                success: function (res) {
                    if (res.status == 0) {
                        for(var i in res.data){
                            var data= res.data[i];
                            that.xdList.push(data);
                        }

                    }
                }
            })
        },
        //勾选协议
        radio_no_checkd(){
            this.radio_checkd=false;
            this.initSubmitBtn();
        },
        radio_check(){
            this.radio_checkd=true;
            this.initSubmitBtn();
        },
        //close 转卖
        resell_commit(){
            this.resell_loading=false;
            this.resell_js_result='A';

        },
        //置亮按钮
        initSubmitBtn(){
            if(this.isCanBuy===1){
                if(this.r_name.length>=1&& this.r_num.length>=1&& this.r_confirm_num.length>=1&&this.radio_checkd==false){
                    this.isShow=false;
                }else{
                    this.isShow=true;
                }
            }
        },
        //提交表单
        resell_submit(){
            var that=this;
            if(this.r_num!==this.r_confirm_num){
                this.form_tip='两次输入的账号不一致';
                return;
            }else{
                this.form_tip='';
            }
            var payAccount=encodeURIComponent(this.r_confirm_num);
            var name=encodeURIComponent(this.r_name);
            $.get('/WebApi/TradeManager/SecondHandAutoCreateOrder',{token:that.token,payAccount:payAccount,name:name},(res)=>{
                if(res.status===0){
                    that.GetUserInfo();
                    that.resell_js_result='D';
                } else {
                        that.resell_js_result='E';
                        that.err_message=res.message;
                }

            })
        },
        //进入协议
        into_resell_xy(){
            this.resell_js_result='C';
        },
        //返回计算结果
        return_result_js(){
            this.resell_js_result='A';
        },
        //进入详情
        into_result_detail(){
            this.resell_js_result='B';
        },
        //进入购物卖钱
        Resell(){
            this.resell_loading=true;
            var that=this;
            that.r_name='';
            that.r_num='';
            that.r_confirm_num='';
            that.radio_checkd=true;
            that.isShow=true;
            $.get("/WebApi/TradeManager/SecondHandBuyCardData", {token: that.token},(res)=> {
                if(res.status===0){
                    that.resell_con=false;
                    var data=res.data;
                    that.isCanBuy=data.isCanBuy;
                    that.result_Card100=data.canBuyQty_Card100;
                    that.result_Card10=data.canBuyQty_Card10;
                    that.rdLeft=data.goldBeanTotal;
                    that.rdRight=data.canUseGoldBeans;
                    that.jdmk=data.holdMoney/10000;
                    that.remainingGoldBeans=data.remainingGoldBeans;
                    that.actualGetCash=data.actualGetCash;
                    that.actualPayGoldBeans=data.actualPayGoldBeans;
                    that.cardCashTotal=data.cardCashTotal;
                    that.platformFeeTotal=data.platformFeeTotal;

                }
            })
        },
        todetail(id) {
            this.$router.push('/goodsdetail/' + id + '/null')
        },
        //广告轮播
        adScroll(){
            if (this.id < this.ads.length) {
                this.ads.push(this.ads[0]);
                this.ads.shift();
            } else {
                this.id = 0
            }

        },
        /**
         * 跳转到天天领红包
         */
       	enterGetRedPacket(){
       			return;
        		var that=this;
           	if(!that.token){
            		confirm('尚未登录,是否登录？',function () {
                		that.$router.push({path:'/login',query:{redirect:('/#/gamehall')}});
              	});
            		return;
           	}
           	this.$router.push({path:'/getredpacket'});
       	},
        /**
         * 获取游戏列表
         */
        learn_yjzm(){
           window.location.href="/Event/six?token="+ encodeURIComponent(this.token) + "&redirect="+encodeURIComponent(location.href);
        },
        /**
         * 获取游戏列表
         */
        Shref(){
            var href=window.location.href;
            return href.indexOf("flag")>-1?this.sHref=true: this.sHref=false;
        },
        GetGameList:function(){
            var that=this;
            $.post("/WebApi/game/GameBaseData", { "type": 1 },(res)=> {
                if(res.status==0){
                    var data = res.data
                    var isWeixin = ua.indexOf('micromessenger') != -1;
                    if(isWeixin){
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            if (item.is_show_at_wechat == "1") {
                                that.games.push(item);
                            }
                        }
                    }
                    else{
                        for (var i = 0; i < data.length; i++) {
                        	var item = data[i];
                        	that.games.push(item);
                    	}
                    }
                }
            })
        },
        /*
        *点击小游戏跳转
        */
        call_beans_playing(sta) {
            sta=sta.replace(/[^0-9]/ig,"");
            this.all_playing(parseInt(sta), 2);  //金豆
        },
        /**
        * 点击大家都在玩跳转不同游戏页面
        */
        all_playing(sta, payType) {
            var that=this;
            if(!that.token){
                confirm('尚未登录,是否登录',function(){
                    that.$router.push({path:'/login',query:{redirect:('/#/gamehall')}});
                })
                return;
            }
            var ver = (new Date().getTime() / 100).toFixed(0);
            if (this.token) {
                var host = "http://" + location.host;
                var redirect_current_href_encode = "&redirect="+ encodeURIComponent(location.href);
                switch (sta) {
                    //http://game.rrlsz.com.cn   -->rrl-game.sdoudou.net
                    case 0:
                        location.href = "/Game/Roulette_V2/?token=" + encodeURIComponent(this.token) + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        break;
                    case 1:
                        var url = "http://rrl-game.sdoudou.net:8360/static/dazhuanpan/?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;// host + "/Game/GameJump?iframe=" + encodeURIComponent(url) + "&token=" + encodeURIComponent(token);
                        break;
                    case 2:
                        var url = "http://rrl-game.sdoudou.net:8096/sxl/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;// host + "/Game/GameJump?iframe=" + encodeURIComponent(url) + "&token=" + encodeURIComponent(token);
                        break;
                    case 3:
                        var url = "http://rrl-game.sdoudou.net:8096/fkp/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;// host + "/Game/GameJump?iframe=" + encodeURIComponent(url) + "&token=" + encodeURIComponent(token);
                        break;
                    case 4:
                        var url = location.href = "http://rrl-game.sdoudou.net:8360/static/caidaxiao/?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;// host + "/Game/GameJump?iframe=" + encodeURIComponent(url) + "&token=" + encodeURIComponent(token);
                        break;
                    case 5:
                        var url = "http://rrl-game.sdoudou.net:8360/static/xiaozhuanpan/?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;// host + "/Game/GameJump?iframe=" + encodeURIComponent(url) + "&token=" + encodeURIComponent(token);
                        break;
                    case 6:
                         var url = "http://rrl-game2.sdoudou.top/ddz/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                         break;
                    case 7:
                        if (payType == 1) {
                            location.href = "http://rrl-game.sdoudou.net:8736/snk_client_free/index.html?token=" + encodeURIComponent(this.token) + "&platform=2&ver=" + ver + redirect_current_href_encode;
                        }
                        else if (payType == 2) {
                            location.href = "http://rrl-game.sdoudou.net:8736/snk_client_jindou/index.html?token=" + encodeURIComponent(this.token) + "&platform=2&ver=" + ver + redirect_current_href_encode;
                        }
                        break;
                    case 8:
                        var url = "http://rrl-game2.sdoudou.top/ddz/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 10:
                        var url = "http://e-shop.rrlsz.com.cn/gameimg/zzz/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 11:
                        var url = "http://rrl-game2.sdoudou.top/fan?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 13:
                        var url = "http://120.78.253.83:8360/static/liuliuliu/?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 14:
                        var url = "http://rrl-game2.sdoudou.top/qiqiu/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 15:
                        var url = "http://rrl-game2.sdoudou.top/qiqiu2/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 16:
                        var url = "http://rrl-game2.sdoudou.top/qiqiu10/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 17:
                        var url = "http://rrl-game2.sdoudou.top/chebiao/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 18:
                        var url = "http://rrl-game2.sdoudou.top/honghei/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                    case 19:
                        var url = "http://sztest.rrlsz.com.cn/index.html?token=" + encodeURIComponent(this.token) + "&ver=" + ver + "&platform=2&paytype=" + payType + redirect_current_href_encode;
                        location.href = url;
                        break;
                }
            }
        },
        /**
        * 点击休闲游戏跳转不同游戏页面
        */
        iframeVal(val) {
            val=val.replace("iframeVal('"," ");
            val=val.replace("');"," ");
            console.log(val);
            this.OpenThirdPartyUrl(val);
        },
        OpenThirdPartyUrl(url) {
            if (url.length > 0) {
                var redirect = location.href;
                location.href = StringUtils.format("/Game/ThirdPartyGames?iframe={0}&redirect={1}", encodeURIComponent(url), redirect);
            }
        },

         /**
         * 获取休闲游戏列表
         */
        GetOtherGameList:function(){
            var that=this;
            $.post("/WebApi/game/GameBaseData", { "type": 2 },(res)=> {
                if(res.status==0){
                    var data = res.data
                    var isWeixin = ua.indexOf('micromessenger') != -1;
                    if(isWeixin){
                    		for (var i = 0; i < data.length; i++) {
                        		var item = data[i];
                        		if (item.is_show_at_wechat == "1") {
                                that.pageArr.push(item);
                           }
                    		}
                    }
                    else {
                    		for (var i = 0; i < data.length; i++) {
                        		var item = data[i];
                        		that.pageArr.push(item);
                    		}
                    }
                }
            })
        },

        /**
         * 获取用户信息
         */
        GetUserInfo:function(){
            var  that=this;
            if(!that.token){
                return;
            }
            $.post("/WebApi/Game/GetUserInfo", {token: this.token},(res)=> {
                if(res.status==0){
                    let data=res.data;
                    if(data.level_category=='A'){
                        that.showA=true;
                    }
                    that.redPacket=data.h_money_free;
                    that.JDPacket=data.h_money;
                    that.vRedpacket=data.h_money_pay;
                    window.h_money_pay = data.h_money_pay;
                    that.$refs.bottomCOM.showRedSup(); // call子组件方法
                }
            })
        },
        /**
        * 点击弹出提示框
        */
        lhbTip:function(){

            confirm(`<ul>
                        <li>1. 当红包和金豆总数少于一定数值时，系统会自动发放指定红包进行游戏；---</li>
                        <li>2. 平台每日不定时发放V红包，V红包只能用于指定游戏，且不可以在平台直接参与购物以及兑换；</li>
                        <li>3. 在领取下一个V红包时上一个V红包的金额和所玩的局数自动清零，不累计计算；V红包需要在指定游戏内完成指定局数并满足其他规定条件后才可转化为金豆；</li>
                        <li>4. 金豆可以直接在平台根据规则购物及兑换；</li>
                        <li>5. 如果用户存在违规行为（包括但不限于洗钱、虚假交易、赌博、恶意套现、作弊、刷信誉），主办方将取消用户的活动资格、并有权撤销相关违规交易、收回奖励（包括已消费金额）等利益，同时依照相关规则进行处罚。</li>
                        <li>6. 如出现不可抗力或情势变更的情况（包括但不限于重大灾害事件、活动受政府机关指令需要停止举办或调整的、活动遭受严重网络攻击或因系统故障需要暂停举办的），则主办方可依相关法律法规的规定主张免责。</li>
                        </ul>`
            , function (){
            })
        },
        /**
        * 点击分享领取红包
        */
        share:function () {
            var that=this;
            if(!this.token){
                confirm('尚未登录,是否登录？',function () {
                    that.$router.push({path:'/login',query:{redirect:('/#/gamehall')}});
                });
                return;
            }
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
         * 购券送豆
         */
        buy_gold_bean:function () {
          var that=this;
          if(!this.token){
                confirm('尚未登录,是否登录？',function () {
                    that.$router.push({path:'/login'});
                });
                return;
            }
            window.location.href = "/Game/PeasPay?token=" + encodeURIComponent(this.token) + "&redirect=" + encodeURIComponent(location.href);
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
         * 获取游戏大厅的轮播图数据
         */
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
                            if (item.type == 1) {
                                that.swiperSlides.push(item);
                            }
                        }
                    }
                }
            })
        },
        /**
       * 点击轮播图跳转指向
       */
        goBanner(url) {
            if (!url) return;
            if (url.indexOf("javascript:") > -1) {
                eval(url.replace("javascript:", ""));
                return;
            }
            //debugger;
            if(url.indexOf("/LeisureGameArea")>-1)
            {
              if(this.is_show_game=="0")
            {
              return;
            }
            }
            window.location.href = url.replace("#token", this.token);
        },
        	method_get_hold_money()
        	{
        		return parseInt(this.data_hold_money/10000);
        	},
        	//获取配置列表
        	method_ajax_getUserHoldMoneyConfig()
        	{
        		var this_vm=this;
        		$.ajax({
                    url: "/WebApi/UserManager/GetUserHoldMoneyConfig?token="+this_vm.token,
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
            jdDetail(){
               this.showRule=true;
            },
            hideBtn(){
              this.showRule=false;
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
                                    that.enter_zp=true;
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
                                                elText += res.data.PayMoney.Msg+ "<br />";
                                            }
                                            if (res.data.BeansMoney.Money > 0) {
                                                elText += res.data.BeansMoney.Msg;
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
                    that.data_timeout_id=setTimeout(function(){
                        that.loopPage();
                        that.getTimeStateBsx();
                        that.getTimeStateBdx();
                    }, 2000);
            },
            closeRedFree:function(){
               $('.maskxx').css('display', 'none');
            },

         }
      };
    export default  gamehall;
</script>

