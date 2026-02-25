/* ══════════════════════════════════════════════
   rhx-split-panel.js — Resizable split panel
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxSplitInit) return;
        root._rhxSplitInit = true;

        var startPanel = root.querySelector('.rhx-split-panel__start');
        var divider = root.querySelector('.rhx-split-panel__divider');
        var endPanel = root.querySelector('.rhx-split-panel__end');

        if (!startPanel || !divider || !endPanel) return;

        // ── Read config ──

        var isVertical = root.hasAttribute('data-rhx-vertical');
        var isDisabled = root.hasAttribute('data-rhx-disabled');

        var snapPoints = [];
        var snapAttr = root.getAttribute('data-rhx-snap');
        if (snapAttr) {
            snapPoints = snapAttr.split(',').map(function (s) {
                return parseFloat(s.trim());
            }).filter(function (n) {
                return !isNaN(n);
            });
        }

        var snapThreshold = parseInt(root.getAttribute('data-rhx-snap-threshold'), 10) || 12;

        // ── Helpers ──

        function getContainerSize() {
            return isVertical ? root.offsetHeight : root.offsetWidth;
        }

        function getDividerSize() {
            return isVertical ? divider.offsetHeight : divider.offsetWidth;
        }

        function clamp(value, min, max) {
            return Math.max(min, Math.min(max, value));
        }

        function applySnap(percent) {
            if (snapPoints.length === 0) return percent;

            var containerSize = getContainerSize();
            var thresholdPercent = (snapThreshold / containerSize) * 100;

            for (var i = 0; i < snapPoints.length; i++) {
                if (Math.abs(percent - snapPoints[i]) <= thresholdPercent) {
                    return snapPoints[i];
                }
            }
            return percent;
        }

        function setPosition(percent) {
            percent = clamp(percent, 0, 100);
            startPanel.style.flexBasis = percent + '%';
            divider.setAttribute('aria-valuenow', Math.round(percent).toString());

            root.dispatchEvent(new CustomEvent('rhx:split:change', {
                bubbles: true,
                detail: { position: percent }
            }));
        }

        // ── Mouse/Touch drag ──

        var dragging = false;
        var startPos = 0;
        var startBasis = 0;

        function onPointerDown(e) {
            if (isDisabled) return;
            if (e.button && e.button !== 0) return; // only primary button

            e.preventDefault();
            dragging = true;
            root.classList.add('rhx-split-panel--dragging');

            var containerSize = getContainerSize();
            startBasis = (parseFloat(startPanel.style.flexBasis) || 50);
            startPos = isVertical ? (e.touches ? e.touches[0].clientY : e.clientY)
                                  : (e.touches ? e.touches[0].clientX : e.clientX);

            document.addEventListener('mousemove', onPointerMove);
            document.addEventListener('mouseup', onPointerUp);
            document.addEventListener('touchmove', onPointerMove, { passive: false });
            document.addEventListener('touchend', onPointerUp);
        }

        function onPointerMove(e) {
            if (!dragging) return;
            e.preventDefault();

            var currentPos = isVertical ? (e.touches ? e.touches[0].clientY : e.clientY)
                                        : (e.touches ? e.touches[0].clientX : e.clientX);
            var containerSize = getContainerSize();
            var delta = currentPos - startPos;
            var deltaPercent = (delta / containerSize) * 100;
            var newPercent = applySnap(startBasis + deltaPercent);

            setPosition(newPercent);
        }

        function onPointerUp() {
            if (!dragging) return;
            dragging = false;
            root.classList.remove('rhx-split-panel--dragging');

            document.removeEventListener('mousemove', onPointerMove);
            document.removeEventListener('mouseup', onPointerUp);
            document.removeEventListener('touchmove', onPointerMove);
            document.removeEventListener('touchend', onPointerUp);
        }

        divider.addEventListener('mousedown', onPointerDown);
        divider.addEventListener('touchstart', onPointerDown, { passive: false });

        // ── Keyboard ──

        var STEP = 1;
        var LARGE_STEP = 10;

        divider.addEventListener('keydown', function (e) {
            if (isDisabled) return;

            var current = parseFloat(divider.getAttribute('aria-valuenow')) || 50;
            var step = e.shiftKey ? LARGE_STEP : STEP;
            var newVal = current;

            if (isVertical) {
                switch (e.key) {
                    case 'ArrowUp':
                        e.preventDefault();
                        newVal = current - step;
                        break;
                    case 'ArrowDown':
                        e.preventDefault();
                        newVal = current + step;
                        break;
                    case 'Home':
                        e.preventDefault();
                        newVal = 0;
                        break;
                    case 'End':
                        e.preventDefault();
                        newVal = 100;
                        break;
                    default:
                        return;
                }
            } else {
                switch (e.key) {
                    case 'ArrowLeft':
                        e.preventDefault();
                        newVal = current - step;
                        break;
                    case 'ArrowRight':
                        e.preventDefault();
                        newVal = current + step;
                        break;
                    case 'Home':
                        e.preventDefault();
                        newVal = 0;
                        break;
                    case 'End':
                        e.preventDefault();
                        newVal = 100;
                        break;
                    default:
                        return;
                }
            }

            newVal = applySnap(newVal);
            setPosition(newVal);
        });
    }

    // ── Registration ──

    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('split-panel', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-split-panel]').forEach(init);
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
            el.querySelectorAll('[data-rhx-split-panel]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-split-panel]')) {
            init(el);
        }
    });
})();
