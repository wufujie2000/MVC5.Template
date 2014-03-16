// Header
(function () {
    $(document).on('mouseleave', '.dropdown', function () {
        $(this).removeClass('open');
    });
    $(document).on('mouseleave', '.dropdown-menu', function () {
        $(this).parent().removeClass('open');
    });
}());

// Alerts closing & fading
(function () {
    $('div.alert').each(function (index, element) {
        var alertDiv = $(element);
        if (alertDiv.hasClass('alert-danger')) {
            var alertFor = alertDiv.data('alert-for');

            var alertForSpan = $('span[data-valmsg-for="' + alertFor + '"]');
            if (alertForSpan.length > 0)
                alertDiv.remove();
            else
                alertDiv.fadeTo(300, 1);
        } else {
            alertDiv.fadeTo(300, 1);
        }
    });

    $('div.alert').each(function (index, element) {
        var alertDiv = $(element);
        if (alertDiv.data('fade-out-after') != null && alertDiv.data('fade-out-after') != 0) {
            setTimeout(function () {
                alertDiv.fadeTo(300, 0).slideUp(300, function () {
                    $(this).remove();
                });
            }, alertDiv.data('fade-out-after') * 1000);
        }
    });

    $(document).on('click', '.alert > a.close', function () {
        $(this).parent().fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });
    });
}());

// Sidebar navigation resize
(function () {
    /*$(window).on('resize', function () {
        wwidth = $(window).width();
        if (wwidth >= 768 && wwidth <= 991) {
            $('#SidebarNavigation > ul > li.open ul').attr('style', '').parent().removeClass('open');
            $('#SidebarNavigation > ul').css({ 'display': 'block' });
        }

        if (wwidth <= 767){
            $('#Sidebar').niceScroll();
            $('#Sidebar').getNiceScroll().resize();

            if ($(window).scrollTop() > 35) {
                $('body').addClass('fixed');
            }
            $(window).scroll(function () {
                if ($(window).scrollTop() > 35) {
                    $('body').addClass('fixed');
                } else {
                    $('body').removeClass('fixed');
                }
            });
        }

        if (wwidth > 767) {
            $('#SidebarNavigation > ul').css({ 'display': 'block' });

            $('body').removeClass('menu-open');
            $('#Sidebar').attr('style', '');
            $('#TopNavigation > ul').css({ width: 'auto', margin: '0' });
        }

    });

    if ($(window).width() <= 767) {
        if ($(window).scrollTop() > 35) {
            $('body').addClass('fixed');
        }
        $(window).scroll(function () {
            if ($(window).scrollTop() > 35) {
                $('body').addClass('fixed');
            } else {
                $('body').removeClass('fixed');
            }
        });

        $('#Sidebar').niceScroll({
            zindex: '9999'
        });
        $('#Sidebar').getNiceScroll().resize();
    }

    if ($(window).width() > 767) {
        $('#SidebarNavigation > ul').css({ 'display': 'block' });
    }
    if ($(window).width() > 767 && $(window).width() < 992) {
        $('#SidebarNavigation > ul > li.open ul').css({ 'display': 'none' });
    }

    $(document).on('click', '#SidebarTrigger', function () {
        if ($(window).width() <= 767) {
            if ($('body').hasClass('menu-open')) {
                $('body').removeClass('menu-open');
            } else {
                $('body').addClass('menu-open');
            }
        }
        return false;
    });

    $(document).on('click', '.submenu > a', function (e) {
        e.preventDefault();
        var submenu = $(this).siblings('ul');
        var li = $(this).parents('li');

        var submenus = $('#Sidebar li.submenu ul');
        var submenus_parents = $('#Sidebar li.submenu');

        if (li.hasClass('open')) {
            if (($(window).width() > 976) || ($(window).width() < 768)) {
                submenu.slideUp();
            } else {
                submenu.fadeOut(150);
            }
            li.removeClass('open');
        } else {
            if (($(window).width() > 976) || ($(window).width() < 768)) {
                submenus.slideUp();
                submenu.slideDown();
            } else {
                submenus.fadeOut(150);
                submenu.fadeIn(150);
            }
            submenus_parents.removeClass('open');
            li.addClass('open');
        }
        $('#Sidebar').getNiceScroll().resize();
    });

    $(document).on('mouseleave', '#Sidebar li.submenu ul', function () {
        if ($(window).width() >= 768 && $(window).width() < 977) {
            $(this).fadeOut(150).parent().removeClass('open');
        }
    });*/
}());

// Globalization
(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || Globalize.parseDate(value);
    };
    $.validator.methods.number = function (value, element) {
        var pattern = new RegExp('^(?=.*\\d+.*)[-+]?\\d*[' + Globalize.culture().numberFormat['.'] + ']?\\d*$');
        return this.optional(element) || pattern.test(value);
    };
}());

(function () {
    $(".datepicker").datepicker();
}());

// JsTree binding
(function () {
    $('.tree-view').each(function () {
        var treeView = $(this).jstree({
            'plugins': [
                'checkbox'
            ],
            'checkbox': {
                'keep_selected_style': false
            }
        }).jstree();

        $(this).prev('.tree-view-ids').children().each(function () {
            treeView.select_node($(this).val(), false, true);
        });

        treeView.element.show();
    });

    $(document).on('submit', 'form', function (e) {
        $(this).find('.tree-view').each(function () {
            var treeView = $(this).jstree();
            var treeIdSpan = $(this).prev('.tree-view-ids');

            treeIdSpan.empty();
            $.each(treeView.get_selected(), function () {
                var node = treeView.get_node(this);
                if (node.li_attr.id) {
                    treeIdSpan.append('<input type="hidden" value="' + node.li_attr.id + '" name="' + treeView.element.attr('for') + '" />');
                }
            });
        });
    });
}());

// Datalist
(function () {
    $('.datalist-input').datalist();
}());
