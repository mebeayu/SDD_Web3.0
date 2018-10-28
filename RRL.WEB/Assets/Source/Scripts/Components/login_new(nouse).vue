<template>
    <div style='background:#fff;text-align:center;'>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
            <p>省兜兜</p>
        </div>
        <img src='/Assets/Source/img/微信图片_20171031171209.png' style='margin-top:100px;' />
        <br/>
        <img src='/Assets/Source/img/微信图片_20171031171221.jpg' style='width:60%;' />
        <div v-on:click='getopenid' v-if='this.$store.state.openid.length != 0' style='width:80%;line-height:40px;border-radius:20px;background:#ba3027;margin:20px auto;color:#fff;'>
            <img src='/Assets/Source/img/微信图片_20171031171217.png' style='height:30px;vertical-align:middle;margin-right:15px;' /> 微信登录
        </div>
        <router-link tag='div' to='/login/null' style='width:80%;line-height:40px;border-radius:20px;margin:20px auto;color:#7a7a7a'>
            <img src='/Assets/Source/img/微信图片_20171031171228.png' style='height:30px;vertical-align:middle;margin-right:15px;' /> 手机登录
        </router-link>
    </div>
</template>

<script>
    import $ from 'jquery';
    let login_new = {
        // beforeRouteEnter: (to, from, next) => {
        //     // 设置redirect 
        //     if(from.path == '/') {
        //         sessionStorage.setItem('redirect',true)
        //     }else if (from.name == 'login'){

        //     } else {
        //         sessionStorage.setItem('redirect', false)
        //     }

        //     if(from.name == 'login') {
        //         if( sessionStorage.getItem('redirect') == 'true') {
        //             next( login_new => {
        //                 login_new.$router.push('/my')
        //             })
        //         } else {
        //             next(login_new => {
        //                 login_new.$router.back()
        //             })
        //         }
        //     } else {
        //         next()
        //     }
        // },
        created() {
            const rec = this.$route.query.rec
            if (rec != undefined) {
                sessionStorage.setItem('recommand', rec);
            }
        },
        methods: {
            back() {
                this.$router.back()
            },
            getopenid() {
                const that = this
                $.ajax({
                    url: 'WebApi/UserManager/ApplyTokenViaOpenId?openid=' + this.$store.state.openid + '&type=WxMP',
                    type: 'get',
                    success: function(data) {
                        if (data.status == 0) {
                            that.$store.commit('SET_SHORTTOKEN', data.data)
                            localStorage.setItem('longtoken', data.additional_data)
                            that.$router.push('/my')
                            
                        } else {
                            if(sessionStorage.getItem('recommand')) {
                                const rec = sessionStorage.getItem('recommand')
                                that.$router.push('/regist/' + rec +'?from=bind')
                            }else {
                                 that.$router.push('/regist/null?from=bind')
                            }
                            
                        }
                    }
                })
            },
        }
    }
    export default login_new
</script>