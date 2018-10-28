<!--登录跳转-->
<template>
    <div class="login-wrap-box" >
    </div>
</template>
<script>
    import $ from 'jquery';

    let login = {
        data() {
            return {
            }
        },
        created() {
            this.tologin();
        },
        methods: {
            //登录
            tologin() {
                var allParam = window.UI.Page.getAllparam();
                if(allParam.token!==''){
                    this.$store.commit('SET_SHORTTOKEN', allParam.token);
                    localStorage.setItem('shorttoken', allParam.token);
                }
                var redirect_to = "/my";
                const para = allParam.to || allParam.redirect;
                if (para) {
                    redirect_to = para.replace("#token", this.$store.state.shorttoken);
                } else {
                    redirect_to = "/my";
                }

                if(redirect_to.indexOf('/#') == 0){
                	redirect_to = redirect_to.substr(2);
                }

                this.$router.replace(redirect_to);                
            }
                   
        }
           
    }
    export default login
</script>