import $ from 'jquery';

const orderdetail = {
    state: {
        orderdetail: {},
    },
    mutations: {
        SETORDERDETAIL(state, data) {
            state.orderdetail = data
            console.log(state.orderdetail)
        }
    },
    actions: {
        //获取订单详情
        setorderdetail({ commit, rootState }, id) {
            $.ajax({
                url: 'WebApi/TradeManager/OrderDetail?token=' + encodeURIComponent(rootState.shorttoken) + '&order_id=' + id,
                type: "get",
                success: function (data) {
                    console.log(data)
                    commit('SETORDERDETAIL', data.data)
                }
            })
        },
    }
}

export default orderdetail