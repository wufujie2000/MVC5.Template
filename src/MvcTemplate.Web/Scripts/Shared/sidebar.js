// Search
(function () {
    $(document).on('keyup', '#SearchInput', function () {
        var searchString = this.value.toLowerCase();
        if ($('.sidebar').width() < 100) {
            searchString = '';
        }

        var menus = $('.sidebar li');
        for (var i = 0; i < menus.length; i++) {
            var menu = $(menus[i]);
            if (menu.text().toLowerCase().indexOf(searchString) >= 0) {
                if (menu.hasClass('submenu')) {
                    if (menu.find('li:not(.submenu)').text().toLowerCase().indexOf(searchString) >= 0) {
                        menu.show(500);
                    } else {
                        menu.hide(500);
                    }
                } else {
                    menu.show(500);
                }
            } else {
                menu.hide(500);
            }
        }
    });
}());

// Hovering
(function () {
    $(document).on('mouseleave', '.sidebar > ul', function () {
        if ($('.sidebar').width() < 100) {
            var submenu = $('.sidebar li.open');
            submenu.children('ul').fadeOut();
            submenu.toggleClass('open');
        }
    });
}());

// Clicking
(function () {
    $(document).on('click', '.submenu > a', function (e) {
        e.preventDefault();
        var action = $(this);
        var submenu = action.parent();
        var openSiblings = submenu.siblings('.open');

        if ($(window).width() > 767) {
            openSiblings.toggleClass('changing');
            openSiblings.children('ul').slideUp(function () {
                openSiblings.removeClass('changing open');
            });

            submenu.toggleClass('changing');
            action.next('ul').slideToggle(function () {
                submenu.toggleClass('changing open');
            });
        } else {
            openSiblings.children('ul').fadeOut(function () {
                openSiblings.removeClass('open');
            });

            action.next('ul').fadeToggle(function () {
                submenu.toggleClass('open');
            });
        }
    });
}());

// Rendering on low resolutions
(function () {
    if ($('.sidebar').width() < 100) {
        $('.sidebar li.open').removeClass('open');
    }

    $(window).on('resize', function () {
        if ($('.sidebar').width() < 100) {
            $('.sidebar .open').removeClass('open').children('ul').hide();
            $('#SearchInput').keyup();
        }
    });
}());
