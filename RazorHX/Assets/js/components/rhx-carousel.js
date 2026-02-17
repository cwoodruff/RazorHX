/**
 * RazorHX Carousel
 * Slide mechanics with keyboard navigation and touch support.
 */
(function () {
  "use strict";

  function initCarousels(root) {
    var carousels = root.querySelectorAll("[data-rhx-carousel]");
    carousels.forEach(function (carousel) {
      if (carousel._rhxCarouselInit) return;
      carousel._rhxCarouselInit = true;

      var track = carousel.querySelector("[data-rhx-carousel-track]");
      var slides = Array.from(carousel.querySelectorAll("[data-rhx-carousel-slide]"));
      var prevBtn = carousel.querySelector("[data-rhx-carousel-prev]");
      var nextBtn = carousel.querySelector("[data-rhx-carousel-next]");
      var currentIndex = 0;

      if (!track || slides.length === 0) return;

      function goTo(index) {
        currentIndex = Math.max(0, Math.min(index, slides.length - 1));
        var offset = -(currentIndex * 100);
        track.style.transform = "translateX(" + offset + "%)";

        slides.forEach(function (slide, i) {
          slide.setAttribute("aria-hidden", i !== currentIndex ? "true" : "false");
        });

        if (prevBtn) prevBtn.disabled = currentIndex === 0;
        if (nextBtn) nextBtn.disabled = currentIndex === slides.length - 1;
      }

      if (prevBtn) {
        prevBtn.addEventListener("click", function () {
          goTo(currentIndex - 1);
        });
      }

      if (nextBtn) {
        nextBtn.addEventListener("click", function () {
          goTo(currentIndex + 1);
        });
      }

      carousel.addEventListener("keydown", function (e) {
        if (e.key === "ArrowLeft") {
          e.preventDefault();
          goTo(currentIndex - 1);
        } else if (e.key === "ArrowRight") {
          e.preventDefault();
          goTo(currentIndex + 1);
        }
      });

      goTo(0);
    });
  }

  if (window.RHX) {
    window.RHX.register("carousel", initCarousels);
  }
})();
