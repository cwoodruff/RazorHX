/**
 * RazorHX Color Picker
 * Full HSV-based color picker with saturation area, hue slider,
 * optional opacity slider, text input, and preset swatches.
 * All color math handled client-side.
 */
(function () {
  "use strict";

  // ── Color conversion utilities ──

  function hsvToRgb(h, s, v) {
    h = h / 360;
    var i = Math.floor(h * 6);
    var f = h * 6 - i;
    var p = v * (1 - s);
    var q = v * (1 - f * s);
    var t = v * (1 - (1 - f) * s);
    var r, g, b;
    switch (i % 6) {
      case 0: r = v; g = t; b = p; break;
      case 1: r = q; g = v; b = p; break;
      case 2: r = p; g = v; b = t; break;
      case 3: r = p; g = q; b = v; break;
      case 4: r = t; g = p; b = v; break;
      case 5: r = v; g = p; b = q; break;
    }
    return {
      r: Math.round(r * 255),
      g: Math.round(g * 255),
      b: Math.round(b * 255)
    };
  }

  function rgbToHsv(r, g, b) {
    r /= 255; g /= 255; b /= 255;
    var max = Math.max(r, g, b), min = Math.min(r, g, b);
    var h, s, v = max;
    var d = max - min;
    s = max === 0 ? 0 : d / max;
    if (max === min) {
      h = 0;
    } else {
      switch (max) {
        case r: h = (g - b) / d + (g < b ? 6 : 0); break;
        case g: h = (b - r) / d + 2; break;
        case b: h = (r - g) / d + 4; break;
      }
      h /= 6;
    }
    return { h: h * 360, s: s, v: v };
  }

  function hexToRgb(hex) {
    hex = hex.replace(/^#/, "");
    if (hex.length === 3) {
      hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    var num = parseInt(hex, 16);
    return {
      r: (num >> 16) & 255,
      g: (num >> 8) & 255,
      b: num & 255
    };
  }

  function rgbToHex(r, g, b) {
    return "#" + ((1 << 24) + (r << 16) + (g << 8) + b)
      .toString(16).slice(1);
  }

  function rgbToHsl(r, g, b) {
    r /= 255; g /= 255; b /= 255;
    var max = Math.max(r, g, b), min = Math.min(r, g, b);
    var h, s, l = (max + min) / 2;
    if (max === min) {
      h = s = 0;
    } else {
      var d = max - min;
      s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
      switch (max) {
        case r: h = (g - b) / d + (g < b ? 6 : 0); break;
        case g: h = (b - r) / d + 2; break;
        case b: h = (r - g) / d + 4; break;
      }
      h /= 6;
    }
    return {
      h: Math.round(h * 360),
      s: Math.round(s * 100),
      l: Math.round(l * 100)
    };
  }

  function formatColor(rgb, format, alpha) {
    if (format === "rgb") {
      if (alpha < 1) return "rgba(" + rgb.r + ", " + rgb.g + ", " + rgb.b + ", " + alpha.toFixed(2) + ")";
      return "rgb(" + rgb.r + ", " + rgb.g + ", " + rgb.b + ")";
    }
    if (format === "hsl") {
      var hsl = rgbToHsl(rgb.r, rgb.g, rgb.b);
      if (alpha < 1) return "hsla(" + hsl.h + ", " + hsl.s + "%, " + hsl.l + "%, " + alpha.toFixed(2) + ")";
      return "hsl(" + hsl.h + ", " + hsl.s + "%, " + hsl.l + "%)";
    }
    // hex
    return rgbToHex(rgb.r, rgb.g, rgb.b);
  }

  function parseColorValue(val) {
    // Try hex
    if (/^#?[0-9a-f]{3,6}$/i.test(val)) {
      return hexToRgb(val);
    }
    // Try rgb/rgba
    var m = val.match(/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)/);
    if (m) return { r: parseInt(m[1]), g: parseInt(m[2]), b: parseInt(m[3]) };
    // Fallback
    return { r: 0, g: 0, b: 0 };
  }

  // ── Initialization ──

  function initColorPickers(root) {
    var pickers = root.querySelectorAll("[data-rhx-color-picker]");
    pickers.forEach(function (picker) {
      if (picker._rhxColorPickerInit) return;
      picker._rhxColorPickerInit = true;

      var format = picker.getAttribute("data-rhx-format") || "hex";
      var hasOpacity = picker.hasAttribute("data-rhx-opacity");
      var isInline = picker.hasAttribute("data-rhx-inline");

      var trigger = picker.querySelector(".rhx-color-picker__trigger");
      var panel = picker.querySelector(".rhx-color-picker__panel");
      var satArea = picker.querySelector(".rhx-color-picker__saturation");
      var satCursor = picker.querySelector(".rhx-color-picker__saturation-cursor");
      var hueInput = picker.querySelector(".rhx-color-picker__hue-input");
      var opacityInput = picker.querySelector(".rhx-color-picker__opacity-input");
      var textInput = picker.querySelector(".rhx-color-picker__input");
      var swatch = picker.querySelector(".rhx-color-picker__swatch");
      var textDisplay = picker.querySelector(".rhx-color-picker__text");
      var hidden = picker.querySelector(".rhx-color-picker__value");
      var presets = picker.querySelectorAll(".rhx-color-picker__preset");

      if (!hidden) return;

      // State
      var hsv = { h: 0, s: 1, v: 1 };
      var alpha = 1;

      // Initialize from hidden input
      var initRgb = parseColorValue(hidden.value);
      hsv = rgbToHsv(initRgb.r, initRgb.g, initRgb.b);
      if (hueInput) hueInput.value = Math.round(hsv.h);

      function updateAll() {
        var rgb = hsvToRgb(hsv.h, hsv.s, hsv.v);
        var colorStr = formatColor(rgb, format, hasOpacity ? alpha : 1);

        if (hidden) {
          hidden.value = colorStr;
          hidden.dispatchEvent(new Event("input", { bubbles: true }));
          hidden.dispatchEvent(new Event("change", { bubbles: true }));
        }
        if (textInput) textInput.value = colorStr;
        if (swatch) swatch.style.backgroundColor = "rgb(" + rgb.r + "," + rgb.g + "," + rgb.b + ")";
        if (textDisplay) textDisplay.textContent = colorStr;

        // Update saturation area background
        if (satArea) {
          satArea.style.background =
            "linear-gradient(to bottom, transparent, #000)," +
            "linear-gradient(to right, #fff, hsl(" + Math.round(hsv.h) + ", 100%, 50%))";
        }

        // Update cursor position
        if (satArea && satCursor) {
          satCursor.style.left = (hsv.s * 100) + "%";
          satCursor.style.top = ((1 - hsv.v) * 100) + "%";
        }

        picker.dispatchEvent(new CustomEvent("rhx:color-picker:change", {
          bubbles: true,
          detail: { value: colorStr, rgb: rgb, hsv: hsv }
        }));
      }

      // ── Panel toggle ──
      if (trigger && panel && !isInline) {
        trigger.addEventListener("click", function () {
          var isOpen = !panel.hidden;
          panel.hidden = !isOpen ? false : true;
          if (!panel.hidden) updateAll(); // sync on open
          panel.hidden = isOpen;
        });

        document.addEventListener("click", function (e) {
          if (!panel.hidden && !picker.contains(e.target)) {
            panel.hidden = true;
          }
        });
      }

      // ── Saturation area drag ──
      if (satArea) {
        function handleSaturation(e) {
          var rect = satArea.getBoundingClientRect();
          var x = Math.max(0, Math.min(e.clientX - rect.left, rect.width));
          var y = Math.max(0, Math.min(e.clientY - rect.top, rect.height));
          hsv.s = x / rect.width;
          hsv.v = 1 - (y / rect.height);
          updateAll();
        }

        var dragging = false;
        satArea.addEventListener("mousedown", function (e) {
          dragging = true;
          handleSaturation(e);
        });
        document.addEventListener("mousemove", function (e) {
          if (dragging) handleSaturation(e);
        });
        document.addEventListener("mouseup", function () {
          dragging = false;
        });
      }

      // ── Hue slider ──
      if (hueInput) {
        hueInput.addEventListener("input", function () {
          hsv.h = parseFloat(hueInput.value);
          updateAll();
        });
      }

      // ── Opacity slider ──
      if (opacityInput && hasOpacity) {
        opacityInput.addEventListener("input", function () {
          alpha = parseFloat(opacityInput.value) / 100;
          updateAll();
        });
      }

      // ── Text input ──
      if (textInput) {
        textInput.addEventListener("change", function () {
          var rgb = parseColorValue(textInput.value);
          hsv = rgbToHsv(rgb.r, rgb.g, rgb.b);
          if (hueInput) hueInput.value = Math.round(hsv.h);
          updateAll();
        });
      }

      // ── Preset swatches ──
      presets.forEach(function (preset) {
        preset.addEventListener("click", function () {
          var color = preset.getAttribute("data-color");
          if (color) {
            var rgb = parseColorValue(color);
            hsv = rgbToHsv(rgb.r, rgb.g, rgb.b);
            if (hueInput) hueInput.value = Math.round(hsv.h);
            updateAll();
          }
        });
      });

      // Initial sync
      updateAll();
    });
  }

  if (window.RHX) {
    window.RHX.register("color-picker", initColorPickers);
  }
})();
