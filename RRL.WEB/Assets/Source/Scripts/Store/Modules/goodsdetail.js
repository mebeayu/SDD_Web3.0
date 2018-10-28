import $ from 'jquery';

import AskPromise from './promise.js';

let uesrequire = new AskPromise()


const goodsdetail = {
    state:{
        goodsbanner:[],
        goodsdetail:{},
        goodsfav: false,
        defaultshare:true,
        mutiple:true,
    },
    mutations:{
        SET_GOODSDETAIL(state, data) {
            try {
                state.goodsbanner = data.additional_data
                state.goodsdetail = data.data[0]
            } catch (e) {
               
            }
            console.log(state.goodsdetail)
        },
        CHANGEGOODSFAV(state, data) {
            state.goodsfav = data
        },
        //是否用默认分享
        SET_DEFAULTSHARE(state, data){
            state.defaultshare = data;
        },
        //是否有多图
        SET_MUTIPLE(state,data){
            state.mutiple = data
        },
    },
    actions:{
        //获取商品详情
        setgoodsdetail: function (context) {
            const data = {            
                url: 'WebApi/ShopManager/GoodsDetail/' + context.rootState.goodsid + '?token=' + encodeURIComponent(context.rootState.shorttoken),
                type: 'get',
            },
            callback = function(res) {
                context.commit('SET_GOODSDETAIL', res)
                context.commit('CHANGEGOODSFAV', res.data[0].goodsfav)
            }
            return uesrequire.require(data,callback)
        },
        //添加商品收藏
        addgoodsfav: function (context) {
            const data = {            
                url: 'WebApi/UserManager/AddFavouriteGoods?goods_id=' + context.rootState.goodsid + '&token=' + encodeURIComponent(context.rootState.shorttoken),
                type: 'get',
            },
            callback = function(res) {
                
            }
            return uesrequire.require(data,callback)
        },
        //移除商品收藏
        removegoodsfav: function (context) {
            $.ajax({
                url: 'WebApi/UserManager/RemoveFavouriteGoods?goods_id=' + context.rootState.goodsid + '&token=' + encodeURIComponent(context.rootState.shorttoken),
                type: "get",
                success: function (data) {
                }
            })
        },
        //判断是否有分享图片
        jugimg(context) {
            $.ajax({
                url:'WebApi/ShopManager/GoodsShareInfo/'+context.rootState.goodsid,
                type:'get',
                success:function(data){
                    if(data.status == 0){
                        context.commit('SET_DEFAULTSHARE', true)
                    }else {
                        context.commit('SET_DEFAULTSHARE', false)
                    }
                    try{
                        context.commit('SET_MUTIPLE',data.additional_data['mutiple'])
                    } catch(e) {
                        // commit('setmutiple',false)
                    }
                }
            })
        },
    }
}

export default goodsdetail