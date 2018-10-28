<!-- 购物车页 -->
<template>
    <div class="expect">
        <!--头部导航开始-->
        <header class="expect-navbar">
            <h4>敬请期待</h4>
            <router-link  class="goback-btn" to='/'><img src="/Assets/Game/img/left.png"></router-link>
        </header>
        <!--头部导航结束-->
        <div class="expect_content">
            <div class="expect_box">
                <img src="/Assets/Source/img/expect_05.png" alt="敬请期待">
            </div>
            <router-link  class="expect_btn" to='/' >
                返回首页
            </router-link >
        </div>
    </div>
</template>

<style scoped lang='scss'>
    @function rem($px){
         $rem: 75px;  
         @return   ($px/$rem) + rem;
    } 
    .expect-navbar{
        box-sizing: border-box;
        font-size: .45rem;
        color: #2d2d2d;
        text-align: center;
        border-bottom: .02666667rem solid #dcdcdc;
        width: 100%;
        background: #fff;
        position: relative;
        height: 1.2rem;
        line-height: 1.2rem;
        z-index: 8;
        .goback-btn{
            position: absolute;
            left: .2rem;
            top: .1rem;
        }
    }
    .expect_content{
        width: 100%;
        height: 100%;
        position: fixed;
        top: 1.2rem;
        left: 0;
        background: #f0f0f0;
        text-align: center;
        .expect_box{
            width:rem(350px);
            height:rem(386px); 
            margin:20% auto  2%;
            img{
                width: 100%;
                height: auto;
            }
        }
        .expect_btn{
            text-align: center;
            color: #fff;
            font-size: .38rem;
            border-radius: .533333rem;
            width: 3.2rem;
            height: 1rem;
            line-height: 1rem;
            background: #65c4aa;
            border: none;
            display: block;
            margin: 0 auto;
        }   
    }
</style>

<script>
    import $ from "jquery";
    import bottom from './bottom';
    import VueLazyload from 'vue-lazyload'  //引入这个懒加载插件
    Vue.use(VueLazyload)

    // 或者添加VueLazyload 选项
    import Vue from 'vue';

    Vue.use(VueLazyload, {
        preLoad: 1.3,
        error: 'Assets/Source/img/默认图像.png',
        loading: 'Assets/Source/img/默认图像.png',
        attempt: 1,
        listenEvents: ['scroll']
    });

    let shopcart = {
        components: {
            bottom
        },
        data() {
            return {
                shopCartList: [],        // 所有购物车的商品
                shopCartListY: [],       // 没有失效的购物车的商品
                shopCartListN: [],       // 失效的购物车的商品
                guessYouLike: [],        // 猜你喜欢
                startX: 0,
                endX: 0,
                isCheckedYes: false,     // 单选
                isAllCheckedYes: false,  // 全选
                isChangeText: false,      // 编辑 => 完成
                alertFlag: false,
                alertImg: null,
                alertTil: null,
                alertImgData: ["status_loading.gif", "sign-check-icon.png", "sign-error-icon.png"],
                alertTilData: ["数据加载中 ...", "数据加载成功", "数据加载异常"],
                is_show_game: "1"
            }
        },
        created() {
            this.is_show_game = window.is_show_game;
            this.getShopCartListAPI();
            this.getGuessYouListAPI(1, 6);
        },
        filters: {
            // 过滤所有价格
            formatPrice(oldVal) {
                return (oldVal).toFixed(2);
            }
        },
        computed: {
            // 计算全选 合计 下单是否显示
            isGetComputShow() {
                return this.shopCartListY.some(item => item.isCheckedYes);
            },
            // 计算合计
            getComputPriceT() {
                let total = null;
                this.shopCartListY.forEach(item => {
                    if (item.isCheckedYes) {
                        total += item.price * item.goods_count;
                    }
                });
                return total;
            }
        },
        methods: {
            // 跳转至游戏页面
            goGameHall() {
                //window.location.href = "/Game/LeisureGameArea?token=" + encodeURIComponent(this.$store.state.shorttoken);
                window.location.href = "/#/gamehall?token=" + encodeURIComponent(this.$store.state.shorttoken);
            },
            // 去逛逛
            goStroll() {
                this.$router.push("/sortofgoods");
                /*if (this.$store.state.shorttoken.length === 0) {
                    this.$router.push("/login?redirect=" + encodeURIComponent("/#/shopcart"));
                    return;
                }
                this.$router.push("/goodssort");*/
            },
            // 失效商品清空
            shopCartListClean() {
                let arrId = [];
                let flag = this.shopCartListN.some(item => item.checked);
                if (!flag) {
                    alert("客官~ 请先选择一件商品再来清空");
                }
                this.shopCartListN = this.shopCartListN.filter((item, index) => {
                    if (item.checked) {
                        arrId.push(item.id);
                        this.removeShopCartListAPI(arrId.toString());
                        return !item.checked;
                    }
                })
            },
            // 下单
            tomakeorder() {
                let list = [], _this = this;
                this.shopCartListY.forEach((item, index) => {
                    // 购物车选中才往确认订单传值
                    if (item.isCheckedYes) {
                        list.push(item.id);
                    }
                });
                $.ajax({
                    url: '/WebApi/TradeManager/CreateOrderFromCartV3?cartlist=' + list.toString() + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                clearTimeout(timer);
                            }, 1000);
                            // alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            // console.log("下单成功");
                            //获取购物车列表
                            _this.$router.push('/makesureorder/:shopcart/' + XMLHttpRequest.responseJSON.data.toString());
                            clearTimeout(timer);
                        }, 1000);
                    }
                });
            },
            // 猜你喜欢调取的API
            getGuessYouListAPI(start, end) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/ShopManager/GoodsList?Page=' + start + '&PageSize=' + end,
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.guessYouLike = XMLHttpRequest.responseJSON.data;
                    }
                });
            },
            // 点击加减调取的API
            getChangeNumAPI(id, goodsCount) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/AddGoodsToCart?goods_id=' + id + '&goods_count=' + goodsCount + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            _this.alertImg = _this.alertImgData[2];
                            _this.alertTil = _this.alertTilData[2];
                            let timer = setTimeout(() => {
                                _this.alertFlag = false;
                                clearTimeout(timer);
                            }, 1000);
                            // alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        _this.alertImg = _this.alertImgData[1];
                        _this.alertTil = _this.alertTilData[1];
                        let timer = setTimeout(() => {
                            _this.alertFlag = false;
                            clearTimeout(timer);
                        }, 1000);
                    }
                });
            },
            // 点击加减按钮进行数字的加减
            changeNum(item, num) {
                // 加
                if (num > 0) {
                    // 显示数据加载中
                    this.alertImg = this.alertImgData[0];
                    this.alertTil = this.alertTilData[0];
                    this.alertFlag = true;

                    if (item.goods_count === 99) {
                        alert("客官~ 不能再多了");
                        return;
                    }
                    this.shopCartListY.forEach(citem => {
                        if (citem === item) {
                            citem.goods_count++;
                            this.getChangeNumAPI(citem.goods_id, 1);
                        }
                    });
                    return;
                }
                // 减
                if (num < 0) {
                    if (item.goods_count === 1) {
                        alert("客官~ 已经最少了");
                        return;
                    }
                    this.shopCartListY.forEach(citem => {
                        if (citem === item) {
                            // 显示数据加载中
                            this.alertImg = this.alertImgData[0];
                            this.alertTil = this.alertTilData[0];
                            this.alertFlag = true;

                            citem.goods_count--;
                            this.getChangeNumAPI(citem.goods_id, -1);
                        }
                    });
                }
            },
            // 点击加减中间的数字阻止默认跳转页面
            cancel() {
                return false;
            },
            // 点击顶部将完成改为编辑
            changeEditor(e) {
                let text = "编辑", target = e.target;
                if (text === target.innerHTML) {
                    target.innerHTML = "完成";
                    this.isChangeText = true;
                } else {
                    target.innerHTML = "编辑";
                    this.isChangeText = false;
                }
            },
            // 计算是否全选
            isGetComputChecked() {
                this.isAllCheckedYes = this.shopCartListY.every(item => item.isCheckedYes);
                this.shopCartListY.forEach(item => {
                    if (typeof item.isCheckedYes === "undefined") {
                        this.$set(item, "isCheckedYes", this.isAllCheckedYes);
                        item.checked = item.isCheckedYes = this.isAllCheckedYes;
                    }
                });
            },
            // 下单全选的按钮
            setAllCheckedY() {
                if (this.shopCartListY.length === 0) return;
                this.isAllCheckedYes ? this.isAllCheckedYes = false : this.isAllCheckedYes = true;
                let isChecked = this.isAllCheckedYes;
                this.shopCartListY.forEach(item => {
                    if (typeof item.isCheckedYes === "undefined") {
                        this.$set(item, "isCheckedYes", isChecked);
                        item.checked = item.isCheckedYes = isChecked;
                    }
                    item.checked = item.isCheckedYes = isChecked;
                });
                this.isGetComputChecked();
            },
            // 有商品的单选按钮
            setCheckedY(item) {
                if (typeof item.isCheckedYes === "undefined") {
                    this.$set(item, "isCheckedYes", true);
                    item.checked = item.isCheckedYes = true;
                } else {
                    item.checked = item.isCheckedYes = !item.isCheckedYes;
                }
                this.isGetComputChecked();
            },
            // 删除购物车的商品
            removeShopCartListAPI(id) {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/RemoveGoodsInCart?cart_id=' + id + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                    }
                });
            },
            // 获取购物车列表
            getShopCartListAPI() {
                let _this = this;
                $.ajax({
                    url: '/WebApi/TradeManager/CartList?token=' + encodeURIComponent(_this.$store.state.shorttoken),
                    type: "get",
                    complete: (XMLHttpRequest) => {
                        // 失败
                        if ((XMLHttpRequest.readyState !== 4) && (XMLHttpRequest.status !== 200)) {
                            alert(XMLHttpRequest.responseJSON.message);
                            return;
                        }
                        this.shopCartList = XMLHttpRequest.responseJSON.data||[];

                        this.shopCartList.forEach(item => {
                            if (item.is_out_date === "1") {
                                this.shopCartListN.push(item);
                            } else {
                                this.shopCartListY.push(item);
                            }
                        });

                    }
                });
            },
            //跳转
            skip() {
                if (this.checkSlide()) {
                    this.restSlide();
                }
            },
            //滑动开始
            touchStart(e) {
                // 记录初始位置
                this.startX = e.touches[0].clientX;
            },
            //滑动结束
            touchEnd(e) {
                // 当前滑动的父级元素
                let parentElement = e.currentTarget.parentElement;
                // 记录结束位置
                this.endX = e.changedTouches[0].clientX;
                // 左滑
                if (parentElement.dataset.type == 0 && this.startX - this.endX > 30) {
                    this.restSlide();
                    parentElement.dataset.type = 1;
                }
                // 右滑
                if (parentElement.dataset.type == 1 && this.startX - this.endX < -30) {
                    this.restSlide();
                    parentElement.dataset.type = 0;
                }
                this.startX = 0;
                this.endX = 0;
            },
            //判断当前是否有滑块处于滑动状态
            checkSlide() {
                let listItems = document.querySelectorAll('.li-remove-scroll-bar');
                for (let i = 0; i < listItems.length; i++) {
                    if (listItems[i].dataset.type == 1) {
                        return true;
                    }
                }
                return false;
            },
            //复位滑动状态
            restSlide() {
                let listItems = document.querySelectorAll('.li-remove-scroll-bar');
                // 复位
                for (let i = 0; i < listItems.length; i++) {
                    listItems[i].dataset.type = 0;
                }
            },
            //删除
            deleteItem(item) {
                // 复位
                this.restSlide();
                // 删除
                this.shopCartListY = this.shopCartListY.filter(citem => citem !== item);
                this.removeShopCartListAPI(item.id);
                this.isGetComputChecked();
            }
        }
    }
    export {
        shopcart as
            default
    }
</script>
