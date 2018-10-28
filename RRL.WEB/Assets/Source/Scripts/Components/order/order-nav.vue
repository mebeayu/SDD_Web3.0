<!-->订单列表导航<-->
<template>

</template>
<script>
    let ordernav={
        created(){
            //提交显示状态及获取相应订单列表
            this.$store.commit('setorderstatus',this.$route.params.status)
            this.$store.dispatch('getorderlist')

        },
        methods:{
            //导航切换及列表展示
            status(e){
                this.$store.commit('setorderstatus',e)
                switch(e){
                    case 0:{
                        if(this.$store.state.orderlist.all.length==0){
                            this.$store.dispatch('getorderlist')
                        }else{
                            let hash = {};
                            let arr=this.$store.state.orderlist.all
                            arr = arr.reduce(function(item, next) {
                                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                                return item
                            }, [])
                            this.$store.commit('uniqueorderlist',arr)
                        }
                    }
                    break;
                    case 1:{
                        if(this.$store.state.orderlist.notpay.length==0){
                            this.$store.dispatch('getorderlist')
                        }else{
                            let hash = {};
                            let arr=this.$store.state.orderlist.notpay
                            arr = arr.reduce(function(item, next) {
                                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                                return item
                            }, [])
                            this.$store.commit('uniqueorderlist',arr)
                            //this.$store.commit('setorderlist',arr)
                        }
                    }
                    break;
                    case 2:{
                        if(this.$store.state.orderlist.notrecived.length==0){
                            this.$store.dispatch('getorderlist')
                        }else{
                            let hash = {};
                            let arr=this.$store.state.orderlist.notrecived
                            arr = arr.reduce(function(item, next) {
                                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                                return item
                            }, [])
                            this.$store.commit('uniqueorderlist',arr)
                            //this.$store.commit('setorderlist',arr)
                        }
                    }
                    break;
                    case 3:{
                        if(this.$store.state.orderlist.notcommend.length==0){
                            this.$store.dispatch('getorderlist')
                        }else{
                            let hash = {};
                            let arr=this.$store.state.orderlist.notcommend
                            arr = arr.reduce(function(item, next) {
                                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                                return item
                            }, [])
                            this.$store.commit('uniqueorderlist',arr)
                            //this.$store.commit('setorderlist',arr)
                        }
                    }
                    break;
                    case 4:{
                        if(this.$store.state.orderlist.return.length==0){
                            this.$store.dispatch('getorderlist')
                        }else{
                            let hash = {};
                            let arr=this.$store.state.orderlist.return
                            arr = arr.reduce(function(item, next) {
                                hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                                return item
                            }, [])
                            this.$store.commit('uniqueorderlist',arr)
                            //this.$store.commit('setorderlist',arr)
                        }
                    }
                    break;
                }
            }
        }
    }
    export  default ordernav
</script>