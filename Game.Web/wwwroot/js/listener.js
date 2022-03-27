// listen for page resize
function resizeListener(dotnethelper) {
    $(window).resize(() => {
        let browserHeight = $(window).innerHeight();
        let browserWidth = $(window).innerWidth();
        dotnethelper.invokeMethodAsync('SetBrowserDimensions', browserWidth, browserHeight).then(() => {
            // success, do nothing
        }).catch(error => {
            console.log("Error during browser resize: " + error);
        });
    });
}

function getHeightWindow() {
    let browserHeight = $(window).innerHeight();
    return parseInt(browserHeight, 10);
}
function getWidthWindow() {
    let browserWidth = $(window).innerWidth();
    return parseInt(browserWidth, 10);
}



function getWidth() {
    var x = document.getElementById('box').getBoundingClientRect().width;
    return parseInt(x, 10);;
}