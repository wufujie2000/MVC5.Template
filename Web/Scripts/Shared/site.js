// Header dropdown closure
(function () {
    $(document).on('mouseleave', '.header-navigation .dropdown', function () {
        $(this).removeClass('open');
    });
}());

// Alerts closing & fading
(function () {
    $('div.alert').each(function (index, element) {
        var alertDiv = $(element);

        if (alertDiv.data('fadeout-after') != null && alertDiv.data('fadeout-after') != 0) {
            setTimeout(function () {
                alertDiv.fadeTo(300, 0).slideUp(300, function () {
                    $(this).remove();
                });
            }, alertDiv.data('fadeout-after'));
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

    $(".datetimepicker").each(function () {
        $(this).datetimepicker();
    });
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

    $(document).on('submit', 'form', function () {
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