window.alert = function(str,type){
	$("#mask,#dialog").remove();
	console.log(type)
	// if(params && params.type == 2){
	// 	btn = '<div style="text-align:center;color:#3699ba;height:.8rem;line-height:.8rem"><span id="dialog_btn1" style="width:50%;display:inline-block;">'+params.btn1.text+'</span><span id="dialog_btn2" style="width:50%;display:inline-block;">'+params.btn2.text+'</span></div>'
	// }else{
	// 	btn = '<div id="dialog_close" style="text-align:center;color:#3699ba;height:.8rem;line-height:.8rem">好</div>'
	// }

	// var html = '<div id="mask" style="width:100%;height:100%;position:fixed;background:#000;opacity:.7;z-index:9998;left:0;top:0"></div>';
	// html += '<div id="dialog" style="font-size:.3rem;box-shadow:0 1px 3px rgba(0,0,0,.3);position:fixed;z-index:9999;left:0;top:0;right:0;bottom:0;width:4.1rem;height:2.3rem;background:#fff;margin:auto;border-radius:0.15rem"><div style="text-align:center;padding:.25rem .5rem;height:1rem;border-bottom:1px solid #ddd;" id=""><img id="alertimg"/>'+str+'</div><div id="alertbtn"></div></div>'
	var html = '<div id="mask" style="width:100%;height:100%;position:fixed;background:#000;opacity:.7;z-index:9998;left:0;top:0"></div>\
			<div id="dialog" style="font-size:.3rem;box-shadow:0 1px 3px rgba(0,0,0,.3);position:fixed;z-index:9999;left:0;top:0;right:0;bottom:0;width:4.1rem;height:4.3rem;background:#fff;margin:auto;border-radius:0.15rem">\
				<div style="text-align:center;padding:.25rem .25rem;height:3rem;" id="">\
					<img id="alertimg" style="margin-bottom:0.5rem"/>\
					<p id="aletinfo" style="color:rgb(84,181,252)"></p>\
				</div>\
				<div id="alertbtn">\
					<p id="alertdialog" style="width:2rem;background:url(\'/Assets/Game/img/圆角矩形17.png\') no-repeat; background-size:100% 100%; margin: 0 auto; color:#fff;line-height:0.7rem;text-align:center" ></p>\
				</div>\
			</div>';
	$("body").append(html)	
	console.log(typeof str)		
	if(type == 0   ) { //guess wrong
		$("#alertimg").attr({
			'src' : '/Assets/Game/img/金币拷贝.png'
		})
        var info = "很遗憾<span style='color:#ba3027'>猜错了</span>,金豆去火星了~";
		$('#aletinfo').html(info)
		$('#alertdialog').text("再来一局")
	} else if(type == 1  ) {
		$("#alertimg").attr({
			'src' : '/Assets/Game/img/金币拷贝2.png'
		})
        var info = "<p>恭喜您中奖，获得了<span style='color:rgb(251, 177, 26)'>" + str + "</span>金豆~</p>";
						 
		$('#aletinfo').html(info)
		$('#alertdialog').text("再来一局")
	} else if(type == 2) {
		$("#alertimg").attr({
			'src' : '/Assets/Game/img/底部线5.png'
		})
		$('#aletinfo').text(str)
		$('#alertdialog').text("知道了")
    } else if (type == 3) {
        $("#alertimg").attr({
            'src': '/Assets/Game/img/金币拷贝2.png'
        })
        var info = "<p>恭喜您中奖，获得了<span style='color:rgb(251, 177, 26)'>" + str + "</span>免费红包~</p> ";
        $('#aletinfo').html(info)
        $('#alertdialog').text("再来一局")
    }
    else
    {
		$("#alertimg").attr({
			'src' : '/Assets/Game/img/底部线5.png'
		})
		$('#aletinfo').html(str)
		$('#alertdialog').text("知道了")
	}

	$("#alertdialog").on("click",function(){
		$("#mask,#dialog").remove();



		if(str == "对不起，您在10秒内没有摇动手机"){
			location.href = "index.html?r="+Math.random()
		}
	})
	$("#dialog_btn1").on("click",function(){
		params.btn1.cb();
	})
	$("#dialog_btn2").on("click",function(){
		params.btn2.cb();
	})
}

window.showalert = window.alert;