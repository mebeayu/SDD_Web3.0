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
    .red_packet_main{
        background: #c0453e;padding: 0 0 0.5rem;
    }
    .rp_banner{
        width: 100%;
        height:rem(211px);
        background: url('/Assets/Game/img/rp_02.png') no-repeat center top;
        background-size:100%  100%;
        position: relative;
        .rp_active_rules{
            color: #c0453e;
            background: #f8e9d2;
            min-width: 2rem;
            height: .9rem;
            line-height: .9rem;
            position: absolute;
            text-align: center;
            right: 0;
            top: .5rem;
            border-radius: .5rem 0 0 .5rem;
        }
        .rp_carousel_box{
            position: absolute;
            left: 0;
            bottom: .5rem;
            min-width: 2.5rem;
            height: 1.5rem;
            overflow: hidden;
            .rp_carousel{
                @include flex;
                margin-bottom: .3rem;
                background: rgba(0, 0, 0, 0.2);
                padding: .2rem;
                 @include  rounded-corners(0 1rem 1rem 0);
                .carousel_img{
                    width: 1rem;
                    height: 1rem;
                    @include  rounded-corners(50%);
                    overflow: hidden;
                    background: plum;
                    float: left;
                    img{
                        width: 100%;
                        height: auto;
                    }
                }
                .carousel_con{
                    margin-left: .1rem;
                    float: left;
                    h4{
                        color: #fff;
                        font-size: .35rem;
                    }
                    p{
                        color: #edb714;
                        font-size: .32rem;
                    }
                }
            }
        }
    }
    .rp_sign_in{
        width: 90%;
        margin: 0 auto;
        padding: .2rem;
        @include  rounded-corners(.2rem);
        background: #f8e9d2;
        .sign_in_content{
            border: 1px solid #eccfa2;
            @include  rounded-corners(.2rem);
            li{
                padding: 0 .15rem;
                margin-bottom: .5rem!important;
            }
            .sign_in_title{
                border-bottom: 1px dashed #eccfa2;
                height: 1.5rem;
                line-height: 1.5rem;
                width: 88%;
                margin: 0 auto;
                @include flex;
                span{
                    font-size: .5rem;
                    color: #333;
                    font-weight: bold;
                    em{
                        color: #b63a33;
                    }
                }
                img{
                    width: 1.3rem;
                    height:1.37rem ;
                }
            }
            .sign_in_time{
                margin-left: .3rem;
                @include flex;
                span{
                    display: block;
                    width: .8rem;
                    height: .8rem;
                    background: #f8e9d2;
                    border: 1px solid #eccfa2;
                    text-align: center;
                    line-height: .8rem;
                    font-size: 0.35rem;
                     color: #333;   
                }
            }
            .sign_in_days{
                @include flex;
                span{
                    display: block;
                    width: .8rem;
                    height: .8rem;
                    @include  rounded-corners(50%);
                    background: #bfbfbf;
                    text-align: center;
                    line-height: .8rem;
                    color: #fff;
                }
                .current_sign{
                    background: #cfae7c;
                }
               
            }
            .sign_in_list{
                border: 1px dashed #eccfa2;
                width: 65%;
                padding: .2rem .3rem;
                margin: 0 auto;
                color: #333;
                p{
                    line-height: .6rem;
                }
            }
            .sign_in_btn{
                display: block;
                width: rem(369px);
                height: rem(104px);
                background: url('/Assets/Game/img/rp_05.png');
                background-size:100%  100%;
                margin: 0 auto;
                color: #fff;
                text-align: center;
                line-height:  rem(104px);
                font-size: .4rem;
                border: none;
            }
            .firstShare{
                color: #333;
                font-size: .35rem;
                text-align: center;
            }

        }

    }
    .rp_record{
        width: 90%;
        margin: 0.25rem auto 0;
        padding: .2rem;
        @include  rounded-corners(.2rem);
        background: #f8e9d2; 
        .ro_record_title{
            float: left;
            width: 1.13rem;
            height: .92rem;
            margin-top: .29rem;
            margin-right: .2rem;
            img{
                width: 100%;
                height: 100%;
            }
        }
        dl{
            dd{
                float: left;
                width: 1.4rem;
                height: 1.4rem;
                @include  rounded-corners(50%);
                margin-bottom: .2rem;
                overflow: hidden;
                margin-left: .12rem;
                img{
                    width: 100%;
                    height: 100%;
                }
            }
        }
    }
    .rp_ad{
        width: 100%;
        height: rem(57px);
        margin: .25rem 0;
        img{
            width: 100%;height: auto;
        }
    }
    .rp_recommend{
        width: 90%;
        margin: 0.25rem auto 0;
        padding: .2rem;
        @include  rounded-corners(.2rem);
        background: #fff; 
        ul{
            width: 100%;
            @include flex;
        }
        li{
            width: 50%;
            font-size: .36rem;
            .rp_recommend_pic{
                width: 4.2rem;
                height: 4.2rem;
                margin-top: .266667rem;
                overflow: hidden;
                margin: 0 auto;
                img{
                    width: 100%;
                    height: 100%;
                }
            }
            .rp_recommend_price{
                line-height: .8rem;
                color: #f91434;
                font-weight: bold;
                font-size: .5rem;
            }
        }
    }
    .get_red_packet  #shadow1 .tiplogout {
        position: fixed;
        top: 10%!important;
    }
</style>

<template>
    <div class="get_red_packet">
        <!--头部开始 -->
        <div class="goback_gamehall">
            <h4>天天领红包</h4>
            <router-link  to="/gamehall" class="goback-btn" ><img src="/Assets/Game/img/left.png"></router-link>
        </div>  
        <!--头部结束-->
        <div class="red_packet_main">
            <div class="rp_banner">
                <div class="rp_active_rules" @click="rp_active_rules">活动规则</div>
                <ul class="rp_carousel_box">
                    <li class="rp_carousel" v-for="(item,index) in carousel" ref="rollli" :key="index" >
                        <div class="carousel_img">
                            <img :src="$store.state.imgs+item.head_pic+$store.state.img_suffix"  :alt="item.username"/>
                            </div>
                        <div class="carousel_con">
                            <h4>{{item.username}}</h4>
                            <p>已拆得{{item.receive_redpackage_total}}红包</p>
                        </div>
                    </li>
                 </ul>
            </div>
            
            <!-- 签到 -->
            <div class="rp_sign_in">
                <ul class="sign_in_content">
                    <li class="sign_in_title">
                        <span>已拆得 <em>{{receivedMoney}}红包</em></span>
                        <img src="/Assets/Game/img/rp_20.png" alt="">
                    </li>
                    <li class="sign_in_time">
                        <span>{{hours}}</span>:
                        <span>{{minutes}}</span>:
                        <span>{{seconds}}</span>后红包将失效，赶紧拆～
                    </li>
                    <li class="sign_in_days">
                        <span class="current_sign" v-for="i in receivedCount">{{i}}</span>
                        <span  v-for="i in 10-receivedCount">{{receivedCount+i}}</span>
                    </li>   
                    <li class="sign_in_list">
                        <p>邀请满3个用户注册可得 10个红包</p>
                        <p>邀请满5个用户注册可得 20个红包</p>
                        <p>邀请满8个用户注册可得 35个红包</p>
                        <p>邀请满10个用户注册可得 50个红包</p>
                    </li>
                    <li ><button  class="sign_in_btn"  id="signInBtn" :disabled='signInBtn'>{{btnCon}}</button></li>
                    <!-- <li class="firstShare"><p v-show="firstShare">每天首次分享得100红包</p></li> -->
                </ul>  
            </div>
            
            <!-- 红包记录 -->
            <div class="rp_record">
                <div class="ro_record_title">
                    <img src="/Assets/Game/img/rp_jl_03.png" alt="红包记录">
                </div>
                <dl  class="clearfix" v-if="pics===null">
                    <dd  v-for="i in 10"  >
                        <img src="/Assets/Game/img/rp_95.png" alt="">
                    </dd>
                </dl>
                <dl  class="clearfix"  v-else>
                    <dd v-for="pic of pics">
                        <img :src="$store.state.imgs+pic.head_pic+$store.state.img_suffix" alt="">
                    </dd>
                    <dd v-for="i in 10-pics.length" class="signInBtn" >
                        <img src="/Assets/Game/img/rp_95.png" alt="">
                    </dd>
                </dl>
              
            </div>

            <div class="rp_ad"><img src="/Assets/Game/img/rp_04.png" alt="天天领红包"></div>
            <div class="rp_recommend">
                <ul>
                    <router-link    
                        tag="li"
                        v-for="item in sentimentRecList"
                        :key="item.id"
                        :to="{name:'goodsdetail',params: { goodsid: item.id ,username:'null'  }}"
                    >
                        <div  class="rp_recommend_pic">
                            <img  :src="'SWebApi/Public/picture/'+item.pic_id+'.jpg'"
                                  :load="'/Assets/Source/img/默认banner.png'"/>
                        </div> 
                        <p>{{item.name}}</p> 
                        <p  class="rp_recommend_price">￥{{item.price}}</p>
                    </router-link>

                    <!-- <li>
                        <div  class="rp_recommend_pic">
                            <img  src="SWebApi/Public/picture/90628.jpg" load="/Assets/Source/img/默认banner.png">
                        </div> 
                        <p>【爆款】公牛USB插座接线板智能插排转换器插线板拖线板插板1.5米  UUA 123</p> 
                        <p  class="rp_recommend_price">￥59.9</p>
                    </li>
 -->
                </ul>
            </div>

        </div>
         <!--分享区域开始-->
        <div class="hboxfixed" v-show="hbox">
            <div class="shaw"></div>
            <div class="img"  @click="this.hbox=false"><img src="/Assets/Source/img/hbox.png"/></div>
        </div>
        <!--分享区域结束-->
       
    </div>
</template>

<script>
export default {
    name:'getredpacket',
    data(){
        return{
            hbox:false,
            show: true,
            sentimentRecList:[],
            pics:[],
            token:localStorage.getItem("shorttoken"),// 检测是否登陆
            carousel:[],
            btnCon:'领取任务',
            receivedMoney:0,
            days:'00',
            hours:'00',
            minutes:'00',
            seconds:'00',
            receivedCount:'',
            firstShare:true,
            isCanReceivedTask:0,
            signInBtn:false,
        }
    },
    created(){
        this.rpFissionTaskData();
        this.RedpackageFissionHistory();
        this.getRecomList();
    },
    mounted(){
        setInterval(this.rpScroll,3000);  
    },
    methods:{
        //获取用户已经领取了红包的数据
        RedpackageFissionHistory(){
            var _this=this;
            $.ajax({
                url:'/WebApi/game/RedpackageFissionHistory',
                type:'get',
                data:{token:this.token},
                success:function(res){
                    if(res.status===0){
                        _this.carousel=res.data;
                    }
                }
            })       
        },
        //领取新的红包裂变任务
        ReceiveRedpackageFissionTask(){
            var _this=this;
            $.ajax({
                url:'/WebApi/game/ReceiveRedpackageFissionTask',
                type:'get',
                data:{token:_this.token},
                success:function(res){
                    if(res.status===0){
                        var title = encodeURIComponent("关系铁我才告诉你，我在这里天天领50红包");
                        var remark = encodeURIComponent("天天领50红包，全场商品任意购。");
                        var redirect = location.origin + "/Event/OpenFissionRedpackage?token=" + _this.token + "&taskId=" + res.data;
                        window.location.href = "/event/RedpackageFissionShare?token=" + _this.token + "&title=" + title + "&remark=" + remark + "&redirect=" + encodeURIComponent(redirect);     
                    }else{
                        alert(res.message)
                    }
                }
            })
        },
        //获取用户红包裂变任务数据
        rpFissionTaskData(){
            var _this=this;
            $.ajax({
               url:'/WebApi/game/UserRedpackageFissionTaskData',
               type:'get',
               data:{token:_this.token},
               success:function(res){
                    if(res.status===0){
                       var data=res.data;
                        _this.receivedMoney=data.receivedMoney;  //已拆得的红包资金总额
                        _this.formatDuring(data.countdown);    //用于表示倒计时开始的秒数
                        _this.isCanReceivedTask=data.isCanReceivedTask;
                        var signInBtn=document.getElementById('signInBtn');
                        if(data.isCanReceivedTask===1){
                            signInBtn.onclick=function(){
                                _this.rpFissionTaskData();
                                _this.ReceiveRedpackageFissionTask();
                            };
                        }else{
                            signInBtn.onclick=function(){
                                var title = encodeURIComponent(" 关系铁我才告诉你，我在这里天天领50红包");
                                var remark = encodeURIComponent("天天领50红包，全场商品任意购。");
                                var redirect = location.origin + "/Event/OpenFissionRedpackage?token=" + _this.token + "&taskId=" + data.taskid;
                                window.location.href = "/event/RedpackageFissionShare?token=" + _this.token + "&title=" + title + "&remark=" + remark + "&redirect=" + encodeURIComponent(redirect);  
                            };
                        }
                        if(data.receivedCount>10){
                            data.receivedCount==10;
                        }  
                        _this.receivedCount=data.receivedCount;  //已拆得的红包个数
                        switch (data.taskStatus) {
                            case 1:
                                _this.btnCon='领取任务';
                                _this.receivedMoney=0;
                                break;
                            case 2:
                                 _this.btnCon='重新领取';
                                 break;
                            case 3:       
                                _this.btnCon='分享后继续拆红包';
                                 break;
                            default:
                                _this.btnCon='已完成，明天再来';
                                _this.signInBtn=true;
                        }
                       _this.pics=data.userList;
                   }
               }
            })   
        },
        //时间处理
        formatDuring(params) {
            var timer=setInterval(()=>{
                if(params>0){
                    var  days = parseInt(params / (60 * 60 * 24));
                    var  hours = parseInt(params / (60 * 60)) - (days * 24);
                    var  minutes = parseInt(params / 60) - (days * 24 * 60) - (hours * 60);
                    var  seconds = parseInt(params) - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60);
                }
                this.hours=this.duringAdd0(hours);
                this.minutes=this.duringAdd0(minutes);
                this.seconds=this.duringAdd0(seconds);
                params--;
               
            },1000)
            if(params <=0){
                clearInterval(timer);
            }
        },
        duringAdd0(m){
            if (m <= 9) {
                return '0' + m;
            }
            return m
        },
        //图片滚动
        rpScroll(){
            let rollli= this.$refs.rollli; 
            for(var i in rollli){
                rollli[i].style.marginTop='.5rem'
            }
            var that = this; 
            setTimeout(function(){
            	if(that.carousel && that.carousel.length > 0){
	                that.carousel.push(that.carousel[0]);
	                that.carousel.shift();
	                rollli[0].style.marginTop='0';
               	}
            },0)
        },
        //获取人气推荐
        getRecomList(){
            var _this=this;
            $.ajax({
                url: "/SWebApi/ShopManager/GetGoodsTypeList/1.json?pagesize=6",
                type:'get',
                dataType:'json',
                success:res =>{
                    if(res.status==0){
                        _this.sentimentRecList=res.data.PopData;
                    }
                }
            })
        },
        /**
        * 点击弹出提示框
        */
        rp_active_rules(){
            confirm(`<ul>
                        <li style='font-size:.35rem;'>1、红包获得后，任务有效期为24小时，过期失效后需要重新领取，任务有效期内不可领取新的任务，每个用户24小时内仅可以领取1次任务；</li>
                        <li style='font-size:.35rem;'>2、获得红包后，需要分享并邀请好友注册下载省兜兜APP才可拆开；</li>
                        <li style='font-size:.35rem;'>3、邀请好友拆得红包金额总量为：邀请3个累计获得10红包，邀请5个累计获得20红包，邀请8个累计获得35红包，邀请10个累计获得50红包，邀请3个以下无红包奖励；</li>
                        <li style='font-size:.35rem;'>4、如果用户存在违规行为（包括但不限于洗钱、虚假交易、赌博、恶意套现、作弊、刷信誉），主办方将取消用户的活动资格、并有权撤销相关违规交易、收回奖励（包括已消费金额）等利益，同时依照相关规则进行处罚。</li>
                        <li style='font-size:.35rem;'>4、如出现不可抗力或情势变更的情况（包括但不限于重大灾害事件、活动受政府机关指令需要停止举办或调整的、活动遭受严重网络攻击或因系统故障需要暂停举办的），则主办方可依相关法律法规的规定主张免责。</li>
                      </ul>`
            , function (){
            })
        }
    }
}
</script>
