// Header dropdown closure
(function () {
    $(document).on('mouseleave', '.header-navigation .dropdown', function () {
        $(this).removeClass('open');
    });
}());

// Alerts fading & closing
(function () {
    $('.alerts div.alert').each(function () {
        var alert = $(this);

        if (alert.data('fadeout-after') != null && alert.data('fadeout-after') != 0) {
            setTimeout(function () {
                alert.fadeTo(300, 0).slideUp(300, function () {
                    $(this).remove();
                });
            }, alert.data('fadeout-after'));
        }
    });

    $(document).on('click', '.alert a.close', function () {
        $(this.parentNode.parentNode).fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
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

    $.validator.methods.min = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) >= parseFloat(param);
    };

    $.validator.methods.max = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) <= parseFloat(param);
    };

    $.validator.methods.range = function (value, element, param) {
        return this.optional(element) || (Globalize.parseFloat(value) >= parseFloat(param[0]) && Globalize.parseFloat(value) <= parseFloat(param[1]));
    };
}());

// Datepicker binding
(function () {
    var datePickers = $(".datepicker");
    for (var i = 0; i < datePickers.length; i++) {
        $(datePickers[i]).datepicker();
    }

    var datetimePickers = $(".datetimepicker");
    for (i = 0; i < datetimePickers.length; i++) {
        $(datetimePickers[i]).datetimepicker();
    }
}());

// JsTree binding
(function () {
    var jsTrees = $('.js-tree-view');
    for (var i = 0; i < jsTrees.length; i++) {
        var jsTree = $(jsTrees[i]).jstree({
            'plugins': [
                'checkbox'
            ],
            'checkbox': {
                'keep_selected_style': false
            }
        }).jstree();

        var selectedNodes = jsTree.element.prev('.js-tree-view-ids').children();
        for (var j = 0; j < selectedNodes.length; j++) {
            jsTree.select_node(selectedNodes[j].value, false, true);
        }

        jsTree.element.show();
    }

    if (jsTrees.length > 0) {
        $(document).on('submit', 'form', function () {
            var jsTrees = $(this).find('.js-tree-view');
            for (var i = 0; i < jsTrees.length; i++) {
                var jsTree = $(jsTrees[i]).jstree();
                var treeIdSpan = jsTree.element.prev('.js-tree-view-ids');

                treeIdSpan.empty();
                var selectedNodes = jsTree.get_selected();
                for (var j = 0; j < selectedNodes.length; j++) {
                    var node = jsTree.get_node(selectedNodes[j]);
                    if (node.li_attr.id) {
                        treeIdSpan.append('<input type="hidden" value="' + node.li_attr.id + '" name="' + jsTree.element.attr('for') + '" />');
                    }
                }
            }
        });
    }
}());

// Mvc.Grid binding
(function () {
    var mvcGrids = $('.mvc-grid');
    for (var i = 0; i < mvcGrids.length; i++) {
        $(mvcGrids[i]).mvcgrid();
    }
}());

// NiceScroll binding
(function () {
    $('body').niceScroll({
        cursoropacitymin: 0.3,
        cursorborderradius: 0,
        cursorborder: "none",
        zindex: 1000
    });

    $('.mvc-grid').niceScroll({
        cursorborderradius: 0,
        cursorborder: "none"
    });
}());

// Bootstrap binding
(function () {
    $('[rel=tooltip]').tooltip();
}());
