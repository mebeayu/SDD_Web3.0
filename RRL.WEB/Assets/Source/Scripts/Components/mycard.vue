<template>
    <div class='buy-card' v-finger:swipe="swipe" style='background:#fff'>
        <viewtitle></viewtitle>
        <ul class='card-list' id='first-item'>
            <li v-for='(item, index) in this.$store.state.mycard.list'>
                <!--<div class='cl-left'>
                    <i class='moneyicon'></i><span>{{item.countmoney}}</span>
                </div>
                <div class='cl-right' id='cl-right'>
                    <p style='white-space:normal'>
                        {{item.name}}
                    </p>
                    <p style='white-space:normal;margin-top:14px;'>
                        购买时间:{{item.addtimestr}}
                    </p>
                    <p style='white-space:normal;margin-top:14px;'>
                        有效期至:{{item.endtimestr}}
                    </p>
                </div>-->
                <div class='flexbox'>
                    <div class='cl-left'>
                    <i class ='moneyicon'></i><span>{{item.countmoney}}</span>
                    </div>
                    <div class='cl-right'>
                        <p style='margin-top:14px;'>
                           {{item.name}}
                        </p>
                        <p style='margin-top:14px;'>
                                                      购买时间:{{item.addtimestr}}
                        </p>
                    </div>
                </div>
                <div class='cardtime'>有效时间至 {{item.endtimestr.split('T')[0]}}</div>
            </li>
        </ul>
    </div>
</template>

<script>
    import $ from 'jquery';
    import viewtitle from './title';

    const mycard = {
        data() {
            return {
                choseindex:0
            }
        },
        components:{
            viewtitle
        },
        created() {
            this.$store.dispatch('get_card')
        },
        methods:{
            swipe(e){
                console.log(e)
                if(e.direction == 'Up') {
                    this.$store.commit('ADD_PAGE')
                    this.$store.dispatch('get_card')
                }
            },
        }
    }   

    export default mycard 
</script>