<!--底部导航-->
<template>
    <div class="wj-bottom-navbar wj-bottom-nav">
        <ul>
            <router-link tag="li" to="/index" class="cur">
                <img src="Assets/Source/img/首页灰色@3x.png" alt="">
                <img src="Assets/Source/img/首页选中@3x.png" alt="">
                <span>首页</span>
            </router-link>
            <router-link tag="li" to="/sortofgoods">
                <img src="Assets/Source/img/分类灰色@3x.png" alt="">
                <img src="Assets/Source/img/分类选中@3x.png" alt="">
                <span>分类</span>
            </router-link>
            <router-link tag="li" to="/gamehall" v-if="isShowGame==1">
                <img src="Assets/Source/img/collar_red_gray.png" alt="">
                <img src="Assets/Source/img/collar_red_light.png" alt="">
                <span>娱乐中心</span>
                <sup v-if="isShowRedSup"></sup>
            </router-link>
            <!--
            <router-link tag="li" to="/card" v-if="isShowGame==1">
                <img src="Assets/Source/img/collar_red_gray.png" alt="">
                <img src="Assets/Source/img/collar_red_light.png" alt="">
                <span>卡券中心</span>
                <sup v-if="isShowRedSup"></sup>
            </router-link>
            -->
            <router-link tag="li" to="/shopcart">
                <img src="Assets/Source/img/购物车灰色@3x.png" alt="">
                <img src="Assets/Source/img/购物车选中@3x.png" alt="">
                <span>购物车</span>
            </router-link>
            <router-link tag="li" to="/my">
                <img src="Assets/Source/img/我的灰色@3x.png" alt="">
                <img src="Assets/Source/img/我的选中@3x.png" alt="">
                <span>我的</span>
            </router-link>
        </ul>
    </div>
</template>

<style scoped>
	/*小红点*/
	.wj-bottom-navbar ul li sup {
    width: 0.2rem;
    height: 0.2rem;
    border-radius: 100%;
    background: #f23030;
    display: block;
    position: absolute;
    top: 0.2rem;
    /* left: 0.95rem; */
    left: 53%;
	}
	
    .wj-bottom-nav ul {
        -ms-flex-direction: row;
        flex-direction: row
    }

    .wj-bottom-nav li, .wj-bottom-nav ul {
        display: -ms-flexbox;
        display: flex
    }

    .wj-bottom-nav li {
        -ms-flex: 1;
        flex: 1;
        -ms-flex-direction: column;
        flex-direction: column;
        -ms-flex-pack: center;
        justify-content: center;
        -ms-flex-align: center;
        align-items: center;
        -ms-flex-line-pack: center;
        align-content: center;
        height: 1.306667rem;
        border-top: .013333rem solid #dadada;
        box-sizing: border-box
    }

    .wj-bottom-nav li.router-link-exact-active.router-link-active span {
        color: #42b983
    }

    .wj-bottom-nav li img {
        display: block;
        width: .533333rem;
        height: .533333rem;
    }

    .wj-bottom-nav li img:nth-of-type(2) {
        display: none;
    }

    .wj-bottom-nav li.router-link-exact-active.router-link-active img:nth-of-type(2) {
        display: block;
    }

    .wj-bottom-nav li.router-link-exact-active.router-link-active img:nth-of-type(1) {
        display: none;
    }

    .wj-bottom-nav li span {
        font-size: .266667rem;
        padding-top: .133333rem;
        color: #9e9fa9
    }

    .wj-bottom-navbar {
        bottom: 0
    }

    .wj-bottom-navbar {
        position: absolute;
        left: 0;
        width: 100%;
        background: #fff
    }
</style>

<script>
    let bottom = {
        data() {
            return {
                isShowGame: null,
                isShowRedSup:false,
                h_money_free:window.h_money_free
            }
        },
        created() {
            this.isShowGame = window.is_show_game;
            this.showRedSup();
        },
        activated () {
           this.showRedSup();
		    },
        methods: {
            toLink() {
                window.location.href = "/#/GameHall?token="+localStorage.getItem('shorttoken');
                //window.location.href = "/#/GameHall?redirect=" + window.location.href;
            },
            showRedSup(){
            	if( this.h_money_free>0)
	            {
	            	this.isShowRedSup=true;
	            }
            }
        }
    }
    export {bottom as default}
</script>