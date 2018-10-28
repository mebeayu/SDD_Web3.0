<!-->商品详情页操作弹出框 belong to 商品详情<-->
<template>
    <div class="game_pop">
        <div class="pop_box">
            <i id="customAlert_close" class="customAlert_close" @click='return_game'> × </i>
            <div class="customAlert_content">
                完成指定任务,获得更多红包
            </div>
            <div class='pop_btns'>
                <div class='pop_btn popSurebtn' id="popSurebtn" @click='goto_gamehall'>返回大厅做任务</div>
                <div class='pop_btn popcancelbtn' id="popcancelbtn" @click='buy_gold_bean'>我是土豪直接充值</div>
            </div>
        </div>
    </div>
</template>

<script>
    let alertbox={
        data(){
            return{
                token:localStorage.getItem("shorttoken")
            }
        },
        methods:{
            goto_gamehall(){
                window.location.href = "/#/gamehall?flag=1&token=" + encodeURIComponent(this.token);
            },
            buy_gold_bean(){
            	let redirectUrl = window.UI.Page.getAllparam().redirect;
            	if(redirectUrl){
            		window.location.href = "/Game/PeasPay?token=" + encodeURIComponent(this.token) + "&redirect=" + encodeURIComponent(redirectUrl);
            	}
            	else {
            		window.location.href = "/Game/PeasPay?token=" + encodeURIComponent(this.token) + "&redirect=/#/gamehall?flag=1";	
            	}
            },
            return_game(){
            	let redirectUrl = window.UI.Page.getAllparam().redirect;
            	if(redirectUrl){
            		window.location.href = redirectUrl; 
            	}
            	else {
            		window.location.href = "/#/gamehall?flag=1token=" + encodeURIComponent(this.token)	
            	}
            }
        }
    }
    export default alertbox
</script>
<style scoped>
        * {
            margin: 0;
            padding: 0;
        }
        body {
            position: relative;
            width: 100%;
            height: 100%;
        }
        .game_pop{
            width: 100%;
            height: 100%;
            background:rgba(0,0,0,.5);
            position: fixed;
            top: 0;
            left: 0;
        }
        .pop_box{
            width: 80%;
            height: 3rem;
            background: #fff;
            border-radius: .3rem;
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            margin:auto;
            color: #333;
            margin: auto;
            padding: .5rem 0;
            text-align: center;
        }
        .customAlert_close{
            color: #666;
            font-size: .8rem;
            position: absolute;
            top: 0;
            right: .3rem;
            font-style: normal;
        }
        .pop_btns{
            display: flex;
            justify-content: space-around;
            align-items: center;
        }
        .pop_btn{
            width:39%;
            border-radius: .2rem;
            font-size: .32rem;
            text-align: center;
            padding: .25rem .1rem;
        }
        .popSurebtn{
            border: 1px solid #666;
        }
        .popcancelbtn{
            background:#ba1b18;
            color: #fff;
        }
        .customAlert_content{
            height: 70%; 
            font-size: .38rem; 
            display: flex;
            justify-content: space-around;
            align-items: center;
            color: #333;
        }
        
</style>
