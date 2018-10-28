<!-->商品详情banner及头部导航<-->
<template>
<div class="goods-detail-header">
    <div v-bind:style="{width:goodsbanner.length*100+'%' ,marginLeft:-imgindex*this.$store.state.screenWidth+'px' }">
        <img v-for="(item,index) in goodsbanner"
            v-bind:src="$store.state.imgs+item+'.jpg'"
            v-bind:index="index"
            v-finger:swipe="swipe"
            v-bind:style="{width:$store.state.screenWidth+'px'}" 
            :onerror="$store.state.defautbanner"
            :load='$store.state.defautbanner'/>
        <!--img v-bind:src="$store.state.img+$store.state.goodsdetail.goodsdetail.pic_id" v-if="goodsbanner.length==0" /-->
    </div>
        <div class="top-nav">
            <div>
                <img src="/Assets/Source/img/goback.png" v-on:click="back" />
                <img src="/Assets/Source/img/顶部分享@3x.png" v-on:click="showshare" />
                <ul class="goods-showbox" v-if="showbox==true">
                    <li v-on:click="share">分享</li>
                    <li v-on:click="kefu">帮助</li>
                    <li v-on:click="kefu">反馈</li>
                    <router-link tag="li" to="/index">首页</router-link>
                </ul>
            </div>
        </div>
        <ul class="focus-dot">
            <li v-for="(item,index) in goodsbanner"
                v-bind:class="index==imgindex?'beenchose':''"
                @click="changebanner(index)"
                ></li>
        </ul>
    </div>
</template>
<script>
    let goodsheader={
        data(){
            return {
                showbox:false,
                imgindex:0 
                
            }
        },
        props:[
            'goodsbanner','goodsdetail'
        ],
        created(){
            //获取屏幕宽度
            this.$store.commit('getscreenwidth')
        },
        mouted()
        {
        	
        },
        methods:{
        	//
        	getGoodsdetail()
        	{
        		return this.goodsdetail;
        	},
        	
            //点击焦点切换banner
            changebanner(index){
                //this.$store.commit('clickbannerdot',index)
                this.imgindex = index
            },
            //滑动切换banner
            swipe(e){
                // this.$store.commit('changegoodsbanner',e.direction)
                if (e.direction == 'Left') {
                    this.imgindex++
                    if (this.imgindex > this.goodsbanner.length - 1) {
                        this.imgindex = 0
                    }
                } else if (e.direction == 'Right') {
                    this.imgindex--
                    if (this.imgindex < 0) {
                        this.imgindex = this.goodsbanner.length - 1
                    }
                }
            },
            //返回
            back(){
                this.$router.back(-1)
            },
            //分享下拉框显示
            showshare(){
            	
                this.showbox=!this.showbox
            },
            //客服弹出框
            kefu(){

                this.$store.commit('setkefushow',true)
            },
            //分享弹出框
            share(){
            	var username=localStorage.getItem('username');
            	if(!username)
            	{
            		username="";
            	} 
            	 var ua = window.navigator.userAgent.toLowerCase();
            	
            	 var goodsdetail=this.getGoodsdetail();
		         if (ua.match(/MicroMessenger/i) == 'micromessenger') {
		            alert('请点击顶部导航栏的右上角分享');
		         }else
		         {
		         	 var redirect=('https://e-shop.rrlsz.com.cn/'+ location.hash+'?reference='+username);
		         	if (ua.indexOf('iphone') > -1) {
                        redirect = encodeURIComponent(redirect);
                    }
		         	location.href=`/Event/SharingGuide?title=${encodeURIComponent(goodsdetail.name)}&recommend=${encodeURIComponent(goodsdetail.propaganda)}&pic_id=${encodeURIComponent(goodsdetail.pic_id)}&redirect=${redirect}`;
		        }
            }
      }
    }
    export {goodsheader as default}
</script>