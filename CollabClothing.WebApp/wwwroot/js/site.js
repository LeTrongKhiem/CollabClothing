// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
    this.initialize = function () {
        loadCartData();
        CartAddSite();
        removeProductCart();
        selectSize();
        //ButtonLoadMore();
    }
    function removeProductCart() {
        $('body').on('click', '.header__cart-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = 0;
            $.ajax({
                type: "POST",
                url: '/Cart/UpdateCart',
                data: {
                    id: id,
                    quantity: quantity
                },
                success: function (response) {
                    const count = response.reduce((item, object) => {
                        return item + object.quanitty;
                    }, 0);
                    loadCartData();//load lai cart update price
                    $('#count-cart-quantity').text(count);
                },
                error: function (err) {
                    console.log(err)
                },
            })
        });
    }
    function loadCartData() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetCart',
            success: function (response) {
                if (response.length === 0) {
                    $('#no-product-in-cart').css('display', 'block')
                }
                const count = response.reduce((accumulator, object) => {
                    return accumulator + object.quantity
                }, 0);
                $('#count-cart-quantity').text(count);
                console.log(count);
                console.log(response);
                var html = '';
                $.each(response, (e, item) => {
                    const selectSize = $('#select-size').val();

                    html += "<a style=\"text-decoration:none;\" href=\"/Product/Detail/" + item.productId + "\">"
                        + "<li class=\"header__cart-item\">"
                        + "<img src=\"" + $('#hiBaseAddress').val() + item.image + "\" class=\"header__cart-img\">"
                        + "<div class=\"header__cart-item-info\">"
                        + "<div class=\"header__cart-item-head\">"
                        + "<h5 class=\"header__cart-item-name\">" + item.name + "</h5>"
                        + "<div class=\"header__cart-item-price-wrap\">"
                        + " <span class=\"header__cart-price\">" + item.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + "</span>"
                        + "<span class=\"header__cart-mutiply\">x</span>"
                        + "<span class=\"header__cart-qnt\">" + item.quantity + "</span>"
                        + "</div>"
                        + "</div>"
                        + "<div class=\"header__cart-item-body\">"
                        + "<span class=\"header__cart-item-desciption\">Phân loại: Size " + $('#select-size').val() + "</span>"
                        + "<a class=\"header__cart-remove\" data-id=\"" + item.productId + "\">Xóa</a>"
                        + "</div>"
                        + "</div>"
                        + "</li>"
                        + "</a>"
                })
                $('#list_product_cart_item').html(html);
            },
        });
    }
    function selectSize() {
        $(document).ready(function () {
            $("#select-size").change(function () {
                    var selected = $('#select-size option:selected').val();
                $.ajax({
                    type: 'POST',
                    data: {
                        //select : $('#select-size option:selected').val()
                    }
                });
                //alert(selected);
                $('#size-selected').html(selected);

            });
        });
    }

    function CartAddSite() {
        $('body').on('click', '.container-product__item-btn', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const selectSize = $('#select-size').val();
            $.ajax({
                type: "POST",
                url: '/Cart/AddToCart/',
                success: function (response) {
                    const count = response.reduce((accumulator, object) => {
                        return accumulator + object.quantity
                    }, 0);
                    $('#count-cart-quantity').text(count);
                    console.log(count);
                    loadCartData();
                    console.log(selectSize);
                },
                data: {
                    id: id
                },
            });
        })
    }
}
