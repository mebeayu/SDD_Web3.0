<!-->3级商品列表<-->
<template>
    <div v-finger:swipe="swipe">
        <searchbox></searchbox>
        <!--ul class='sortnav'>
            <li>
                <p>综合</p>
            </li>
            <li>
                <p>销量</p>
            </li>
            <li><p>新品</p></li>
            <li><p>价格</p></li>
            <li><p>筛选</p></li>
        </ul-->
        <sortlistcell></sortlistcell>
        <img @click="goTop" class="wj-gotop" src="/Assets/Source/img/totop.png" alt=""/>

        <div v-if="!$store.state.wjIsPageListFlag" @click="clickMoreDate" class="more">
            <button>{{ $store.state.wjClickMoreTil }}</button>
        </div>
        <p v-else class="no-data">数据已加载完毕 ~</p>
    </div>
</template>

<style scoped>
    .wj-gotop {
        bottom: .4rem !important;
        z-index: 4;
    }

    .no-data {
        padding: 0.4rem;
        text-align: center;
        color: #b2b2b2;
        font-size: 0.34666667rem;
    }

    .more {
        padding: 0 .4rem .4rem;
    }

    .more button {
        display: block;
        width: 100%;
        padding: 0.4rem 0;
        background: rgba(0, 0, 0, 0.1);
        border-radius: 0.05333333rem;
        border: none;
        color: #666;
        font-size: 0.32rem;
    }
</style>

<script>
    import searchbox from './sort-of-goods/searchbox';
    import sortlistcell from './sort-of-goods/sort-list-cell';
    import pageVisit from "./PublicModules/PageVisit.js";

    let goodslistthird = {
        data() {
            return {
            }
        },
        created() {
            this.$store.commit('clearlistofgoods');
            this.$store.dispatch('getlistofgoods', this.$route.params.id);
        },
        activated () {
        	pageVisit.logVisitTime({pageType:'list', goodsId:0, visitPath:location.href, deep:2});
		},
	    deactivated () {
	    	try{
	    		pageVisit.logLeaveTime();
	    	}catch(e){}
	    },
        components: {
            searchbox,
            sortlistcell
        },
        methods: {
            // 点击加载更多
            clickMoreDate() {
                // if (this.isPageListFlag) return;
                // this.clickMoreTil = "数据在加载中......";
                // this.getUnreadMessageCountAPI(this.curPage += 1);
                // if (this.$store.state.wjIsPageListFlag) return;
                this.$store.commit('setlistofgoodspage');
                this.$store.dispatch('getlistofgoods', this.$route.params.id);
            },
            swipe(e) {
                if (e.direction == 'Up') {
                    // console.log("Up");
                    this.$store.commit('setlistofgoodspage');
                    this.$store.dispatch('getlistofgoods', this.$route.params.id);
                }
            },
            goTop() {
                // 返回顶部
                const $parent = $('html,body');
                //if ($parent.scrollTop() === 0) return;
                $parent.animate({scrollTop: '0'}, 800);
            }
        },
        beforeRouteEnter: (to, from, next) => {
            if (from.name != 'goodsdetail') {
                next(goodslistthird => {
                        goodslistthird.$store.commit('clearlistofgoods')
                        goodslistthird.$store.commit('clearlistofgoodspage')
                        goodslistthird.$store.dispatch('getlistofgoods', goodslistthird.$route.params.id)
                    }
                )
            } else {
                next()
            }

        }
    }

    export default goodslistthird
</script>