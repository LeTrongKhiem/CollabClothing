// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
    this.initialize = function () {
        acceptOrder();
        selectSizeInQuantityRemain();
    }

    function acceptOrder() {
        $('body').on('click', '#btn-confirm-accept', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            var check = confirm("Bạn xác nhận đơn hàng này");
            if (check) {
                $.ajax({
                    type: "POST",
                    url: '/Cart/AcceptOrder',
                    data: {
                        id: id,
                        status: true
                    },
                    success: function (response) {
                        location.reload();
                    },
                    error: function (err) {
                        console.log(err)
                    },
                })
            }
           
        });
    }

    function selectSizeInQuantityRemain() {
        $(document).ready(function () {
            $("#select-size").change(function () {
                var select = $('#select-size option:selected').val();
                var selectSizeId = $(this).find(':selected').data('id');
                console.log(select);
                //$('#selected-sizeid').html(select);
            })
        });
    }
}