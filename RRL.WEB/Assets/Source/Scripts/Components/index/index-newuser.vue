<!-->首页新人有礼<-->
<template>
    <div class="new-user" v-on:click="tonews" style="padding:0 15px;">
         <img src="/Assets/Source/img/一元抢购.png" />
    </div>
</template>
<script>
    let newUser={
        methods:{
            //跳转至新人有礼注册页
            tonews(){
                //this.$router.push('/goodsdetail/295/null')
                // if(this.$store.state.longtoken!=undefined){
                //     this.$router.push('/ffff/:index')
                // }else{
                //     this.$router.push('/hhhh/:')
                // }
                window.location.href='https://e-shop.rrlsz.com.cn/Event/Seven/'
            }
        }
    }
    export  {newUser as default}
</script>