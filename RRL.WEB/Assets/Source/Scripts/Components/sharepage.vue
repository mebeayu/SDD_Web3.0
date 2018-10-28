<template>
    <div class='sharepage'>
        <viewtitle></viewtitle>
        <div class = 'way way1'>
            <span>
                分享方法一
            </span>
            <p>
                单张图片分享，清晰明了
            </p>
            <div>
                <img v-bind:src='img'/>
            </div>
            
            <div class='madeimg_btn' v-on:click='next(0)'>
                生成分享图片
            </div>
        </div>
        <div class = 'way way2'>
            <span>
                分享方法二
            </span>
            <p>
                多张图片分享，自主编辑和介绍
            </p>
            <div class='imgbox'>
                <div v-for="(item,index) in this.$store.state.shareimglist"> 
                    <img v-bind:src="item"/>
                </div>
                <div>
                    <qrcode v-bind:value='add'></qrcode>
                </div>
            </div>
            <div class='madeimg_btn' v-on:click='next(1)'>
                生成分享图片
            </div>
        </div>

    </div>

</template>

<script>
import viewtitle from './title';

    let sharepage = {
        data(){
            return {
                add:'',
                img:''
            }
        },
        components:{
            viewtitle
        },
        created(){
            //获取用户信息
            this.$store.dispatch('getuserinfo')
            const id = sessionStorage.getItem('myid');
            this.$store.commit('setshareid', id);
            this.add = 'https://e-shop.rrlsz.com.cn/#/regist/'+this.$store.state.shareid;
            this.img = 'https://e-shop.rrlsz.com.cn/WebApi/Public/ShareGoodsPic/'+this.$store.state.goodsid+'?token='+encodeURIComponent(this.$store.state.shorttoken)
            this.$store.commit('setshareimg', this.img)
            this.$store.dispatch('getshareimglist')
        },
        methods:{
            next(sta) {
                this.$router.push('/makeshareimg/'+ sta)
            },
            back() {
                this.$router.back()
            }
        }
    }

    export default sharepage
</script>