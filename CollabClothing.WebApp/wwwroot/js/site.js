// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
    this.initialize = function () {
        CartAddSite();
        loadCartData();
    }
    function loadCartData() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetCart',
            success: function (response) {
                const count = response.reduce((accumulator, object) => {
                    return accumulator + object.quantity
                }, 0);
                $('#count-cart-quantity').text(count);
                console.log(count)
            },
        });
    }
    function CartAddSite() {
        $('body').on('click', '.container-product__item-btn', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            $.ajax({
                type: "POST",
                url: '/Cart/AddToCart/',
                success: function (response) {
                    const count = response.reduce((accumulator, object) => {
                        return accumulator + object.quantity
                    }, 0);
                    $('#count-cart-quantity').text(count);
                    console.log(count)
                },
                data: {
                    id: id
                },
            });
        })
    }
}
