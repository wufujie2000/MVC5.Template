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
}());

// GridMvc binding
(function () {
    if (window.GridMvc) {
        GridMvc.prototype.getClearFilterButton = function () {
            return '<button class="btn btn-info grid-filter-clear " type="button">' + this.lang.clearFilterLabel + '</button>';
        };
    }

    if (window.TextFilterWidget) {
        TextFilterWidget.prototype.renderWidget = function () {
            var html = '<div class="form-group">\
                        <select class="grid-filter-type form-control">\
                            <option value="1" ' + (this.value.filterType == "1" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.Equals + '</option>\
                            <option value="2" ' + (this.value.filterType == "2" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.Contains + '</option>\
                            <option value="3" ' + (this.value.filterType == "3" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.StartsWith + '</option>\
                            <option value="4" ' + (this.value.filterType == "4" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.EndsWith + '</option>\
                        </select>\
                    </div>\
                    <div class="form-group">\
                        <input type="text" class="grid-filter-input form-control" value="' + this.value.filterValue + '" />\
                    </div>\
                    <div class="grid-filter-buttons">\
                        <button type="button" class="btn btn-primary grid-apply" >' + this.lang.applyFilterButtonText + '</button>\
                    </div>';
            this.container.append(html);
        };
    }

    if (window.NumberFilterWidget) {
        NumberFilterWidget.prototype.renderWidget = function () {
            var html = '<div class="form-group">\
                        <select class="grid-filter-type form-control">\
                            <option value="1" ' + (this.value.filterType == "1" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.Equals + '</option>\
                            <option value="5" ' + (this.value.filterType == "5" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.GreaterThan + '</option>\
                            <option value="6" ' + (this.value.filterType == "6" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.LessThan + '</option>\
                        </select>\
                    </div>\
                    <div class="form-group">\
                        <input type="text" class="grid-filter-input form-control" value="' + this.value.filterValue + '" />\
                    </div>\
                    <div class="grid-filter-buttons">\
                        <button type="button" class="btn btn-primary grid-apply">' + this.lang.applyFilterButtonText + '</button>\
                    </div>';
            this.container.append(html);
        };
    }

    if (window.DateTimeFilterWidget) {
        DateTimeFilterWidget.prototype.renderWidget = function () {
            var html = '<div class="form-group">\
                        <select class="grid-filter-type form-control">\
                            <option value="1" ' + (this.value.filterType == "1" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.Equals + '</option>\
                            <option value="5" ' + (this.value.filterType == "5" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.GreaterThan + '</option>\
                            <option value="6" ' + (this.value.filterType == "6" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.LessThan + '</option>\
                        </select>\
                    </div>' +
                            (this.datePickerIncluded ?
                                '<div class="grid-filter-datepicker"></div>'
                                :
                                '<div class="form-group">\
                                <label>' + this.lang.filterValueLabel + '</label>\
                                <input type="text" class="grid-filter-input form-control" value="' + this.value.filterValue + '" />\
                             </div>\
                             <div class="grid-filter-buttons">\
                                <input type="button" class="btn btn-primary grid-apply" value="' + this.lang.applyFilterButtonText + '" />\
                             </div>');
            this.container.append(html);
            //if window.jQueryUi included:
            if (this.datePickerIncluded) {
                var datePickerOptions = this.data || {};
                datePickerOptions.dateFormat = datePickerOptions.dateFormat || "yy-mm-dd";
                datePickerOptions.language = datePickerOptions.language || this.lang.code;

                var $context = this;
                var dateContainer = this.container.find(".grid-filter-datepicker");
                dateContainer.datepicker(datePickerOptions).on('change', function (ev) {
                    var type = $context.container.find(".grid-filter-type").val();
                    var filterValues = [{ filterType: type, filterValue: ev.target.value }];
                    $context.cb(filterValues);
                });

                if (this.value.filterValue)
                    dateContainer.datepicker('setDate', this.value.filterValue);
            }
        };
    }

    if (window.BooleanFilterWidget) {
        BooleanFilterWidget.prototype.renderWidget = function () {
            var html = '<ul class="menu-list">\
                        <li><a class="grid-filter-choose ' + (this.value.filterValue == "true" ? "choose-selected" : "") + '" data-value="true" href="javascript:void(0);">' + this.lang.boolTrueLabel + '</a></li>\
                        <li><a class="grid-filter-choose ' + (this.value.filterValue == "false" ? "choose-selected" : "") + '" data-value="false" href="javascript:void(0);">' + this.lang.boolFalseLabel + '</a></li>\
                    </ul>';
            this.container.append(html);
        };
    }
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