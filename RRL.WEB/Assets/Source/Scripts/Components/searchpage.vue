<!-->搜索页面<-->
<template>
    <div  v-finger:swipe="swipe">
        <div class='searchbox-inpage'>
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back"/>
            <div>
                <i></i>
                <input placeholder='搜索你想要的商品' v-model='name'/>
                <b v-on:click='clearname' v-if='this.name.length != 0'></b>
            </div>
            <div v-on:click='search' class='tosearchbtn'>
                搜索
            </div>
        </div>
        <sortgoods v-bind:goodslist='this.$store.state.searchlist'></sortgoods>
    </div>
</template>

<script>
    import sortgoods from './jingxuan/sort-goodscell'

    let searchpage={
        data(){
            return {
                name:'',
            }
        },
        components:{
            sortgoods
        },
        // computed:{
        //     //监听input
        //     getname(){
        //         return this.name
        //     },
        // },
        // watch:{
        //     getname(){
        //         this.$store.commit('clearsearchlist')
        //         this.$store.commit('clearsearchpage')
        //     },
        // },
        created(){
            
        },
        methods:{
            //清除搜索word
            clearname(){
                this.name=''
            },
            //搜索
            search(){
                if(this.name != this.$store.state.searchword) {
                    this.$store.commit('clearsearchlist')
                    this.$store.commit('clearsearchpage')
                    //编码word
                    this.$store.commit('SET_SEARCHWORD', this.name)
                    let word=encodeURIComponent(this.name)
                    this.$store.dispatch('search',word)
                }
                
            },
            back(){
                this.$router.go(-1)
            },
            swipe(e){
                if(e.direction=='Up'){
                    this.$store.commit('setsearchpage')
                    this.$store.dispatch('search',this.name)
                    
                }
            }
        },
        beforeRouteEnter : (to, from, next) => {
            if(from.name != 'goodsdetail'){
                next(function(searchpage){
                    searchpage.$store.commit('clearsearchlist')
                    searchpage.$store.commit('clearsearchpage')
                    searchpage.name = ''
                })
            }else{
                next()
            }
            
        }
    }   

    export default  searchpage
</script>