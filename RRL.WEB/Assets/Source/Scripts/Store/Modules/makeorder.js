import $ from 'jquery';

import AskPromise from './promise.js';

let uesrequire = new AskPromise()

const makeorder = {
    state: {
        list:[]
    },
    mutations: {
        SETLIST(state, data) {
            state.list = data
        }
    },
    actions: {
        getlist({commit, rootState}) {
    		let  arrId = [];
            rootState.PreOrderList.forEach(item => {
                arrId.push(item.id);
            });
            let orderlist=  arrId.length > 1 ? arrId.join(",") : arrId.join("");
            const data = {
                url:'WebApi/TradeManager/GetAvailableCoupons',
                type:'get',
                data:{
                    'order_list' :orderlist,// (rootState.orderid).toString(),
                    'token' : rootState.shorttoken,
                    //'token' : 'XoW68IidGJVf5Z8iAVC8BBXuFfAzaUGIcGs6aMVs0Jk806NGy72KG87yyHfVXDlIqXIEGANG50JkJZkRQDENGHAO',
                    'page_index' : 1,
                    'page_size' : 20
                },
            },
            callback = function(res) {
                commit('SETLIST', res.data)
            };
            return uesrequire.require(data,callback)
        }, 
        sendcoupons({commit, rootState},id) {
            /*const data = {
                url:'WebApi/TradeManager/ApplyCouponsPay?order_id= '+ (rootState.orderid).toString()+'&coupons_id='+id+'&token='+encodeURIComponent(rootState.shorttoken),
                type:'get',
            },
            callback= function(res) {

            };
            return uesrequire.require(data,callback)
            */
        }
    }
}

export default makeorder