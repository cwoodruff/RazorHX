/**
 * RazorHX Dropdown
 * Keyboard navigation, open/close, outside-click dismissal.
 */
(function () {
  "use strict";

  function initDropdowns(root) {
    var dropdowns = root.querySelectorAll("[data-rhx-dropdown]");
    dropdowns.forEach(function (dropdown) {
      if (dropdown._rhxDropdownInit) return;
      dropdown._rhxDropdownInit = true;

      var trigger = dropdown.querySelector("[data-rhx-dropdown-trigger]");
      var menu = dropdown.querySelector("[data-rhx-dropdown-menu]");
      if (!trigger || !menu) return;

      function open() {
        menu.hidden = false;
        trigger.setAttribute("aria-expanded", "true");
        var first = menu.querySelector("[role='menuitem']:not([disabled])");
        if (first) first.focus();
      }

      function close() {
        menu.hidden = true;
        trigger.setAttribute("aria-expanded", "false");
        trigger.focus();
      }

      function isOpen() {
        return !menu.hidden;
      }

      trigger.addEventListener("click", function () {
        isOpen() ? close() : open();
      });

      menu.addEventListener("keydown", function (e) {
        var items = Array.from(menu.querySelectorAll("[role='menuitem']:not([disabled])"));
        var idx = items.indexOf(document.activeElement);

        if (e.key === "ArrowDown") {
          e.preventDefault();
          items[(idx + 1) % items.length].focus();
        } else if (e.key === "ArrowUp") {
          e.preventDefault();
          items[(idx - 1 + items.length) % items.length].focus();
        } else if (e.key === "Escape") {
          e.preventDefault();
          close();
        } else if (e.key === "Home") {
          e.preventDefault();
          items[0].focus();
        } else if (e.key === "End") {
          e.preventDefault();
          items[items.length - 1].focus();
        }
      });

      document.addEventListener("click", function (e) {
        if (isOpen() && !dropdown.contains(e.target)) {
          close();
        }
      });
    });
  }

  if (window.RHX) {
    window.RHX.register("dropdown", initDropdowns);
  }
})();
