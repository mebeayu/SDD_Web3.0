<template>
	<div class="mycard-box">
		<div class="back-page-btn" id="carttop">
		    <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
		    <p>我的银行卡 </p>
		</div>
		<div class="mycard">
			<ul>
				<li v-for="item in this.$store.state.mycard" class="c">
					<div class="left fl">
						<p class="bank-name">{{item.card_holder}}</p>
						<p class="bank-card">{{item.card_no}}</p>
					</div>
					<div class="right fl">
						<p class="btn" v-on:click="openremove(item.id)" v-bind:cardid="item.id">移除</p>
					</div>
				</li>
			</ul>
		</div>
		<div class="create-mycard" v-on:click="createcard($event)">+添加银行卡</div>
		<div class="create-box" id="createBox">
			<div class="list-sty">选择银行:
			<select v-model="createData.bank">
				<option v-for="item in this.$store.state.bank" v-bind:value="item.bank_en" >{{item.bank_name}}</option>
			</select>
			</div>
			<div class="list-sty">输入用户名:
			 <input type="text" v-model="createData.name" value=""  />
			</div>
			<div class="list-sty">输入银行卡号:
			 <input type="text" v-model="createData.card" value="" />
			</div>
			<div class="list-sty" style="padding-top: 20px;">验证码:
			 <input type="text"  v-model="createData.sms" value="" />
			 <span class="sms" v-on:click="sendsms" v-if="hassend==false">发送验证码</span>
			  <span class="sms" v-else>{{time}}s</span>
			</div>
			<div class="btn-box">
				<button v-on:click="turecreated">提交</button>
			</div>
		</div>
		<div class="remove-card create-box" id="removeCard">
			<div class="list-sty" style="padding-top: 20px;">验证码:
			 <input type="text"  v-model="romovecard.sms" value="" />
			 <span class="sms" v-on:click="sendsms" v-if="hassend==false">发送验证码</span>
			  <span class="sms" v-else>{{time}}s</span>
			</div>
			<div class="btn-box">
				 <button v-on:click="close" style="margin-right: 20px;background-color: #ccc;">取消</button><button v-on:click="removecard">确定</button>
			</div>
		</div>
	</div>
</template>

<script>
	import $ from 'jquery';
	let mycardlist={
		data(){
			return{
				createData:{
					bank:'',
					name:'',
					card:'',
					sms:'',
				},
				hassend:false,
				time:30,
				romovecard:{
					cardid:'',
					sms:'',
				}
				
			}
		},
		created(){
			this.$store.dispatch('getbank');
			this.$store.dispatch('getmycard');
		},
		methods:{
			createcard(e){
				var el =e.target;
				$(el).css({"display":"none"});
				$("#createBox").css({"display":"block"});
			},
			back(){
                this.$router.go(-1)
            },
            turecreated(){
            	if(this.createData.bank!=''&&this.createData.name!=''&&this.createData.card!=''&&this.createData.sms!=''){
            		this.$store.dispatch('createbankcard',this.createData)
            	}
            	else{
            	    alert("所属银行,持卡人姓名，卡号，验证码不能为空!")
            	}
            	
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
            openremove(data){
            	$("#removeCard").css({"display":"block"});
            	this.romovecard.cardid=data;
            },
            close(){
            	$("#removeCard").css({"display":"none"});
            },
            removecard(){
            	if(this.romovecard.sms!=''){
            		this.$store.dispatch('removebankcard',this.romovecard)
            	}else{
            		alert("请输入验证码！")
            	}
            	
            },
		}
	}
	export default mycardlist
</script>