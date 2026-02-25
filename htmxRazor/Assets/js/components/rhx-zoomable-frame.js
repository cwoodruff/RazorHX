/* ══════════════════════════════════════════════
   rhx-zoomable-frame.js — Pinch/scroll to zoom
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxZoomableInit) return;
        root._rhxZoomableInit = true;

        var content = root.querySelector('.rhx-zoomable-frame__content');
        if (!content) return;

        var minScale = parseFloat(root.getAttribute('data-rhx-min-scale')) || 0.5;
        var maxScale = parseFloat(root.getAttribute('data-rhx-max-scale')) || 5;
        var scale = parseFloat(root.getAttribute('data-rhx-scale')) || 1;
        var translateX = 0;
        var translateY = 0;

        function clamp(val, min, max) {
            return Math.max(min, Math.min(max, val));
        }

        function applyTransform() {
            content.style.transform =
                'translate(' + translateX + 'px, ' + translateY + 'px) scale(' + scale + ')';
        }

        function setScale(newScale, originX, originY) {
            newScale = clamp(newScale, minScale, maxScale);

            if (typeof originX === 'number' && typeof originY === 'number') {
                // Adjust translation so zoom centers on pointer
                var ratio = newScale / scale;
                translateX = originX - ratio * (originX - translateX);
                translateY = originY - ratio * (originY - translateY);
            }

            scale = newScale;
            applyTransform();

            root.dispatchEvent(new CustomEvent('rhx:zoom:change', {
                bubbles: true,
                detail: { scale: scale }
            }));
        }

        function reset() {
            scale = 1;
            translateX = 0;
            translateY = 0;
            applyTransform();
        }

        // ── Scroll wheel zoom ──
        root.addEventListener('wheel', function (e) {
            e.preventDefault();
            var rect = root.getBoundingClientRect();
            var originX = e.clientX - rect.left;
            var originY = e.clientY - rect.top;
            var delta = e.deltaY > 0 ? -0.1 : 0.1;
            setScale(scale + delta, originX, originY);
        }, { passive: false });

        // ── Pinch zoom (touch) ──
        var initialPinchDist = 0;
        var initialPinchScale = 1;

        function getPinchDistance(touches) {
            var dx = touches[0].clientX - touches[1].clientX;
            var dy = touches[0].clientY - touches[1].clientY;
            return Math.sqrt(dx * dx + dy * dy);
        }

        root.addEventListener('touchstart', function (e) {
            if (e.touches.length === 2) {
                e.preventDefault();
                initialPinchDist = getPinchDistance(e.touches);
                initialPinchScale = scale;
            }
        }, { passive: false });

        root.addEventListener('touchmove', function (e) {
            if (e.touches.length === 2) {
                e.preventDefault();
                var dist = getPinchDistance(e.touches);
                var ratio = dist / initialPinchDist;
                var rect = root.getBoundingClientRect();
                var midX = (e.touches[0].clientX + e.touches[1].clientX) / 2 - rect.left;
                var midY = (e.touches[0].clientY + e.touches[1].clientY) / 2 - rect.top;
                setScale(initialPinchScale * ratio, midX, midY);
            }
        }, { passive: false });

        // ── Pan (drag to move when zoomed) ──
        var panning = false;
        var panStartX = 0;
        var panStartY = 0;
        var panStartTransX = 0;
        var panStartTransY = 0;

        root.addEventListener('mousedown', function (e) {
            if (e.button !== 0 || scale <= 1) return;
            e.preventDefault();
            panning = true;
            root.classList.add('rhx-zoomable-frame--panning');
            panStartX = e.clientX;
            panStartY = e.clientY;
            panStartTransX = translateX;
            panStartTransY = translateY;

            document.addEventListener('mousemove', onPanMove);
            document.addEventListener('mouseup', onPanEnd);
        });

        function onPanMove(e) {
            if (!panning) return;
            translateX = panStartTransX + (e.clientX - panStartX);
            translateY = panStartTransY + (e.clientY - panStartY);
            applyTransform();
        }

        function onPanEnd() {
            panning = false;
            root.classList.remove('rhx-zoomable-frame--panning');
            document.removeEventListener('mousemove', onPanMove);
            document.removeEventListener('mouseup', onPanEnd);
        }

        // ── Single-touch pan (when zoomed) ──
        var touchPanning = false;
        var touchStartX = 0;
        var touchStartY = 0;

        root.addEventListener('touchstart', function (e) {
            if (e.touches.length === 1 && scale > 1) {
                touchPanning = true;
                touchStartX = e.touches[0].clientX;
                touchStartY = e.touches[0].clientY;
                panStartTransX = translateX;
                panStartTransY = translateY;
            }
        }, { passive: true });

        root.addEventListener('touchmove', function (e) {
            if (touchPanning && e.touches.length === 1 && scale > 1) {
                e.preventDefault();
                translateX = panStartTransX + (e.touches[0].clientX - touchStartX);
                translateY = panStartTransY + (e.touches[0].clientY - touchStartY);
                applyTransform();
            }
        }, { passive: false });

        root.addEventListener('touchend', function () {
            touchPanning = false;
        });

        // ── Double-click to reset ──
        root.addEventListener('dblclick', function () {
            reset();
        });

        // ── Keyboard ──
        root.addEventListener('keydown', function (e) {
            switch (e.key) {
                case '+':
                case '=':
                    e.preventDefault();
                    setScale(scale + 0.25);
                    break;
                case '-':
                    e.preventDefault();
                    setScale(scale - 0.25);
                    break;
                case '0':
                    e.preventDefault();
                    reset();
                    break;
            }
        });

        // Apply initial transform
        applyTransform();

        root._rhxZoomReset = reset;
        root._rhxZoomSetScale = setScale;
    }

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('zoomable-frame', init);
    }

    function initAll() {
        document.querySelectorAll('[data-rhx-zoomable-frame]').forEach(init);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAll);
    } else {
        initAll();
    }

    document.addEventListener('htmx:afterSettle', function (e) {
        var el = e.detail.elt;
        if (el && el.querySelectorAll) {
            el.querySelectorAll('[data-rhx-zoomable-frame]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-zoomable-frame]')) {
            init(el);
        }
    });
})();
