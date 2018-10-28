<template>
    <div class='indexsortbox'>
        <div class='sort-title'>
            商品分类
        </div>
        <div class='overbox'>
            <div class='ulbox' v-bind:style="{'marginLeft':-marginLeft+'px'}" v-finger:swipe="swipe">
                <ul class='sort-del' v-for=' (list, index) in sortarr'>
                    <li v-for='item in list' v-on:click='tosort(item.redirect_type, item.redirect_url,item.id)'>
                        <img v-bind:src='$store.state.imgs + item.icon+$store.state.img_suffix' :onerror="$store.state.defaultimg" :load='$store.state.defaultimg' />
                        <p>{{item.name}}</p>
                    </li>
                </ul>
            </div>
        </div>
        <ul class='sort-dot'>
            <li v-for=' (list, index) in sortarr' v-bind:class="sortarrindex == index? 'on':'' "></li>
        </ul>
    </div>
</template>
<script>
    import $ from 'jquery';
    const indexSort = {
        data() {
            return {
                sortarr: [],
                sortarrindex: 0,
                marginLeft: 0,
                coinid:0,
                gameid:0,
                reg:/\#token\#/,
            }
        },
        created() {
            const that = this
            this.$store.dispatch('getgameandcoinid')
            $.ajax({
                url: 'WebApi/ShopManager/SimpleGoodsTypeList',
                type: 'get',
                success: function(data) {
                    const len = data.data.length
                    for (let i = 0; i < len; i += 10) {
                        that.sortarr.push(data.data.slice(i, i + 10));
                    }
                    console.log(that.sortarr)
                }
            });
            
        },
        methods: {
            swipe(e) {
                if (e.direction == 'Left' && this.sortarrindex < this.sortarr.length - 1) {
                    this.sortarrindex++
                } else if (e.direction == 'Right' && this.sortarrindex > 0) {
                    this.sortarrindex--
                }
                this.marginLeft = this.$store.state.screenWidth * this.sortarrindex
            },
            tosort(type,url,id) {
                // if(id == this.$store.state.gameid) {
                //     this.$router.push('/gamearea')
                //     //window.location.href = 'https://e-shop.rrlsz.com.cn/Game/Roulette/?token=' + encodeURIComponent(this.$store.state.shorttoken);
                // } else if(id == this.$store.state.coinid) {
                //     this.$router.push('/coinspage')
                // }else {
                //     this.$router.push('/goodslistthird/' + id)
                // }
                if(type == 0) {
                    this.$router.push('/goodslistthird/' + id)
                } else {
                    //var testul = 'https://tyu?#token#';
                    url = url.replace(this.reg,this.$store.state.shorttoken);
                    window.location.href  = url
                }
            }
        }
    }
    export default indexSort
</script>