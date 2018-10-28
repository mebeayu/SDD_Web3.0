<template>
    <div v-finger:swipe="swipe">
        <div class="top"><span class="icon-left" v-on:click='back'></span>我的钱包</div>
        <div class="content pb20 pt20">
            <ul class="explain">
                <li>
                    <div class="box">
                        <div class="left"><span class="dot"></span></div>
                        <div class="right">
                            <p style='word-break:break-all;white-space:normal' v-if='status == 0'><span
                                    class="color-333">现金账户:</span>在省兜兜购物后由于订单取消、退货、退款产生的无法原路退回付款用户账号的金额将会退至用户的现金账户，
                                现金账户的金额可通过绑定银行卡进行提现，提现不收取任何手续费，提现申请成功后将在1-7个工作日内打款至提现申请时填写的银行账户。</p>
                            <p style='word-break:break-all;white-space:normal;display:none;' v-if='status == 1'><span
                                    class="color-333">消费奖励账户:</span>是用户在省兜兜购物消费成功后平台发放的奖励金的账户。账户内金额可进行提现或购物时进行金额抵扣。提现需收取10%的手续费，提现申请成功后将在1-7个工作日内打款至提现申请时填写的银行账户。
                            </p>
                            <p style='word-break:break-all;white-space:normal' v-if='status == 2'><span
                                    class="color-333">推荐奖励账户:</span>
                              是推荐他人在省兜兜购物消费成功后，平台将按照订单金额的一定百分比发放奖励（一级推荐 2%，二级推荐 1%）。账户内金额可提现或购物时的金额抵扣。提现需收取1%的手续费，提现申请成功后将在1-7个工作日内打款至提现申请时填写的银行账户(部分活动商品无奖励，如秒杀商品、购物卡、特惠商品)。</p>                        </div>
                    </div>
                </li>
                <li class="c">
                    <div class="box">
                        <div class="left"><span class="dot"></span></div>
                        <div class="right">
                            <p>如有疑问可联系:<span class="color-red pl10">400-990-7881</span></p>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div class="content">
            <ul class="c classification mt20">
                <li v-bind:class="{'active':status==0}" v-on:click='chang(0)'>
                    <p>现金账户</p>
                    <p><span class="color-red">￥{{this.$store.state.wallet.UserAccountInfo.x_money }}</span></p>
                </li>
                <!--<li v-bind:class="{'active':status==1}"  v-on:click='chang(1)'>
                    <p>消费奖励</p>
                    <p><span class="color-red">￥{{this.$store.state.wallet.UserAccountInfo.r_money  }}</span></p>
                </li>-->
                <li v-bind:class="{'active':status==2}" v-on:click='chang(2)'>
                    <p>推荐奖励</p>
                    <p><span class="color-red">￥{{this.$store.state.wallet.UserAccountInfo.y_money   }}</span></p>
                </li>
            </ul>
        </div>
        <div class="content" id='xiaofeijiangli' v-if='status == 1'>
            <h3 class="pl10 pt10 pb10">消费明细</h3>
            <ul class="detailed" v-if='$store.state.wallet.xiaofeilist.length != 0'>
                <li v-for='item in $store.state.wallet.xiaofeilist'>
                    <div class="box">
                        <div class="left">
                            <p class="color-333">{{item.addtime | timeformat}}</p>
                        </div>
                        <div class="right">
                            <p class="b">￥ {{item.money | moneyformat}}<span class="color-9e925f pl10"></span></p>
                            <p class="s">{{item.remark}}</p>
                        </div>
                    </div>
                </li>
            </ul>
            <ul class="detailed" v-else>
                <li style='color:#999;text-align:center;'>
                    未查询到相关数据
                </li>
            </ul>
        </div>
        <div class="content" id='xianjin' v-if='status == 0'>
            <h3 class="pl10 pt10 pb10">消费明细</h3>
            <ul v-if='$store.state.wallet.xianjinlist.length != 0' class="detailed">
                <li v-for='item in $store.state.wallet.xianjinlist'>
                    <div class="box">
                        <div class="left">
                            <p class="color-333">{{item.addtime | timeformat}}</p>
                        </div>
                        <div class="center">
                            <img src="/Assets/Source/img/get.png">
                        </div>
                        <div class="right">
                            <p class="b">￥ {{item.money | moneyformat}}</p>
                            <p class="s">{{item.remark}}</p>
                        </div>
                    </div>
                </li>
            </ul>
            <ul v-else>
                <li style='color:#999;text-align:center;'>
                    未查询到相关数据
                </li>
            </ul>
        </div>
        <div class="content" id='tuijian' v-if='status == 2'>
            <ul class="c user">
                <li class="pb10"
                    v-bind:class="{'active' : showundeline == false}"
                    v-on:click='showundeline = false'>
                    <div class="img pt10 pb10 my-bill"></div>
                    <p class="color-333">我的账单明细</p>
                </li>
                <li class="pb10"
                    v-bind:class="{'active' : showundeline == true}"
                    v-on:click='showundeline = true'>
                    <div class="img pt10 pb10 my-user"><!--span class="icon">99+</span--></div>
                    <p class="color-333">我推荐的用户</p>
                </li>
                <li></li>
            </ul>
        </div>
        <div class="content" v-if='status == 2'>
            <h3 class="pl10 pt10 pb10" v-if='showundeline == false'>消费明细</h3>
            <h3 class="pl10 pt10 pb10" v-else>推荐明细</h3>
            <div>
                <ul v-if='showundeline == false && $store.state.wallet.tuijianlist.length !== 0' class="detailed">
                    <li v-for='item in this.$store.state.wallet.tuijianlist'>
                        <div class="box">
                            <div class="left">
                                <p class="color-333">{{item.addtime | timeformat}}</p>
                            </div>
                            <div class="right">
                                <p class="b">￥ {{item.money | moneyformat}}<span class="color-9e925f pl10"></span></p>
                                <p class="s">{{item.remark}}</p>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul v-if='showundeline == true && $store.state.wallet.tuijianlist.length === 0' class="detailed">
                    <li style='color:#999;text-align:center;'>
                        未查询到相关数据
                    </li>
                </ul>
            </div>

            <div>
                <ul v-if='showundeline == true && $store.state.walletlist[3].length !== 0' class="detailed">
                    <li style='display:flex'>
                        <div style='margin-right:80px;'>
                            用户账号
                        </div>
                        <div>
                            推荐成功时间
                        </div>
                    </li>
                    <li v-for='item in this.$store.state.walletlist[3]'>
                        <span style='margin-right:45px;'>{{item.username }}</span>
                        <span>{{item.addtime | timeformat}}</span>
                    </li>
                </ul>
                <ul v-if='showundeline == false && $store.state.walletlist[3].length === 0' class="detailed">
                    <li style='color:#999;text-align:center;'>
                        未查询到相关数据
                    </li>
                </ul>
            </div>
            <!--<ul class="detailed">
                <li v-for='item in this.$store.state.wallet.tuijianlist'
                    v-if='showundeline == false &&  $store.state.wallet.tuijianlist.length != 0'>
                    <div class="box">
                        <div class="left">
                            <p class="color-333">{{item.addtime | timeformat}}</p>
                        </div>
                        <div class="right">
                            <p class="b">￥ {{item.money | moneyformat}}<span class="color-9e925f pl10"></span></p>
                            <p class="s">{{item.remark}}</p>
                        </div>
                    </div>
                </li>
                <li v-else style='color:#999;text-align:center;'>
                    未查询到相关数据
                </li>
                <li v-if='showundeline == true' style='display:flex'>
                    <div style='margin-right:80px;'>
                        用户账号
                    </div>
                    <div>
                        推荐成功时间
                    </div>
                </li>
                <li v-if='showundeline == true  && $store.state.walletlist[3].length != 0'
                    v-for='item in this.$store.state.walletlist[3]'>
                    <span style='margin-right:45px;'>{{item.username }}</span>
                    <span>{{item.addtime | timeformat}}</span>
                </li>
                <li v-else style='color:#999;text-align:center;'>
                    未查询到相关数据
                </li>
            </ul>-->
        </div>
    </div>

</template>

<script>
    import $ from 'jquery';

    const walletdetail = {

        data() {
            return {
                status: 0,
                page: [1, 1, 1],
                list: [[], [], []],
                underline: [],
                underlinepage: 1,
                showundeline: false,
            }
        },
        methods: {
            back() {
                this.$router.go(-1)
            },
            chang(sta) {
                this.status = sta;
            },


            getlinelist(page) {
                const that = this
                $.ajax({
                    url: 'WebApi/UserManager/GetUserReferralsList',
                    type: 'post',
                    data: {
                        "token": this.$store.state.shorttoken,
                        "Page": page,
                        "PageSize": 50
                    },
                    success: function (data) {
                        let arr = that.underline.concat(data.data)
                        let hash = {};
                        arr = arr.reduce(function (item, next) {
                            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                            return item
                        }, [])
                        that.underline = arr
                        that.$store.commit('setwalletlist3', that.underline)
                    }
                })
            },
            swipe(e) {
                if (e.direction == 'Up') {
                    this.page[this.status]++;
                    switch (this.status) {
                        case 0:
                            this.$store.dispatch('getxianjin', this.page[0]);
                            break;

                        case 1:
                            this.$store.dispatch('getxiaofei', this.page[1]);
                            break;

                        case 2:
                            this.$store.dispatch('getlinelist', this.page[2]);
                            this.underlinepage++;
                            this.getlinelist(this.underlinepage);
                    }
                }
            }
        },
        created() {
            // this.getxianjin(this.page[0]);
            // this.getxiaofei(this.page[1]);
            // this.gettuijian(this.page[2]);
            this.$store.dispatch('getxianjin')
            this.$store.dispatch('getxiaofei')
            this.$store.dispatch('gettuijian')

            this.getlinelist(this.underlinepage);
        },
        filters: {
            //设置详情列表滤镜
            timeformat(time) {
                walletdetail.nowtime = new Date()
                let newtime
                newtime = time.split('T')
                time = newtime[0].split('-')
                let year = time[0]
                let mouth = time[1]
                let day = time[2]
                if (year == walletdetail.nowtime.getFullYear() && mouth == walletdetail.nowtime.getMonth() + 1 && day == walletdetail.nowtime.getDate()) {
                    time = '今日'
                    return time
                } else if (year == walletdetail.nowtime.getFullYear() && mouth == walletdetail.nowtime.getMonth() + 1 && day == walletdetail.nowtime.getDate() - 1) {
                    time = '昨日'
                    return time
                } else {
                    time = newtime[0]
                    return time
                }
            },
            phoneformat(tel) {
                let newtel;
                tel = tel.toString();
                newtel = tel.substr(0, 3) + '****' + tel.substr(7)
                return newtel;
            },
            moneyformat(num) {
                num > 0 ? num = '+' + num : num;
                return num;
            }
        },
    }


    export default walletdetail
</script>