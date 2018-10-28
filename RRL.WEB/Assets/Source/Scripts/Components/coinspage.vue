<template>
    <div class='coinspage' v-finger:swipe="swipe">
        <viewtitle></viewtitle>
        <div class="hot-goods-title-new" id='first-item'>
            <p>我的金币：<span>{{coins}}</span></p>
            <router-link tag='p' to='/coinsdetail'>金币明细</router-link>
        </div>
        <div class="index-banner">
            <div v-bind:style="{width:this.$store.state.coinbanner.length*100+'%' ,marginLeft:-this.$store.state.imgindex*this.$store.state.screenWidth+'px' }">
            <img v-for="(item,index) in this.$store.state.coinbanner"
                    v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix"
                    v-bind:index="index"
                    v-finger:swipe="swipebanner"
                    v-bind:style="{width:$store.state.screenWidth+'px'}"
                    :onerror="$store.state.defautbanner"
                    :load='$store.state.defautbanner'
                    v-on:click="direct(item)" />
            </div>
            <ul class="focus-dot">
            <li v-for="(item,index) in this.$store.state.indexbanner"
                v-bind:class="index==$store.state.imgindex?'beenchose':''"
                @click="changebanner(index)"
            ></li>
            </ul>
        </div>
        <ul class='coinspage_nav'>
            <li v-bind:class=" type == 0? 'on':''" v-on:click = 'chose(0)'>
                默认
            </li>
            <li v-on:click = 'chose(1)' v-bind:class=" type == 1? 'on':''">
                兑换量
            </li>
            <li v-on:click = 'chose(2)' v-bind:class=" type == 2? 'on':''">
                金币
            </li>
        </ul>
        <ul class='coinscell' style='padding:15px;'>
            <li v-for='item in this.$store.state.coinpagelist' v-on:click='todetail(item.id)'>
                <img v-bind:src='$store.state.imgs+item.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg" />
                <p>
                    {{item.name}}
                </p>
                <p>
                    {{item.propaganda }}
                </p>
                <p>
                    {{item.price }} + {{item.discount }}金币
                </p>
                <p>
                    官方指导价:￥ {{item.local_price }}
                </p>
            </li>
        </ul>
    </div>
</template>

<script>
import $ from 'jquery'
import viewtitle from './title';
    let coinspage = {
        data(){
            return {
                page:1,
                coins:0,
                type:0
            }
        },
        components:{
            viewtitle
        },
        methods:{
            back() {
                this.$router.back(-1)
            },
            swipe(e){
                let word;
                if(e.direction=='Up'){
                    this.page++;
                    switch(this.type) {
                        case 0:
                        word = 'order_weight';
                        break;
                        case 1:
                        word = 'sell_count';
                        break;
                        case 2 :
                        word = 'return_money_discount';
                        break;
                    }
                    this.$store.dispatch('getcoinpagelist',[this.page, word]);
                }
            },
            todetail(id) {
                this.$router.push('/coingoods/'+ id )
            },
            chose(type) {
                this.type = type;
                let word = '';
                switch(type) {
                    case 0:
                    word = 'order_weight';
                    break;
                    case 1:
                    word = 'sell_count';
                    break;
                    case 2 :
                    word = 'return_money_discount';
                    break;
                }
                this.$store.commit('clearcoinpagelist');
                this.$store.dispatch('getcoinpagelist', [1, word]);
            },
            getbanner() {
                const that = this;
                $.ajax({
                    url:'WebApi/ConfigManager/ExchangeCarousel',
                    type: 'get',
                    success:function(data) {
                        that.$store.commit('setcoinbanner',  data.data);
                    }
                })
            },
            changebanner(index){
                this.$store.commit('clickbannerdot',index)
            },
            swipebanner(e){
                this.$store.commit('changebannerindex',e.direction)
            },
            direct(item){
                if(item.direct_type==1){
                    this.$router.push('goodsdetail/'+item.url+'/null')
                }else if(item.direct_type==2){
                    //go to shopdetail
                }else{
                    window.location.href=item.url
                }
            }
        },
        created() {
            this.$store.dispatch('getcoinpagelist', [this.page, 'order_weight']);
            this.$store.dispatch('getUserAccountInfo');
            this.getbanner();
            this.$store.commit('getscreenwidth')
        },
        computed:{
            getwallet(){
                return this.$store.state.wallet.UserAccountInfo
            }
        },
        watch:{
            //钱包显示计算
            getwallet(){
                this.coins = this.$store.state.wallet.UserAccountInfo.plate_to_return_money + this.$store.state.wallet.UserAccountInfo.ex_plate_to_return_money 
                this.coins = parseFloat(this.coins.toFixed(2));
                sessionStorage.setItem('coins', this.coins);
            }
        },
    }   

    export default  coinspage
</script>