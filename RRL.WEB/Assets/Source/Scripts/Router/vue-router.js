import Vue from 'vue'
import VueRouter from 'vue-router'
import vuex from '../Store/store'
import $ from 'jquery'

Vue.use(VueRouter);



/*import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
Vue.use(ElementUI);*/

import index from '../Components/index';
import goodssort from '../Components/goods-sort';
import gamehall from  '../Components/gamehall.vue';
import shopcart from '../Components/shopcart';
import my from '../Components/my';
//import card from '../Components/card';
import goodsdetail from '../Components/goods-detail';
import order from '../Components/order';
import orderdetail from '../Components/orderdetail';
import makesureorder from '../Components/makesureorder';
import login from '../Components/login';
import BindAccount from '../Components/BindAccount';
import store from '../Store/store';
import choseadd from '../Components/chose-address';
import editadd from '../Components/editadd';
import wallet from '../Components/wallet';
import regist from '../Components/regist';
import pay from '../Components/pay';
// import hhhh from '../Components/hhhh';
// import gamerule from '../Components/gamerule';
// import ffff from '../Components/ffff';
import goodscollection from '../Components/goodscollection';
// import downloadapp from'../Components/downloadapp';
import myset from '../Components/set';
import setinfo from '../Components/setuserinfo';
import sortofgoods from '../Components/sortOfGoods';
// import aboutus from '../Components/aboutus';
import goodslistthird from '../Components/goods-list-3rd';
import articledetail from '../Components/article-detail';
//import localregist from '../Components/local-registe';
import searchpage from '../Components/searchpage';
import shopdetail from '../Components/shopdetail';
import walletdetail from '../Components/walletdetail';
import coinslist from '../Components/coinslist';
import coinspage from '../Components/coinspage';
import coininfo from '../Components/coininfo';
import coinsdetail from '../Components/coinsdetail';
import coingoods from '../Components/coingoods';
// import login_new from '../Components/login_new';
import sharepage from '../Components/sharepage';
import makeshareimg from '../Components/makeshareimg';
import payway from '../Components/payway';
import payqrcode from '../Components/payqrcode';
import myrecreward from '../Components/my-rec-reward';
import myrecorder from '../Components/my-rec-order';
import myrecpeople from '../Components/my-rec-people';
//import hisbind from '../Components/hisbind';
import idcard from '../Components/idcard';
import gamearea from '../Components/gamearea';
import upaccount from '../Components/up-account';
import Sureupaccount from '../Components/sure-up-account';
import myapply from '../Components/my/my-apply';
import mybill from '../Components/my/my-bill';
import mycardlist from '../Components/my/mycard-list';
import mycard from '../Components/mycard';
import transdetail from '../Components/transdetail';

import secondhandarea from '../Components/second-hand-area';
import myrecpeopletwo from '../Components/my-rec-people-two';
import myrecpeoplethree from '../Components/my-rec-people-three';
import recommendrewardranking from '../Components/recommend-ranking/recommend-reward-ranking';
import recommendrewardnumber from '../Components/recommend-ranking/recommend-reward-number';
import arealist from '../Components/arealist';
import mymsgpush from '../Components/mymsgpush';
import pocketcard from '../Components/pocket-card/pocket-card';
import bindnewcards from '../Components/pocket-card/bind-new-cards';

import gamepop from '../Components/alertbox/GamePop';
import agent from '../Components/agent-list/agent';
import agentdetail from '../Components/agent-list/agentdetail';

import getredpacket from '../Components/get-redpacket';
import shareredpacket from '../Components/share-redpacket';

import expect from '../Components/expect';
import Logininsuccess from '../Components/LoginInSuccess';



const routes = [
  {path: '/', redirect: '/index'},
  {path: '/index', component: index, name: 'index', linkActiveClass: "cur", meta: {keepAlive: true, Title: '首页'}},
  {path: '/articledetail/:id', component: articledetail, name: 'articledetail', meta: {keepAlive: true, Title: '文章详情页'}},
  {path: '/goodssort', component: goodssort, name: 'goodssort', meta: {keepAlive: true, Title: '商品分类'}},
  {path: '/gamehall', component: gamehall, name: 'gamehall', meta: { keepAlive: false, Title: '签到'}},
  {path: '/shopcart', component: shopcart, meta: {keepAlive: false, Title: '购物车'}},
  {path: '/my', component: my, meta: {keepAlive: false, Title: '个人中心'}},
//{path: '/card', component: card, meta: {keepAlive: true, Title: '卡券中心'}},
  {path: '/gamepop',component:gamepop,meta:{keepAlive:true,Title:'游戏弹框'}},
  {path: '/agent',component:agent,meta:{keepAlive:false,Title:'代理信息'}},
  {path: '/agentdetail',component:agentdetail,name: 'agentdetail',meta: {keepAlive: false, Title: '代理用户详情'}},
  {path:'/getredpacket',component:getredpacket,name:'getredpacket',meta:{keepAlive: false, Title: '天天领红包'}},
  {path:'/shareredpacket',component:shareredpacket,name:'shareredpacket',meta:{keepAlive:false,Title:'分享天天领红包'}},
  {path:'/expect',component:expect,name:'expect',meta:{keepAlive:true,Title:'敬请期待'}},
  {path:'/Logininsuccess',component:Logininsuccess,name:'Logininsuccess',meta:{keepAlive:true,Title:'登录中转页'}},
  {
    path: '/goodsdetail/:goodsid/:username',
    component: goodsdetail,
    name: 'goodsdetail',
    meta: {keepAlive: false, Title: '商品详情'}
  },
  {
    path: '/order/:status',
    name: 'order',
    component: order,
    meta: {requiresAuth: false, keepAlive: false, Title: '我的订单'}
  },
  {path: '/orderdetail/:orderid', name: 'orderdetail', component: orderdetail, meta: {keepAlive: false, Title: '订单详情'}},
  {
    path: '/makesureorder/:from/:orderList',
    component: makesureorder,
    name: 'makesureorder',
    meta: {keepAlive: false, Title: '订单确认'}
  },
  {path: '/BindAccount', component: BindAccount, name: 'BindAccount', meta: {keepAlive: false, Title: '绑定账号'}},
  {path: '/login', component: login, name: 'login', meta: {keepAlive: false, Title: '登录'}},
  {path: '/choseadd', component: choseadd, meta: {keepAlive: false, Title: '选择地址'}},
  {path: '/editadd', component: editadd, meta: {keepAlive: false, Title: '编辑地址'}},
  {path: '/wallet', component: wallet, meta: {keepAlive: false, Title: '钱包'}},
  {path: '/regist/:username', component: regist, meta: {keepAlive: false, Title: '注册'}},
  {path: '/pay', component: pay, meta: {keepAlive: false, Title: '付款'}},
  //{path:'/hhhh/:username',component:hhhh},
  // {path:'/gamerule',component:gamerule,  meta: { keepAlive: false }},
  // {path:'/ffff/:from',component:ffff,  meta: { keepAlive: false }},
  {path: '/goodscollection', component: goodscollection, meta: {requiresAuth: true, keepAlive: false, Title: '商品收藏'}},
  // {path:'/downloadapp',component:downloadapp,  meta: { keepAlive: false }},
  {path: '/myset', component: myset, meta: {keepAlive: false, Title: '我的设置'}},
  {path: '/setinfo', component: setinfo, meta: {keepAlive: false, Title: '设置信息'}},
  // {path:'/aboutus',component:aboutus,  meta: { keepAlive: false }},
  {path: '/sortofgoods', component: sortofgoods, meta: {keepAlive: true, Title: '商品分类'}},
  {
    path: '/goodslistthird/:id',
    component: goodslistthird,
    name: 'goodslistthird',
    meta: {keepAlive: true, Title: '商品分类'}
  },
  //{path:'/localregist/:username',component:localregist,  meta: { keepAlive: false }},
  {path: '/searchpage', component: searchpage, name: 'searchpage', meta: {keepAlive: true, Title: '搜索列表'}},
  {path: '/shopdetail', component: shopdetail, name: 'shopdetail', meta: {keepAlive: false, Title: '商家详情'}},
  {path: '/walletdetail', component: walletdetail, meta: {keepAlive: false, Title: '钱包详情'}},
  {path: '/coinslist', component: coinslist, meta: {keepAlive: true, requiresAuth: true, Title: '金币商品'}},
  {path: '/coinspage', component: coinspage, meta: {keepAlive: true, Title: '金币专区'}},
  {path: '/coininfo', component: coininfo, meta: {requiresAuth: true, Title: '金币规则'}},
  {path: '/coinsdetail', component: coinsdetail, meta: {keepAlive: false, Title: '金币详情'}},
  {path: '/coingoods/:goodsid', component: coingoods, meta: {keepAlive: false, Title: '金币商品'}},
  // {path:'/login_new',component:login_new,name:login_new,  meta: { keepAlive: false }},
  {path: '/sharepage', component: sharepage, meta: {keepAlive: false, Title: '商品分享'}},
  {path: '/makeshareimg/:sta', component: makeshareimg, meta: {keepAlive: false, Title: '商品分享'}},
  {path: '/payway/', component: payway, meta: {keepAlive: false, Title: '付款方式'}},
  {path: '/payqrcode/', component: payqrcode, meta: {keepAlive: false, Title: '二维码付款'}},
  {path: '/myrecreward/', component: myrecreward, meta: {keepAlive: false, requiresAuth: true, Title: '我的推荐奖励'}},
  {path: '/myrecorder/', component: myrecorder, meta: {keepAlive: false, requiresAuth: true, Title: '推荐订单'}},
  {path: '/myrecpeople/', component: myrecpeople, meta: {keepAlive: false, requiresAuth: true, Title: '我的推荐用户'}},
  //{path:'/hisbind',component:hisbind, meta:{ keepAlive: false, requiresAuth: true }},
  //{path:'/shareqrcode',component:shareqrcode, meta:{ keepAlive: false}},
  {path: '/idcard', component: idcard, meta: {keepAlive: false, Title: '清关信息'}},
  {path: '/gameareaV2', component: gamearea, meta: {keepAlive: false, Title: '游戏专区'}},
  {path: '/upaccount', component: upaccount, name: 'upaccount', meta: {keepAlive: true, Title: "余额分类"}},
  {path: '/Sureupaccount', component: Sureupaccount, name: 'Sureupaccount'},
  {path: '/myapply', component: myapply, name: 'myapply'},
  {path: '/mybill', component: mybill, name: 'mybill'},
  {path: '/mycardlist', component: mycardlist, name: 'mycardlist'},
  {path: '/mycard', component: mycard, meta: {keepAlive: true, Title: '我的卡券'}},
  {path: '/transdetail', component: transdetail, name: 'transdetail'},
  {path: '/secondhandarea', component: secondhandarea, name: 'secondhandarea', meta: {keepAlive: false, Title: '二手交易'}},
  {path: '/myrecpeopletwo', component: myrecpeopletwo, name: 'myrecpeopletwo', meta: {keepAlive: false, Title: '查看下级'}},
  {path: '/myrecpeoplethree', component: myrecpeoplethree, name: 'myrecpeoplethree', meta: {keepAlive: false, Title: '查看下级'}},
  {path: '/recommendrewardranking', component: recommendrewardranking, name: 'recommendrewardranking', meta: {keepAlive: false, Title: '本月推荐奖励排名'}},
  {path: '/recommendrewardnumber', component: recommendrewardnumber, name: 'recommendrewardnumber', meta: {keepAlive: false, Title: '本月推荐人数排名'}},
  {path: '/arealist', component: arealist, name: 'arealist', meta: {keepAlive: false, Title: '编辑收件地址'}},
  {path: '/mymsgpush', component: mymsgpush, name: 'mymsgpush', meta: {keepAlive: false, Title: '我的消息'}},
    {path: '/pocketcard', component: pocketcard, name: 'pocketcard', meta: {keepAlive: false, Title: '兜兜卡'}},
    {path: '/bindnewcards', component: bindnewcards, name: 'bindnewcards', meta: {keepAlive: true, Title: '绑定新卡'}},
  {path: '**', redirect: '/index', component: index}
]

const router = new VueRouter({
  mode: 'hash',    //路由的模式
  routes
});


router.beforeEach((to, from, next) => {
  //需要登录的页面判定
  //const top=sessionStorage.getItem('top')
  const title = to.meta.Title;
  vuex.commit('SET_VIEWTITLE', title)
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (store.state.shorttoken.length == 0) {
      next({
        path: '/login',
        query: {redirect: to.fullPath}
      })
    } else {
      if (to.meta.keepAlive === true) {
        //document.documentElement&&document.documentElement.scrollTop? document.documentElement.scrollTop=top:document.body.scrollTop=top;
      } else {
        //document.documentElement&&document.documentElement.scrollTop? document.documentElement.scrollTop=0:document.body.scrollTop=0;
      }
      next()
    }
  } else {
    if (to.meta.keepAlive === true) {
      //document.documentElement&&document.documentElement.scrollTop? document.documentElement.scrollTop=top:document.body.scrollTop=top;
    } else {
      //document.documentElement&&document.documentElement.scrollTop? document.documentElement.scrollTop=0:document.body.scrollTop=0;
    }
    next()
  }
})

export default router