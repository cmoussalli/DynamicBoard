function newSrc(url) {
    var chartFrame = document.getElementById("ChartFrame");
    if (chartFrame !== null) {
        var hostUrl = window.location.protocol + '//' + window.location.host;;

        chartFrame.src = hostUrl+url;
    } else {
        console.error("Url not found");
    }
}

