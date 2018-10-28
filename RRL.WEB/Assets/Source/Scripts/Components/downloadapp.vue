<!-->下载app提示页<-->
<template>
    <div style="position:relative">
        <img src="/Assets/Source/img/返回@3x.png" style="width:15px;height:20px;position:relative;top:15px;left:15px;" v-on:click="back" />
        <img src="/Assets/Source/img/download.png" style="width:100%;" />
    </div>
</template>
<script>
    let downloadapp={
        methods:{
            back(){
                this.$router.back(-1)
            }
        }
    }

    export default downloadapp
</script>