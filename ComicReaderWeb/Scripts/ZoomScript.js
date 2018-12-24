function ZoomOut(obj) {
    var integer = parseInt(obj, 10);
    var ZoomOut = integer-100;
    var url = window.location.href;
    if (url.indexOf("?") > 0) {
        url = url.substring(0, url.indexOf("?"));
    }
    var url = url + "?size=" + ZoomOut;
    window.location.replace(url);
}
function ZoomIn(obj) {
    var integer = parseInt(obj, 10);
    var ZoomIn = integer+100;
    var url = window.location.href;
    if (url.indexOf("?") > 0) {
        url = url.substring(0, url.indexOf("?"));
    }
    var url = url + "?size=" + ZoomIn;
    window.location.replace(url);
}