<!-->头部返回条<-->
<template>
<div class="back-page-btn" id="carttop">
        <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
        <p v-if="hash=='#/choseadd'">选择收货地址</p>
        <p v-else>我的订单</p>

        <!--p>订单详情</p-->
        <!--span v-if="hash=='#/editadd'" style="position: absolute;right: 15px;top:15px;font-size:17px;" v-on:click="save">保存</span-->
    </div>
</template>
<script>
    let backbtn={
        data(){
            return {
                hash:''
            }
        },
        created(){
            //获取url信息
            this.hash=location.hash
        },
        methods:{
            //返回页面
            back(){
                if(this.$route.params.status){
                    this.$router.push('/my')
                }else{
                    this.$router.back(-1)
                }

            },
        }
    }

    export  default backbtn
</script>