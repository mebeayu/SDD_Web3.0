<template>
    <div style='background:#f5f5f5'>
        <viewtitle></viewtitle>
        <!--        <ul class='rec-reward-title'>
                    <li>
                        {{montharr[0]}}月：<span>￥{{this.$store.state.rec.thisMonthReward.month_total_money || 0}}</span>
                        <p>待确认：￥{{this.$store.state.rec.thisMonthReward.month_frz_money || 0}}</p>
                    </li>
                    <li>
                        全部：<span>￥{{this.$store.state.rec.thisMonthReward.year_total_money || 0}}</span>
                        <p>待确认：￥{{this.$store.state.rec.thisMonthReward.year_frz_money || 0}}</p>
                    </li>
                </ul>-->

            <div class="wj-rec-reward-top-list">
                <ul>
                    <li>
                        <div class="wj-left wj-width-r">
                            <p>本月奖励</p>
                            <p class="wj-red">￥{{ myRecUserList.current_month_recommand_rewards_money || 0 }}</p>
                        </div>
                        <div @click="thisMonthRrankingList(1)" class="wj-width-r">
                            <p>奖励排名：<span class="wj-red">{{ myRecUserList.current_month_recommand_rewards_ranking | formatRankingNum }}</span></p>
                            <p class="wj-check-detail">查看详情</p>
                        </div>
                        <div class="wj-width-r">
                            <p>待确认奖励</p>
                            <p class="wj-red">{{ myRecUserList.current_month_unconfirm_order_sum_reward || 0 }}</p>
                        </div>
                    </li>

                    <li>
                        <div class="wj-left wj-width-r">
                            <p>上月奖励</p>
                            <p class="wj-red">￥{{ myRecUserList.last_month_recommand_rewards_money || 0 }}</p>
                        </div>
                        <div class="wj-width-r">
                            <p>奖励排名：<span class="wj-red">{{ myRecUserList.last_month_recommand_rewards_ranking | formatRankingNum }}</span></p>
                        </div>
                        <div class="wj-width-r">
                            <!--<p class="wj-check-detail">查看详情</p>-->
                            <p class="wj-red">{{ myRecUserList.last_month_unconfirm_order_sum_reward || 0 }}</p>
                        </div>
                    </li>

                    <li>
                        <div class="wj-left wj-width-r">
                            <p>总奖励</p>
                            <p class="wj-red">￥{{ myRecUserList.all_recommand_rewards_money }}</p>
                        </div>
                        <div class="wj-width-r">
                            <p>总奖励排名：<span class="wj-red">{{ myRecUserList.all_recommand_rewards_ranking | formatRankingNum }}</span></p>
                        </div>
                        <div class="wj-width-r">
                            <p>-</p>
                        </div>
                        <!--<div @click="thisMonthRrankingList(0)" class="wj-width-r">
                            <p class="wj-check-detail">查看详情</p>
                        </div>-->
                    </li>
                </ul>
                <p class="wj-tips">温馨提示：合伙人可查看所有级别推荐人的奖励及账单信息</p>
            </div>

            <ul class='rec-reward-month wj-rec-reward-month'>
                <li v-on:click='chosemonth(0)' v-bind:class="monthstatus == 0?'on':'' ">
                    全部
                </li>
                <li v-for='num in montharr' v-on:click='chosemonth(num)' v-bind:class="monthstatus == num?'on':'' ">
                    {{num}}月
                </li>
            </ul>
            <table class='rec-reward-nav' cellpadding='15'
                   v-if='this.$store.state.rec.MonthRewardRecData.is_shop_keeper == true'>
                <tr class='nav'>
                    <th>来源|下单用户</th>
                    <th>金额|奖励金</th>
                    <th>状态</th>
                </tr>
                <tr v-for='item in this.$store.state.rec.MonthRewardList'>
                    <td class='left'>
                        <img src='/Assets/Source/img/ti.png' v-if='item.recommand_class == 1'/>
                        <img src='/Assets/Source/img/jin.png' v-else/>
                        <div>
                            {{item.buyer_name }}
                            <p>
                                {{item.addtime | timeformate}}
                            </p>
                        </div>
                    </td>
                    <td>
                        ￥{{(item.order_money).toFixed(2)}}
                        <p>奖励￥{{item.recommand_money}}</p>
                    </td>
                    <td v-if='item.is_frz == true'>待确认</td>
                    <td v-else>已入账</td>
                </tr>
            </table>
            <table class='rec-reward-nav' cellpadding='15' v-else>
                <tr class='nav'>
                    <th>下单用户</th>
                    <th>金额|奖励金</th>
                    <th>状态</th>
                </tr>
                <tr v-for='item in this.$store.state.rec.MonthRewardList'>
                    <td class='left'>
                        <div>
                            {{item.buyer_name | phoneformate}}
                            <p>
                                {{item.addtime | timeformate}}
                            </p>
                        </div>
                    </td>
                    <td>
                        ￥{{item.order_money.toFixed(2)}}
                        <p>奖励￥{{item.recommand_money}}</p>
                    </td>
                    <td v-if='item.is_frz == true'>待确认</td>
                    <td v-else>已入账</td>
                </tr>
            </table>
    </div>

</template>

<style scoped>
    .wj-rec-reward-top-list ul {
        padding: .4rem 0;
    }

    .wj-width-r {
        width: 33.3333333333%;
    }

    .no-data{
        padding-top: 1.54rem;
    }
    .no-data p{
        padding: .4rem;
        text-align: center;
        color: #b2b2b2;
        font-size: .34666667rem;
    }
</style>

<script>
    import viewtitle from './title';

    let myrecreward = {
        data() {
            return {
                montharr: [],
                monthstatus: 0,
                myRecUserList: [], // 获取我的推荐奖励的数据
                isDataFlag: false
            }
        },
        components: {
            viewtitle
        },
        created() {
            const date = new Date()
            const month = date.getMonth() + 1;
            for (let i = 0; i <= month - 1; i++) {
                this.montharr.unshift(i + 1);
            }
            //获取本月的推荐信息
            this.$store.dispatch('getThisMonthReward', month);
            //动态获取每月推荐信息
            this.$store.dispatch('getMonthRewardData', this.monthstatus);
            // 获取我的推荐奖励的数据
            this.getMyRecUserListAPI();
        },
        methods: {
            // 点击查看详情跳转
            thisMonthRrankingList(monthNum) {
                let date = new Date(),
                    year = date.getFullYear(),
                    month = date.getMonth() + 1,
                    isMonth = monthNum,
                    urlRouter = "/recommendrewardranking";
                // 总的
                if (isMonth === 0) {
                    month = 0;
                }
                // 上月
                if (isMonth === 2) {
                    month = month - 1;
                }
                this.$router.push(urlRouter + "?year=" + year + "&month=" + month + "&isMonth=" + isMonth);
            },
            // 获取我的推荐奖励的数据
            getMyRecUserListAPI() {
                let _that = this;
                $.ajax({
                    url: '/WebApi/UserManager/MyRecommandRewardsStat',
                    type: 'post',
                    data: {
                        "token": encodeURIComponent(_that.$store.state.shorttoken)
                    },
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            _that.myRecUserList = res.data;
                            _that.isDataFlag = true;
                        }
                    }
                })
            },
            chosemonth(sta) {
                this.monthstatus = sta;
                this.$store.dispatch('getMonthRewardData', this.monthstatus);
            },
            back() {
                this.$router.back();
            }
        },
        filters: {
            timeformate(time) {
                let fortime = time.replace('T', ` `)
                let arr = time.split('T')
                let montharr = arr[0].split('-')
                let timestamparr = arr[1].split(':')
                let timestr = montharr[1] + '-' + montharr[2] + ` ` + timestamparr[0] + ':' + timestamparr[1]
                return timestr
            },
            phoneformate(num) {
                return num.substr(0, 3) + "****" + num.substr(7)
            },
            // 格式化本月/上月/总的排名   例如：0： 1000+  否则返回原来的
            formatRankingNum(val) {
                if (val === 0) {
                    return "1000+";
                }
                return val;
            }
        },
    }


    export default myrecreward
</script>