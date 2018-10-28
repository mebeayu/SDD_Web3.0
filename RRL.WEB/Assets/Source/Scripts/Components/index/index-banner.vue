<!-->首页轮播图<-->
<template>
  <div class="index-banner">
    <div v-bind:style="{width:this.$store.state.indexbanner.length*100+'%' ,marginLeft:-this.$store.state.imgindex*this.$store.state.screenWidth+'px' }">
      <img v-for="(item,index) in this.$store.state.indexbanner"
            v-bind:src="$store.state.imgs+item.pic_id+$store.state.img_suffix"
            v-bind:index="index"
            v-finger:swipe="swipe"
            v-bind:style="{width:$store.state.screenWidth+'px'}"
            :onerror="$store.state.defautbanner"
            :load='$store.state.defautbanner'
            v-on:click="direct(item)" />
    </div>
    <ul class="focus-dot">
      <li v-for="(item,index) in this.$store.state.indexbanner"
          v-bind:class="index==$store.state.imgindex?'beenchose':''"
          @click="changebanner(index)"
      ></li>
    </ul>
  </div>
</template>
<script>
  //import alloyfinger from 'alloyfinger';
  //import AlloyFingerVue from '../alloy_finger.vue.js';
  //import Vue from 'vue';

  //Vue.use(alloyfinger)
  //Vue.use(AlloyFingerVue)
  //import alloyfinger from 'alloyfinger';

  let indexBanner={
      created(){
        //获取屏幕宽度
        this.$store.commit('getscreenwidth')
      },
      methods:{
        //点击焦点切换轮播图
        changebanner(index){
            this.$store.commit('clickbannerdot',index)
        },
        //滑动切换轮播图
        swipe(e){
          this.$store.commit('changebannerindex',e.direction)
        },
        //点击轮播图跳转指向
        direct(item){
          if(item.direct_type==1){
              this.$router.push('goodsdetail/'+item.url+'/null')
          }else if(item.direct_type==2){
              //go to shopdetail
          }else{
              if (this.$store.state.shorttoken.length == 0 ) {
                //window.location.href=item.url + '?token='; 
              } else {
                window.location.href=item.url + '/?token=' + this.$store.state.shorttoken; 
              }
              
          }
        }
      }
      //轮播图自动播放设置在首页
      // beforeRouteLeave: (to, from, next) => {
      //
      //   //next()
      // }
  }
  export {indexBanner as default}
</script>