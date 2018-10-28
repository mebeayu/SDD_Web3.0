import $ from 'jquery';

const rec = {
    state: {
        MonthUserRecData: {},
        MonthRecList: [],
        thisMonthRec: {},
        MonthOrderRecData: {},
        MonthOrderList: [],
        thisMonthOrder: {},
        MonthRewardRecData: {},
        MonthRewardList: [],
        thisMonthReward: {},
        // consumedList: [],
        // not_consumedList : []
    },
    getters: {
        //会员分类
        consumedlist(state) {
            const consumedList = [];
            const not_consumedList = [];
            for (let i = 0, len = state.MonthRecList.length; i < len; i++) {
                if (state.MonthRecList[i].total_cash > 0) {
                    consumedList.push(state.MonthRecList[i])
                } else {
                    not_consumedList.push(state.MonthRecList[i])
                }
            }
            return {
                consumedList,
                not_consumedList
            }
        },
        //订单分类
        orderlist(state) {
            const consumedList = [];
            const not_consumedList = [];
            for (let i = 0, len = state.MonthOrderList.length; i < len; i++) {
                if (state.MonthOrderList[i].status == 3) {
                    consumedList.push(state.MonthOrderList[i])
                } else {
                    not_consumedList.push(state.MonthOrderList[i])
                }
            }
            return {
                consumedList,
                not_consumedList
            }
        }
    },
    mutations: {
        //设置各月推荐人
        SET_MONTHUSERRECDATA(state, data) {
            state.MonthUserRecData = data;
            state.MonthRecList = data.list;
        },
        SET_THISMONTHREC(state, data) {
            state.thisMonthRec = data
        },

        //设置各月推荐订单
        SET_MONTHORDERRECDATA(state, data) {
            state.MonthOrderRecData = data;
            state.MonthOrderList = data.list;
            console.log(state.MonthOrderRecData, state.MonthOrderList)
        },
        SET_THISMONTHORDER(state, data) {
            state.thisMonthOrder = data;
            console.log(state.thisMonthOrder)
        },

        //设置各月推荐奖励金
        SET_MONTHREWARDRECDATA(state, data) {
            state.MonthRewardRecData = data;
            state.MonthRewardList = data.list;
            console.log(state.MonthRewardRecData, state.MonthRewardList)
        },
        SET_THISMONTHREWARD(state, data) {
            state.thisMonthReward = data;
            console.log(state.thisMonthReward)
        }
    },
    actions: {
        //获取推荐人
        getMonthUserRec({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandUserData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    //"token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+Wovabzqrl0yNYelYqirO9erWeqqKr8M1SEI=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_MONTHUSERRECDATA', data.data)
                    }
                }
            })
        },
        getThisMonthRec({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandUserData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    // "token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+Wovabzqrl0yNYelYqirO9erWeqqKr8M1SEI=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_THISMONTHREC', data.data)
                    }
                }
            })
        },

        //获取推荐订单
        getMonthOrderData({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandOrderData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    // "token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+WovJsMAcadfx7N2yGxno5gZ3BfBQmJo9Qa4=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_MONTHORDERRECDATA', data.data)
                    }
                }
            })
        },
        getThisMonthOrder({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandOrderData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    // "token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+WovJsMAcadfx7N2yGxno5gZ3BfBQmJo9Qa4=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_THISMONTHORDER', data.data)
                    }
                }
            })
        },

        //获取推荐奖励
        getMonthRewardData({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandMoneyData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    // "token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+WovJsMAcadfx7N2yGxno5gZ3BfBQmJo9Qa4=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_MONTHREWARDRECDATA', data.data)
                    }
                }
            })
        },
        getThisMonthReward({ commit, rootState }, month) {
            $.ajax({
                'url': 'WebApi/UserManager/MonthRecommandMoneyData',
                'data': {
                    "month": month,
                    "token": rootState.shorttoken,
                    // "token": 'XoW68IidGJXEIlTEXw7hi2sdWarNT5DTLjqVswx+WovJsMAcadfx7N2yGxno5gZ3BfBQmJo9Qa4=',
                    "Page": 1,
                    "PageSize": 10000
                },
                'type': 　'post',
                'success': function (data) {
                    console.log(data)
                    if (data.status == 0) {
                        commit('SET_THISMONTHREWARD', data.data)
                    }
                }
            })
        },
    }
}

export default rec