function getPageLayout() {
    var t = $(window).width();
    return t >= 1200 ? "lg" : t >= 992 && 1199 >= t ? "md" : t >= 768 && 991 >= t ? "sm" : 767 >= t ? "xs" : void 0
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

var toCurrency = function (amount) {
    if (!amount) {
        return "";
    }
    amount += '';
    x = amount.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

var toPersian = function (numberValue) {
    persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
    numberValue += '';
    for (var i = 0; i <= 9; i++) {
        var re = new RegExp(i, "g");
        numberValue = numberValue.replace(re, persian[i]);
    }
    return numberValue;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function ajaxUpdateOn() {
    if ($(".ajax-update")[0]) {
        $(".ajax-update").css("opacity", 0.5);
        $(".ajax-update").css("pointer-events", 'none');
    }
 //else {
 //       $("body").css("opacity", 0.5);
 //       $("body").css("pointer-events", 'none');
 //   }

}
function ajaxUpdateOff() { 
    if ($(".ajax-update")[0]) {
        $(".ajax-update").css("opacity", 1);
        $(".ajax-update").css("pointer-events", 'visible');
    }
    //else
    //{
    //    $("body").css("opacity",1);
    //    $("body").css("pointer-events", 'visible');
    //}
}
function discountCounter() {

    var elements = $(".discount_counter");
    elements.each(function (index) {
        var currentE = $(this);
        var endDate = currentE.attr("end_date");
        var countDownDate = new Date(endDate).getTime();

        // Update the count down every 1 second
        var x = setInterval(function () {
            // Get today's date and time
            var now = new Date().getTime();

            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            currentE.find(".discount_days").html(days);

            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            currentE.find(".discount_hours").html(hours);
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            currentE.find(".discount_minutes").html(minutes);
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);
            currentE.find(".discount_seconds").html(seconds);
            //    console.log(days + " " + hours + " " + minutes + " " + seconds)
        }, 1000);
    });

}
discountCounter();