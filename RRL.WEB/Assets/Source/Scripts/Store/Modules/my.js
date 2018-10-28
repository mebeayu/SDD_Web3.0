import $ from 'jquery';

import AskPromise from './promise.js';

let uesrequire = new AskPromise()

const my = {
    state: {
        userinfo: {},
        usercount:{}
    },
    mutations: {
        SET_USERINFO(state,data) {
            state.userinfo = data
            console.log(state.userinfo)
        },
        SET_USERCOUNT(state, data) {
            state.usercount = data;
        }
    },
    actions: {
        get_userinfo({commit,rootState}) {
            const token = rootState.shorttoken;
            const data = {
                url : 'WebApi/UserManager/UserInfo?token=' + encodeURIComponent(token),
                type:'GET',
            },
            callback = function(res) {
                if (res.status == 0) {
                    commit('SET_USERINFO', res.data[0])
                    sessionStorage.setItem('myid', res.data[0].username)
                } else {
                    localStorage.removeItem('longtoken')
                    sessionStorage.removeItem('myid')                                
                }
            };
            if(token.length != 0 && token != undefined){
                return uesrequire.require(data,callback)
            }
        },
        get_usercount({commit, rootState} ) {
            const token = rootState.shorttoken;
            const data = {
                url : 'WebApi/UserManager/UserRecommandCount',
                type: "POST",
                data:{
                    "token" : token
                }
            },
            callback = function(res) {
                if ( res.status == 0 ) {
                    commit('SET_USERCOUNT', res.data)
                }
            }
            if(token.length != 0 && token != undefined){
                return uesrequire.require(data,callback)
            }
            
        }
    }
}

export default my