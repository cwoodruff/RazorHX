/**
 * htmxRazor Slider
 * Updates fill bar width, tooltip text, and tooltip position on input.
 */
(function () {
  "use strict";

  function initSliders(root) {
    var sliders = root.querySelectorAll("[data-rhx-slider]");
    sliders.forEach(function (slider) {
      if (slider._rhxSliderInit) return;
      slider._rhxSliderInit = true;

      var native = slider.querySelector(".rhx-slider__native");
      var fill = slider.querySelector(".rhx-slider__fill");
      var tooltip = slider.querySelector(".rhx-slider__tooltip");

      if (!native) return;

      function updateFill() {
        var min = parseFloat(native.min) || 0;
        var max = parseFloat(native.max) || 100;
        var val = parseFloat(native.value) || 0;
        var percent = ((val - min) / (max - min)) * 100;
        percent = Math.max(0, Math.min(100, percent));

        if (fill) {
          fill.style.width = percent + "%";
        }

        if (tooltip) {
          tooltip.textContent = native.value;
          // Position tooltip above thumb
          tooltip.style.left = percent + "%";
        }
      }

      native.addEventListener("input", function () {
        updateFill();
        slider.dispatchEvent(new CustomEvent("rhx:slider:change", {
          bubbles: true,
          detail: { value: native.value }
        }));
      });

      // Initial update
      updateFill();
    });
  }

  // ── Registration ──
  if (typeof RHX !== 'undefined' && RHX.register) {
    RHX.register('slider', initSliders);
  }

  // Auto-init
  function initAll() {
    initSliders(document);
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
      initSliders(el);
    }
  });
})();
