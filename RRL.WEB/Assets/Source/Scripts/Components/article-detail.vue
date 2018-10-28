<!--底部导航-->
<template>
    <div>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" @click="back"/>
            <p>文章详情</p>
        </div>
        <div class="wj-detail-cont" v-for="(item,index) in articleDetailList" :key="index">
            <h5 style="margin: 0.26666666666666666rem auto 0.4rem">{{ item.title }}</h5>
            <div  v-html='item.content'></div>
        </div>
    </div>
</template>

<style scoped>
    .wj-detail-cont{
        padding: 1.4666666666666666rem .4rem 0;
    }
    .wj-detail-cont h5{
        font-weight: normal;
        font-size: .36rem;
        color: #444;
        margin: .1rem 0 .2rem;
        text-align: center;
    }
    .wj-detail-cont p{
        font-size: .3rem;
        color: #666;
        margin: .1rem 0;
        line-height: .6rem;
        text-indent: .5rem;
        white-space: normal;
    }
</style>

<script>
  import $ from 'jquery'

  let articledetail = {
    data() {
      return {
        articleDetailList: []
      }
    },
    created() {
      this.getArticleDetail();
    },
    methods: {
      //返回页面
      back() {
        this.$router.go(-1)
      },
      // 获取新闻详情的主要内容API
      getArticleDetail() {
        $.ajax({
          url: '/Webapi/Notice/NoticeDetail?id=' + this.$route.params.id,
          type: "get",
          complete: (xhr, textStatus) => {
            // 失败
            if ((xhr.readyState !== 4) && (xhr.status !== 200)) {
              alert(xhr.responseJSON.message);
              return;
            }
            // 成功
            if (xhr.responseJSON.status === 0) {
              this.articleDetailList = xhr.responseJSON.data;
            }
          }
        });
      }

    }
  }
  export {articledetail as default}
</script>