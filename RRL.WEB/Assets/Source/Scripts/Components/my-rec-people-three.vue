<template>
    <div style='background:#f5f5f5'>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" @click="back"/>
            <p>查看下级</p>
        </div>
        <ul class='rec-reward-title wj-rec-reward-title'>
            <li class="wj-top-rec-reward-til">
                <div>上级号码：<span>{{ (iphone || 0) | formatSecrecy }}</span></div>
                <div>全部：<span>{{ twoCount || 0 }}</span></div>
            </li>
        </ul>

        <table class='rec-reward-nav' cellpadding='15'>
            <tr class='nav wj-rec-reward-nav'>
                <th>绑定时间</th>
                <th>电话号码</th>
            </tr>
            <tr class='nav wj-rec-reward-nav' v-if="twoDataList.length!==0" v-for='item in twoDataList'>
                <td>
                    {{ item.addtime | timeformate }}
                </td>
                <td>
                    <p>
                        <!--<img class="wj-iphone" src="/Assets/Source/img/my-rec-people-iphone.jpg" alt=""/>-->
                        <span class="wj-iphone-til" style="color:#2d2d34;">{{ item.username | formatSecrecy }}</span>
                        <!-- <span class="wj-iphone-til" @click="wjDialing(item.username)">{{ item.username }}</span>-->
                    </p>
                </td>
            </tr>
            <tr v-if="twoDataList.length!==0">
                <td class="wj-click-more">
                    <p>
                        <button @click="clickMoreData">{{ dataFlagTil }}</button>
                    </p>
                </td>
            </tr>
            <tr v-if="twoDataList.length===0">
                <td class="wj-click-more"><p class="no-more">暂无数据</p></td>
            </tr>
        </table>
    </div>
</template>
<style scoped>
    .rec-reward-nav .wj-rec-reward-nav th, .rec-reward-nav .wj-rec-reward-nav td{
        width: 50%;
    }
</style>
<script>
    let myrecpeoplethree = {
        data() {
            return {
                uid: null,
                iphone: null,
                twoData: [],
                twoDataList: [],
                twoCount: null,
                page: 1,
                pageSize: 10,
                dataFlag: false,
                dataFlagTil: "点击加载更多"
            }
        },
        created() {
            let queryVal = this.$route.query;
            this.uid = queryVal.uid;
            this.iphone = queryVal.iphone;
            this.getSperaderListByUidAPI(this.page, this.pageSize);
        },
        methods: {
            // 拨打电话向app端发送电话号码
            wjDialing(username) {
                window.location.href = "/Device/Dialing?tel=" + username;
            },
            getSperaderListByUidAPI(page, pageSize) {
                let _that = this;
                $.ajax({
                    url: '/WebApi/UserManager/getSperaderListByUid',
                    type: 'get',
                    data: {
                        "uid": _that.$route.query.uid,
                        "token": encodeURIComponent(_that.$store.state.shorttoken),
                        "pageIndex": page,
                        "PageSize": pageSize
                    },
                    complete: (xhr, textStatus) => {
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(xhr.responseJSON.message);
                            return;
                        }
                        // 成功
                        if (xhr.responseJSON.status === 0) {
                            if (xhr.responseJSON.data.list.length < 10) {
                                _that.dataFlag = true;
                            }
                            _that.twoData = xhr.responseJSON.data;
                            _that.twoDataList.push(...xhr.responseJSON.data.list);
                            _that.twoCount = _that.twoData.lv_one_total_spreader_count;
                        }
                    }
                })
            },
            back() {
                this.$router.go(-1);
            },
            // 点击加载更多
            clickMoreData() {
                if (this.dataFlag) {
                    this.dataFlagTil = "数据已加载完成";
                    return;
                }
                this.getSperaderListByUidAPI(this.page += 1, this.pageSize, this.monthstatus, 0);
            },
        },
        filters: {
            timeformate(time) {
                let fortime = time.replace('T', ` `);
                let arr = time.split('T');
                let montharr = arr[0].split('-');
                let timestamparr = arr[1].split(':');
                let timestr = montharr[1] + '-' + montharr[2] + ` ` + timestamparr[0] + ':' + timestamparr[1];
                return timestr;
            },
            phoneformate(num) {
                return num.substr(0, 3) + "****" + num.substr(7);
            },
            // 格式化电话号码 === 例如： 134****1234
            formatSecrecy(val) {
                if (val) {
                    return val.slice(0, 3) + "****" + val.slice(7);
                }
                return val;
            }
        }
    }
    export default myrecpeoplethree;
</script>