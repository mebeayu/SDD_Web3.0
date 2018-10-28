<template>
	<div>
		<p class="time">{{this.time}}</p>
	</div>
</template>

<script>
	let walletime={
		data(){
			return {
				time:''
			}
		},
		props:['addtime'],
		created(){
			var dataone =this.addtime.split("T");
			var datatwo = dataone[1].split(".");
			this.time =dataone[0]+'    '+datatwo[0];
		}
		
	}
	export default walletime
</script>

<style>
</style>