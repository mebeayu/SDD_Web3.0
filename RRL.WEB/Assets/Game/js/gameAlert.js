
(function(){
    var count=1.2;
    var redpacket = 3;

    function gameAlert(token){
        var html = '<div id="game_alert_box" class="game_alert_box" style="width: 100%;height: 100%;background:rgba(0,0,0,.5);position: fixed; top: 0;z-index: 9999;"> '
            + '<div id="game_alert" class="game_alert" style="background: url(https://e-shop.rrlsz.com.cn/Assets/Game/img/game_alert.png) no-repeat center top; background-size:100% 100%; width:70%; height:46%; position: absolute; top: 0; left:0; bottom: 0; right: 0; margin: auto; z-index: 11;">'
            +'<div id="game_alert_close" class="game_alert_close"  style="width: 10%; height: 10%; position: absolute; top: 7%; right: 3%; "></div>'
            +'<div class="game_alert_con" style="color: #666; width: 100%;text-align: center; position: absolute; bottom: 24%; font-weight: bold; font-style: normal;">'
            + '<p style="font-size:inherit">仅需 <em style="font-style: normal; color: #fc2626;">' + count + '</em> 元</p>'
            + '<p style="font-size:inherit;width:90%; margin: 0 auto;letter-spacing:-1px;">即可获得 <em style="font-style: normal; color: #fc2626;">' + redpacket + '</em> 红包及购物券</p>'
            +'</div>'
            +'<div id="game_alert_btn" class="game_alert_btn" style="width:87%;height:13%;position:absolute;bottom:9%;right:6%;"></div>'
            +'</div>'
            +'</div>';
        $("body").append(html);
        $("#game_alert_close").on("click",function(){
            $("#game_alert_box").remove();
        })
        $("#game_alert_btn").on("click", function () {
            $("#game_alert_box").remove();
            window.location.href = "https://e-shop.rrlsz.com.cn/game/RechargeForGiveRedpackage?token=" + token;
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
    window.gameAlert = gameAlert;
})();
