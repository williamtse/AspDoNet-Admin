﻿$(function () {

    NProgress.configure({ parent: '#app' });

    $.pjax.defaults.timeout = 5000;
    $.pjax.defaults.maxCacheLength = 0;

    $(document).pjax('a:not(a[target="_blank"])', {
        container: '#pjax-container'
    });

    $(document).on('pjax:timeout', function (event) {
        event.preventDefault();
    })

    $(document).on('submit', 'form[pjax-container]', function (event) {
        $.pjax.submit(event, '#pjax-container')
    });

    $(document).on("pjax:popstate", function () {
        $(document).one("pjax:end", function (event) {
            $(event.target).find("script[data-exec-on-popstate]").each(function () {
                $.globalEval(this.text || this.textContent || this.innerHTML || '');
            });
        });
    });

    $(document).on('pjax:send', function (xhr) {
        if (xhr.relatedTarget && xhr.relatedTarget.tagName && xhr.relatedTarget.tagName.toLowerCase() === 'form') {
            $submit_btn = $('form[pjax-container] :submit');
            if ($submit_btn) {
                $submit_btn.button('loading')
            }
        }
        NProgress.start();
    });

    $(document).on('pjax:complete', function (xhr) {
        if (xhr.relatedTarget && xhr.relatedTarget.tagName && xhr.relatedTarget.tagName.toLowerCase() === 'form') {
            $submit_btn = $('form[pjax-container] :submit');
            if ($submit_btn) {
                $submit_btn.button('reset')
            }
        }
        NProgress.done();
    });


});

$(function () {
    $('.sidebar-menu li:not(.has-treeview) > a').on('click', function () {
        $('.sidebar-menu .active').removeClass('active');
        $(this).addClass('active');
    });
    var menu = $('.sidebar-menu li > a.active').parent().addClass('active');
    menu.parents('ul.nav-treeview').show();
    menu.parents('li.has-treeview').addClass('menu-open');

    $('[data-toggle="popover"]').popover();

    // Sidebar form autocomplete
    $('.sidebar-form .autocomplete').on('keyup focus', function () {
        var $menu = $('.sidebar-form .dropdown-menu');
        var text = $(this).val();

        if (text === '') {
            $menu.hide();
            return;
        }

        var regex = new RegExp(text, 'i');
        var matched = false;

        $menu.find('li').each(function () {
            if (!regex.test($(this).find('a').text())) {
                $(this).hide();
            } else {
                $(this).show();
                matched = true;
            }
        });

        if (matched) {
            $menu.show();
        }
    }).click(function (event) {
        event.stopPropagation();
    });

    $('.sidebar-form .dropdown-menu li a').click(function () {
        $('.sidebar-form .autocomplete').val($(this).text());
    });
});
