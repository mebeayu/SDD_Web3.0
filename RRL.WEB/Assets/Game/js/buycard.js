
    var cardnum = 1;
    var _input = $('#valueofnum');
    var cardlistshow = false;
    var youhuijuan, chosedcard;
    var request;

    _input.attr("value",cardnum);

    function GetRequest() {

        var url = location.search; //获取url中含"?"符后的字串
    
        var theRequest = new Object();
    
        if (url.indexOf("?") != -1) {
    
        var str = url.substr(1);
    
        strs = str.split("&");
    
        for(var i = 0; i < strs.length; i ++) {
    
            theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]);
    
        }
    
        }
    
        return theRequest;
    
    }

    request = GetRequest()
    console.log(request);

    function buycard() {
        cardlistshow = true;
        $.ajax({
            url:'/WebApi/Game/KaQuanList',
            type:'get',
            success:function(res) {
                youhuijuan = res.data;
                console.log(youhuijuan)
                rendercard(youhuijuan)
                chosedcard = youhuijuan[0]
                rendermoney()
            }
        })
        
        $(".buy-card").css({
            "display" : "block"
        })
    }




    $(".closebtn").click(function() {// 购卡界面关闭
        //$(".buy-card").css({
        //    "display" : "none"
        //})
    })

    function rendercard (list) { //渲染优惠券列表
        console.log(list)
        var $ul = $('.card-list');
        var li_len = list.length;
        var money = 0, name = '', daijinquan = 0, jindou = 0;
        
        var liarr = '';
        for(var i =0 ; i< li_len; i++) {
            money = list[i].money;
            name = list[i].goodsname;
            daijinquan = list[i].countmoney;
            jindou = list[i].goldbean;
            syjindou=jindou-(jindou%1000);
            jinbi = list[i].goldcoin;
            startTime=list[i].starttime;
            endtime=list[i].endtime;
            needStartTime=startTime.substr(0,10);
            needEndtime=endtime.substr(0,10);
            var str='';
            if(jindou>1000&&jindou%1000!=0){
                str=`<div class="sendjindou">
                        <p class="ps sendj_tl">送金豆</p> 
                        <p class="ps sendj_zs">`+syjindou+`</p>
                        <p class="ps sendj_plus">+</p>
                        <p class="sendj_ys">`+jindou%1000+`</p>
                    </div>`;
            }else{
                str=`<div class="sendjindou1">
                    <p class="ps sendj_tl">送金豆</p> 
                    <p class="ps sendj_zs">`+jindou+`</p>
                </div>`;
            }

            if(daijinquan == 0 ){
                var li = `<li class="list_li">
                <div class='cl-top'>
                    <div class='cl-left'>
                        <span class="cl-left-log"></span>
                        <div class="cardtime_box">
                        <p class="cl_price">￥<strong>`+  money +`</strong></p>   
                        <p class="cardtime">使用时间：`+needStartTime+`-`+needEndtime+`</p>
                        </div>
                    </div>
                    <div class='cl-right'>
                        `+str+`
                    </div>
                </div> 
                <div class ='cl-bottom'>
                    ` + name + jinbi + `元
                </div>
            </li>`;
            } else if(jinbi == 0){
                var li = ` <li class="list_li">
                <div class='cl-top'>
                    <div class='cl-left'>
                        <span class="cl-left-log"></span>
                        <div class="cardtime_box">
                        <p class="cl_price">￥<strong>`+  money +`</strong></p>   
                        <p class="cardtime">使用时间：`+needStartTime+`-`+needEndtime+`</p>
                        </div>
                    </div>
                    <div class='cl-right'>
                        `+str+`
                    </div>
                </div> 
                <div class ='cl-bottom'>
                    ` + name + daijinquan + `元
                </div>
            </li>`;
            }
            
            liarr += li


            
        }
        $ul.html(liarr)
        $(".card-list li:first").addClass('lion')
            cardli = $(".card-list li")
            cardli.click(chosecardli)

        var listLi=$('.list_li');
        listLi.each(function(index){
            var  price=listLi.eq(index).find('strong').html();
            if(price==2){
                listLi.eq(index).css("background",'#ee5b6e');
                listLi.eq(index).find('.ps').css("color",'#ee5b6e');
            }
            if(price==5){
                listLi.eq(index).css("background",'#f47e5b');
                listLi.eq(index).find('.ps').css("color",'#f47e5b');
            }
            if(price==10){
                listLi.eq(index).css("background",'#68c39e');
                listLi.eq(index).find('.ps').css("color",'#68c39e');
            }
            if(price==20){
                listLi.eq(index).css("background",'#a1ca69');
                listLi.eq(index).find('.ps').css("color",'#a1ca69');
            }
            if(price==50){
                listLi.eq(index).css("background",'#45c4bb');
                listLi.eq(index).find('.ps').css("color",'#45c4bb');
            }
            if(price==100){
                listLi.eq(index).css("background",'#f4b733');
                listLi.eq(index).find('.ps').css("color",'#f4b733');
            }
            
        })
        
    }

    function rendermoney() { //渲染付款详情
        $(".trulypay").text('实付：￥' + (cardnum * chosedcard.money).toFixed(2))
        $(".getjindou").text((cardnum * chosedcard.goldbean).toFixed(0))
    }

    function chosecardli () { //选择优惠圈
        var _this = $(this);
        // console.log(_this)
        // var lilist = $(".card-list li")
        var li_index = _this.index()
        chosedcard = youhuijuan[li_index]
        // console.log(chosedcard)
        _this.addClass('lion').siblings().removeClass();
        cardnum = 1;
        _input.attr("value",cardnum);
        rendermoney()
    }
    // cardli.click(function(e) { 
        
    // })

    $(".reduce").click(function() { //减少数量
        cardnum --;
        if(cardnum == 0){
            cardnum =1
        }
        _input.attr("value",cardnum);
        rendermoney()
    })

    $(".plus").click(function() { //增加数量
        cardnum ++;
        _input.attr("value",cardnum);
        rendermoney()
    })

    $('.paybtn').click(function() {//买券
        chosedcard.num = cardnum;
        chosedcard.token = request.token;
        var total_money = (cardnum * chosedcard.money).toFixed(2);
    //  console.log(chosedcard)
        $.ajax({
            url:'/WebApi/Game/BuyQuan',
            type:'post',
            data:{
                'id':chosedcard.id,
                'num': chosedcard.num,
                'token': request.token
            },
            success:function(data) {
            //  console.log(data)
            if(data.status == 0) {
                var res = data.data
                var host = window.location.host
                console.log(request.redirect)
                    //  console.log('https://e-shop.rrlsz.com.cn/CouponsPay?list=' + res +'&token=' + encodeURIComponent(token))
                window.location.href = '/CouponsPay?list=' + res + '&token=' + encodeURIComponent(request.token) + '&type=1,2,3&wallet=0&jd=0&cash=' + total_money+'&redirect=' + request.redirect ;

            } else {
                alert(data.message)
            }
            }
        })
    })
    window.onload = function () {
        buycard()
    }

    //$("head").append("<link>");
    //css = $("head").children(":last");
    //css.attr({
    //    rel: "stylesheet",
    //    type: "text/css",
    //    href: "./css/buycard.css"
    //});