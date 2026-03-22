(function () {
    "use strict";

    function init(root) {
        root.querySelectorAll("[data-rhx-htmx-form]").forEach(function (form) {
            if (form._rhxHtmxFormInit) return;
            form._rhxHtmxFormInit = true;

            // Clear error container and add submitting state on new request
            form.addEventListener("htmx:beforeRequest", function () {
                var errContainer = form.querySelector(".rhx-htmx-form__error-container");
                if (errContainer) {
                    errContainer.hidden = true;
                    errContainer.innerHTML = "";
                }
                form.classList.add("rhx-htmx-form--submitting");
                form.setAttribute("aria-busy", "true");
            });

            // Remove submitting state after request completes
            form.addEventListener("htmx:afterRequest", function (e) {
                form.classList.remove("rhx-htmx-form--submitting");
                form.removeAttribute("aria-busy");

                // Reset form on success if configured
                if (e.detail.successful && form.dataset.rhxResetOnSuccess === "true") {
                    form.reset();
                }
            });

            // Show error container when it receives content
            form.addEventListener("htmx:afterSwap", function () {
                var errContainer = form.querySelector(".rhx-htmx-form__error-container");
                if (errContainer && errContainer.innerHTML.trim()) {
                    errContainer.hidden = false;
                }
            });
        });
    }

    if (typeof RHX !== "undefined" && RHX.register) {
        RHX.register("htmx-form", init);
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
