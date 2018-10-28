import Vue from 'vue';
import Vuex from 'vuex';
import $ from 'jquery';
import router from '../Router/vue-router.js';

import AskPromise from './Modules/promise.js'

import Login from './Modules/login.js';
import My from './Modules/my.js';
import Wallet from './Modules/wallet.js';
import Rec from './Modules/recommand.js';
import Shopcart from './Modules/shopcart.js';
import Orderdetail from './Modules/orderdetail.js';
import Gamearea from './Modules/gamearea.js';
import Goodsdetail from './Modules/goodsdetail.js';
import Makeorder from './Modules/makeorder.js';
import Mycard from './Modules/mycard.js';


//import axios from 'axios'

Vue.use(Vuex)

let uesrequire = new AskPromise()

const state = {
    //图片配置
    defaultimg: 'this.src="/Assets/Source/img/默认图像.png"',
    defaultimg_v2: 'javascript:this.src=\'/Assets/Source/img/默认图像.png\'',
    defaultimg_src: '/Assets/Source/img/默认图像.png',
    defautbanner: 'this.src="/Assets/Source/img/默认banner.png"',
    img: 'WebApi/Public/picture/',
    imgs: 'SWebApi/Public/picture/',
    img_suffix: '.jpg',
    //首页
    hotgoodslist: [],
    mainrecommend: [],
    indexbanner: [],
    coinlist: [],

    //精选
    sorthotgoods: {},
    sortbanner: [],
    sortpagelist: [],
    page: 1,

    //商品详情
    goodsid: '',
    // goodsfav: false,
    // defaultshare:true,
    // mutiple:true,

    //订单列表
    orderstatus: 0,
    orderlist: {
        all: [],
        notpay: [],
        notrecived: [],
        notcommend: [],
        return: []
    },
    orderlistpage: [1, 1, 1, 1, 1],

    //用户登录及注册
    shorttoken: '',
    login_change:false,
    rec: '',

    //组件高度
    screenWidth: document.body.clientWidth,
    bottomHeight: 0,
    topHeight: 0,

    //设置轮播
    imgindex: 0,
    timer: null,

    //订单详情
    orderdetail: {},

    //购物车
    editstatus: 0,
    shopcartlist: [],

    //地址
    addresslist: [],
    newaddinfo: {},
    receive_info: {
        id: ''
    },

    //创建订单
    cartlist: [],
    cartmoney: 0,
    totalcash: 0,

    //已创建订单预览
    orderid: [],
    PreOrderList: [],
    userdiscount: 0,
    x_money: 0,
    goldbean: 0,

    //chosegoods:[],
    //haschose:false,

    //商品收藏列表
    goodscolist: [],
    goodscopage: 1,

    //弹出框
    kefushow: false,
    alertshow: false,

    //用户信息
    useinfo: {},

    my_usrInfoList:null,
    //钱包
    UserAccountInfo: {},
    UserMoneyRecord: [],
    umrpage: 1,
    walletlist: [],
    coinlist: [],
    tips: false,

    //微信分享
    wxshare: {
        jsApiList: []
    },
    sharemsg: {},
    sharetimeline: {},
    sharepath: '',
    sharelogo: 'https://e-shop.rrlsz.com.cn/Assets/Source/img/sharelogo.png',

    //物流
    transinfo: {},
    trackcom: [],
    com: '',

    cartitem: [],
    //ordergoods:[],

    //商品分类
    sortlist: [],
    listofgoods: [],
    listofgoodspage: 1,
    sortstatus: 0,

    //搜索结果
    searchlist: [],
    searchpage: 1,
    searchword: '',

    //金币专区
    coinpagelist: [],
    coinbanner: [],

    //share
    shareid: '',
    shareimg: '',
    shareimglist: [],
    sharetext: '',

    //openid
    openid: '',

    //金币可抵扣金额
    jinbidiscount: 0,

    //支付二维码
    payqrcode: '',

    //分类id
    gameid: 0,
    coinid: 0,

    //页面title
    viewtitle: '',
    //钱包
    useraccount: {},
    mycard: {},
    bank: {},
    apply: [],
    billlist: [],

    // 分类页面点击加载更多初始化
    wjIsPageListFlag: false,
    wjClickMoreTil: "点击加载更多"

}

const mutations = {
    ///
    setshareid(state, data) {
        state.shareid = data;
    },

    ////首页
    //热门商品
    sethotgoods(state, data) {
        state.hotgoodslist = data
    },
    //主推商品
    setmaingoods(state, data) {
        state.mainrecommend = data
    },
    //首页轮播
    setindexbanner(state, data) {
        state.indexbanner = data
    },
    //金币商品
    setcoinlist(state, data) {
        state.coinlist = data;
    },


    ///商品详情
    //商品详情

    //收藏商品状态
    // changegoodsfav(state, data) {
    //     state.goodsfav = data
    // },
    // //是否用默认分享
    // setdefaultshare(state, data){
    //     state.defaultshare = data;
    // },
    // //是否有多图
    // setmutiple(state,data){
    //     state.mutiple = data
    // },

    ///一系列元素高
    getscreenwidth(state) {
        state.screenWidth = document.body.clientWidth
    },
    bottomheight(state) {
        state.bottomHeight = document.getElementById('bottom').offsetHeight
    },
    toolheight(state) {
        state.bottomHeight = document.getElementById('goodsbottom').clientHeight
    },
    topheight(state) {
        state.topHeight = document.getElementById('carttop').clientHeight
    },

    ///轮播图
    //设置轮播图手动改变
    setimgindex(state) {
        state.imgindex = 0
    },
    changesortbanner(state, status) {
        if (status == 'Left') {
            state.imgindex++
            if (state.imgindex > state.sortbanner.length - 1) {
                state.imgindex = 0
            }
        } else if (status == 'Right') {
            state.imgindex--
            if (state.imgindex < 0) {
                state.imgindex = state.sortbanner.length - 1
            }
        }
    },
    changegoodsbanner(state, status) {
        if (status == 'Left') {
            state.imgindex++
            if (state.imgindex > state.goodsbanner.length - 1) {
                state.imgindex = 0
            }
        } else if (status == 'Right') {
            state.imgindex--
            if (state.imgindex < 0) {
                state.imgindex = state.goodsbanner.length - 1
            }
        }
    },
    changebannerindex(state, status) {
        if (status == 'Left') {
            state.imgindex++
            if (state.imgindex > state.indexbanner.length - 1) {
                state.imgindex = 0
            }
        } else if (status == 'Right') {
            state.imgindex--
            if (state.imgindex < 0) {
                state.imgindex = state.indexbanner.length - 1
            }
        }
    },
    clickbannerdot(state, status) {
        state.imgindex = status
    },
    settimer(state, data) {
        state.timer = data
    },
    cleartimer(state) {

        clearTimeout(state.timer)
    },


    ///精选页
    //商品页主推
    setsorthotgoods(state, data) {
        state.sorthotgoods = data
    },
    //商品页banner
    setsortbanner(state, data) {
        state.sortbanner = data
    },
    //设置商品id
    setgoodsid(state, data) {
        state.goodsid = data
    },
    //设置商品页分页
    setpage(state) {
        state.page++
    },
    //精选页数组列表
    setsortgoods(state, data) {
        var arr = state.sortpagelist.concat(data)
        var arrResult = [];
        for (var i = 0; i < arr.length; i++) {
            arr[i] = JSON.stringify(arr[i]);//对象转成字符串的方法
            arrResult.push(arr[i]);//重新填装成一个《字符串》组成的数组
        }
        arr = [];//清空原来的数据
        arrResult = $.unique(arrResult); // 去掉重复的字符串
        for (var j = 0; j < arrResult.length; j++) {
            arrResult[j] = JSON.parse(arrResult[j]);// 重新给字符串转换成对象
            arr.push(arrResult[j]);//重新填装成一个《对象》组成的数组
        }
        state.sortpagelist = arr
    },


    ///登录和注册页
    //设置用户名和密码
    setuser_info(state, info) {
        state.rec = info.rec
    },


    ///创建订单页
    //地址列表
    getaddlist(state, data) {
        state.addresslist = data
    },
    setnewadd(state, data) {
        state.newaddinfo = data
    },
    setreceiveinfo(state, data) {
        state.receive_info = data
    },


    ///购物车
    setshopcartlist(state, data) {
        state.shopcartlist = data
    },
    addcartlist(state, data) {
        state.cartlist.push(data)
    },
    removecartlist(state, data) {
        let index = state.cartlist.indexOf(data)
        state.cartlist.splice(index, 1)
    },
    clearcartlist(state) {
        state.cartlist = []
    },
    changecartmonry(state, data) {
        //data=parseFloat(data.toFixed(2))
        state.cartmoney += data
        state.cartmoney = parseFloat(state.cartmoney.toFixed(2))
        //state.cartmoney.toFixed(2)
        //state.cartmoney.parseFloat(state.cartmoney)
    },
    clearcartmonry(state) {
        state.cartmoney = 0
        state.cartlist.length = 0
    },
    setorderid(state, data) {
        //state.orderid=state.orderid.concat(data)
        state.orderid = data
        console.log(state.orderid)
    },


    ///订单列表页
    setorderstatus(state, data) {
        state.orderstatus = data
    },
    setorderlist(state, data) {
        var arr = state.orderlist[state.orderstatus].concat(data)
        let hash = {};
        arr = arr.reduce(function (item, next) {
            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
            return item
        }, [])
        state.orderlist[state.orderstatus] = arr
    },
    uniqueorderlist(state, data) {
        state.orderlist[state.orderstatus] = data
    },


    ///订单创建预览页
    setPreOrderList(state, data) {
        state.PreOrderList = data;
    },
    settotalcash(state, data) {
        state.totalcash = data
    },
    setgoldbean(state, data) {
        state.goldbean = data;
    },

    setcartitem(state, data) {
        state.cartitem = data
    },

    ///订单列表页
    setorderlist(state, data) {
        switch (state.orderstatus) {
            case 0:
                state.orderlist.all = state.orderlist.all.concat(data)
                break;
            case 1:
                state.orderlist.notpay = state.orderlist.notpay.concat(data)
                break;
            case 2:
                state.orderlist.notrecived = state.orderlist.notrecived.concat(data)
                break;
            case 3:
                state.orderlist.notcommend = state.orderlist.notcommend.concat(data)
                break;
            case 4:
                state.orderlist.return = state.orderlist.return.concat(data)
                break;
        }
        console.log(state.orderlist)
    },
    changeorderlistpage(state, data) {
        state.orderlistpage[state.orderstatus]++
    },


    ///订单详情
    setorderdetail(state, data) {
        state.orderdetail = data
    },
    settransinfo(state, data) {
        state.transinfo = data.data
        state.com = data.com
    },
    settrackcom(state, data) {
        state.trackcom = data
    },


    ///用户详情页
    userinfo(state, data) {
        state.useinfo = data
    },
    setuserdiscount(state, data) {
        state.userdiscount = data
    },


    ///商品收藏列表
    setgoodscolist(state, data) {
        //state.goodscolist=data
        var arr = state.goodscolist.concat(data)
        var arrResult = [];
        for (var i = 0; i < arr.length; i++) {
            arr[i] = JSON.stringify(arr[i]);//对象转成字符串的方法
            arrResult.push(arr[i]);//重新填装成一个《字符串》组成的数组
        }
        arr = [];//清空原来的数据
        arrResult = $.unique(arrResult); // 去掉重复的字符串
        for (var j = 0; j < arrResult.length; j++) {
            arrResult[j] = JSON.parse(arrResult[j]);// 重新给字符串转换成对象
            arr.push(arrResult[j]);//重新填装成一个《对象》组成的数组
        }
        state.goodscolist = arr
    },
    setgoodscopage(state) {
        state.goodscopage++
    },


    ///钱包页
    setUserAccountInfo(state, data) {
        state.UserAccountInfo = data
    },
    settips(state, data) {
        state.tips = data;
    },
    setUserMoneyRecord(state, data) {
        var arr = state.UserMoneyRecord.concat(data)
        var arrResult = [];
        for (var i = 0; i < arr.length; i++) {
            arr[i] = JSON.stringify(arr[i]);//对象转成字符串的方法
            arrResult.push(arr[i]);//重新填装成一个《字符串》组成的数组
        }
        arr = [];//清空原来的数据
        arrResult = $.unique(arrResult); // 去掉重复的字符串
        for (var j = 0; j < arrResult.length; j++) {
            arrResult[j] = JSON.parse(arrResult[j]);// 重新给字符串转换成对象
            arr.push(arrResult[j]);//重新填装成一个《对象》组成的数组
        }
        state.UserMoneyRecord = arr
    },
    setwalletlist0(state, data) {
        state.walletlist[0] = data;
    },
    setwalletlist1(state, data) {
        state.walletlist[1] = data;
    },
    setwalletlist2(state, data) {
        state.walletlist[2] = data;
    },
    setwalletlist3(state, data) {
        state.walletlist[3] = data;
    },
    setcoinlist(state, data) {
        state.coinlist = data;
    },

    ///弹出框状态
    setkefushow(state, data) {
        state.kefushow = data
    },
    changealertshow(state, data) {
        state.alertshow = data
        setTimeout(function () {
            state.alertshow = false
        }, 1500)
    },


    ///微信分享配置
    SET_WXSHARE(state, data) {
        state.wxshare = data
        state.wxshare.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage']
        console.log(state.wxshare)
    },
    sharepath(state, data) {
        state.sharepath = data
    },
    sharemsg(state, data) {
        state.sharemsg = data
    },
    sharetimeline(state, data) {
        state.sharetimeline = data
    },
    sethasopenid(state) {
        hasopenid = true
    },


    ///商品分类页
    setsortlist(state, data) {
        state.sortlist = data
    },
    setlistofgoods(state, data) {
        var arr = state.listofgoods.concat(data)
        let hash = {};
        arr = arr.reduce(function (item, next) {
            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
            return item
        }, [])
        state.listofgoods = arr
    },
    setlistofgoodspage(state, data) {
        state.listofgoodspage++
    },
    clearlistofgoods(state) {
        state.wjIsPageListFlag = false;
        state.wjClickMoreTil = "点击加载更多";
        state.listofgoods = [];
    },
    clearlistofgoodspage(state) {
        state.listofgoodspage = 1
    },
    setsortstatus(state, data) {
        state.sortstatus = data
    },


    ///搜索结果
    setsearchlist(state, data) {
        let arr = state.searchlist.concat(data)
        let hash = {};
        arr = arr.reduce(function (item, next) {
            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
            return item
        }, [])
        state.searchlist = arr
    },
    uniquesearchlist(state, data) {
        state.searchlist = data
    },
    setsearchpage(state) {
        state.searchpage++
    },
    clearsearchlist(state) {
        state.searchlist = []
    },
    clearsearchpage(state) {
        state.searchpage = 1
    },

    ///金币专区
    setcoinpagelist(state, data) {
        let arr = state.coinpagelist.concat(data)
        let hash = {};
        arr = arr.reduce(function (item, next) {
            hash[next.id] ? '' : hash[next.id] = true && item.push(next);
            return item
        }, [])
        state.coinpagelist = arr
    },
    clearcoinpagelist(state) {
        state.coinpagelist = [];
    },
    setcoinbanner(state, data) {
        state.coinbanner = data;
    },


    ///openid
    setopenid(state, data) {
        state.openid = data
    },


    ///分享图片
    setshareimg(state, data) {
        state.shareimg = data;
    },
    setshareimglist(state, data) {
        state.shareimglist = data
    },
    setsharetext(state, data) {
        state.sharetext = data;
    },
    ///存储shorttoken
    SET_SHORTTOKEN(state, data) {
        state.shorttoken = data;
    },
    SET_JINBIDISCOUNT(state, data) {
        state.jinbidiscount = data;
    },
    SET_PAYQRCODE(state, data) {
        state.payqrcode = data;
    },
    SETGAMEANDCOIN(state, data) {
        state.gameid = data[0];
        state.coinid = data[1]
    },
    SET_VIEWTITLE(state, data) {
        state.viewtitle = data
    },
    SET_SEARCHWORD(state, data) {
        state.searchword = data
    },
    //钱包部分
    setuseraccount(state, data) {
        state.useraccount = data
    },
    setmycard(state, data) {
        state.mycard = data
    },
    setbank(state, data) {
        state.bank = data
    },
    //申请记录
    setapply(state, data) {
        var arr = data;
        if (data.length > 0) {
            for (var i = 0; i < state.apply.length; i++) {
                console.log(state.apply[0].id);
                if (state.apply[0].id === data[0].id) {
                    arr = data;
                    return;
                } else {
                    arr = state.apply.concat(data);
                    return;
                }
            }
        } else {
            arr = state.apply
        }
        state.apply = arr;
    },
    //消费记录
    setbilllist(state, data) {
        var arr = data;
        if (data.length > 0) {
            for (var i = 0; i < state.billlist.length; i++) {
                console.log(state.billlist[0].id);
                if (state.billlist[0].id === data[0].id) {
                    arr = data;
                    return;
                } else {
                    arr = state.billlist.concat(data);
                    return;
                }
            }
        } else {
            arr = state.billlist
        }
        state.billlist = arr;
    }
}

const actions = {
    ///首页
    //设置首页热门商品
    sethotgoods: function (context) {
        const data = {
                url: 'WebApi/ShopManager/HotRecommend?Page=1&PageSize=3',
                type: 'get',
            },
            callback = function (res) {
                context.commit('sethotgoods', res.data)
                console.log(state.hotgoodslist)
            }
        return uesrequire.require(data, callback)
    },
    //设置首页推荐商品
    setmaingoods: function (context) {
        const data = {
                url: 'WebApi/ShopManager/GoodsList?Page=1&PageSize=6',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setmaingoods', res.data)
            }
        return uesrequire.require(data, callback)
    },
    //设置首页轮播图
    setindexbanner: function (context) {
        const data = {
                url: 'WebApi/ConfigManager/MainCarousel',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setindexbanner', res.data)
            }
        return uesrequire.require(data, callback)
    },
    //首页轮播图自动播放
    autobannerchange: function (context) {
        context.commit('settimer', setTimeout(function () {
            context.commit('changebannerindex', 'Left');
            actions.autobannerchange(context)
        }, 5000))
    },
    //首页金币专区
    getcoinlist({commit}) {
        const data = {
                url: 'WebApi/ShopManager/ExChangeAreaGoodsList?OrderField=order_weight&Page=1&PageSize=6',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setcoinlist', res.data)
            }
        return uesrequire.require(data, callback)
    },


    ///精选页
    //设置精选页商品列表
    setsortgoods: function (context) {
        const data = {
                url: 'WebApi/ShopManager/GoodsList?Page=' + state.page + '&PageSize=5',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setsortgoods', res.data)
            }
        return uesrequire.require(data, callback)
    },
    //设置精选页录播图
    setsortbanner: function (context) {
        const data = {
                url: 'WebApi/ConfigManager/GoodsCarousel',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setsortbanner', res.data)
            }
        return uesrequire.require(data, callback)
    },
    //设置精选页主推商品
    setsorthotgoods: function (context) {
        const data = {
                url: 'WebApi/ShopManager/HotRecommend?Page=1&PageSize=1',
                type: 'get',
            },
            callback = function (res) {
                context.commit('setsorthotgoods', res.data)
            }
        return uesrequire.require(data, callback)
    },
    //精选页轮播图自动播放
    autosortbannerchange: function (context) {
        let t = setTimeout(function () {
            context.commit('changesortbanner', 'Left');
            actions.autobannerchange(context)
        }, 2000);
        context.commit('settimer', t)
    },
    //清楚轮播计时器
    cleartimeout: function (context) {
        clearTimeout(context.state.timer)
    },

    ///商品详情页


    //获取分享图片
    // getshareimg({commit}) {
    //     $.ajax({
    //         url:'WebApi/Public/ShareGoodsPic/'+state.goodsid+'?token='+encodeURIComponent(state.shorttoken),
    //         type:'get',
    //         success:function(data) {
    //             commit('setshareimg', data)
    //         }
    //     })
    // },
    getshareimglist({commit}) {
        $.ajax({
            url: 'WebApi/Public/ShareGoodsPics?goods_id=' + state.goodsid,
            type: 'get',
            success: function (data) {
                commit('setshareimglist', data.data);
            }
        })
    },

    ///地址列表
    //获取地址列表
    getaddlist: function ({commit}) {
        $.ajax({
            url: 'WebApi/UserManager/ReceiveList?token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                commit('getaddlist', data.data)
            }
        })
    },
    //添加新地址
    addnewaddress: function ({commit}) {
        $.ajax({
            url: 'WebApi/UserManager/AddReceiveInfo',
            type: "get",
            data: state.newaddinfo,
            success: function (data) {
                if (data.status == 0) {
                    router.go(-1)
                } else {
                    alert(data.message)
                }

            }
        })
    },
    //添加默认地址
    adddefaultaddress: function ({commit}, e) {
        $.ajax({
            url: 'WebApi/UserManager/AddReceiveInfoAsDefault',
            type: "get",
            data: state.newaddinfo,
            success: function (data) {
                if (data.status == 0) {
                    router.go(-1)
                } else {
                    alert(data.message)
                }
            }
        })
    },
    //删除地址
    deladd: function ({commit}, e) {
        $.ajax({
            url: 'WebApi/UserManager/RemoveReceiveInfo',
            type: "get",
            data: {
                receive_id: e,
                token: state.shorttoken
            },
            success: function (data) {
            }
        })
    },


    ///购物车页
    //获取购物车列表


    ///创建订单页

    //从商品详情创建订单
    makeorderfromgoods: function ({commit, dispatch}) {
        const data = {
                url: 'WebApi/TradeManager/CreateOrderFromGoodsV3?goods_id=' + state.goodsid + '&goods_count=1&token=' + encodeURIComponent(state.shorttoken),
                type: 'get'
            },
            callback = function (res) {
                if (res.status == 0) {
                    commit('setorderid', res.data)
                } else if (res.status == 137) {
                    //commit('seterror',data.message)
                    alert("超过商品最大购买数量限制")
                } else {
                    console.log(res.message)
                }
            }

        return uesrequire.require(data, callback);
    },


    ///订单预览
    //获取订单预览
    setPreOrderList: function ({commit}, id) {
        const data = {
                url: 'WebApi/TradeManager/PreOrderListV3',
                data: {
                    orderlist: id || state.orderid.toString(),
                    token: state.shorttoken
                },
                type: 'get',
            },
            callback = function (res) {
                if (res.status == 0) {
                    commit('setPreOrderList', res.data)
                    commit('setuserdiscount', res.additional_data)
                    commit('setgoldbean', res.add_data.h_money);
                    localStorage.setItem("hMoney", res.add_data.h_money);
                } else {
                    //commit('seterror', data.message)
                }
            };
        return uesrequire.require(data, callback)
    },


    ///订单列表页
    //获取订单列表详情
    getorderlist: function (context) {
        if (state.orderstatus == 0) {
            const data = {
                    url: 'WebApi/TradeManager/OrderList',
                    data: {
                        page: state.orderlistpage[0],
                        pagesize: 5,
                        token: state.shorttoken
                    },
                    type: 'get',
                },
                callback = function (res) {
                    if (data.status == 0) {
                        context.commit('setorderlist', res.data)
                    } else {
                        //context.commit('seterror', res.message)
                        //router.push('/login')
                    }
                }
            return uesrequire.require(data, callback)
        } else {
            const data = {
                    url: 'WebApi/TradeManager/OrderListByStatus',
                    data: {
                        status: state.orderstatus,
                        page: state.orderlistpage[state.orderstatus],
                        pagesize: 5,
                        token: state.shorttoken
                    },
                    type: 'get',
                },
                callback = function (res) {
                    if (data.status == 0) {
                        context.commit('setorderlist', res.data)
                    } else {
                        // context.commit('seterror', res.message)
                        //router.push('/login')
                    }
                }

            return uesrequire.require(data, callback)
        }
    },
    //获取默认收货地址
    getdefaultrecived: function ({commit, dispatch}) {
        $.ajax({
            url: 'WebApi/UserManager/DefaultReceive?token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                if (data.status != 0) {
                    commit('setreceiveinfo', {})
                } else {
                    commit('setreceiveinfo', data.data[0])
                }
            }
        })
    },
    //设置收货地址
    sendrecivedid({commit}) {
        const orderId = [];
        state.PreOrderList.forEach(item => {
            orderId.push(item.id);
        });
        let orserIdNew = orderId.toString();
        const data = {
                url: 'WebApi/TradeManager/FillReceiveInfo?token=' + encodeURIComponent(state.shorttoken) + '&order_id=' + orserIdNew + '&receive_id=' + state.receive_info.id,
                type: "get",
            },
            callback = function () {

            }
        return uesrequire.require(data, callback)
    },


    ///订单详情页
    //获取订单详情
    setorderdetail({commit}, id) {
        $.ajax({
            url: 'WebApi/TradeManager/OrderDetail?token=' + encodeURIComponent(state.shorttoken) + '&order_id=' + id,
            type: "get",
            success: function (data) {
                commit('setorderdetail', data.data)
            }
        })
    },
    //获取物流详情
    settransinfo({commit, dispatch}) {
        const p = new Promise((resolve, reject) => {
            $.ajax({
                url: 'https://e-shop.rrlsz.com.cn/WebApi/Public/TrackInfo?TrackCom=' + state.orderdetail.track_com + '&TrackNum=' + state.orderdetail.track_num,
                //url:'WebApi/Public/TrackInfo?TrackCom=shunfeng&TrackNum=236560068552',
                type: "get",
                success: function (data) {
                    commit('settransinfo', data);
                    resolve()
                }
            })
        })
        return p
    },
    //获取物流字典
    settrackcom({commit}) {
        $.ajax({
            url: 'WebApi/Public/trackcom',
            type: "get",
            success: function (data) {
                commit('settrackcom', data.data)
            }
        })
    },
    //确认收货
    suregetgoods({commit}, id) {
        const data = {
                url: 'WebApi/TradeManager/SettleOrder?order_id=' + id + '&token=' + encodeURIComponent(state.shorttoken),
                type: 'get',
            },
            callback = function (res) {
                if (res.status != 0) {
                    alert(res.message)
                }
            }
        return uesrequire.require(data, callback)
    },
    //取消订单
    cancelorder({commit}, id) {
        $.ajax({
            url: 'WebApi/TradeManager/CancelOrder?order_id=' + id + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                if (data.status == 0) {
                } else {
                    alert(data.message)
                    location.reload()
                }
                //commit('settrackcom',data.data)
            },
            error: function (data) {
                alert(data.message)
            }
        })
    },
    //订单退换
    RefundOrder({commit}, id) {
        $.ajax({
            url: 'WebApi/TradeManager/RefundOrder?order_id=' + id + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                alert(data.message)
                location.reload()
            }
        })
    },


    ///用户页
    //获取用户详情
    getuserinfo({commit}) {
        $.ajax({
            url: 'WebApi/UserManager/UserInfo?token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                if (data.status == 0) {
                    commit('userinfo', data.data[0])
                    sessionStorage.setItem('myid', data.data[0].username)
                    commit('setshareid', data.data[0].username)
                } else {
                    commit('setlongtoken', null)
                    localStorage.removeItem('longtoken')
                }
            },
            error: function () {
                commit('setlongtoken', null)
                localStorage.removeItem('longtoken')
            }
        })
    },


    ///收藏列表
    //获取收藏列表
    getgoodscolist({commit}) {
        $.ajax({
            url: 'WebApi/UserManager/FavouriteGoodsList?token=' + encodeURIComponent(state.shorttoken) + '&Page=' + state.goodscopage + '&PageSize=10',
            type: "get",
            success: function (data) {
                commit('setgoodscolist', data.data)
            }
        })
    },


    ///钱包页
    //获取钱包信息


    ///设置页
    //修改用户信息
    setuserinfo({commit}, {nickname, sex}) {
        $.ajax({
            url: 'WebApi/UserManager/SetUserInfo?nick_name=' + nickname + '&token=' + encodeURIComponent(state.shorttoken) + '&sex=' + sex,
            type: "get",
            success: function (data) {
            }
        })
    },


    ///微信分享及支付
    //获取微信分享config
    getwxshare({commit}, url) {
        const data = {
                url: 'WebApi/Public/WxJsApiConfig?url=' + url,
                type: "get"
            },
            callback = function (res) {
                commit('SET_WXSHARE', res)
            }
        return uesrequire.require(data, callback)
    },
    //请求微信支付
    ApplyWxJsPay({commit}, dis) {
        $.ajax({
            url: 'WebApi/TradeManager/ApplyWxJsPay?orderlist=' + state.orderid.toString() + '&discount=' + dis + '&token=' + state.shorttoken,
            type: "get",
            success: function (data) {
                //commit('setwxshare',data)
            }
        })
    },


    ///商品分类页
    //获取商品分类
    getsortlist({commit}) {
        $.ajax({
            url: '/SWebApi/ShopManager/GoodsTypeList.json',
            type: 'get',
            success: function (data) {
                if (data.status == 0) {
                    commit('setsortlist', data.data)
                }
            }
        })
    },
    //通过商品分类id获取商品列表
    getlistofgoods({commit}, id) {
        if (state.wjIsPageListFlag) {
            return;
        }
        $.ajax({
            url: '/SWebApi/ShopManager/GoodsListByType/' + id + '/' + state.listofgoodspage + '.json?' + '&PageSize=6',
            type: 'get',
            success: function (data) {
                if(data.status == 115){
                    state.wjIsPageListFlag = true;
                    return;
                }
                if (data.status == 0) {
                    var data = data.data;
                    if (data.length < 6) {
                        state.wjIsPageListFlag = true;
                        // return;
                    }
                    commit('setlistofgoods', data)
                }
            }
        })
    },


    ///地推注册页
    registfrompush({commit}) {
        //硕禾
        $.ajax({
            url: 'WebApi/UserManager/ResgistFromPush?username=' + state.username + '&smscode=' + state.userpassword + '&spreader_code=shuohe' + '&spreader=' + state.rec,
            type: 'get',
            success: function (data) {
                if (data.status == 0) {
                    commit('setlongtoken', data.additional_data)
                    commit('setshorttoken', data.data)
                    localStorage.setItem('longtoken', state.longtoken)
                    var expiresDate = new Date();
                    expiresDate.setTime(expiresDate.getTime() + (24 * 60 * 60 * 1000));
                    document.cookie = 'shorttoken' + "=" + escape(state.shorttoken) + ";expires=" + expiresDate.toGMTString();
                    alert('注册成功')
                    router.push('/index')
                } else {
                    //commit('seterror', data.message)
                    alert(data.message)
                }

            },
            error: function (data) {
                alert(data.message)
            }
        })
    },


    ///搜索页
    search({commit}, name) {
        $.ajax({
            url: 'WebApi/ShopManager/GoodsListByKeyWord?KeyWord=' + name + '&Page=' + state.searchpage + '&PageSize=5',
            type: 'get',
            success: function (data) {
                commit('setsearchlist', data.data)
            }
        })
    },


    ///金币页
    getcoinpagelist({commit}, [page, type]) {
        $.ajax({
            url: 'WebApi/ShopManager/ExChangeAreaGoodsList?Page=' + page + '&PageSize=6&OrderField=' + type,
            type: 'get',
            success: function (data) {
                commit('setcoinpagelist', data.data)
            }
        })
    },

    ///新登录页


    getsharetext({commit}) {
        $.ajax({
            url: 'WebApi/Public/ShareGoodsText/' + state.goodsid,
            type: 'get',
            success: function (data) {
                commit('setsharetext', data.data);
            }
        })
    },


    // 获取分类id
    getgameandcoinid({commit}) {
        $.ajax({
            url: 'WebApi/ConfigManager/GameArea',
            type: 'get',
            success: function (data) {
                const gameid = data.data;
                $.ajax({
                    url: 'WebApi/ConfigManager/GoldenCoinArea',
                    type: 'get',
                    success: function (data) {
                        const coinid = data.data
                        commit('SETGAMEANDCOIN', [gameid, coinid])
                    }
                })
            }
        });
    },
    //获取用户余额
    getuseraccount({commit}) {
        $.ajax({
            url: '/WebApi/UserManager/UserAccountInfo?token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                console.log(data);
                commit('setuseraccount', data.data[0])
            }
        })
    },
    //获取银行卡列表
    getmycard({commit}) {
        $.ajax({
            url: '/WebApi/UserManager/BankCardList?token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                console.log("1111")
                console.log(data)
                commit('setmycard', data.data)
            }
        })
    },
    //获取银行列表
    getbank({commit}) {
        $.ajax({
            url: '/WebApi/Public/bankdic',
            type: "get",
            success: function (data) {
                commit('setbank', data.data)
            }
        })
    },
    //添加银行卡
    createbankcard({commit}, data) {
        $.ajax({
            url: '/WebApi/UserManager/AddBankCard?bank_en=' + data.bank + '&card_holder=' + data.name + '&card_no=' + data.card + '&sms_code=' + data.sms + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                if (data.status == 0) {
                    alert("银行卡添加成功");
                    location.reload()
                } else {
                    alert("银行卡添加失败")
                }
            }
        })
    },
    //移除银行卡
    removebankcard({commit}, data) {
        $.ajax({
            url: '/WebApi/UserManager/RemoveBankCard?card_id=' + data.cardid + '&sms_code=' + data.sms + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                if (data.status == 0) {
                    alert("银行卡移除成功");
                    location.reload()
                } else {
                    alert("银行卡移除失败")
                }
            }
        })
    },
    //提现申请
    getcash({commit}, data) {
        let p = new Promise(function (resolve) {
            $.ajax({
                url: '/WebApi/UserManager/MakeCashApply?apply_money=' + data.money + '&type=' + data.type + '&card_id=' + data.cardid + '&sms_code=' + data.sms + '&token=' + encodeURIComponent(state.shorttoken),
                type: "get",
                success: function (data) {
                    if (data.status == 0) {
                        resolve()
                    }
                    else {
                        alert(data.message)
                    }
                }
            })
        })
        return p
    },
    //提现记录及账单列表
    getapply({commit}, data) {
        $.ajax({
            url: '/WebApi/UserManager/CashApplyList?Page=' + data.page + '&PageSize=' + data.size + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                console.log(data);
                commit('setapply', data.data)
            }
        })
    },
    getbilllist({commit}, data) {
        $.ajax({
            url: '/WebApi/UserManager/UserMoneyRecord?Page=' + data.page + '&PageSize=' + data.size + '&token=' + encodeURIComponent(state.shorttoken),
            type: "get",
            success: function (data) {
                console.log(data);
                commit('setbilllist', data.data)
            }
        })
    },
    //发送验证码
    sendsms: function ({commit}, phone) {
        $.ajax({
            url: 'WebApi/AuthManager/SendSMS?mobile=' + phone,
            type: "get",
            success: function (data) {
                //console.log(data)
            }
        })
    },
}
const store = new Vuex.Store({
    state,
    mutations,
    actions,
    modules: {
        login: Login,
        my: My,
        wallet: Wallet,
        rec: Rec,
        shopcart: Shopcart,
        orderdetail: Orderdetail,
        gamearea: Gamearea,
        goodsdetail: Goodsdetail,
        makeorder: Makeorder,
        mycard: Mycard
    }
})

export default store