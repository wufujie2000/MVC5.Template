// Alerts closing & fading
(function () {
    $(document).on('click', '.alert a.close', function () {
        $(this).parent().parent().fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });
    });
}());

// Language selector
(function () {
    $("#Language").select2({
        minimumResultsForSearch: -1,
        formatResult: formatSelection,
        formatSelection: formatSelection
    }).select2('val', currentLanguage);
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
    $('#AuthBox > form').on('submit').submit(function (e) {
        var thisForm = $(this);
        var userInput = $('#Username');
        var passInput = $('#Password');
        var emailInput = $('#Email');

        if (userInput.val() == '' || passInput.val() == '' || emailInput.val() == '') {
            e.preventDefault();
            toggleError(userInput);
            toggleError(passInput);
            toggleError(emailInput);
            $('#AuthBox').effect('shake', { times: 2, distance: 10 }, 300);

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
