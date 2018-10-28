<!-->头部返回条<-->
<template>
    <div class="back-page-btn" id="carttop">
        <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
        <p>{{this.$store.state.viewtitle}}</p>
    </div>
</template>
<script>
    const viewtitle={
        data(){
            return {
                hash:''
            }
        },
        methods:{
            //返回页面
            back(){
                this.$router.go(-1)
            },
        }
    }

    export  default viewtitle
</script>