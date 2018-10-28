<template>
     <!-- 商品分享弹框 -->
        <div class="share_frame" ref="frame" v-show="isShowDiloag">
            <div class="share_frame_box">
                <dl class="share_box">
                    <dt class="share_box_title">
                        <h3>省兜兜</h3>
                        <i @click='closeShare'><img src="/Assets/Source/img/gs_03.png"/></i>
                    </dt>
                    <dd class="share_box_img">
                        <img  ref='goodImg' 
                         :src="$store.state.imgs+goodsdetail.pic_id+'.jpg'"
                         :onerror="$store.state.defautbanner"
                         :load='$store.state.defautbanner'
                         alt="产品图片"
                          >
                    </dd>
                    <dd class="share_goods_info">
                        <div class="share_goods_info_left">
                            <p class="GD-name">
                                {{this.$store.state.goodsdetail.goodsdetail.name}}
                            </p>
                            <p class="GD-intro">
                                {{this.$store.state.goodsdetail.goodsdetail.propaganda}}
                            </p>
                        </div>
                        <div class="share_goods_info_right">
                            <span >￥{{this.$store.state.goodsdetail.goodsdetail.price}}</span>
                            <span >(￥{{ this.$store.state.goodsdetail.goodsdetail.cash_price }} +{{ this.$store.state.goodsdetail.goodsdetail.beans_price }}金豆)</span>
                        </div>
                    </dd>
                    <dd class="share_QR_code">
                        <div class="share_QR_code_left">
                            <!-- <img  :src="'/sport/SportShareQRCode?token='+encodeURI(this.token)" alt="二维码"> -->
                            
                            <img  :src="'/shop/GoodsShareQRCode?token='+encodeURI(this.token)+'&goodsId='+this.$store.state.goodsdetail.goodsdetail.id" alt="二维码">
                        </div>
                        <div class="share_QR_code_right">
                            <a href="javascript:;" @click="share_goods('friend')"  class="share_friend"><img src="/Assets/Source/img/friend.png" alt="微信好友"></a>
                            <a href="javascript:;" @click="share_goods('circle')" class="share_circle"><img src="/Assets/Source/img/circle.png" alt="微信朋友圈"></a>
                        </div>
                    </dd>
                </dl>
            </div>
          
        </div>
        <!-- 商品分享弹框 -->   
</template>
<style scoped lang="scss">
    @function px2rem($px) {
        $rem: 75px;
        @return ($px/$rem) + rem;
    }
    @mixin flex {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .share_frame{
        width: 100%;
        height: 100%;
        background: rgba(0,0,0,.5);
        position: fixed;
        top: 0;
        left: 0;
        z-index: 7;
    }
    .share_frame_box{
        position: absolute;
        width: 80%;
        top: 12%;
        left: 6%;
        background: #fff;
        padding: 0 .4rem .4rem;
        border-radius: .2rem;
        -webkit-box-shadow: 0.1rem 0.1rem 0.1rem rgba(0, 0, 0, 0.2);
        box-shadow: 0.1rem 0.1rem 0.1rem rgba(0, 0, 0, 0.2);
    }
    .share_box_title{
        line-height: 1rem;
        @include  flex;
        h3{
            color: #65c4aa;
            font-size: px2rem(34px);
            height:px2rem(120px);
            line-height: px2rem(120px);
            font-weight: bold;
        }
        i{
            display: block;
            width: px2rem(50px);
            height: px2rem(50px);
            img{
                width: 100%;
                height: 100%;
            }
        }
    }
    .share_box_img{
        width:100%;
        margin: 0 auto;
        height: 5.5rem;
        img{
            width: 100%;
            height: 100%;
        }
    }
    .share_goods_info{
        @include  flex;
        margin: px2rem(40px) 0;
        .share_goods_info_left{
            width:px2rem(355px) ;
            overflow: hidden;
            text-overflow:ellipsis;
            white-space: nowrap;
            .GD-name {
                color: #2d2d2d;
                font-size: .4rem;
                text-overflow: ellipsis;
                overflow: hidden;
                margin-top: .3rem;
            }
            .GD-intro {
                color: #7a7a7a;
                font-size: .32rem;
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;
                margin-bottom: .4rem;
                margin-top: .3rem;
            }
        }
        .share_goods_info_right{
            width:px2rem(230px) ;
            padding: .1rem 0;
            background: url('/Assets/Source/img/info_right_03.png') no-repeat center top;
            background-size:100% 100%; 
            span{
                display: block;
                color: #fff;
                font-size: .2rem;
                text-align: center;
                &:first-child{
                    font-size: .5rem;
                    font-weight: bold;
                    margin-bottom: .2rem;
                }
            }
        }

    }
    .share_QR_code{
        @include flex;
        .share_QR_code_left{
            width: px2rem(215px);
            height: px2rem(215px);
            border: 1px solid #7a7a7a;
            img{
                width: px2rem(215px);
                height: px2rem(215px);
            }
        }
        .share_QR_code_right{
            @include flex;
            a{
                display: block;
            }
            img{
                width: 100%;
                height: 100%;
            }
            .share_friend{
                width: px2rem(132px);
                height: px2rem(134px);
            }
            .share_circle{
                width: px2rem(152px);
                height: px2rem(134px);
            }
        }
    }

</style>

<script>
import  $      from 'jquery'
export default {
  name:'shareFrame',
  props:[
      'goodsdetail'
      ],
  data(){
      return{
          token:localStorage.getItem("shorttoken"),
          isShowDiloag:false
      }
  },
  mounted(){
      var that=this;
      this.bus.$on('showShareDiloag',function(){
          that.isShowDiloag=true;
      })
  },
  methods:{
    share_goods(shareType) {
        let that=this;
        if(!this.token){
            confirm('尚未登录,是否登录？',() =>{
                that.$router.push({path:"/login"});
            })
        }else{
            if(shareType == 'friend'){
                that.share_friend()
            }
            if(shareType == 'circle'){
                that.share_circle()
            }
        }
    },
    closeShare:function(){
        this.isShowDiloag=false
    },
    share_friend:function(){
        window.location.href = "/Event/ShareGoodsToWechatFriend?token=" + encodeURIComponent(this.token)+"&goodsId="+this.$store.state.goodsdetail.goodsdetail.id+"&goodsPictureId="+this.$store.state.goodsdetail.goodsdetail.pic_id;
    },
    share_circle:function(){
        window.location.href = "/Event/ShareGoodsToWechatCircle?token=" + encodeURIComponent(this.token)+"&goodsId="+this.$store.state.goodsdetail.goodsdetail.id+"&goodsPictureId="+this.$store.state.goodsdetail.goodsdetail.pic_id;
    }
  } 
}
</script>
