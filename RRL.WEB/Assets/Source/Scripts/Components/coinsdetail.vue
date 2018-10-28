<template>
    <div>
        <viewtitle></viewtitle>
        <div class="hot-goods-title-new" style='margin-top:54px;padding:0 15px;'>
            <p>我的金币：<span>{{coins}}</span></p>
            <router-link tag='p' to='/coininfo'>金币规则</router-link>
        </div>
        <div class='coinsort'>
            <span>
                可用：{{canusecoin}}
            </span>
            <span>
                冻结：{{frzcoin}}
            </span>
        </div>
        <div class="content mt20">
			<h3 class="pl10 pt10 pb10">金币明细</h3>
			<ul class="detailed">
				<li v-if = 'list.length != 0' v-for ='item in this.$store.state.coinlist'>
					<div class="box">
					<div class="left">
						<p class="color-333">{{item.addtime | timeformat}}</p>
						<p class="s"></p>
					</div>
					<div class="right">
						<p class="b">+{{item.money}}</p>
						<p class="s">{{item.remark }}</p>
					</div>
					</div>
				</li>
				<li style='color:#999;text-align:center;' v-if='list.length == 0'>
                    未查询到相关数据
                </li>
			</ul>
		</div>
    </div>

</template>

<script>
import $ from 'jquery';
import viewtitle from './title';
    let coinsdetail = {
        data(){
            return {
                coins:0,
                page:1,
                list:[],
                canusecoin:0,
                frzcoin:0
            }
        },
        
        components:{
            viewtitle
        },
        methods:{
            back() {
                this.$router.go(-1)
            },
            getdata(page){
                const that = this
              $.ajax({
                    url: 'WebApi/UserManager/PlatToReturnRecord',
                    type: 'post',
                    data:{
                        "token": this.$store.state.shorttoken,
                        "Page": page,
                        "PageSize": 10     
                   },         
                 success: function (data) {
                    let arr = that.list.concat(data.data)
                    let hash = {};
                    arr = arr.reduce(function (item, next) {
                    hash[next.id] ? '' : hash[next.id] = true && item.push(next);
                        return item
                    }, [])
                    that.list = arr;
                    that.$store.commit('setcoinlist',that.list)
                }
            });
            $.ajax({
                url: 'WebApi/UserManager/UserAccountInfo',
                type: 'post',
                data:{
                    "token": this.$store.state.shorttoken
                },
                success:function(data) {
                    that.frzcoin = data.data.ex_plate_to_return_money.toFixed(2) 
                    that.canusecoin = (data.data.plate_to_return_money).toFixed(2)
                    that.coins = (data.data.ex_plate_to_return_money + data.data.plate_to_return_money).toFixed(2)
                }
            })
            },
        },
        created() {
            // this.coins = sessionStorage.getItem('coins');
            this.getdata(this.page)
        },
        filters:{
            //设置详情列表滤镜
            timeformat(time){
                coinsdetail.nowtime=new Date()
                let newtime
                newtime=time.split('T')
                time=newtime[0].split('-')
                let year=time[0]
                let mouth=time[1]
                let day=time[2]
                if(year==coinsdetail.nowtime.getFullYear() && mouth==coinsdetail.nowtime.getMonth()+1 && day==coinsdetail.nowtime.getDate()){
                    time='今日'
                    return time
                }else if(year==coinsdetail.nowtime.getFullYear() && mouth==coinsdetail.nowtime.getMonth()+1 && day==coinsdetail.nowtime.getDate()-1){
                    time='昨日'
                    return time
                }else{
                    time=newtime[0]
                    return time
                }
            }
        },
    }   

    export default  coinsdetail
</script>