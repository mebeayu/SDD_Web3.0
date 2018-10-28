<template>
    <div class='makeshareimg'>
        <viewtitle></viewtitle>
        <div style='margin-top:120px;padding:0 15px;'>
            图片中二维码带您的推荐标识，分享后好友识别或扫描二维码即可购买。
            <br/>
            <span>
                推荐好友购买后，您将获得购物订单的10%作为推荐奖励，推荐奖励会在支付成功后到账，但需要订单确认交易完成且不产生退货后方可提现或用作平台消费
            </span>
        </div>
        <div class='step'>
            <p>
                第一步：长按保存下方图片
            </p>
            <div v-if='sta == 0'>
                <img v-bind:src='img'/>
                &uarr;&uarr;长按图片保存
            </div>
            <div v-else class='way2' style='display:flex;justify-content: space-between;flex-wrap:wrap'>
                <div v-for="(item,index) in this.$store.state.shareimglist" style='width:33%;'>
                     <img v-bind:src="item"/>
                </div>
                <div id='oao' style='width:33%;background:rgba(0,0,0,.3);
                margin-bottom:15px;'>
                    <qrcode v-bind:value='add' id='xoxo' style='display:none;'></qrcode>
                </div>
                &uarr;&uarr;长按图片保存
            </div>
        </div>
        <div class='step'>
            <p>
                第二步：你如不想编辑配文，可复制下方文案
            </p>
            <div>
                <textarea id='bar'>
                    {{this.$store.state.sharetext}}
                </textarea>
            </div>
            <div class='copy_text_btn' data-clipboard-target="#bar">
                复制文字
            </div>
        </div>
    </div>
</template>
<script>
import $ from 'jquery';
import Clipboard from 'clipboard';
import viewtitle from './title';
    let makeshareimg = {
        data(){
            return {
                sta:'',
                add:'',
                img:''
            }
        },
        components:{
            viewtitle
        },
        created(){
            this.img = 'https://e-shop.rrlsz.com.cn/SWebApi/Public/ShareGoodsPic/'+this.$store.state.goodsid+'?token='+encodeURIComponent(this.$store.state.shorttoken)
            this.sta = this.$route.params.sta
            const id = sessionStorage.getItem('myid');
            this.$store.commit('setshareid', id);
            this.add = 'https://e-shop.rrlsz.com.cn/#/regist/'+this.$store.state.shareid;
        },
        mounted(){
            const clipboard = new Clipboard($(this.$el).find('.copy_text_btn')[0]);
            clipboard.on('success', function(e) {
                alert('复制成功！')
            });
            clipboard.on('error', function(e) {
                alert('复制失败,请手动复制')
            });
            const myCanvas = document.getElementsByTagName("canvas")[0];
            const img = this.convertCanvasToImage(myCanvas);
            const box = $('#oao')
            box.append(img);
        },
        methods:{
            back() {
                this.$router.back()
            },
            convertCanvasToImage(canvas){
                //新Image对象,可以理解为DOM;
                var image = new Image();
                //canvas.toDataURL返回的是一串Base64编码的URL,当然,浏览器自己肯定支持
                //指定格式PNG
                try{
                    image.src = canvas.toDataURL("image/png");
                } catch(e) {
                    
                }
                return image;
            } 
        },
    }

    export default makeshareimg
</script>