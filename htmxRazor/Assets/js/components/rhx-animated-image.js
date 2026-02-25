/* ══════════════════════════════════════════════
   rhx-animated-image.js — Play/pause animated images
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxAnimatedImageInit) return;
        root._rhxAnimatedImageInit = true;

        var img = root.querySelector('.rhx-animated-image__img');
        var canvas = root.querySelector('.rhx-animated-image__canvas');
        var control = root.querySelector('.rhx-animated-image__control');

        if (!img || !canvas || !control) return;

        var originalSrc = img.src;
        var playing = !root.hasAttribute('data-rhx-paused');

        function captureFrame() {
            canvas.width = img.naturalWidth || img.width;
            canvas.height = img.naturalHeight || img.height;
            var ctx = canvas.getContext('2d');
            if (ctx) {
                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
            }
        }

        function updateIcon() {
            var svg = control.querySelector('svg');
            if (!svg) return;

            if (playing) {
                // Show pause icon
                svg.innerHTML = '<rect x="6" y="4" width="4" height="16" /><rect x="14" y="4" width="4" height="16" />';
                control.setAttribute('aria-label', 'Pause animation');
            } else {
                // Show play icon
                svg.innerHTML = '<polygon points="5 3 19 12 5 21 5 3" />';
                control.setAttribute('aria-label', 'Play animation');
            }
        }

        function pause() {
            if (!playing) return;
            captureFrame();
            playing = false;
            root.classList.add('rhx-animated-image--paused');
            root.setAttribute('data-rhx-paused', '');
            updateIcon();

            root.dispatchEvent(new CustomEvent('rhx:animated-image:pause', {
                bubbles: true,
                detail: { element: root }
            }));
        }

        function play() {
            if (playing) return;
            playing = true;
            root.classList.remove('rhx-animated-image--paused');
            root.removeAttribute('data-rhx-paused');
            // Force reload to restart animation
            img.src = '';
            img.src = originalSrc;
            updateIcon();

            root.dispatchEvent(new CustomEvent('rhx:animated-image:play', {
                bubbles: true,
                detail: { element: root }
            }));
        }

        function toggle() {
            if (playing) {
                pause();
            } else {
                play();
            }
        }

        control.addEventListener('click', toggle);

        // If initially paused, capture frame once image loads
        if (!playing) {
            if (img.complete) {
                captureFrame();
            } else {
                img.addEventListener('load', function () {
                    captureFrame();
                }, { once: true });
            }
        }

        // Expose API
        root._rhxPlay = play;
        root._rhxPause = pause;
    }

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('animated-image', init);
    }

    function initAll() {
        document.querySelectorAll('[data-rhx-animated-image]').forEach(init);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAll);
    } else {
        initAll();
    }

    document.addEventListener('htmx:afterSettle', function (e) {
        var el = e.detail.elt;
        if (el && el.querySelectorAll) {
            el.querySelectorAll('[data-rhx-animated-image]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-animated-image]')) {
            init(el);
        }
    });
})();
