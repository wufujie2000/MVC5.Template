/*!
 * Datalist 3.0.1
 * https://github.com/NonFactors/MVC.Datalist
 *
 * Copyright © 2014 NonFactors
 *
 * Licensed under the terms of the MIT License
 * http://www.opensource.org/licenses/mit-license.php
 */
(function ($) {
    $.widget('mvc.datalist', {
        _create: function () {
            if (!this.element.hasClass('datalist-input')) return;

            this._initOptions();
            this._initFilters();
            this._initAutocomplete();
            this._initDatalistOpenSpan();

            this._loadSelected();
            this._cleanUp();
        },
        _initOptions: function () {
            var e = this.element;
            var o = this.options;

            o.hiddenElement = $('#' + e.attr('data-datalist-hidden-input'))[0];
            o.recordsPerPage = e.attr('data-datalist-records-per-page');
            o.filters = e.attr('data-datalist-filters').split(',');
            o.sortColumn = e.attr('data-datalist-sort-column');
            o.sortOrder = e.attr('data-datalist-sort-order');
            o.title = e.attr('data-datalist-dialog-title');
            o.term = e.attr('data-datalist-term');
            o.page = parseInt(e.attr('data-datalist-page'));
            o.url = e.attr('data-datalist-url');
            e.addClass('mvc-datalist');
        },
        _initFilters: function () {
            for (i = 0; i < this.options.filters.length; i++)
                this._initFilter($('#' + this.options.filters[i]));
        },
        _initFilter: function (filter) {
            var that = this;
            that._on(filter, {
                change: function () {
                    var event = $.Event(that._select);
                    if (that.options.filterChange)
                        that.options.filterChange(event, that.element[0], that.options.hiddenElement, filter[0]);
                    if (!event.isDefaultPrevented())
                        that._select(null);
                }
            });
        },
        _initAutocomplete: function () {
            var that = this;
            this.element.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: that._formAutocompleteUrl(request.term),
                        success: function (data) {
                            response($.map(data.Rows, function (item) {
                                return {
                                    label: item.DatalistAcKey,
                                    value: item.DatalistAcKey,
                                    item: item
                                }
                            }));
                        }
                    });
                },
                select: function (e, selection) {
                    that._select(selection.item.item);
                },
                minLength: 1
            });

            this.element.bind('keyup.datalist', function () {
                if (this.value.length == 0)
                    that._select(null);
            });
            this.element.prevAll('.ui-helper-hidden-accessible').remove();
        },
        _initDatalistOpenSpan: function () {
            var datalistAddon = this.element.nextAll('.datalist-open-span:first');
            if (datalistAddon.length != 0) {
                var datalist = $('#Datalist');
                var that = this;

                this._on(datalistAddon, {
                    click: function () {
                        datalist
                            .find('.datalist-search-input')
                            .unbind('keyup.datalist')
                            .bindWithDelay('keyup.datalist', function () {
                                that.options.term = this.value;
                                that.options.page = 0;
                                that._update(datalist);
                            }, 500, false)
                            .val(that.options.term);
                        datalist
                            .find('.datalist-items-per-page')
                            .spinner({
                                change: function () {
                                    this.value = that._limitTo(this.value, 1, 99);
                                    that.options.recordsPerPage = this.value;
                                    that.options.page = 0;
                                    that._update(datalist);
                                }
                            })
                            .val(that._limitTo(that.options.recordsPerPage, 1, 99));

                        datalist.find('.datalist-search-input').attr('placeholder', $.fn.datalist.lang.Search);
                        datalist.find('.datalist-error-span').html($.fn.datalist.lang.Error);
                        datalist.dialog('option', 'title', that.options.title);
                        datalist.find('.datalist-table-head').empty();
                        datalist.find('.datalist-table-body').empty();
                        datalist.dialog('open');
                        that._update(datalist);
                    }
                });
            }
        },

        _formAutocompleteUrl: function (term) {
            return this.options.url +
                '?SearchTerm=' + term +
                '&RecordsPerPage=20' +
                '&SortOrder=Asc' +
                '&Page=0' +
                this._formFiltersQuery();
        },
        _formDatalistUrl: function (term) {
            return this.options.url +
                '?SearchTerm=' + term +
                '&RecordsPerPage=' + this.options.recordsPerPage +
                '&SortColumn=' + this.options.sortColumn +
                '&SortOrder=' + this.options.sortOrder +
                '&Page=' + this.options.page +
                this._formFiltersQuery();
        },
        _formFiltersQuery: function () {
            var additionaFilter = '';
            for (i = 0; i < this.options.filters.length; i++) {
                var filter = $('#' + this.options.filters[i]);
                if (filter.length == 1)
                    additionaFilter += '&' + this.options.filters[i] + '=' + filter.val();
            }

            return additionaFilter;
        },

        _defaultSelect: function (data) {
            if (data) {
                $(this.options.hiddenElement).val(data.DatalistIdKey).change();
                $(this.element).val(data.DatalistAcKey).change();
            }
            else {
                $(this.element).val(null).change();
                $(this.options.hiddenElement).val(null).change();
            }
        },
        _loadSelected: function () {
            var that = this;
            var id = $(that.options.hiddenElement).val();
            if (id) {
                $.ajax({
                    url: that.options.url + '?Id=' + id + '&RecordsPerPage=1',
                    cache: false,
                    success: function (data) {
                        if (data.Rows.length > 0)
                            that._select(data.Rows[0]);
                    }
                });
            }
        },
        _select: function (data) {
            var event = $.Event(this._defaultSelect);
            if (this.options.select)
                this.options.select(event, this.element[0], this.options.hiddenElement, data);
            if (!event.isDefaultPrevented())
                this._defaultSelect(data);
        },

        _limitTo: function (value, min, max) {
            value = parseInt(value);
            if (isNaN(value))
                return 20;
            if (value < 1)
                return 1;
            if (value > 99)
                return 99;

            return value;
        },
        _cleanUp: function () {
            this.element.removeAttr('data-datalist-records-per-page')
            this.element.removeAttr('data-datalist-dialog-title');
            this.element.removeAttr('data-datalist-hidden-input');
            this.element.removeAttr('data-datalist-sort-column');
            this.element.removeAttr('data-datalist-sort-order');
            this.element.removeAttr('data-datalist-filters');
            this.element.removeAttr('data-datalist-term');
            this.element.removeAttr('data-datalist-page');
            this.element.removeAttr('data-datalist-url');
        },

        _update: function (datalist) {
            var that = this;
            var term = datalist.find('.datalist-search-input').val();

            datalist.find('.datalist-error').fadeOut(300);
            var timeOut = setTimeout(function () {
                datalist.find('.datalist-processing').fadeIn(300);
                datalist.find('.datalist-data').fadeOut(300);
            }, 500);

            $.ajax({
                url: that._formDatalistUrl(term),
                cache: false,
                success: function (data) {
                    that._updateHeader(datalist, data.Columns);
                    that._updateData(datalist, data);
                    that._updateNavbar(datalist, data.FilteredRecords);

                    clearTimeout(timeOut);
                    datalist.find('.datalist-processing').fadeOut(300);
                    datalist.find('.datalist-pager').fadeIn(300);
                    datalist.find('.datalist-data').fadeIn(300);
                },
                error: function () {
                    clearTimeout(timeOut);
                    datalist.find('.datalist-processing').fadeOut(300);
                    datalist.find('.datalist-pager').fadeOut(300);
                    datalist.find('.datalist-data').fadeOut(300);
                    datalist.find('.datalist-error').fadeIn(300);
                }
            });
        },
        _updateHeader: function (datalist, columns) {
            var that = this;
            var header = '';
            var columnCount = 0;
            $.each(columns, function (index, column) {
                header += '<th class="' + (column.CssClass != null ? column.CssClass : '') + '" data-column="' + column.Key + '">' + (column.Header != null ? column.Header : '');
                if (that.options.sortColumn == column.Key || (that.options.sortColumn == '' && columnCount == 0)) {
                    header += '<span class="datalist-sort-arrow glyphicon glyphicon-arrow-' + (that.options.sortOrder == 'Asc' ? 'down' : 'up') + '"></span>';
                    that.options.sortColumn = column.Key;
                }

                header += '</th>';
                columnCount++;
            });

            datalist.find('.datalist-table-head').html('<tr>' + header + '<th class="datalist-select-header"></th></tr>');
            datalist.find('.datalist-table-head th').click(function () {
                var header = $(this);
                if (!header.attr('data-column')) return false;
                if (that.options.sortColumn == header.attr('data-column'))
                    that.options.sortOrder = that.options.sortOrder == 'Asc' ? 'Desc' : 'Asc';
                else
                    that.options.sortOrder = 'Asc';

                that.options.sortColumn = header.attr('data-column');
                that._update(datalist);
            });
        },
        _updateData: function (datalist, data) {
            if (data.Rows.length == 0) {
                datalist.find('.datalist-table-body').html('<tr><td colspan="0" style="text-align: center">' + $.fn.datalist.lang.NoDataFound + '</td></tr>');
                return;
            }

            var tableData = '';
            for (var i = 0; i < data.Rows.length; i++) {
                var tableRow = '<tr>'
                var row = data.Rows[i];
                $.each(data.Columns, function (index, column) {
                    tableRow += '<td class="' + (column.CssClass != null ? column.CssClass : '') + '">' + (row[column.Key] != null ? row[column.Key] : '') + '</td>';
                });

                tableRow += '<td class="datalist-select-cell"><div class="datalist-select-container"><i class="glyphicon glyphicon-ok"></i></div></td></tr>';
                tableData += tableRow;
            }

            datalist.find('.datalist-table-body').html(tableData);
            var selectCells = datalist.find('td.datalist-select-cell');
            for (var i = 0; i < selectCells.length; i++)
                this._bindSelect(datalist, selectCells[i], data.Rows[i]);
        },
        _updateNavbar: function (datalist, filteredRecords) {
            var that = this;
            var pageLength = datalist.find('.datalist-items-per-page').val();
            var totalPages = parseInt(filteredRecords / pageLength) + 1;
            if (filteredRecords % pageLength == 0)
                totalPages--;

            if (totalPages == 0)
                datalist.find('.datalist-pager > .pagination').empty();
            else
                datalist.find('.datalist-pager > .pagination').bootstrapPaginator({
                    currentPage: that.options.page + 1,
                    bootstrapMajorVersion: 3,
                    totalPages: totalPages,
                    onPageChanged: function (e, oldPage, newPage) {
                        that.options.page = newPage - 1;
                        that._update(datalist);
                    },
                    tooltipTitles: function (type, page, current) {
                        return '';
                    },
                    itemTexts: function (type, page, current) {
                        switch (type) {
                            case 'first':
                                return '&laquo;';
                            case 'prev':
                                return '&lsaquo;';
                            case 'next':
                                return '&rsaquo;';
                            case 'last':
                                return '&raquo;';
                            case 'page':
                                return page;
                        }
                    }
                });
        },
        _bindSelect: function (datalist, selectCell, data) {
            var that = this;
            that._on(selectCell, {
                click: function () {
                    datalist.dialog('close');
                    that._select(data);
                }
            });
        },

        _destroy: function () {
            var e = this.element;
            var o = this.options;

            e.attr('data-datalist-records-per-page', o.recordsPerPage);
            e.attr('data-datalist-hidden-input', o.hiddenElement.id);
            e.attr('data-datalist-filters', o.filters.join());
            e.attr('data-datalist-sort-column', o.sortColumn);
            e.attr('data-datalist-sort-order', o.sortOrder);
            e.attr('data-datalist-dialog-title', o.title);
            e.attr('data-datalist-term', o.term);
            e.attr('data-datalist-page', o.page);
            e.attr('data-datalist-url', o.url);
            e.removeClass('mvc-datalist');
            e.autocomplete('destroy');

            return this._super();
        }
    });
})(jQuery);

(function ($) {
    $.fn.datalist.lang = {
        Error: 'Error while retrieving records',
        NoDataFound: 'No data found',
        Search: 'Search...'
    };

    var datalist = $('#Datalist');
    datalist.find('.datalist-items-per-page').spinner({ min: 1, max: 99 }).parent().addClass('input-group-addon');
    datalist.dialog({
        autoOpen: false,
        minHeight: 210,
        height: 'auto',
        minWidth: 455,
        width: 'auto',
        modal: true
    });
}(jQuery));