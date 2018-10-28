<!-->分类页<-->
<template>
    <div class="wj-sortlst-wrap">
        <searchbox></searchbox>
        <div class='sortlst'>
            <ul class='sortlist-left'>
                <li v-for='(item,index) in this.$store.state.sortlist'
                    v-if="item.id!=332"
                    :key="index"
                    @click='chosesort(index)'
                    :class="$store.state.sortstatus===index? 'hassort' : ''">
                    {{item.name}}
                </li>
            </ul>
            <!--<transition name="fade">-->
            <div class='sortlist-right'> <!--:key="num"-->
                <!--<img v-for='(img,index) in this.banner' :key="index" :src='$store.state.imgs + img.pic_id+$store.state.img_suffix'/>-->
                <ul id='last-item'>
                    <li v-for='(sec,index) in secondsort' :key="index">
                        <!--广告位置开始-->
                        <!--<div v-if="index===0" class="wj-sortlist-top-banner">
                            <img src="http://e-shop.rrlsz.com.cn/SWebApi/Public/picture/39308.jpg" alt="">
                        </div>-->
                        <!--广告位置结束-->
                        <div>
                            <div class='seclist'>
                                <i></i>
                                <p>{{sec.name}}</p>
                                <i></i>
                            </div>
                            <ul class='thirdlist'>
                                <li v-for='thd in sec.nodes' v-on:click='togoodsdetail(thd.id)'>
                                    <div>
                                        <img v-bind:src='$store.state.imgs + thd.icon+$store.state.img_suffix'
                                             :onerror="$store.state.defaultimg"/>
                                        <p>{{thd.name}}</p>
                                    </div>

                                </li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
            <!--</transition>-->

        </div>
        <bottom></bottom>

    </div>
</template>

<style scoped>
    .wj-sortlst-wrap {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        overflow: hidden;
    }

    .sortlst {
        position: absolute;
        width: 100%;
        left: 0;
        top: 1.2rem;
        bottom: 1.3066666666666666rem;
        background: #fff;
    }

    .sortlist-right {
        border-left: none !important;
        overflow-y: auto;
    }

    .sortlist-left {
        background: #f6f6f6;
        width: 1.8666666666666667rem;
        padding: 0 .2rem 3.2rem;
        height: auto;
    }

    .sortlist-left li {
        padding: 0;
        margin: .8rem 0 0;
        height: .7466666666666667rem;
        line-height: .7466666666666667rem;
        margin-bottom: 0;
        border-radius: .37333333333333335rem;
        font-size: .37333333333333335rem;
        color: #7a7b87;
    }

    .sortlst .sortlist-left .hassort {
        border: none;
        color: #fff;
        background-color: #65c4aa;
    }

    .sortlist-right .seclist p {
        color: #2d2d34;
        font-size: .37333333333333335rem;
    }

    .sortlist-right li i {
        width: .13333333333333333rem;
        background-color: #202020;
    }

    .sortlist-right li i:first-of-type {
        margin-right: .10666666666666667rem;
    }

    .sortlist-right li i:last-of-type {
        margin-left: .10666666666666667rem;
    }

    .sortlist-right .seclist {
        height: auto;
        padding: .5333333333333333rem 0 .8rem;
    }

    .wj-sortlist-top-banner {
        height: 2.6666666666666665rem;
        overflow: hidden;
    }

    .wj-sortlist-top-banner img {
        display: block;
        width: 100%;
        height: 100%;
    }

    .thirdlist div p {
        font-size: .32rem;
        color: rgb(45, 45, 52);
        padding-top: .26666666666666666rem;
    }

    .thirdlist li {
        margin-bottom: .8rem;
        width: 33.333333333333%;
    }

    .thirdlist div {
        padding: 0;
    }

    .thirdlist {
        -webkit-box-pack: start;
        -ms-flex-pack: start;
        justify-content: flex-start;
    }

    .fade-enter-active {
        animation: fadeInUp 1s ease-in;
    }

    .fade-leave-active {
        animation: fadeInUp 1s ease-in;
    }

    /*    @keyframes fadeInUp {
            0% {
                opacity: 0;
                transform: translate3d(0, 0, 0)
            }
            50%{
                opacity: 0;
                transform: translate3d(0, 100%, 0)
            }
            to {
                opacity: 1;
                transform: translate3d(0, 0, 0)
            }
        }*/

</style>

<script>
    import bottom from './bottom';
    import searchbox from './sort-of-goods/searchbox'

    let sortofgoods = {
        data() {
            return {
                secondsort: [],
                banner: [],
                num: null
            }
        },
        components: {
            bottom,
            searchbox
        },
        created() {
            if (this.$store.state.sortlist.length == 0) {
                this.$store.dispatch('getsortlist')
            } else {
                let status = this.$store.state.sortstatus
                this.secondsort = this.$store.state.sortlist[status].nodes
            }
        },
        /* mounted() {
             this.$store.commit('bottomheight')
         },*/
        computed: {
            getlist() {
                return this.$store.state.sortlist
            }
        },
        watch: {
            getlist() {
                this.banner = this.$store.state.sortlist[this.$store.state.sortstatus].carousl
                this.secondsort = this.$store.state.sortlist[this.$store.state.sortstatus].nodes
            }
        },
        methods: {
            chosesort(index) {
                this.$store.commit('setsortstatus', index)
                this.secondsort = this.$store.state.sortlist[this.$store.state.sortstatus].nodes;
                /*this.num = index;*/
            },
            togoodsdetail(id) {
                this.$router.push('/goodslistthird/' + id)
            }
        }
    }

    export default sortofgoods
</script>