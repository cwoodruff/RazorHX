/**
 * htmxRazor Rating
 * Interactive star rating with click, keyboard, and hover preview support.
 * Supports whole and half-star precision.
 */
(function () {
  "use strict";

  function initRatings(root) {
    var ratings = root.querySelectorAll("[data-rhx-rating]");
    ratings.forEach(function (rating) {
      if (rating._rhxRatingInit) return;
      rating._rhxRatingInit = true;

      var stars = rating.querySelectorAll(".rhx-rating__star");
      var hidden = rating.querySelector(".rhx-rating__value");
      var max = parseInt(rating.getAttribute("data-rhx-max") || "5", 10);
      var precision = parseFloat(rating.getAttribute("data-rhx-precision") || "1");
      var isReadonly = rating.classList.contains("rhx-rating--readonly");
      var isDisabled = rating.classList.contains("rhx-rating--disabled");

      if (isReadonly || isDisabled || !hidden) return;

      function getCurrentValue() {
        return parseFloat(hidden.value) || 0;
      }

      function setValue(val) {
        val = Math.max(0, Math.min(max, val));
        if (precision === 1) val = Math.round(val);
        else val = Math.round(val * 2) / 2; // snap to 0.5

        hidden.value = val;
        rating.setAttribute("aria-valuenow", val);
        updateStars(val);

        hidden.dispatchEvent(new Event("input", { bubbles: true }));
        hidden.dispatchEvent(new Event("change", { bubbles: true }));

        rating.dispatchEvent(new CustomEvent("rhx:rating:change", {
          bubbles: true,
          detail: { value: val }
        }));
      }

      function updateStars(val) {
        stars.forEach(function (star, idx) {
          var starVal = idx + 1;
          star.className = "rhx-rating__star";
          if (val >= starVal) {
            star.className += " rhx-rating__star--filled";
          } else if (precision <= 0.5 && val >= starVal - 0.5) {
            star.className += " rhx-rating__star--half";
          }
        });
      }

      function previewStars(val) {
        stars.forEach(function (star, idx) {
          var starVal = idx + 1;
          if (val >= starVal) {
            star.setAttribute("data-hover", "true");
          } else {
            star.removeAttribute("data-hover");
          }
        });
      }

      function clearPreview() {
        stars.forEach(function (star) {
          star.removeAttribute("data-hover");
        });
      }

      // ── Click handler ──
      stars.forEach(function (star) {
        star.addEventListener("click", function (e) {
          var starVal = parseInt(star.getAttribute("data-value") || "0", 10);
          if (precision <= 0.5) {
            // Determine if click is on left or right half
            var rect = star.getBoundingClientRect();
            var x = e.clientX - rect.left;
            if (x < rect.width / 2) {
              setValue(starVal - 0.5);
            } else {
              setValue(starVal);
            }
          } else {
            setValue(starVal);
          }
        });

        star.addEventListener("mouseenter", function () {
          var starVal = parseInt(star.getAttribute("data-value") || "0", 10);
          previewStars(starVal);
        });
      });

      // Clear preview on leave
      var starsContainer = rating.querySelector(".rhx-rating__stars");
      if (starsContainer) {
        starsContainer.addEventListener("mouseleave", function () {
          clearPreview();
        });
      }

      // ── Keyboard handler ──
      rating.addEventListener("keydown", function (e) {
        var current = getCurrentValue();
        var step = precision <= 0.5 ? 0.5 : 1;

        switch (e.key) {
          case "ArrowRight":
          case "ArrowUp":
            e.preventDefault();
            setValue(current + step);
            break;
          case "ArrowLeft":
          case "ArrowDown":
            e.preventDefault();
            setValue(current - step);
            break;
          case "Home":
            e.preventDefault();
            setValue(0);
            break;
          case "End":
            e.preventDefault();
            setValue(max);
            break;
        }
      });
    });
  }

  // ── Registration ──
  if (typeof RHX !== 'undefined' && RHX.register) {
    RHX.register('rating', initRatings);
  }

  // Auto-init
  function initAll() {
    initRatings(document);
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
      initRatings(el);
    }
  });
})();
