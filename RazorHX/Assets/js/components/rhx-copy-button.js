/**
 * RazorHX Copy Button
 * Clipboard copy with visual success feedback.
 */
(function () {
  "use strict";

  function initCopyButtons(root) {
    var buttons = root.querySelectorAll("[data-rhx-copy-button]");
    buttons.forEach(function (btn) {
      if (btn._rhxCopyInit) return;
      btn._rhxCopyInit = true;

      btn.addEventListener("click", function () {
        if (btn.disabled) return;

        var text = "";
        var value = btn.getAttribute("data-rhx-copy-value");
        var from = btn.getAttribute("data-rhx-copy-from");

        if (value) {
          text = value;
        } else if (from) {
          var el = document.querySelector(from);
          if (el) text = el.textContent || "";
        }

        if (!text) return;

        navigator.clipboard.writeText(text).then(function () {
          var duration =
            parseInt(btn.getAttribute("data-rhx-copy-duration"), 10) || 2000;
          var successLabel =
            btn.getAttribute("data-rhx-copy-success-label") || "Copied!";
          var originalLabel = btn.getAttribute("aria-label");

          btn.classList.add("rhx-copy-button--success");
          btn.setAttribute("aria-label", successLabel);

          btn.dispatchEvent(
            new CustomEvent("rhx:copy", {
              bubbles: true,
              detail: { text: text },
            })
          );

          setTimeout(function () {
            btn.classList.remove("rhx-copy-button--success");
            if (originalLabel) btn.setAttribute("aria-label", originalLabel);
          }, duration);
        });
      });
    });
  }

  if (window.RHX) {
    window.RHX.register("copy-button", initCopyButtons);
  }
})();
