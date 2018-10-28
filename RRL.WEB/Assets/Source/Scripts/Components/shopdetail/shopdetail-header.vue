<template>
    <div class='shopbannerbox'>
            <div class='shopbanner'>
                <img src='/Assets/Source/img/默认banner.png'/>
                <div class='shopbanner-name'>
                    <div class='left'>
                        <img src='/Assets/Source/img/默认图像.png'/>
                        <p>shop.name</p>
                    </div>
                    <div class='right'>
                        <img src="/Assets/Source/img/收藏@3x.png"/>
                        <p>收藏商家</p>
                    </div>
                </div>
            </div>
            <div class="top-nav">
                <div>
                    <img src="/Assets/Source/img/goback.png" style="width:25px;height:25px" v-on:click="back" />
                    <img src="/Assets/Source/img/顶部分享@3x.png" style="width:25px;height:5px;" v-on:click="showshare" />
                    <ul class="goods-showbox" v-if="showbox==true">
                        <li v-on:click="share">分享商家</li>
                        <li v-on:click="kefu">联系商家</li>
                    </ul>
                </div>
            </div>
        </div>    
</template>

<script>
    let shopdetailheader={
        data(){
            return {
                showbox:false
            }
        },
        methods:{
            back(){
                this.$router.back(-1)
            },
            //分享下拉框显示
            showshare(){
                this.showbox=!this.showbox
            },
            //客服弹出框
            kefu(){

                this.$store.commit('setkefushow',true)
            },
            //分享弹出框
            share(){
                alert('请点击右上角分享')
            }
        }
    }    

    export default shopdetailheader
</script>