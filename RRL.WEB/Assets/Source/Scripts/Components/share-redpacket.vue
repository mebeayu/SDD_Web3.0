<style lang='scss' scoped>
    .clearfix{
        zoom: 1;
    }
    .clearfix:after{
        content: "";
        clear: both;
        display: block;
        z-index: 999;
    }
    @function rem($px){
        $rem: 75px;
        @return ($px/$rem) + rem;
    }
    @mixin flex {
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        -webkit-box-pack: justify;
        -ms-flex-pack: justify;
        flex-wrap: wrap;
        justify-content: space-between;
        -webkit-box-align: center;
        -ms-flex-align: center;
        align-items: center;
    }
    @mixin rounded-corners($border){
        -moz-border-radius: $border;
        -webkit-border-radius:$border;
        border-radius:$border;
    }
    .goback_gamehall{
        box-sizing: border-box;
        font-size: .45rem;
        color: #2d2d2d;
        text-align: center;
        border-bottom: .02666667rem solid #dcdcdc;
        width: 100%;
        background: #fff;
        position: relative;
        height: 1.2rem;
        line-height: 1.2rem;
        z-index: 8;
        .goback-btn{
            position: absolute;
            left: .2rem;
            top: .1rem;
        }
    }
    .share_rp{
        background: #feebe0;
    }
    .share_rp_top{
        width: 100%;
        height: rem(264px);
        background: url('/Assets/Game/img/rp_hd_02.jpg') no-repeat center top;
        background-size: 100% 100%;
        position: relative;
        .rp_active_rules{
            color: #fff;
            background: #c0453e;
            padding: .2rem .1rem .2rem .3rem;
            min-width: 1.5rem;
            position: absolute;
            text-align: center;
            right: 0;
            top: .5rem;
            border-radius: .5rem 0 0 .5rem;
        }
    }
    .share_rp_bag_box{
        padding: .5rem 0  2rem;
    }
    .share_rp_bag{
        width: rem(621px);
        height: rem(792px);
        margin: 0 auto;
        background: url('/Assets/Game/img/rp_bg_08.png') no-repeat center top;
        background-size: 100% 100%;
        position: relative;
        .self_head{
            position: absolute;
            left: 3.59rem;
            top: .37rem;
            width: rem(80px);
            height: rem(80px);
            @include rounded-corners(50%);
            overflow: hidden;
            img{
                width: 100%;
                height: auto;
            }
        }
        .open_btn{
            position: absolute;
            left: rem(214px);
            top: rem(273px);
            width: rem(190px);
            height: rem(190px);
            @include rounded-corners(50%);
        }
        .share_rp_list{
            position: absolute;
            left: 0;
            bottom:0;
            width: 100%;
            height: 4rem;
            li{
                width: 90%;
                margin:0 auto;
                background: rgba(0,0,0,.2);
                @include rounded-corners(1rem);
                height: 1rem;
                line-height: 1rem;
                @include flex;
                margin-bottom: .5rem;
                color: #fff;
                margin-right: .5rem;
            }
            .share_rp_her_head{
                width: rem(80px);
                height: rem(80px);
                @include rounded-corners(50%);
                overflow: hidden;
                img{
                    width: 100%;
                    height: auto;
                }
            }
            .share_rp_her_p{
                margin-right: .5rem;
            }

        }
    }
        
    .share_rp_open{
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,.5);
        position: absolute;
        top: 0;
    }
    .share_rp_open_box{
        width: rem(513px);
        height: rem(535px);
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        margin: auto;
        background: url('/Assets/Game/img/rp_open_03.png') no-repeat center top;
        background-size: 100% 100%;
        .rp_closeBtn{
            width: rem(60px);
            height: rem(60px);
            position: absolute;
            top: 0;
            right: 0;
            img{
                width: 100%;
                height: 100%;
            }
        }
        .share_rp_p{
            width: 100%;
            text-align: center;
            font-size: .5rem;
            font-weight: bold;
            color: #f60d0e;
            margin-top: 2.7rem;
        }
        .share_rp_btn{
            width: rem(220px);
            height: rem(80px);
            margin: 0 auto;
            margin-top: 1.5rem;
        }
    }

</style>

<template>
   
    <div class="share_red_packet">
        <!--头部开始 -->
        <div class="goback_gamehall">
            <h4>天天领红包</h4>
            <router-link id="goback-btn" class="goback-btn" to='/getredpacket'><img src="/Assets/Game/img/left.png"></router-link>
        </div>
        <!--头部结束-->
        <div class="share_rp">
            <div class="share_rp_top">
                <div class="rp_active_rules"  @click="rp_active_rules">活动规则</div>
            </div> 
            <div class="share_rp_bag_box">
                <div class="share_rp_bag">
                    <div class="self_head"> <img :src="picHead" alt=""></div>
                    <div class="open_btn" @click='openRedPacket'></div>
                    <ul class="share_rp_list">
                        
                        <li v-show='index<=1' v-for="(item,index) in carousel" ref="rollli" :key="index">
                            <p class="share_rp_her_head"><img :src="$store.state.imgs+item.head_pic+$store.state.img_suffix"  :alt="item.username"/></p>
                            <span>{{item.username}}</span>
                            <p class="share_rp_her_p">已经领取了{{item.receive_redpackage_total}}红包</p>
                        </li>
                        <!-- <li>
                            <p  class="share_rp_her_head"><img src="/Assets/Game/img/left.png" alt=""></p>
                            <span>北冥有鱼</span>
                            <p class="share_rp_her_p">已经领取了3000红包</p>
                        </li> -->
                    </ul> 
                </div>
            </div>  
           
        </div>

        <div class="share_rp_open" v-show="share_rp_open">
            <div class="share_rp_open_box">
                <div id="rp_closeBtn" class="rp_closeBtn" @click="closeBtnAuto"><img src="/Assets/Game/img/rp_close_43.png"></div>
                <p class="share_rp_p">{{redpackcount}}红包</p>
                <div @click="share_rp_btn" class="share_rp_btn"></div>
            </div>
        </div>
        
    </div>
</template>

<script>
export default {
    name:'shareredpacket',
    data(){
        return{
            carousel:[],
            share_rp_open:false,
            token:localStorage.getItem("shorttoken"),// 检测是否登陆
            picHead:'',
            redpackcount:'2'
        }
    },
    created(){
        this.RedpackageFissionHistory();
        this.GetUserInfo();
    },
    methods:{
        //跳转到分享页面
        share_rp_btn(){
            localStorage.setItem("hello", this.redpackcount);
            window.location.href='/Event/Newshare?token=' + window.UI.Page.getAllparam().token + '&type=2&redpackcount='+this.redpackcount+'&tid=' + window.UI.Page.getAllparam().taskId;
        },
        //获取用户数据
        GetUserInfo:function(){
            var _this=this;
            $.post("/WebApi/Game/GetUserInfo", {token: this.token},(res)=> {
                if(res.status==0){
                    _this.picHead=res.data.head_pic;
                }   
            })
        },
        //获取用户已经领取了红包的数据
        RedpackageFissionHistory(){
            var _this=this;
            var token= window.UI.Page.getAllparam().token;
            $.ajax({
                url:'/WebApi/game/RedpackageFissionHistory',
                type:'get',
                data:{token: token},
                success:function(res){
                    if(res.status===0){
                        _this.carousel=res.data;
                    }
                }
            })       
        },
        //拆红包
        openRedPacket(){
            var _this=this;
            var token= window.UI.Page.getAllparam().token;
            var taskId= window.UI.Page.getAllparam().taskId;
            $.ajax({
                url:'/WebApi/game/CheckIfOpenFissionRedpackage',
                type:'get',
                data:{token:token,taskId:taskId},
                success:function(res){
                    if(res.status==0){
                        _this.share_rp_open=true;
                    }else{
                        alert(res.message)
                    }
                }
            })
        },
        closeBtnAuto(){
            this.share_rp_open=false;
        },
        //活动规则
        rp_active_rules(){
            confirm(`<ul>
                        <li >1、当红包和金豆总数少于5时，系统会自动发放20红包，可用于继续游戏，每天有二次机会；</li>
                        <li >2、每天二次机会用完后，可通过完成任务，获得更多红包，任务会在二次机会用完后，自动出现；</li>
                        <li >3、用红包进行游戏，获胜则直接变为金豆，金豆可以用于商城购物；</li>
                        <li >4、金豆购物门槛为100，即金豆大于100便可使用金豆尽情购物。</li>
                      </ul>`
            , function(){
            })
        }
    }

}
</script>
