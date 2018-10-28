<!-->商品详情页操作弹出框 belong to 商品详情<-->
<template>
    <div class="alertbox" v-if="this.$store.state.alertshow==true">
        <p v-if="this.$store.state.shorttoken.length != 0">
            添加成功
        </p>
        <p v-else>
            请先登录
        </p>
        <img src='/Assets/Source/img/选择@3x.png' v-if="this.$store.state.shorttoken.length != 0" />
        <img src='/Assets/Source/img/error.png' v-else />
    </div>
</template>

<script>
    let alertbox={

    }

    export default alertbox
</script>