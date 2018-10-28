<!--绑定新卡-->
<template>
    <div>
        <div class="wj-loadingbox" v-if="loadingFlag">
            <van-loading class="center" type="spinner" color="white"/>
            <p>数据正在加载 ......</p>
        </div>
        <div v-else class="order-creationbar">
            <!--头部导航开始-->
            <header class="wj-top-navbar">
                <h4>绑定新卡</h4>
                <div class="goback" @click="goback">
                    <img src="/Assets/Source/img/返回@3x.png" alt=""/>
                </div>
            </header>
            <!--头部导航结束-->

            <!--中间主要内容区域开始-->
            <div class="bind-new-cards-cont">
               <div class="border-t bind-new-cards">
                   <p>请输入兜兜卡密码</p>
                   <p>(凭收到的短信密码进行绑定，可到待评价订单中获取。)</p>
                   <input type="text" placeholder="卡密码不区分大小写" />
               </div>
                <div class="btn">
                    <input type="button" value="确认修改" />
                </div>
            </div>
            <!--中间主要内容区域结束-->

        </div>
    </div>
</template>

<script>
    import Vue from "vue";
    import {Loading} from "vant";

    Vue.use(Loading);

    import $ from 'jquery';

    let bindnewcards = {
        data() {
            return {
                loadingFlag: true
            }
        },
        created() {
            this.setLoadingFlag();
        },
        methods: {
            // 返回上一页
            goback() {
                this.$router.go(-1);
            },
            setLoadingFlag() {
                let timer = setInterval(() => {
                    this.loadingFlag = false;
                    clearInterval(timer);
                }, 1000);
            }
        }

    }

    export default bindnewcards
</script>