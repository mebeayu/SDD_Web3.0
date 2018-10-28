import Vue from 'vue';
import VueRouter from 'vue-router';
import Vuex from 'vuex';
import app from './app.vue';
import router from './Router/vue-router.js';
import store from './Store/store.js';
import AlloyFinger from 'alloyfinger';
import AlloyFingerVue from './alloy_finger.vue.js';
import VueQrcode from '@xkeshi/vue-qrcode';
import Clipboard from 'clipboard';


var fource = 1;

Vue.use(VueRouter)
Vue.use(Vuex)
Vue.prototype.bus=new Vue();

//Vue.use(alloyfinger);
//Vue.use(alloyfinger)
//Vue.use(AlloyFingerVue)
Vue.use(AlloyFingerVue, {
  AlloyFinger
});

Vue.component('qrcode', VueQrcode);


/*const index = resolve => require(['./Components/index.vue'], resolve);
const goodssort = resolve => require(['./Components/goods-sort.vue'], resolve);
const shopcart = resolve => require(['./Components/shopcart.vue'], resolve);
const my = resolve => require(['./Components/my.vue'], resolve);*/
// const router = new VueRouter({
//   mode: 'hash',    //路由的模式
//   routes
// });

// 过滤搜索页和商品详情页的价格和金豆 例如：393333 => 39万3333
Vue.filter("formatBeansPrice", num => {
  if(num<0){
    num = Math.abs(num);
  }
  if(num>=10000){
    return (num/10000).toFixed(2)+"万";
  }
  return num;
});

const abc = new Vue({
  el: '#application',
  router,
  store,
  render: h => h(app),
});