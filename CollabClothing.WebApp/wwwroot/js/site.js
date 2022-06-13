// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('body').on('click', '.container-product__item-btn', function (e) {
    e.preventDefault();
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Cart/AddToCart/',
        success: function (response) {
            console.log(response)
        },
        data: {
            id : id
        },
        dataType: 'json'
    });
})