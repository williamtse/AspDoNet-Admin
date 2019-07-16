// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $('.tree_branch_delete').unbind('click').click(function () {
        var id = $(this).data('id');
        swal({
            title: "确认删除?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "确认",
            showLoaderOnConfirm: true,
            cancelButtonText: "取消",
            preConfirm: function () {
                return new Promise(function (resolve) {
                    $.ajax({
                        method: 'post',
                        url: '/admin/articles/' + id,
                        data: {
                            _method: 'delete',
                            _token: LA.token,
                        },
                        success: function (data) {
                            $.pjax.reload('#pjax-container');

                            resolve(data);
                        }
                    });
                });
            }
        }).then(function (result) {
            var data = result.value;
            if (typeof data === 'object') {
                if (data.status) {
                    swal(data.message, '', 'success');
                } else {
                    swal(data.message, '', 'error');
                }
            }
        });
    });
});