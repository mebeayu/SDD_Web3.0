<!-->昵称及性别设置<-->
<template>
    <ul class="setinfo">
        <viewtitle></viewtitle>
        <li style="padding-top:2rem;" v-on:click="setnickname" class="fuckoo">
            <span>昵称</span>
            <span >{{this.$store.state.useinfo.nick_name}}</span>
        </li>
        <li class="fuckoo">
            <span>性别</span>
            <span>
                <select v-model="sex" v-on:change="getsex">
                    <option value="2" sex>女</option>
                    <option value="1" sex>男</option>
                </select>
            </span>
        </li>
        <li class="shadow1" v-if="showalert==true">
            <div class="tiplogout">
                <p>请输入昵称</p>
                <input value="" v-on:input="getnickname" v-model="nickname" />
                <div>
                    <div v-on:click='opera(false)'>取消</div>
                    <div v-on:click='opera(true)'>确定</div>
                </div>
            </div>
        </li>
        <!--<router-link tag='li' to='/idcard'>
            <span>清关信息</span>
            <span></span>
        </router-link>-->
    </ul>
</template>
<script>
import viewtitle from './title';
    let setinfo={
        data(){
            return {
                nickname:'',
                showalert:false,
                sex:''
            }
        },
        components:{
            viewtitle
        },
        computed:{
            //获取用户信息
            getinfo(){
                return this.$store.state.useinfo
            }
        },
        watch:{
            //用户信息显示
            getinfo(){
                this.nickname=this.$store.state.useinfo.nick_name
                this.sex=this.$store.state.useinfo.sex
            }
        },
        created(){
            //判定是否需要获取用户信息
            if(this.$store.state.useinfo.sex==undefined){
                this.$store.dispatch('getuserinfo')
            }
            // this.nickname=this.$store.state.useinfo.nick_name||''
            // this.sex=this.$store.state.useinfo.sex
        },
        methods:{
            back(){
                this.$router.go(-1)
             },
             //设置昵称
             setnickname(){
                this.showalert=true
             },
             //获取昵称
             getnickname(){
                console.log(this.nickname)
             },
             //设置性别
             getsex(){
                console.log(this.sex)
                 this.$store.dispatch('setuserinfo',{nickname:this.nickname,sex:this.sex})
             },
             //确认修改设置操作
             opera(e){
                if(e==false){
                    this.showalert=false
                }else{
                    this.$store.dispatch('setuserinfo',{nickname:this.nickname,sex:this.sex})
                    this.showalert=false
                    location.reload()
                }
            },
        }
    }

    export default setinfo
</script>