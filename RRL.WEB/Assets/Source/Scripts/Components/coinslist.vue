<template>
    <div v-finger:swipe="swipe">
        <div class="top"><span class="icon-left" v-on:click = 'back'></span>我的钱包</div>
		<div class="content">
			<h3 class="pl10 pt10 pb10">我的金币:<span class="color-red pl10">{{this.$store.getters.mycoins}}</span></h3>
		</div>
		<div class="content mt20">
			<h3 class="pl10 pt10 pb10">金币明细</h3>
			<ul class="detailed">
				<li v-for ='item in this.$store.state.coinlist' v-if = 'list.length != 0'>
					<div class="box">
					<div class="left">
						<p class="color-333">{{item.addtime | timeformat}}</p>
						<p class="s"></p>
					</div>
					<div class="right">
						<p class="b">{{item.money}}</p>
						<p class="s">{{item.remark }}</p>
					</div>
					</div>
				</li>
				<li v-if='list.length == 0' style='color:#999;text-align:center;'>
                    未查询到相关数据
                </li>
			</ul>
		</div>

    </div>
</template>

<script>

import $ from 'jquery';
      const coinslist = {
          data(){
              return {
                  page:1,
                  list:[],
                  coin:0
              }
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
                    })
              },
              swipe(e){
                if(e.direction=='Up'){
                    this.page++;
                    this.getdata(this.page)
                }
              },
          },
          created() {
              this.getdata(this.page)
                this.coins = this.$store.state.UserAccountInfo.plate_to_return_money + this.$store.state.UserAccountInfo.ex_plate_to_return_money 
                this.coins = parseFloat(this.coins.toFixed(2));
          },
          filters:{
            //设置详情列表滤镜
            timeformat(time){
                coinslist.nowtime=new Date()
                let newtime
                newtime=time.split('T')
                time=newtime[0].split('-')
                let year=time[0]
                let mouth=time[1]
                let day=time[2]
                if(year==coinslist.nowtime.getFullYear() && mouth==coinslist.nowtime.getMonth()+1 && day==coinslist.nowtime.getDate()){
                    time='今日'
                    return time
                }else if(year==coinslist.nowtime.getFullYear() && mouth==coinslist.nowtime.getMonth()+1 && day==coinslist.nowtime.getDate()-1){
                    time='昨日'
                    return time
                }else{
                    time=newtime[0]
                    return time
                }
            }
        },
      }   

      export default  coinslist
</script>