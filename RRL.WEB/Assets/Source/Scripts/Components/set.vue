<!-->设置页<-->
<template>
    <div class="myset">
        <viewtitle></viewtitle>
        <ul>
            <router-link tag="li" to="/setinfo" id="first-item">
                <span>个人信息</span>
                <img/>
            </router-link>
            <!--<router-link tag="li" to="/arealist">
                <span>收件地址</span>
                <img/>
            </router-link>-->
            <li>
                <span>关于我们</span>
                <img/>
            </li>
            <li @click="update">
                <span>检查更新</span>
            </li>
        </ul>
        <div class="fuck">
            <router-link to='/login' tag="p" v-if="$store.state.shorttoken.length === 0">登录/注册</router-link>
            <p v-on:click="logout" v-else>注销登录</p>
        </div>
        <div class="shadow1" v-if="showalert==true">
            <div class="tiplogout">
                <p>确认退出登录?</p>
                <div>
                    <div v-on:click='opera(false)'>取消</div>
                    <div v-on:click='opera(true)'>确定</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
  import viewtitle from './title';

  let myset = {
    data() {
      return {
        showalert: false
      }
    },
    components: {
      viewtitle
    },
    methods: {
      //检查更新
      update(){
        window.location.href = "https://e-shop.rrlsz.com.cn/Event/Update";
      },
      back() {
        this.$router.go(-1)
      },
      //退出登录提示
      logout() {
        this.showalert = true
      },
      //退出操作
      opera(e) {
        if (e == false) { 
          this.showalert = false
        } else {
          this.$store.dispatch('logout');
          this.showalert = false;
          this.$store.commit('SET_SHORTTOKEN', null);
          this.$store.commit('setlongtoken', null);
          //this.$store.shorttoken = "";
          //localStorage.removeItem("usrInfoList");
          this.$store.state.my_usrInfoList=null;
          localStorage.removeItem("longtoken");
          localStorage.removeItem("shorttoken");
          sessionStorage.removeItem("myid");
          this.$router.push("/index");
        }
      }
    }
  }

  export default myset
</script>