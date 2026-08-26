(function () {
    "use strict";
    var menuButton = document.querySelector(".mobile-menu-button");
    var mobileNavigation = document.getElementById("mobile-navigation");

    if (menuButton && mobileNavigation) {
        menuButton.addEventListener("click", function () {
            var isOpen = menuButton.getAttribute("aria-expanded") === "true";
            menuButton.setAttribute("aria-expanded", isOpen ? "false" : "true");
            mobileNavigation.hidden = isOpen;
        });

        mobileNavigation.addEventListener("click", function (event) {
            if (event.target.closest("a")) {
                menuButton.setAttribute("aria-expanded", "false");
                mobileNavigation.hidden = true;
            }
        });

        window.addEventListener("resize", function () {
            if (window.innerWidth > 820) {
                menuButton.setAttribute("aria-expanded", "false");
                mobileNavigation.hidden = true;
            }
        });
    }
    var loadMore = document.getElementById("load-more");
    var cards = Array.prototype.slice.call(document.querySelectorAll(".course-card"));
    var visible = window.matchMedia("(max-width: 560px)").matches ? 4 : 8;
    function applyVisibility() {
        cards.forEach(function (card, index) { card.classList.toggle("is-hidden", index >= visible); });
        if (loadMore) { loadMore.hidden = cards.length <= visible; }
    }
    if (loadMore) {
        applyVisibility();
        loadMore.addEventListener("click", function () { visible += 4; applyVisibility(); });
    }
    var sort = document.getElementById("course-sort");
    var grid = document.getElementById("course-grid");
    if (sort && grid) {
        sort.addEventListener("change", function () {
            var sorted = cards.slice();
            if (sort.value === "title") {
                sorted.sort(function (a, b) { return a.dataset.title.localeCompare(b.dataset.title, "fa"); });
            } else if (sort.value === "lessons") {
                sorted.sort(function (a, b) { return Number(b.dataset.lessons) - Number(a.dataset.lessons); });
            }
            sorted.forEach(function (card) { grid.appendChild(card); });
        });
    }
    var videoShell = document.getElementById("video-shell");
    var playButton = videoShell && videoShell.querySelector(".big-play");
    if (playButton) {
        playButton.addEventListener("click", function () {
            videoShell.classList.add("is-playing");
            playButton.setAttribute("aria-label", "ویدئو در حال پخش است");
        });
    }
    document.querySelectorAll(".session-item:not(:disabled)").forEach(function (item) {
        item.addEventListener("click", function () {
            document.querySelectorAll(".session-item").forEach(function (session) { session.classList.remove("active"); });
            item.classList.add("active");
        });
    });
}());
