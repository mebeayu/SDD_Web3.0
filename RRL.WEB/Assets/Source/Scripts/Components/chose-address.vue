<!-->编辑收货地址<-->
<template>
    <div>
        <viewtitle></viewtitle>
        <ul class="addList" style="margin-bottom:80px" id='first-item'>
            <li class="addcell" v-for="item in this.$store.state.addresslist" v-on:click='choseadd(item)'>
                <div style="display:flex;align-items:center;padding:15px;padding-right:0;">
                    <!--input v-bind:id="'check'+index" type="checkbox" style="display:none;" v-model="toggle[index]" v-on:click.stop="test(item.id,index)" />
                    <label v-bind:for="'check'+index">
                        <img src="/Assets/Source/img/选择@3x.png" class="choseBTN" v-if="toggle[index]==true" />
                        <img src="/Assets/Source/img/未选择@3x.png" class="choseBTN" v-else />
                    </label-->
                    <div class="addcell-info">
                        <p>
                            <span>{{item.name}}</span>
                            <span>{{item.mobile}}</span>
                            <img src="/Assets/Source/img/默认@3x.png" v-if="item.is_default==true">
                        </p>
                        <p>
                            {{item.address}}
                        </p>
                    </div>
                </div>
                <div class="deladd" v-on:click.stop="toconfirm(item.id)">
                    删除
                </div>
            </li>
        </ul>
        <router-link class="newadd" tag="div" to="/editadd">
            <div>
                <img src="/Assets/Source/img/增加@3x.png">
                新建地址
            </div>
        </router-link>
        <div class="shadow" v-if="confirm==true">
            <div class="confirmbox">
                <p>确认删除此地址？</p>
                <div class="conbtn">
                    <div v-on:click="deladd">确定</div>
                    <div v-on:click="cancel">取消</div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import viewtitle from './title';

    let choseadd={
        data(){
            return {
                confirm:false,
                id:'',
            }
        },
        components:{
            viewtitle
        },
        created(){
            //获取地址列表
            this.$store.dispatch('getaddlist')

        },
        mounted(){
            window.scrollTo(0,0);
            //获取地址列表
            this.$store.dispatch('getaddlist')
        },
        methods:{
            //确认删除地址弹出框
            toconfirm(e){
                this.confirm=true
                this.id=e
            },
            //删除地址
            deladd(){
                this.$store.dispatch('deladd',this.id)
                location.reload()
            },
            //取消删除地址弹出框
            cancel(){
                this.confirm=false
            },
            //选中该地址并返回上页
            choseadd(e){
                this.$store.commit('setreceiveinfo',e)
                this.$router.go(-1)
            }
        }
    }

    export default choseadd
</script>