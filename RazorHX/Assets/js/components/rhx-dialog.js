/**
 * RazorHX Dialog
 * Focus trap, ESC to close, backdrop click dismiss.
 */
(function () {
  "use strict";

  var FOCUSABLE = 'a[href], button:not([disabled]), input:not([disabled]), textarea:not([disabled]), select:not([disabled]), [tabindex]:not([tabindex="-1"])';

  function initDialogs(root) {
    var dialogs = root.querySelectorAll("[data-rhx-dialog]");
    dialogs.forEach(function (dialog) {
      if (dialog._rhxDialogInit) return;
      dialog._rhxDialogInit = true;

      var backdrop = dialog.querySelector("[data-rhx-dialog-backdrop]");
      var closeBtns = dialog.querySelectorAll("[data-rhx-dialog-close]");

      function open() {
        dialog.hidden = false;
        dialog.setAttribute("aria-hidden", "false");
        document.body.style.overflow = "hidden";
        trapFocus(dialog);
      }

      function close() {
        dialog.hidden = true;
        dialog.setAttribute("aria-hidden", "true");
        document.body.style.overflow = "";
      }

      function trapFocus(container) {
        var focusable = container.querySelectorAll(FOCUSABLE);
        if (focusable.length > 0) {
          focusable[0].focus();
        }

        container.addEventListener("keydown", function (e) {
          if (e.key === "Escape") {
            e.preventDefault();
            close();
            return;
          }

          if (e.key !== "Tab") return;

          var focusableEls = Array.from(container.querySelectorAll(FOCUSABLE));
          var first = focusableEls[0];
          var last = focusableEls[focusableEls.length - 1];

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
      }

      if (backdrop) {
        backdrop.addEventListener("click", close);
      }

      closeBtns.forEach(function (btn) {
        btn.addEventListener("click", close);
      });

      // Expose open/close on the element
      dialog.rhxOpen = open;
      dialog.rhxClose = close;
    });

    // Wire up dialog triggers
    var triggers = root.querySelectorAll("[data-rhx-dialog-open]");
    triggers.forEach(function (trigger) {
      if (trigger._rhxDialogTriggerInit) return;
      trigger._rhxDialogTriggerInit = true;

      var targetId = trigger.getAttribute("data-rhx-dialog-open");
      trigger.addEventListener("click", function () {
        var target = document.getElementById(targetId);
        if (target && target.rhxOpen) target.rhxOpen();
      });
    });
  }

  if (window.RHX) {
    window.RHX.register("dialog", initDialogs);
  }
})();
