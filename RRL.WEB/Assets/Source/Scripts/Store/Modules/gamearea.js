import $ from 'jquery';

import AskPromise from './promise.js'

let uesrequire = new AskPromise()

const gamearea = {
    state:{
        gameuserinfo:{},
        cardlist:[],
        addlogslist: [],
        starttime: new Date().getTime(),
        rdcode: String(Math.random()).split(".")[1]

    },
    mutations:{
        SET_GAMEUSERINFO(state, data) {
            state.gameuserinfo = data
        },
        SET_CARDLIST(state, data) {
            state.cardlist = data
        },
        SET_ADDLOGSLIST(state, data) {
            state.addlogslist = data
        },
    },
    actions:{
        get_gameuserinfo({commit,rootState}) {
            const data = {
                url:'WebApi/Game/GetUserInfo',
                data:{
                    'token': rootState.shorttoken
                },
                type:'post',
            },
            callback = function(res) {
                commit('SET_GAMEUSERINFO', res.data)
            };
            return uesrequire.require(data,callback)
        },
        get_cardlist({commit}) {
            const data = {
                url:'WebApi/Game/KaQuanList',
                type:'get',
            },
            callback = function(res) {
                commit('SET_CARDLIST', res.data)
            };
            return uesrequire.require(data,callback)
        },
        get_xhb({commit}) {
            const data = {
                url: 'WebApi/Game/GetRedpacketCouponsList?payRedpacket='+ payRedpacket,
                type: 'get',
            },
            callback = function(res) {
                commit('SET_CARDLIST', res.data)
            };
            return uesrequire.require(data,callback)
        },
        get_addlogslist({commit}) {
            const data = {
                url:'WebApi/Game/GetAddLogs',
                type:'get',
            },
            callback = function(res) {
                commit('SET_ADDLOGSLIST', res.data)
            };
            return uesrequire.require(data,callback)
        },
        loop_page(context) {
            const endtime = new Date().getTime();//用户退出时间
            const duration = Math.floor((endtime - context.state.starttime) / 1000);
            $.post("/WebApi/Game/SetState", 
            { starttime: context.state.starttime, endtime: endtime, duration: duration, rdcode: context.state.rdcode, token: context.rootState.shorttoken }, 
            function (ret) {
                if (ret.data == true) {
                    $.post("/WebApi/Game/RefreshRandom", { token: context.rootState.shorttoken },
                    function (data) {
                        if (data.additional_data !== 0) {
                            $('.maskxx').css('display', 'block')
                            $('.gethbnum').text(data.additional_data )
                            // getuser()
                            context.dispatch('get_gameuserinfo')
                        } else if (data.add_data !== 0) {
                            // alert(data.data)
                        } else {
                            // alert('领取失败，请明天再试')
                        }
                    },'json')
                };
                if (ret.additional_data == true) {
                    $.post("/WebApi/Game/RefreshPer",
                        { "token": context.rootState.shorttoken },
                        function (res) {
                            console.log(res)
                            if (res.status == 0) {
                            	var elText="";
                            	if(res.data.FreeMoney.Money>0){
                            		elText+=res.data.FreeMoney.Msg+"</br>";
                            	}
                            	if(res.data.PayMoney.Money>0){
                            		elText+=res.data.PayMoney.Msg;
                            	}
                                $('.maskxx').css('display', 'block')
                                $('.gethbnum').html(elText);
                            } else {
                                // alert(res.data)
                            }
                            
                        },'json')
                }
            }, 'json')
            setTimeout(function () {
    
                context.dispatch('loop_page')
            }, 2000)
    
        }
    }
}

export default gamearea