<template>
    <div id="app" ref='app'>
        <keep-alive>
            <router-view v-if="$route.meta.keepAlive"></router-view>
        </keep-alive>
        <router-view v-if="!$route.meta.keepAlive"></router-view>
        <div class='isloading' v-show = 'show'><img src="/Assets/Source/img/loading.gif"/></div>
    </div>
</template>
<style>
    .isloading {
        width:100%;
        height:100%;
        background:#333;
        position:absolute;
        top:0;
        color:#fff;
        font-size:24px;
        z-index: 9999;
        overflow: hidden;
    }
    .isloading img{
        width: 100%;
        height: 100%;
    }
    #shadow1 .tiplogout {
        position: fixed;
        width: 8rem;
        left: 50%;
        top: 10%;
        background: #fff;
        padding: .2rem 0;
        text-align: center;
        border-radius: .2rem;
        font-size: .38rem;
        border: none;
        -webkit-box-shadow: 0.1rem 0.1rem 0.1rem rgba(0, 0, 0, 0.2);
        box-shadow: 0.1rem 0.1rem 0.1rem rgba(0, 0, 0, 0.2);
        line-height: 2.66666667rem;
        transform: translate(-50%);
        margin: 0 auto;
    }
    .tiplogout{
        border: none!important;
    }

    #shadow1 .tiplogout ul{
        padding: 0 .3rem;
    }

    #shadow1 .tiplogout li{
      text-align: left;
      line-height: .48rem;
      padding-top: .2rem;
    }

    #shadow1 .tiplogout li:nth-child(1){
        padding-top: 0;
    }

</style>
<script>
	import $ from 'jquery'
    let app={
        data() {
            return {
                scrollTop:0,
                show:true
            }
        },
        created(){
            console.log('app created')
            console.log(this.$store.state.shorttoken)
            //判定是否登录
            if(GLOBE_SHORT_TOKEN) {
                this.$store.commit('SET_SHORTTOKEN', GLOBE_SHORT_TOKEN)
                this.show = false
            } else {
                if(this.$store.state.shorttoken.length == 0 || this.$store.state.shorttoken == undefined) {
                    if ( localStorage.getItem('longtoken') ) {
                        const longtoken = localStorage.getItem('longtoken')
                        this.$store.dispatch('getshorttoken', longtoken).then(() => {
                            this.$store.dispatch('get_userinfo').then(() => {
                                this.show = false
                            })
                        })
                    }else{
                        this.show = false
                    }
                } else {
                    this.show = false
                }
            }
            this.$store.commit('setopenid', GLOBLE_OPEN_ID)
            // const gg = new Promise(function(resolve, reject){
            //     setTimeout(function(){resolve();alert('onegg');},300)
            // })
            // gg.then(() => {
            //     setTimeout(function(){alert('twogg');},300)
            // })
        },
        mounted() {
            console.log('app mounted')
            window.addEventListener('scroll', this.handleScroll)
  			//判断是否是ios打开
       	   var u = navigator.userAgent;
       	   if (u.indexOf('iPhone') > -1) {//苹果手机
       	   	 //$("#app").css({"paddingBottom":"60px"});
			}else{
				console.log("3322233")
			}
        },
        methods:{
            handleScroll () {
                this.scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop
                const route = this.$route
                if(route.meta.keepAlive == true) {
                    sessionStorage.setItem('top', this.scrollTop)
                }
            },
        },
    }

    export default app
</script>