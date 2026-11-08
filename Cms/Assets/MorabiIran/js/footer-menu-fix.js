(function () {
    'use strict';

    function findFooterMenuButton() {
        var direct = document.querySelector(
            '#mobileFooterMenu, #mobile-footer-menu, [data-footer-menu], ' +
            '.mobile-footer-menu, .footer-menu-button, .bottom-nav-menu, .mobile-bottom-menu'
        );
        if (direct) return direct;

        var nodes = document.querySelectorAll(
            '.mobile-bottom-nav a, .mobile-bottom-nav button, ' +
            '.bottom-navigation a, .bottom-navigation button, ' +
            '.mobile-fixed-nav a, .mobile-fixed-nav button, ' +
            'footer a, footer button'
        );

        for (var i = 0; i < nodes.length; i++) {
            var label = (nodes[i].textContent || '').replace(/\s+/g, ' ').trim();
            if (label === 'منو' || label === 'منوی سایت') return nodes[i];
        }

        var allActions = document.querySelectorAll('a, button');
        for (var j = 0; j < allActions.length; j++) {
            var text = (allActions[j].textContent || '').replace(/\s+/g, ' ').trim();
            var rect = allActions[j].getBoundingClientRect();
            if ((text === 'منو' || text === 'منوی سایت') && rect.bottom > window.innerHeight - 180) {
                return allActions[j];
            }
        }
        return null;
    }

    function findHeaderMenuButton(footerButton) {
        var selectors = [
            '#mobileMenuToggle', '#mobile-menu-toggle', '#menuToggle',
            '[data-menu-toggle]', '[data-mobile-menu-toggle]',
            '.mobile-menu-toggle', '.navbar-toggler', '.menu-toggle',
            '.hamburger-menu', '.hamburger', '.menu-btn', '.mobile-menu-btn'
        ];
        var candidates = [];
        var seen = [];

        function addCandidate(action) {
            if (!action || seen.indexOf(action) !== -1 || action === footerButton) return;
            seen.push(action);

            var rect = action.getBoundingClientRect();
            var style = window.getComputedStyle(action);
            var isBottomNav = action.closest('footer, .mobile-bottom-nav, .bottom-navigation, .mobile-fixed-nav');
            var isLegacy = action.closest('.old-header, .legacy-header, .old-menu, .legacy-menu');
            var isVisible = style.display !== 'none' && style.visibility !== 'hidden' &&
                parseFloat(style.opacity || '1') > 0 && rect.width > 20 && rect.height > 20;

            if (!isBottomNav && !isLegacy && isVisible && rect.top >= 0 && rect.top < 220) {
                candidates.push({ node: action, rect: rect });
            }
        }

        for (var i = 0; i < selectors.length; i++) {
            var matches = document.querySelectorAll(selectors[i]);
            for (var j = 0; j < matches.length; j++) addCandidate(matches[j]);
        }

        var topActions = document.querySelectorAll(
            '.morabi-header button, .morabi-header a, .new-header button, .new-header a, ' +
            'header button, header a, .site-header button, .site-header a'
        );
        for (var k = 0; k < topActions.length; k++) {
            var action = topActions[k];
            var aria = (action.getAttribute('aria-label') || '').toLowerCase();
            var title = (action.getAttribute('title') || '').toLowerCase();
            if (aria.indexOf('menu') !== -1 || aria.indexOf('منو') !== -1 ||
                title.indexOf('menu') !== -1 || title.indexOf('منو') !== -1 ||
                action.matches('.menu-toggle, .hamburger, .hamburger-menu, .menu-btn, .mobile-menu-btn')) {
                addCandidate(action);
            }
        }

        if (!candidates.length) return null;

        /* The redesigned hamburger is the visible top action nearest the right edge. */
        candidates.sort(function (a, b) {
            var aRight = window.innerWidth - a.rect.right;
            var bRight = window.innerWidth - b.rect.right;
            return aRight - bRight || a.rect.top - b.rect.top;
        });
        return candidates[0].node;
    }

    function bindFooterMenu() {
        var footerButton = findFooterMenuButton();
        if (!footerButton || footerButton.getAttribute('data-menu-bound') === '1') return;

        footerButton.setAttribute('data-menu-bound', '1');
        footerButton.setAttribute('href', '#');
        footerButton.addEventListener('click', function (event) {
            event.preventDefault();
            event.stopPropagation();

            var headerButton = findHeaderMenuButton(footerButton);
            if (headerButton) {
                headerButton.dispatchEvent(new MouseEvent('click', {
                    bubbles: true,
                    cancelable: true,
                    view: window
                }));
            }
        }, true);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', bindFooterMenu);
    } else {
        bindFooterMenu();
    }

    window.addEventListener('load', bindFooterMenu);
    setTimeout(bindFooterMenu, 500);
}());
