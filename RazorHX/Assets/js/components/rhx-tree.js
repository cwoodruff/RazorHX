/* ══════════════════════════════════════════════
   rhx-tree.js — Tree view behavior
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(root) {
        if (root._rhxTreeInit) return;
        root._rhxTreeInit = true;

        var selection = root.getAttribute('data-rhx-selection') || 'single';

        // ── Helpers ──

        function getVisibleItems() {
            var all = root.querySelectorAll('[role="treeitem"]');
            var result = [];
            all.forEach(function (item) {
                var parent = item.parentElement;
                var hidden = false;
                while (parent && parent !== root) {
                    if (parent.getAttribute('role') === 'group' && parent.hasAttribute('hidden')) {
                        hidden = true;
                        break;
                    }
                    parent = parent.parentElement;
                }
                if (!hidden) result.push(item);
            });
            return result;
        }

        function getChildren(item) {
            var group = item.querySelector(':scope > .rhx-tree__children');
            if (!group) return [];
            return Array.from(group.querySelectorAll(':scope > [role="treeitem"]'));
        }

        function getParentItem(item) {
            var group = item.parentElement;
            if (group && group.getAttribute('role') === 'group') {
                return group.closest('[role="treeitem"]');
            }
            return null;
        }

        function isExpandable(item) {
            return item.hasAttribute('aria-expanded');
        }

        function isExpanded(item) {
            return item.getAttribute('aria-expanded') === 'true';
        }

        function isDisabled(item) {
            return item.getAttribute('aria-disabled') === 'true';
        }

        function isLeaf(item) {
            return !isExpandable(item);
        }

        // ── Expand / Collapse ──

        function expand(item) {
            if (!isExpandable(item) || isExpanded(item) || isDisabled(item)) return;

            item.setAttribute('aria-expanded', 'true');
            item.classList.add('rhx-tree__item--expanded');

            var group = item.querySelector(':scope > .rhx-tree__children');
            if (group) group.removeAttribute('hidden');

            root.dispatchEvent(new CustomEvent('rhx:tree:expand', {
                bubbles: true,
                detail: { item: item }
            }));

            // Lazy loading: dispatch toggle event for htmx to intercept
            if (item.hasAttribute('data-rhx-tree-lazy')) {
                item.removeAttribute('data-rhx-tree-lazy');
                item.classList.remove('rhx-tree__item--lazy');
                item.dispatchEvent(new CustomEvent('toggle', { bubbles: true }));
            }
        }

        function collapse(item) {
            if (!isExpandable(item) || !isExpanded(item) || isDisabled(item)) return;

            item.setAttribute('aria-expanded', 'false');
            item.classList.remove('rhx-tree__item--expanded');

            var group = item.querySelector(':scope > .rhx-tree__children');
            if (group) group.setAttribute('hidden', '');

            root.dispatchEvent(new CustomEvent('rhx:tree:collapse', {
                bubbles: true,
                detail: { item: item }
            }));
        }

        function toggleExpand(item) {
            if (isExpanded(item)) collapse(item);
            else expand(item);
        }

        // ── Selection ──

        function selectItem(item) {
            if (isDisabled(item)) return;
            if (selection === 'leaf' && !isLeaf(item)) return;

            if (selection === 'single' || selection === 'leaf') {
                // Deselect all
                root.querySelectorAll('[role="treeitem"][aria-selected="true"]').forEach(function (i) {
                    i.setAttribute('aria-selected', 'false');
                    i.classList.remove('rhx-tree__item--selected');
                });
                item.setAttribute('aria-selected', 'true');
                item.classList.add('rhx-tree__item--selected');
            } else if (selection === 'multiple') {
                // Toggle
                var isSelected = item.getAttribute('aria-selected') === 'true';
                item.setAttribute('aria-selected', String(!isSelected));
                item.classList.toggle('rhx-tree__item--selected');
            }

            root.dispatchEvent(new CustomEvent('rhx:tree:select', {
                bubbles: true,
                detail: { item: item }
            }));
        }

        // ── Focus management ──

        function focusItem(item) {
            root.querySelectorAll('[role="treeitem"]').forEach(function (i) {
                i.setAttribute('tabindex', '-1');
            });
            item.setAttribute('tabindex', '0');
            item.focus();
        }

        // ── Click handler ──

        root.addEventListener('click', function (e) {
            var content = e.target.closest('.rhx-tree__item-content');
            if (!content) return;

            var item = content.closest('[role="treeitem"]');
            if (!item || isDisabled(item)) return;

            var expandIcon = e.target.closest('.rhx-tree__expand-icon');

            if (expandIcon) {
                // Click on expand icon: toggle expand only
                toggleExpand(item);
            } else {
                // Click on content: select and toggle expand
                selectItem(item);
                if (isExpandable(item)) toggleExpand(item);
            }

            focusItem(item);
        });

        // ── Keyboard handler ──

        root.addEventListener('keydown', function (e) {
            var item = e.target.closest('[role="treeitem"]');
            if (!item) return;

            var visible = getVisibleItems();
            var idx = visible.indexOf(item);

            switch (e.key) {
                case 'ArrowDown':
                    e.preventDefault();
                    if (idx < visible.length - 1) {
                        focusItem(visible[idx + 1]);
                    }
                    break;

                case 'ArrowUp':
                    e.preventDefault();
                    if (idx > 0) {
                        focusItem(visible[idx - 1]);
                    }
                    break;

                case 'ArrowRight':
                    e.preventDefault();
                    if (isExpandable(item) && !isExpanded(item)) {
                        expand(item);
                    } else if (isExpanded(item)) {
                        var children = getChildren(item);
                        if (children.length > 0) {
                            focusItem(children[0]);
                        }
                    }
                    break;

                case 'ArrowLeft':
                    e.preventDefault();
                    if (isExpandable(item) && isExpanded(item)) {
                        collapse(item);
                    } else {
                        var parent = getParentItem(item);
                        if (parent) focusItem(parent);
                    }
                    break;

                case 'Home':
                    e.preventDefault();
                    if (visible.length > 0) focusItem(visible[0]);
                    break;

                case 'End':
                    e.preventDefault();
                    if (visible.length > 0) focusItem(visible[visible.length - 1]);
                    break;

                case 'Enter':
                case ' ':
                    e.preventDefault();
                    selectItem(item);
                    if (isExpandable(item)) toggleExpand(item);
                    break;
            }
        });

        // Set initial tabindex: first item gets tabindex 0
        var firstItem = root.querySelector('[role="treeitem"]');
        if (firstItem) {
            firstItem.setAttribute('tabindex', '0');
        }
    }

    // ── Registration ──

    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('tree', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-tree]').forEach(init);
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
            el.querySelectorAll('[data-rhx-tree]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-tree]')) {
            init(el);
        }
    });
})();
