<!-->我的页面<-->
<template>
    <div>
        <div class="wallet-header">
            <div class="wallet-set">
                <router-link tag="img" src="/Assets/Source/img/set.png" to="/myset">
                </router-link>
            </div>
            <div class="wallet-user">
                <div>
                    <img :onerror="$store.state.defaultimg" class="wallet-user-icon" :src="'https://e-shop.rrlsz.com.cn/SWebApi/Public/picture/' + this.$store.state.my.userinfo.head_pic+'.jpg'" />
                    <span>
                        {{this.$store.state.my.userinfo.nick_name || this.$store.state.my.userinfo.username}}
                    </span>
                    <span v-on:click='login' v-if="this.$store.state.shorttoken.length === 0">登录/注册</span>
                </div>
            </div>
        </div>
        <div class="wallet-content">
            <router-link :to="{name:'order',params: { status: 0 }}" tag="div" class="all-order">
                <span>全部订单</span>
                <span>
                    查看更多订单>
                </span>
            </router-link>
            <ul class="wallet-order-cate">
                <router-link :to="{name:'order',params: { status: 1}}" tag="li">
                    <img src="/Assets/Source/img/待付款@3x.png" alt="" />
                    <p>待付款</p>
                </router-link>
                <router-link :to="{name:'order',params: { status: 2 }}" tag="li">
                    <img src="/Assets/Source/img/待消费收货@3x.png" alt="" />
                    <p>待消费/收货</p>
                </router-link>
                <router-link :to="{name:'order',params: { status:3}}" tag="li">
                    <img src="/Assets/Source/img/待评价@3x.png" alt="" />
                    <p>待评价</p>
                </router-link>
                <router-link :to="{name:'order',params: { status:4}}" tag="li">
                    <img src="/Assets/Source/img/退换售后@3x.png" alt="" />
                    <p>退换/售后</p>
                </router-link>
            </ul>
            <div class="all-order">
                <span>我的推荐</span>
                <span></span>
            </div>
            <ul class="my-my_rec" v-if='this.$store.state.my.usercount.is_shop_keeper'>
                <router-link to="/myrecreward" tag="li">
                    <p>￥{{this.$store.state.my.usercount.month_recommand_award || 0}}</p>
                    <p>本月推荐奖励</p>
                    <p>总：{{this.$store.state.my.usercount.total_recommand_award || 0}}</p>
                </router-link>
                <router-link to="/myrecorder" tag="li">
                    <p>{{this.$store.state.my.usercount.month_recommand_order_count || 0}}</p>
                    <p>本月订单</p>
                    <p>总：{{this.$store.state.my.usercount.total_recommand_order_count || 0}}</p>
                </router-link>
            </ul>
            <ul class="my-my_rec" v-else>
                <router-link to="/myrecpeople" tag="li">
                    <p style='color:#9e925f'>{{this.$store.state.my.usercount.total_recommand_user_count || 0 }}</p>
                    <p>我推荐的用户</p>
                </router-link>
                <router-link to="/myrecreward" tag="li">
                    <p>￥{{this.$store.state.my.usercount.month_recommand_award || 0}}</p>
                    <p>本月推荐奖励</p>
                </router-link>
                <router-link to="/myrecorder" tag="li">
                    <p>{{this.$store.state.my.usercount.month_recommand_order_count || 0 }}</p>
                    <p>本月订单</p>
                </router-link>
            </ul>
            <div class="all-order" v-if='this.$store.state.my.usercount.is_shop_keeper'>
                <span>我的会员</span>
                <span></span>
            </div>
            <ul class='my-my_rec_num' v-if='this.$store.state.my.usercount.is_shop_keeper'>
                <router-link to="/myrecpeople" tag="li">
                    <p>已购会员：<span>{{this.$store.state.my.usercount.recommand_user_consumed || 0}}</span></p>
                </router-link>
                <router-link to="/myrecpeople" tag="li">
                    <p>未购会员：<span>{{this.$store.state.my.usercount.recommand_user_not_consumed || 0}}</span></p>
                </router-link>
            </ul>
            <ul class="wallet-support" id='last-item'>
                <router-link tag='li' to="/goodscollection">
                    <img src="/Assets/Source/img/收藏商品@3x.png" alt="" />
                    <p>我的收藏</p>
                </router-link>
                <li v-on:click="showcode(true)">
                    <img src="/Assets/Source/img/my_qrcod.png" alt="" />
                    <p>我的推荐码</p>
                </li>
                <li v-on:click="ooofuck">
                    <img src="/Assets/Source/img/客户服务@3x.png" alt="" />
                    <p>客服服务</p>
                </li>
                <li v-on:click="">
                    <img src="/Assets/Source/img/客户服务@3x.png" alt="" />
                    <p>我的卡券</p>
                </li>
            </ul>
            <div class='shadow' v-show='showqrcode == true' v-on:click="showcode(false)">
                <div id='aoa' style='margin-bottom:15px;position:absolute;top:50%;
                                    left:50%;margin-left:-100px;
                                    margin-top:-100px;width:200px;height:200px;'>
                    <qrcode v-bind:value='add' id='xoxo' style='display:none'>
                    </qrcode>
                    <img id='IMG' />
                    <div class='sharetext'>
                        长按图片进行分享
                    </div>
                </div>
            </div>
            <bottom></bottom>
        </div>
    </div>
</template>

<style>
    .sharetext {
        font-size: 20px;
        text-align: center;
        color: #fff;
        margin-bottom: 10px;
    }
    #aoa img {
        width: 100%;
        height: 200px;
    }
</style>

<script>
    import bottom from './bottom';
    import $ from 'jquery';
    let my = {
        data() {
            return {
                showqrcode: false,
                add: '',
                id: ''
            }
        },
        components: {
            bottom,
        },
        created() {
            //获取用户信息
            this.$store.dispatch('get_userinfo').then(() => {
                this.id = sessionStorage.getItem('myid');
                this.add = 'https://e-shop.rrlsz.com.cn/#/regist/' + this.id
                this.$store.dispatch('get_usercount')
                // this.add = 'https://e-shop.rrlsz.com.cn/#/regist/99999999999'
            })
        },
        computed:{
            token() {
                return this.$store.state.shorttoken;
            }
        }, 
        watch:{
            token() {
                this.$store.dispatch('get_usercount')
            }
        },
        methods: {
            login() {
                this.$router.push('/login')
            },
            ooofuck() {
                alert('敬请期待')
            },
            showcode(sta) {
                if (this.id != null) {
                    this.showqrcode = sta;
                    const myCanvas = document.getElementsByTagName("canvas")[0];
                    const img = this.convertCanvasToImage(myCanvas);
                    const box = $('#IMG')
                    IMG.src = img.src
                } else {
                    this.$router.push('/login');
                }
            },
            convertCanvasToImage(canvas) {
                //新Image对象,可以理解为DOM;
                var image = new Image();
                //canvas.toDataURL返回的是一串Base64编码的URL,当然,浏览器自己肯定支持
                //指定格式PNG
                try {
                    image.src = canvas.toDataURL("image/png");
                } catch (e) {}
                return image;
            },
        }
    }
    export default my;
</script>