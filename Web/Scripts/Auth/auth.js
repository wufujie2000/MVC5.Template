// Language selector
(function () {
    $("#Language").each(function () {
        $(this).select2({
            minimumResultsForSearch: -1,
            formatResult: formatSelection,
            formatSelection: formatSelection
        }).select2('val', currentLanguage);
    });
    $('#TempLanguage').hide();

    $(document).on("change", "#Language", function (e) {
        if (e.val == 'en-GB')
            document.location = '/Auth/Login';
        else
            document.location = '/' + e.val + '/Auth/Login';
    });

    function formatSelection(selection) {
        return "<img src='/Images/Flags/" + selection.id + ".gif' /> " + selection.text;
    }
}());
