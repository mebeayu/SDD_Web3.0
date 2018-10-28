<template>
    <div class='fucku' style='text-align:center'>
        <viewtitle></viewtitle>
        <input placeholder= '请输入身份证号' v-model='idcardnum'
            style='width:80%;height:40px;margin-top:60px;border:1px solid #000'/>
        <input placeholder= '请输入姓名' v-model ='name'
        style='width:80%;height:40px;margin-top:30px;border:1px solid #000'/>
        <div v-on:click='save' style='color:#fff;margin:30px auto;width:80px;line-height:40px;background:#ba3027'>
        保存
        </div>
        <p style='color:#f00'>{{error}}</p>
    </div>
</template>

<script>

import $ from 'jquery';
import viewtitle from './title';
    const idcard = {
        data(){
            return {
                idcardnum:'',
                name: '',
                idreg:/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/,
                error:'',
                regName :/^[\u4e00-\u9fa5]{2,4}$/,
            }
        },
        components:{
            viewtitle
        },
        methods:{
            save() {
                const that = this
                if(this.idreg.test(this.idcardnum) == false) {
                    this.error = '请填写正确的身份证号'
                } else if (this.regName.test(this.name) == false) {
                    this.error = '请填写姓名'
                } else {
                    this.error = ''
                    // do
                    $.ajax({
                        url:'WebApi/UserManager/Identity',
                        data:{
                            "realname": this.name,
                            "identity": this.idcardnum,
                            "token": this.$store.state.shorttoken
                        },
                        type:'post',
                        success:function(data) {
                            if(data.status == 0){
                                alert('保存成功')
                                that.$router.back()
                            } else {
                                alert(data.message)
                            }
                        },
                        error:function(res) {
                            alert(res.message)
                        }
                    })
                }
            },
            back() {
                this.$router.back()
            }
        }
    }

    export default idcard
</script>