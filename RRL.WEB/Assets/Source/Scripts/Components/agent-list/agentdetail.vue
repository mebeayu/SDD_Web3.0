<style scoped>
    .clearfix{
        zoom: 1;
    }
    .clearfix:after{
        content: "";
        clear: both;
        display: block;
        z-index: 999;
    }
   .goback-page {
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
    }
    .goback-page img{
        position: absolute;
        left: .5rem;width: .3rem;height:auto;margin-top: .3rem;
    }
    #agent{
        width: 100%;
        height: 100%;
    } 
    .agentdetail_title{
        line-height: .5rem;
    }
    @media screen and  (orientation: portrait) {
        .agentdetail_int{
            width: 28rem;
            height: 100%;
            transform: rotate(90deg);
            transform-origin:50% 50%;
            margin-left: -18.4rem;
        }
        .agentdetail{
            height:28rem;
            width: 100%;
            padding: .2rem;
            overflow: hidden;
        }
        .agentdetail_title{
            margin-bottom: .2rem;
        }
        #agentdetail{
            overflow: hidden;
        }
    }
    @media screen and  (orientation: landscape) {
        .agentdetail_int{
            width: 28rem;
            height: 100%;
            padding: .2rem;
        }
        .goback-page{
            display: none;
        }
        .table_head li,.table_body li{
            height: .6rem!important;
            line-height: .6rem!important;
            width: 2.5rem!important;
        }
        .table_body{
            max-height: 3rem!important  ;
            overflow: auto;
        }
    } 
    .table_body li{
        border-color: #999!important;
        color: #666;
    }
    .table_body{
        max-height: 7rem;
        overflow: auto;
    }
    .table_head li,.table_body li{
        float: left;
        height: 1rem;
        line-height: 1rem;
        border: 1px solid #666;
        text-align: center;
        margin-right: .5rem;
        box-sizing: border-box;
        margin-top: -1px;
        padding: 0;
        margin: 0;
        width:2.6rem;
    }
    .agentdetail_people{
        font-weight: bold;
    }
  
   
</style>
<template>
    <div  id='agentdetail'>
        <!--返回上一页开始-->
        <div  class="goback-page" @click="goback"><img src="/Assets/Source/img/返回@3x.png"> <p>当天的代理活跃</p></div>
        <!--返回上一页结束-->
        <div class="agentdetail">
            <div class="agentdetail_int">
                <h3 class="agentdetail_title" v-if="this.$route.query.total">
                    <span class="agentdetail_time">{{date}}月</span>
                    新增用户
                    <span class="agentdetail_people"> {{userCount}}个</span>(根据盈利小计从多到少排序,小计为0的靠后)
                </h3>
                <h3 class="agentdetail_title" v-else>
                    <span class="agentdetail_time">{{date}}</span> 
                    新增用户<span class="agentdetail_people"> 
                    {{userCount}}个</span>(根据盈利小计从多到少排序,小计为0的靠后)
                </h3>
            
                <div class="table_head">
                    <ul class="clearfix">
                        <li>用户</li>
                        <li>买券金额</li>
                        <li>金豆余额</li>
                        <li>领取红包</li>
                        <li>购物用豆</li>
                        <li>游戏利润的70%</li>
                        <li>纯现金购物</li>
                        <li>购物总金额的7%</li>
                        <li>盈利小计</li>
                    </ul>
                </div>
                <div class="table_body">
                    <ul class="clearfix" v-for="(dateUserlist,index) of dateUseritem" :key="index">
                        <li>{{dateUserlist.username}}</li>
                        <li>{{dateUserlist.buy_coupons_cash}} 元</li>
                        <li>{{dateUserlist.h_money}} 个</li>
                        <li>{{dateUserlist.receive_redpackage_money}} 个</li>
                        <li>{{dateUserlist.shopping_bean}} 元</li>
                        <li>{{dateUserlist.bean_profit}} 元</li>
                        <li>{{dateUserlist.shopping_cash}} 元</li>
                        <li>{{dateUserlist.cash_profit}} 元</li>
                        <li>{{dateUserlist.profit_total}} 元</li>
                    </ul>
                </div>   
                
            </div>
        </div>
    </div>
</template>
<script>
import $ from 'jquery';
export default {
    name:'agentdetail',
    data(){
        return{
            token:localStorage.getItem('shorttoken'),
            date:this.$route.query.statDate, //传过来的时间
            userCount:this.$route.query.userCount,  //传过来的下线人数
            dateUseritem:[]
        }
    },
    mounted(){
        if(this.$route.query.total){
            this.ProxyStatisticsByUserMonthly();
        }else{
            this.ProxyDataByDate()
        }
    },
    methods:{
        ProxyStatisticsByUserMonthly(){
            var _this=this;
            var date=new Date();
            var year=date.getFullYear();
            var month=date.getMonth()+1;
            console.log(_this.date);
            $.ajax({
                url:"/WebApi/UserManager/ProxyStatisticsByUserMonthly",
                type:'get',
                data:{token:_this.token,year:year,month:month},
                dataType:"JSON",
                success:function(res){
                   if(res.status==0){
                       for(var i in res.data){
                            var data=res.data[i];
                            _this.dateUseritem.push(data);
                       }
                   } 
                }
            })
        },
        //根据指定的日期，获取所有被推荐人的数据
        ProxyDataByDate(){
            var _this=this;
            $.ajax({
                url:"/WebApi/UserManager/ProxyDataByDate",
                type:'get',
                data:{token:_this.token,date:_this.date},
                dataType:"JSON",
                success:function(res){
                   if(res.status==0){
                       for(var i in res.data){
                            var data=res.data[i];
                            _this.dateUseritem.push(data);
                       }
                   } 
                }
            })
        },
        //返回上一页
        goback(){
            this.$router.push('/agent')
        },
        //强制横屏
        landscape(){
           const width = document.documentElement.clientWidth;
           const height=document.documentElement.clientHeight;
           var  $agentdetail =  $('#agentdetail');
           if( width < height ){
                $agentdetail.width(height);
                $agentdetail.height(width);
                $agentdetail.css('top',  (height-width)/2 );
                $agentdetail.css('left',  0-(height-width)/2 );
                $agentdetail.css('transform' , 'rotate(90deg)');
                $agentdetail.css('transform-origin' , '50% 50%');
            }
            var evt = "onorientationchange" in window ? "orientationchange" : "resize";
            window.addEventListener(evt, function() {
                var width = document.documentElement.clientWidth;
                var height =  document.documentElement.clientHeight;
                var $agentdetail =  $('#agentdetail');
                if( width > height ){
                    $agentdetail.width(width);
                    $agentdetail.height(height);
                    $agentdetail.css('top',  0 );
                    $agentdetail.css('left',  0 );
                    $agentdetail.css('transform' , 'none');
                    $agentdetail.css('transform-origin' , '50% 50%');
                }
                else{
                    $agentdetail.width(height);
                    $agentdetail.height(width);
                    $agentdetail.css('top',  (height-width)/2 );
                    $agentdetail.css('left',  0-(height-width)/2 );
                    $agentdetail.css('transform' , 'rotate(90deg)');
                    $agentdetail.css('transform-origin' , '50% 50%');
                }
            }, false);
         
        }
    }
}
</script>


