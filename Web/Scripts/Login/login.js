// Alerts closing & fading
(function () {
    $(document).on('click', '.alert a.close', function () {
        $(this).parent().parent().fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });
    });

    $(document).on('keyup', '.login-input', function () {
        $(this).parent().removeClass('has-error');
        $('#LoginBox .alert').fadeTo(300, 0).slideUp(300, function () {
            $('#LoginBox .alert').remove();
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

// Login form submit handling
(function () {
    $('#LoginBox > form').on('submit').submit(function (e) {
        var thisForm = $(this);
        var userinput = $('#Username');
        var passinput = $('#Password');

        if (userinput.val() == '' || passinput.val() == '') {
            e.preventDefault();
            toggleError(userinput);
            toggleError(passinput);
            $('#LoginBox').effect('shake', { times: 2, distance: 10 }, 100);

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
