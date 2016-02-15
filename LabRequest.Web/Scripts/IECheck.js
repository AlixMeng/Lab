function closeWindows() {
    var browserName = navigator.appName;
    var browserVer = parseInt(navigator.appVersion);
    if (browserName == "Microsoft Internet Explorer") {
        var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;
        if (ie7) {
            window.open('', '_parent', '');
            window.close();
        }
        else {
            this.focus();
            self.opener = this;
            self.close();
        }
    }
}

function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0)
        var ieVersion = parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)));
    if (ieVersion <= 8) {
        alert('با مرورگر جاری امکان اجرای سامانه وجو ندارد');
        closeWindows();
    } else
        return false;
}

msieversion();