<template>
    <div class='coininfo'>
        <viewtitle></viewtitle>
        <div>
            <p>
            Q:什么是金币？
            </p>
            <p>
            A:金币是在省兜兜商城推出的积分。
            </p>
        </div>
        <div>
            <p>
            Q:金币可以做什么？
            </p>
            <p>
            A:金币可用作金币专区购物抵扣,参与游戏和抽奖。其中购物产生的金币还可以参与平台购物消费的奖励分配。
            </p>
        </div>
        <div>
            <p>
            Q:金币怎么获得？
            </p>
            <p>
            A:金币可通过购物，推荐好友注册，签到，参与活动获得。
            </p>
        </div>
        <div>
            <p>
            Q:金币怎么抵扣？
            </p>
            <p>
            A:金币抵扣时会有优先抵扣通过推荐好友注册，签到，参与活动获得的金币。
            </p>
        </div>
        <div>
            <p>
            Q:金币为什么会冻结？
            </p>
            <p>
            A:省兜兜商城在订单付款成功后会给用户发放金币，金币需要在订单确认收货后15天内不产生退货或退款才会解除冻结。
            </p>
        </div>
    </div>

</template>

<script>
import viewtitle from './title';

    let coininfo = {
        methods:{
            back() {
                  this.$router.go(-1)
              },
        },
        
        components:{
            viewtitle
        },
    }   

    export default  coininfo
</script>