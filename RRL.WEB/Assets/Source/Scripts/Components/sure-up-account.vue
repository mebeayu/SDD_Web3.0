<template>
	<div>
         <div class="back-page-btn" id="carttop" >
        <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
        <p>
                     余额分类
        </p>
        </div>
	    <div class="pay-box" id="payBox">
    	<div class="content">
    		<div class="acc-sty" style="border: none; padding-top: 0;">
    		 <div style="padding: 10px 0;" class="font16">提现金额:</div>
    		 <div>￥<input type="text" value="" v-model="paydata.money" id="money" /><span class="fr" style="color: #BA3027;" v-on:click="getmoney">全部提现</span></div>
    		</div>
    	</div>
    		<div class="type" v-if="this.urldata.type==1">消费奖励:{{this.urldata.money}}</div>
    		<div class="type" v-if="this.urldata.type==2">现金奖励:{{this.urldata.money}}</div>
    		<div class="type" v-if="this.urldata.type==3">推荐奖励:{{this.urldata.money}}</div>
    	<div class="content">
    		<div class="acc-sty">选择银行卡:
    		 <select v-model="paydata.cardid" v-if="this.$store.state.mycard.length>0">
    		 	<option v-for="item in this.$store.state.mycard" v-bind:value="item.id">{{item.card_no}}     {{item.card_holder}}</option>
    		 </select>
    		 <span v-else style="color: #999;">没有银行卡<router-link tag="span" to="/mycardlist" style="padding-left: 10px; color: #aaa;">去设置</router-link></span>
    		 <span class="fr"><img src="/Assets/Source/img/icon-bottom.png"></span>
    		</div>
            <router-link tag="div" to="/mycardlist" class="acc-sty">新增银行卡:<span
                    style="padding-left: 10px; color: #aaa;">去设置</span>
                <span class="fr"><img src="/Assets/Source/img/icon-right.png"></span></router-link>
    		<div class="acc-sty" style="padding-top: 20px;">验证码:
			 <input type="text"  v-model="paydata.sms" value="" />
			 <span class="sms fr" v-on:click="sendsms" v-if="hassend==false">发送验证码</span>
			  <span class="sms" v-else>{{time}}s</span>
			</div>
			<div class="btn-box" v-on:click="surePay"><button >提现</button></div>
    	</div>
    </div>
    <!--<div class="fixed" v-on:click="surePay">提现</div>-->
</div>
</template>

<style scoped>
	.pay-box .content .btn-box {
		height: .3rem;
		line-height: .3rem;
		font-size: .4rem;
		color: #fff;
		border-radius: .1rem;
		background-color: #fff;
	}
	.pay-box .content .btn-box button {
		width: 100%;
		height: 1rem;
		line-height: 1rem;
		font-size: 0.42666667rem;
		background-color: #DB3025;
		color: #FFF;
		text-align: center;
		margin: auto;
		border: none;
		border-radius: .1rem;
	}
</style>

<script>
import $ from 'jquery';
	let Sureupaccount={
		data(){
			return {
				accdata:{
					money:'',
					type:'',
				},
				time:30,
				hassend:false,
				paydata:{
					money:'',
					type:this.$route.query.type,
					cardid:'',
					sms:'',
				},
				urldata:{
					type:this.$route.query.type,
					money:this.$route.query.money,
				}
			}
		},
		created(){
			this.$store.dispatch('getmycard');
		},
		methods:{
			back(){
                this.$router.back(-1)
          },
            sendsms(){
            	var that=this
            	this.$store.dispatch('sendsms',sessionStorage.getItem('myid'))
            	this.hassend=true
                var sh
                sh=setInterval(function(){
                    that.time--
                    if(that.time==0){
                        that.hassend=false
                        clearInterval(sh)
                        that.time=30
                    }
                },1000)
            },
            surePay(){
            	if(this.urldata.money==''||this.paydata.money>this.urldata.money||this.paydata.money==0){
            		if(this.paydata.money>this.urldata.money){
            			alert("余额不足!请重新输入提现金额");
            		}else{
            			alert("请输入正确的提现金额");
            		}
            	}
            	else{
            		if(this.paydata.cardid!=''&&this.paydata.sms!=''){
            			var that =this;
            			this.$store.dispatch('getcash',this.paydata).then(function(){
            				alert("提现成功!");
            				that.$router.back(-1)
            			})
            		}else{
            			alert("银行卡或者验证码不能为空");
            		}

            	}
            },
            getmoney(){
            	this.paydata.money=this.urldata.money
            },
        }
	}
	export default Sureupaccount;
</script>

<style>
</style>