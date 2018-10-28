<template>
    <div>
        <!--头部区域开始-->
        <div @click="back" class="back-page-btn">
            <img src="/Assets/Source/img/返回@3x.png" alt="">
            <p>{{ isMonthDate | formatThisMonth }}推荐奖励排名（显示前50名）</p>
        </div>
        <!--头部区域结束-->

        <!--中间主体区域开始-->
        <div class="wj-recommended-ranking-cont">
            <div v-if="monthRecommandRewardsList.length!==0">
                <div class="wj-recommended-ranking-top">
                    <ul>
                        <li>月份：{{ monthRecommandRewardsList.month }}</li>
                        <li>我的排名：{{ monthRecommandRewardsList.current_user_ranking }}|￥{{
                            monthRecommandRewardsList.current_user_rewards }}
                        </li>
                    </ul>
                </div>
                <div class="wj-recommended-ranking-list">
                    <ul>
                        <li>
                            <span>序号</span>
                            <span>用户帐号</span>
                            <span>金额</span>
                        </li>
                        <li v-for="(item,index) in ranking_list" :key="index">
                            <span>{{ item.rows }}</span>
                            <span>{{ item.username }}</span>
                            <span>￥{{ item.money }}</span>
                        </li>
                    </ul>
                </div>
            </div>

            <!--暂无数据-->
            <p v-else class="wj-recommended-no-data">
                暂无数据
            </p>
        </div>
        <!--中间主体区域结束-->

    </div>
</template>

<script>
    import $ from "jquery";

    let recommendrewardranking = {
        name: "recommendrewardranking",
        data() {
            return {
                yearDate: "",
                monthDate: "",
                isMonthDate: "",
                monthRecommandRewardsList: [], // 本月推荐奖励排名（显示前50名）
                ranking_list: [] // 序号 ==== 用户账号 ==== 金额
            }
        },
        created() {
            this.getUrlParamsList();
            this.getMonthRecommandRewardsRankingAPI();
        },
        filters: {
            // 格式化标题的文字
            formatThisMonth(val) {
                if (!val) return;
                val = parseInt(val);
                let arrThisMonth = ["总", "本月", "上月"];
                return arrThisMonth[val];
            }
        },
        methods: {
            // 获取url地址栏参数
            getUrlParamsList() {
                let params = window.UI.Page.getAllparam();
                this.yearDate = params.year;
                this.monthDate = params.month;
                this.isMonthDate = params.isMonth;
            },
            // 获取本月推荐奖励排名（显示前50名）的数据
            getMonthRecommandRewardsRankingAPI() {
                let _this = this,
                    month = "";
                if (parseInt(_this.isMonthDate) !== 0) {
                    month = _this.yearDate + "-" + _this.monthDate;
                }
                $.ajax({
                    url: "/WebApi/UserManager/MonthRecommandRewardsRanking",
                    data: {
                        "token": encodeURIComponent(_this.$store.state.shorttoken),
                        "month": month
                    },
                    type: "post",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 异常
                        if (res.status === 98) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            _this.monthRecommandRewardsList = res.data;
                            _this.ranking_list = _this.monthRecommandRewardsList.ranking_list;
                        }
                    }
                });
            },
            // 返回上一页
            back() {
                this.$router.go(-1);
            }
        }
    };
    export default recommendrewardranking;
</script>

<style scoped>

</style>