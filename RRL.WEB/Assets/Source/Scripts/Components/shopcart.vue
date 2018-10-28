<!-- 购物车页 -->
<template>
    <div class="bg">
        <!--头部导航开始-->
        <header class="wj-top-navbar">
            <h4>购物车</h4>
            <button v-if="shopCartListY.length!==0" @click="changeEditor">编辑</button>
        </header>
        <!--头部导航结束-->

        <!--中间主要内容开始-->
        <div class="wj-center-cont" @click="skip" :class="{'wj-add-bottom':!isGetComputShow}">
            <!--=========== 购物车区域开始 ===========-->
            <div class="wj-goods-carts">
                <!--购物车有内容开始-->
                <div v-if="shopCartListY.length!==0" class="wj-goods-carts-yes">
                    <ul class="wj-goods-carts-list">
                        <router-link tag="li" class="li-remove-scroll-bar"
                                     v-for="(item,index) in shopCartListY"
                                     :to="{name:'goodsdetail',params: { goodsid: item.goods_id,username:'null' }}"
                                     data-type="0">
                            <div class="li-remove-scroll"
                                 @touchstart.capture="touchStart"
                                 @touchend.capture="touchEnd"
                                 @click="skip">
                                <div class="wj-goods-carts-list-box wj-arrow-remove-item">
                                    <div class="wj-goods-carts-list-l">
                                        <label @click.stop="setCheckedY(item)">
                                            <input type="checkbox" name="checkbox"
                                                   class="mgr mgr-success mgr-lg"
                                                   :checked="item.isCheckedYes"/>
                                        </label>
                                        <img v-lazy="$store.state.imgs+item.pic_id+$store.state.img_suffix"
                                             class="pic"/>
                                    </div>
                                    <div class="wj-goods-carts-list-r">
                                        <p class="til">{{ item.goods_name }}</p>
                                        <p class="spec" style="display:none">{{ item.propaganda }}</p>
                                        <!--<p class="spec">220*230cm</p>-->
                                        <div class="item">
                                            <span class="price">￥{{(item.price).toFixed(2)}}</span>
                                            <div v-show="isChangeText" class="count-btn">
                                                <input @click.stop="changeNum(item,-1)" type="button" value="-"/>
                                                <input @click.stop="cancel" type="button" readonly
                                                       :value="item.goods_count"/>
                                                <input @click.stop="changeNum(item,1)" type="button" value="+"/>
                                            </div>
                                            <span v-show="!isChangeText"
                                                  class="count">×{{ item.goods_count }}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <button
                                    @click.stop="deleteItem(item)"
                                    class="remove-item">删除
                            </button>
                        </router-link>
                    </ul>
                </div>
                <!--购物车有内容结束-->

                <!--购物车没有内容开始-->
                <div v-if="shopCartListY.length===0" class="wj-goods-carts-no">
                    <img src="/Assets/Source/img/goods-cart-no.png" alt=""/>
                    <p>购物车还是空的</p>
                    <p>快去挑选几件喜欢的商品吧~</p>
                    <button @click="goStroll">去逛逛</button>
                </div>
                <!--购物车没有内容结束-->

                <!--游戏大厅图片区域开始-->
                <div   @click="goGameHall" class="wj-goods-game" v-if="is_show_game==1">
                    <!--<img src="/Assets/Source/img/game-banner-big.jpg" alt="">-->
                    <img src="/Assets/Source/img/gift-leisure.png" alt="">
                    <div style="display:none">
                        <p class="head">游戏大厅</p>
                        <p class="til">玩游戏 奖励金豆 购商品</p>
                    </div>
                </div>
                <!--游戏大厅图片区域结束-->

            </div>
            <!--=========== 购物车区域结束 ===========-->

            <!--=========== 失效商品区域开始 ===========-->

            <div class="wj-dead-goods" v-if="shopCartListN.length!==0">
                <h5>失效商品
                    <div class="wj-goods-clear">
                        <img src="/Assets/Source/img/goods-cart-clear.jpg" alt="">
                        <button @click="shopCartListClean">清空</button>
                    </div>
                </h5>

                <ul v-if="shopCartListN.length!==0" class="wj-goods-carts-list">
                    <li v-for="(item,index) in shopCartListN">
                        <!--:to="{name:'goodsdetail',params: { goodsid: item.goods_id,username:'null' }}"-->
                        <div class="wj-goods-carts-list-box">
                            <div class="wj-goods-carts-list-l">
                                <label>
                                    <input type="checkbox" name="checkbox"
                                           class="mgr mgr-success mgr-lg"
                                           @click.stop="setCheckedY(item)"
                                           :checked="item.isCheckedYes"/>
                                </label>
                                <img v-lazy="$store.state.imgs+item.pic_id+$store.state.img_suffix" class="pic"/>
                            </div>
                            <div class="wj-goods-carts-list-r">
                                <p class="til">{{ item.goods_name }}</p>
                                <p class="spec" style="display:none">220*230cm</p>
                                <div class="item">
                                    <span class="price">￥{{ (item.price).toFixed(2) }}</span>
                                    <span class="count">×{{ item.goods_count }}</span>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
            <!--=========== 失效商品区域结束 ===========-->

            <!--=========== 猜你喜欢区域开始 ===========-->
            <div class="guess-you-like">
                <h5>猜你喜欢</h5>
                <ul class="guess-you-like-list">
                    <router-link tag="li" v-if="guessYouLike.length!==0" v-for="item in guessYouLike"
                                 :to="{name:'goodsdetail',params: { goodsid: item.id,username:'null' }}"
                                 class="goods-cell">
                        <div class="pic">
                            <img v-lazy="$store.state.imgs+item.pic_id+$store.state.img_suffix"
                                 :onerror="$store.state.defaultimg" alt="">
                        </div>
                        <p class="til wj-til">{{ item.name }}</p>
                        <p class="item">
                            <span class="price">￥{{ (item.price).toFixed(2) }}</span>
                            <span style="display:none" class="count">月销1000笔</span>
                        </p>
                    </router-link>
                </ul>
            </div>
            <!--=========== 猜你喜欢区域结束 ===========-->

        </div>
        <!--中间主要内容结束-->

        <!--=========== 下单区域开始 ===========-->
        <div class="wj-place-order" v-if="isGetComputShow">
            <div class="wj-place-order-l">
                <label @click="setAllCheckedY">
                    <input type="checkbox" name="checkbox" class="mgr mgr-success mgr-lg" :checked="isAllCheckedYes"/>全选
                </label>
            </div>
            <div class="wj-place-order-r">
                <span>合计:<span>￥{{ getComputPriceT | formatPrice }}</span></span>
                <button ref="concessionBtn" @click="tomakeorder">下单</button>
            </div>
        </div>
        <!--=========== 下单区域结束 ===========-->

        <!--底部导航开始-->
        <bottom></bottom>
        <!--底部导航结束-->

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
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box
    }

    button, input {
        outline: none;
        background: #fff;
        border: none
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

    .bg {
        height: 100%;
        background: #f3f4f5
    }

    .bg, .wj-top-navbar {
        position: absolute;
        left: 0;
        top: 0;
        width: 100%
    }

    .wj-top-navbar {
        height: 1.17333333rem;
        line-height: 1.17333333rem;
        text-align: center;
        color: #2d2d34;
        font-size: .42666667rem;
        background: #fff;
        border-bottom: .01333333rem solid #f3f4f5
    }

    .wj-top-navbar h4 {
        font-weight: 400
    }

    .wj-top-navbar button {
        position: absolute;
        top: 50%;
        right: .4rem;
        transform: translateY(-50%);
        font-size: .42666667rem;
        border: none;
        /*background: #fafafa;*/
        color: #65c4aa
    }

    .wj-center-cont {
        position: absolute;
        top: 1.17333333rem;
        left: 0;
        width: 100%;
        bottom: 2.613334rem;
        overflow-x: hidden;
        overflow-y: scroll
    }

    .wj-add-bottom {
        bottom: 1.3066666666666666rem;
    }

    .wj-center-cont .wj-goods-carts {
        padding-top: .21333333rem
    }

    .wj-goods-carts-list li {
        position: relative;
        height: 2.93333333rem;
        padding: .4rem;
        border-bottom: .01333333rem solid #e9eaec;
        background: #fff;
        transition: all .2s
    }

    .wj-goods-carts-list li:last-child {
        border-bottom: none
    }

    .wj-goods-carts-list li .li-remove-scroll {
        position: relative;
        display: -ms-flexbox;
        display: flex;
        height: 100%
    }

    .wj-goods-carts-list li .li-remove-scroll .wj-arrow-remove-item {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0
    }

    .wj-goods-carts-list li.li-remove-scroll-bar[data-type="0"] {
        transform: translateZ(0)
    }

    .wj-goods-carts-list li.li-remove-scroll-bar[data-type="1"] {
        transform: translate3d(-1.70666667rem, 0, 0)
    }

    .wj-goods-carts-list li .remove-item {
        position: absolute;
        top: 0;
        right: -1.70666667rem;
        width: 1.70666667rem;
        height: 100%;
        line-height: 100%;
        background: #f03d43;
        font-size: .42666667rem;
        color: #fff
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box {
        position: relative;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row;
        height: 100%
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-l {
        position: absolute;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-l label {
        display: flex;
        align-items: center;
        margin-right: .26666667rem
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-l .pic {
        display: block;
        width: 2.10666667rem;
        height: 2.10666667rem;
        border: .01333333rem solid #f2f2f2
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r {
        position: relative;
        display: -ms-flexbox;
        display: flex;
        width: 100%;
        -ms-flex-direction: column;
        flex-direction: column;
        padding-top: .13333333rem;
        margin-left: 3.17333333rem;
        overflow: hidden
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r p {
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r p.til {
        font-size: .34666667rem;
        color: #2d2d34
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r p.spec {
        padding-top: .2rem;
        font-size: .18666667rem;
        color: #7a7b87
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item {
        position: absolute;
        bottom: .13333333rem;
        width: 100%
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .price {
        float: left;
        font-size: .32rem;
        color: #e50008
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn {
        position: absolute;
        right: 0;
        bottom: 0;
        border: .03rem solid #7a7b87;
        border-left: none;
        border-right: none;
        border-radius: .05333333rem;
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count, .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn {
        float: right;
        font-size: .32rem;
        color: #617f57
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input, .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input {
        float: left;
        color: #2d2d34;
        font-size: .32rem;
        text-align: center;
        height: .66666667rem;
        line-height: .66666667rem
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input:first-of-type, .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input:nth-of-type(3), .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input:first-of-type, .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input:nth-of-type(3) {
        width: .65333333rem
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input:first-of-type, .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input:first-of-type {
        border-left: .03rem solid #7a7b87
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input:nth-of-type(3), .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input:nth-of-type(3) {
        border-right: .03rem solid #7a7b87
    }

    .pic img {
        display: block;
        width: 100%;
        height: 100%;
    }

    .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count-btn input:nth-of-type(2), .wj-goods-carts-list li .wj-goods-carts-list-box .wj-goods-carts-list-r div.item .count input:nth-of-type(2) {
        width: .77333333rem;
        border: .03rem solid #7a7b87;
        border-top: none;
        border-bottom: none
    }

    .wj-goods-game {
        position: relative;
        height: 3.46666667rem;
        text-align: center;
        margin-top: .21333333rem;
        overflow: hidden
    }

    .wj-goods-game img {
        display: block;
        width: 100%;
        height: 100%
    }

    .wj-goods-game div {
        position: absolute;
        width: 100%;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%)
    }

    .wj-goods-game p {
        color: #fff
    }

    .wj-goods-game p.head {
        font-size: .46666667rem;
        font-weight: 700;
        padding-top: .24rem
    }

    .wj-goods-game p.til {
        font-size: .37333333rem
    }

    .wj-dead-goods {
        margin-top: .21333333rem;
        background: #fff
    }

    .wj-dead-goods h5 {
        position: relative;
        height: 1.22666667rem;
        line-height: 1.22666667rem;
        font-weight: 400;
        text-align: center;
        color: #2d2d34;
        font-size: .37333333rem;
        border-bottom: .01333333rem solid #f3f4f5
    }

    .wj-dead-goods .wj-goods-clear {
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
        right: .4rem;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row
    }

    .wj-dead-goods .wj-goods-clear img {
        display: -ms-flexbox;
        display: -webkit-box;
        display: flex;
        width: .37333333rem;
        height: .37333333rem;
        font-size: 0;
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
    }

    .wj-dead-goods .wj-goods-clear button {
        display: -ms-flexbox;
        display: flex;
        color: #e50008;
        padding-left: .5rem;
        font-size: .37333333rem
    }

    .guess-you-like {
        margin: .21333333rem 0;
        background: #fff
    }

    .guess-you-like h5 {
        height: 1.22666667rem;
        line-height: 1.22666667rem;
        text-align: center;
        font-weight: 400;
        color: #2d2d34;
        font-size: .37333333rem
    }

    .guess-you-like .guess-you-like-list {
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
        padding: 0 .4rem
    }

    .guess-you-like .guess-you-like-list li {
        display: -ms-flexbox;
        display: flex;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
        width: 49%;
        height: 100%
    }

    .guess-you-like .guess-you-like-list li:nth-of-type(2n) {
        margin-left: 1%
    }

    .guess-you-like .guess-you-like-list li:nth-of-type(odd) {
        margin-right: 1%
    }

    .guess-you-like .guess-you-like-list li .pic {
        width: 4.32rem;
        height: 4.32rem;
        overflow: hidden
    }

    .guess-you-like .guess-you-like-list li .pic img {
        display: block;
        width: 100%;
        height: 100%
    }

    .guess-you-like .guess-you-like-list li .til {
        font-size: .34666667rem;
        color: #303037;
        line-height: .53333333rem;
        word-break: break-all;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        overflow: hidden
    }

    .guess-you-like .guess-you-like-list li .wj-til {
        padding-top: .2rem;
        word-break: normal;
        display: block;
        -webkit-line-clamp: 1;
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    }

    .guess-you-like .guess-you-like-list li .item {
        display: -ms-flexbox;
        display: flex;
        width: 100%;
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-pack: justify;
        justify-content: space-between;
        padding: .21333333rem 0
    }

    .guess-you-like .guess-you-like-list li .item .price {
        font-size: .32rem;
        color: #e50008
    }

    .guess-you-like .guess-you-like-list li .item .count {
        font-size: .32rem;
        color: #7a7b87
    }

    .wj-place-order {
        position: absolute;
        bottom: 1.306667rem;
        left: 0;
        width: 100%;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: row;
        flex-direction: row;
        -ms-flex-pack: justify;
        justify-content: space-between;
        height: 1.306667rem;
        line-height: 1.306667rem;
        background: #fff;
        border-top: .01333333rem solid #e1e5e8
    }

    .wj-place-order .wj-place-order-l {
        padding-left: .4rem;
        font-size: .36rem;
        color: #303037;
        vertical-align: middle
    }

    .wj-place-order .wj-place-order-l input {
        margin-right: .13333333rem
    }

    .wj-place-order .wj-place-order-r {
        font-size: .34666667rem
    }

    .wj-place-order .wj-place-order-r span {
        color: #303037
    }

    .wj-place-order .wj-place-order-r span span {
        color: #e50008
    }

    .wj-place-order .wj-place-order-r button {
        width: 3.33333333rem;
        height: 1.30666667rem;
        font-size: .4rem;
        color: #fff;
        background: #65c4aa;
        margin-left: .4rem
    }

    .wj-goods-carts-no {
        padding-bottom: .85333333rem;
        text-align: center
    }

    .wj-goods-carts-no img {
        display: block;
        width: 2.74666667rem;
        height: 2.74666667rem;
        margin: .96rem auto .88rem
    }

    .wj-goods-carts-no p:first-of-type {
        color: #2d2d34;
        font-size: .34666667rem
    }

    .wj-goods-carts-no p:nth-of-type(2) {
        padding-top: .2rem;
        color: #7a7b87;
        font-size: .36rem
    }

    .wj-goods-carts-no button {
        margin-top: .53333333rem;
        font-size: .37333333rem;
        color: #fff;
        padding: .28rem 1.06666667rem;
        background: #65c4aa;
        border-radius: .66666667rem
    }

    .mgr {
        position: relative;
        width: 16px;
        height: 16px;
        background-clip: border-box;
        -webkit-appearance: none;
        -moz-appearance: none;
        appearance: none;
        margin: -.15px .6px 0 0;
        vertical-align: text-bottom;
        border-radius: 50%;
        background-color: #fff;
        border: 1px solid #d7d7d7
    }

    .mgr:disabled {
        opacity: .65
    }

    .mgr:before {
        content: "";
        display: block;
        height: 0;
        width: 0;
        transition: width .25s, height .25s
    }

    .mgr:checked:before {
        height: .26rem;
        width: .26rem;
        border-radius: 50%;
        margin: .1rem 0 0 .1rem
    }

    .mgr:focus {
        outline: none;
        box-shadow: inset 0 1px 1px hsla(0, 0%, 100%, .075), 0 0 2px #38a7ff
    }

    .mgr:checked {
        border: .026rem solid #e1e5e8
    }

    .mgr-success, .mgr:checked:before {
        background-color: #fff
    }

    .mgr-success, .mgr-success:checked {
        border: .026rem solid #e1e5e8
    }

    .mgr-success:checked:before {
        background-color: #65c4aa
    }

    .mgr-lg {
        width: .506rem;
        height: .506rem
    }

    .mgr-lg:checked:before {
        height: .26rem;
        width: .26rem;
        border-radius: 50%;
        margin: .1rem 0 0 .1rem
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
        margin: 0 10px
    }

    a[data-v-694cd902] {
        color: #42b983
    }
</style>

<script>
    import $ from "jquery";
    import bottom from './bottom';
    import VueLazyload from 'vue-lazyload'  //引入这个懒加载插件
    Vue.use(VueLazyload)

    // 或者添加VueLazyload 选项
    import Vue from 'vue';

    Vue.use(VueLazyload, {
        preLoad: 1.3,
        error: 'Assets/Source/img/默认图像.png',
        loading: 'Assets/Source/img/默认图像.png',
        attempt: 1,
        listenEvents: ['scroll']
    });

    let shopcart = {
        components: {
            bottom
        },
        data() {
            return {
                shopCartList: [],        // 所有购物车的商品
                shopCartListY: [],       // 没有失效的购物车的商品
                shopCartListN: [],       // 失效的购物车的商品
                guessYouLike: [],        // 猜你喜欢
                startX: 0,
                endX: 0,
                isCheckedYes: false,     // 单选
                isAllCheckedYes: false,  // 全选
                isChangeText: false,      // 编辑 => 完成
                alertFlag: false,
                alertImg: null,
                alertTil: null,
                alertImgData: ["status_loading.gif", "sign-check-icon.png", "sign-error-icon.png"],
                alertTilData: ["数据加载中 ...", "数据加载成功", "数据加载异常"],
                is_show_game: "1"
            }
        },
        created() {
            this.is_show_game = window.is_show_game;
            this.getShopCartListAPI();
            this.getGuessYouListAPI(1, 6);
        },
        filters: {
            // 过滤所有价格
            formatPrice(oldVal) {
                return (oldVal).toFixed(2);
            }
        },
        computed: {
            // 计算全选 合计 下单是否显示
            isGetComputShow() {
                return this.shopCartListY.some(item => item.isCheckedYes);
            },
            // 计算合计
            getComputPriceT() {
                let total = null;
                this.shopCartListY.forEach(item => {
                    if (item.isCheckedYes) {
                        total += item.price * item.goods_count;
                    }
                });
                return total;
            }
        },
        methods: {
            // 跳转至游戏页面
            goGameHall() {
                //window.location.href = "/Game/LeisureGameArea?token=" + encodeURIComponent(this.$store.state.shorttoken);
                window.location.href = "/#/gamehall?token=" + encodeURIComponent(this.$store.state.shorttoken);
            },
            // 去逛逛
            goStroll() {
                this.$router.push("/sortofgoods");
                /*if (this.$store.state.shorttoken.length === 0) {
                    this.$router.push("/login?redirect=" + encodeURIComponent("/#/shopcart"));
                    return;
                }
                this.$router.push("/goodssort");*/
            },
            // 失效商品清空
            shopCartListClean() {
                let arrId = [];
                let flag = this.shopCartListN.some(item => item.checked);
                if (!flag) {
                    alert("客官~ 请先选择一件商品再来清空");
                }
                this.shopCartListN = this.shopCartListN.filter((item, index) => {
                    if (item.checked) {
                        arrId.push(item.id);
                        this.removeShopCartListAPI(arrId.toString());
                        return !item.checked;
                    }
                })
            },
            // 下单
            tomakeorder() {
                let list = [], _this = this;
                this.shopCartListY.forEach((item, index) => {
                    // 购物车选中才往确认订单传值
                    if (item.isCheckedYes) {
                        list.push(item.id);
                    }
                });
                $.ajax({
                    url: '/WebApi/TradeManager/CreateOrderFromCartV3?cartlist=' + list.toString() + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                clearTimeout(timer);
                            }, 1000);
                            // alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            // console.log("下单成功");
                            //获取购物车列表
                            _this.$router.push('/makesureorder/:shopcart/' + XMLHttpRequest.responseJSON.data.toString());
                            clearTimeout(timer);
                        }, 1000);
                    }
                });
            },
            // 猜你喜欢调取的API
            getGuessYouListAPI(start, end) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/ShopManager/GoodsList?Page=' + start + '&PageSize=' + end,
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.guessYouLike = XMLHttpRequest.responseJSON.data;
                    }
                });
            },
            // 点击加减调取的API
            getChangeNumAPI(id, goodsCount) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/AddGoodsToCart?goods_id=' + id + '&goods_count=' + goodsCount + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                clearTimeout(timer);
                            }, 1000);
                            // alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            clearTimeout(timer);
                        }, 1000);
                    }
                });
            },
            // 点击加减按钮进行数字的加减
            changeNum(item, num) {
                // 加
                if (num > 0) {
                    // 显示数据加载中
                    this.alertImg = this.alertImgData[0];
                    this.alertTil = this.alertTilData[0];
                    this.alertFlag = true;

                    if (item.goods_count === 99) {
                        alert("客官~ 不能再多了");
                        return;
                    }
                    this.shopCartListY.forEach(citem => {
                        if (citem === item) {
                            citem.goods_count++;
                            this.getChangeNumAPI(citem.goods_id, 1);
                        }
                    });
                    return;
                }
                // 减
                if (num < 0) {
                    if (item.goods_count === 1) {
                        alert("客官~ 已经最少了");
                        return;
                    }
                    this.shopCartListY.forEach(citem => {
                        if (citem === item) {
                            // 显示数据加载中
                            this.alertImg = this.alertImgData[0];
                            this.alertTil = this.alertTilData[0];
                            this.alertFlag = true;

                            citem.goods_count--;
                            this.getChangeNumAPI(citem.goods_id, -1);
                        }
                    });
                }
            },
            // 点击加减中间的数字阻止默认跳转页面
            cancel() {
                return false;
            },
            // 点击顶部将完成改为编辑
            changeEditor(e) {
                let text = "编辑", target = e.target;
                if (text === target.innerHTML) {
                    target.innerHTML = "完成";
                    this.isChangeText = true;
                } else {
                    target.innerHTML = "编辑";
                    this.isChangeText = false;
                }
            },
            // 计算是否全选
            isGetComputChecked() {
                this.isAllCheckedYes = this.shopCartListY.every(item => item.isCheckedYes);
                this.shopCartListY.forEach(item => {
                    if (typeof item.isCheckedYes === "undefined") {
                        this.$set(item, "isCheckedYes", this.isAllCheckedYes);
                        item.checked = item.isCheckedYes = this.isAllCheckedYes;
                    }
                });
            },
            // 下单全选的按钮
            setAllCheckedY() {
                if (this.shopCartListY.length === 0) return;
                this.isAllCheckedYes ? this.isAllCheckedYes = false : this.isAllCheckedYes = true;
                let isChecked = this.isAllCheckedYes;
                this.shopCartListY.forEach(item => {
                    if (typeof item.isCheckedYes === "undefined") {
                        this.$set(item, "isCheckedYes", isChecked);
                        item.checked = item.isCheckedYes = isChecked;
                    }
                    item.checked = item.isCheckedYes = isChecked;
                });
                this.isGetComputChecked();
            },
            // 有商品的单选按钮
            setCheckedY(item) {
                if (typeof item.isCheckedYes === "undefined") {
                    this.$set(item, "isCheckedYes", true);
                    item.checked = item.isCheckedYes = true;
                } else {
                    item.checked = item.isCheckedYes = !item.isCheckedYes;
                }
                this.isGetComputChecked();
            },
            // 删除购物车的商品
            removeShopCartListAPI(id) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/RemoveGoodsInCart?cart_id=' + id + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                    }
                });
            },
            // 获取购物车列表
            getShopCartListAPI() {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/CartList?token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        this.shopCartList = XMLHttpRequest.responseJSON.data||[];

                        this.shopCartList.forEach(item => {
                            if (item.is_out_date === "1") {
                                this.shopCartListN.push(item);
                            } else {
                                this.shopCartListY.push(item);
                            }
                        });

                    }
                });
            },
            //跳转
            skip() {
                if (this.checkSlide()) {
                    this.restSlide();
                }
            },
            //滑动开始
            touchStart(e) {
                // 记录初始位置
                this.startX = e.touches[0].clientX;
            },
            //滑动结束
            touchEnd(e) {
                // 当前滑动的父级元素
                let parentElement = e.currentTarget.parentElement;
                // 记录结束位置
                this.endX = e.changedTouches[0].clientX;
                // 左滑
                if (parentElement.dataset.type == 0 && this.startX - this.endX > 30) {
                    this.restSlide();
                    parentElement.dataset.type = 1;
                }
                // 右滑
                if (parentElement.dataset.type == 1 && this.startX - this.endX < -30) {
                    this.restSlide();
                    parentElement.dataset.type = 0;
                }
                this.startX = 0;
                this.endX = 0;
            },
            //判断当前是否有滑块处于滑动状态
            checkSlide() {
                let listItems = document.querySelectorAll('.li-remove-scroll-bar');
                for (let i = 0; i < listItems.length; i++) {
                    if (listItems[i].dataset.type == 1) {
                        return true;
                    }
                }
                return false;
            },
            //复位滑动状态
            restSlide() {
                let listItems = document.querySelectorAll('.li-remove-scroll-bar');
                // 复位
                for (let i = 0; i < listItems.length; i++) {
                    listItems[i].dataset.type = 0;
                }
            },
            //删除
            deleteItem(item) {
                // 复位
                this.restSlide();
                // 删除
                this.shopCartListY = this.shopCartListY.filter(citem => citem !== item);
                this.removeShopCartListAPI(item.id);
                this.isGetComputChecked();
            }
        }
    }
    export {
        shopcart as
            default
    }
</script>
