<!-->添加收货地址 belong to 订单创建<-->
<template>
    <router-link to="/choseadd" tag="div" class="buyaddress">
            <img src='/Assets/Source/img/定位@3x.png'>
            <div class="address-details">
                <p>
                    <span class="consignee" v-if="this.$store.state.receive_info.name!=undefined"><strong>收货人：</strong>{{this.$store.state.receive_info.name}}</span>
                    <span v-if="this.$store.state.receive_info.name!=undefined"><strong>联系电话：</strong>{{this.$store.state.receive_info.mobile}}</span>
                    <span v-else style="color:#ba3027">请添加收货地址</span>
                        <img src='/Assets/Source/img/展开@3x.png'
                            style="width:10px"
                            v-if="hash!='#/orderdetail'">
                    <span class="shipp-address" style="white-space:normal" v-if="this.$store.state.receive_info.name!=undefined"><strong>收货地址：</strong>{{this.$store.state.receive_info.address}}</span>
                </p>
            </div>
        </router-link>
</template>

<style scoped>
    .buyaddress{
        position: relative;
    }
    .buyaddress div{
        width: 87%;
    }
    .buyaddress img{
        width: .5rem!important;
        height: .5rem;
    }
    .address-details p img{
        position: absolute;
        top: 50%;
        right: .2rem;
        transform: translateY(-50%);
        width: .23rem!important;
        height: .37rem;
    }
    .address-details p strong{
        color: #888;
        font-weight: normal;
        display: inline-block;
        width: 26%;
        font-size: .38rem;
    }
    .address-details p{
        display: block;
    }
    .address-details span{
        display: block;
        line-height: .6rem;
        font-size: .38rem;
        white-space: normal;
    }
</style>

<script>
    let add={
        data(){
            return {
                hash:'',
                add:{}
            }
        },
        created(){
            //获取默认收货地址
            this.hash=location.hash
            if(this.$store.state.receive_info.id==''){
                    this.$store.dispatch('getdefaultrecived')
            }
        }
    }

    export default add
</script>