<<<<<<< HEAD
$(function () {
    getWaitCheck(0);
});


//获取等待审核的菜谱
function getWaitCheck(status) {
    $.ajax({
        type: 'get',
        url: '/UserCenter/GetWaitCheckCookBook',
        dataType: 'json',
        success: function (data) {
            if(data.StatusCode==1)
            {
                initList(data.Data);
            }else
                return layer.alert(data.Message, { icon: 3, skin: 'layer-ext-moon' });
        }
    });
}

function initList(json) {
    console.log(json);
    var str = '';
    $.each(json, function () {
        str += '<li onclick="getDetail(&quot;' + this.CookBookId + '&quot;) "><div class="pic"><a href="#" ><img class="imgLoad" src="' + this.ImgUrl + '" alt="' + this.Name + '" width="120" height="90" /></a></div><div class="detail"><h4><a href="#">' + this.Name + '</a></h4><div class="substatus clear"><div class="right"><a href="#">编辑</a></div></div></div></li>';
    })
    $('.ui_list_1').html(str);
}

function getDetail(cookbookId) {
    window.location.href = "/UserCenter/ShowDetail?cookBookId=" + cookbookId;
}
=======
$(function () {
    getWaitCheck(0);
});


//获取等待审核的菜谱
function getWaitCheck(status) {
    $.ajax({
        type: 'get',
        url: '/UserCenter/GetWaitCheckCookBook',
        dataType: 'json',
        success: function (data) {
            if(data.StatusCode==1)
            {
                initList(data.Data);
            }else
                return layer.alert(data.Message, { icon: 3, skin: 'layer-ext-moon' });
        }
    });
}

function initList(json) {
    console.log(json);
    var str = '';
    $.each(json, function () {
        str += '<li onclick="getDetail(&quot;' + this.CookBookId + '&quot;) "><div class="pic"><a href="#" ><img class="imgLoad" src="' + this.ImgUrl + '" alt="' + this.Name + '" width="120" height="90" /></a></div><div class="detail"><h4><a href="#">' + this.Name + '</a></h4><div class="substatus clear"><div class="right"><a href="#">编辑</a></div></div></div></li>';
    })
    $('.ui_list_1').html(str);
}

function getDetail(cookbookId) {
    window.location.href = "/UserCenter/ShowDetail?cookBookId=" + cookbookId;
}
//=======
//﻿$(function () {
//    getWaitCheck(0);
//});


////获取等待审核的菜谱
//function getWaitCheck(status) {
//    $.ajax({
//        type: 'get',
//        url: '/UserCenter/GetWaitCheckCookBook',
//        dataType: 'json',
//        success: function (data) {
//            if(data.StatusCode==1)
//            {
//                initList(data.Data);
//            }else
//                return layer.alert(data.Message, { icon: 3, skin: 'layer-ext-moon' });
//        }
//    });
//}

//function initList(json) {
//    console.log(json);
//    var str = '';
//    $.each(json, function () {
//        str += '<li><div class="pic"><a href="#" ><img class="imgLoad" src="' + this.ImgUrl + '" alt="' + this.Name + '" width="120" height="90" /></a></div><div class="detail"><h4><a href="#">' + this.Name + '</a></h4><div class="substatus clear"><div class="right"><a href="#">编辑</a></div></div></div></li>';
//    })
//    $('.ui_list_1').html(str);
//>>>>>>> b203b63077616f3d5067d0a19d132b6f64d0d53c
//}
>>>>>>> 8d477415f2152a810a107191a5908a08577b3cab
