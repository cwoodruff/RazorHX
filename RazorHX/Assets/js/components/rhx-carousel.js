/**
 * RazorHX Carousel
 * Full-featured carousel with navigation, pagination, autoplay,
 * touch/mouse dragging, keyboard navigation, and loop support.
 */
(function () {
  "use strict";

  function initCarousels(root) {
    var carousels = root.querySelectorAll("[data-rhx-carousel]");
    carousels.forEach(function (el) {
      if (el._rhxCarouselInit) return;
      el._rhxCarouselInit = true;
      initCarousel(el);
    });
  }

  function initCarousel(el) {
    var track = el.querySelector(".rhx-carousel__track");
    var items = Array.from(el.querySelectorAll(".rhx-carousel__item"));
    var viewport = el.querySelector(".rhx-carousel__viewport");
    if (!track || !viewport || items.length === 0) return;

    // ── Config from data attributes ──
    var loop = el.hasAttribute("data-rhx-loop");
    var autoplayMs = el.hasAttribute("data-rhx-autoplay")
      ? parseInt(el.getAttribute("data-rhx-autoplay"), 10)
      : 0;
    var perPage = parseInt(el.getAttribute("data-rhx-slides-per-page") || "1", 10);
    var perMove = parseInt(el.getAttribute("data-rhx-slides-per-move") || "1", 10);
    var orientation = el.getAttribute("data-rhx-orientation") || "horizontal";
    var mouseDrag = el.hasAttribute("data-rhx-mouse-dragging");
    var isVertical = orientation === "vertical";
    var total = items.length;
    var maxIndex = Math.max(0, total - perPage);

    var current = 0;
    var autoplayTimer = null;
    var dragState = null;

    // ── Elements ──
    var prevBtn = el.querySelector(".rhx-carousel__nav-button--prev");
    var nextBtn = el.querySelector(".rhx-carousel__nav-button--next");
    var dots = Array.from(el.querySelectorAll(".rhx-carousel__dot"));

    // Multi-slide item sizing
    if (perPage > 1) {
      items.forEach(function (item) {
        item.style.flex = "0 0 " + (100 / perPage) + "%";
      });
    }

    // Enhance ARIA labels with total count
    items.forEach(function (item, i) {
      item.setAttribute("aria-label", "Slide " + (i + 1) + " of " + total);
    });

    // ── Navigation ──

    function goTo(index, instant) {
      if (loop) {
        if (index < 0) index = maxIndex;
        else if (index > maxIndex) index = 0;
      } else {
        index = Math.max(0, Math.min(index, maxIndex));
      }

      current = index;
      var offset = -(index * (100 / perPage));

      if (instant) track.style.transition = "none";

      track.style.transform = isVertical
        ? "translateY(" + offset + "%)"
        : "translateX(" + offset + "%)";

      if (instant) {
        /* force reflow */ track.offsetHeight;
        track.style.transition = "";
      }

      syncControls();
      el.dispatchEvent(
        new CustomEvent("rhx:carousel:change", {
          bubbles: true,
          detail: { index: current },
        })
      );
    }

    function prev() {
      goTo(current - perMove);
    }
    function next() {
      goTo(current + perMove);
    }

    function syncControls() {
      if (prevBtn) prevBtn.disabled = !loop && current <= 0;
      if (nextBtn) nextBtn.disabled = !loop && current >= maxIndex;

      dots.forEach(function (dot, i) {
        var active = i === current;
        dot.setAttribute("aria-selected", active ? "true" : "false");
        dot.setAttribute("tabindex", active ? "0" : "-1");
      });

      items.forEach(function (item, i) {
        var visible = i >= current && i < current + perPage;
        item.setAttribute("aria-hidden", visible ? "false" : "true");
      });
    }

    // ── Button events ──

    if (prevBtn)
      prevBtn.addEventListener("click", function () {
        prev();
        restartAutoplay();
      });
    if (nextBtn)
      nextBtn.addEventListener("click", function () {
        next();
        restartAutoplay();
      });

    // ── Dot events ──

    dots.forEach(function (dot, i) {
      dot.addEventListener("click", function () {
        goTo(i);
        restartAutoplay();
      });

      dot.addEventListener("keydown", function (e) {
        var target = -1;
        if (e.key === "ArrowRight" || e.key === "ArrowDown") {
          e.preventDefault();
          target = (i + 1) % dots.length;
        } else if (e.key === "ArrowLeft" || e.key === "ArrowUp") {
          e.preventDefault();
          target = (i - 1 + dots.length) % dots.length;
        } else if (e.key === "Home") {
          e.preventDefault();
          target = 0;
        } else if (e.key === "End") {
          e.preventDefault();
          target = dots.length - 1;
        }
        if (target >= 0) {
          dots[target].focus();
          goTo(target);
          restartAutoplay();
        }
      });
    });

    // ── Carousel keyboard ──

    el.addEventListener("keydown", function (e) {
      // Skip if inside pagination (handled above)
      if (e.target.closest(".rhx-carousel__pagination")) return;

      var k = e.key;
      if (k === "ArrowLeft" || (isVertical && k === "ArrowUp")) {
        e.preventDefault();
        prev();
        restartAutoplay();
      } else if (k === "ArrowRight" || (isVertical && k === "ArrowDown")) {
        e.preventDefault();
        next();
        restartAutoplay();
      } else if (k === "Home") {
        e.preventDefault();
        goTo(0);
        restartAutoplay();
      } else if (k === "End") {
        e.preventDefault();
        goTo(maxIndex);
        restartAutoplay();
      }
    });

    // ── Touch / Mouse drag ──

    function beginDrag(x, y) {
      dragState = { startX: x, startY: y, curX: x, curY: y, active: false };
    }

    function updateDrag(x, y) {
      if (!dragState) return;
      dragState.curX = x;
      dragState.curY = y;

      var delta = isVertical
        ? y - dragState.startY
        : x - dragState.startX;

      if (!dragState.active) {
        if (Math.abs(delta) < 5) return;
        dragState.active = true;
        el.classList.add("rhx-carousel--dragging");
        stopAutoplay();
      }

      var size = isVertical ? viewport.offsetHeight : viewport.offsetWidth;
      var pct = (delta / size) * 100;
      var base = -(current * (100 / perPage));

      track.style.transform = isVertical
        ? "translateY(" + (base + pct) + "%)"
        : "translateX(" + (base + pct) + "%)";
    }

    function finishDrag() {
      if (!dragState || !dragState.active) {
        dragState = null;
        return;
      }

      el.classList.remove("rhx-carousel--dragging");

      var delta = isVertical
        ? dragState.curY - dragState.startY
        : dragState.curX - dragState.startX;
      var size = isVertical ? viewport.offsetHeight : viewport.offsetWidth;
      var threshold = size * 0.2;

      if (Math.abs(delta) > threshold) {
        delta < 0 ? next() : prev();
      } else {
        goTo(current);
      }

      dragState = null;
      restartAutoplay();
    }

    // Touch
    viewport.addEventListener(
      "touchstart",
      function (e) {
        var t = e.touches[0];
        beginDrag(t.clientX, t.clientY);
      },
      { passive: true }
    );

    viewport.addEventListener(
      "touchmove",
      function (e) {
        if (!dragState) return;
        var t = e.touches[0];
        if (dragState.active) e.preventDefault();
        updateDrag(t.clientX, t.clientY);
      },
      { passive: false }
    );

    viewport.addEventListener("touchend", finishDrag);
    viewport.addEventListener("touchcancel", finishDrag);

    // Mouse
    if (mouseDrag) {
      viewport.addEventListener("mousedown", function (e) {
        if (e.button !== 0) return;
        e.preventDefault();
        beginDrag(e.clientX, e.clientY);

        function onMove(ev) {
          updateDrag(ev.clientX, ev.clientY);
        }
        function onUp() {
          document.removeEventListener("mousemove", onMove);
          document.removeEventListener("mouseup", onUp);
          finishDrag();
        }
        document.addEventListener("mousemove", onMove);
        document.addEventListener("mouseup", onUp);
      });
    }

    // Prevent native image drag
    items.forEach(function (item) {
      item.querySelectorAll("img").forEach(function (img) {
        img.setAttribute("draggable", "false");
      });
    });

    // ── Autoplay ──

    function startAutoplay() {
      if (!autoplayMs || autoplayTimer) return;
      autoplayTimer = setInterval(next, autoplayMs);
    }

    function stopAutoplay() {
      if (autoplayTimer) {
        clearInterval(autoplayTimer);
        autoplayTimer = null;
      }
    }

    function restartAutoplay() {
      stopAutoplay();
      startAutoplay();
    }

    if (autoplayMs) {
      el.addEventListener("mouseenter", stopAutoplay);
      el.addEventListener("mouseleave", startAutoplay);
      el.addEventListener("focusin", stopAutoplay);
      el.addEventListener("focusout", function (e) {
        if (!el.contains(e.relatedTarget)) startAutoplay();
      });
      startAutoplay();
    }

    // ── Initial state ──
    goTo(0, true);
  }

  // Auto-init
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", function () {
      initCarousels(document);
    });
  } else {
    initCarousels(document);
  }

  // Re-init after htmx swaps
  document.addEventListener("htmx:afterSettle", function (e) {
    initCarousels(e.detail.elt);
  });
})();
