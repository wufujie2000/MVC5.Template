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

// Auth form submit handling
(function () {
    $('form').on('submit').submit(function (e) {
        var thisForm = $(this);
        var userInput = $('#Username');
        var passInput = $('#Password');
        var emailInput = $('#Email');

        if (userInput.val() == '' || passInput.val() == '' || emailInput.val() == '') {
            toggleError(emailInput);
            toggleError(passInput);
            toggleError(userInput);
            e.preventDefault();

            return false;
        } else {
            return true;
        }
    });

    function toggleError(input) {
        if (input.val() == '')
            input.parent().addClass('has-error');
        else
            input.parent().removeClass('has-error');
    }
}());
