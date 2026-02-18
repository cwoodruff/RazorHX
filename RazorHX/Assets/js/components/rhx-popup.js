/**
 * RazorHX Popup Component
 * Low-level positioning utility using the shared rhx-position.js engine.
 */
(function () {
  "use strict";

  function initPopups(root) {
    var popups = root.querySelectorAll("[data-rhx-popup]");
    popups.forEach(function (popup) {
      if (popup._rhxPopupInit) return;
      popup._rhxPopupInit = true;

      var anchorSel = popup.getAttribute("data-rhx-anchor");
      if (!anchorSel) return;

      var anchor = document.querySelector(anchorSel);
      if (!anchor) return;

      var arrowEl = popup.querySelector("[data-rhx-popup-arrow]");

      function reposition() {
        if (popup.hidden) return;
        if (!window.RHX || !window.RHX.positionElement) return;

        window.RHX.positionElement(anchor, popup, {
          placement: popup.getAttribute("data-rhx-placement") || "bottom-start",
          distance: parseInt(popup.getAttribute("data-rhx-distance") || "4", 10),
          skidding: parseInt(popup.getAttribute("data-rhx-skidding") || "0", 10),
          strategy: popup.getAttribute("data-rhx-strategy") || "absolute",
          flip: !popup.hasAttribute("data-rhx-no-flip"),
          shift: !popup.hasAttribute("data-rhx-no-shift"),
          arrowElement: arrowEl,
          arrowPadding: parseInt(popup.getAttribute("data-rhx-arrow-padding") || "8", 10)
        });
      }

      // Position if already active
      if (popup.classList.contains("rhx-popup--active")) {
        reposition();
      }

      // Observe attribute changes to reposition on active toggle
      var observer = new MutationObserver(function () {
        if (popup.classList.contains("rhx-popup--active") && !popup.hidden) {
          reposition();
        }
      });
      observer.observe(popup, { attributes: true, attributeFilter: ["class", "hidden"] });

      // Reposition on scroll/resize
      window.addEventListener("scroll", reposition, { passive: true });
      window.addEventListener("resize", reposition, { passive: true });

      // Expose reposition for programmatic use
      popup._rhxReposition = reposition;
    });
  }

  if (window.RHX) {
    window.RHX.register("popup", initPopups);
  }
})();
