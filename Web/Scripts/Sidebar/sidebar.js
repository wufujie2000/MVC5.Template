// Search
(function () {
    $(document).on('keyup', '#SearchInput', function () {
        var searchString = $(this).val().toLowerCase();
        var menus = $('#SidebarNavigationList li');
        menus.each(function (index, element) {
            var liElement = $(element);
            if (liElement.text().toLowerCase().contains(searchString))
                if (liElement.hasClass('submenu')) {
                    if (liElement.find('li:not(.submenu)').text().toLowerCase().contains(searchString))
                        liElement.show(500);
                    else
                        liElement.hide(500);
                }
                else
                    liElement.show(500);
            else
                liElement.hide(500);
        });
    });
}());

// Hovering
(function () {
    if ($('#SidebarNavigationList li:not(.active):hover').length == 0)
        $('#SidebarNavigationList li.active').removeClass('active-hovering');
    if ($('#SidebarNavigationList li.active:hover').length > 0)
        $('#SidebarNavigationList li.active').removeClass('active-hovering');

    $(document).on('mouseenter', '#SidebarNavigationList a', function () {
        var liElement = $(this).parent();
        if (liElement.hasClass('active'))
            liElement.removeClass('active-hovering');
        else
            $('#SidebarNavigationList').find('.active').addClass('active-hovering');
    });

    $(document).on('mouseleave', '#SidebarNavigationList', function () {
        $('#SidebarNavigationList li.active').removeClass('active-hovering');
    });
}());

// Clicking
(function () {
    $(document).on('click', '.submenu > a', function (e) {
        var submenu = $(this).parent();
        var openedMenu = submenu.siblings('.open');
        if (openedMenu.length > 0) {
            openedMenu.toggleClass('closing');
            openedMenu.toggleClass('open');
            openedMenu.children('ul').slideUp({
                complete: function () {
                    submenu.toggleClass('closing');
                }
            });
        }

        if (submenu.hasClass('open')) {
            submenu.toggleClass('closing');
            submenu.toggleClass('open');
            $(this).siblings('ul').slideUp({
                complete: function () {
                    submenu.toggleClass('closing');
                }
            });
        }
        else {
            submenu.toggleClass('opening');
            $(this).siblings('ul').slideDown({
                complete: function () {
                    submenu.toggleClass('open');
                    submenu.toggleClass('opening');
                }
            });
        }
    });
}());
