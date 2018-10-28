
/**
 * utils.js 只依赖jQuery
 * @param $
 * @returns {@G}
 */

/**
 *  ===============================================UrlUtils=====================================================
 * @param $
 * @returns {@G}
 */
 (function(){
	 UrlUtils={};
	/**
	 * html编码 
	 * 1) escape()该方法不会对 ASCII 字母和数字进行编码，也不会对下面这些 ASCII 标点符号进行编码： - _ . ! ~ * ' ( ) 。其他所有的字符都会被转义序列替换
	 * 2) encodeURI()            包含escape()  ;/?:@&=+$,# 不会转义
	 * 3) encodeURIComponent()   包含escape()  ;/?:@&=+$,#   会转义
	 */
	 UrlUtils.encode=function(str){return encodeURIComponent(str);};
	/**
	 * html 解码
	 */
	 UrlUtils.decode=function(str){return decodeURIComponent(str);};
})();
/**
 *  ===============================================HtmlUtils=====================================================
 * @param $
 * @returns {@G}
 */
 (function(){
	 HtmlUtils={};
	/**
	 * html编码
	 */
	HtmlUtils.encode=function(str){var s="";if(!str||str.length==0){return""}s=str.replace(/&/g,"&amp;");s=s.replace(/</g,"&lt;");s=s.replace(/>/g,"&gt;"); s=s.replace(/\'/g,"&#39;");s=s.replace(/\"/g,"&quot;");return s};
	/**
	 * html 解码
	 */
	HtmlUtils.decode=function(str){var s="";if(!str||str.length==0){return""}s=str.replace(/&amp;/g,"&");s=s.replace(/&lt;/g,"<");s=s.replace(/&gt;/g,">"); s=s.replace(/&#39;/g,"'");s=s.replace(/&quot;/g,'"');return s};
})();
 
/**
 * =============================================StringUtils=========================================
 * @param $
 * @returns {@G}
 */
 (function(){
	
	StringUtils={};
	/**
	 * 是否是空字符串
	 */
	StringUtils.isNullOrEmpty=function(b){var a=true;if(b==undefined||b==""||b=="null"){a=false}return a};
	/**
	 * 字符串格式c#样子的个格式化 eg：323｛0｝ 
	 */
	StringUtils.format=function(){if(arguments.length==0)return null;var str=arguments[0];for(var i=1;i<arguments.length;i++){var re=new RegExp('\\{'+(i-1)+'\\}','gm');str=str.replace(re,(arguments[i]))}return str};
	/**
	 *  字符串格式c#样子的个格式化 eg：323｛0｝ and HtmlEncode
	 */
	StringUtils.formatHtmlEncode=function(){if(arguments.length==0)return null;var str=arguments[0];for(var i=1;i<arguments.length;i++){var re=new RegExp('\\{'+(i-1)+'\\}','gm');str=str.replace(re,HtmlUtils.encode(arguments[i]))}return str};
	/**
	 * 转换成对象
	 */
	StringUtils.toObject=function(obj){return $.parseJSON(obj);};
	/**
	 * 是否是字符串
	 */
	StringUtils.isString=function(str){return Object.prototype.toString.call(str) === "[object String]";};
	/**
	 * 获取int哈希码
	 */
	StringUtils.hashCode=function(str) { var hash = 0; if (!str||str.length == 0) {  return hash;} for (i = 0; i < str.length; i++) { var char = str.charCodeAt(i);hash = ((hash<<5)-hash)+char; hash = hash & hash; }return hash; };
	
	/**
	 * 获取uuid 
	 */
	StringUtils.uuid=function(){var s=[];var hexDigits="0123456789abcdef";for(var i=0;i<36;i++){s[i]=hexDigits.substr(Math.floor(Math.random()*16),1)}s[14]="4";s[19]=hexDigits.substr((s[19]&3)|8,1);s[8]=s[13]=s[18]=s[23]="-";var uuid=s.join("");return uuid};
	StringUtils.guid=StringUtils.uuid;
	StringUtils.random=function (){return DateUtils.format(new Date(),"yyyyMMddhhmmss"); };
	/**
	 * 是否是图片
	 */
	StringUtils.isImage=function (t){ return (/\.ico$|\.jpg$|\.jpeg$|\.gif$|\.png$|\.bmp$|\.svg$/i).test(t);};
	
	StringUtils.replaceAll=function (s,a,b){ return s.replace(new RegExp(a,"gm"),b);};
})();

/**
 * ==============================================DateUtils===========================================
 * @param $
 */
 
(function (){
	window.DateUtils={};
	/**
	 * 创建
	 */
	DateUtils.create=function(y,M,d,h,m,s){M=M?M:0;d=d?d:0;h=h?h:0;m=m?m:0;s=s?s:0;return new Date(y,M-1,d,h,m,s)};
	/**
	 *  格式化日期yyy MM dd hh mm ss
	 */
	DateUtils.format=function(date,format){var o={"M+":date.getMonth()+1,"d+":date.getDate(),"h+":date.getHours(),"m+":date.getMinutes(),"s+":date.getSeconds(),"q+":Math.floor((date.getMonth()+3)/3),"S":date.getMilliseconds()};if(/(y+)/.test(format)){format=format.replace(RegExp.$1,(date.getFullYear()+"").substr(4-RegExp.$1.length))}for(var k in o){if(new RegExp("("+k+")").test(format)){format=format.replace(RegExp.$1,RegExp.$1.length==1?o[k]:("00"+o[k]).substr((""+o[k]).length))}}return format};
	/**
	 * 上一年
	 */
	DateUtils.lastYear=function(a){if(!a){a=new Date()}var b=new Date(a);b.setMonth(b.getMonth()-12);return b};
	
	/**
	 * 获取上个月1号 a=now,b=1
	 */
	DateUtils.lastMonth=function(a,b){if(!a){a=new Date()}if(!b){b=1}var a=this.create(a.getFullYear(),a.getMonth()+1,1);a.setMonth(a.getMonth()-b);return a};
	/**
	 * cnt=1获取上个季度,返回{year:,quarter:}
	 */
	DateUtils.lastQuarter=function(now,cnt){var lastyear=new Date(now);lastyear.setMonth(lastyear.getMonth()-3*cnt);var quarter=1;var year=lastyear.getUTCFullYear();var month=lastyear.getUTCMonth();if(month>=3&&month<6){quarter=2}else{if(month>=6&&month<9){quarter=3}else{if(month>=9&&month<12){quarter=4}}}return{year:year,quarter:quarter}};
	
	
})();

/**
 * ==============================================ObjectUtils=========================================
 * @param $
 */
 (function(){
	 ObjectUtils={};
	/**
	 * 获取jquery 对象
	 */
	 ObjectUtils.get=function(b){var a=b;if(StringUtils.isString(b)){if(b[0]=="#"){a=$(b)}else{a=$("#"+b)}}return a};
	/**
	 * 转换成json 对象
	 */
	 ObjectUtils.toJson = function(obj){ return JSON.parse(obj);};
	 
     /**
	 * 是否是属性Prop
	 */
	ObjectUtils.isProp=function(obj,attr){ return obj.hasOwnProperty(attr);};
	
	/**
	 * 属性变数组
	 */
	ObjectUtils.keyToArray=function(obj){ var v=[];for(var k in obj){ v.push(k); } return v; };
	/**
	 * 属性值变数组
	 */
	ObjectUtils.valueToArray=function(obj){ var v=[];for(var k in obj){ v.push(obj[k]); } return v; };
	 /**
	  * 转换成json 字符串
	 */
	 ObjectUtils.toString = function(obj){ return JSON.stringify(obj);};

 })();

/**
 * =============================================MathUtils=================================================
 * @param $
 * @returns {@G}
 */
(function(){
	MathUtils={};
	/**
	 * 最大
	 */
	MathUtils.max=function(a,b){if(a){if(b){return Math.max(a,b)}else{return a}}else{return b}};
	/**
	 * 最小
	 */
	MathUtils.min=function(d,c){if(d){if(c){return Math.min(d,c)}else{return d}}else{return c}};
	/**
	 * 去重复
	 */
	MathUtils.distinct=function(e){var b=[],d={};for(var a=0,c;(c=e[a])!=null;a++){if(!d[c]){b.push(c);d[c]=true}}return b};
	/**
	 * 对比
	 */
	MathUtils.compare=function(a){return function(e,d){var c=e[a];var b=d[a];if(b<c){return -1}else{if(b>c){return 1}else{return 0}}}};
})();


/**
 * ===============================================ArrayUtils=====================================================
 * @param $
 * @returns {@G}
 */
(function(){
	ArrayUtils={};
	ArrayUtils.init=function(len,val){var rv=[]; for(var i=0;i<len;i++){rv.push(val);} return rv;};
	ArrayUtils.max=function(c){if(c.length==0){return null}else{var b=c[0];for(var d=1;d<c.length;d++){if(b<c[d]){b=c[d]}}return b}};
	ArrayUtils.min=function(b){if(b.length==0){return null}else{var d=b[0];for(var c=1;c<b.length;c++){if(d>b[c]){d=b[c]}}return d}};
	ArrayUtils.toMap=function(e,c){var d={};if($.isArray(e)){for(var b=0;b<e.length;b++){var a=e[b];if(!d.hasOwnProperty(a[c])){d[a[c]]=[]}d[a[c]].push(a)}}return d};
	ArrayUtils.isArray=function(b){return $.isArray(b);};
	ArrayUtils.contains=function(a,b){ for (i in a) { if (a[i] == b) return true; } return false;};
})();

/**
 *  ===============================================MapUtils=====================================================
 * @param $
 * @returns {@G}
 */
(function($){
	 
})();

UI = {};
/**
 * 打印日记
 */
UI.log = function (msg) {
    if (window.console && console.log) {
        window.console.log(msg);
    }
};
(function () {
UI.Page = {};
/**
* 获取所有的参数
*/
UI.Page.getAllparam = function () {
    var param = {};
    var url = document.location.toString();
    var arrObj = url.split("?")[1];
    if (!arrObj)
        return param;
    var search = arrObj;//location.search.slice(1); // 得到get方式提交的查询字符串
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        var value = "";
        if (decodeURIComponent(ar[1]) == 'undefined') {
            value = "";
        } else {
            value = decodeURIComponent(ar[1]);
        }
        param[ar[0]] = value;
    }
    return param;
    }}) ();
/**
 * =============UI.Ajax========
 */
(function () {
    UI.Ajax = {};
    /**
     * post2完成后返回结果
     */
    UI.Ajax.post = function (url, postData, completeCallBack, options) {
        var defaultOptions = {
            type: 'post',
            dataType: "json",
            url: url,
            data: postData,
            postData: postData,
            cache: false,
            async: true,
            complete: function (XMLHttpRequest, textStatus) {
                var isSuccess = false;
                var message = "操作失败!";
                var data = null;
                if (XMLHttpRequest.readyState == 4
                    && XMLHttpRequest.status == 200) {
                    isSuccess = true;
                    message = "操作成功!";
                    if (this.dataType == "json") {
                        data = XMLHttpRequest.responseJSON;
                        if (data.status != 0) {
                            // alert(data.message);
                            //return;
                        }
                    } else {
                        data = XMLHttpRequest.responseText;
                    }
                } else {
                    alert("服务器异常！");
                }
                if ($.isFunction(completeCallBack))
                    completeCallBack(data, this.postData, isSuccess,
                        message);

            }

        };
        var finalOptions = $.extend(defaultOptions, options);
        $.ajax(finalOptions);
    };

})();


/**
 * ============================================$Utils=======================================
 * @param $
 */
/*(function($){
	 
	
	  //form表单序列号成对象
	  $.fn.form2object=function(){var array=$(this).serializeArray();var obj={};for(var i=0;i<array.length;i++){if(obj[array[i].name]){}else{obj[array[i].name]=array[i].value}}return obj};
	  //form表单序列号成json字符串
	  $.fn.form2json=function(){var obj=$(this).form2object();return ObjectUtils.toJson(obj)};
	})(jQuery);*//*=======================================jQuery扩展方法================================*/

 

 