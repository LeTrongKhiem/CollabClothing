var CartJS = function () {
    this.initialize = function () {
        loadCart()
    }
    function loadCart() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetCart',
            success: function (response) {
                var html = '';
                var total = 0;
                var count = 0;
                $.each(response, function (e, item) {
                    html += "<div class=\"row border - top border - bottom\">"
                        + "<div class=\"row main-cart align-items-center\" >"
                        + "<div class=\"col-2\"><img class=\"img-fluid\" src=\"" + $('#hiBaseAddress').val() + item.image + "\"></div>"
                        + "<div class=\"col\">"
                        + "<div class=\"row-in text-muted\">"+item.name+"</div>"
                        + "<div class=\"row-in\">Cotton T-shirt</div>"
                        + " <div class=\"row-in\">Size: S</div>"
                        + " <div class=\"row-in\">Brand: "+item.brandName+"</div>"
                        + "</div>"
                        + "<div class=\"col\" style=\"margin : auto\">"
                        + "<a href=\"#\">-</a><a href=\"#\" class=\"border\">" + item.quantity + "</a><a href=\"#\">+</a>"
                        + "</div>"
                        + " <div class=\"col\" style=\"margin : auto\"> " + item.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + " <a class=\"close\">&#10005;</a></div>"
                        + "</div>"
                        + "</div>"
                    total += item.price * item.quantity
                    count += item.quantity
                    console.log(count);
                });
                $('#cart-body').html(html);
                $('#total-cart').text(total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));
                $('.count-cart').text(count);
            },
        });
    }
}