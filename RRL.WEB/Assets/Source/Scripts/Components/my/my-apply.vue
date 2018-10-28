<template>
	<div class="wallet-box">
		<div class="back-page-btn" id="carttop">
		        <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
		        <p>
		           提现申请记录
		        </p>
		</div>		
	    <div class="c" style="padding-top: 10px;">
	    <div class="title fl" style="width: 40%;">申请时间</div>
	    <div class="title fl" style="width: 30%;">提现类型</div>
	    <div class="title fl" style="width: 30%;">金额</div>
	    </div>
	    <div class="content" v-finger:swipe="swipe">
	    	<ul class="apply">
	    		<li v-for="item in this.$store.state.apply" class="c">
	    			<div class="left fl">
	    				<walletime :addtime="item.addtime"></walletime>
	    			</div>
	    			<div class="center fl">
	    				<span v-if="item.type==1">消费提现</span><span v-else-if="item.type==2">现金提现</span><span v-else>推荐提现</span>
	    			</div>
	    			<div class="right fl">{{item.apply_money}}</div>
	    		</li>
	    	</ul>
	    </div>		
	</div>
</template>

<script>
	import walletime from './walletime';
	let myapply={
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
			this.$store.dispatch('getapply',this.applydata);
		},
		methods:{
			back(){
				this.$router.go(-1)
			},
			 //上划分页
            swipe(e){
                if(e.direction=='Up'){
                this.applydata.page++;
                 this.$store.dispatch("getcatshop",this.applydata)
                }
            },
		}
		
	}
	export default myapply
</script>

<style>
</style>