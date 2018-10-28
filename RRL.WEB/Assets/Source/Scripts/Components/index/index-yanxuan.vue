<template>
    <div class='indexsortbox'>
        <div class='sort-title'>
            每日严选
        </div>
        <div class='yanxuan'>
            <div class='left' v-on:click='todel($store.state.hotgoodslist[0].id)' 
                v-if='this.$store.state.hotgoodslist[0]'>
                <img v-bind:src='$store.state.imgs + $store.state.hotgoodslist[0].pic_id+$store.state.img_suffix'
                :onerror="$store.state.defautbanner" :load='$store.state.defautbanner'/>
            </div>
            <div class='right'>
                <div class='top' v-on:click='todel($store.state.hotgoodslist[1].id)'
                v-if='this.$store.state.hotgoodslist[1]'>
                    <img v-bind:src='$store.state.imgs + $store.state.hotgoodslist[1].pic_id+$store.state.img_suffix'
                    :onerror="$store.state.defaultimg" :load='$store.state.defaultimg'/>
                </div>
                <div class='bottom' v-on:click='todel($store.state.hotgoodslist[2].id)'
                v-if='this.$store.state.hotgoodslist[2]'>
                    <img v-bind:src='$store.state.imgs + $store.state.hotgoodslist[2].pic_id+$store.state.img_suffix'
                    :onerror="$store.state.defaultimg" :load='$store.state.defaultimg'/>
                </div>

            </div>
        </div>
        <ul class='yanxuan-dot'>
            <li class='on'></li>
        </ul>
    </div>

</template>
<style>
    
</style>
<script>
    const indexyanxuan = {
        data() {
            return {
                yanxuangoods:[]
            }
        },
        created(){
            this.$store.dispatch('sethotgoods')
        },
        computed:{
            hot() {
                return this.$store.state.hotgoodslist
            }
        },
        watch: {
            hot() {
            }
        },
        methods:{
            todel(id) {
                this.$router.push('/goodsdetail/'+ id +'/null')
            }
        }

    }


    export default indexyanxuan;
</script>