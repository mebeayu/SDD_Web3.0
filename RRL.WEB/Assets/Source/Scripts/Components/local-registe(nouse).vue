<!-->地推注册页(shuohe)<-->
<template>
    <div>
        <div class="back-page-btn" id="carttop">
            <!--img src="/Assets/Source/img/返回@3x.png" v-on:click="back"/-->
            <p>
               注册
            </p>
        </div>
        <img style="width:100%;margin-bottom:10px;" src="/Assets/Source/img/标志定稿-01.jpg" v-bind:style="{marginTop:this.$store.state.topHeight+'px'}"/>
        <div class="loginbox">
            <div>
                <img src="/Assets/Source/img/我的1@3x.png"/>
                <input class="username" v-model="username" placeholder="请输入手机号码" v-on:blur="getphone"/>
            </div>
            <div style="justify-content: space-between">
                <input class="password" v-model ="userpassword" placeholder="请输入验证码"/>
                <p v-on:click="getsms" v-if="hassend==false">获取验证码</p>
                <p class="hassend" v-else>验证码已发送({{time}}s)</p>
            </div>
            <div v-on:click="logincli" class="logbtn">
                <p>注册</p>
            </div>
            <div class="erromsg">
                {{erro}}
            </div>
        </div>
    </div>
</template>

<script>
    let localregist={
        data(){
            return {
                username:'',
                userpassword:'',
                reg:/^1[0-9]{10}$/,
                erro:'',
                time:30,
                hassend:false,
                recommand:''
            }
        },
        created(){
            //存储及获取推荐人信息
            if(this.$route.params.username!='null'){
                sessionStorage.setItem('recommand',this.$route.params.username)
            }
            this.recommand=sessionStorage.getItem("recommand")
        },
        mounted(){
            //获取头高
            this.$store.commit('topheight')
        },
        methods:{
            //注册
            logincli(){
                var that=this
                this.$store.commit('setuser_info',{name:that.username,password:that.userpassword,rec:that.recommand})
                this.$store.dispatch('registfrompush')
                //this.$router.go(-2)
            },
            // back(){
            //     this.$router.back(-1)
            // },
            //发送验证码
            getsms(){
                var that=this
                if(this.erro==''&&this.reg.test(this.username)){
                    this.$store.dispatch('sendsms',that.username)
                    this.hassend=true
                    var sh
                    sh=setInterval(function(){
                        that.time--
                        console.log(that.time)
                        if(that.time==0){
                            that.hassend=false
                            clearInterval(sh)
                            that.time=30
                        }
                    },1000)
                }else{
                    this.erro="请输入正确的手机号码"
                }
                
            },
            //手机合法性判定
            getphone(){
                if(this.reg.test(this.username)){
                    this.erro=''
                }else{
                    this.erro="请输入正确的手机号码"
                }
            }
        }
    }

    export default localregist
</script>