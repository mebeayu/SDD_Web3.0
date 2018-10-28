import $ from 'jquery';

const wallet = {
    state: {
        UserAccountInfo: {},
        UserMoneyRecord: [],
        tips:true,
        walletlist: [],
        coinlist: [],
        xianjinlist: [],
        xiaofeilist: [],
        tuijianlist: [],
    },
    mutations: {
        SET_ACCOUNTINFO( state, data ) {
            state.UserAccountInfo = data
        },
        SET_TIPS( state, data ) {
            state.tips = data
        },
        SET_MONEYRECORD( state, data ){
            state.UserMoneyRecord = data;
        },
        SET_XIANJIN(state, data) {
            state.xianjinlist = data;
        },
        SET_XIAOFEI(state, data) {
            state.xiaofeilist = data;
        },
        SET_TUIJIAN(state, data) {
            state.tuijianlist = data;
        }
    },
    getters:{
        mywallet(state ) {
            const all = state.UserAccountInfo.r_money+state.UserAccountInfo.y_money+state.UserAccountInfo.x_money +state.UserAccountInfo.y_money_frz || 0;
            return parseFloat(all.toFixed(2));
        },
        mycoins( state) {
            const coins = state.UserAccountInfo.plate_to_return_money + state.UserAccountInfo.ex_plate_to_return_money || 0 ;
            return  parseFloat(coins.toFixed(2));
        },
        unique( state ) {

        }
    },
    actions: {
        getUserAccountInfo({ commit , rootState}) {
        	console.log(rootState.shorttoken)
            $.ajax({
                url: 'WebApi/UserManager/UserAccountInfo',
                type: "post",
                data: {
                    "token": rootState.shorttoken
                },
                success: function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_ACCOUNTINFO', data.data)
                        commit('SET_TIPS', data.data.if_info_has_read)
                    } else {
                        alert('尚未登录或token验证失败，请重新登录2')
                    }
                },
                error: function () {
                    alert('尚未登录或token验证失败，请重新登录1')
                }
            })
        },
        //获取钱包交易明细信息
        getUserMoneyRecord({ commit , rootState}) {
            $.ajax({
                url: 'WebApi/UserManager/UserIncomeRecord',
                type: "post",
                data: {
                    "token": rootState.shorttoken,
                    "Page": 1,
                    "PageSize": 30
                },
                success: function (data) {
                    console.log(data)
                    commit('SET_MONEYRECORD', data.data)
                }
            })
        },
        getxianjin({state, commit, rootState} , page) {
            $.ajax({
                url: 'WebApi/UserManager/UserXMoneyRecord',
                type: 'post',
                data:{
                    "token": rootState.shorttoken,
                    "Page": page,
                    "PageSize": 10
                },
                success: function (data) {
                    let arr = state.xianjinlist.concat(data.data)
                    let hash = {};
                    arr = arr.reduce(function (item, next) {
                    hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                        return item
                    }, [])
                    commit('SET_XIANJIN', arr)
                }
            })
        },
        getxiaofei({state, commit, rootState}, page) {
            $.ajax({
                 url: 'WebApi/UserManager/UserRMoneyRecord',
                type: 'post',
                data:{
                    "token": rootState.shorttoken,
                    "Page": page,
                    "PageSize": 10
                },
                success: function (data) {
                    let arr = state.xiaofeilist.concat(data.data)
                    let hash = {};
                    arr = arr.reduce(function (item, next) {
                    hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                        return item
                    }, [])
                    commit('SET_XIAOFEI', arr)
                }
            })
        },
        gettuijian({state, commit, rootState} , page) {
            $.ajax({
                url: 'WebApi/UserManager/UserYMoneyRecord',
                type: 'post',
                data:{
                    "token": rootState.shorttoken,
                    "Page": page,
                    "PageSize": 10
                },
                success: function (data) {
                    let arr = state.tuijianlist.concat(data.data)
                    let hash = {};
                    arr = arr.reduce(function (item, next) {
                    hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                        return item
                    }, [])
                    commit('SET_TUIJIAN', arr)                }
            })
        },
    }
}

export default wallet