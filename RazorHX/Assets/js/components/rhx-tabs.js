/* ══════════════════════════════════════════════
   rhx-tabs.js — Tab navigation behavior
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxTabsInit) return;
        root._rhxTabsInit = true;

        var tablist = root.querySelector('[role="tablist"]');
        if (!tablist) return;

        var placement = root.getAttribute('data-rhx-placement') || 'top';
        var activation = root.getAttribute('data-rhx-activation') || 'auto';
        var isVertical = placement === 'start' || placement === 'end';

        // ── Helpers ──

        function getTabs() {
            return Array.from(tablist.querySelectorAll('[role="tab"]'));
        }

        function getEnabledTabs() {
            return getTabs().filter(function (t) {
                return !t.disabled && t.getAttribute('aria-disabled') !== 'true';
            });
        }

        function getPanels() {
            var body = root.querySelector('.rhx-tab-group__body');
            return body ? Array.from(body.querySelectorAll(':scope > [role="tabpanel"]')) : [];
        }

        // ── Activation ──

        function activateTab(tab, focus) {
            var tabs = getTabs();
            var panelId = tab.getAttribute('aria-controls');
            var panel = panelId ? root.querySelector('#' + CSS.escape(panelId)) : null;

            // Deactivate all
            tabs.forEach(function (t) {
                t.setAttribute('aria-selected', 'false');
                t.setAttribute('tabindex', '-1');
                t.classList.remove('rhx-tab--active');
            });

            getPanels().forEach(function (p) {
                p.setAttribute('hidden', 'hidden');
                p.classList.remove('rhx-tab-panel--active');
            });

            // Activate selected
            tab.setAttribute('aria-selected', 'true');
            tab.setAttribute('tabindex', '0');
            tab.classList.add('rhx-tab--active');

            if (panel) {
                panel.removeAttribute('hidden');
                panel.classList.add('rhx-tab-panel--active');
            }

            if (focus !== false) {
                tab.focus();
            }

            // Dispatch custom event
            root.dispatchEvent(new CustomEvent('rhx:tab:change', {
                bubbles: true,
                detail: { tab: tab, panel: panel }
            }));
        }

        function focusTab(tab) {
            if (activation === 'auto') {
                activateTab(tab);
            } else {
                // Manual: move focus without activating
                var tabs = getTabs();
                tabs.forEach(function (t) {
                    t.setAttribute('tabindex', '-1');
                });
                tab.setAttribute('tabindex', '0');
                tab.focus();
            }
        }

        // ── Click handler ──

        tablist.addEventListener('click', function (e) {
            var tab = e.target.closest('[role="tab"]');
            if (!tab || tab.disabled || tab.getAttribute('aria-disabled') === 'true') return;

            // Close button click
            var closeBtn = e.target.closest('.rhx-tab__close');
            if (closeBtn) {
                e.stopPropagation();
                var panelId = tab.getAttribute('aria-controls');
                var panel = panelId ? root.querySelector('#' + CSS.escape(panelId)) : null;
                var wasActive = tab.getAttribute('aria-selected') === 'true';

                // Dispatch cancelable event
                var closeEvent = new CustomEvent('rhx:tab:close', {
                    bubbles: true,
                    cancelable: true,
                    detail: { tab: tab, panel: panel }
                });
                var allowed = root.dispatchEvent(closeEvent);
                if (!allowed) return;

                // Remove tab and panel
                if (wasActive) {
                    var allTabs = getTabs();
                    var idx = allTabs.indexOf(tab);
                    tab.remove();
                    if (panel) panel.remove();

                    // Activate adjacent tab
                    var remaining = getEnabledTabs();
                    if (remaining.length > 0) {
                        var nextIdx = Math.min(idx, remaining.length - 1);
                        activateTab(remaining[nextIdx]);
                    }
                } else {
                    tab.remove();
                    if (panel) panel.remove();
                }
                return;
            }

            activateTab(tab);
        });

        // ── Keyboard handler ──

        tablist.addEventListener('keydown', function (e) {
            var tab = e.target.closest('[role="tab"]');
            if (!tab) return;

            var enabled = getEnabledTabs();
            var idx = enabled.indexOf(tab);
            if (idx === -1) return;

            var prevKey = isVertical ? 'ArrowUp' : 'ArrowLeft';
            var nextKey = isVertical ? 'ArrowDown' : 'ArrowRight';

            if (e.key === nextKey) {
                e.preventDefault();
                focusTab(enabled[(idx + 1) % enabled.length]);
            } else if (e.key === prevKey) {
                e.preventDefault();
                focusTab(enabled[(idx - 1 + enabled.length) % enabled.length]);
            } else if (e.key === 'Home') {
                e.preventDefault();
                focusTab(enabled[0]);
            } else if (e.key === 'End') {
                e.preventDefault();
                focusTab(enabled[enabled.length - 1]);
            } else if ((e.key === 'Enter' || e.key === ' ') && activation === 'manual') {
                e.preventDefault();
                activateTab(tab);
            }
        });
    }

    // ── Registration ──

    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('tabs', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-tabs]').forEach(init);
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
            el.querySelectorAll('[data-rhx-tabs]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-tabs]')) {
            init(el);
        }
    });
})();
