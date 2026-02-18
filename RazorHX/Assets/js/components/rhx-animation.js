/**
 * RazorHX Animation Component
 * Applies CSS animations from data attributes, with htmx integration.
 */
(function () {
  "use strict";

  function initAnimations(root) {
    var els = root.querySelectorAll("[data-rhx-animation]");
    els.forEach(function (el) {
      if (el._rhxAnimInit) return;
      el._rhxAnimInit = true;
      applyAnimation(el);
    });
  }

  function applyAnimation(el) {
    var name = el.getAttribute("data-rhx-animation");
    var duration = parseInt(el.getAttribute("data-rhx-duration") || "300", 10);
    var delay = parseInt(el.getAttribute("data-rhx-delay") || "0", 10);
    var direction = el.getAttribute("data-rhx-direction") || "normal";
    var easing = el.getAttribute("data-rhx-easing") || "ease";
    var iterations = el.getAttribute("data-rhx-iterations") || "1";
    var fill = el.getAttribute("data-rhx-fill") || "both";
    var paused = el.hasAttribute("data-rhx-paused");

    if (paused) return;

    el.style.animation =
      "rhx-" + name + " " +
      duration + "ms " +
      easing + " " +
      delay + "ms " +
      iterations + " " +
      direction + " " +
      fill;

    el.addEventListener("animationend", function handler() {
      el.removeEventListener("animationend", handler);
      el.dispatchEvent(new CustomEvent("rhx:animation:end", {
        bubbles: true,
        detail: { name: name }
      }));
    });
  }

  /**
   * Play animation on a specific element (for programmatic use).
   */
  function playAnimation(el) {
    // Reset animation
    el.style.animation = "none";
    // Force reflow
    void el.offsetHeight;
    el.removeAttribute("data-rhx-paused");
    el.classList.add("rhx-animation--playing");
    el._rhxAnimInit = false;
    applyAnimation(el);
  }

  // Expose for programmatic use
  if (window.RHX) {
    window.RHX.playAnimation = playAnimation;
    window.RHX.register("animation", initAnimations);
  }
})();
