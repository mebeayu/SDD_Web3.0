<!-->我的页面<-->
<template>
    <div v-cloak>
        <!--中间主要内容区开始-->
        <div class="wj-my-cont">
            <!--头部区域开始-->
            <div class="wj-my-top-header">
                <ul class="wj-my-customer-list">
                    <router-link tag="li" to="/mymsgpush">
                        <img src="/Assets/Source/img/my-msg-push.png"/>
                        <span v-if="msgPushVal">{{ msgPushVal | filterMsgData }}</span>
                    </router-link>
                    <li @click="CustomerService"><img src="/Assets/Source/img/客户服务@3x.png" alt=""/></li>
                    <li>
                        <router-link tag="img" src="/Assets/Source/img/设置的副本@3x.png" to="/myset"/>
                        </router-link></li>
                </ul>

                <!--登录过显示的区域-->
                <!-- -->
                <div v-if="isLogin()" class="wj-my-user-pic">
                    <!--<img src="/Assets/Source/img/我的拷贝7@3x_2.png" alt=""/>-->
                    <img :src="'/SWebApi/Public/picture/' + head_pic+'.jpg'"
                         :onerror="$store.state.defaultimg_v2" alt=""/>
                    <!-- <img v-else :src="imgFileSrc"
                          :onerror="$store.state.defaultimg" alt=""/>-->
                    <input :disabled="fileButton" @change="fileChange($event)"  accept="image/*" class="wj-my-head-sculpture"
                           type="file"  />
                    <span>{{ username }}</span>
                </div>

                <!--未登录过显示的区域-->
                <div @click='login' v-else class="wj-my-user-pic">
                    <img :src="$store.state.defaultimg_src" :onerror="$store.state.defaultimg_v2" alt=""/>
                    <span>登录/注册</span>
                </div>

                <div class="wj-my-top-nav-list">
                    <ul>
                        <li @click="thisMonthLookDetail('/goodscollection')"><img src="/Assets/Source/img/收藏的副本@3x.png"
                                                                                  alt=""/>
                            <p>我的收藏</p></li>
                        <li @click="recommendationCode(true)"><img src="/Assets/Source/img/矩形40拷贝2@3x_46.png" alt=""/>
                            <p>我的推荐码</p></li>
                        <li @click="thisMonthLookDetail('/mycard')"><img style="width:.5333333333333333rem;"
                                                                         src="/Assets/Source/img/优惠券@3x.png" alt=""/>
                            <p>我的卡券</p></li>
                        <li @click="thisMonthLookDetail('/wallet')"><img src="/Assets/Source/img/矩形33@3x.png" alt=""/>
                            <p>我的钱包</p></li>
                    </ul>
                </div>
            </div>
            <!--头部区域结束-->

            <!--全部签订单开始-->
            <div>
                <div @click="thisMonthLookDetail('/order/0')"
                     class="wj-my-header-h6">
                    <h6>全部订单</h6>
                    <p><span>全部</span><img src="/Assets/Source/img/下一步@3x.png" alt=""/></p>
                </div>

                <div class="wj-my-all-order-list wj-my-border-b">
                    <ul>
                        <li @click="thisMonthLookDetail('/order/1')"><img
                                src="/Assets/Source/img/待付款@3x.png" alt=""/>
                            <p>待付款</p></li>
                        <li @click="thisMonthLookDetail('/order/2')"><img
                                src="/Assets/Source/img/标签-发货@3x.png" alt=""/>
                            <p>待收货</p></li>
                        <li @click="thisMonthLookDetail('/order/3')"><img
                                src="/Assets/Source/img/评论-2@3x.png" alt=""/>
                            <p>待评价</p></li>
                        <li @click="thisMonthLookDetail('/order/4')"><img
                                src="/Assets/Source/img/售后@3x.png" alt=""/>
                            <p>退货/售后</p></li>
                    </ul>
                </div>
            </div>
            <!--全部签订单结束-->

            <!--我的推荐奖励开始-->
            <div>
                <div @click="thisMonthLookDetail('/myrecreward')"
                     class="wj-my-header-h6">
                    <h6>我的推荐奖励</h6>
                    <p><span>更多</span><img src="/Assets/Source/img/下一步@3x.png" alt=""/></p>
                </div>

                <div class="wj-my-recommended-awards-list wj-my-border-b">
                    <ul>
                        <li><img src="/Assets/Source/img/图层23拷贝@3x.png" alt=""/>
                            <p><span>本月奖励</span><span
                                    v-if="isLogin()">￥{{ useraccount.month_recommand_award || 0 }}</span><span
                                    v-else>0</span></p></li>
                        <li @click="thisMonthLookDetail('/recommendrewardranking')">
                            <img src="/Assets/Source/img/圆角矩形1拷贝2@3x.png" alt=""/>
                            <p><span>本月奖励排名<i v-if="isLogin()">{{ useraccount.month_recommand_rewards_ranking | formatRankingNum }}</i><i
                                    v-else>-</i></span><span class="wj-my-check-more"><i>查看详情</i><img
                                    class="wj-my-check-img" src="/Assets/Source/img/下一步@3x.png"/></span></p>
                        </li>
                        <li><img src="/Assets/Source/img/形状2@3x.png" alt=""/>
                            <p><span>待确认奖励</span><span>{{ useraccount.month_unconfirm_order_sum_reward }}</span></p>
                        </li>
                    </ul>
                </div>
            </div>
            <!--我的推荐奖励结束-->

            <!--我的代理信息开始-->
            <div class="wj-my-border-b" @click='agent' v-show="show_agent">  
                <div class="wj-my-header-h6">
                    <h6>我的代理信息</h6>
                </div>
            </div>  
            <!--我的代理信息结束-->


            <!--我的推荐用户开始-->
            <div>
                <div @click="thisMonthLookDetail('/myrecpeople')"
                     class="wj-my-header-h6">
                    <h6>我的推荐用户</h6>
                    <p><span>更多</span><img src="/Assets/Source/img/下一步@3x.png" alt=""/></p>
                </div>

                <div class="wj-my-recommended-user-list wj-my-border-b">
                    <ul class="wj-my-recommended-user">
                        <li class="wj-my-recommended-num">
                            <div class="wj-my-recommended-l">
                                <img src="/Assets/Source/img/人物，我的@3x.png" alt=""/>
                                <div class="wj-my-recommended-detail">
                                    <p>本月推荐人数</p>
                                    <span>{{ useraccount.month_recommand_user_count || 0 }}</span>
                                </div>
                            </div>

                            <div class="wj-my-recommended-r-list">
                                <!--<ul>
                                    <li><span>一级</span>{{ useraccount.recommand_user_first_class || "-" }}</li>
                                    <li><span>二级</span>{{ useraccount.recommand_user_second_class || "-" }}</li>
                                    <li><span>三级</span>{{ useraccount.recommand_user_third_class || "-" }}</li>
                                    <li><span>其他级</span>-</li>
                                </ul>-->
                                <ul>
                                    <li><span>一级</span>{{ useraccountLeave[0] || 0 }}</li>
                                    <li><span>二级</span>{{ useraccountLeave[1] || 0 }}</li>
                                    <!-- <li><span>三级</span>{{ useraccountLeave[2] || 0 }}</li>
                                    <li v-if="useraccountLeave[3]>=0"><span>其他级</span>{{ useraccountLeave[3]}}</li> -->
                                </ul>
                            </div>
                        </li>

                        <li @click="thisMonthLookDetail('/recommendrewardnumber')" class="wj-my-recommended-ranks">
                            <div class="wj-my-recommended-l">
                                <img src="/Assets/Source/img/圆角矩形1拷贝2@3x_78.png" alt=""/>
                                <div class="wj-my-recommended-detail">
                                    <p>本月推荐人数排名</p>
                                    <span>{{ useraccount.month_recommand_user_count_ranking || 0  }}</span>
                                </div>
                            </div>

                            <div class="wj-my-recommended-r">
                                <span class="wj-my-check-more"><i>查看详情</i><img class="wj-my-check-img"
                                                                               src="/Assets/Source/img/下一步@3x.png"/></span>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <!--我的推荐用户结束-->

            <!--我的分享开始-->
            <div class='shadow' v-show='showqrcode == true' @click="recommendationCode(false)">
                <div class="wj-right-share" v-show='show' @click="showshaw">
                    分享
                </div>

                <div id='aoa' class="wj-aoa">
                    <qrcode v-bind:value='add' id='xoxo' style='display:none'>
                    </qrcode>
                    <img id='IMG'/>
                    <div class='sharetext'>
                        {{word}}
                    </div>
                </div>
            </div>
            <!--我的分享结束-->

        </div>
        <!--中间主要内容区结束-->

        <!--底部导航区-->
        <bottom></bottom>
    </div>
</template>

<style scoped>
    /*分享*/
    .wj-aoa {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 5rem;
        height: 5rem;
        transform: translate(-50%, -50%);
    }

    .sharetext {
        font-size: .4rem !important;
        text-align: center;
        color: #fff;
        padding-top: .4rem;
        margin-bottom: 10px;
    }

    #aoa img {
        width: 100%;
        height: 100% !important;
        border-radius: .2rem;
        overflow: hidden;
    }

    .wj-right-share {
        position: absolute;
        right: .3rem;
        top: .3rem;
        padding: .14rem .6rem;
        color: #f00;
        background: #fff;
        border-radius: 38px;
        font-size: .4rem;

    }

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

    .wj-bottom-navbar {
        position: fixed !important;
    }

    #last-item {
        margin-bottom: 1.7066666666666668rem;
    }

    .wallet-support {
        padding-top: 0.53333333rem;
    }
</style>

<script>
    import bottom from './bottom';
    import $ from 'jquery';

    let my = {
        data() {
            return {
                show_agent:false, //代理信息入口显示
                showqrcode: false,
                show:true,
                word:'长按图片/点击右上角分享',
                add: '',
                id: '',
                //isLoginFlag: this.$store.state.shorttoken, // 检测是否登陆
                useraccount: [], // 我的推荐奖励 + 我的推荐用户
                useraccountLeave: [], // 本月推荐人数的一级 + 二级 + 三级 + 其他级
                fileButton: false,
                imgFileSrc: "", // 用户登陆后的上传头像

                filesArr: [],
                userInfoList: [], // 存储用户信息
                msgPushVal: "",    // 获取未读消息总数量
                head_pic:"0",
                username:"登录/注册"
            }
        },
        components: {
            bottom,
        },
        filters: {
            // 格式化本月/上月/总的排名   例如：0： 1000+  否则返回原来的
            formatRankingNum(val) {
                if (val === 0) {
                    return "1000+";
                }
                return val;
            },
            filterMsgData(val) {
                if (val >= 9) {
                    return 9 + "+";
                }
                return val;
            }
        },
        created() {
            // alert("state.token="+this.$store.state.shorttoken+"----localStorage="+localStorage.getItem('shorttoken'));
            //获取用户信息
            //debugger;
            this.getUserInfoAPI();

            // 获取消息提醒的数量
            this.getUnreadMessageCount();
        },
        mounted(){
        },
        computed: {
            /*token() {
                if (!this.isLoginFlag) return 0;
                return this.$store.state.shorttoken;
            }*/
        },
        watch: {
            /*token() {
                if (!this.isLoginFlag) return 0;
                this.getUserRecommendAPI();
            }*/
        },
        methods: {
        	agent(){
              this.$router.push('/agent');
            },
        	isLogin(){
        		var token=localStorage.getItem('shorttoken');
        		if(token)
        		{
        			return true;
        		}
        		return false;
        	},
        	getToken(){
        		var token=localStorage.getItem('shorttoken');
        		if(token)
        		{
        			return token;
        		}
        		return '';
        	},
            // 获取未读消息总数量
            getUnreadMessageCount() {
                let _this = this;
                if (!_this.isLogin()) return;
                $.ajax({
                    url: "/WebApi/Message/UnreadMessageCount",
                    data: {
                        "token": _this.getToken()
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
                            _this.msgPushVal = res.data;
                        }
                    }
                });
            },
            // 获取用户信息
            getUserInfoAPI() {
                 let _this = this;
                   if (_this.$store.state.my_usrInfoList) {
                   	   _this.userInfoList=_this.$store.state.my_usrInfoList;
                   	   _this.head_pic=_this.userInfoList[0].head_pic;
                   	   _this.username=_this.userInfoList[0].nick_name || _this.userInfoList[0].username;
                       _this.add = 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + _this.getToken();
                       _this.getUserRecommendAPI();
                       if(_this.userInfoList[0].is_agent==1){
                       		_this.show_agent=true;
                       }
                       return;
                   }

                $.ajax({
                    url: "/WebApi/UserManager/UserInfo?token=" + _this.getToken(),
                    type: "get",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            _this.userInfoList=_this.$store.state.my_usrInfoList=res.data;
                            _this.head_pic=_this.userInfoList[0].head_pic;
                            _this.username=_this.userInfoList[0].nick_name || _this.userInfoList[0].username;
                            _this.add = 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + _this.getToken();
                            _this.getUserRecommendAPI();
                            if(res.data[0].is_agent==1){
                                _this.show_agent=true;
                            }
                        }
                    }
                });
            },
            /*getUserInfor() {
                this.$store.dispatch('get_userinfo').then(() => {
                    this.id = sessionStorage.getItem('myid');
                    this.add = 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + this.token;
                    this.getUserRecommendAPI();
                });
            },*/
            // 上传用户图像
            fileChange(el) {
                let fileName = el.target.value,
                    files = el.target.files,
                    self = this;
                // 如果没有选中文件，直接返回
                if (files.length === 0) return;

                // 判断文件大小
                if (!el.target.files[0].size) {
                    alert("上传的图片不能为空！");
                    return;
                }

                // 判断文件类型
                if (fileName) {
                    let suffixIndex = fileName.lastIndexOf("."),
                        suffix = fileName.substring(suffixIndex + 1).toUpperCase();
                    if (suffix !== "BMP" && suffix !== "JPG" && suffix !== "JPEG" && suffix !== "PNG" && suffix !== "GIF") {
                        alert("请上传图片（格式BMP、JPG、JPEG、PNG、GIF等）!");
                        return;
                    }
                }

                for (let i = 0; i < files.length; i++) {
                    let file = files[i];
                    self.filesArr.push(file);
                    let reader = new FileReader();
                    // if (file.size > 1048576) {
                    //     alert('图片太大，不允许上传！');
                    //     return;
                    // }
                    this.fileButton = true;
                    reader.readAsDataURL(file);
                    reader.onload = function (e) {
                        let img = new Image();
                        img.onload = function () {
                            let canvas = document.createElement('canvas');
                            let ctx = canvas.getContext('2d');
                            let w = img.width;
                            let h = img.height;
                            let scale = w / h;
                            w = parseInt(300);
                            h = parseInt(w / scale);
                            // 设置 canvas 的宽度和高度
                            canvas.width = w;
                            canvas.height = h;
                            ctx.drawImage(img, 0, 0, w, h);
                            let base64 = canvas.toDataURL('image/jpeg').split("base64,")[1];
                            // 插入到预览区
                            self.setUserHeadPicAPI(base64);

                        };
                        img.src = e.target.result;
                    };
                   
                }

            },

            // 发送图片地址到后端
            setUserHeadPicAPI(src) {
                let _this = this;
                $.ajax({
                    url: "/WebApi/UserManager/SetUserHeadPic",
                    data: {
                        "token": encodeURIComponent(_this.$store.state.shorttoken),
                        "base64": src
                    },
                    type: "post",
                    complete: (xhr, textStatus) => {
                        let res = xhr.responseJSON;
                        // 失败
                        if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
                            alert(res.message);
                            _this.fileButton = false;
                            return;
                        }
                        // 成功
                        if (res.status === 0) {
                            /*_this.$store.dispatch('get_userinfo').then(() => {
                                _this.id = sessionStorage.getItem('myid');
                                _this.add = 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + this.getToken();
                                _this.getUserRecommendAPI();
                            });*/
                            
                           	 _this.$store.state.my_usrInfoList[0].head_pic=res.data;
                             _this.userInfoList=_this.$store.state.my_usrInfoList;
                              _this.head_pic=res.data;
                            // _this.getUserInfoAPI();

                            _this.fileButton = false;
                        }
                    }
                });
            },

            // 我的订单 + 查看详情 + 我的推荐奖励 + 我的推荐用户 ===> 跳转至对应的页面你，否则跳至登录页
            thisMonthLookDetail(urlRouter) {
                if (this.isLogin()) {
                    let date = new Date(),
                        year = date.getFullYear(),
                        month = date.getMonth() + 1,
                        isMonth = 0; // 默认总/全部
                    if (urlRouter === "/recommendrewardnumber") {
                        isMonth = 1; // 本月：1  上月：2  总的：不传
                        urlRouter = urlRouter + "?year=" + year + "&month=" + month + "&isMonth=" + isMonth;
                    }
                    if (urlRouter === "/recommendrewardranking") {
                        isMonth = 1; // 本月：1  上月：2  总的：不传
                        urlRouter = urlRouter + "?year=" + year + "&month=" + month + "&isMonth=" + isMonth;
                    }
                    this.$router.push(urlRouter);
                } else {
                    this.$router.push("/login?redirect=/#/my");
                }
            },
            // 获取我的推荐奖励和我的推荐用户数据
            getUserRecommendAPI() {
                let _this = this;
                $.ajax({
                    url: "/WebApi/UserManager/UserRecommandCountV2",
                    data: {
                        "token": encodeURIComponent(_this.$store.state.shorttoken)
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
                            _this.useraccount = res.data;
                            _this.useraccountLeave = _this.useraccount.each_level_recommand_user_count;
                        }
                    }
                });
            },
            // 跳转至登录页面
            login() {
                this.$router.push('/login?redirect=/#/my');
            },
            // 客服系统
            CustomerService() {
                let iframe = encodeURIComponent("https://kefu.easemob.com/webim/im.html?configId=8d149d8f-bc70-45f8-9c44-96c6ef6098c9");
                window.location.href = "/Game/CustomerService?iframe=" + iframe + "&redirect=" + encodeURIComponent("/#/my");
            },
            // 分享
            showshaw() {
                var title = encodeURIComponent("新用户注册领取红包");
                var recommend = encodeURIComponent("重磅福利!省兜兜红包天天送~");
                var pic_id = "35642";
                //var redirect="https://e-shop.rrlsz.com.cn/Event/Xxxshare?token="+encodeURIComponent(this.$store.state.shorttoken);
                var redirect = 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + this.getToken();
                if (ua.indexOf('iphone') > -1) {
                    redirect = encodeURIComponent(redirect);
                }
                window.location.href = "/Event/SharingGuide?title=" + title + "&recommend=" + recommend + "&pic_id=" + pic_id + "&redirect=" + redirect;
            },
            // 收藏
            recommendationCode(sta) {
                var ua = navigator.userAgent.toLowerCase();
                var isWeixin = ua.indexOf('micromessenger') != -1;
                if (isWeixin) {
                    this.show=false
                    this.word='长按图片分享'
                }
                if (this.$store.state.shorttoken) {
                    this.showqrcode = sta;
                    const myCanvas = document.getElementsByTagName("canvas")[0];
                    const img = this.convertCanvasToImage(myCanvas);
                    const box = $('#IMG');
                    IMG.src = img.src;
                } else {
                    this.$router.push('/login?redirect=/#/my');
                }
            },
            convertCanvasToImage(canvas) {
                //新Image对象,可以理解为DOM;
                let image = new Image();
                //canvas.toDataURL返回的是一串Base64编码的URL,当然,浏览器自己肯定支持
                //指定格式PNG
                try {
                    image.src = canvas.toDataURL("image/png");
                } catch (e) {
                }
                return image;
            },
        }
    };
    export default my;
</script>