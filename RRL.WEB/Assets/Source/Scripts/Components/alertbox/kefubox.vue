<!-->客服弹出框<-->
<template>
        <div class="shadow1" v-if="this.$store.state.kefushow==true" v-on:click="close">
            <div class="tiplogout">
                <p>详情请联系客服</p>
                <p>
                    400-990-7881
                </p>
            </div>
        </div>
</template>

<script>
    let kefubox={
        methods:{
            close(){
                //关闭客服弹出框
                this.$store.commit('setkefushow',false)
            }
        }
    }

    export default kefubox
</script>