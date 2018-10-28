<template>
	<div class="wallet-box">
		<div class="back-page-btn" id="carttop">
		        <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
		        <p>
		           账单列表
		        </p>
		</div>
	    <div class="c" style="padding-top: 10px;">
	    <div class="title fl" style="width: 40%;">消费时间时间</div>
	    <div class="title fl" style="width: 30%;">消费类型</div>
	    <div class="title fl" style="width: 30%;">金额</div>
	    </div>
	    <div class="content" v-finger:swipe="swipe">
	    	<ul class="apply">
	    		<li v-for="item in this.$store.state.billlist" class="c">
	    			<div class="left fl">
	    				<walletime :addtime="item.addtime"></walletime>
	    			</div>
	    			<div class="center fl">{{item.remark}}</div>
	    			<div class="right fl">{{item.money}}</div>
	    		</li>
	    	</ul>
	    </div>				
	</div>
</template>

<script>
import walletime from './walletime';
	let mybill={
		data(){
			return{
				applydata:{
					page:1,
					size:20
				}
			}
		},
		components:{
			walletime
		},		
		created(){			
			this.$store.dispatch('getbilllist',this.applydata)
		},
		methods:{
			back(){
				this.$router.go(-1)
			},
			 //上划分页
            swipe(e){
                if(e.direction=='Up'){
                this.applydata.page++;
                 this.$store.dispatch("getbilllist",this.applydata)
                }
            },
		}
				
	}
	export default mybill
</script>

<style>
</style>