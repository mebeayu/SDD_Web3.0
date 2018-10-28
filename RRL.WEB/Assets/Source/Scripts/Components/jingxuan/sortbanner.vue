<!-->精选页banner<-->
<template>
  <div class="index-banner">
    <div v-bind:style="{width:this.$store.state.sortbanner.length*100+'%' ,marginLeft:-this.$store.state.imgindex*this.$store.state.screenWidth+'px' }"
        v-finger:swipe="swipe">
      <img v-for="(item,index) in this.$store.state.sortbanner"
            v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix"
            v-bind:index="index"
            v-bind:style="{width:$store.state.screenWidth+'px'}"
            :onerror="$store.state.defautbanner"
            :load='$store.state.defautbanner'
            v-on:click="direct(item)" />
    </div>
    <ul class="focus-dot">
      <li v-for="(item,index) in this.$store.state.sortbanner"
          v-bind:class="index==$store.state.imgindex?'beenchose':''"
          @click="changebanner(index)"
          ></li>
    </ul>
  </div>
</template>
<script>
  let sortbanner={
      created(){
        //获取屏幕宽
        this.$store.commit('getscreenwidth')
      },
      methods:{
        //点击焦点切换banner
        changebanner(index){
            this.$store.commit('clickbannerdot',index)
        },
        //滑动切换banner
        swipe(e){
          this.$store.commit('changesortbanner',e.direction)
        },
        //banner图点击指向
        direct(item){
          if(item.direct_type==1){
              this.$router.push('goodsdetail/'+item.url+'/null')
          }else if(item.direct_type==2){
              //go to shopdetail
          }else{
              window.location.href=item.url
          }
        }
      }
  }
  export {sortbanner as default}
</script>