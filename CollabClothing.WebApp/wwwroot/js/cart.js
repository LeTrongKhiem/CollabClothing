var CartJS = function () {
    this.initialize = function () {
        loadCart()
    }
    function loadCart() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetCart',
            success: function (response) {
                console.log(response)
            },
        });
    }
}