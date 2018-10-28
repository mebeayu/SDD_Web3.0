<!-->付款页<-->
<template>
    <div style="background:#f5f5f5">
        <viewtitle></viewtitle>
        <div class="totalcash">
            需支付：<span>{{this.$store.state.totalcash}}</span>
        </div>
        <div class="payway">
            <div class="payway-left">
                <img src="/Assets/Source/img/微信支付@3x.png" />
                <p>
                    <span>微信支付</span></br>
                    <span style="color:#7a7a7a;font-size:14px;">推荐安装微信5.0及以上版本的用户使用</span>
                </p>
            </div>
            <img src="/Assets/Source/img/选择@3x.png" />
        </div>
        <div class="paybtn">
            <input type="button" value="立即支付" v-on:click="pay" />
        </div>
    </div>
</template>

<script>
import viewtitle from './title';

    let pay={
        created(){
            let url=window.location.href
            url=url.split('#')
            url=url[0]
            //this.$store.dispatch('getwxshare')
            this.$store.dispatch('getwxshare',url)
        },
        components:{
            viewtitle
        },
        computed:{
            getconfig(){
                return this.$store.state.wxshare
            }
        },
        watch:{
            getconfig(){
                wx.config(this.$store.state.wxshare)
            }
        },
        methods:{
            back(){
                this.$router.push('/order/1')
            },
            pay(){
                wx.chooseWXPay(
                    this.$store.state.xxx
                    //{
                    // timestamp: 0, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                    // nonceStr: '', // 支付签名随机串，不长于 32 位
                    // package: '', // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                    // signType: '', // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                    // paySign: '', // 支付签名
                    // success: function (res) {
                    //     // 支付成功后的回调函数
                    // }
                //}
                );
            }
        }
    }

    export default pay
</script>