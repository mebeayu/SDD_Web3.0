<!-->商品简介  belong to 商品详情<-->
<template>
    <div class="goods-detail-info">
        <div class="GD-info-title">
            商品详情
        </div>
        <div id='infobox'>
       
       
       
        </div>
    </div>
</template>

<script>
    //import $ from '../../../../Event/jquery-3.2.1.min.js'
    import $ from 'jquery'
    let goodsinfo={
        computed:{
            detail(){
                return this.$store.state.goodsdetail.goodsdetail
            }
        },
        watch:{
            detail(){
                $('#infobox').html(this.$store.state.goodsdetail.goodsdetail.goods_info_html);
                $('#infobox>p').children().css('font-size','.18666666666666668rem')
                $('#infobox>span').children().css('font-size','.18666666666666668rem')
                $('#infobox>p>span').children().css('font-size','.18666666666666668rem')
                $('#infobox>table').children().css('width','100%');
                const reg=/<img.*?(?:>|\/>)/gi
                const arr = this.$store.state.goodsdetail.goodsdetail.goods_info_html.match(reg);
                const srcReg = /src=[\'\"]?([^\'\"]*)[\'\"]?/i;
                let imglist=[]
                try{
                    for (let i = 0; i < arr.length; i++) {
                        var src = arr[i].match(srcReg);
                        //获取图片地址
                        imglist.push(src[1])
                    }
                }catch(e) {
                    
                }

                const newarr = imglist.slice(0,5)
                this.$store.commit('setshareimglist', newarr)
            }
        }
    }
    export {goodsinfo as default}
</script>