<!-->搜索框组件<-->
<template>
    <router-link tag='div' to='/searchpage' class='sortsearch'>
        <img class="wj-arrow-left" src="/Assets/Source/img/返回@3x.png" v-on:click.prevent="back()"
             v-if="this.$route.name== 'goodslistthird'"/>
        <div class='searchbox'>
            <img src="/Assets/Source/img/搜索@3x.png"/>
            <p>搜索你想要的商品</p>
        </div>
    </router-link>
</template>

<style scoped>
    .sortsearch {
        position:absolute;
        left: 0;
        top: 0;
        width: 100%;
        padding: .186667rem .266667rem;
        border-bottom: .013333333333333334rem solid #dadada;
    }
    .searchbox{
        height: .8rem;
        background: #f4f4f4;
        border-radius: .053333rem;
        margin-left: .82rem;
    }
    .searchbox p{
        font-size: .32rem;
        color: #83848f;
        padding-left: .186667rem;
    }
    .searchbox img{
        width: .293333rem;
        height: .293333rem;
        padding-right: 0;
    }
</style>

<script>
    let searchbox = {
        methods: {
            back() {
                this.$router.go(-1)
            }
        }
    }

    export default searchbox
</script>