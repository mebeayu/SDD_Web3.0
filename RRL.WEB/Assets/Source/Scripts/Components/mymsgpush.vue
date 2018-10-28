<!-->消息推送页<-->
<template>
    <div>
        <viewtitle></viewtitle>
        <div class="my-msg-push-cont">
            <div v-if="isTokenFlag">
                <ul class="my-msg-push-list" v-if="msgPageList && msgPageList.length>0">
                    <li v-for="(item,index) in msgPageList" :key="index">
                        <h6 class="head"><span>[已读消息]</span><span>{{ item.title }}</span></h6>
                        <p class="time">{{ item.create_time }}</p>
                        <p class="detail">{{ item.content }}</p>
                    </li>
                </ul>
                <p v-if="msgPageList && msgPageList.length===0" class="no-data">暂无可读消息</p>
                <div v-show="!isPageListFlag" v-else @click="clickMoreDate" class="more">
                    <button>{{ clickMoreTil }}</button>
                </div>
                <p v-if="msgPageList && msgPageList.length>0 && isPageListFlag" class="no-data">数据已加载完毕 ~</p>
            </div>
            <p v-else class="no-data">请先登录，再来查看</p>
        </div>
    </div>
</template>

<script>
    import viewtitle from './title';

    let mymsgpush = {
        data() {
            return {
                isTokenFlag: this.$store.state.shorttoken,
                msgPageList: [],            // 获取消息分页数据
                curPage: 1,                 // 当前第几页
                curPageSize: 5,             // 当前显示几条数据
                isPageListFlag: false,      // 判断是否有更多的数据
                clickMoreTil: "点击加载更多"  // 更多的数据文字内容
            }
        },
        components: {
            viewtitle
        },
        created() {
            // 获取消息分页数据
            this.getUnreadMessageCountAPI(this.curPage, this.curPageSize);
            // 设置消息为已读
            this.SetAsHasBeenReadAPI();
        },
        methods: {
            back() {
                this.$router.go(-1)
            },
            // 点击加载更多
            clickMoreDate() {
                if (this.isPageListFlag) return;
                this.clickMoreTil = "数据在加载中......";
                this.getUnreadMessageCountAPI(this.curPage += 1);
            },
            // 设置消息为已读
            SetAsHasBeenReadAPI() {
                let _this = this,
                    token = _this.isTokenFlag;
                $.ajax({
                    url: "/WebApi/Message/SetAsHasBeenRead",
                    data: {
                        "token": token
                    },
                    type: "post",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            return;
                        }
                    }
                });
            },
            // 获取消息分页数据
            getUnreadMessageCountAPI(page, pageSize = 5) {
                let _this = this,
                    token = _this.isTokenFlag;
                $.ajax({
                    url: "/WebApi/Message/MessageByPage",
                    data: {
                        "token": token,
                        "Page": page,
                        "PageSize": pageSize
                    },
                    type: "post",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            this.clickMoreTil = res.message;
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            if (res.data < pageSize) {
                                _this.isPageListFlag = true;
                            }

                            if(res.data === null){
                                _this.msgPageList = [];
                            }else{
                                _this.msgPageList.push(...res.data);
                            }
                            _this.clickMoreTil = "点击加载更多";
                        }
                    }
                });
            },
        }
    }

    export default mymsgpush
</script>