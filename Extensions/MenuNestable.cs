using BootstrapHtmlHelper.UI;
using BootstrapHtmlHelper.Util.Tree;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Extensions
{
    sealed class MenuNestable:Nestable
    {
        private Dictionary<int, Menu> _dic;
        public MenuNestable(Dictionary<int, Menu> dic, List<Node> nodes, string id)
        {
            _dic = dic;
            Builder builder = new Builder(nodes);
            _nodes = builder.getTree();
            _id = id;
        }
        public override string GetScript()
        {
            string script = @"$('.tree_branch_delete').click(function() {
                                var id = $(this).data('id');
                                swal({
                                    title: '确认删除 ? ',
                                    type: 'warning',
                                    showCancelButton: true,
                                    confirmButtonColor: '#DD6B55',
                                    confirmButtonText: '确认',
                                    showLoaderOnConfirm: true,
                                    cancelButtonText: '取消',
                                    preConfirm: function() {
                                    return new Promise(function(resolve) {
                                            $.ajax({
                                                method: 'delete',
                                                url: '/Menus/Delete/' + id,
                                                data: {
                                                    _method:'delete',
                                                    __RequestVerificationToken:LA,
                                                },
                                                success: function (data) {
                                                    $.pjax.reload('#pjax-container');
                                                    toastr.success('删除成功 !');
                                                    resolve(data);
                                                }
                                            });
                                        });
                                    }
                                }).then(function(result) {
                                    console.log(result);
                                    var data = result.value;
                                    if (typeof data === 'object')
                                    {
                                        if (data.code===0)
                                        {
                                            swal(data.message, '', 'success');
                                        }
                                        else
                                        {
                                            swal(data.message, '', 'error');
                                        }
                                    }
                                });
                            });";
            return script;
        }

        protected override string _handle(Node node)
        {
            Menu menu = _dic[node.ID];
            return @"<i class='fa " + menu.Icon + @"'></i><strong>" + menu.Title + @"</strong>
             <a href='" + menu.Uri + "' class='dd-nodrag'>" + menu.Uri + @"</a>
             <span class='pull-right dd-nodrag'>
             <a href = '/Menus/Edit/" + menu.ID + @"' ><i class='fa fa-edit'></i></a>
             <a href = 'javascript:void(0);' data-id='" + menu.ID + @"' class='tree_branch_delete'><i class='fa fa-trash'></i></a>
             </span>";
        }
    }
}
