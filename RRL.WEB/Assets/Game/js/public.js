 //var m_pay_act = 2
var m_pay_act_money_pre = {
    "1": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 },
    "2": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 },
    "3": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 }
};
//上一次的下单;

var m_pay_act_money = {
    "1": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 },payTimes:0 },
    "2": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 },
    "3": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 }
};//本次押注
 
//xiazhu_type
//da
//xiao
//dan
//shuang
//hong
 

/**
money 押注的钱,正整数 type=1 免费红包,2=金豆,3=小红包
*/
function changeMoneyByPatType(type, money, xiazhu_type) {
    money = Number(money);
    //start条件判断
    if (m_pay_act_money[type].money - money < 0) {
        if (type == "1"||type == "2") {//弹出送豆页面
            //buycard(1);
            return false;
        } else if (type == "3") {//弹出送小红包页面
            //buycard(2);
            return false;
        }
    }

    if (type == 1) { // 免费红包
	//debugger;
	   if ( m_pay_act_money["2"].payTimes >= 1 ||  m_pay_act_money["3"].payTimes >= 1) {
            //alert("用金豆或小红包押注后,不能再换成免费红包!");
            alert("用金豆押注后,不能再换成红包!");
            return false;
        }
		if (xiazhu_type != "hong") {
             
			if((m_pay_act_money[type].yazhu.da>0&&xiazhu_type=="xiao")
				||(m_pay_act_money[type].yazhu.xiao>0&&xiazhu_type=="da")
				||(m_pay_act_money[type].yazhu.dan>0&&xiazhu_type=="shuang")
				||(m_pay_act_money[type].yazhu.shuang>0&&xiazhu_type=="dan"))
			{
				alert("禁止对押，不能同时压大小或者单双！");
				return false;
			}
			
			/*if(money!=1)
			{
				alert("除红外，红包只能押1分!");
                return false;
			}*/
			 
			var ispass=false;
			if(m_pay_act_money[type].yazhu.da+m_pay_act_money[type].yazhu.xiao
			+m_pay_act_money[type].yazhu.dan+m_pay_act_money[type].yazhu.shuang==0
			)
			 {
				 ispass=true;
			 } 
			if((m_pay_act_money[type].yazhu.da>0&&(xiazhu_type=="dan"||xiazhu_type=="shuang"||xiazhu_type=="da")))	
			{
				ispass=true;
			} 
			if(m_pay_act_money[type].yazhu.xiao>0&&(xiazhu_type=="dan"||xiazhu_type=="shuang"||xiazhu_type=="xiao"))
			{
				ispass=true;
			} 
			
			if((m_pay_act_money[type].yazhu.dan>0&&(xiazhu_type=="da"||xiazhu_type=="xiao"||xiazhu_type=="dan")))
			{
				ispass=true;
			} 
			
			if(m_pay_act_money[type].yazhu.shuang>0&&(xiazhu_type=="da"||xiazhu_type=="xiao"||xiazhu_type=="shuang"))
			 {
				 ispass=true;
			 } 
			 /*
			 if(m_pay_act_money[type].yazhu.da>0&&xiazhu_type=="da")	
			{
				ispass=false;
			} 
			if(m_pay_act_money[type].yazhu.xiao>0&&xiazhu_type=="xiao")	
			{
				ispass=false;
			}
			
			if(m_pay_act_money[type].yazhu.dan>0&&xiazhu_type=="dan")	
			{
				ispass=false;
			}
			
			if(m_pay_act_money[type].yazhu.shuang>0&&xiazhu_type=="shuang")	
			{
				ispass=false;
			}
			 */
			 
			 
			 if(!ispass)
			 {
				alert("禁止对押，不能同时压大小或者单双！");
                return false;
			 } 
				 
        }
        if (m_pay_act_money["2"].payTimes >= 1) {
            alert("用红包押注后,不能再换成金豆!");
            return false;
        }
        if (xiazhu_type == "hong" && (m_pay_act_money[type].yazhu.hong + money > m_maxYahong)) {
            alert("押注红不得超过" + m_maxYahong);
            return false;
        }
        if (m_pay_act_money[type].yazhu.da
            + m_pay_act_money[type].yazhu.xiao
            + m_pay_act_money[type].yazhu.dan
            + m_pay_act_money[type].yazhu.shuang
            + m_pay_act_money[type].yazhu.hong

            + money > m_maxYaAll) {
            alert("总押注红包不得超过" + m_maxYaAll);
            return false;
        }
    } else if (type == 2) { // 金豆
        if ( m_pay_act_money["1"].payTimes >= 1 ||  m_pay_act_money["3"].payTimes >= 1) {
            //alert("用金豆或小红包押注后,不能再换成免费红包!");
            //alert("用金豆押注后,不能再换成红包!");
			alert("用红包押注后,不能再换成金豆!");
            return false;
        }
        if (xiazhu_type == "hong" && (m_pay_act_money[type].yazhu.hong + money > m_maxYahong)) {
            alert("押注红不得超过" + m_maxYahong);
            return false;
        }

        if (m_pay_act_money[type].yazhu.da
            + m_pay_act_money[type].yazhu.xiao
            + m_pay_act_money[type].yazhu.dan
            + m_pay_act_money[type].yazhu.shuang
            + m_pay_act_money[type].yazhu.hong
            + money> m_maxYaAll) {
            alert("总押注金豆不得超过" + m_maxYaAll);
            return false;
        }

        /*if (m_pay_act_money[type].payTimes >= 1) {
            alert("免费红包押注,只能押一次");
            return false;
        }*/
    }else if (type == 3) {// 小红包
        if (m_pay_act_money["2"].payTimes >= 1||m_pay_act_money["1"].payTimes >= 1) {
            alert("用金豆押注后,不能再换成红包!");
            return false;
        }
        if (xiazhu_type == "hong" && (m_pay_act_money[type].yazhu.hong + money > m_maxYahong)) {
            alert("押注红不得超过" + m_maxYahong);
            return false;
        }
        if (m_pay_act_money[type].yazhu.da
            + m_pay_act_money[type].yazhu.xiao
            + m_pay_act_money[type].yazhu.dan
            + m_pay_act_money[type].yazhu.shuang
            + m_pay_act_money[type].yazhu.hong
             
            + money > m_maxYaAll) {
            alert("总押注小红包不得超过" + m_maxYaAll);
            return false;
        }
    }
    //end条件判断
    //start减钱
    m_pay_act_money[type].yazhu[xiazhu_type] = m_pay_act_money[type].yazhu[xiazhu_type] + money;
    m_pay_act_money[type].payTimes = m_pay_act_money[type].payTimes + 1;
    m_pay_act_money[type].money = m_pay_act_money[type].money - money;
    $("div[data-act='" + type + "']").find(".hb-number").text(m_pay_act_money[type].money);
    //end减钱
    return true;
}

//获取当前选择的筹码
function getCurrentCheckMoney() {
    var checkMoney = $(".size-temp").find(".cur").attr("data-money");
    return Number(checkMoney);
}
/**
 * 创建押注元素
 * @param {any} $this
 * @param {any} paynumber
 */
function createYazhuElement($this,paynumber) {
    var $conis = $('<div money="' + paynumber + '" class="conis size' + paynumber + ' type' + m_pay_act + '"></div>');
    $this.find(".yz-tip").before($conis)
    $conis.css("left", Math.random() * ($this.width() - $conis.width())).css("top", Math.random() * ($this.height() - $conis.height()))
    referconis($this);
}


function dlg_msg(msg) {

    layer.open({
        content: msg
        , skin: 'msg'
        , time: 2 //2秒后自动关闭
    });
}

function dlg_confirm(msg, fun_yes,fun_no, title) {
    title=title || "省兜兜系统提示";
    layer.open({
        title: [
            title,
            'background-color:#01AAED; color:#fff;'
        ],
        skin: 'footer'

        , anim: 'scale'
        , content: msg
        , btn: ['确认', '取消']
        , yes: fun_yes
    });
}


function redPackage2Beans() {
    //var total_redpackage = spreader_redpackage * freeRedPackage_to_beans_rate;
    //if (m_pay_act_money["1"].money==0) {
    //    alert("免费红包是0,不能转换!");
    //    return;
    //}
    //if (m_pay_act_money["1"].money < total_redpackage) {
    //    alert("免费红包未达到" + total_redpackage + ",继续努力!");
    //    return;
    //}

    dlg_confirm("是否确定把所有免费红包转换成金豆?", function () {
        $.ajax({
            url: '/WebApi/Game/RedPackageBeans',
            data: { token: token },
            type: 'post',
            success: function (res) {
                if (res.status == 0) {
                    dlg_msg("红包转金豆成功");
                    getuser();

                } else {
                    dlg_msg(res.message);
                }

            }
        });
    });
 
}

// 登录判定
if (token.length == 0) {
     dlg_confirm("未能读取到您的登录信息，请重新登录?", function () {
         window.location.href = "/#/login";
     });
     //window.location.reload();
      
} else {
    window.requestNextAnimationFrame = (function () {
        var originalWebkitRequestAnimationFrame = undefined,
            wrapper = undefined,
            callback = undefined,
            geckoVersion = 0,
            userAgent = navigator.userAgent,
            index = 0,
            self = this;

        // Workaround for Chrome 10 bug where Chromeh
        // does not pass the time to the animation function

        if (window.webkitRequestAnimationFrame) {
            // Define the wrapper

            wrapper = function (time) {
                if (time === undefined) {
                    time = +new Date();
                }
                self.callback(time);
            };

            // Make the switch

            originalWebkitRequestAnimationFrame = window.webkitRequestAnimationFrame;

            window.webkitRequestAnimationFrame = function (callback, element) {
                self.callback = callback;

                // Browser calls the wrapper and wrapper calls the callback

                originalWebkitRequestAnimationFrame(wrapper, element);
            }
        }

        // Workaround for Gecko 2.0, which has a bug in
        // mozRequestAnimationFrame() that restricts animations
        // to 30-40 fps.

        if (window.mozRequestAnimationFrame) {
            // Check the Gecko version. Gecko is used by browsers
            // other than Firefox. Gecko 2.0 corresponds to
            // Firefox 4.0.

            index = userAgent.indexOf('rv:');

            if (userAgent.indexOf('Gecko') != -1) {
                geckoVersion = userAgent.substr(index + 3, 3);

                if (geckoVersion === '2.0') {
                    // Forces the return statement to fall through
                    // to the setTimeout() function.

                    window.mozRequestAnimationFrame = undefined;
                }
            }
        }

        return window.requestAnimationFrame ||
            window.webkitRequestAnimationFrame ||
            window.mozRequestAnimationFrame ||
            window.oRequestAnimationFrame ||
            window.msRequestAnimationFrame ||

            function (callback, element) {
                var start,
                    finish;

                window.setTimeout(function () {
                    start = +new Date();
                    callback(start);
                    finish = +new Date();

                    self.timeout = 1000 / 60 - (finish - start);

                }, self.timeout);
            };
    }());

    window.cancelNextRequestAnimationFrame = window.cancelRequestAnimationFrame
        || window.webkitCancelAnimationFrame
        || window.webkitCancelRequestAnimationFrame
        || window.mozCancelRequestAnimationFrame
        || window.oCancelRequestAnimationFrame
        || window.msCancelRequestAnimationFrame
        || clearTimeout;

    var ua = window.navigator.userAgent.toLowerCase();
    var isMobile = ua.indexOf('iphone') > -1 || ua.indexOf('ipad') > -1 || ua.indexOf('android') > -1;


    // if (ua.match(/MicroMessenger/i) !== 'micromessenger') {
    //     $('.downloadtool').style.display = 'none';
    // }
    
    var $zpbg = $(".zp-bg"), // 转盘图片
        $startbtn = $(".start-btn"), // undefined
        $sizelist = $(".size"), // 筹码
        $yztemp = $(".yztemp") //押注
        $buycard = $(".gouwuka") //购卡
    var speed = 2;
    var deg = 0;
    var isloop = false;
    var sigle = 13.846;
    var num = 0;
    var isrun = false;
    var canover = false;
    var index = 1;// 筹码indexindex
    var overdeg = 0;
    var maxoverdeg = 0;
    var user = {};
    var yazhucount = 0;
    var lastyazhucount = 0;
    var addcount = 0;
    var addfreecount = 0;
    
    var yazhu = { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 };//
    var yazhu2 = { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 };//
    var yazhurecord = [[], [], [], [], []]
    var yazhurecord2 = [[], [], [], [], []]
    var lastyazhurecord = [[], [], [], [], []]
    var lastyazhurecord2 = [[], [], [], [], []]
    var jiangsu = false;
    var starttime = new Date().getTime();
    var endtime;
    var rdcode = String(Math.random()).split(".")[1];
    var cardlistshow = false;
    var youhuijuan = [
    ];
    var cardli;
    var chosedcard;
    var cardnum = 1;
    var _input = $('#valueofnum');

    _input.attr("value",cardnum);

    daily = parseFloat(daily/100).toFixed();
    first = parseFloat(first/100).toFixed();
    


    var $xinren = $('.xinren')
    var $meiri = $('.meiri')
    if( has_requestd_first_free_h_money == 'False' ) {
        // 弹个红包 点击后执行 新人红包
        //$xinren.css({
        //    "display" :"block"
        //})
        //$('.xirenjine').text(first + '元');
        
    }
    if( is_can_request_free_h_money == 'True') {
        // 弹个红包 同上 每日红包
        //$meiri.css({
        //    "display" :"block"
        //})
        //$('.meirijine').text(daily + '元');
        
    }
    $('.xirenbtn').click(function(){
        $.ajax({
            url:'/WebApi/Game/FirstHongFreeMoneyRequest',
            type:'post',
            data:{
                'token' : token
            },
            success: function (data) {
                getuser(function () { });
                alert(data.add_data)
            }
        })
        $xinren.css({
            "display" :"none"
        })
    })
    $('.meiribtn').click(function(){
        $.ajax({
            url:'/WebApi/Game/DailyHongFreeMoneyRequest',
            type:'post',
            data:{
                'token' : token
            },
            success: function (data) {
                getuser(function () { });
                alert(data.add_data)
            }
        })
        $meiri.css({
            "display" :"none"
        })
    })

    $('.closebtnoo').click(function(){
        $('.maskoo').css('display', 'none')
        getuser()
        window.location.href='/Product'
    })

    $('.closebtnxx').click(function() {
        $('.maskxx').css('display', 'none')    
        getuser()    
    })

    var wxshare = {}
    $(function () {
        var url1 = window.location.href.split('#')[0]
        url1 = encodeURIComponent(url1)
        $.ajax({
            url: '/WebApi/Public/WxJsApiConfigFroPage?url=' + url1,
            type: "get",
            async: true,
            success: function (data) {
               // console.log(data)
                wxshare = data;
                //wxshare.debug = true;
                wxshare.jsApiList = ['onMenuShareTimeline', 'onMenuShareAppMessage'];
                wx.config(wxshare)
            }
        })
    })

    wx.ready(function () {
        wx.onMenuShareAppMessage({
            title: '呼朋唤友，奖励无限！', // 分享标题
            desc: '友谊小船不翻，红包拿到手软。', // 分享描述
            //link: 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + encodeURIComponent(token), // 分享链接
            link: 'http://sdd.szrundao.com/Event/Xxxshare?token=' + encodeURIComponent(token), // 分享链接
            imgUrl: 'https://e-shop.rrlsz.com.cn/Assets/Event/img/lve.jpg', // 分享图标
            type: '', // 分享类型,music、video或link，不填默认为link
            success: function () {
                //调用分享刷新
                alert('分享成功')
            },
            cancel: function () {
                alert('分享取消')
            }
        });
        wx.onMenuShareTimeline({
            title: '重磅福利！省兜兜红包天天送~', // 分享标题
            //link: 'https://e-shop.rrlsz.com.cn/Event/Xxxshare?token=' + encodeURIComponent(token),
            link: 'http://sdd.szrundao.com/Event/Xxxshare?token=' + encodeURIComponent(token),
            imgUrl: 'https://e-shop.rrlsz.com.cn/Assets/Event/img/lve.jpg', // 分享图标
            success: function () {
                //调用分享刷新
                $.ajax({
                    url:'/WebApi/Game/Refresh',
                    type:'post',
                    data:{
                        'token': token
                    },
                    success:function(data) {
                        // console.log(data)
                        //alert('分享成功')
                        if(data.additional_data !== 0 && data.add_data !==0 ) {
                            $('.maskoo').css('display', 'block')
                            $('.gethbnum').text(data.additional_data  )
                            $('.cangettimes').text(data.add_data)
                        }else if(data.add_data == 0 && data.additional_data !== 0){
                            $('.maskxx').css('display', 'block')
                            $('.gethbnum').text(data.additional_data  )
                        } else if(data.add_data !==0 && data.additional_data == 0) {
                            alert(data.data)
                        }else {
                            alert("分享成功")
                        }
                    }   
                })
            },
            cancel: function () {
                alert('分享取消')// 用户取消分享后执行的回调函数
            }
        });
    });



    function loop() { // 转盘方法
        if (!isloop) { //未开始旋转时，触发
            requestNextAnimationFrame(loop)
            return
        }
        deg += speed
        deg = deg % 360;


        if (!jiangsu) { //降速判定
            if (speed < 20) {
                speed += .2
            } else {
                speed = 20;
            }
        } else {
            if (speed > .3) {
                speed -= .001
            } else {
                speed = .3
            }
        }

        if (canover && (deg >= overdeg && deg <= maxoverdeg)) { // 停止旋转
            isloop = false;
            isrun = false;
            canover = false;
        
            if (addcount > 0 || addfreecount > 0) {
                if (addcount > 0) {
                    var msg = "<p>恭喜您中奖，获得了<span style='color:rgb(251, 177, 26)'>" + addcount +"</span>金豆~</p>";
                    alert(msg);
                } else {
                    var msg = "<p>恭喜您中奖，获得了<span style='color:rgb(251, 177, 26)'>" + addfreecount + "</span>金豆~</p>";
                    alert(msg);
                }
               
            } else {

                alert("很遗憾，您未中奖!");
            }
            
            getuser();
            resetconis();
            getlogslist();
            starttimeloop();
        }

        $zpbg.css("transform", "rotate(-" + deg + "deg)")
        requestNextAnimationFrame(loop)
    }

    // rendercard(youhuijuan)


    if (!isMobile) {
        alert('请在移动端进行操作！');
    }
    else { 
        loop();
    }

    // $startbtn.click(function(){
    // lottery();
    // })
    /*
    点击押注类型
    */
    $(".mfhb,.zshb").click(function () {
        var $this = $(this);
        $(".mfhb,.zshb").removeClass('cur');
        $this.addClass("cur");
        m_pay_act = $this.attr("data-act");
        console.log("点击押注类型" + m_pay_act);
        dispaly_pay_count();

    });
    /*----------显示押注数量-----------*/
    function dispaly_pay_count() {
        return;
        $(".size-temp").empty();
        var payMoneyList = null;
        if (m_pay_act == 1) {//免费红包
            payMoneyList = m_playMoney;//m_playMoney.slice(0, 2);
        } else {
            payMoneyList = m_playMoney;// m_playMoney.slice(2);
             
        }
        for (var i = 0; i < payMoneyList.length; i++) {
            var m = payMoneyList[i];
            var cur = "";
            
            if (i == 0 && m_pay_act != 1)
                cur = "cur";
             if (i == 1 && m_pay_act == 1)
                cur = "cur";
            $(".size-temp").append('<div class="size' + m + ' size ' + cur+'" style="left:'+0.8*i+'rem;" data-money="' + m + '" ></div>');
        }
        $(".size-temp").append('<div class="repair-btn" style="display: block;"></div>');
    }

    /*-----------------------------*/
    function defaultchose() {
        return;
        var hbnum = $(".mfhb .hb-number").text();
        var jdnum = $(".zshb .hb-number").text();
        console.log(jdnum)
        if(hbnum > 300) {

        } else if(hbnum < 300 && jdnum > 300) {
            $(".mfhb").removeClass('cur');
            $(".zshb").addClass("cur");
            m_pay_act = 2;
        }
    }
    /*----------------------*/
    $(".qk-btn").click(function () { // 清空当前下注点击事件

        if (isrun) {
            return
        }
        resetconis();
        getuser();
    })
    $("#wj-rule").click(function () { // 规则按钮点击事件
        $(".gz-dialog").show();
    })
    $(".gz-qd-btn").click(function () { //关闭规则按钮
        $(".gz-dialog").hide();
    })
    $(".repair-btn").click(function () { // 重复下注按钮
        if (isrun) {
            return
        }
        yazhurepair();
    })
    //选中筹码
    $(".size-temp").on("click", ".size", function () { // 选中筹码添加class
        var $this = $(this);
        index = $this.index() + 1;
        $this.addClass("cur").siblings().removeClass("cur")
    });
    $(".sc-btn").click(function (e) { // 商城购物按钮点击
        e.preventDefault();
        window.location.href = 'https://e-shop.rrlsz.com.cn/#/index';
    })
    //筹码押注
    $(".yztemp").click(function () { // 
        if (!isMobile) {
            return;
        }        

        if (isrun) {
            return;
        }
        $this = $(this);
        var paynumber = getCurrentCheckMoney();//押注筹码
        if (paynumber<=0) {
            alert("请选择投入数量");
            return;
        }
        var xiazhu_type = $(this).attr("data-xiazhu-type");
        var canChange=changeMoneyByPatType(m_pay_act, paynumber, xiazhu_type);
        if (!canChange)
            return;
        createYazhuElement($this, paynumber); 
         /*
        var $this = $(this);
        var _index = $this.index() + 1;
        console.log(!index)
        if (!index) {
            alert("请选择投入数量");
            return;
        }

        if (isrun) {
            return;
        }
        lastyazhurecord = [[], [], [], [], []];
        lastyazhurecord2 = [[], [], [], [], []];
        //var paynumber = [100, 500, 1000, 5000, 10000][index - 1]; 玩的数量 zetee
        var paynumber = m_playMoney[index - 1];
        // 选择两种押注方式
        var $wallet = $(".btn-temp .cur .hb-number"), type = 1;// 1为免费红包押注，2为金豆押注
        if (m_pay_act == 2) {
            type = 2;
        }
        if($('.hb-number').eq(0).text() == 0 && $('.hb-number').eq(1).text() == 0) {
            buycard(1)
            return
        }

        if ((Number($wallet.text()) - paynumber) < 0) {
            if (type == 1) {
                // alert("免费红包余额不足！",2)
                buycard(1)
            } else {
                // alert("赠送金豆余额不足！",2)
                buycard(1)
            }

            return
        }

        var yzdan=yazhu.dan+yazhu2.dan
        var yzshuang=yazhu.shuang+yazhu2.shuang
        var yzda=yazhu.da+yazhu2.da
        var yzxiao=yazhu.xiao+yazhu2.xiao
        var yzhong=yazhu.hong+yazhu2.hong
        if (_index == 3 && (yzhong + paynumber) > m_maxYahong ){
            alert("押注金豆不得超过" + m_maxYahong);
          return
        }
        if ((yzdan + yzshuang + yzda + yzxiao + yzhong + paynumber) > m_maxYaAll ){
            alert("总押注金豆不得超过" + m_maxYaAll);
          return
        }
        $wallet.text(Number($wallet.text()) - paynumber);

        //押注
        //yazhucount+=paynumber;
        var sizeMoneyC = $($(".size-temp").children().get(index - 1)).attr("data-money");
        var $conis = $('<div class="conis size' + sizeMoneyC + ' type' + type + '"></div>');
        $this.find(".yz-tip").before($conis)
        $conis.css("left", Math.random() * ($this.width() - $conis.width())).css("top", Math.random() * ($this.height() - $conis.height()))
        referconis();
        */
    })

    function buycard (type) {//1:卡券，2：小红包
        cardlistshow = true;
        if(type == 1) {
            $.ajax({
                url:'/WebApi/Game/KaQuanList',
                type:'get',
                success:function(res) {
                    youhuijuan = res.data;
                    rendercard(youhuijuan)
                    chosedcard = youhuijuan[0]
                    rendermoney()
                    $('.reduce').css('display','block');
                    $('.plus').css('display','block');
    
                }
            })
        } else {
            $.ajax({
                url:'/WebApi/Game/GetRedpacketCouponsList?payRedpacket=' + payRedpacket,
                type:'get',
                success:function(res) {
                    youhuijuan = res.data;
                    rendercard(youhuijuan)
                    console.log(youhuijuan)
                    chosedcard = youhuijuan[0]
                    rendermoney()
                    $('.reduce').css('display','none');
                    $('.plus').css('display','none');
    
                }
            })
            // 模拟数据
            // youhuijuan = [
            //     {
            //         money:10000,
            //         goodsname:'testhb',
            //         countmoney:1000000,
            //         goldbean:123123
            //     },
            //     {
            //         money:10000,
            //         goodsname:'testhb',
            //         countmoney:1000000,
            //         goldbean:123123
            //     },
            // ]
            // rendercard(youhuijuan)
            // chosedcard = youhuijuan[0]
            // rendermoney()

        }
        
        
        $(".buy-card").css({
            "display" : "block"
        })
    }

    $buycard.on('click', function() {
        buycard(1)
    })

    function closetool() {
        $('.downloadtool').css('display','none')
    }

    function toproduct() {
        window.location.href = '/product'
    }

    $(".closebtn").click(function() {// 购卡界面关闭
        $(".buy-card").css({
            "display" : "none"
        })
    })

    function rendercard (list) { //渲染优惠券列表
        console.log(list)
        var $ul = $('.card-list');
        var li_len = list.length;
        var money = 0, name = '', daijinquan = 0, jindou = 0,s_time ='',e_time='';
        
        var liarr = '';
        for(var i =0 ; i< li_len; i++) {
            money = list[i].money || 0;
            name = list[i].goodsname;
            daijinquan = list[i].countmoney;
            jindou = list[i].goldbean || 0;
            jinbi = list[i].goldcoin;
            redpacket = list [i].redpacket;
            s_time = list[i].starttime.split('T')[0];
            e_time = list[i].endtime.split('T')[0];
            if(e_time.match(/1900-01-01/)) {
                e_time = '永久有效'
            }
            
            if(daijinquan == 0 && redpacket == 0){
                var li = "<li>\
                <div class ='cl-left'>\
                    <div class='flexbox'>\
                        <i class ='moneyicon'></i><span>" + money + "</span>\
                    </div>\
                    <div class='cardtime'>使用时间：有效时间至"+ e_time+"</div>\
                </div>\
                <div class='cl-right'>\
                    <p style='margin-top:14px;'>\
                        " + name + "\
                    </p>\
                    <p class= 'cardjb'>" + jinbi +"金币</p>\
                    <p class='sendjindou'>\
                        送金豆" + jindou + "\
                    </p>\
                </div>\
            </li>";
            } else if(jinbi == 0 && redpacket == 0){
                console.log(134)
                var li = "<li>\
                <div class='flexbox'>\
                    <div class='cl-left'>\
                    <i class ='moneyicon'></i><span>" + money +"</span>\
                    </div>\
                    <div class='cl-right'>\
                        <p style='margin-top:14px;'>\
                            " + name + "\
                        </p>\
                        <p class='carddjq'>\
                            " + daijinquan + "元\
                        </p>\
                        <p class='sendjindou'>\
                            送金豆" + jindou + "\
                        </p>\
                    </div>\
                </div>\
                <div class='cardtime'>使用时间：有效时间至"+ e_time+"</div>'\
                </li>";

            } else if(jindou == 0) {
                console.log(123)
                var li = "<li>\
                    <div class='flexbox'>\
                        <div class='cl-left'>\
                        <i class ='moneyicon'></i><span>" + money + "</span>\
                        </div>\
                        <div class='cl-right'>\
                            <p style='margin-top:14px;'>\
                                " + name + "\
                            </p>\
                            <p class='carddjq'>\
                                一共送小红包"+ redpacket +"\
                            </p>\
                        </div>\
                    </div>\
                    <div class='cardtime'>使用时间：有效时间至"+ e_time+"</div>\
                </li>";
            }
            
            liarr += li
        }
        $ul.html(liarr)
        
        $(".card-list li:first").addClass('lion')
        cardli = $(".card-list li")
        cardli.click(chosecardli)

    }


    function rendermoney() { //渲染付款详情
        try{
            $(".trulypay").text('实付：￥' + (cardnum * chosedcard.money).toFixed(0))
            $(".getjindou").text((cardnum * chosedcard.goldbean).toFixed(0))
        } catch(e) {
            $(".trulypay").text('实付：￥0')
            $(".getjindou").text(0)
        }
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

    function getuser(call) { // 获取用户信息
        $.post('/WebApi/Game/GetUserInfo', { token: token }, function (ret) {
            if (!ret.errno) {
                if (ret.status == 98) {//登录失败
                    dlg_confirm("未能读取到您的登录信息，请重新登录?", function () {
                        window.location.href = "/#/login";
                    });
                    return;
                }
                console.log(ret)
                var data = ret.data;
                user = data;
                $(".head .left").show();
                if(data.real_name.length != 0) {
                    $(".head .left div").text(data.real_name);
                } else {
                    $(".head .left div").text(data.username);
                }
                $(".head .left img").attr("src", data.head_pic);
                $(".head .left img").error(function() {
                    $(this).onerror=null
                    $(this).attr("src", "/Assets/Game/img/默认图像.png")
                })
                m_pay_act_money["1"].money = data.h_money_free;
                m_pay_act_money["2"].money= data.h_money;
                m_pay_act_money["3"].money = data.small_redpackage_money||0;
                //$(".mfhb .hb-number").text(data.h_money_free);
                //$(".zshb .hb-number").text(data.h_money);
                $('div[data-act="1"]').find(".hb-number").text(m_pay_act_money["1"].money);
                $('div[data-act="2"]').find(".hb-number").text(m_pay_act_money["2"].money);
                $('div[data-act="3"]').find(".hb-number").text(m_pay_act_money["3"].money);
                $(".mask").hide();
                call && call();
            } else {
                //还没有登陆
            }

        }, 'json')
    }
    //显示押注总数
    function referconis($this) {
        $yztip = $this.find(".yz-tip");
        var yazhuList= $this.find(".conis");
        var count = 0;
        for (var i = 0; i < yazhuList.size(); i++) {
            count += Number($(yazhuList[i]).attr("money"));
        }
        if (count > 0) {
            $yztip.text(count).show();
            //$yztip.prepend('<i></i>');
        } else {
            $yztip.hide();
        }
        
    }

    function addyazhu($parent, $this, _count, i, num) {
        // console.log(yazhu);
        // console.log(yazhu2);
        if ($parent.is(".yazhudatemp")) {
            if ($this.is(".type1")) {
                yazhu.da += _count;
            } else {
                yazhu2.da += _count;
            }
        } else if ($parent.is(".yazhuxiaotemp")) {
            if ($this.is(".type1")) {
                yazhu.xiao += _count;
            } else {
                yazhu2.xiao += _count;
            }
        } else if ($parent.is(".yazhudantemp")) {
            if ($this.is(".type1")) {
                yazhu.dan += _count;
            } else {
                yazhu2.dan += _count;
            }
        } else if ($parent.is(".yazhushuangtemp")) {
            if ($this.is(".type1")) {
                yazhu.shuang += _count;
            } else {
                yazhu2.shuang += _count;
            }
        } else if ($parent.is(".yazhuhongtemp")) {
            if ($this.is(".type1")) {
                yazhu.hong += _count;
            } else {
                yazhu2.hong += _count;
            }
        }

        if ($this.is(".type1")) {
            yazhurecord[i].push(num)
        } else {
            yazhurecord2[i].push(num)
        }
    }

    function lottery() {
        if (isrun) {
            return
        }
        
      //  console.log(yazhu,yazhu2)
        $.post("/WebApi/Game/Lottery", { free: m_pay_act_money["1"].yazhu, zs: m_pay_act_money["2"].yazhu, little: m_pay_act_money["3"].yazhu, token: token }, function (ret) {
         //   console.log(ret)
           
            if (ret.errno) {
                alert(ret.errmsg);
                return
            }
            if (isrun) {
                return
            }
            jiangsu = false;
            isrun = true;
            speed = .5;
            isloop = true;
            addcount = ret.data.addcount;
             
            addfreecount = ret.data.addfreecount;
            // setTimeout(function(){
            //     speed=1.5;
            //   setTimeout(function(){
            //     speed=3;
            setTimeout(function () {
                speed = .5;
                var result = ret.data.result;
                if (result == 25) {
                    num = 1;
                } else if (result == 26) {
                    num = 14
                } else if (result >= 13) {
                    num = result + 2;
                } else {
                    num = result + 1;
                }
                overdeg = (num - 1) * sigle;
                maxoverdeg = (num - 0.5) * sigle;
                jiangsu = true;
                canover = true;
            }, 5000)

            //   },500)

            // },500)
        }, 'json')
    }
    var time = 10;
    function timeloop() {
        if (time <= 0) {
            lottery();
            $("#time").text(time + "秒")
            return
        }
        $("#time").text(time + "秒")
        setTimeout(function () {
            --time;
            timeloop();
        }, 1000)
    }


    function starttimeloop() {
        time = 10
        timeloop()
    }
    starttimeloop();
    //重复下单 zetee
    function yazhurepair() {
        //m_pay_act_money_pre
        //createYazhuElement($it, paymoney);
    }
    /**
     * 清空当前下注
     */
    function resetconis() {
        $yztemp.children().not(".yz-tip").remove();
        $yztemp.children(".yz-tip").text("");
        $yztemp.children(".yz-tip").hide();
        $.extend(true, m_pay_act_money_pre, m_pay_act_money);
        m_pay_act_money = {
            "1": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 },
            "2": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 },
            "3": { money: 0, yazhu: { da: 0, xiao: 0, dan: 0, shuang: 0, hong: 0 }, payTimes: 0 }
        };
    }
    getuser(function () {

        getlogslist();
        defaultchose()
    });
    function getlogslist() {
        $.post("/WebApi/Game/GetLogs", { token: token }, function (ret) {
            if (!ret.errno) {
                var html = '', num = ''
                for (var i = 0; i < 68; i++) {
                    if (ret.data[i]) {
                        num = ret.data[i].num > 24 ? "红" : ret.data[i].num
                    } else {
                        num = ''
                    }

                    if (i == 0 && ret.data[0]) {
                        html += '<div class="list new">' + num + '</div>'
                    } else if (ret.data[i]) {
                        html += '<div class="list">' + num + '</div>'
                    } else {
                        html += '<div class="list"></div>'
                    }

                }
                $(".history-temp .right").html(html)
            }
        }, 'json')
    }
    var addlogslist = [];
    function getaddlogslist() {
        $.get("/WebApi/Game/GetAddLogs", function (ret) {
            if (!ret.errno) {
                addlogslist = ret.data;
                if (addlogslist.length) {
                    hbtiploop();
                }
            }
        }, 'json');
    }
    getaddlogslist();
    var addlogsindex = 0;
    function hbtiploop() {
        if (addlogsindex >= addlogslist.length) {
            addlogsindex = 0;
        }
        var phonenum = addlogslist[addlogsindex].real_name;
        try{
            phonenum = phonenum.substr(0, 3) + '******' + phonenum.substr(9)
        } catch(e) {

        }

        if (addlogslist[addlogsindex].addcount > 0) {
            $(".float-tip-inner").text("恭喜用户" + phonenum + "中了" + addlogslist[addlogsindex].addcount + "金豆").animate({ top: "0" }, 1000).delay(3000).animate({ top: "-100%" }, 1000, function () {
                addlogsindex++;
                setTimeout(function () {
                    hbtiploop();
                }, 2000)

            });
        }
        if (addlogslist[addlogsindex].addfreecount > 0) {
            $(".float-tip-inner").text("恭喜用户" + phonenum + "中了" + addlogslist[addlogsindex].addfreecount + "金豆").animate({ top: "0" }, 1000).delay(3000).animate({ top: "-100%" }, 1000, function () {
                addlogsindex++;
                setTimeout(function () {
                    hbtiploop();
                }, 2000)

            });
        }

    }

    function loopPage() {
        return;//
        endtime = new Date().getTime();//用户退出时间
        var duration = Math.floor((endtime - starttime) / 1000)
        $.post("/WebApi/Game/SetState", 
            { starttime: starttime, endtime: endtime, duration: duration, rdcode: rdcode, token: token }, 
        function (ret) {
            /*if (ret.data == true) {
                $.post("/WebApi/Game/RefreshRandom", { token: token },
                function (data) {
                    if (data.additional_data !== 0) {
                        $('.maskxx').css('display', 'block')
                        $('.gethbnum').text(data.additional_data + ".00 元")
                        getuser()
                    } else if (data.add_data !== 0) {
                        alert(data.data)
                    } else {
                        alert('领取失败，请明天再试')
                    }
                },'json')
            };*/
            if (ret.additional_data == true) {
                $.post("/WebApi/Game/RefreshPer",
                    { "token": token },
                    function (res) {
                        console.log(res)
                        if (res.status == 0) {
                        	var elText="";
                        	if(res.data.FreeMoney.Money>0){
                        		elText+=res.data.FreeMoney.Msg+"</br>";
                        	}
                        	if(res.data.PayMoney.Money>0){
                        		elText+=res.data.PayMoney.Msg;
                        	}
                            $('.maskxx').css('display', 'block')
                            $('.gethbnum').html(elText)
                        } else {
                            alert(res.data)
                        }
                        
                    },'json')
            }
        }, 'json')
        setTimeout(function () {

            loopPage();
        }, 2000)
    }
    loopPage();

    $('.paybtn').click(function() {
        chosedcard.num = cardnum;
        chosedcard.token = token;
      //  console.log(chosedcard)
        $.ajax({
            url:'/WebApi/Game/BuyQuan',
            type:'post',
            data:{
                'id':chosedcard.id,
                'num': chosedcard.num,
                'token': chosedcard.token
            },
            success:function(data) {
              //  console.log(data)
              if(data.status == 0) {
                var res = data.data
                  //  console.log('https://e-shop.rrlsz.com.cn/CouponsPay?list=' + res +'&token=' + encodeURIComponent(token))
                window.location.href= '/CouponsPay?list=' + res +'&token=' + encodeURIComponent(token) + '&type=1,2,3&cash=' + (cardnum * chosedcard.money).toFixed(0);
              } else {
                  alert(data.message)
              }
            }
        })
    })

    $('.download-tips').click(function() {
        window.location.href='/Product'
    })

    $('#xhb').text(payRedpacket);
}
