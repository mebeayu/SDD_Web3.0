<!-->新建地址<-->
<template>
    <div>
        <div class="back-page-btn" id="carttop">
            <img src="/Assets/Source/img/返回@3x.png" v-on:click="back" />
            <p>编辑地址</p>
            <span class="save-edit-btn" v-on:click="save">保存</span>
        </div>
        <ul class="editadd" id='first-item'>
            <li>
                <input v-model="name" placeholder="姓名" />
            </li>
            <li>
                <input v-model="phone" placeholder="手机号码" v-on:blur="getphone" />
            </li>
            <li>
                <input placeholder="省份 、城市、区县" v-on:click="seletarea" v-model="aaa" />
            </li>
            <li>
                <input v-model="street" placeholder="详细地址，如街道、楼牌号等" />
            </li>
        </ul>
        <div class="setdef" v-on:click="setdefault(true)" v-if="isdefault==false">
            <img src='/Assets/Source/img/未选择@3x.png' />
            设为默认地址
        </div>
        <div class="setdef" v-on:click="setdefault(false)" v-else>
            <img src='/Assets/Source/img/选择@3x.png' />
            设为默认地址
        </div>
        <div class="erromsg">
            {{erro}}
        </div>
        <div class="shadow" v-if="showarea==true">
            <ul class="selectarea">
                <li class="SAbtn">
                    <button v-on:click="cancel">取消</button>
                    <button v-on:click="confirm">确定</button>
                </li>
                <li class="SAshow">
                    已选
                    <span>{{pro}}-{{city}}-{{area}}</span>
                </li>
                <li class="SAarea">
                    <div v-on:click="title(1)" v-bind:class="[firstpath==true?'in':'']">省</div>
                    <div v-on:click="title(2)" v-bind:class="[secondpath==true?'in':'']">市</div>
                    <div v-on:click="title(3)" v-bind:class="[thirdpath==true?'in':'']">区</div>
                </li>
                <li class="thearea" v-if="firstpath==true">
                    <div v-for="(item,key) in Provinces" v-on:click="province(key)">
                        {{item}}
                    </div>
                </li>
                <li  class="thearea" v-if="secondpath==true">
                    <div v-for="(item,key) in Cities" v-on:click="City(key)">
                        {{item}}
                    </div>
                </li>
                <li class="thearea" v-if="thirdpath==true">
                    <div v-for="(item,key) in Areas" v-on:click="Area(key)">
                        {{item}}
                    </div>
                </li>
            </ul>
        </div>
    </div>
</template>

<style scoped>
    .save-edit-btn{
        position: absolute;
        right: .3rem;
        top: 50%;
        font-size: .4rem;
        color: #fff;
        padding: .2rem .4rem;
        background: #ba3027;
        border-radius: .1rem;
        transform: translateY(-50%);
    }
</style>

<script>
    //获取地址字典
    import zone from '../ZoneCode'

    let editadd={
        data(){
            return {
                name:'',
                phone:'',
                street:'',
                areacode:'',
                showarea:false,
                Provinces:{},
                Cities:{},
                Areas:{},
                firstpath:true,
                secondpath:false,
                thirdpath:false,
                pro:'',
                city:'',
                area:'',
                aaa:'',
                areacode:'',
                isdefault:false,
                reg:/^1[0-9]{10}$/,
                erro:'',
            }
        },
        created(){
            this.Provinces=zone.Provinces
        },
        mounted(){
            window.scrollTo(0,0);
        },
        methods:{
            //获取手机号
            getphone(){
                if(this.reg.test(this.phone)){
                    this.erro=''
                }else{
                    this.erro="请输入正确的手机号码"
                }
            },
            //选区
            seletarea(){
                let that=this
                that.showarea=true
            },
            //选省
            province(e){
                let that=this
                this.pro=this.Provinces[e]
                this.Cities=zone.Cities[e]
                this.firstpath=false
                this.secondpath=true
                this.thirdpath=false
                this.areacode=e
                //传area code
                sessionStorage.setItem("areacode", e)
            },
            //选市
            City(e){
                this.city=this.Cities[e]
                this.Areas=zone.Areas[e]
                this.firstpath=false
                this.secondpath=false
                this.thirdpath=true
                this.areacode=e
                sessionStorage.setItem("areacode", e)
            },
            //选区
            Area(e){
                this.area=this.Areas[e]
                this.areacode=e
                sessionStorage.setItem("areacode", e)
            },
            //3级页面头部跳转
            title:function(e){
                if(e==1){
                    this.firstpath=true
                    this.secondpath=false
                    this.thirdpath=false
                    this.city=''
                    this.area=''
                }else if(e==2){
                    this.firstpath=false
                    this.secondpath=true
                    this.thirdpath=false
                    this.area=''
                }else{
                    this.firstpath=false
                    this.secondpath=false
                    this.thirdpath=true
                }
            },
            //选区确认
            confirm:function(){
                this.showarea=false
                this.aaa=this.pro+this.city+this.area
            },
            //选区取消
            cancel(){
                this.showarea=false
            },
            //保存地址信息并返回上页
            save(){
                let info={
                    name:this.name,
                    address:this.aaa+this.street,
                    mobile:this.phone,
                    area_code:this.areacode,
                    token:this.$store.state.shorttoken,
                    zip_code:''
                }
                if(this.erro==''){
                    this.$store.commit('setnewadd',info)
                    if(this.isdefault==true){
                        this.$store.dispatch('adddefaultaddress')
                        //this.$router.go(-1)
                    }else{
                        this.$store.dispatch('addnewaddress')
                        //this.$router.go(-1)
                    }
                    // this.$router.go(-1)
                }else{
                }

            },
            //返回上页
            back(){
                this.$router.go(-1)
            },
            //是否设为默认地址
            setdefault(e){
                this.isdefault=e
            }
        }
    }

    export default editadd
</script>