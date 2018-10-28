
(function(){
    function luckyRedPacket(token,payMoney,redpacket,couponsMoney){
        var html = '<div id="game_alert_box" class="game_alert_box" style="width: 100%;height: 100%;background:rgba(0,0,0,.5);position: fixed; top: 0;z-index: 9999;"> '
            +'<div id="game_alert" class="game_alert" style="background: url(https://e-shop.rrlsz.com.cn/Assets/Game/img/dialog_05.png) no-repeat center top; background-size:100% 100%; width:70%; height:54%; position: absolute; top: 0; left:0; bottom: 0; right: 0; margin: auto; z-index: 11;">'
            +'<div id="game_alert_close" class="game_alert_close"  style="width: 15%; height: 12%; position: absolute; top:0; right:0; z-index: 998;"></div>'
            +'<div class="game_alert_con" style="color: #efeb62; width: 100%;text-align: center; position: absolute; bottom: 24%; font-style: normal;">'
            +'<p style="font-size:inherit; font-weight: bold;margin:0 0 5%">仅需 <em style="font-style: normal;">' + payMoney + '</em> 元</p>'
            +'<p style="font-size:inherit;width:90%; margin: 0 auto;letter-spacing:-1px;"><span style="color:#f8fec2;">即可获得</span> <em style="font-style: normal;">' + redpacket + '</em> V红包及</p>'
            +'<p style="color:#f8fec2;"> + </p>'    
            +'<p> <em style="font-style: normal; ">' + couponsMoney + '</em>购物券</p>'    
            +'</div>'
            +'<div id="game_alert_btn" class="game_alert_btn" style="width:68%;height:12%;position:absolute;bottom:8%;right:16%;z-index: 998;"></div>'
            +'<div class="animate_lh" style="width: 150%;height:100%;overflow:hidden;height:auto;position: absolute; top: 0; left:0; z-index: 11;">'
            +'<img style="width:80%;height:auto;"  src="https://e-shop.rrlsz.com.cn/Assets/Game/img/lh.gif"/>'
            +'</div> '
            +'</div>'
            +'</div>';
        $("body").append(html);
        $("#game_alert_close").on("click",function(){
            $("#game_alert_box").remove();
        })
        $("#game_alert_btn").on("click", function () {
            $("#game_alert_box").remove();
            window.location.href ='https://e-shop.rrlsz.com.cn/game/ReceiveTimeIntervalRedpaket?token='+encodeURIComponent(token)+ "&redirect="+encodeURIComponent(location.href);
        })
        var width = document.documentElement.clientWidth;
        var height =  document.documentElement.clientHeight;
        if( width>height ){
            console.log(width + " " + height);
            $print =  $("#game_alert");
            $print.css("width","53%");
            $print.css("height","100%");
        }
    }
    window.luckyRedPacket = luckyRedPacket;
})();
