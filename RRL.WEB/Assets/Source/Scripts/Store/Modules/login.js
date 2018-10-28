import $ from 'jquery';

import AskPromise from './promise.js';

let uesrequire = new AskPromise()


const login = {
    state:{
    },
    mutations:{
        
    },
    actions:{
        getshorttoken({commit},longtoken){
            const data = {
                url : 'WebApi/UserManager/ApplyToken?token=' + encodeURIComponent(longtoken) + '&devicecode=&channel=wx',
                type:'GET',
            },
            callback = function(res) {
                if(res.status == 0) {
                    commit('SET_SHORTTOKEN', res.data)
                } else {
                    localStorage.removeItem('longtoken')
                    sessionStorage.removeItem('myid')                            
                }
            };
            return uesrequire.require(data,callback)
        },
        logout({commit, rootState}) {
            $.ajax({
                url:'WebApi/UserManager/Logout?token=' + rootState.shorttoken,
                type:'get',
                success:function(data) {
                    console.log(data);
                    localStorage.removeItem('longtoken')
                    sessionStorage.removeItem('myid')
                    commit('SET_SHORTTOKEN', '')
                    location.reload()
                }
            })
            
        }
    }
}

export default login;