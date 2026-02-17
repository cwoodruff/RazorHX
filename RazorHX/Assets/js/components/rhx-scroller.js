/* ══════════════════════════════════════════════
   rhx-scroller.js — Scroll shadow indicators
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    var THRESHOLD = 2; // pixels of tolerance for edge detection

    function init(root) {
        if (root._rhxScrollerInit) return;
        root._rhxScrollerInit = true;

        var content = root.querySelector('.rhx-scroller__content');
        if (!content) return;

        var startShadow = root.querySelector('.rhx-scroller__shadow--start');
        var endShadow = root.querySelector('.rhx-scroller__shadow--end');
        var orientation = root.getAttribute('data-rhx-orientation') || 'horizontal';

        function update() {
            var showStart = false;
            var showEnd = false;

            if (orientation === 'horizontal' || orientation === 'both') {
                if (content.scrollLeft > THRESHOLD) {
                    showStart = true;
                }
                if (content.scrollLeft < content.scrollWidth - content.clientWidth - THRESHOLD) {
                    showEnd = true;
                }
            }

            if (orientation === 'vertical' || orientation === 'both') {
                if (content.scrollTop > THRESHOLD) {
                    showStart = true;
                }
                if (content.scrollTop < content.scrollHeight - content.clientHeight - THRESHOLD) {
                    showEnd = true;
                }
            }

            if (startShadow) {
                startShadow.classList.toggle('rhx-scroller__shadow--active', showStart);
            }
            if (endShadow) {
                endShadow.classList.toggle('rhx-scroller__shadow--active', showEnd);
            }
        }

        // Listen for scroll events
        content.addEventListener('scroll', update, { passive: true });

        // Observe size changes to re-evaluate shadows
        if (typeof ResizeObserver !== 'undefined') {
            var observer = new ResizeObserver(update);
            observer.observe(content);
            // Also observe children for dynamic content
            var firstChild = content.firstElementChild;
            if (firstChild) {
                observer.observe(firstChild);
            }
        }

        // Initial check
        update();

        // Expose update method on element
        root._rhxScrollerUpdate = update;
    }

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('scroller', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-scroller]').forEach(init);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAll);
    } else {
        initAll();
    }

    // Re-init on htmx content swap
    document.addEventListener('htmx:afterSettle', function (e) {
        var el = e.detail.elt;
        if (el && el.querySelectorAll) {
            el.querySelectorAll('[data-rhx-scroller]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-scroller]')) {
            init(el);
        }
    });
})();
