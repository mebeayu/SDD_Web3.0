<template>
    <div class="upaccount-box">
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back"/>
            <p>
                余额分类
            </p>
        </div>
        <div class="account-box">
            <div class="content-box">
                <div class="content">
                    <router-link tag="p"
                                 v-bind:to="{path:'/Sureupaccount',query:{type:'1',money:this.$store.state.useraccount.r_money}}">
                        消费奖励：{{this.$store.state.useraccount.r_money}}<span class="fr"
                                                                            v-bind:count="this.$store.state.useraccount.r_money"><img
                            src="/Assets/Source/img/icon-right.png"></span></router-link>
                    <router-link tag="p"
                                 v-bind:to="{path:'/Sureupaccount',query:{type:'2',money:this.$store.state.useraccount.x_money}}">
                        现金金额：{{this.$store.state.useraccount.x_money}}<span class="fr"
                                                                            v-bind:count="this.$store.state.useraccount.x_money"><img
                            src="/Assets/Source/img/icon-right.png"></span></router-link>
                    <router-link tag="p"
                                 v-bind:to="{path:'/Sureupaccount',query:{type:'3',money:this.$store.state.useraccount.y_money}}">
                        推荐奖励：{{this.$store.state.useraccount.y_money}}<span class="fr"
                                                                            v-bind:count="this.$store.state.useraccount.y_money"><img
                            src="/Assets/Source/img/icon-right.png"></span></router-link>

                    <keep-alive v-if="$route.meta.keepAlive">
                        <div class="up-account-pagebar">
                            <div v-if="upaccountShow" class="up-account-page">
                                <ul class="up-account-page-top">
                                    <li v-for="(item,index) in upAccountList" :key="index">
                                        <div class="up-account-time">
                                            <span>提现时间：{{ item.addtime | formatTimes }}</span>
                                            <span>提现类型：{{ item.type | formatTypes }}</span>
                                        </div>
                                        <div class="up-account-price">
                                            <span>提现金额：￥{{ item.apply_money }}</span>
                                            <span>状态：{{ item.status | formatStatus }}</span>
                                        </div>
                                    </li>
                                </ul>
                                <div class="up-account-page-bot">
                                    <button @click="addLoadRecord">{{ addLoadRecordData }}</button>
                                </div>
                            </div>
                            <div v-if="!upaccountShow" class="up-account-page-no">暂无提现记录......</div>
                        </div>
                    </keep-alive>

                    <keep-alive v-if="!$route.meta.keepAlive">
                        <div class="up-account-pagebar">
                            <div v-if="upaccountShow" class="up-account-page">
                                <ul class="up-account-page-top">
                                    <li v-for="(item,index) in upAccountList" :key="index">
                                        <div class="up-account-time">
                                            <span>提现时间：{{ item.addtime | formatTimes }}</span>
                                            <span>提现类型：{{ item.type | formatTypes }}</span>
                                        </div>
                                        <div class="up-account-price">
                                            <span>提现金额：￥{{ item.apply_money }}</span>
                                            <span>状态：{{ item.status | formatStatus }}</span>
                                        </div>
                                    </li>
                                </ul>
                                <div class="up-account-page-bot">
                                    <button @click="addLoadRecord">{{ addLoadRecordData }}</button>
                                </div>
                            </div>
                            <div v-if="!upaccountShow" class="up-account-page-no">暂无提现记录......</div>
                        </div>
                    </keep-alive>

                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .account-box .content-box {
        width: auto;
        padding: 0.26666667rem;
    }

    .up-account-page-top li {
        border-bottom: 1px solid #dadada;
    }

    .up-account-time, .up-account-price {
        padding: .2rem 0;
        overflow: hidden;
    }

    .up-account-time span:nth-of-type(1), .up-account-price span:nth-of-type(1) {
        float: left;
    }

    .up-account-time span:nth-of-type(2), .up-account-price span:nth-of-type(2) {
        float: right;
    }

    .up-account-time span, .up-account-price span {
        color: #666666;
        font-size: .33rem;
    }

    .up-account-page-bot button {
        border: none;
        border-radius: .2rem;
        width: 100%;
        padding: .3rem 0;
        background: #e4dcdc;
        margin: .2rem 0;
        font-size: .32rem;
        color: #555;
        outline: none;
    }

    .up-account-page-no {
        color: #a5a5a5;
        font-size: .32rem;
        text-align: center;
        padding-top: .3rem;
    }
</style>

<script>
  import $ from 'jquery';

  let upaccount = {
    name: "upaccount",
    data() {
      return {
        accdata: {
          money: '',
          type: '',
        },
        time: 30,
        hassend: false,
        paydata: {
          money: 0,
          type: '',
          cardid: '',
          sms: '',
        },
        upaccountShow: false,
        upAccountList: null,
        oldStartPage: 1,
        oldStartPageSize: 5,
        len: [],
        addLoadRecordData: "点击加载提现记录"
      }
    },
    filters: {
      formatStatus(val) {
        if (val === 1) {
          return "待审核";
        } else if (val === 2) {
          return "待支付";
        } else if (val === 3) {
          return "未通过";
        } else if (val === 4) {
          return "已支付";
        } else if (val === 5) {
          return "已作废";
        }
      },
      formatTypes(val) {
        if (val === 1) {
          return "消费奖励";
        } else if (val === 2) {
          return "现金金额";
        } else if (val === 3) {
          return "推荐奖励";
        }
      },
      formatTimes(val) {
        let clearT = val.replace("T", " ");
        let indexAry = [];
        for (var i = 0; i < clearT.length; i++) {
          if (clearT[i] === ":") {
            indexAry.push(i);
          }
        }
        return clearT.substr(0, indexAry[1]);
      }
    },
    created() {
      this.$store.dispatch('getuseraccount');
      this.$store.dispatch('getmycard');
      this.getList(this.oldStartPage, this.oldStartPageSize);
    },
    methods: {
      getList(page, pageSize) {
        let _this = this;
        $.ajax({
          url: '/WebApi/UserManager/CashApplyList?Page=' + page + '&PageSize=' + pageSize + '&token=' + encodeURIComponent(_this.$store.state.shorttoken),
          type: "get",
          success: function (data) {
            if (data.data === null || (data.data && !data.data.length)) {
              _this.upaccountShow = false;
              return;
            }
            _this.upaccountShow = true;
            _this.len.push(data.data.length);
            _this.upAccountList = data.data;
          }
        })
      },
      noRepeat(arr) {
        let arrStr = JSON.stringify(arr), str;
        for (let i = 0; i < arr.length; i++) {
          if ((arrStr.match(new RegExp(arr[i], "g")).length) > 1) {
            return true;
          }
        }
        return false;
      },
      addLoadRecord() {
        this.oldStartPageSize += 5;
        let flag = this.noRepeat(this.len);
        // 不重复就加载数据
        if (!flag) {
          this.getList(this.oldStartPage, this.oldStartPageSize);
          return;
        }
        this.addLoadRecordData = "客官~ 这盘菜已经吃完了";
      },
      back() {
        this.$router.back(-1)
      },
      getaccount(money, type) {
        $("#payBox").css({"opacity": 1});
        this.accdata.money = money;
        this.accdata.type = type
      },
      sendsms() {
        var that = this
        this.$store.dispatch('sendsms', sessionStorage.getItem('myid'))
        this.hassend = true
        var sh
        sh = setInterval(function () {
          that.time--
          if (that.time == 0) {
            that.hassend = false
            clearInterval(sh)
            that.time = 30
          }
        }, 1000)
      },
      surePay() {
        if (this.paydata.money > this.accdata.money && this.paydata.money == 0) {
          alert("余额不足!请重新输入提现金额");
          console.log(this.paydata.cardid)
        }
        else {
          if (this.paydata.cardid != null && this.paydata.sms != null) {
            this.paydata.type = this.accdata.type;
            //this.$store.dispatch('getcash',this.paydata)
          } else {
            alert("银行卡或者验证码不能为空");
          }

        }
      },
      close() {
        $("#payBox").css({"opacity": 0});
      }
    }
  }
  export default upaccount
</script>

<style>
</style>