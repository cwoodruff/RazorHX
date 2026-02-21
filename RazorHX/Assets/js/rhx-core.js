/**
 * RazorHX Core JavaScript
 * Minimal JS for shared component behaviors.
 */
(function () {
  "use strict";

  const RHX = window.RHX || {};

  /**
   * Initialize all RazorHX components on the page.
   * Called automatically on DOMContentLoaded and after htmx swaps.
   */
  RHX.init = function (root) {
    root = root || document;
    // Component-specific init hooks will register themselves here
    RHX._initHooks.forEach(function (hook) {
      hook(root);
    });
  };

  RHX._initHooks = [];

  /**
   * Register an initialization hook for a component.
   */
  RHX.register = function (name, initFn) {
    RHX._initHooks.push(initFn);
  };

  /**
   * Theme toggling utility.
   */
  RHX.setTheme = function (theme) {
    document.documentElement.setAttribute("data-rhx-theme", theme);
    try {
      localStorage.setItem("rhx-theme", theme);
    } catch (e) {
      // localStorage unavailable
    }
  };

  RHX.getTheme = function () {
    return document.documentElement.getAttribute("data-rhx-theme") || "light";
  };

  RHX.toggleTheme = function () {
    RHX.setTheme(RHX.getTheme() === "light" ? "dark" : "light");
  };

  // Auto-apply saved theme preference
  (function () {
    try {
      var saved = localStorage.getItem("rhx-theme");
      if (saved) {
        document.documentElement.setAttribute("data-rhx-theme", saved);
      }
    } catch (e) {
      // localStorage unavailable
    }
  })();

  // Initialize on DOM ready — must wait for DOMContentLoaded so that all
  // defer component scripts have registered their hooks via RHX.register()
  if (document.readyState === "complete") {
    RHX.init();
  } else {
    document.addEventListener("DOMContentLoaded", function () {
      RHX.init();
    });
  }

  // Re-initialize after htmx content swaps
  document.addEventListener("htmx:afterSwap", function (event) {
    RHX.init(event.detail.target);
  });

  window.RHX = RHX;
})();
