(function () {
    "use strict";

    function init(root) {
        root.querySelectorAll("[data-rhx-wizard]").forEach(function (wizard) {
            if (wizard._rhxWizardInit) return;
            wizard._rhxWizardInit = true;

            // Keyboard navigation between step indicators (roving tabindex)
            var indicators = wizard.querySelectorAll("button.rhx-wizard__step-indicator");
            if (indicators.length === 0) return;

            indicators.forEach(function (btn, idx) {
                btn.setAttribute("tabindex", idx === 0 ? "0" : "-1");

                btn.addEventListener("keydown", function (e) {
                    var nextIdx = -1;
                    if (e.key === "ArrowRight" || e.key === "ArrowDown") {
                        nextIdx = (idx + 1) % indicators.length;
                    } else if (e.key === "ArrowLeft" || e.key === "ArrowUp") {
                        nextIdx = (idx - 1 + indicators.length) % indicators.length;
                    }

                    if (nextIdx >= 0) {
                        e.preventDefault();
                        btn.setAttribute("tabindex", "-1");
                        indicators[nextIdx].setAttribute("tabindex", "0");
                        indicators[nextIdx].focus();
                    }
                });
            });
        });
    }

    if (typeof RHX !== "undefined" && RHX.register) {
        RHX.register("wizard", init);
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
