<!-->活动规则<-->
<template>
    <div>
        <div class="closehhh">
            <p v-on:click="back">
                <img src="/Assets/Source/img/返回@3x.png" />
                返回
            </p>
               活动规则
        </div>
        <div style="padding:15px;margin-top:63px;text-align:center" class="gamerule">
            <header style="font-size:18px;line-height:60px">
                省兜兜商城新人有礼活动规则
            </header>
            <p>1.输入电话号码，点击“确认领取”按钮，系统提示“领取成功”，则表示已成功领取50元新人奖励，系统会自动将50元划入您省兜兜账户中的“待返奖励”；</p>
            <p>2.领取成功后登录省兜兜APP，进入“钱包”菜单查看，“待返奖励”则显示50元；</p>
            <p>3.“待返奖励”中的50元会根据日平台收益，每日将50元中相应的金额逐渐返到“我的钱包”，直到返完为止；</p>
            <p>4.“我的钱包”显示的金额可用于平台再消费，也可提现到银行卡。其中奖励金提现需收取10%手续费；</p>
            <p>5.新用户首次参与活动则需要成为省兜兜的注册用户后金额才能发放到注册账号中。领取礼包操作的同时会默认注册成为省兜兜用户；</p>
            <p>6.此50元奖励每个账户ID限领一次；</p>
            <p>7.此50元奖励仅限于在省兜兜平台使用或提现，不得做其他用途；</p>
            <p>8.使用规则遵循省兜兜平台规则；</p>
            <p>9.活动最终解释权归省兜兜所有。</p>
        </div>
        <div class="rulefooter">
            人人利网络科技（深圳）有限公司
        </div>
    </div>
</template>

<script>
    let gamerule={
        methods:{
            back(){
                this.$router.back(-1)
            },
        }
    }

    export default gamerule
</script>