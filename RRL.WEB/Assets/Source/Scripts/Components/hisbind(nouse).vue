<template>  
    <div style='background:#f5f5f5'>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
            <p>我的推荐奖励</p>
        </div>
        <ul class='hisbind'>
            <li class='nav'>
                <p>推荐人|时间</p>
                <p>昵称|电话号码</p>
            </li>
            <li class='contxt'>
                <div>
                    scop
                    <p>
                        2017-11-4 19:26
                    </p>
                </div>
                <div style='text-align:right'>
                    sbsbsb
                    <p style='color:#9e925f'>
                        159645679
                    </p>
                </div>
            </li>
        </ul>
    </div>
</template>
<style>
    .hisbind{
        padding:0 15px;
        background:#fff;
        margin-top:55px;
    }
    .hisbind li{
        display:flex;
        justify-content:space-between;
        border-bottom:1px solid #d4d4d4;
    }
    .nav{
        color:#7a7a7a;
        font-size:14px;
        line-height:40px
    }
    .contxt{
        font-size:16px;
        padding:5px 0;
    }
    .contxt p{
        color:#7a7a7a;
        font-size:12px;
    }
</style>
<script>
    let hisbind = {
        methods:{
            back() {
                this.$router.back()
            }
        }
    }


    export default hisbind
</script>