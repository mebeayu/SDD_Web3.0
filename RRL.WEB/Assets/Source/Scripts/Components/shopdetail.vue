<template>
    <div>
        <shopdetailheader></shopdetailheader>
        <div class='shopdetail-sort'>
            <span>爆款商品</span>
            <img src='/Assets/Source/img/火热@3x.png'/>
        </div>
        <sortlistcell></sortlistcell>
    </div>    
</template>

<script>
    import shopdetailheader from './shopdetail/shopdetail-header';
    import sortlistcell from './sort-of-goods/sort-list-cell'

    let shopdetail={
        components:{
            shopdetailheader,
            sortlistcell
        }
    }

    export default shopdetail
</script>