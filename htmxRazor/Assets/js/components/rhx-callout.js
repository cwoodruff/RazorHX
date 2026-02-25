/**
 * htmxRazor Callout
 * Close button dismissal and auto-dismiss via data-rhx-duration.
 * Uses CSS opacity transition before removing the element.
 */
(function () {
  "use strict";

  function initCallouts(root) {
    var callouts = root.querySelectorAll("[data-rhx-callout]");
    callouts.forEach(function (callout) {
      if (callout._rhxCalloutInit) return;
      callout._rhxCalloutInit = true;

      var closeBtn = callout.querySelector(".rhx-callout__close");
      var duration = parseInt(callout.getAttribute("data-rhx-duration"), 10) || 0;

      function dismiss() {
        callout.classList.add("rhx-callout--closing");
        callout.addEventListener("transitionend", function handler() {
          callout.removeEventListener("transitionend", handler);
          callout.hidden = true;
          callout.classList.remove("rhx-callout--closing");
          callout.dispatchEvent(new CustomEvent("rhx:callout:close", { bubbles: true }));
        });

        // Fallback if transition doesn't fire (e.g. reduced motion)
        setTimeout(function () {
          if (!callout.hidden) {
            callout.hidden = true;
            callout.classList.remove("rhx-callout--closing");
            callout.dispatchEvent(new CustomEvent("rhx:callout:close", { bubbles: true }));
          }
        }, 500);
      }

      // Close button click
      if (closeBtn) {
        closeBtn.addEventListener("click", function () {
          dismiss();
        });
      }

      // Auto-dismiss
      if (duration > 0) {
        setTimeout(function () {
          if (!callout.hidden) {
            dismiss();
          }
        }, duration);
      }
    });
  }

  if (window.RHX) {
    window.RHX.register("callout", initCallouts);
  }
})();
