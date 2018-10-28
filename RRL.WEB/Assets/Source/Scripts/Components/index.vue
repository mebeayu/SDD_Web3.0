<!--首页-->
<template>
    <div class="home-wrap">
        <!--顶部主要区域-->
        <div class="wj-top-navbar">
            <!--顶部搜索区-->
            <div class="wj-top-index-search-cont">
                <router-link tag="div" to='/searchpage' class="wj-top-search">
                    <img src="Assets/Source/img/搜索@3x.png" alt="">
                    <span>输入商品关键词或者用户名</span>
                </router-link>
                <div @click="CustomerService">
                    <img src="/Assets/Source/img/index-msg-custorm-service.png" alt=""/>
                </div>
                <router-link tag="div" to="/mymsgpush">
                    <img src="/Assets/Source/img/index-msg-push.png"/>
                    <span v-if="msgPushVal">{{ msgPushVal | filterMsgData }}</span>
                </router-link>
            </div>
            <!--顶部搜索区-->

            <!--导航区-->
            <nav class="wj-top-nav" v-if="topNavList.length!==0">
                <ul>
                    <li v-for="(item,index) in topNavList"
                        :class="index===0?'cur':''"
                        @click="goUrlNavList(item)"
                        :key="item.id">{{ item.name }}
                    </li>
                </ul>
            </nav>
            <!--导航区-->
        </div>
        <!--顶部主要区域-->

        <!--中间主要区域-->
        <div class="wj-center-scrollybar">
            <!--轮播图-->
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
            <!--轮播图-->

            <!--公告区-->
            <div v-if="topTipList.length!==0" class=" wj-top-anouncet">
                <div class="wj-doudou-anouncet-l">
                    <img src="Assets/Source/img/公告@3x.png" alt="">
                    <span class="wj-top-anouncet-head">兜兜公告：</span>
                </div>

                <div class="wj-doudou-anouncet">
                    <div id="demo" class="qimo8">
                        <div class="qimo">
                            <div id="demo1">
                                <ul>
                                    <router-link
                                            tag="li"
                                            v-for='item in topTipList'
                                            :to="{name: 'articledetail', params: {id: item.Id}}"
                                    ><!--@click="goArticleDetail(item)"-->
                                        <a href="javascript:;">{{item.title}}</a>
                                    </router-link>
                                </ul>
                            </div>
                            <div id="demo2"></div>
                        </div>
                    </div>
                </div>
            </div>
            <!--公告区-->

            <!-- 图标入口 -->
            <div class="border-h" v-show="false">
                <ul class="rrl_icon_ul">
                    <li v-for="(icon,index) in iconList" :key="index" >
                        <a :href="icon.url">
                            <img :src="icon.img" :alt="icon.title"/>
                            <p>{{icon.title}}</p>
                        </a>
                    </li>
                </ul>
            </div>
            <!-- 图标入口 -->

            <!--一元秒杀-->
            <div class="border-h wj-sentiment-recommend" v-if="secondkillPageList.length!==0">
                <a :href="secondkillPageList[0].url" class="second-kill-index-bg">
                    <img :src="'SWebApi/Public/picture/'+secondkillPageList[0].pic_id+'.jpg'"
                         :load="'/Assets/Source/img/默认banner.png'" alt=""/>
                </a>
            </div>
            <!--一元秒杀-->

            <!--人气推荐-->
            <div class="border-h wj-sentiment-recommend" v-if="sentimentRecList.length!==0">
                <h4 class="wj-top-header bg-rqtj">
                    <span>人气推荐</span>
                </h4>
                <div class="wj-more-right">
                    <img src="/Assets/Source/img/下一页@3x.png" alt="">
                </div>
                <ul class="wj-sentiment-recommend-list">
                    <router-link tag="li"
                                 v-for="(item,index) in sentimentRecList"
                                 :key="index"
                                 :to="{name:'goodsdetail',params: { goodsid: item.id ,username:'null' }}"
                    >
                        <div class="wj-sentiment-recommend-big">
                            <img
                                    :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"

                                    :load="'/Assets/Source/img/默认banner.png'"
                            >
                        </div>
                        <!--<div class="wj-sentiment-recommend-small">
                            <img src="" alt="">
                            <img src="" alt="">
                        </div>-->
                        <div class="wj-sentiment-recommend-head">{{ item.name }}</div>
                        <!-- <p class="wj-sentiment-recommend-til">{{ item.propaganda }}</p> -->
                        <p class="wj-sentiment-recommend-price">￥{{ item.price }}</p>
                    </router-link>
                </ul>
            </div>
            <!--人气推荐-->

            <!--游戏专区-->
            <div class="border-h wj-game-zone" v-if="is_show_game=='1'"><!--||gameZoneList.length!==0"-->
                <h4 class="wj-top-header bg-yxzq">
                    <span>游戏中心</span>
                </h4>

                <div class="wj-game-zone-banner">
                    <img v-for="(item,index) in gameZoneList" :key="index" @click="goGame(item)"
                         :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"
                         :load="'/Assets/Source/img/默认banner.png'"
                    >
                </div>
                <!--<ul class="wj-game-zone-list">
                     <li @click="othergame(2)" tag="li"><img src="Assets/Source/img/中国风十二生肖拷贝3@3x.png"
                                                             alt=""><span>生肖乐</span></li> 
                    <li @click="othergame(0)" tag="li"><img src="Assets/Source/img/转盘设置@3x.png" alt=""><span>小转盘</span>
                    </li>

                    <li @click="othergame(3)" tag="li"><img src="Assets/Source/img/椭圆6拷贝2@3x.png"
                                                            alt=""><span>翻扑克</span></li>
                </ul>-->
            </div>
            <!--游戏专区-->

            <!--特色专区-->
            <div class="border-h wj-special-zone" v-if="specialZoneList.length!==0">
                <h4 class="wj-top-header bg-tszq">
                    <span>特色专区</span>
                </h4>
                <ul class="wj-special-zone-list">
                    <li v-for="(item,index) in specialZoneList"
                        :key="index"
                        @click="goUrlSpecialZone(item)"
                    >
                        <img
                            :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"

                            :load="'/Assets/Source/img/默认banner.png'"
                        >
                    </li>
                    <!--<li>
                        <img src="https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/50831.jpg" alt="">
                    </li>
                    <li>
                        <img src="/Assets/Source/img/wj-like-buy.png" alt="">
                    </li>
                    <li @click="goUrlSpecialZone">
                        <img src="/Assets/Source/img/wj-like-sell.png" alt="">
                    </li>-->
                </ul>
            </div>
            <!--特色专区-->

            <!--新品首发-->
            <div class="border-h wj-new-start" v-if="newStartList.length!==0">
                <h4 class="wj-top-header bg-xpsf">
                    <span>新品首发</span>
                </h4>
                <div class="wj-more-right">
                    <img src="/Assets/Source/img/下一页@3x.png" alt="">
                </div>
                <ul class="wj-new-start-list">
                    <router-link tag="li"
                                 v-for="(item,index) in newStartList"
                                 :key="index"
                                 :to="{name:'goodsdetail',params: { goodsid: item.id ,username:'null' }}"
                    >
                        <div class="wj-new-start-pic">
                            <img
                                    :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"

                                    :load="'/Assets/Source/img/默认banner.png'"
                            >
                        </div>
                        <p class="wj-new-start-til">{{ item.name }}</p>
                        <p class="wj-new-start-price">￥{{ item.price }}</p>
                    </router-link>
                </ul>
            </div>
            <!--新品首发-->

            <!--精选专题-->
            <div class="border-h wj-select-topics" v-if="selectList.length!==0">
                <h4 class="wj-top-header bg-jxzt">
                    <span>精选专题</span>
                </h4>
                <div class="wj-more-right">
                    <img src="/Assets/Source/img/下一页@3x.png" alt="">
                </div>
                <div class="wj-select-topics-list">
                    <!--<van-swipe :show-indicators="false"
                               :loop="true"
                               :initial-swipe="0"
                    >
                        <van-swipe-item v-for="(item, index) in selectList"
                                        :key="index"
                                        @click.native="goUrlSelectList(item)"
                        >
                            <img v-lazy="'SWebApi/Public/picture/' + item.pic_id+'.jpg'">
                        </van-swipe-item>
                    </van-swipe>-->
                    <ul style="padding-bottom: .4rem">
                        <li v-for="(item,index) in selectList"
                            :key="index"
                            @click="goUrlSelectList(item)"
                        >
                            <div class="wj-select-topics-pic">
                                <img
                                        :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"

                                        :load="'/Assets/Source/img/默认banner.png'"
                                >
                            </div>
                            <h5 v-if="jxztShow" class="wj-select-topics-head">精选专题精选专题精选专题精选专题</h5>
                            <p v-if="jxztShow" class="wj-select-topics-til">精选专题精选专题精选专题精选专题</p>
                        </li>
                    </ul>
                </div>
            </div>
            <!--精选专题-->

            <!--首页浮窗区域开始-->
            <div v-if="bottomFixedWindow.length!==0">
                <div v-if="is_bottom_fixed_window" class="wj-float-window">
                    <img
                            @click="toFixedWindow(bottomFixedWindow[0].float_url)"
                            :src="bottomFixedWindow[0].float_picUrl" alt=""/>
                    <div @click="fixedWindowFlag">
                        <img src="Assets/Source/img/colse_btn_fixed.png" alt=""/>
                    </div>
                </div>
            </div>
            <!--首页浮窗区域结束-->

        </div>
        <!--中间主要区域-->

        <!-- 3000红包领取 -->
        <div class="red_packet_3k" v-show="red_packet_3k">
            <div class="red_packet_3k_box">
                <img  id="red_packet_3k" src="/Assets/Source/img/3000hbv3.png" alt="3000红包">
                <div id="closeBtn_3k" class="closeBtn_3k" @click="closeBtnSk"></div>
                <a @click="gotoGamehall()" >
                    <div id="receiveBtn_3k" class="receiveBtn_3k"></div>
                </a>
            </div>
        </div>
        <!-- 3000红包领取 -->
        
        <!-- 游戏自动红包领取 -->
        <div class="red_packet_auto" v-show="red_packet_auto">
            <div class="red_packet_auto_box">
                <img  id="red_packet_auto" src="/Assets/Source/img/game_first_hb.png" alt="2000红包">
                <div id="closeBtn_auto" class="closeBtn_auto" @click="closeBtnAuto"></div>
                <a @click="gotoLogin()" >
                    <div id="receiveBtn_auto" class="receiveBtn_auto"></div>
                </a>
            </div>
        </div>
        <!-- 游戏自动红包领取 -->
        
        <!--底部导航-->
        <bottom></bottom>
        <!--底部导航-->
    </div>
</template>

<style scoped>
    /* 2018.8.20 */
    .rrl_icon_ul{
        display: -webkit-flex;
        display: flex;                /*设置为flex布局*/
        justify-content: center;   /*水平居中*/
        align-items: center;   
        flex-wrap: wrap;
        padding: .3rem .1rem;     
    }
    .rrl_icon_ul li{
        width:20%;
        height: 2.5rem;
        text-align: center;
        color: #333;
    }
    .rrl_icon_ul li img{
        width: 1.4rem;
        height: 1.4rem;
    }
    .rrl_icon_ul li p{
        text-decoration: none;
        color: #333;
    }


    /* 修改  */
    .red_packet_3k {
        width: 100%;
        height: 100%;
        background:rgba(0,0,0,.5);
        position: absolute;
        top: 0;
    }
    .red_packet_3k_box{
        width:6.79637526652452rem;
        height:7.729211087420042rem;
        position: absolute;
        top: 0;
        left:0;
        bottom: 0;
        right: 0;
        margin: auto;
        z-index: 11;
    }
    .red_packet_3k_box img{
        width: 100%;
        height: 100%;
    }
    .closeBtn_3k{
        width: 1rem;
        height: 1rem;
        position: absolute;
        top: 0;
        right: 0;
    }
    .receiveBtn_3k{
        width: 4.5rem;
        height: 1.1rem;
        position: absolute;
        bottom: 12%;
        right: 18%;
        text-align: center;
        line-height:  1.1rem;
        color: #fff;
        letter-spacing: .1rem;
        font-size: .4rem;
        font-family: '黑体',sans-serif;
    }
    .red_packet_auto {
        width: 100%;
        height: 100%;
        background:rgba(0,0,0,.5);
        position: absolute;
        top: 0;
    }
    .red_packet_auto_box{
        width:6.79637526652452rem;
        position: absolute;
        top: 15%;
        left:0;
        bottom: 0;
        right: 0;
        margin: auto;
        z-index: 11;
    }
    .red_packet_auto_box img{
        width: 100%;
        height: auto;
    }
    .closeBtn_auto{
        width: 1rem;
	    height: 1rem;
	    position: absolute;
	    top: 1.3rem;
	    right: .1rem
    }
    .receiveBtn_auto{
	    width: 5rem;
        height: 6.2rem;
        position: absolute;
        top: 23%;
        right: 14%;
        text-align: center;
        line-height: 1.1rem;
        color: #fff;
        letter-spacing: .1rem;
        font-size: .4rem;
        font-family: '黑体',sans-serif;
    }
    * {
        margin: 0;
        padding: 0
    }

    button, input {
        outline: none
    }

    .qimo8 {
        overflow: hidden;
    }

    .qimo8 .qimo {
        width: 8000%;
        height: 100%;
    }

    .qimo8 .qimo div {
        float: left;
        height: 100%;
    }

    .qimo8 .qimo ul {
        float: left;
        height: 100%;
        overflow: hidden;
        zoom: 1;
    }

    .qimo8 .qimo ul li {
        float: left;
        line-height: .986666667rem;
        list-style: none;
    }

    .qimo8 li a {
        display: block;
        margin-right: .266666667rem;
        color: rgb(122, 123, 135);
        heigth: 100%;
    }

    .wc-pagination .wc-dot {
        width: .16rem;
        height: .16rem;
        background: #fff;
    }

    .slide {
        text-align: center;
        line-height: 200px;
        font-size: 24px;
        width: 100%;
        height: 200px;
        margin-bottom: 50px;
        margin-top: 10px;
    }

    .c-swipe-item:nth-child(odd) {
        background: #9f0
    }

    .c-swipe-item:nth-child(even) {
        background: lightcoral;
    }

    .item4 {
        background: #4ff
    }

    .slide .c-slide-pagination-item.active {
        background-color: #fff;
    }

    .swiper-slide {
        display: block;
        width: 100%;
        height: 100%;
    }

    .van-swipe__indicators > i {
        width: .16rem;
        height: .16rem;
        background: #fff !important;
        margin-right: .16rem;
    }

    .van-swipe__indicator--active {
        background: #65c4aa;
    }

    .van-swipe-item img {
        display: block;
        width: 100%;
        height: 100%;
    }

    .wj-doudou-anouncet {
        margin-right: .26666666666666666rem;
    }

    .wj-sentiment-recommend-list, .wj-select-topics-list {
        padding-top: .266667rem; /* 暂时显示 */
    }

    .wj-more-right {
        display: none; /* 暂时隐藏 */
        margin-top: -.266667rem;
        padding: 0 .4rem .266667rem 0;
        overflow: hidden
    }

    .wj-more-right img {
        float: right;
        width: .48rem;
        height: .48rem
    }

    .wj-bottom-navbar, .wj-center-scrollybar, .wj-top-navbar {
        position: absolute;
        left: 0;
        width: 100%;
        background: #fff
    }

    .wj-center-scrollybar {
        top: 2.106667rem;
        bottom: 1.306667rem;
        overflow: hidden;
        overflow-y: scroll;
        touch-action: pan-y;
        -webkit-overflow-scrolling: touch;
    }

    .wj-top-navbar {
        top: 0
    }

    .wj-select-topics-list {
        padding: .266667rem .4rem 0;
    }

    .wj-select-topics-list ul {
        -ms-flex-direction: row;
        flex-direction: row;
        width: 100%;
        white-space: nowrap;
        overflow-x: scroll
    }

    .wj-select-topics-list li, .wj-select-topics-list ul {
        display: -ms-flexbox;
        display: flex
    }

    .wj-select-topics-list li {
        -ms-flex-direction: column;
        flex-direction: column;
        border-radius: .053333rem;
        padding-right: .266667rem
    }

    .wj-select-topics-list li:last-child {
        padding-right: 0;
    }

    .wj-select-topics-pic {
        height: 4.266667rem
    }

    .wj-select-topics-head, .wj-select-topics-pic, .wj-select-topics-til {
        width: 7.626667rem
    }

    .wj-select-topics-pic img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-select-topics-head {
        font-weight: 400;
        font-size: .373333rem;
        color: #2d2d34;
        padding: .32rem 0 .186667rem
    }

    .wj-select-topics-til {
        font-size: .32rem;
        color: #7a7b87;
        padding-bottom: .4rem
    }

    .wj-select-topics-head, .wj-select-topics-til {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden
    }

    .wj-new-start-list {
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
        -ms-flex-pack: justify;
        justify-content: space-between;
        padding: 0 .4rem
    }

    .wj-new-start-list, .wj-new-start-list li {
        display: -ms-flexbox;
        display: flex
    }

    .wj-new-start-list li {
        -ms-flex-direction: column;
        flex-direction: column;
        /*        -ms-flex: 1;
                flex: 1;*/
        width: 47%;
        margin-right: .4rem
    }

    .wj-new-start-list li:nth-of-type(2n) {
        margin-right: 0
    }

    .wj-new-start-list li:last-of-type, .wj-new-start-list li:nth-last-of-type(2) {
        padding-bottom: .266667rem
    }

    .wj-new-start-pic {
        width: 4.2rem;
        height: 4.2rem;
        margin-top: .266667rem;
        overflow: hidden
    }

    .wj-new-start-pic img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-new-start-til {
        margin: .266667rem 0 .4rem;
        color: #303037;
        font-size: .373333rem;
        text-overflow: ellipsis;
        display: -webkit-box;
        white-space: normal;
        -webkit-line-clamp: 2;
        height: 1.013333rem;
        line-height: .533333rem;
        overflow: hidden
    }

    .wj-new-start-price {
        padding-bottom: .266667rem;
        font-size: .373333rem;
        color: #e50008;
    }

    .wj-special-zone-list {
        position: relative;
        margin-top: -.266667rem;
        z-index: 1;
        padding: 0 .4rem .4rem;
        overflow: hidden;
    }

    .wj-special-zone-list li {
        float: left;
        width: 48%;
        height: 4rem;
        border-radius: .053333rem;
        margin-right: 2%;
        overflow: hidden
    }

    .wj-special-zone-list li:nth-of-type(2), .wj-special-zone-list li:nth-of-type(3) {
        float: right;
        height: 1.8266665rem;
        margin-right: 0;

    }

    .wj-special-zone-list li:nth-of-type(3) {
        margin-top: 0.29rem;
    }

    .wj-special-zone-list li img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-game-zone-banner {
        position: relative;
        margin-top: -.5rem;
        z-index: 0;
        height: 3.466667rem;
        overflow: hidden
    }

    .wj-game-zone-banner img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-game-zone-list {
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-pack: center;
        justify-content: center;
        -ms-flex-line-pack: center;
        align-content: center;
        -ms-flex-align: center;
        align-items: center;
        -ms-flex-wrap: nowrap;
        flex-wrap: nowrap
    }

    .wj-game-zone-list, .wj-game-zone-list li {
        display: -ms-flexbox;
        display: flex
    }

    .wj-game-zone-list li {
        position: relative;
        -ms-flex-direction: column;
        flex-direction: column;
        -ms-flex: 1;
        flex: 1;
        /*height: 1.573333rem;*/
        margin: .64rem 0;
        text-align: center
    }

    .wj-game-zone-list li:after {
        position: absolute;
        right: 0;
        top: 0;
        content: "";
        width: .013333rem;
        height: 100%;
        background: #e3e4e8
    }

    .wj-game-zone-list li:nth-of-type(3n):after {
        display: none
    }

    .wj-game-zone-list li img {
        display: block;
        width: 1.013333rem;
        height: 1.013333rem;
        margin: auto;
        overflow: hidden
    }

    .wj-game-zone-list li span {
        display: block;
        text-align: center;
        color: #2d2d34;
        padding-top: .24rem;
        font-size: .373333rem
    }

    .wj-sentiment-recommend-list {
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
    }

    .wj-sentiment-recommend-list, .wj-sentiment-recommend-list li {
        display: -ms-flexbox;
        display: flex
    }

    .wj-sentiment-recommend-list li {
        width: 33.3333333333%;
        -ms-flex-pack: justify;
        justify-content: space-between;
        -ms-flex-direction: column;
        flex-direction: column
    }

    .wj-sentiment-recommend-big {
        width: 3.333333rem;
        height: 3.333333rem;
        overflow: hidden
    }

    .wj-sentiment-recommend-big img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-sentiment-recommend-small {
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row;
        padding: .266667rem .4rem
    }

    .wj-sentiment-recommend-small img {
        display: -ms-flexbox;
        display: flex;
        width: auto;
        height: .4rem;
        margin-right: .066667rem
    }

    .wj-sentiment-recommend-head {
        font-size: .353333rem;
        color: #303037;
        text-overflow: ellipsis;
        padding: .186667rem 0;
        display: -webkit-box;
        white-space: normal;
        -webkit-line-clamp: 2;
        height: 1.2rem;
        line-height: .65rem;
        overflow: hidden;
    }
    .wj-sentiment-recommend-til {
        font-size: .32rem;
        color: #7a7b87;
        padding-bottom: .4rem
    }

    .wj-sentiment-recommend-price {
        font-size: .373333rem;
        color: #e50008;
        padding-bottom: .24rem
    }

    /* .wj-sentiment-recommend-head, .wj-sentiment-recommend-til {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden
    } */

    .wj-sentiment-recommend-head, .wj-sentiment-recommend-price, .wj-sentiment-recommend-til {
        margin: 0 .2rem
    }

    .wj-sentiment-recommend-list li:last-of-type, .wj-sentiment-recommend-list li:nth-last-of-type(2), .wj-sentiment-recommend-list li:nth-last-of-type(3) {
        padding-bottom: .293333rem
    }

    .wj-top-header {
        position: relative;
        margin-top: -.12rem;
        z-index: 3;
        margin-left: .4rem;
        font-weight: 400
    }

    .wj-top-header span {
        position: relative;
        z-index: 2;
        display: block;
        width: 2.133333rem;
        height: .64rem;
        line-height: .64rem;
        text-align: center;
        color: #fff;
        font-size: .373333rem
    }

    .wj-top-header span:after {
        position: absolute;
        top: .053333rem;
        left: -.12rem;
        z-index: 1;
        content: "";
        width: 0;
        height: 0;
        border-left: .106667rem solid transparent;
        border-right: .106667rem solid transparent;
        border-bottom: .106667rem solid transparent;
        transform: rotate(136deg)
    }

    .bg-rqtj span:after {
        border-bottom-color: #5fa93a
    }

    .bg-rqtj span {
        background: #82c95f
    }

    .bg-yxzq span:after {
        border-bottom-color: #da5568
    }

    .bg-yxzq span {
        background: #fb8394
    }

    .bg-tszq span:after {
        border-bottom-color: #887331
    }

    .bg-tszq span {
        background: #d7b859
    }

    .bg-xpsf span:after {
        border-bottom-color: #d14b31
    }

    .bg-xpsf span {
        background: #ff7055
    }

    .bg-jxzt span:after {
        border-bottom-color: #47b7b6
    }

    .bg-jxzt span {
        background: #4dc3c2
    }

    .border-h {
        border-bottom: .293333rem solid #f3f4f5
    }

    .wj-top-anouncet {
        position: relative;
        -ms-flex-direction: row;
        flex-direction: row;
        height: .986667rem;
        line-height: .986667rem;
        padding: 0 .4rem;
        overflow: hidden
    }

    .wj-top-anouncet, .wj-top-anouncet img {
        display: -ms-flexbox;
        display: flex
    }

    .wj-top-anouncet img {
        width: .346667rem;
        height: .4rem;
        margin: .333333rem .106667rem 0 0
    }

    .wj-top-anouncet span.wj-top-anouncet-head {
        position: relative;
        z-index: 3;
        background: #fff;
        font-size: .373333rem;
        color: #65c4aa
    }

    .wj-doudou-anouncet-l {
        position: relative;
        background: #fff;
        z-index: 2;
        overflow: hidden;
    }

    .wj-doudou-anouncet-l img, .wj-doudou-anouncet-l img {
        float: left;
    }

    .wj-top-anouncet div.wj-doudou-anouncet {
        position: absolute;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row;
        white-space: nowrap;
        left: 2.7rem;
        font-size: .32rem;
        color: #7a7b87;
        overflow: hidden;
    }

    .wj-top-banner, .swiper-container {
        position: relative;
        height: 5.066667rem;
        /*background: rgba(0, 0, 0, .1);*/
        overflow: hidden
    }

    .wj-top-banner {
        background: url("/Assets/Source/img/status_loading.gif") no-repeat center rgba(0, 0, 0, .03);
    }

    .wj-top-banner img {
        display: block;
        width: 100%;
        height: 100%;
    }

    .wj-top-nav {
        overflow: hidden;
        width: 100%;
        white-space: nowrap;
        overflow-x: scroll
    }

    .wj-top-nav ul {
        display: block;
        width: 100%;
    }

    .wj-top-nav ul, .wj-top-nav ul li {
        height: .933333rem;
        line-height: .933333rem;
    }

    .wj-top-nav ul li {
        display: inline-block;
        height: .933333rem;
        line-height: .933333rem;
        width: 1.8rem;
        color: #7b7a87;
        font-size: .373333rem;
        padding: 0 .1rem;
        text-align: center;
    }

    .wj-top-nav ul li.cur {
        position: relative;
        color: #65c4aa
    }

    .wj-top-nav ul li.cur:after {
        position: absolute;
        left: 0;
        bottom: 0;
        content: "";
        width: 100%;
        height: .04rem;
        background-color: #65c4aa
    }

    .wj-top-search {
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-pack: center;
        justify-content: center;
        -ms-flex-line-pack: center;
        align-content: center;
        -ms-flex-align: center;
        align-items: center;
        height: .8rem;
        line-height: .8rem;
        background: #f4f4f4;
        border-radius: .053333rem;
        margin: .186667rem .266667rem;
    }

    .wj-top-search, .wj-top-search img {
        display: -ms-flexbox;
        display: flex
    }

    .wj-top-search img {
        width: .293333rem;
        height: .293333rem
    }

    .wj-top-search span {
        display: -ms-flexbox;
        display: flex;
        padding-left: .186667rem;
        font-size: .32rem;
        color: #83848f
    }

    h1[data-v-694cd902], h2[data-v-694cd902] {
        font-weight: 400
    }

    ul[data-v-694cd902] {
        list-style-type: none;
        padding: 0
    }

    li[data-v-694cd902] {
        display: inline-block;
        margin: 0 .133333rem
    }

    a[data-v-694cd902] {
        color: #42b983
    }

    .swiper-pagination.swiper-pagination-clickable.swiper-pagination-bullets {
        right: 0 !important;
        position: absolute;
        bottom: .4666666666666667rem;
        left: inherit;
        transform: translate3d(43%, 0, 0);
    }

    .slider-pagination-bullet {
        width: .16rem;
        height: .16rem;
        background: #fff none repeat scroll 0 0;
    }

    .slider-pagination-bullet-active {
        background-color: #65c4aa !important;
    }

</style>
<script>
    import $ from 'jquery'
    import Vue from 'vue'

    import {Swipe, SwipeItem} from 'vant';

    Vue.use(Swipe).use(SwipeItem);

    import bottom from "./bottom";
    import pageVisit from "./PublicModules/PageVisit.js";

    export default {
        components: {
            bottom
        },
        data() {
            return {
                topNavList: [],                // 顶部导航
                allList: [],                   // 所有轮播图数据
                swiperSlides: [],              // 顶部主轮播图
                topTipList: [],                // 顶部公告
                sentimentRecList: [],          // 人气推荐
                gameZoneList: [],              // 游戏专区
                specialZoneList: [],           // 特色专区
                newStartList: [],              // 新品首发
                selectList: [],                // 精选专题
                activePageList: [],            // 活动页
                secondkillPageList: [],        // 一元秒杀
                animate: false,
                jxztShow: false,
                is_show_game: null,              // 1显示 0不显示
                bottomFixedWindow: [],          // 右下角浮窗效果
                is_bottom_fixed_window: true,
                msgPushVal: "" ,   // 获取未读消息总数量
                red_packet_3k:false,
                red_packet_auto:false,
                iconList:[                           //图标链接                   
                    {img:'/Assets/Source/img/icon1.gif',title:'一元秒杀',url:'/Event/SecondKill'},
                    {img:'/Assets/Source/img/icon2.png',title:'天天领红包',url:'/#/getredpacket'},
                    {img:'/Assets/Source/img/icon4.gif',title:'红包雨',url:'http://rrl-game2.sdoudou.net/qiqiu10/index.html'},
                    {img:'/Assets/Source/img/icon3.png',title:'轮盘领现金',url:'http://e-shop.rrlsz.com.cn/gameimg/zzz/index.html'},
                    {img:'/Assets/Source/img/icon5.gif',title:'天天特卖',url:'/Game/PeasPay'},
                    {img:'/Assets/Source/img/icon6.png',title:'转卖换钱',url:'/#/secondhandarea'},
                    {img:'/Assets/Source/img/icon7.png',title:'闲置优品',url:'https://e-shop.szrundao.com/xianzhuan/product/index'},
                    {img:'/Assets/Source/img/icon8.png',title:'助力0元购',url:'/#/expect'},
                    {img:'/Assets/Source/img/icon9.png',title:'超值现金券',url:'/#/expect'},
                    {img:'/Assets/Source/img/icon10.png',title:'猜猜拿现金',url:'/#/expect'},
                ]        
            }
        },
        filters: {
            filterMsgData(val) {
                if (val >= 9) {
                    return 9 + "+";
                }
                return val;
            }
        },
        created() {
        	//this.method_is_have_redpackage();
        	var allParams=window.UI.Page.getAllparam();
        	if(allParams.token)
        	{
        		this.$store.commit('SET_SHORTTOKEN', allParams.token);
        	}
            this.is_show_game = window.is_show_game;

            // 获取消息提醒的数量
            this.getUnreadMessageCount();

            // 人气推荐和新品专区
            this.Ajax({
                "url": "ShopManager/GetGoodsTypeList/1.json?pagesize=" + 6,
                "successBack": data => {
                    this.sentimentRecList.push(...data.PopData);
                    this.newStartList.push(...data.NewData);
                }
            });
            // 渲染公告区域
            this.Ajax({
                "url": "Notice/GetNoticeList.json",
                "successBack": data => {
                    this.topTipList = data;
                    setInterval(this.scroll, 1000);
                }
            });
            // 渲染头部导航栏数据
            this.Ajax({
                "url": "ShopManager/SimpleGoodsTypeList.json",
                "successBack": data => {
                    this.topNavList = data;
                }
            });
            // 获取所有的轮播图数据
            this.Ajax({
                "url": "ConfigManager/MainCarousel.json",
                "successBack": data => {
                    this.allList = data || [];
                    this.allList.forEach(item => {
                        switch (item.type) {
                            case 1:  // 1.主轮播图
                                this.swiperSlides.push(item);
                                break;
                            /*case 2:  // 2.精选页
                              this.topBannerList.push(item);
                              break;*/
                            /*case 3: // 3.活动页
                              this.activePageList.push(item);
                              break;*/
                            /*case 4: // 4.兑换区
                              this.topBannerList.push(item);
                              break;*/
                            case 5: // 5.精选专区
                                this.selectList.push(item);
                                break;
                            case 6: // 6.游戏专区
                                this.gameZoneList.push(item);
                                break;
                            case 7: // 7.特色专区
                                this.specialZoneList.push(item);
                                break;
                            case 9: // 9.一元秒杀
                                this.secondkillPageList.push(item);
                                break;
                        }
                    });
                }
            });
            //  获取右下角浮窗效果数据
            this.getFloatWindowInfoAPI();


            // 获取iframe框架里面的二手交易 ==== 转卖详情
            window.getSecondHandResale = function getSecondHandResale() {
                var iframeVal = "";
                var localStorageVal = JSON.parse(localStorage.getItem("iframeSrc"));
                if (localStorageVal) {
                    iframeVal = localStorageVal;
                    window.location.href = iframeVal;
                    return;
                }
                $.ajax({
                    url: '/WebApi/TradeManager/ReSellDetails?token=' + localStorage.getItem("shorttoken"),
                    type: 'get',
                    success: function (res) {
                        if (res.status == 98) {
                            alert("请先登录，再去嗨皮~");
                            return;
                        }
                        if (res.status == 0) {
                            localStorage.setItem("iframeSrc", JSON.stringify(res.data));
                            var iframeVal = res.data;
                            window.location.href = iframeVal;
                        } else {
                            alert("请先登录，再去嗨皮~");
                        }
                    }
                });
            };

            //window.getSecondHandResale();
        },
        mounted() {
            setTimeout(this.ScrollImgLeft, 1000);
        },
        activated () {
        	this.method_is_have_redpackage();
        	pageVisit.logVisitTime({pageType:'index', goodsId:0, visitPath:location.href, deep:1});
        	
            this.getUnreadMessageCount();
            this.GameAutoRedpackage(); // 逼充0.2送免费红包
		},
	    deactivated () {
	    	try{
	    		pageVisit.logLeaveTime();
	    	}catch(e){}
	    },
        methods: {
            // 逼充0.2送免费红包
            GameAutoRedpackage(){
            	var that=this;
            	if(this.token){
                	// 红包+金豆不足时，逼充0.2送免费红包
                	$.post("/WebApi/Game/GameAutoRedpackageDaily", {token: this.token, source: 'gamehall'},
                		function(res){
                    	if(res.status == 0 && res.data){
                    		if(res.data['isPromptRecharge'] == 1){
                          window.gameAlert(that.token);
                        }
                    	};
                    }
                	);
                }
            },
            //3000红包领取
            closeBtnSk(){
                this.red_packet_3k=false;
            },
            closeBtnAuto(){
            	this.red_packet_auto = false;
            },
            gotoGamehall(){
            	this.red_packet_3k=false;
            	let token=localStorage.getItem("shorttoken");
            	if(!token)
            	{
            		this.$router.push({path:"/login",query:{redirect:('/#/index')}});
            		return;
            	}
            	this.$router.push({path:"/gamehall"});
            },
            gotoLogin(){
            	this.red_packet_auto = false;
            	let token=localStorage.getItem("shorttoken");
            	if(!token)
            	{
            		this.$router.push({path:"/login",query:{redirect:('/#/gamehall')}});
            		return;
            	}
            	this.$router.push({path:"/gamehall"});
            },
            method_is_have_redpackage()
            {
            	 
            	var that=this;
            	var token=localStorage.getItem("shorttoken");
            	if(!token)
            	{
            		that.red_packet_3k=true; 
            		return;
            	}
            	
            	var endtime = new Date().getTime();//用户退出时间
                var duration = Math.floor((endtime - starttime) / 1000);
                var rdcode = String(Math.random()).split(".")[1];
                var starttime = new Date().getTime();
                
                $.post("/WebApi/Game/SetState",
                {starttime: starttime,endtime: endtime,duration:duration, rdcode: rdcode, token: token},
                function(res){
                    if(res.data==true){//分时段小红包
                          that.red_packet_3k=true; 
                    } ;
                    /*if(res.additional_data===true){//针对个人发红包
                    }*/
                    });
                
                if(token){
                	// 红包+金豆不足时，自动发红包
                	$.post("/WebApi/Game/GameAutoRedpackageDaily", {token: token},
                		function(res){
                    		if(res.status == 0 && res.data){
                    			if(res.data['isPromptRecharge'] == 1){
                          			window.gameAlert(that.$store.state.shorttoken); 
                          		}
                    	    };
                        }
                	);
                }
            },
            
            // 客服系统
            CustomerService() {
                let iframe = encodeURIComponent("https://kefu.easemob.com/webim/im.html?configId=8d149d8f-bc70-45f8-9c44-96c6ef6098c9");
                window.location.href = "/Game/CustomerService?iframe=" + iframe + "&redirect=" + encodeURIComponent("/#/index");
            },
            // 获取未读消息总数量
            getUnreadMessageCount() {
                let _this = this;
                if (!this.$store.state.shorttoken) return;
                $.ajax({
                    url: "/WebApi/Message/UnreadMessageCount",
                    data: {
                        "token": _this.$store.state.shorttoken
                    },
                    type: "post",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            _this.msgPushVal = res.data;
                        }
                    }
                });
            },
            // 浮窗关闭的事件
            fixedWindowFlag() {
                this.is_bottom_fixed_window = false;
            },
            // 浮窗跳转至的页面
            toFixedWindow(url) {
                if (url) {
                    window.location.href = url.replace("#token", this.$store.state.shorttoken);
                }
            },
            //  
            getFloatWindowInfoAPI() {
            	 
                let _this = this;
                $.ajax({
                    url: "/Webapi/notice/Float_Window_info",
                    type: "get",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            _this.bottomFixedWindow = res.data;
                        }
                    }
                });
            },
            // 公告左右滚动
            ScrollImgLeft() {
                let demo = document.getElementById("demo");
                let demo1 = document.getElementById("demo1");
                let demo2 = document.getElementById("demo2");
                if(demo1&&demo2)
                	demo2.innerHTML = demo1.innerHTML;

                function Marquee() {
                    if (demo.scrollLeft - demo2.offsetWidth >= 0) {
                        demo.scrollLeft -= demo1.offsetWidth;
                    }
                    else {
                    	if(demo)
                        	demo.scrollLeft++;
                    }
                }

                setInterval(Marquee, 30);
            },
            // 点击精选专题跳转至每个页面
            goUrlSelectList(item) {
                if (!item.url) return;
                if (item.type === 5) {
                    window.location.href = item.url;
                    return;
                }
                this.$router.push({name: 'goodsdetail', params: {goodsid: item.id, username: 'null'}});
            },
            // 点击特色专区跳转至每个页面
            goUrlSpecialZone(item) {
                if (!item.url) return;

                if (item.type === 7) {
                    if (item.url.indexOf("javascript:") > -1) {
                        eval(item.url.replace("javascript:", ""));
                        return;
                    }
                    var url = item.url.replace("#token", this.$store.state.shorttoken);
                    window.location.href = url;
                    return;
                }

                this.$router.push({name: 'goodsdetail', params: {goodsid: item.id, username: 'null'}});
            },
            // 点击顶部导航跳转至每个页面
            goUrlNavList(item) {
                if (item.redirect_type === "0") {
                    this.$router.push('/goodslistthird/' + item.id);
                    return;
                }
                if (item.redirect_type === "1") {
                    if (item.redirect_url.indexOf("http") === -1) {
                        this.$router.push(item.redirect_url);
                    } else {
                        window.location.href = item.redirect_url;
                    }

                    return;
                }
                let url = item.redirect_url.replace("#token", this.$store.state.shorttoken);
                window.location.href = url;
            },
            //点击轮播图跳转指向
            goBanner(url) {
            	 
                if (!url) return;
                if (url.indexOf("javascript:") > -1) {
                    eval(url.replace("javascript:", ""));
                    return;
                }
                // 记录轮播图访问的时间点
                if(url.indexOf("/LeisureGameArea")>-1)
                {
                	if(this.is_show_game=="0")
	            	{
	            		return;
	            	}
                }
                this.logBannerVisit(url);
                
                window.location.href = url.replace("#token", this.$store.state.shorttoken);
            },
            // 封装ajax
            Ajax(opt) {
                $.ajax({
                    url: '/SWebApi/' + opt.url,//数据的接口的路径
                    dataType: 'json',
                    type: opt.type || "get",
                    data: opt.param || "",
                    async: opt.async || true,
                    timeout: 10000,
                    cache:true,
                    complete: (xhr, textStatus) => {
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(xhr.responseJSON.message);
                            return;
                        }
                        // 成功
                        if (xhr.responseJSON.status === 0) {
                            opt.successBack instanceof Function && opt.successBack(xhr.responseJSON.data);
                        }
                    }
                });
            },
            // 跳转至游戏页面
            goGame(item) {
                var url = item.url;
                if (!url) return;
                window.location.href = url.replace("#token", this.$store.state.shorttoken);
            },
            // 游戏下方的三个图标
            othergame(sta) {
                let date = new Date();
                let ver = (date.getTime() / 100).toFixed(0);
                if (this.$store.state.shorttoken.length != 0) {
                    switch (sta) {
                        case 0 :
                            location.href = "/Game/Roulette_V2/?token=" + encodeURIComponent(this.$store.state.shorttoken);
                            break;
                        case 1 :
                            location.href = "http://rrl-game.sdoudou.net:8360/static/dazhuanpan/?token=" + encodeURIComponent(this.$store.state.shorttoken) + "&ver=" + ver + "&";
                            break;
                        case 2:
                            location.href = "http://game.rrlsz.com.cn:8096/sxl/index.html?token=" + encodeURIComponent(this.$store.state.shorttoken) + "&ver=" + ver + "&";
                            break;
                        case 3:
                            location.href = "http://rrl-game.sdoudou.net:8096/fkp/index.html?token=" + encodeURIComponent(this.$store.state.shorttoken) + "&ver=" + ver + "&";
                            break;
                        case 4:
                            location.href = "http://game.rrlsz.com.cn:8360/static/caidaxiao/?token=" + encodeURIComponent(this.$store.state.shorttoken) + "&ver=" + ver + "&";
                            break;
                        case 5:
                            location.href = "http://game.rrlsz.com.cn:8360/static/xiaozhuanpan/?token=" + encodeURIComponent(this.$store.state.shorttoken) + "&ver=" + ver + "&";
                            break;
                    }
                } else {
                    confirm('尚未登录,是否登录？', () => {
                        this.$router.push('/login?to=' + encodeURIComponent("/#/GameHall?token=" + (this.$store.state.shorttoken)))
                    })
                }
            },
            logBannerVisit(url){
            	let visitTime = new Date().getTime();
            	let data = {
            		token: localStorage.getItem("shorttoken"),
					visitTime: visitTime,
					leaveTime: visitTime,
					visitInterval: 0,
					pageType: 'banner',
					goodsId: 0,
					visitPath: url,
					deep: 2
            	};
            	pageVisit.logPageVisitOnly(data);
            }
        }
    }
</script>