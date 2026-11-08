(function () {
    "use strict";

    var frame = document.getElementById("pdf-frame");
    var frameBox = frame && frame.closest(".pdf-frame-box");
    var iosViewer = document.getElementById("pdf-ios-viewer");
    var loading = document.getElementById("pdf-loading");
    var error = document.getElementById("pdf-error");
    var pageNumber = document.getElementById("pdf-page-number");
    var pageCount = document.getElementById("pdf-page-count");
    var pageInput = document.getElementById("pdf-page-input");
    var pageGo = document.getElementById("pdf-page-go");
    var reader = frame && frame.closest(".pdf-reader-custom");
    var prev = document.getElementById("pdf-prev");
    var next = document.getElementById("pdf-next");
    var pdfUrl = frame && frame.getAttribute("data-pdf-url");
    var currentPage = 1;
    var totalPages = 0;
    var iosMode = false;

    function isIOSDevice() {
        return /iPad|iPhone|iPod/.test(navigator.userAgent) ||
            (navigator.platform === "MacIntel" && navigator.maxTouchPoints > 1);
    }

    function pageUrl(page) {
        var separator = pdfUrl.indexOf("?") === -1 ? "?" : "&";
        var source = pdfUrl + separator + "viewerPage=" + page;

        return source + "#page=" + page +
            "&zoom=page-width&view=FitH&toolbar=0&navpanes=0&scrollbar=1";
    }

    function updateButtons() {
        if (prev) prev.disabled = currentPage <= 1;
        if (next) next.disabled = totalPages > 0 && currentPage >= totalPages;
        if (pageInput && document.activeElement !== pageInput) pageInput.value = currentPage;
    }

    function setCurrentPage(page) {
        currentPage = Math.max(1, totalPages ? Math.min(totalPages, page) : page);
        pageNumber.textContent = currentPage;
        updateButtons();
    }

    function showPage(page) {
        if (page < 1) return;

        currentPage = page;
        pageNumber.textContent = currentPage;
        updateButtons();

        var replacement = frame.cloneNode(false);
        replacement.removeAttribute("src");
        replacement.addEventListener("load", function () {
            loading.hidden = true;
            error.hidden = true;
        }, { once: true });

        frame.parentNode.replaceChild(replacement, frame);
        frame = replacement;

        window.requestAnimationFrame(function () {
            frame.src = pageUrl(currentPage);
        });
    }

    function clearRenderedPage(holder) {
        var canvas = holder.querySelector("canvas");
        if (canvas) {
            canvas.width = 1;
            canvas.height = 1;
            canvas.remove();
        }
        holder.dataset.rendered = "0";
    }


    async function registerPersianFallbackFonts() {
        if (!window.FontFace || !document.fonts || !iosViewer) return;

        var regularUrl = iosViewer.dataset.fallbackFont;
        var boldUrl = iosViewer.dataset.fallbackFontBold;
        var aliases = ["Vazirmatn", "Vazirmatn Regular", "Vazirmatn-Regular", "Vazir"];
        var jobs = [];

        aliases.forEach(function (name) {
            var face = new FontFace(name, 'url("' + regularUrl + '")', { weight: "400", style: "normal" });
            jobs.push(face.load().then(function (loaded) { document.fonts.add(loaded); }).catch(function () { return null; }));
        });

        ["Vazirmatn", "Vazirmatn Bold", "Vazirmatn-Bold", "Vazir Bold"].forEach(function (name) {
            var face = new FontFace(name, 'url("' + boldUrl + '")', { weight: "700", style: "normal" });
            jobs.push(face.load().then(function (loaded) { document.fonts.add(loaded); }).catch(function () { return null; }));
        });

        await Promise.all(jobs);
    }
    function goToIOSPage(page, smooth) {
        if (!iosMode || !iosViewer) return;
        page = Math.max(1, Math.min(totalPages, Number(page) || 1));
        var holder = iosViewer.querySelector('[data-page="' + page + '"]');
        if (!holder) return;
        setCurrentPage(page);
        frameBox.scrollTo({
            top: Math.max(0, holder.offsetTop - iosViewer.offsetTop),
            behavior: smooth ? "smooth" : "auto"
        });
    }

    async function startIOSViewer() {
        if (!Promise.withResolvers) {
            Promise.withResolvers = function () {
                var resolve, reject;
                var promise = new Promise(function (res, rej) {
                    resolve = res;
                    reject = rej;
                });
                return { promise: promise, resolve: resolve, reject: reject };
            };
        }
        iosMode = true;
        frameBox.classList.add("ios-pdf-mode");
        if (reader) reader.classList.add("ios-reader-mode");
        iosViewer.hidden = false;
        frame.hidden = true;

        try {
            var pdfjs = await import(iosViewer.dataset.pdfModule);
            pdfjs.GlobalWorkerOptions.workerSrc = iosViewer.dataset.pdfWorker;

            var task = pdfjs.getDocument({
                url: iosViewer.dataset.pdfUrl,
                cMapUrl: iosViewer.dataset.pdfCmaps,
                cMapPacked: true,
                standardFontDataUrl: iosViewer.dataset.pdfFonts,
                disableFontFace: true,
                useSystemFonts: false,
                fontExtraProperties: true,
                rangeChunkSize: 65536,
                disableAutoFetch: true,
                disableStream: true,
                isEvalSupported: false
            });
            var pdf = await task.promise;
            totalPages = pdf.numPages;
            if (pageCount) pageCount.textContent = totalPages;
            if (pageInput) pageInput.max = totalPages;
            updateButtons();
            var firstPage = await pdf.getPage(1);
            var firstViewport = firstPage.getViewport({ scale: 1 });
            var pageRatio = firstViewport.width / firstViewport.height;
            firstPage.cleanup();

            var fragment = document.createDocumentFragment();
            for (var i = 1; i <= pdf.numPages; i++) {
                var holder = document.createElement("div");
                holder.className = "pdf-ios-page";
                holder.dataset.page = String(i);
                holder.dataset.rendered = "0";
                holder.style.aspectRatio = firstViewport.width + " / " + firstViewport.height;
                holder.style.minHeight = Math.round(Math.max(1, iosViewer.clientWidth) / pageRatio) + "px";
                fragment.appendChild(holder);
            }
            iosViewer.appendChild(fragment);
            loading.hidden = true;

            var renderQueue = Promise.resolve();

            async function renderPage(holder) {
                if (holder.dataset.visible !== "1" || holder.dataset.rendered === "1") return;
                holder.dataset.rendered = "loading";

                var page = await pdf.getPage(Number(holder.dataset.page));
                if (holder.dataset.visible !== "1") {
                    holder.dataset.rendered = "0";
                    page.cleanup();
                    return;
                }

                var base = page.getViewport({ scale: 1 });
                var cssWidth = Math.max(1, holder.clientWidth);
                var viewport = page.getViewport({ scale: cssWidth / base.width });
                var deviceRatio = window.devicePixelRatio || 1;
                var isIOSRender = isIOSDevice();
                var isMobileRender = window.matchMedia("(max-width: 768px)").matches;
                var pixelRatio = isIOSRender
                    ? Math.min(deviceRatio * 2, 6)
                    : isMobileRender
                        ? Math.min(deviceRatio * 1.5, 4.5)
                        : Math.min(Math.max(deviceRatio * 1.5, 2), 3);
                var canvas = document.createElement("canvas");
                var context = canvas.getContext("2d", { alpha: false });

                canvas.width = Math.ceil(viewport.width * pixelRatio);
                canvas.height = Math.ceil(viewport.height * pixelRatio);
                canvas.style.width = "100%";
                canvas.style.height = "auto";
                holder.style.minHeight = "0";
                holder.appendChild(canvas);

                await page.render({
                    canvasContext: context,
                    viewport: viewport,
                    transform: pixelRatio === 1 ? null : [pixelRatio, 0, 0, pixelRatio, 0, 0],
                    background: "rgb(255,255,255)"
                }).promise;

                holder.dataset.rendered = "1";
                page.cleanup();

                if (holder.dataset.visible !== "1") clearRenderedPage(holder);
            }

            var observer = new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    var holder = entry.target;
                    if (entry.isIntersecting) {
                        holder.dataset.visible = "1";
                        renderQueue = renderQueue.then(function () {
                            return renderPage(holder);
                        }).catch(function () {
                            holder.dataset.rendered = "0";
                        });
                    } else {
                        holder.dataset.visible = "0";
                        if (holder.dataset.rendered === "1") clearRenderedPage(holder);
                    }
                });
            }, {
                root: frameBox,
                rootMargin: isIOSDevice() ? "40% 0px" : "120% 0px",
                threshold: 0.01
            });

            var holders = Array.prototype.slice.call(iosViewer.querySelectorAll(".pdf-ios-page"));
            holders.forEach(function (holder) {
                observer.observe(holder);
            });

            var scrollTicking = false;
            frameBox.addEventListener("scroll", function () {
                if (scrollTicking) return;
                scrollTicking = true;
                window.requestAnimationFrame(function () {
                    scrollTicking = false;
                    var targetTop = frameBox.scrollTop + 8;
                    var nearest = holders[0];
                    var nearestDistance = Infinity;
                    holders.forEach(function (holder) {
                        var distance = Math.abs((holder.offsetTop - iosViewer.offsetTop) - targetTop);
                        if (distance < nearestDistance) {
                            nearestDistance = distance;
                            nearest = holder;
                        }
                    });
                    if (nearest) setCurrentPage(Number(nearest.dataset.page));
                });
            }, { passive: true });
        } catch (exception) {
            iosMode = false;
            frameBox.classList.remove("ios-pdf-mode");
            if (reader) reader.classList.remove("ios-reader-mode");
            iosViewer.hidden = true;
            frame.hidden = false;
            error.hidden = false;
            loading.hidden = true;
            frame.src = pageUrl(1);
        }
    }

    if (prev) {
        prev.addEventListener("click", function () {
            if (iosMode) goToIOSPage(currentPage - 1, true);
            else showPage(currentPage - 1);
        });
    }

    if (next) {
        next.addEventListener("click", function () {
            if (iosMode) goToIOSPage(currentPage + 1, true);
            else showPage(currentPage + 1);
        });
    }


    if (pageGo) {
        pageGo.addEventListener("click", function () {
            if (iosMode) goToIOSPage(pageInput.value, true);
            else showPage(Number(pageInput.value));
        });
    }

    if (pageInput) {
        pageInput.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                pageGo.click();
            }
        });
    }
    document.addEventListener("contextmenu", function (event) {
        if (event.target.closest("[data-protected-media]")) {
            event.preventDefault();
            event.stopPropagation();
        }
    }, true);

    document.addEventListener("keydown", function (event) {
        if ((event.ctrlKey || event.metaKey) &&
            (event.key.toLowerCase() === "s" || event.key.toLowerCase() === "p")) {
            event.preventDefault();
        }
    });

    updateButtons();

    if (iosViewer && frameBox) {
        startIOSViewer();
        return;
    }

    currentPage = 0;
    showPage(1);
}());
