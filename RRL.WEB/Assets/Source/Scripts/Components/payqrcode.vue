<template>
    <div class='shadow'>
        <qrcode v-bind:value='this.$store.state.payqrcode' :options="{ size: 200 }"
            style = 'position:absolute;top:50%;
            left:50%;margin-left:-100px;
            margin-top:-100px;'>
        </qrcode>
    </div>

</template>

<script>
    let payqrcode = {
        
    }  


    export default payqrcode
</script>