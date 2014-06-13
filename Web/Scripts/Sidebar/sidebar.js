// Search
(function () {
    $(document).on('keyup', '#SearchInput', function () {
        var searchString = $(this).val().toLowerCase();
        if ($('#Sidebar').width() < 100)
            searchString = '';

        var menus = $('#SidebarNavigation li');
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
    $(document).on('mouseenter', '#SidebarNavigation a', function () {
        var parent = $(this).parent();
        if (!parent.hasClass('active') && !parent.hasClass('inactive'))
            $('#SidebarNavigation .active').removeClass('active').addClass('inactive');
        else
            parent.removeClass('inactive').addClass('active');

        if (!parent.hasClass('has-active') && !parent.hasClass('has-active-hover'))
            $('#SidebarNavigation .has-active').removeClass('has-active').addClass('has-active-hover');
        else
            parent.removeClass('has-active-hover').addClass('has-active');
    });

    $(document).on('mouseleave', '#SidebarNavigation', function () {
        $('#SidebarNavigation .has-active-hover').removeClass('has-active-hover').addClass('has-active');
        $('#SidebarNavigation .inactive').removeClass('inactive').addClass('active');

        if ($('#Sidebar').width() < 100) {
            var submenu = $('#SidebarNavigation li.open');
            submenu.toggleClass('closing');
            submenu.toggleClass('open');
            submenu.children('ul').fadeOut(200, function () {
                $(this).parent().toggleClass('closing');
            });
        }
    });
}());

// Clicking
(function () {
    $(document).on('click', '.submenu > a', function (e) {
        if ($(window).width() > 767)
            toggleSubmenu($(this));
        else
            toggleDropdown($(this));
    });

    function toggleSubmenu(submenuAction) {
        var submenu = submenuAction.parent();
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
            submenuAction.siblings('ul').slideUp({
                complete: function () {
                    submenu.toggleClass('closing');
                }
            });
        } else {
            submenu.toggleClass('opening');
            submenuAction.siblings('ul').slideDown({
                complete: function () {
                    submenu.toggleClass('open');
                    submenu.toggleClass('opening');
                }
            });
        }
    }
    function toggleDropdown(submenuAction) {
        var submenu = submenuAction.parent();
        var openedMenu = submenu.siblings('.open');
        if (openedMenu.length > 0) {
            openedMenu.toggleClass('closing');
            openedMenu.toggleClass('open');
            openedMenu.children('ul').fadeOut(200, function () {
                submenu.toggleClass('closing');
            });
        }

        if (submenu.hasClass('open')) {
            submenu.toggleClass('closing');
            submenu.toggleClass('open');
            submenuAction.siblings('ul').fadeOut(200, function () {
                submenu.toggleClass('closing');
            });
        } else {
            submenu.toggleClass('opening');
            submenuAction.siblings('ul').fadeIn(200, function () {
                submenu.toggleClass('open');
                submenu.toggleClass('opening');
            });
        }
    }
}());

// Rendering on low resolutions
(function () {
    if ($('#Sidebar').width() < 100)
        $('#SidebarNavigation li.open').removeClass('open');

    $(window).on('resize', function () {
        if ($('#Sidebar').width() < 100)
            $('#SidebarNavigation li.open').removeClass('open').children('ul').hide();
        else
            $('#SidebarNavigation li.has-active').addClass('open').children('ul').show();

        $('#SearchInput').keyup();
    });
}());