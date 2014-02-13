/*
 * Datalist v3.0.0 beta
 */
(function ($) {
    $.fn.datalist = function () {
        if (this.length == 0) return false;
        for (var i = 0; i < this.length; i++) {
            var datalistInput = $(this[i]);
            var datalists = $('.datalist[data-identifier="' + datalistInput.attr('data-identifier') + '"]');
            removeDuplicates(datalists);

            var datalist = $(datalists[0]);
            createAutocomplete(datalistInput, datalist);
            bindOpenSpan(datalistInput, datalist);
            bindEvents(datalistInput);

            addTextSpans(datalist);
            createDialog(datalist);
            bindInputs(datalist);

            loadSelected(datalistInput, datalist);
            bindFilters(datalistInput, datalist);
        }
    };
    enableSorting();
    setLanguage();

    function removeDuplicates(datalists) {
        $.each(datalists, function (index, element) {
            if (index > 0)
                $(element).remove();
        });
    }

    function createAutocomplete(datalistInput, datalist) {
        datalistInput.autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: datalist.attr('data-datalist-url') + '?SearchTerm=' + request.term + getAutoCompleteFilters(datalist),
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data.Rows, function (item) {
                            return {
                                label: item['DatalistAcKey'],
                                value: item['DatalistAcKey'],
                                item: item
                            }
                        }));
                    }
                });
            },
            minLength: 1,
            select: function (event, selectedItem) {
                select($('#' + datalistInput.data('hidden-input')), datalistInput,
                    selectedItem.item.item['DatalistIdKey'], selectedItem.item.item['DatalistAcKey']);
            }
        });
        datalistInput.prevAll('.ui-helper-hidden-accessible').remove();
    }
    function getAutoCompleteFilters(datalist) {
        return '&RecordsPerPage=20&SortOrder=ASC&Page=0' + getAdditionalFilterQuery(datalist);
    }
    function getAdditionalFilterQuery(datalist) {
        var filters = datalist.attr('data-filters').split(',');
        filters = $.grep(filters, function (item) { return (item != null && item != ''); });
        var filtersCount = filters.length;

        var additionaFilter = '';
        for (index = 0; index < filtersCount; index++) {
            var value = $('#' + filters[index]).val();
            additionaFilter += '&' + filters[index] + '=' + (value != null ? value : '');
        }

        return additionaFilter;
    }
    function bindOpenSpan(datalistInput, datalist) {
        var openSpan = datalistInput.nextAll('.datalist-open-span:first');
        openSpan.click(function () {
            datalist.attr('data-id-input', datalistInput.data('hidden-input'));
            datalist.attr('data-value-input', datalistInput.attr('id'));
            datalist.dialog('open').parent().css('top', '130px');
            // TODO: Fix 130px is not valid mesurement for scrolled down page
            update(datalist);
        });
    }
    function bindEvents(datalistInput) {
        datalistInput.keyup(function () {
            if ($(this).val().length == 0)
                $('#' + $(this).data('hidden-input')).val('').change();
        });
    }

    function update(datalist) {
        var term = datalist.find('.datalist-search-input').val();
        var timeOut = setTimeout(function () {
            datalist.find('.datalist-processing').fadeIn(300);
        }, 500);
        datalist.find('.datalist-error').fadeOut(300);
        datalist.find('.datalist-data').fadeIn(300);

        $.ajax({
            'url': datalist.attr('data-datalist-url') + formQuery(datalist, term == null ? '' : term),
            'async': true,
            'cache': false,
            'dataType': 'json',
            'success': function (data) {
                updateTable(datalist, data);
                updateNavigationBar(datalist, data);

                clearTimeout(timeOut);
                datalist.find('.datalist-processing').fadeOut(300);
                datalist.find('.datalist-pager').fadeIn(300);
            },
            'error': function () {
                clearTimeout(timeOut);
                datalist.find('.datalist-pager').fadeOut(300);
                datalist.find('.datalist-processing').fadeOut(300);
                datalist.find('.datalist-data').fadeOut(300);
                datalist.find('.datalist-error').fadeIn(300);
            }
        });
    }
    function formQuery(datalist, term) {
        var datalistLength = datalist.find('.datalist-items-per-page').val();
        var datalistSortCol = datalist.attr('data-sort-column');
        var datalistSortOrder = datalist.attr('data-sort-order');
        var datalistPage = datalist.attr('data-page');

        return '?SearchTerm=' + term + '&RecordsPerPage=' + datalistLength + '&SortColumn=' + datalistSortCol + '&SortOrder=' + datalistSortOrder + '&Page=' + datalistPage + getAdditionalFilterQuery(datalist);
    }
    function updateTable(datalist, data) {
        var tableHeaderRow = '<tr>';
        var columnCount = 1;
        var tableData = '';

        for (var key in data.Columns) {
            var sortArrow = '';
            var sortOrder = datalist.attr('data-sort-order');
            if (datalist.attr('data-sort-column') == key || (datalist.attr('data-sort-column') == '' && columnCount == 1))
                sortArrow += '<span class="datalist-sort-arrow glyphicon glyphicon-arrow-' + (sortOrder == 'ASC' ? 'down' : 'up') + '"></span>';

            tableHeaderRow += '<th data-column="' + key + '">' + data.Columns[key] + sortArrow + '</th>';

            columnCount++;
        }

        if (data.Rows.length > 0) {
            for (var index in data.Rows) {
                var tableRow = '<tr>'
                var row = data.Rows[index];
                for (var key in data.Columns)
                    tableRow += '<td>' + row[key] + '</th>';

                tableRow +=
                    '<td class="datalist-select-cell" data-id="' + row['DatalistIdKey'] + '" data-value="' + row['DatalistAcKey'] + '">' +
                    '<div class="datalist-select-container"><i class="glyphicon glyphicon-ok"></div></i></th>';
                tableRow += '</tr>';
                tableData += tableRow;
            }

            tableHeaderRow += '<th class="datalist-select-cell"></th>';
        } else {
            tableData += '<tr><td colspan="' + (columnCount - 1) + '" style="text-align: center">' + $.fn.datalist.lang.NoDataFound + '</tr>';
        }

        tableHeaderRow += "</tr>";

        datalist.find('.datalist-table-head').html(tableHeaderRow);
        datalist.find('.datalist-table-body').html(tableData);
        datalist.find('td.datalist-select-cell').click(function () {
            select($('#' + datalist.attr('data-id-input')), $('#' + datalist.attr('data-value-input')), $(this).data('id'), $(this).data('value'));
            datalist.find('.datalist-pager').hide();
            datalist.dialog('close');
        });
    }
    function updateNavigationBar(datalist, data) {
        var pageLength = datalist.find('.datalist-items-per-page').val();
        var currentPage = parseInt(datalist.attr('data-page')) + 1;
        var totalPages = parseInt(data.FilteredRecords / pageLength) + 1;
        if (data.FilteredRecords % pageLength == 0 || data.FilteredRecords <= pageLength)
            totalPages--;

        if (totalPages == 0)
            datalist.find('.datalist-pager > .pagination').empty();
        else
            datalist.find('.datalist-pager > .pagination').bootstrapPaginator({
                bootstrapMajorVersion: 3,
                currentPage: currentPage,
                totalPages: totalPages,
                onPageChanged: function (e, oldPage, newPage) {
                    datalist.attr('data-page', newPage - 1);
                    update(datalist);
                },
                tooltipTitles: function (type, page, current) {
                    return "";
                },
                itemTexts: function (type, page, current) {
                    switch (type) {
                        case "first":
                            return "&laquo;";
                        case "prev":
                            return "&lsaquo;";
                        case "next":
                            return "&rsaquo;";
                        case "last":
                            return "&raquo;";
                        case "page":
                            return page;
                    }
                }
            });
    }
    function select(input, datalistInput, id, value) {
        input.attr('value', id != null ? id : '');
        input.val(id != null ? id : '').change();
        datalistInput.attr('value', value != null ? value : '');
        datalistInput.val(value != null ? value : '').change();
    }

    function addTextSpans(datalist) {
        datalist.find('.datalist-search-input').attr('placeholder', $.fn.datalist.lang.Search);
        datalist.find('.datalist-error-span').html($.fn.datalist.lang.Error);
    }
    function createDialog(datalist) {
        datalist.dialog({
            title: datalist.attr('data-dialog-title'),
            resizable: true,
            autoOpen: false,
            minHeight: 210,
            height: 'auto',
            minWidth: 455,
            width: 'auto',
            modal: true
        });
    }
    function bindInputs(datalist) {
        datalist.find('.datalist-search-input').bindWithDelay('keyup', function () {
            datalist.attr('data-page', 0);
            update(datalist);
        }, 500, false);

        datalist.find('.datalist-items-per-page').spinner({
            change: function () {
                var newValue = parseInt($(this).val());
                if (isNaN(newValue))
                    newValue = 20;
                if (newValue < 1)
                    newValue = 1;
                if (newValue > 99)
                    newValue = 99;

                $(this).val(newValue);
                datalist.attr('data-page', 0);
                update(datalist);
            },
            max: 99,
            min: 1
        }).val(20).parent().addClass('input-group-addon');
    }

    function loadSelected(datalistInput, datalist) {;
        var hiddenInput = $('#' + datalistInput.attr('data-hidden-input'));
        var id = hiddenInput.val();
        if (id != '' && id != 0) {
            $.ajax({
                'url': datalist.data('datalist-url') + '?Id=' + id + '&RecordsPerPage=1&Page=0',
                'async': true,
                'cache': false,
                'dataType': 'json',
                'success': function (data) {
                    if (data.Rows.length > 0) {
                        select(hiddenInput, datalistInput, data.Rows[0]['DatalistIdKey'], data.Rows[0]['DatalistAcKey']);
                    }
                }
            });
        }
    }
    function bindFilters(datalistInput, datalist) {
        var filters = datalist.attr('data-filters').split(',');
        filters = $.grep(filters, function (item) { return (item != null && item != ''); });
        for (i = 0; i < filters.length; i++) {
            $('#' + filters[i]).change(function () {
                datalistInput.val('').keyup();
            });
        }
    }

    function enableSorting() {
        $(document).on('click', '.datalist-table-head th', function () {
            var header = $(this);
            if (!header.attr('data-column')) return false;
            var datalist = header.parents('.datalist:first');
            if (datalist.attr('data-sort-column') == header.attr('data-column'))
                datalist.attr('data-sort-order', datalist.attr('data-sort-order') == 'ASC' ? 'DESC' : 'ASC');
            else
                datalist.attr('data-sort-order', 'ASC');

            datalist.attr('data-sort-column', header.attr('data-column'));
            update(datalist);
        });
    }
    function setLanguage() {
        if ($.fn.datalist.lang == null) {
            $.fn.datalist.lang = {
                Error: "Error while retrieving records",
                NoDataFound: "No data found",
                Search: "Search..."
            };
        }
    }
}(jQuery));
