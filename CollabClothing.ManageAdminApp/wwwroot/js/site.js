// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
    this.initialize = function () {
        acceptOrder();
    }

    //function acceptOrder() {
    //    $('window').on('load', '#btn-confirm', function (e) {
    //        e.preventDefault();
    //        //var check = confirm("Delete?");
    //        //if (check) {
    //        const id = $(this).data('id');

    //        var modalConfirm = function (callback) {

    //            $("#btn-confirm").on("click", function () {
    //                $("#mi-modal").modal('show');
    //            });

    //            $("#modal-btn-si").on("click", function () {
    //                callback(true);
    //                $("#mi-modal").modal('hide');
    //            });

    //            $("#modal-btn-no").on("click", function () {
    //                callback(false);
    //                $("#mi-modal").modal('hide');
    //            });
    //        };


    //        modalConfirm(function (confirm) {
    //            if (confirm) {
    //                $.ajax({
    //                    type: "DELETE",
    //                    url: '/Cart/AcceptOrder',
    //                    data: {
    //                        id: id,
    //                        status: true
    //                    },
    //                    success: function (data) {
    //                        alert(data);
    //                    },
    //                    error: function (err) {
    //                        console.log(err)
    //                    },
    //                })
    //            }
    //        });
    //        //}
    //    });
    //}
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
}