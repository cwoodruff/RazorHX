/* ══════════════════════════════════════════════
   rhx-command-palette.js — Keyboard shortcut, navigation, open/close
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    var ITEM_SELECTOR = '[role="option"]:not([aria-disabled="true"])';

    function init(root) {
        if (root._rhxCommandPaletteInit) return;
        root._rhxCommandPaletteInit = true;

        var backdrop = root.querySelector('.rhx-command-palette__backdrop');
        var input = root.querySelector('.rhx-command-palette__input');
        var results = root.querySelector('.rhx-command-palette__results');
        var emptyEl = root.querySelector('.rhx-command-palette__empty');
        var previousFocus = null;
        var focusedIdx = -1;

        if (!input || !results) return;

        function getItems() {
            return Array.from(results.querySelectorAll(ITEM_SELECTOR));
        }

        function open() {
            if (!root.hidden) return;
            previousFocus = document.activeElement;
            root.removeAttribute('hidden');
            input.setAttribute('aria-expanded', 'true');
            input.value = '';
            focusedIdx = -1;
            clearFocused();
            setTimeout(function () { input.focus(); }, 10);
        }

        function close() {
            if (root.hidden) return;
            root.setAttribute('hidden', 'hidden');
            input.setAttribute('aria-expanded', 'false');
            clearFocused();
            if (previousFocus && previousFocus.focus) {
                previousFocus.focus();
            }
        }

        function clearFocused() {
            var prev = results.querySelector('[data-rhx-focused]');
            if (prev) prev.removeAttribute('data-rhx-focused');
            input.removeAttribute('aria-activedescendant');
        }

        function setFocused(idx, items) {
            items = items || getItems();
            clearFocused();
            if (idx >= 0 && idx < items.length) {
                focusedIdx = idx;
                items[idx].setAttribute('data-rhx-focused', '');
                items[idx].scrollIntoView({ block: 'nearest' });
                if (items[idx].id) {
                    input.setAttribute('aria-activedescendant', items[idx].id);
                }
            }
        }

        function activateItem(item) {
            if (!item) return;
            var href = item.getAttribute('data-rhx-href');
            if (href) {
                close();
                window.location.href = href;
                return;
            }
            item.dispatchEvent(new CustomEvent('rhx:command:select', {
                bubbles: true,
                detail: { value: item.getAttribute('data-rhx-value') }
            }));
            close();
        }

        function updateEmptyState() {
            if (!emptyEl) return;
            var hasContent = results.querySelector(ITEM_SELECTOR) ||
                             results.textContent.trim().length > 0;
            // Only show empty if we have searched (input has value) and no items
            if (input.value.trim().length > 0 && !results.querySelector(ITEM_SELECTOR)) {
                emptyEl.removeAttribute('hidden');
            } else {
                emptyEl.setAttribute('hidden', '');
            }
        }

        // ── Keyboard: input ──
        input.addEventListener('keydown', function (e) {
            var items = getItems();

            if (e.key === 'ArrowDown') {
                e.preventDefault();
                var next = focusedIdx + 1;
                if (next < items.length) setFocused(next, items);
                else if (items.length) setFocused(0, items);
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                var prev = focusedIdx - 1;
                if (prev >= 0) setFocused(prev, items);
                else if (items.length) setFocused(items.length - 1, items);
            } else if (e.key === 'Enter') {
                if (focusedIdx >= 0 && focusedIdx < items.length) {
                    e.preventDefault();
                    activateItem(items[focusedIdx]);
                }
            } else if (e.key === 'Escape') {
                e.preventDefault();
                close();
            } else if (e.key === 'Home' && items.length) {
                e.preventDefault();
                setFocused(0, items);
            } else if (e.key === 'End' && items.length) {
                e.preventDefault();
                setFocused(items.length - 1, items);
            }
        });

        // ── Backdrop click ──
        if (backdrop) {
            backdrop.addEventListener('click', close);
        }

        // ── Item click ──
        results.addEventListener('click', function (e) {
            var item = e.target.closest(ITEM_SELECTOR);
            if (item && results.contains(item)) {
                activateItem(item);
            }
        });

        // ── Global shortcut ──
        var shortcut = root.getAttribute('data-rhx-shortcut') || 'mod+k';
        document.addEventListener('keydown', function (e) {
            var isMod = navigator.platform.indexOf('Mac') >= 0 ? e.metaKey : e.ctrlKey;
            if (shortcut === 'mod+k' && isMod && e.key === 'k') {
                e.preventDefault();
                if (root.hidden) open(); else close();
            }
        });

        // ── Update empty state after htmx swap ──
        results.addEventListener('htmx:afterSwap', function () {
            focusedIdx = -1;
            clearFocused();
            updateEmptyState();
            var items = getItems();
            if (items.length) setFocused(0, items);
        });
    }

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('command-palette', function (root) {
            if (root && root.querySelectorAll) {
                root.querySelectorAll('[data-rhx-command-palette]').forEach(init);
            }
            if (root && root.matches && root.matches('[data-rhx-command-palette]')) {
                init(root);
            }
        });
    }

    function initAll() {
        document.querySelectorAll('[data-rhx-command-palette]').forEach(init);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAll);
    } else {
        initAll();
    }

    document.addEventListener('htmx:afterSettle', function (e) {
        var el = e.detail.elt;
        if (el && el.querySelectorAll) {
            el.querySelectorAll('[data-rhx-command-palette]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-command-palette]')) {
            init(el);
        }
    });
})();
