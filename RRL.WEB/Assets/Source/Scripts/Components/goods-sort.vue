<!-->精选页面<-->
<template>
    <div>
        <div v-finger:swipe="swipe" class="wj-selected-cont" id='ss'>
            <sortbanner style="background:#fff;height:5.066667rem;overflow:hidden;"></sortbanner>
            <recommendgoods style="background:#fff;display:none"></recommendgoods>
            <sortgoods style="background:#fff"
                       v-bind:goodslist='this.$store.state.sortpagelist'></sortgoods>
            <!--v-bind:style="{marginBottom:$store.state.bottomHeight+'px'}"-->
        </div>
        <img @click="goTop" class="wj-gotop" src="/Assets/Source/img/totop.png" alt="" />
        <img @click="goBack" class="wj-goback" src="/Assets/Source/img/goback.png" alt="" />
        <bottom></bottom>
    </div>
</template>

<style scoped>
    .wj-selected-cont {
        position: absolute;
        top: 0;
        bottom: 1.306667rem;
        width: 100%;
        background: #f5f5f5;
        overflow-y: scroll;
    }
</style>

<script>
    //引入 banner 主推商品 商品列表cell 底部导航
    import sortbanner from './jingxuan/sortbanner';
    import recommendgoods from './jingxuan/sort-recommend-goods';
    import sortgoods from './jingxuan/sort-goodscell';
    import bottom from './bottom';
    import $ from 'jquery'

    let goodsSort = {
        data() {
            return {
                page: 1
            }
        },
        components: {
            sortbanner,
            recommendgoods,
            sortgoods,
            bottom
        },
        activated() {
        },
        created() {
            let that = this
            //获取主推商品
            this.$store.dispatch('setsorthotgoods')
            //获取banner
            this.$store.dispatch('setsortbanner')
            //重置banner下标为0
            this.$store.commit('setimgindex')
            //获取商品列表
            this.$store.dispatch('setsortgoods')
            //设置轮播
            //this.$store.dispatch('autosortbannerchange')
        },
        mounted() {
            //获取底部导航高
            this.$store.commit('bottomheight')
            //window.addEventListener('scroll',console.log(document.body.scrollTop))
        },
        methods: {
            //滑动进行分页
            swipe(e) {
                if (e.direction == 'Up') {
                    //设置页面
                    this.$store.commit('setpage')
                    //获取商品
                    this.$store.dispatch('setsortgoods')
                }
            },
            goTop() {
                // 返回顶部
                const $parent = $('#ss');
                if ($parent.scrollTop() === 0) return;
                //$('html,body').animate({scrollTop: '0px'}, 800);
                $parent.animate({scrollTop: '0'}, 800);
            },
            goBack() {
                // 返回上一页
                this.$router.go(-1);
            }
        },
        beforeDestroy() {
            //清除banner轮播计时器
            this.$store.dispatch('cleartimeout')
        },
        beforeRouteLeave: (to, from, next) => {
            //存储滚动条位置
            sessionStorage.setItem('scrolltop', window.scrollY)
            next()
            // ...
        }
        // beforeDestroy(){
        //     console.log(document.documentElement.scrollTop)
        // }
    }
    export {goodsSort as default}
</script>