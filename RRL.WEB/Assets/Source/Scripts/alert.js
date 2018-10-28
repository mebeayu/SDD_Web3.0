// import $ from 'jquery'
window.alert =  function(str) {
    $(".shadow1").remove();
    const box = `<div class="shadow1" id="shadow1" style="z-index:10">
                <div class="tiplogout">
                    <p>` + str+`</p>
                    <div id='test'>
                        <div class="alertdialog" id="alertdialog">确定</div>
                    </div>
                </div>
            </div>`
    $("body").append(box);
    $(".alertdialog").on("click",function(event){
		$(".shadow1").remove();
	})
}
// window.showalert = window.alert;
window.confirm = function(str, callback) {
    $(".shadow1").remove();
    const box = `<div class="shadow1" id="shadow1" style="z-index:10">
                <div class="tiplogout">
                    <p>` + str+`</p>
                    <div id='test'>
                        <div class='surebtn'>确定</div>
                        <div class='cancelbtn'>取消</div>
                    </div>
                </div>
            </div>`
    $("body").append(box)
    $(".surebtn").on("click", function(){
        callback();
        $(".shadow1").remove();
    });
    $(".cancelbtn").on("click", function(){
        $(".shadow1").remove();
    });
    $(document).on("click",function(){
        $(".shadow1").remove();
    });
}