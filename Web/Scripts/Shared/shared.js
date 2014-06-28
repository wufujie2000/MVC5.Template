// Dropdown closure
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

        if (alertDiv.data('fade-out-after') != null && alertDiv.data('fade-out-after') != 0) {
            setTimeout(function () {
                alertDiv.fadeTo(300, 0).slideUp(300, function () {
                    $(this).remove();
                });
            }, alertDiv.data('fade-out-after') * 1000);
        }
    });

    $(document).on('click', '.alert a.close', function () {
        $(this).parent().parent().fadeTo(300, 0).slideUp(300, function () {
            $(this).hide();
        });
    });
}());

// Globalization binding
(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || Globalize.parseDate(value);
    };

    $.validator.methods.number = function (value, element) {
        var pattern = new RegExp('^(?=.*\\d+.*)[-+]?\\d*[' + Globalize.culture().numberFormat['.'] + ']?\\d*$');
        return this.optional(element) || pattern.test(value);
    };
}());

// Datepicker binding
(function () {
    $(".datepicker").each(function () {
        $(this).datepicker();
    });
}());

// JsTree binding
(function () {
    $('.js-tree-view').each(function () {
        var treeView = $(this).jstree({
            'plugins': [
                'checkbox'
            ],
            'checkbox': {
                'keep_selected_style': false
            }
        }).jstree();

        $(this).prev('.js-tree-view-ids').children().each(function () {
            treeView.select_node($(this).val(), false, true);
        });

        treeView.element.show();
    });

    $(document).on('submit', 'form', function (e) {
        $(this).find('.js-tree-view').each(function () {
            var treeView = $(this).jstree();
            var treeIdSpan = $(this).prev('.js-tree-view-ids');

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

// Datalist binding
(function () {
    $('.datalist-input').each(function () {
        $(this).datalist();
    });
}());
