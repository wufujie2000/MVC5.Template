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

    $(document).on('mouseenter', '#SidebarNavigationList > li > a', function () {
        var liElement = $(this).parent();
        if (liElement.hasClass('has-active-child'))
            liElement.removeClass('active-child-hovering');
        else
            $('#SidebarNavigationList > li.has-active-child').addClass('active-child-hovering');
    });

    $(document).on('mouseenter', '#SidebarNavigationList > li > ul a', function () {
        $('#SidebarNavigationList > .has-active-child').addClass('active-child-hovering');
    });

    $(document).on('mouseleave', '#SidebarNavigationList', function () {
        $('#SidebarNavigationList li.active-hovering').removeClass('active-hovering');
        $('#SidebarNavigationList li.active-child-hovering').removeClass('active-child-hovering');

        if ($('#Sidebar').width() < 100) {
            var submenu = $('#SidebarNavigationList li.open');
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
        }
        else {
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
        }
        else {
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
        $('#SidebarNavigationList li.open').removeClass('open');

    $(window).on('resize', function () {
        if ($('#Sidebar').width() < 100)
            $('#SidebarNavigationList li.open').removeClass('open').children('ul').hide();
        else
            $('#SidebarNavigationList li.has-active-child').addClass('open').children('ul').show();
    });
}());