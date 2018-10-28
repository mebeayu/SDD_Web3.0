var blob_a = [];
    function loadVideoFully(url, v_id) {
        GET(url);
     
        function onProgress(event) {
            if (event.lengthComputable) {
                var completion = (event.loaded / event.total) * 100;
               // console.log(completion);
            }
        }
     
        function onLoad(event) {
            if (this.status === 200) {
                var type = 'video/mp4';
                window['blob' + v_id] = new Blob([event.target.response], {
                    type: type
                });
                window.URL = window.URL || window.webkitURL;
                blob_a.push(window.URL.createObjectURL(window['blob' + v_id]));
                if (v_id == 0) {
                    v_id++;
                    loadVideoFully(
                        'img/v11.mp4?r=2', v_id);
                } else if (v_id == 1) {
                    v_id++;
                    loadVideoFully(
                        'img/v2.mp4?r=2', v_id);
                } else if (v_id == 2) {
                    v_id++;
                    loadVideoFully(
                        'img/v22.mp4?r=2', v_id);
                } else if (v_id == 3) {
                    v_id++;
                    loadVideoFully(
                        'img/v3.mp4?r=2', v_id);
                } else if (v_id == 4) {
                    v_id++;
                    loadVideoFully(
                        'img/v33.mp4?r=2', v_id);
                } else if (v_id == 5) {
                   v_id++;
                    loadVideoFully(
                        'img/v4.mp4?r=2', v_id);
                } else if (v_id == 6) {
                    v_id++;
                    loadVideoFully(
                        'img/v44.mp4?r=2', v_id);
                }else if (v_id == 7) {
                    $(".loading").hide();
                   setTimeout(function(){
                        $('.paneltemp').children().addClass("fade")
                    },500)
                }
            }
        }
     
        function GET(url) {
            var xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);
            xhr.responseType = 'blob';
            xhr.onprogress = onProgress;
            xhr.onload = onLoad;
            xhr.send();
        }
    }
    loadVideoFully('img/v1.mp4?r=2',0)
    var curindex=0;
    var u = navigator.userAgent;
var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
    var arr = [
        "img/c1.mp4?r=2","img/c1.mp4?r=2",
        "img/c1.mp4?r=2","img/c1.mp4?r=2",
        "img/c1.mp4?r=2","img/c1.mp4?r=2",
        "img/c1.mp4?r=2","img/c1.mp4?r=2",
    ]
    // function loadvideo(url,call){
    //     $("").load(url,call)
    // }

    // function loadvideos(){
    //     var index = 0,len=arr.length;


    //     function loop(){
    //         if(index >= len){
    //             video.src=arr[0]
    //             setTimeout(function(){
    //                 $('.paneltemp').children().addClass("fade")
    //             },500)
    //             return
    //         }
    //         loadvideo(arr[index],function(){
    //             index++;
    //             loop()
    //         })

    //     }
    //     loop();
        

    // }
    // loadvideos();


    $(function(){
        var type;
        var video =$(".page2 video")[0];





        function createVideo(){
            $(video).remove()
            $(".videotemp").html('<video id="video" preload="auto" x-webkit-airplay="allow"  style="object-fit:fill"  src="" x5-video-player-fullscreen="true" x5-video-orientation="portraint" x5-video-player-type="h5" playsinline webkit-playsinline></video>')
            video=$("#video")[0]


        video.addEventListener('ended',function(){
            if(type==1){

            $(".v"+(curindex+1) ).show()
        }else{

            $(".end").show()
            //$(video).attr("x5-video-player-fullscreen",false)
            //alert(3)
            if(isAndroid){
                $(".page2").hide();
            }
            
        }
        })
        video.addEventListener("x5videoenterfullscreen", function(){
  //alert("player enterfullscreen");
})


        video.addEventListener("x5videoexitfullscreen", function(){
                    $(".page2").show();

       // video.style.width = window.innerWidth+"px"
        })
            
        }


        function playVideo2(){

                    video.src=videostemp[1]
                    video.play();
                    type=2;

        }


    //运动事件监听
    if (window.DeviceMotionEvent) {
        window.addEventListener('devicemotion',deviceMotionHandler,false);
    }

    //获取加速度信息
    //通过监听上一步获取到的x, y, z 值在一定时间范围内的变化率，进行设备是否有进行晃动的判断。
    //而为了防止正常移动的误判，需要给该变化率设置一个合适的临界值。
    var SHAKE_THRESHOLD = 4000;
    var last_update = 0;
    var x, y, z, last_x = 0, last_y = 0, last_z = 0;
    function deviceMotionHandler(eventData) {
            var acceleration =eventData.accelerationIncludingGravity;
            var curTime = new Date().getTime();
            if ((curTime-last_update)> 10) {
                var diffTime = curTime -last_update;
                last_update = curTime;
                x = acceleration.x;
                y = acceleration.y;
                z = acceleration.z;
                var speed = Math.abs(x +y + z - last_x - last_y - last_z) / diffTime * 10000;
                if (speed > SHAKE_THRESHOLD && curindex ==1 && $(".v2:visible").length) {
                    $(".v2").hide();
                    playVideo2();
                }
                last_x = x;
                last_y = y;
                last_z = z;
            }
    }

    touch.on('.page2', 'swipeup', function(ev){

        if(curindex != 3){
            return
        }
            $(".v4").hide();
                    playVideo2();

    });
    window.onresize=function(){
        //video.style.width = window.innerWidth+"px"
        //video.style.height = window.innerHeight+"px"
    }
        //$(video).width($(window).width())

        $('.paneltemp').children().click(function(){
            var index = $(this).index();
            curindex=index;
            videostemp=[blob_a[index*2],blob_a[index*2+1]]

            $(".page1").hide()
            $('.paneltemp').children().removeClass("fade")
            $(".page2").show()

        createVideo();
                video.src=videostemp[0]
                video.play();
                type=1;

        })

        $(".v1center").click(function(){
            //$(".page2").hide()
            //$(".page4").show()
            $(".v1").hide();
                    playVideo2();

        })
        var timer = null;
        $(".v3center").on("touchstart",function(){
            if(curindex != 2){
                return
            }
             timer = setTimeout(function(){
                    $(".v3").hide();
                    playVideo2();
             },1000)
        })

        $("body").on("touchend",function(){
            clearTimeout(timer);
        })

        $(".sharebtn").click(function(){
            $(".share").show();
        })

        $(".changebtn").click(function(){
            $(".page2").hide();
            $(".page1").show();
            $(".end").hide();
            setTimeout(function(){
                $('.paneltemp').children().addClass("fade")
            },100)
        })

        if($(".paneltemp").height() > $(window).height()){
            $(".paneltemp").css("transform","scale("+($(window).height()/$(".paneltemp").height())+")")
        };

        //$(".paneltemp").css("transform")
    })