import $ from 'jquery';
const mycard = {
    state : {
        page:1,
        list:[],
    },
    mutations: {
        ADD_PAGE(state) {
            state.page ++;
        },
        SET_LIST(state, data) {
            var arr = state.list.concat(data)
            let hash = {};
            arr = arr.reduce(function (item, next) {
                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                return item
            }, [])
            state.list = arr
        }
    },
    actions : {
        get_card(context) {
            $.ajax({
                url:' /WebApi/UserManager/GetMyCoupons?token='+context.rootState.shorttoken+'&page_index='+context.state.page+'&page_size=10',
                type:'get',
                success:function(data) {
                    console.log(data)
                    if(data.status == 0) {
                        context.commit('SET_LIST', data.data)
                    }
                }
            })
        }
    }

}

export default mycard