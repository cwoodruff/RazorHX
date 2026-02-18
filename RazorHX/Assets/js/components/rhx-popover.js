/**
 * RazorHX Popover Component
 * Content-rich popover with click/hover/focus triggers.
 * Uses the shared rhx-position.js engine for positioning.
 */
(function () {
  "use strict";

  function initPopovers(root) {
    var popovers = root.querySelectorAll("[data-rhx-popover]");
    popovers.forEach(function (popover) {
      if (popover._rhxPopoverInit) return;
      popover._rhxPopoverInit = true;

      var triggerSel = popover.getAttribute("data-rhx-trigger");
      if (!triggerSel) return;

      var trigger;
      if (triggerSel === "previous") {
        trigger = popover.previousElementSibling;
      } else {
        trigger = document.querySelector(triggerSel);
      }
      if (!trigger) return;

      var arrowEl = popover.querySelector(".rhx-popover__arrow");
      var triggerEvent = popover.getAttribute("data-rhx-trigger-event") || "click";
      var hideTimer = null;
      var HOVER_DELAY = 100;

      function show() {
        clearTimeout(hideTimer);
        popover.hidden = false;
        popover.style.display = "block";
        popover.setAttribute("data-rhx-visible", "");
        popover.removeAttribute("aria-hidden");
        popover.classList.add("rhx-popover--open");

        reposition();

        trigger.setAttribute("aria-expanded", "true");

        popover.dispatchEvent(new CustomEvent("rhx:popover:show", {
          bubbles: true
        }));
      }

      function hide() {
        popover.removeAttribute("data-rhx-visible");
        popover.setAttribute("aria-hidden", "true");
        popover.classList.remove("rhx-popover--open");

        trigger.setAttribute("aria-expanded", "false");

        // Wait for transition
        setTimeout(function () {
          if (!popover.hasAttribute("data-rhx-visible")) {
            popover.hidden = true;
            popover.style.display = "";
          }
        }, 200);

        popover.dispatchEvent(new CustomEvent("rhx:popover:hide", {
          bubbles: true
        }));
      }

      function toggle() {
        if (popover.hasAttribute("data-rhx-visible")) {
          hide();
        } else {
          show();
        }
      }

      function isOpen() {
        return popover.hasAttribute("data-rhx-visible");
      }

      function reposition() {
        if (!window.RHX || !window.RHX.positionElement) return;

        window.RHX.positionElement(trigger, popover, {
          placement: popover.getAttribute("data-rhx-placement") || "bottom",
          distance: parseInt(popover.getAttribute("data-rhx-distance") || "8", 10),
          strategy: "absolute",
          flip: true,
          shift: true,
          arrowElement: arrowEl,
          arrowPadding: 8
        });
      }

      // Set up ARIA on trigger
      trigger.setAttribute("aria-haspopup", "dialog");
      trigger.setAttribute("aria-expanded", isOpen() ? "true" : "false");
      if (popover.id) {
        trigger.setAttribute("aria-controls", popover.id);
      }

      // ── Click trigger ──
      if (triggerEvent === "click") {
        trigger.addEventListener("click", function (e) {
          e.preventDefault();
          e.stopPropagation();
          toggle();
        });

        // Click outside to close
        document.addEventListener("click", function (e) {
          if (isOpen() && !popover.contains(e.target) && !trigger.contains(e.target)) {
            hide();
          }
        });

        // Escape to close
        document.addEventListener("keydown", function (e) {
          if (e.key === "Escape" && isOpen()) {
            hide();
            trigger.focus();
          }
        });
      }

      // ── Hover trigger ──
      if (triggerEvent === "hover") {
        trigger.addEventListener("mouseenter", function () { show(); });
        trigger.addEventListener("mouseleave", function () {
          hideTimer = setTimeout(function () {
            if (!popover.matches(":hover")) hide();
          }, HOVER_DELAY);
        });

        popover.addEventListener("mouseenter", function () {
          clearTimeout(hideTimer);
        });
        popover.addEventListener("mouseleave", function () {
          hideTimer = setTimeout(hide, HOVER_DELAY);
        });
      }

      // ── Focus trigger ──
      if (triggerEvent === "focus") {
        trigger.addEventListener("focusin", function () { show(); });
        trigger.addEventListener("focusout", function (e) {
          if (!popover.contains(e.relatedTarget)) {
            hide();
          }
        });
      }

      // Reposition on scroll/resize
      window.addEventListener("scroll", function () { if (isOpen()) reposition(); }, { passive: true });
      window.addEventListener("resize", function () { if (isOpen()) reposition(); }, { passive: true });

      // Handle server-rendered open state
      if (popover.classList.contains("rhx-popover--open")) {
        popover.hidden = false;
        popover.style.display = "block";
        popover.setAttribute("data-rhx-visible", "");
        popover.removeAttribute("aria-hidden");
        reposition();
      }
    });
  }

  if (window.RHX) {
    window.RHX.register("popover", initPopovers);
  }
})();
