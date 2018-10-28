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
    @media screen and  (orientation: portrait) {
        .agent_int{
            width: 33rem;
            height: 100%;
            transform: rotate(90deg);
            transform-origin:50% 50%;
            margin-left: -23.4rem;
        }
        .agent{
            height: 33rem;
            width: 100%;
            padding: .2rem;
            overflow: hidden;
        }
        .agentdetail_title{
            margin-bottom: .2rem;
        }
        #agent{
            overflow: hidden;
        }
    }
    @media screen and  (orientation: landscape) {
        .agent_int{
            width: 33rem;
            height: 100%;
            padding: .2rem;
        }
        .goback-page{
            display: none;
        }
        .table_head li,.table_body li,.table_foot li{
            height: .6rem!important;
            line-height: .6rem!important;
            width: 2.5rem!important;
        }
        .table_body{
            max-height: 3rem!important  ;
            overflow: auto;
        }
    } 
    #agent{
        width: 100%;
        height: 100%;
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
    .agent_title{
        line-height: .5rem;
    }
    .agent_tips{
        line-height: .8rem;
    }
    .agent_tips span{
        font-weight: bold;
    }
    .table_body li{
        border-color: #999!important;
        color: #666;
    }
    .table_body{
        max-height: 5rem;
        overflow: auto;
    }
    .table_head li,.table_body li,.table_foot li{
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
        width: 2.6rem;
    }
</style>
<template>
    <div id='agent'>
        <div  class="goback-page" @click="goback"><img src="/Assets/Source/img/返回@3x.png"> <p>当月的代理活跃</p></div>
        <div class="agent">
            <div class="agent_int">
                <h3 class="agent_title">{{dataMonth}}月份</h3>
                <div class="table_head">
                    <ul class="clearfix">
                        <li>日期</li>
                        <li>买券金额</li>
                        <li>金豆余额</li>
                        <li>领取红包</li>
                        <li>购物用豆</li>
                        <li>游戏利润的70%</li>
                        <li>纯现金购物</li>
                        <li>购物总金额的7%</li>
                        <li>盈利小计</li>
                        <li>新增用户</li>
                        <li>操作</li>
                    </ul>
                </div>
                <div class="table_body">
                    <ul v-for="(list,index) of item" :key="index" class="clearfix">
                        <li class="first_li">{{list.stat_date}}</li> 
                        <li>{{list.buy_coupons_cash}} 元 </li> 
                        <li>{{list.h_money}}个</li> 
                        <li>{{list.receive_redpackage_money}} 个</li> 
                        <li>{{list.shopping_bean}} 个</li> 
                        <li>{{list.bean_profit}} 元</li>
                        <li>{{list.shopping_cash}} 元</li>
                        <li>{{list.cash_profit}} 元</li>   
                        <li>{{list.profit_total}} 元</li>  
                        <li class="userCount">{{list.userCount}}个</li>  
                        <slot>
                            <li @click="gotoDetail(index)">详情</li>           
                        </slot>
                    </ul>
                </div>
                <div class="table_foot">
                    <ul class="clearfix" v-for="(totallist,index) of totalitem" :key="index">
                        <li>本月累计</li>
                        <li>{{totallist.buy_coupons_cash}}元</li> 
                        <li>{{totallist.h_money}}个</li> 
                        <li>{{totallist.receive_redpackage_money}}个</li> 
                        <li>{{totallist.shopping_bean}}个</li> 
                        <li>{{totallist.bean_profit}}元</li>
                        <li>{{totallist.shopping_cash}}元</li>
                        <li>{{totallist.cash_profit}}元</li>   
                        <li>{{totallist.profit_total}}元</li>  
                        <li>{{totallist.userCount}}个</li>  
                        <li @click='gotoTotalList'>详情</li>
                    </ul>
                </div>  
                <p class="agent_tips">历史盈利状况: <span>{{tips}}</span></p> 
            </div>
        </div>
    </div>
</template>
<script>
import $ from 'jquery';
export default {
    name:'agent',
    data(){
        return{
            token:localStorage.getItem('shorttoken'),
            item:[],    //按天统计所有被推荐人在指定月份内的汇总数据
            totalitem:[],
            tips:"",
            totaluserCount:0,
            statDate:"",
        }
    },
    mounted(){
        this.ProxyStatisticsDaily();
    },
    computed:{
        dataMonth(){
            var date=new Date();
            return date.getMonth()+1;
        }
    },
    methods:{
        //去总共的详情页
        gotoTotalList(){
            this.$router.push({name:'agentdetail',query:{userCount:this.totaluserCount,statDate:this.dataMonth,total:1}});
        },
        //去详情页
        gotoDetail(index){
            var userCount = this.item[index].userCount;
            var stat_date = this.item[index].stat_date;
            this.$router.push({name:'agentdetail',query:{userCount:userCount,statDate:stat_date}});
        },
        //按天统计所有被推荐人在指定月份内的汇总数据
        ProxyStatisticsDaily(){
            var date=new Date();
            var year=date.getFullYear();
            var month=date.getMonth()+1;
            var _this=this;
            console.log(year); 
            $.ajax({
                url:"/WebApi/UserManager/ProxyStatisticsDaily",
                type:'get',
                data:{token:_this.token,year:year,month:month},
                dataType:"JSON",
                success:function(res){
                    if(res.status==0){
                        for(var i in res.data){
                            var data=res.data[i];
                            _this.item.push(data);
                        }
                        _this.totalitem.push(res.additional_data);
                        _this.tips=res.add_data;
                        _this.totaluserCount=res.additional_data.userCount;

                    }
                }
            })
        },
        //返回上一页
        goback(){
            this.$router.push('/my');
        },
        //强制横屏
        landscape(){
           const width = document.documentElement.clientWidth;
           const height=document.documentElement.clientHeight;
           console.log(width);
           var  $agent =  $('#agent');
           if( width < height ){
                $agent.width(height);
                $agent.height(width);
                $agent.css('top',  (height-width)/2 );
                $agent.css('left',  0-(height-width)/2 );
                $agent.css('transform' , 'rotate(90deg)');
                $agent.css('transform-origin' , '50% 50%');
            }
            var evt = "onorientationchange" in window ? "orientationchange" : "resize";
            window.addEventListener(evt, function() {
                var width = document.documentElement.clientWidth;
                var height =  document.documentElement.clientHeight;
                var $agent =  $('#agent');
                if( width > height ){
                    $agent.width(width);
                    $agent.height(height);
                    $agent.css('top',  0 );
                    $agent.css('left',  0 );
                    $agent.css('transform' , 'none');
                    $agent.css('transform-origin' , '50% 50%');
                }
                else{
                    $agent.width(height);
                    $agent.height(width);
                    $agent.css('top',  (height-width)/2 );
                    $agent.css('left',  0-(height-width)/2 );
                    $agent.css('transform' , 'rotate(90deg)');
                    $agent.css('transform-origin' , '50% 50%');
                }
            }, false);
         
        }
    }
}
</script>


