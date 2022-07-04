var CartJS = function () {
    this.initialize = function () {
        loadCart()
        EventListener()
    }
    function EventListenerBase(id, quantity) {
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
                loadCart();//load lai cart update price
                $('#count-cart-quantity').text(count);
            },
            error: function (err) {
                console.log(err)
            },
        })
    }
    function EventListener() {
        $('body').on('click', '.btn_asc', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            EventListenerBase(id, quantity);
        });
        $('body').on('click', '.btn_desc', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            EventListenerBase(id, quantity);
        });
        $('body').on('click', '.btn_remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            EventListenerBase(id, 0);
        });
    }
    function loadCart() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetCart',
            success: function (response) {
                if (response.length === 0) {
                    $('#cart-content').hide()
                    $('#cart-empty').css('display', 'block')
                }
                var html = '';
                var total = 0;
                var count = 0;
                $.each(response, function (e, item) {
                    html += "<div class=\"row border - top border - bottom\">"
                        + "<div class=\"row main-cart align-items-center\" >"
                        + "<div class=\"col-2\"><img class=\"img-fluid\" src=\"" + $('#hiBaseAddress').val() + item.image + "\"></div>"
                        + "<div class=\"col\">"
                        + "<div class=\"row-in text-muted\">" + item.name + "</div>"
                        + "<div class=\"row-in\">" + item.type + "</div>"
                        + " <div class=\"row-in\">Size: " + item.sizeName +"</div>"
                        + " <div class=\"row-in\">Brand: " + item.brandName + "</div>"
                        + "</div>"
                        + "<div class=\"col\" style=\"margin : auto\">"
                        + "<a href=\"#\" class=\"a-link btn_desc\" data-id=\"" + item.productId + "\">-</a><input id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\" class=\"border-quantity-cart\"/><a href=\"#\" class=\"a-link btn_asc\" data-id=\"" + item.productId + "\">+</a>"
                        + "</div>"
                        + " <div class=\"col\" style=\"margin : auto\"> " + item.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + " <a  class=\"close btn_remove\" data-id=\"" + item.productId + "\">&#10005;</a></div>"
                        + "</div>"
                        + "</div>"
                    total += item.price * item.quantity
                    count += item.quantity
                    console.log(count);
                });
                $('#cart-body').html(html);
                $('.total-cart').text(total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
                $('.count-cart').text(count);
            },
        });
    }
}