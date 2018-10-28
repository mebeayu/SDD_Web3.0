<template>
    <div style='background:#f5f5f5'>
        <viewtitle></viewtitle>
        <ul class='rec-reward-title'>
            <li>
                {{montharr[0]}}月：<span>{{this.$store.state.rec.thisMonthOrder.month_total_order || 0}}</span>
                <p v-if='this.$store.state.rec.MonthOrderRecData.is_shop_keeper == true'>冻结：{{this.$store.state.rec.thisMonthOrder.month_total_order - this.$store.state.rec.thisMonthOrder.month_settled_order }}</p>
            </li>
            <li>
                全部：<span>{{this.$store.state.rec.MonthOrderRecData.year_total_order  || 0}}</span>
                <p v-if='this.$store.state.rec.MonthOrderRecData.is_shop_keeper == true'>冻结：{{this.$store.state.rec.MonthOrderRecData.year_total_order - this.$store.state.rec.MonthOrderRecData.year_settled_order  }}</p>
            </li>
        </ul>
        <ul class='rec-reward-month'>
            <li  v-on:click='chosemonth(0)' v-bind:class="monthstatus == 0?'on':'' ">
                全部
            </li>
            <li v-for='num in montharr' v-on:click='chosemonth(num)' v-bind:class="monthstatus == num?'on':'' ">
                {{num}}月
            </li>
        </ul>
        <ul class='rec-reward-sure' v-if='this.$store.state.rec.MonthOrderRecData.is_shop_keeper == true'>
            <li v-on:click = 'chosetype(0)' v-bind:class="typestatus == 0?'on':'' ">
                待确认：<span>{{this.$store.getters.orderlist.not_consumedList.length}}</span>
            </li>
            <li v-on:click = 'chosetype(1)' v-bind:class="typestatus == 1?'on':'' ">
                 已确认：<span>{{this.$store.getters.orderlist.consumedList.length}}</span>
            </li>
        </ul>
        <ul class='rec-reward-detail' v-if='this.$store.state.rec.MonthOrderRecData.is_shop_keeper == true'>
            <li v-for='item in this.$store.getters.orderlist.not_consumedList' v-if='typestatus == 0'>
                <div>
                    订单号：{{item.ordercode}}
                    <a>
                        <img src='/Assets/Source/img/ti.png' v-if='item.recommand_class == 1' />
                        <img src='/Assets/Source/img/jin.png' v-else/>
                        ￥{{item.money | moneyformate(item.money, item.recommand_class)}}
                    </a>
                </div>
                <p>
                    购买用户：{{item.username}}
                </p>
                <p>
                    购买时间：{{item.addtime | timeformate}}
                </p>
                <span>￥{{item.money}}</span>
            </li>
            <li v-for='item in this.$store.getters.orderlist.consumedList' v-if='typestatus == 1'>
                <div>
                    订单号：{{item.ordercode}}
                    <a>
                        <img src='/Assets/Source/img/ti.png' v-if='item.recommand_class == 1' />
                        <img src='/Assets/Source/img/jin.png' v-else/>
                        ￥{{(item.money/10).toFixed(2)}}
                    </a>
                </div>
                <p>
                    购买用户：{{item.username}}
                </p>
                <p>
                    购买时间：{{item.addtime | timeformate}}
                </p>
                <p>
                    确认时间：{{item.checktime | timeformate}}
                </p>
                <span>￥{{item.money}}</span>
            </li>
        </ul>
        <ul class='rec-reward-detail' v-if='this.$store.state.rec.MonthOrderRecData.is_shop_keeper == false'>
            <li v-for='item in this.$store.state.rec.MonthOrderList'>
                <div>
                    订单号：{{item.ordercode}}
                    <a>
                        <img src='/Assets/Source/img/ti.png' v-if='item.recommand_class == 1' />
                        <img src='/Assets/Source/img/jin.png' v-else/>
                        ￥{{item | moneyformate(item.money, item.recommand_class)}}
                    </a>
                </div>
                <p>
                    购买用户：{{item.username | phoneformate}}
                </p>
                <p>
                    购买时间：{{item.addtime | timeformate}}
                </p>
                <p v-if='item.status == 3'>
                    确认时间：{{item.checktime | timeformate}}
                </p>
                <p style='position:absolute; bottom:5px; right:15px;'>
                    <a style='font-size:14px'>￥{{item.money}}
                    </a>
                    <br/>
                    <a style='color:#ba3027;font-size:16px' v-if='item.status == 2'>
                        待确认
                    </a>
                    <a style='color:#7a7a7a;font-size:16px' v-else>
                        已确认
                    </a>
                </p>
                
            </li>
        </ul>
    </div>
</template>

<script>
    import viewtitle from './title';
    let myrecorder = {
        data(){
            return {
                montharr:[],
                monthstatus:0,
                typestatus:0
            }
        },
        components:{
            viewtitle
        },
        created() {
            const date = new Date()
            const month = date.getMonth()+1
            for(let i =0; i<= month-1; i++) {
                this.montharr.unshift(i+1);
            }
            //获取本月的推荐信息
            this.$store.dispatch('getThisMonthOrder', month)
            //动态获取每月推荐信息
            this.$store.dispatch('getMonthOrderData', this.monthstatus)
        },
        methods:{
            chosemonth(sta) {
                this.monthstatus = sta;
                this.$store.dispatch('getMonthOrderData', this.monthstatus)
            },
            chosetype(sta) {
                this.typestatus =sta
            },
            back() {
                this.$router.back()
            }
        },
        filters: {
            timeformate(time) {
                let fortime = time.replace('T', ` `)
                let arr = time.split('T')
                let montharr = arr[0].split('-')
                let timestamparr = arr[1].split(':')
                let timestr = montharr[1] + '-' + montharr[2] + ` ` + timestamparr[0] + ':' + timestamparr[1]
                return timestr
            },
            phoneformate(num) {
                return num.substr(0, 3) + "****" + num.substr(7)
            },
            moneyformate(money, type) {
                //return (money / 100) + ',' +type
                if(type == 1 || type ==2 ) {
                    return (money / 100).toFixed(2)
                } else {
                    return (money /100 * 2).toFixed(2)
                }
            }
        },
    }


    export default myrecorder
</script>