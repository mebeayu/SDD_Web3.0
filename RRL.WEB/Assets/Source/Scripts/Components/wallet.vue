<!-->钱包页<-->
<template>
    <div style="background:#f5f5f5">
        <div class="wallet-header-2">
            <div style="background: transparent;box-shadow:none;"></div>
            <!--<router-link tag='div' to="/pocketcard">兜兜卡</router-link>-->
            <div>
                <p>我的钱包</p>
                <p>{{this.$store.getters.mywallet}}</p>
                <p v-on:click="oooo">
                    查看我的钱包
                </p>
            </div>
            <router-link tag='div' to="/upaccount">提现</router-link>
        </div>
        <div class='mycoins' style='border-bottom:1px solid #d4d4d4;'>
            <p>我的金币：<span>{{this.$store.getters.mycoins}}</span></p>
            <p v-on:click='jinbi'>查看详情</p>
        </div>
        <div class='mycoins' style='border-bottom:1px solid #d4d4d4;'>
            <p>我的金豆：<span>{{this.$store.state.wallet.UserAccountInfo.h_money}}</span></p>
            <p v-on:click='jinbi'></p>
        </div>
        <div class='mycoins' style='border-bottom:1px solid #d4d4d4;'>
            <p>
                我的卡券：<span>{{this.$store.state.wallet.UserAccountInfo.coupons_count}}</span> 张
            </p>
            <router-link tag='p' to='/mycard'>查看详情</router-link>
        </div>
        <div class='mycoins' v-if='this.$store.state.wallet.UserAccountInfo.point != 0'>
            <p>
                我的积分：
                <span>{{this.$store.state.wallet.UserAccountInfo.point}}</span>
            </p>
        </div>
        <div class='income' style='margin-top:10px;margin-bottom:1.8rem;'>
            <p>我的收益</p>
            <div id='lunbo'>
                <ul>
                    <li v-for='item in this.$store.state.wallet.UserMoneyRecord'>
                        <span>{{item.addtime | timeformat}}</span>
                        <span>{{item.remark}}</span>
                        <span>{{item.money}}</span>
                    </li>
                </ul>
            </div>
        </div>
        <div class='tradenum' style="display:none" id='last-item' v-on:click='showtips(true)'>
            <p>平台交易数据</p>
            <div>
                <div class='img'>
                    <img src='/Assets/Source/img/lanzi.png'/>
                </div>
                <ul>
                    <li>
                        <div>
                            <p></p>
                            今日平台交易总额
                        </div>
                        <div>
                            ￥{{this.$store.state.wallet.UserAccountInfo.total_trans }}
                        </div>
                    </li>
                    <li>
                        <div>
                            <p></p>
                            今日发放奖励总额
                        </div>
                        <div>
                            ￥{{this.$store.state.wallet.UserAccountInfo.total_return }}
                        </div>
                    </li>
                    <li>
                        <div>
                            <p></p>
                            今日每个权重分配金额
                        </div>
                        <div>
                            ￥{{this.$store.state.wallet.UserAccountInfo.return_ave_weight}}
                        </div>
                    </li>
                    <li>
                        <div>
                            <p></p>
                            当前平台总权重数量
                        </div>
                        <div>
                            {{this.$store.state.wallet.UserAccountInfo.total_weight }}
                        </div>
                    </li>
                    <li>
                        <div>
                            <p></p>
                            您当前的权重数量
                        </div>
                        <div>
                            {{this.$store.state.wallet.UserAccountInfo.plate_to_return_weight }}
                        </div>
                    </li>
                </ul>
            </div>

        </div>
        <bottom>
        </bottom>
        <div class='shadow' v-if='show == true' v-on:click='showtips(false)'>
            <div class='ruletips'>
                <img src='/Assets/Source/img/ruletips.png'/>
                <ul>
                    <li>
                        • 今日平台交易总额：每日平台订单总交易额
                    </li>
                    <li>
                        • 今日开放奖励总额：是平台吧当日交易总金额的2.5%作为奖励金额发放给平台用户。
                    </li>
                    <li>
                        • 今日每个权重分配金额：是根据平台今日发放奖励总额除以平台当前总权重数量得出。
                    </li>
                    <li>
                        • 当前平台总权重数量：平台参与分配的权重总数
                    </li>
                    <li>
                        • 您的当前权重数量：是根据您在平台的消费计算得出，每消费满100可获得1个权重。当前消费金额对应的金币返完后权重消失。
                    </li>
                </ul>
            </div>
        </div>
        <div class='shadow' v-if='this.$store.state.wallet.tips == false'>
            <div style='padding:15px;'>
                <div style='width:100%;padding:20px 15px;position:relative;top:150px;border-radius:8px;
                background:#fff;box-sizing:border-box'>
                    <p style='margin-bottom:20px;white-space:normal'>"待发放金额"更名为"我的金币"，仅更换名称，收益不受影响</p>
                    <p style='margin-bottom:20px;white-space:normal'>查看现金账户、消费账户、推荐账户金额及明细，可点击"查看我的钱包"查看</p>
                    <p style='white-space:normal'>奖励金账户拆分为"消费账户""推荐账户"，可点击"查看我的钱包"查看</p>
                </div>
                <p class='readtipbtn' style='position:relative;top:160px;width:90px;left:50%;
                margin-left:-45px;background:#ba3027;line-height:40px;
                color:#fff;border-radius:5px;text-align:center'
                   v-on:click='readtips'>我知道了</p>
            </div>
        </div>
    </div>
</template>

<script>
  import bottom from './bottom'
  import $ from 'jquery';

  const wallet = {
    data() {
      return {
        nowtime: new Date(),
        mywallet: 0,
        show: false,
        coins: 0,
        lunbo: {},
        t: {},
      }
    },
    components: {
      bottom
    },
    activated() {
    },
    created() {
      // console.log('wallet created')
      //this.$store.dispatch('islogin',['getUserAccountInfo', 'getUserMoneyRecord'])
      //获取钱包相关信息
      //console.log(this.nowtime)

      //判定是否登录
      if (GLOBE_SHORT_TOKEN) {
        this.$store.commit('SET_SHORTTOKEN', GLOBE_SHORT_TOKEN)
        this.$store.dispatch('getUserMoneyRecord')
        this.$store.dispatch('getUserAccountInfo')
      } else {
        if (this.$store.state.shorttoken.length == 0 || this.$store.state.shorttoken == undefined) {
          if (localStorage.getItem('longtoken')) {
            const longtoken = localStorage.getItem('longtoken')
            this.$store.dispatch('getshorttoken', longtoken).then(() => {
              this.$store.dispatch('getUserMoneyRecord')
              this.$store.dispatch('getUserAccountInfo')
            })
          }
        } else {
          this.$store.dispatch('getUserMoneyRecord')
          this.$store.dispatch('getUserAccountInfo')
        }
      }
    },
    mounted() {
      // console.log('wallet mounted')
      this.$store.commit('bottomheight')
    },
    filters: {
      //设置详情列表滤镜
      timeformat(time) {
        wallet.nowtime = new Date()
        let newtime
        newtime = time.split('T')
        time = newtime[0].split('-')
        let year = time[0]
        let mouth = time[1]
        let day = time[2]
        time = newtime[0]
        return time
      }
    },
    methods: {
      //登录确定
      oooo() {
        if (this.$store.state.shorttoken.length == 0) {
          confirm('尚未登录,是否登录？', () => {
            this.$router.push({path: '/login/?redirect=' + encodeURIComponent("/#/wallet")})
          })
        } else {
          this.$router.push('/walletdetail');
        }

      },
      tixian() {
        alert('重新调整中...');
      },
      showtips(sta) {
        this.show = sta;
      },
      jinbi() {
        this.$router.push('/coinslist');
      },
      autoplay(node) {
        // node.scrollTop ++;
        if (node.scrollHeight - node.scrollTop === node.clientHeight) {
          node.scrollTop = 0
        }
      },
      readtips() {
        const that = this;
        $.ajax({
          url: 'WebApi/UserManager/MarkInfoRead',
          type: "post",
          data: {
            "token": this.$store.state.shorttoken
          },
          success: function (data) {
            that.$store.commit('SET_TIPS', true)
          }
        });
      }
    },
    mounted() {
      this.lunbo = $('#lunbo')
      this.lunbo.scrollTop++;
      this.t = setInterval(function () {
        this.lunbo.scrollTop += 3;
        if (this.lunbo.scrollHeight - this.lunbo.scrollTop === this.lunbo.clientHeight) {
          this.lunbo.scrollTop = 0
        }
      }, 500);
    },
    beforeDestroy() {
      clearInterval(this.t);
    }
  }

  export default wallet
</script>