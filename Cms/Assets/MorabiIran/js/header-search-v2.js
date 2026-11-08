(function () {
    'use strict';

    var desktopWidget = document.querySelector('.mr-search-desktop');
    var mobileNav = document.querySelector('.mr-mobile-nav');
    if (desktopWidget && mobileNav && !mobileNav.querySelector('.mr-search-mobile')) {
        var mobileWidget = desktopWidget.cloneNode(true);
        mobileWidget.classList.remove('mr-search-desktop');
        mobileWidget.classList.add('mr-search-mobile');
        mobileWidget.querySelector('.mr-search-panel').innerHTML = '';
        mobileNav.insertBefore(mobileWidget, mobileNav.firstChild);
    }

    var widgets = Array.prototype.slice.call(document.querySelectorAll('.mr-search-widget'));
    if (!widgets.length) return;

    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (character) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[character];
        });
    }

    function getList(data, name) {
        return data[name] || data[name.toLowerCase()] || [];
    }

    function getValue(item, names) {
        for (var index = 0; index < names.length; index++) {
            if (item && item[names[index]] != null) return item[names[index]];
        }
        return '';
    }

    function resultRow(item, type) {
        var title = getValue(item, ['Fullname', 'FullName', 'Name', 'Title', 'fullname', 'name', 'title']);
        var href = getValue(item, ['Href', 'Url', 'href', 'url']) || '#';
        var picture = getValue(item, ['Picture', 'picture']) || {};
        var image = getValue(picture, ['Url', 'url']) || getValue(item, ['Image', 'image']);
        var initial = String(title).trim().charAt(0) || 'م';
        var thumb = image
            ? '<img src="' + escapeHtml(image) + '" alt="" loading="lazy" onerror="this.remove()">'
            : escapeHtml(initial);
        return '<a class="mr-search-result" href="' + escapeHtml(href) + '">' +
            '<span class="mr-result-thumb">' + thumb + '</span>' +
            '<span><strong>' + escapeHtml(title) + '</strong><small>' + escapeHtml(type) + '</small></span>' +
            '<span class="mr-result-arrow">←</span></a>';
    }

    function resultGroup(title, type, items) {
        if (!items || !items.length) return '';
        return '<section class="mr-search-group"><span class="mr-search-group-title">' + title + '</span>' +
            items.map(function (item) { return resultRow(item, type); }).join('') + '</section>';
    }

    function renderResults(panel, data, query) {
        var html = resultGroup('دسته‌بندی‌ها', 'دسته‌بندی', getList(data, 'Categories')) +
            resultGroup('دوره‌ها و محصولات', 'دوره یا محصول', getList(data, 'Products')) +
            resultGroup('برندها', 'برند', getList(data, 'Brands')) +
            resultGroup('مجله و محتوا', 'مقاله', getList(data, 'Posts'));
        var total = data.TotalCount != null ? data.TotalCount : data.totalCount;
        if (!html || total === 0) {
            html = '<div class="mr-search-status">نتیجه‌ای برای «' + escapeHtml(query) + '» پیدا نشد.</div>';
        }
        panel.innerHTML = html;
    }

    widgets.forEach(function (widget) {
        var input = widget.querySelector('.mr-search-input');
        var panel = widget.querySelector('.mr-search-panel');
        var clearButton = widget.querySelector('.mr-search-clear');
        var timer = null;
        var controller = null;
        var lastQuery = '';
        if (!input || !panel) return;

        function closePanel() {
            widget.classList.remove('is-open');
        }

        function resetSearch() {
            if (controller) controller.abort();
            clearTimeout(timer);
            lastQuery = '';
            input.value = '';
            panel.innerHTML = '';
            widget.classList.remove('has-value', 'is-open');
        }

        function runSearch() {
            var query = input.value.trim();
            widget.classList.toggle('has-value', query.length > 0);
            if (query.length < 3) {
                if (controller) controller.abort();
                panel.innerHTML = query.length ? '<div class="mr-search-status">حداقل سه حرف وارد کنید.</div>' : '';
                widget.classList.toggle('is-open', query.length > 0);
                return;
            }
            if (query === lastQuery) return;
            lastQuery = query;
            if (controller) controller.abort();
            controller = window.AbortController ? new AbortController() : null;
            panel.innerHTML = '<div class="mr-search-status"><span class="mr-search-spinner"></span>در حال جستجو...</div>';
            widget.classList.add('is-open');
            fetch('/cpr/search?q=' + encodeURIComponent(query) + '&size=5', {
                headers: { Accept: 'application/json' },
                credentials: 'same-origin',
                signal: controller ? controller.signal : undefined
            }).then(function (response) {
                if (!response.ok) throw new Error('search failed');
                return response.json();
            }).then(function (data) {
                renderResults(panel, data || {}, query);
            }).catch(function (error) {
                if (error && error.name === 'AbortError') return;
                panel.innerHTML = '<div class="mr-search-status">جستجو انجام نشد؛ دوباره تلاش کنید.</div>';
            });
        }

        input.addEventListener('input', function () {
            clearTimeout(timer);
            timer = setTimeout(runSearch, 120);
        });
        input.addEventListener('focus', function () {
            if (panel.innerHTML) widget.classList.add('is-open');
        });
        input.addEventListener('keydown', function (event) {
            if (event.key === 'Escape') {
                closePanel();
                input.blur();
            } else if (event.key === 'Enter') {
                var firstResult = panel.querySelector('.mr-search-result');
                if (firstResult) window.location.href = firstResult.href;
            }
        });
        if (clearButton) clearButton.addEventListener('click', function () {
            resetSearch();
            input.focus();
        });
        document.addEventListener('click', function (event) {
            if (!widget.contains(event.target)) closePanel();
        });
    });
})();
