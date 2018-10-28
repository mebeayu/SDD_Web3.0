import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex)

export default {
    state:{
        //defainde data
        test:'hello',
    },
    getters:{
        //definde methods
    },
    mutations:{
        //definde methods,can change the data in store,it must be async fn
        //fn(state,b){
            //1st parame is state,2nd parame is paylod,payload shoud be an object
        //}
        add:function(state,word){
            state.test=state.test+word
            console.log(state.test)
        }
    },
    actions:{
        //like mutations,but it commit mutation,can has unasync operation
        //fn(context){
            //context is store's copy but not store itself
            //context.commit(mutations.fn) ues this to commit a mutations' fn
        //}
        //store.dispatch({
            //dispath actions
            //type:'aaa',
            //amout:1
        //})
    }
}