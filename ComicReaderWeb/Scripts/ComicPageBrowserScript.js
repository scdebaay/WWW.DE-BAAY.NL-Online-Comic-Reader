function NextPage() {
    document.getElementById("ComicLeaf").src = RewriteComicSource("Next");
}

function PrevPage() {
    document.getElementById("ComicLeaf").src = RewriteComicSource("Previous");
}

function RewriteComicSource(request) {
    //update Comic Image source
    var baseurl = "https://www.de-baay.nl/ComicCloud/";
    var queryObjects = {
        file: getQueryVariable('file'),
        page: parseInt(getQueryVariable('page'), 10),
        size: parseInt(getQueryVariable('size'), 10),
    };
    if (request == "Next") {        
        queryObjects.page = queryObjects.page + 1;        
    }
    else {
        queryObjects.page = queryObjects.page - 1;
    }
    var queryString = Object.keys(queryObjects).map(key => key + '=' + queryObjects[key]).join('&');
    var URL = baseurl + "?" + queryString;
    //update URL
    var documentTitle = document.title;
    var pathArray = window.location.pathname.split('/');
    pathArray.shift();
    pathArray[pathArray.length - 1] = queryObjects.page;
    var newPathname = "";
    for (i = 0; i < pathArray.length; i++) {
        newPathname += "/";
        newPathname += pathArray[i];
    }
    window.history.pushState(null, documentTitle, newPathname);
    return URL;
}

function getQueryVariable(variable) {
    var query = document.getElementById("ComicLeaf").src.split("?")[1];
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    console.log('Query variable %s not found', variable);
}