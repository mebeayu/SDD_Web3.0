<template>
    <div>
        <viewtitle></viewtitle>
        <div class="totalcash">
            需支付：<span id="gggcash">{{totalcash}}</span>元
        </div>
        <ul class='chosepayway'>
            <li>
                <img src="/Assets/Source/img/wxpay.png" />
                <div>
                    <p>微信支付</p>
                    推荐安装微信5.0以上版本的用户使用
                </div>
                <input type='radio' v-model="picked" value="wx" />
            </li>
            <!--li>
                    <img src="/Assets/Source/img/wxpay.png"/>
                    <div>
                        <p>微信二维码支付</p>
                        扫描二维码支付
                    </div>
                    <input type='radio' v-model="picked"  value="code"/>
                </li-->
            <li>
                <img src="/Assets/Source/img/jinbipay.png" />
                <div>
                    <p>钱包支付</p>
                    你的钱包余额为：{{this.$store.state.userdiscount}}
                </div>
                <input type='radio' v-model="picked" value="wallet" v-bind:disabled='this.$store.state.userdiscount < (parseFloat(totalcash) + 12)' />
            </li>
            <li>
                <img src="/Assets/Source/img/glodbean.png" />
                <div>
                    <p>金豆支付(100金豆 = 1元)</p>
                    你的金豆余额为：{{this.$store.state.goldbean}}
                </div>
                <input type='radio' v-model="picked" value="goldbean" v-bind:disabled='!canusejindou' />
            </li>
        </ul>
        <div class="paybtnaa">
            <input type="button" value="立即支付" v-on:click="pay()" />
        </div>
    </div>
</template>


<script>
    import $ from 'jquery';
    import viewtitle from './title';

    let payway = {
        data() {
            return {
                picked: 'wx',
                totalcash: 0,
                canusetuikuan:false,
                canusejindou:false,
                puremoney:0
            }
        },
        components:{
            viewtitle
        },
        created() {
            this.totalcash = sessionStorage.getItem('totalcash');
            this.totalcash = parseFloat(this.totalcash)
            if( this.totalcash >= 88) {
                this.$store.state.goldbean/100 < this.totalcash? this.canusejindou = false: this.canusejindou =true;
                this.$store.state.userdiscount < this.totalcash? this.canusetuikuan =false: this.canusetuikuan = true;
            } else {
                this.$store.state.goldbean/100 < this.totalcash + 12 ? this.canusejindou = false: this.canusejindou =true;
                this.$store.state.userdiscount < this.totalcash + 12 ? this.canusetuikuan =false: this.canusetuikuan = true;
            }
        },
        methods: {
            back() {
                this.$router.go(-1)
            },
            pay() {
                const that = this
                switch (this.picked) {
                    case 'wx':
                        const rec = sessionStorage.getItem('recommand')
                        window.location.href = 'https://e-shop.rrlsz.com.cn/WxJsPay/?orderlist=' + this.$store.state.orderid.toString() + '&discount=' + this.$store.state.jinbidiscount + '&token=' + encodeURIComponent(this.$store.state.shorttoken) + '&sperador=' + rec;
                        break;
                    case 'code':
                        $.ajax({
                            url: 'WebApi/TradeManager/ApplyPayVer2?orderlist=' + this.$store.state.orderid.toString() + '&discount=' + this.$store.state.jinbidiscount + '&trans_type=4&token=' + encodeURIComponent(this.$store.state.shorttoken) + '&IP=',
                            type: 'get',
                            success: function(data) {
                                if (data.status == 0) {
                                    that.$store.commit('SET_PAYQRCODE', data.data);
                                    that.$router.push('/payqrcode');
                                } else {
                                    alert('二维码生成失败，请用其他方式支付')
                                }
                            }
                        })
                        break;
                    case 'wallet':
                        $.ajax({
                            url: 'WebApi/TradeManager/ApplyPayVer2?orderlist=' + this.$store.state.orderid.toString() + '&discount=' + this.$store.state.jinbidiscount + '&trans_type=3&token=' + encodeURIComponent(this.$store.state.shorttoken),
                            type: 'get',
                            success: function(data) {
                                if (data.status == 0) {
                                    alert('支付成功')
                                    that.$router.push('/order/0');
                                } else {
                                    alert(data.message)
                                }
                            }
                        })
                        break;
                    case 'goldbean':
                        $.ajax({
                            url: 'WebApi/TradeManager/ApplyPayVer2?orderlist=' + this.$store.state.orderid.toString() + '&discount=' + this.$store.state.jinbidiscount + '&trans_type=5&token=' + encodeURIComponent(this.$store.state.shorttoken),
                            type: 'get',
                            success: function(data) {
                                if (data.status == 0) {
                                    alert('支付成功')
                                    that.$router.push('/order/0');
                                } else {
                                    alert(data.message)
                                }
                            }
                        })
                        break;
                }
            }
        }
    }
    export default payway
</script>