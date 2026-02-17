/**
 * RazorHX Input Components JavaScript
 * Handles: clear button, password toggle, textarea auto-resize, number input steppers.
 */
(function () {
  "use strict";

  // ── Clear Button ──

  function initClearButtons(root) {
    var inputs = root.querySelectorAll("[data-rhx-input]");
    inputs.forEach(function (wrapper) {
      var native = wrapper.querySelector(".rhx-input__native");
      var clearBtn = wrapper.querySelector(".rhx-input__clear");
      if (!native || !clearBtn) return;
      if (clearBtn._rhxBound) return;
      clearBtn._rhxBound = true;

      function updateClear() {
        clearBtn.hidden = !native.value;
      }

      native.addEventListener("input", updateClear);
      native.addEventListener("change", updateClear);
      updateClear();

      clearBtn.addEventListener("click", function () {
        native.value = "";
        native.focus();
        updateClear();
        native.dispatchEvent(new Event("input", { bubbles: true }));
        native.dispatchEvent(new Event("change", { bubbles: true }));
      });
    });
  }

  // ── Password Toggle ──

  function initPasswordToggles(root) {
    var toggles = root.querySelectorAll("[data-rhx-input-toggle]");
    toggles.forEach(function (btn) {
      if (btn._rhxBound) return;
      btn._rhxBound = true;

      btn.addEventListener("click", function () {
        var control = btn.closest(".rhx-input__control");
        if (!control) return;
        var native = control.querySelector(".rhx-input__native");
        if (!native) return;

        var isPassword = native.type === "password";
        native.type = isPassword ? "text" : "password";

        var showIcon = btn.querySelector(".rhx-input__toggle-show");
        var hideIcon = btn.querySelector(".rhx-input__toggle-hide");
        if (showIcon && hideIcon) {
          showIcon.hidden = isPassword;
          hideIcon.hidden = !isPassword;
        }

        btn.setAttribute(
          "aria-label",
          isPassword ? "Hide password" : "Show password"
        );
        native.focus();
      });
    });
  }

  // ── Textarea Auto-Resize ──

  function initAutoResize(root) {
    var textareas = root.querySelectorAll("[data-rhx-auto-resize]");
    textareas.forEach(function (textarea) {
      if (textarea._rhxBound) return;
      textarea._rhxBound = true;

      function resize() {
        textarea.style.height = "auto";
        textarea.style.height = textarea.scrollHeight + "px";
      }

      textarea.addEventListener("input", resize);
      // Initial resize
      resize();
    });
  }

  // ── Number Input Steppers ──

  function initNumberSteppers(root) {
    var wrappers = root.querySelectorAll("[data-rhx-number-input]");
    wrappers.forEach(function (wrapper) {
      var native = wrapper.querySelector(".rhx-number-input__native");
      var decBtn = wrapper.querySelector(".rhx-number-input__decrement");
      var incBtn = wrapper.querySelector(".rhx-number-input__increment");
      if (!native) return;

      function step(direction) {
        if (native.disabled || native.readOnly) return;

        var current = parseFloat(native.value) || 0;
        var stepVal = parseFloat(native.step) || 1;
        var min = native.min !== "" ? parseFloat(native.min) : -Infinity;
        var max = native.max !== "" ? parseFloat(native.max) : Infinity;

        var next = current + stepVal * direction;

        // Clamp to range
        next = Math.max(min, Math.min(max, next));

        // Handle floating point precision
        var decimals = (stepVal.toString().split(".")[1] || "").length;
        native.value = next.toFixed(decimals);

        native.focus();
        native.dispatchEvent(new Event("input", { bubbles: true }));
        native.dispatchEvent(new Event("change", { bubbles: true }));
      }

      if (decBtn && !decBtn._rhxBound) {
        decBtn._rhxBound = true;
        decBtn.addEventListener("click", function () {
          step(-1);
        });
      }

      if (incBtn && !incBtn._rhxBound) {
        incBtn._rhxBound = true;
        incBtn.addEventListener("click", function () {
          step(1);
        });
      }
    });
  }

  // ── Registration ──

  function initAll(root) {
    initClearButtons(root);
    initPasswordToggles(root);
    initAutoResize(root);
    initNumberSteppers(root);
  }

  if (window.RHX && RHX.register) {
    RHX.register("input", initAll);
  } else {
    if (document.readyState === "loading") {
      document.addEventListener("DOMContentLoaded", function () {
        initAll(document);
      });
    } else {
      initAll(document);
    }
    document.addEventListener("htmx:afterSwap", function (e) {
      initAll(e.detail.target);
    });
  }
})();
