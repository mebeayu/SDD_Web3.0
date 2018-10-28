import $ from 'jquery';
import router from '../../Router/vue-router.js';
import AskPromise from './promise.js'


let uesrequire = new AskPromise()


const shopcart = {
    state:{
        shopcartlist:[]
    },

    mutations: {
        SETSHOPCARTLIST(state, data) {
            state.shopcartlist = data
            console.log(state.shopcartlist)
        }
    },
    actions:{
        setshopcartlist: function ({ commit, rootState }) {
            $.ajax({
                url: 'WebApi/TradeManager/CartList?token=' + encodeURIComponent(rootState.shorttoken),
                //url:'WebApi/TradeManager/CartList?token='+state.shorttoken,
                type: "get",
                success: function (data) {
                    console.log(data)
                    let newdata = []
                    try {
                        for (var i = 0; i < data.data.length; i++) {
                            let obj = data.data[i]
                            obj.toggle = false
                            newdata = newdata.concat(obj)
                        }
                    } catch(e){
    
                    }
                    
                    commit('SETSHOPCARTLIST', newdata)
                }
            })
        },
        //添加商品至购物车
        addgoods: function ({rootState}, {num, id}) {
            $.ajax({
                url: 'WebApi/TradeManager/AddGoodsToCart',
                type: "get",
                data: {
                    goods_id: id,
                    goods_count: num,
                    token: rootState.shorttoken
                },
                success: function (data) {
                    console.log(data)
                }
            })
        },
        //删除商品从购物车
        delgoods: function ({rootState}, id) {
            $.ajax({
                url: 'WebApi/TradeManager/RemoveGoodsInCart',
                type: "get",
                data: {
                    cart_id: id,
                    token: rootState.shorttoken
                },
                success: function (data) {
                    console.log(data)
                }
            })
        },
            //从购物车创建订单
        makeorderfromcart: function ({ commit, dispatch, rootState },list) {
            const data = {
                url: 'WebApi/TradeManager/CreateOrderFromCartV3',
                data: {
                    cartlist: list.toString(),
                    token: rootState.shorttoken
                },
                type: 'get',
            },
            callback = function(res) {
                if (res.status == 0) {
                    commit('setorderid', res.data)
                    //dispatch('setPreOrderList')
                    commit('clearcartlist')
                } else {
                    // commit('seterror', res.message)
                    alert(res.message)
                }
            }
            return uesrequire.require(data, callback);
        }
    }
}

export default shopcart