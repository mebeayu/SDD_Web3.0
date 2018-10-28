<template>
    <div class='coinsbox'>
        <div class="hot-goods-title-new">
            <p>金币专区</p>
            <router-link tag='p' to='/coinspage'>查看更多</router-link>
        </div>
        <ul class='coinscell'>
            <li v-for='item in this.$store.state.coinlist' v-on:click='todetail(item.id)'>
                <img v-bind:src='$store.state.imgs+item.pic_id+$store.state.img_suffix' :onerror="$store.state.defaultimg"/>
                <p>
                    {{item.name}}
                </p>
                <p>
                    {{item.propaganda }}
                </p>
                <p>
                    ￥{{item.price }} + {{item.discount }}金币
                </p>
                <p>
                    官方指导价:￥ {{item.local_price }}
                </p>
            </li>
        </ul>
    </div>
</template>

<script>
    let coins = {
        methods:{
            todetail(id) {
                this.$router.push('/goodsdetail/' + id + '/null' );
            }
        }
    }   

    export default coins 
</script>