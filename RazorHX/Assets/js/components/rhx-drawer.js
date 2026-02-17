/* ══════════════════════════════════════════════
   rhx-drawer.js — Slide-out drawer panel
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxDrawerInit) return;
        root._rhxDrawerInit = true;

        var overlay = root.querySelector('.rhx-drawer__overlay');
        var panel = root.querySelector('.rhx-drawer__panel');
        var closeBtn = root.querySelector('.rhx-drawer__close');
        var previousFocus = null;

        function isOpen() {
            return root.classList.contains('rhx-drawer--open');
        }

        function open() {
            if (isOpen()) return;

            previousFocus = document.activeElement;
            root.classList.add('rhx-drawer--open');
            root.setAttribute('aria-hidden', 'false');

            // Focus first focusable element in panel
            var focusable = panel.querySelector(
                'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
            );
            if (focusable) {
                setTimeout(function () { focusable.focus(); }, 50);
            }

            root.dispatchEvent(new CustomEvent('rhx:drawer:open', {
                bubbles: true,
                detail: { drawer: root }
            }));
        }

        function close() {
            if (!isOpen()) return;

            root.classList.remove('rhx-drawer--open');
            root.setAttribute('aria-hidden', 'true');

            if (previousFocus && previousFocus.focus) {
                previousFocus.focus();
            }

            root.dispatchEvent(new CustomEvent('rhx:drawer:close', {
                bubbles: true,
                detail: { drawer: root }
            }));
        }

        // ── Close button ──
        if (closeBtn) {
            closeBtn.addEventListener('click', close);
        }

        // ── Overlay click ──
        if (overlay) {
            overlay.addEventListener('click', close);
        }

        // ── ESC key ──
        root.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && isOpen()) {
                e.preventDefault();
                close();
            }
        });

        // ── Focus trap ──
        panel.addEventListener('keydown', function (e) {
            if (e.key !== 'Tab' || !isOpen()) return;

            var focusable = panel.querySelectorAll(
                'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
            );
            if (focusable.length === 0) return;

            var first = focusable[0];
            var last = focusable[focusable.length - 1];

            if (e.shiftKey) {
                if (document.activeElement === first) {
                    e.preventDefault();
                    last.focus();
                }
            } else {
                if (document.activeElement === last) {
                    e.preventDefault();
                    first.focus();
                }
            }
        });

        // Open if rhx-open was set server-side
        if (root.classList.contains('rhx-drawer--open')) {
            root.setAttribute('aria-hidden', 'false');
        }

        // Expose open/close on element
        root._rhxDrawerOpen = open;
        root._rhxDrawerClose = close;
    }

    // ── Trigger buttons: data-rhx-drawer-open="drawer-id" ──
    document.addEventListener('click', function (e) {
        var trigger = e.target.closest('[data-rhx-drawer-open]');
        if (!trigger) return;

        var drawerId = trigger.getAttribute('data-rhx-drawer-open');
        var drawer = document.getElementById(drawerId);
        if (drawer && drawer._rhxDrawerOpen) drawer._rhxDrawerOpen();
    });

    // Close triggers
    document.addEventListener('click', function (e) {
        var trigger = e.target.closest('[data-rhx-drawer-close]');
        if (!trigger) return;

        var targetId = trigger.getAttribute('data-rhx-drawer-close');
        var drawer = targetId
            ? document.getElementById(targetId)
            : trigger.closest('[data-rhx-drawer]');
        if (drawer && drawer._rhxDrawerClose) drawer._rhxDrawerClose();
    });

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('drawer', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-drawer]').forEach(init);
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
            el.querySelectorAll('[data-rhx-drawer]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-drawer]')) {
            init(el);
        }
    });
})();
