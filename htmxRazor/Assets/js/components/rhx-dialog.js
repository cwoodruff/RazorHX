/* ══════════════════════════════════════════════
   rhx-dialog.js — Native dialog behavior
   ══════════════════════════════════════════════ */
(function () {
    'use strict';

    function init(dialog) {
        if (dialog._rhxDialogInit) return;
        dialog._rhxDialogInit = true;

        // ── Close button ──
        var closeBtn = dialog.querySelector('.rhx-dialog__close');
        if (closeBtn) {
            closeBtn.addEventListener('click', function () {
                close(dialog);
            });
        }

        // ── Backdrop click (click on dialog element itself, not panel) ──
        dialog.addEventListener('click', function (e) {
            if (e.target === dialog) {
                close(dialog);
            }
        });

        // ── Native close event → sync state ──
        dialog.addEventListener('close', function () {
            dialog.dispatchEvent(new CustomEvent('rhx:dialog:close', {
                bubbles: true,
                detail: { dialog: dialog }
            }));
        });

        // Auto-open if 'open' attribute is present at init
        if (dialog.hasAttribute('open')) {
            // Remove native open and use showModal for proper modal behavior
            dialog.removeAttribute('open');
            dialog.showModal();
        }

        // Expose open/close on element
        dialog.rhxOpen = function () { open(dialog); };
        dialog.rhxClose = function () { close(dialog); };
    }

    function open(dialog) {
        if (!dialog || dialog.open) return;
        dialog.showModal();
        dialog.dispatchEvent(new CustomEvent('rhx:dialog:open', {
            bubbles: true,
            detail: { dialog: dialog }
        }));
    }

    function close(dialog) {
        if (!dialog || !dialog.open) return;
        dialog.close();
    }

    // ── Trigger buttons: data-rhx-dialog-open="dialog-id" ──
    document.addEventListener('click', function (e) {
        var trigger = e.target.closest('[data-rhx-dialog-open]');
        if (!trigger) return;

        var dialogId = trigger.getAttribute('data-rhx-dialog-open');
        var dialog = document.getElementById(dialogId);
        if (dialog) open(dialog);
    });

    // Close triggers: data-rhx-dialog-close
    document.addEventListener('click', function (e) {
        var trigger = e.target.closest('[data-rhx-dialog-close]');
        if (!trigger) return;

        var targetId = trigger.getAttribute('data-rhx-dialog-close');
        var dialog = targetId
            ? document.getElementById(targetId)
            : trigger.closest('dialog[data-rhx-dialog]');
        if (dialog) close(dialog);
    });

    // ── htmx integration: listen for rhx:dialog:open event from HX-Trigger header ──
    document.addEventListener('rhx:dialog:open', function (e) {
        var detail = e.detail;
        if (detail && detail.target) {
            var dialog = document.querySelector(detail.target);
            if (dialog) open(dialog);
        }
    });

    // ── Registration ──
    if (typeof RHX !== 'undefined' && RHX.register) {
        RHX.register('dialog', init);
    }

    // Auto-init
    function initAll() {
        document.querySelectorAll('[data-rhx-dialog]').forEach(init);
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
            el.querySelectorAll('[data-rhx-dialog]').forEach(init);
        }
        if (el && el.matches && el.matches('[data-rhx-dialog]')) {
            init(el);
        }
    });
})();
