(function () {
    "use strict";

    function init(root) {
        // ── Switch optimistic ──
        root.querySelectorAll("[data-rhx-switch][data-rhx-optimistic]").forEach(function (el) {
            if (el._rhxOptimisticInit) return;
            el._rhxOptimisticInit = true;

            var checkbox = el.querySelector('input[type="checkbox"]');
            if (!checkbox) return;

            checkbox.addEventListener("change", function () {
                el._rhxPrevState = !checkbox.checked;
            });

            el.addEventListener("htmx:responseError", function () {
                if (el._rhxPrevState !== undefined) {
                    checkbox.checked = el._rhxPrevState;
                    checkbox.setAttribute("aria-checked", String(el._rhxPrevState));
                    el._rhxPrevState = undefined;
                    el.classList.add("rhx-optimistic--reverted");
                    setTimeout(function () { el.classList.remove("rhx-optimistic--reverted"); }, 600);
                }
            });

            el.addEventListener("htmx:afterRequest", function (e) {
                if (e.detail.successful) {
                    el._rhxPrevState = undefined;
                }
            });
        });

        // ── Rating optimistic ──
        root.querySelectorAll("[data-rhx-rating][data-rhx-optimistic]").forEach(function (el) {
            if (el._rhxOptimisticRatingInit) return;
            el._rhxOptimisticRatingInit = true;

            var hidden = el.querySelector(".rhx-rating__value");
            if (!hidden) return;
            var prevValue = hidden.value;

            hidden.addEventListener("change", function () {
                prevValue = hidden.value;
            });

            el.addEventListener("htmx:responseError", function () {
                hidden.value = prevValue;
                hidden.dispatchEvent(new Event("change", { bubbles: true }));
                el.classList.add("rhx-optimistic--reverted");
                setTimeout(function () { el.classList.remove("rhx-optimistic--reverted"); }, 600);
            });
        });

        // ── Button optimistic ──
        root.querySelectorAll("button[data-rhx-optimistic], a[data-rhx-optimistic]").forEach(function (el) {
            if (el._rhxOptimisticBtnInit) return;
            el._rhxOptimisticBtnInit = true;

            el.addEventListener("htmx:beforeRequest", function () {
                el.classList.add("rhx-button--loading");
                el.setAttribute("aria-busy", "true");
                el.disabled = true;
            });

            el.addEventListener("htmx:afterRequest", function (e) {
                el.classList.remove("rhx-button--loading");
                el.removeAttribute("aria-busy");
                el.disabled = false;

                if (!e.detail.successful) {
                    el.classList.add("rhx-optimistic--reverted");
                    setTimeout(function () { el.classList.remove("rhx-optimistic--reverted"); }, 600);
                }
            });
        });
    }

    if (typeof RHX !== "undefined" && RHX.register) {
        RHX.register("optimistic", init);
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", function () { init(document); });
    } else {
        init(document);
    }

    document.addEventListener("htmx:afterSettle", function (e) {
        init(e.detail.elt);
    });
})();
