<!--兜兜卡-->
<template>
    <div class="order-creationbar">
        <!--头部导航开始-->
        <header class="wj-top-navbar">
            <h4>兜兜卡</h4>
            <div class="goback" @click="goback">
                <img src="/Assets/Source/img/返回@3x.png" alt=""/>
            </div>
        </header>
        <!--头部导航结束-->

        <!--中间主要内容区域开始-->
        <div class="wj-pocket-card-cont">
           <!--暂无可用兜兜卡区域开始-->
            <div class="no-pocket-card">
                <img src="/Assets/Source/img/wj-pocket-card-no-@3x.png" alt="" />
                <p>您暂无可用兜兜卡</p>
            </div>
           <!--暂无可用兜兜卡区域结束-->

            <!--有可用兜兜卡区域开始-->
            <!--<div class="yes-pocket-card">
                <ul>
                    <li>
                        <div>
                            <img class="cards" src="/Assets/Source/img/pocket-cards-100@2x.png" alt="" />
                            <div class="cards-num">
                                <span>100</span>
                                <img src="/Assets/Source/img/wj-pocket-card-rmb-@3x.png" alt="" />
                            </div>
                        </div>
                    </li>

                    <li>
                        <div>
                            <img class="cards" src="/Assets/Source/img/pocket-cards-100@2x.png" alt="" />
                            <div class="cards-num">
                                <span>100</span>
                                <img src="/Assets/Source/img/wj-pocket-card-rmb-@3x.png" alt="" />
                            </div>
                        </div>
                    </li>
                </ul>
            </div>-->
            <!--有可用兜兜卡区域结束-->
        </div>
        <!--中间主要内容区域结束-->

        <!--底部主要按钮区域开始-->
        <div class="wj-pocket-card-bot">
           <ul class="pocket-card-btn-list">
               <li>购买新卡</li>
               <router-link
                       tag="li"
                       to="/bindnewcards"
               >绑定新卡</router-link>
           </ul>
        </div>
        <!--底部主要按钮区域结束-->

    </div>
</template>

<script>
    import $ from 'jquery';

    let pocketcard = {
        data() {
            return {}
        },
        methods: {
            // 返回上一页
            goback() {
                this.$router.go(-1);
            }
        }

    }

    export default pocketcard
</script>