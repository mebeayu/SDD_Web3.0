<!-->物流详情<-->
<template>
    <div style="background:#f5f5f5; height:100%;">
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
            <p>快递详情</p>
        </div>
        <div  class='transdetail-info' id="af-title">
            <!--<img src="/Assets/Source/img/时间@3x.png"/>-->
            <div>
                <!--<h2>-->
                    <!--已签收-->
                <!--</h2>-->
                <p>快递单号：{{ express }}</p>
                <p>快递名称：{{ courierNumber }}</p>
            </div>
        </div>
        <!--<div class="buyaddress" style='margin-bottom:0'>
            <img src='/Assets/Source/img/地址的副本@2x.png'>
            <div>
                <p>
                    <span>收货人:{{this.$store.state.orderdetail.receive_name}}</span>
                    <span>{{this.$store.state.orderdetail.receive_mobile}}</span>

                </p>
                <p style="white-space:normal;word-wrap:break-word;word-break:break-all" >收货地址:{{this.$store.state.orderdetail.receive_address}}</p>
            </div>
        </div>
        <div class='border-top'>
            <i></i>
        </div>-->
        <ul class="trans-ul" >
            <li class="newtransinfo" v-for="(item,index) in msg">
                <p>{{ item.context }}</p>
                <p>{{ item.time  | timeformat}}</p>
            </li>
        </ul>
        <div class='notransinfo' v-if='false'>
            <img src='/Assets/Source/img/not.png'/>
            <p>暂无物流信息</p>
        </div>
    </div>
</template>

<style scoped>
    .transdetail-info{
        padding: 1.8rem 0 .2rem .2rem;
        /*border-bottom: 1px solid rgba(0,0,0,.2);*/
    }
    .trans-ul {
        padding: 0.2rem;
    }

    .trans-ul li{
        padding: .18rem 0;
        border-top: 1px dotted rgba(0,0,0,.1);
    }
    .transdetail-info p,.trans-ul li p{
        color: #555;
        font-size: .45rem;
    }
    .trans-ul li p{
        color: #666;
    }
</style>

<script>
  /*import bus from "../bus.js";*/

  let transdetail = {
    name:"transdetail",
    data() {
      return {
        msg: [],
        express:"",
        courierNumber:""
      }
    },
    methods:{
      //返回页面
      back(){
        this.$router.go(-1)
      }
    },
    mounted(){
      let list = JSON.parse(localStorage.getItem("transdetailInfo"));
      this.msg = list.data;
      this.express = list.com;
      this.courierNumber = list.nu;
    },
    filters:{
      timeformat(time) {
        return time.replace(/T/g,' ')
      }
    }
  }


  export default transdetail
</script>