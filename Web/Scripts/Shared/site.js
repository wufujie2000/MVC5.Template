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

// GridMvc binding
(function () {
    if (window.GridMvc) {
        GridMvc.prototype.getClearFilterButton = function () {
            return '<button class="btn btn-info grid-filter-clear" type="button">' + this.lang.clearFilterLabel + '</button>';
        };

        GridMvc.prototype.openFilterPopup = function (self, html) {
            var columnType = $(this).attr("data-type") || "";
            var widget = self.getFilterWidgetForType(columnType);
            if (widget == null)
                return false;

            var columnName = $(this).attr("data-name") || "";
            var filterData = $(this).attr("data-filterdata") || "";
            var widgetData = $(this).attr("data-widgetdata") || "{}";
            var filterDataObj = self.parseFilterValues(filterData) || {};
            var filterUrl = $(this).attr("data-url") || "";

            $(".grid-dropdown").remove();
            $("body").append(html);

            var widgetContainer = $("body").children(".grid-dropdown").find(".grid-popup-widget");
            if (typeof (widget.onRender) != 'undefined')
                widget.onRender(widgetContainer, self.lang, columnType, filterDataObj, function (values) {
                    self.closeOpenedPopups();
                    self.applyFilterValues(filterUrl, columnName, values, false);
                }, $.parseJSON(widgetData));

            if ($(this).find(".grid-filter-btn").hasClass("filtered") && widget.showClearFilterButton()) {
                var inner = $("body").find(".grid-popup-additional");
                inner.append(self.getClearFilterButton(filterUrl));
                inner.find(".grid-filter-clear").click(function () {
                    self.applyFilterValues(filterUrl, columnName, "", true);
                });
            }
            var openResult = self.openMenuOnClick.call(this, self);
            if (typeof (widget.onShow) != 'undefined')
                widget.onShow();

            self.setupPopupInitialPosition($(this));
            return openResult;
        };

        GridMvc.prototype.setupPopupInitialPosition = function (popup) {
            var dropdown = $(".grid-dropdown");
            var arrow = dropdown.find(".grid-dropdown-arrow");

            var dropdownWidth = dropdown.width();
            var popupLeft = popup.offset().left;
            var popupTop = popup.offset().top;
            var winWidth = $(window).width();
            var dropdownTop = popupTop + 20;
            var dropdownLeft = 0;
            var arrowLeft = 0;

            if (popupLeft + dropdownWidth + 10 > winWidth) {
                dropdownLeft = winWidth - dropdownWidth - 10;
                arrowLeft = popupLeft - dropdownLeft - 3;
            } else {
                dropdownLeft = popupLeft - 20;
                arrowLeft = 17;
            }

            dropdown.attr("style", "display: block; left: " + dropdownLeft + "px; top: " + dropdownTop + "px !important");
            arrow.css("left", arrowLeft + "px");
        };

        GridMvc.prototype.openMenuOnClick = function (self) {
            if ($(this).hasClass("clicked")) return true;
            self.closeOpenedPopups();
            $(this).addClass("clicked");
            var popup = $("body").find(".grid-dropdown");
            if (popup.length == 0) return true;
            popup.show();
            popup.addClass("opened");
            self.openedMenuBtn = $(this);
            $(document).bind("click.gridmvc", function (e) {
                self.documentCallback(e, self);
            });
            return false;
        };
    }

    $(window).resize(function () {
        $(".grid-dropdown").hide().removeClass("opened");
    });
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

// NiceScroll binding
(function () {
    $('body').niceScroll({
        cursoropacitymin: 0.3,
        cursorborderradius: 0,
        cursorborder: "none",
        zindex: 1000
    });

    $('.grid-wrap').niceScroll({
        cursorborderradius: 0,
        cursorborder: "none"
    });
}());

// Bootstrap binding
(function () {
    $('[rel=tooltip]').tooltip();
}());
