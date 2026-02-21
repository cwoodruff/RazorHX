/**
 * RazorHX Animation Component
 * Applies CSS animations from data attributes, with htmx integration.
 */
(function () {
  "use strict";

  function initAnimations(root) {
    if (root && root.querySelectorAll) {
      root.querySelectorAll("[data-rhx-animation]").forEach(function (el) {
        if (el._rhxAnimInit) return;
        el._rhxAnimInit = true;
        applyAnimation(el);
      });
    }
    if (root && root.matches && root.matches("[data-rhx-animation]")) {
      if (!root._rhxAnimInit) {
        root._rhxAnimInit = true;
        applyAnimation(root);
      }
    }
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

    el.style.animation =
      "rhx-" + name + " " +
      duration + "ms " +
      easing + " " +
      delay + "ms " +
      iterations + " " +
      direction + " " +
      fill;

    if (paused) {
      el.style.animationPlayState = "paused";
    }

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

  // Register with RHX core
  if (typeof RHX !== "undefined" && RHX.register) {
    RHX.register("animation", initAnimations);
    window.RHX.playAnimation = playAnimation;
  }

  // Self-init fallback
  function initAll() {
    document.querySelectorAll("[data-rhx-animation]").forEach(function (el) {
      if (el._rhxAnimInit) return;
      el._rhxAnimInit = true;
      applyAnimation(el);
    });
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initAll);
  } else {
    initAll();
  }

  // Re-init on htmx content swap
  document.addEventListener("htmx:afterSettle", function (e) {
    var el = e.detail.elt;
    if (el && el.querySelectorAll) {
      el.querySelectorAll("[data-rhx-animation]").forEach(function (a) {
        if (a._rhxAnimInit) return;
        a._rhxAnimInit = true;
        applyAnimation(a);
      });
    }
    if (el && el.matches && el.matches("[data-rhx-animation]")) {
      if (!el._rhxAnimInit) {
        el._rhxAnimInit = true;
        applyAnimation(el);
      }
    }
  });
})();
