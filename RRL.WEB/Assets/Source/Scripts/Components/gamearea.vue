<template>
    <div>
        <!--游戏主界面开始-->
        <iframe :src="iframeVal"
                width="100%"
                height="100%"
                frameborder="0"
                scrolling="auto"
                style="position:absolute;top:0;left:0;"></iframe>
        <!--游戏主界面结束-->

        <!--底部导航开始-->
        <bottom></bottom>
        <!--底部导航结束-->
    </div>
</template>

<script>
    import $ from "jquery";
    import bottom from "./bottom";

    const gamearea = {
        data() {
            return {
                iframeVal: ""
            }
        },
        created() {
            this.iframeVal = '/#/GameHall?token=' + this.$store.state.shorttoken;
        },
        components: {
            bottom
        }
    }
    export default gamearea
</script>