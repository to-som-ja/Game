//let el = document.getElementById("box");
function getWdith() {
    DotNet.invokeMethodAsync("Game.Web", "GetValue").then(result => {
        alert('Message from method: ' + result);
    });
}
function getWidth() {
    var x = document.getElementById('box').getBoundingClientRect().width;

    return parseInt(x, 10);;
}