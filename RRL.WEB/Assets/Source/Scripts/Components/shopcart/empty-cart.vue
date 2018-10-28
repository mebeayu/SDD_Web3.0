<!-->空购物车显示框 belong to 购物车<-->
<template>
    <div class="empty-cart">
        <img src="/Assets/Source/img/购物车@3x.png" />
        <p>购物车是空的</p>
        <div class="empty-cart-btn">
        <router-link to="/goodssort">
            <input type="button" value="去逛逛" />
        </router-link>
        <router-link to="/login" v-if="this.$store.state.shorttoken.length == 0">
            <input type="button" value="登录" />
        </router-link>
        <router-link to="/goodscollection" v-else>
            <input type="button" value="看看收藏" />
        </router-link>
        </div>
    </div>
</template>
<script>
    let emptycart={

    }

    export {emptycart as default}
</script>