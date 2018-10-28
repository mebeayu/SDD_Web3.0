<template>
    <div style='background:#f5f5f5'>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" @click="back"/>
            <p>我的推荐用户</p>
        </div>
        <!--        <ul class='rec-reward-title wj-rec-reward-title'>
                    <li>
                        <div class="wj-rec-people-l">
                            <span>{{ montharr[0] }}月：</span>
                            <span>{{ thisMonthRec.month_total_recommand_user || 0}}</span>
                        </div>

                        <p class="wj-rec-people-r" v-if="thisMonthRec.length!==0">
                            <span class="wj-level-span">一级：<span>{{thisMonthRec.month_first_class_recommand_user || 0}}</span></span>
                            <span class="wj-level-span">二级：<span>{{thisMonthRec.month_second_class_recommand_user || 0}}</span></span>
                            <span class="wj-level-span">三级：<span>{{thisMonthRec.month_third_class_recommand_user || 0}}</span></span>
                        </p>
                    </li>
                    <li>
                        <div class="wj-rec-people-l">
                            <span>全部：</span>
                            <span>{{ totalMonthRec.total_recommand_user_count || 0}}</span>
                        </div>
                        <p class="wj-rec-people-r" v-if="totalMonthRec.length!==0">
                            <span class="wj-level-span">一级：<span>{{ totalMonthRec.recommand_user_first_class || 0 }}</span></span>
                            <span class="wj-level-span">二级：<span>{{ totalMonthRec.recommand_user_second_class || 0 }}</span></span>
                            <span class="wj-level-span">三级：<span>{{ totalMonthRec.recommand_user_third_class || 0 }}</span></span>
                        </p>
                    </li>
                </ul>-->

        <div v-if="myRecUserList.length!==0" class="wj-rec-reward-top-list">
            <ul>
                <li>
                    <div class="wj-left">
                        <p>本月推荐人数：<span
                                class="wj-red">{{ myRecUserList.current_month_recommand_user_count  || 0 }}</span></p>
                        <p class="wj-default-p">（1级:{{ firstRecUserArry[0] || 0 }} 2级:{{ firstRecUserArry[1] || 0 }} 3级:{{ firstRecUserArry[2] || 0 }}<span v-if="firstRecUserArry[3]>=0"> 其他级:{{ firstRecUserArry[3] }}</span>）</p>
                    </div>
                    <div @click="thisMonthRrankingList(1)">
                        <p>本月推荐人数排名：<span
                                class="wj-red">{{ myRecUserList.current_month_recommand_user_ranking  || 0 }}</span>
                        </p>
                        <p class="wj-check-detail">查看详情</p>
                    </div>
                </li>

                <li>
                    <div class="wj-left">
                        <p>上月推荐人数：<span class="wj-red">{{ myRecUserList.last_month_recommand_user_count || 0 }}</span>
                        </p>
                        <p class="wj-default-p">（1级:{{ secondRecUserArry[0] || 0 }} 2级:{{ secondRecUserArry[1] || 0 }} 3级:{{ secondRecUserArry[2] || 0 }}<span v-if="secondRecUserArry[3]>=0"> 其他级:{{ secondRecUserArry[3] }}</span>）</p>
                    </div>
                    <div @click="thisMonthRrankingList(2)">
                        <p>上月推荐人数排名：<span
                                class="wj-red">{{ myRecUserList.last_month_recommand_user_ranking || 0 }}</span></p>
                        <p class="wj-check-detail">查看详情</p>
                    </div>
                </li>

                <li>
                    <div class="wj-left">
                        <p>总推荐人数：<span class="wj-red">{{ myRecUserList.all_recommand_user_count  || 0 }}</span></p>
                        <p class="wj-default-p">（1级:{{ allRecUserArry[0] || 0 }} 2级:{{ allRecUserArry[1] || 0 }} 3级:{{ allRecUserArry[2] || 0 }}<span v-if="allRecUserArry[3]>=0"> 其他级:{{ allRecUserArry[3]}}</span>）</p>
                    </div>
                    <div @click="thisMonthRrankingList(0)">
                        <p>总推荐人数排名：<span class="wj-red">{{ myRecUserList.all_recommand_user_ranking || 0 }}</span></p>
                        <p class="wj-check-detail">查看详情</p>
                    </div>
                </li>
            </ul>
            <p class="wj-tips" v-if="firstRecUserArry[3]>=0">温馨提示：合伙人可查看所有级别推荐人的数量及相关信息</p>
        </div>

        <div v-else class="wj-rec-reward-top-list">
            <!--暂无数据-->
            <p class="wj-recommended-no-data"
               style="text-align: center;padding: .4rem;font-size:.37333333rem;color: #cac5c5;">
                暂无数据
            </p>
        </div>


        <ul class='rec-reward-month wj-rec-reward-month'>
            <li @click='chosemonth(0)' :class="monthstatus == 0?'on':'' ">
                全部
            </li>
            <li v-for='num in montharr' @click='chosemonth(num)' :class="monthstatus === num?'on':'' ">
                {{ num }}月
            </li>
        </ul>

        <table class='rec-reward-nav' cellpadding='15'>
            <tr class='nav wj-rec-reward-nav'>
                <th>绑定时间</th>
                <th>电话号码</th>
                <th>操作</th>
            </tr>
            <tr class='nav wj-rec-reward-nav' v-if="thisMonthRecList && thisMonthRecList.length!==0"
                v-for='item in thisMonthRecList'>
                <td>
                    {{item.addtime | timeformate}}
                </td>
                <td>
                    <p>
                        <img class="wj-iphone" src="/Assets/Source/img/my-rec-people-iphone.jpg" alt=""/>
                        <span class="wj-iphone-til" @click="wjDialing(item.username)">{{ item.username }}</span>
                        <!-- <img class="wj-iphone" src="/Assets/Source/img/my-rec-people-iphone.jpg" alt=""/>
                         <span class="wj-iphone-til" @click="wjDialing(item.username)">{{ item.username }}</span>-->
                    </p>
                </td>
                <td>
                    <p v-if="item.recommand_count>0">
                        <!--<router-link
                                :to="{path:'/myrecpeopletwo',query: {uid:13,iphone:item.username}}"
                                tag="span">查看下级
                        </router-link>-->
                        <router-link
                                :to="{path:'/myrecpeopletwo',query: {uid:item.id,iphone:item.username}}"
                                tag="span">查看下级
                        </router-link>
                        <span style="color:#666;">{{ item.recommand_count }}</span>
                        <img class="wj-leave" src="/Assets/Source/img/my-rec-people-right.gif" alt=""/>
                    </p>
                    <p v-else style="color:#666;">暂无下级</p>
                </td>
            </tr>
            <tr v-if="thisMonthRecList.length!==0">
                <td class="wj-click-more">
                    <p>
                        <button @click="clickMoreData">{{ dataFlagTil }}</button>
                    </p>
                </td>
            </tr>
            <tr v-if="thisMonthRecList.length===0">
                <td class="wj-click-more"><p class="no-more">暂无数据</p></td>
            </tr>
        </table>
    </div>
</template>
<style>
    .wj-rec-people-l {
        width: 35%;
    }

    .wj-rec-people-r {
        width: 65%;
    }

    .wj-rec-people-r span {
        font-size: .32rem;
        color: rgb(122, 123, 135) !important;
    }

    .wj-rec-people-r span span {
        color: rgb(45, 45, 52) !important;
    }

    .shadow {
        z-index: 5;
        padding-top: 55px;
        box-sizing: border-box
    }

    .rec-reward-title p {
        float: right;
        font-size: 12px;
        color: #7a7a7a;
    }

    .rec-reward-sure li span {
        color: #9e925f
    }

    .chose {
        background: #fff;
        height: 100%;
        width: 75%;
        position: relative;
        left: 25%;
    }

    .top {
        padding: 15px;
        background: #fff;
    }

    .top p {
        font-size: 14px;
        color: #7a7a7a;
        text-align: left;
        margin-bottom: 15px;
    }

    .rank {
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
    }

    .rank li {
        width: 40%;
        line-height: 40px;
        font-size: 12px;
        text-align: center;
        border: 1px solid #333;
        border-radius: 3px;
        margin-bottom: 15px;
    }

    .rank .on {
        background: #9e925f;
        color: #fff;
        border-color: #9e925f;
    }

    .btn {
        position: absolute;
        bottom: 0;
        width: 100%;
    }

    .btn span {
        display: inline-block;
        width: 50%;
        line-height: 40px;
        color: #fff;
        background: #d4d4d4;
        text-align: center;
    }

    .btn span + span {
        background: #ba3027
    }
</style>
<script>
    import $ from 'jquery';

    let myrecpeople = {
        data() {
            return {
                montharr: [],
                monthstatus: 0,
                typestatus: 0,
                showshadow: false,
                ranktype: 0,
                bindtype: 0,
                doarr: false,
                thisMonthRec: [], // 本月推荐信息
                thisMonthRecList: [], // 本月推荐信息列表
                totalMonthRec: [], // 总计推荐信息
                page: 1,
                pageSize: 10,
                dataFlag: false,
                dataFlagTil: "点击加载更多",
                myRecUserList: [], // 获取我的推荐用户的列表
                firstRecUserArry: [],
                secondRecUserArry: [],
                allRecUserArry: []
            }
        },
        created() {
            const date = new Date();
            const month = date.getMonth() + 1;
            for (let i = 0; i <= month - 1; i++) {
                this.montharr.unshift(i + 1);
            }
            //获取本月的推荐信息
            this.getThisMonthRecAPI(this.page, this.pageSize, 0);
            // 获取顶部的全部信息
            this.getTotalMonthRecAPI();
            // 获取我的推荐用户第一层的顶部所有信息
            this.getMyRecUserListAPI();
        },
        methods: {
            // 点击查看详情跳转
            thisMonthRrankingList(monthNum) {
                let date = new Date(),
                    year = date.getFullYear(),
                    month = date.getMonth() + 1,
                    isMonth = monthNum,
                    urlRouter = "/recommendrewardnumber";
                if (isMonth === 0) {
                    month = 0;
                }
                if (isMonth === 2) {
                    month = month - 1;
                }
                this.$router.push(urlRouter + "?year=" + year + "&month=" + month + "&isMonth=" + isMonth);
            },
            // 拨打电话向app端发送电话号码
            wjDialing(username) {
                window.location.href = "/Device/Dialing?tel=" + username;
            },
            // 获取我的推荐用户的数据
            getMyRecUserListAPI() {
                let _that = this;
                $.ajax({
                    url: '/WebApi/UserManager/MyRecommandUserStat',
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
                            _that.firstRecUserArry = _that.myRecUserList.current_month_each_level_recommand_user;
                            _that.secondRecUserArry = _that.myRecUserList.last_month_each_level_recommand_user;
                            _that.allRecUserArry = _that.myRecUserList.all_each_level_recommand_user;
                        }
                    }
                })
            },
            // 获取本月的推荐信息
            getThisMonthRecAPI(page, pageSize, month, flag = 1) {
                let _that = this;
                $.ajax({
                    url: '/WebApi/UserManager/MonthRecommandUserData',
                    type: 'post',
                    data: {
                        "month": month,
                        "token": encodeURIComponent(_that.$store.state.shorttoken),
                        "Page": page,
                        "PageSize": pageSize
                    },
                    complete: (xhr, textStatus) => {
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(xhr.responseJSON.message);
                            return;
                        }
                        // 成功
                        if (xhr.responseJSON.status == 0) {
                            if (xhr.responseJSON.data.list.length < 10) {
                                _that.dataFlag = true;
                                _that.dataFlagTil = "暂无数据";
                            }
                            _that.thisMonthRec = xhr.responseJSON.data;
                            _that.thisMonthRecList.push(...xhr.responseJSON.data.list);
                        }
                    }
                })
            },
            // 获取顶部的全部信息
            getTotalMonthRecAPI() {
                let _that = this;
                $.ajax({
                    url: '/WebApi/UserManager/UserRecommandCount',
                    type: 'post',
                    data: {
                        "token": encodeURIComponent(_that.$store.state.shorttoken)
                    },
                    complete: (xhr, textStatus) => {
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(xhr.responseJSON.message);
                            return;
                        }
                        // 成功
                        if (xhr.responseJSON.status === 0) {
                            _that.totalMonthRec = xhr.responseJSON.data;
                        }
                    }
                })
            },
            // 初始化基本信息
            initData() {
                this.dataFlagTil = "点击加载数据";
                this.dataFlag = false;
                this.thisMonthRecList = [];
                this.page = 1;
            },
            // 获取每月推荐信息
            chosemonth(sta) {
                this.initData();
                this.monthstatus = sta;
                this.getThisMonthRecAPI(1, 10, sta);
            },
            // 点击加载更多
            clickMoreData() {
                if (this.dataFlag) {
                    this.dataFlagTil = "数据已加载完成";
                    return;
                }
                this.dataFlagTil = "点击加载数据";
                this.dataFlag = false;
                this.getThisMonthRecAPI(this.page += 1, this.pageSize, this.monthstatus, 0);
            },
            // 返回上一页
            back() {
                this.$router.go(-1);
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
        }
    }
    export default myrecpeople
</script>