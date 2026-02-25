/**
 * htmxRazor Combobox
 * Open/close, keyboard navigation, client-side filtering (or server-side via htmx),
 * option selection, form sync via hidden input, click-outside dismissal,
 * and ARIA state management.
 */
(function () {
  "use strict";

  var OPTION_SELECTOR =
    "[role='option']:not([aria-disabled='true']):not(.rhx-combobox__option--disabled)";

  function initComboboxes(root) {
    var comboboxes = root.querySelectorAll("[data-rhx-combobox]");
    comboboxes.forEach(function (combobox) {
      if (combobox._rhxComboboxInit) return;
      combobox._rhxComboboxInit = true;

      var input = combobox.querySelector("[role='combobox']");
      var triggerBtn = combobox.querySelector(".rhx-combobox__trigger");
      var listbox = combobox.querySelector("[role='listbox']");
      var hiddenInput = combobox.querySelector("[data-rhx-combobox-value]");
      var isServerFilter = combobox.hasAttribute("data-rhx-server-filter");

      if (!input || !listbox) return;

      var focusedIdx = -1;
      var lastQuery = "";

      function getOptions() {
        return Array.from(listbox.querySelectorAll(OPTION_SELECTOR));
      }

      function getVisibleOptions() {
        return getOptions().filter(function (opt) {
          return !opt.hidden && opt.style.display !== "none";
        });
      }

      function open() {
        listbox.hidden = false;
        input.setAttribute("aria-expanded", "true");
        setMaxHeight();

        var options = getVisibleOptions();
        if (options.length) {
          setFocused(0, options);
        }
      }

      function close() {
        listbox.hidden = true;
        input.setAttribute("aria-expanded", "false");
        focusedIdx = -1;
        clearFocused();
      }

      function isOpen() {
        return !listbox.hidden;
      }

      function setMaxHeight() {
        var maxVisible = parseInt(listbox.getAttribute("data-rhx-max-visible") || "8", 10);
        var options = listbox.querySelectorAll("[role='option']");
        if (options.length > maxVisible && options[0]) {
          var itemHeight = options[0].offsetHeight;
          listbox.style.maxHeight = (itemHeight * maxVisible + 8) + "px";
        }
      }

      function setFocused(idx, options) {
        options = options || getVisibleOptions();
        clearFocused();
        if (idx >= 0 && idx < options.length) {
          focusedIdx = idx;
          options[idx].setAttribute("data-rhx-focused", "");
          options[idx].scrollIntoView({ block: "nearest" });
          input.setAttribute("aria-activedescendant",
            options[idx].id || "rhx-cbo-" + idx);
        }
      }

      function clearFocused() {
        var prev = listbox.querySelector("[data-rhx-focused]");
        if (prev) prev.removeAttribute("data-rhx-focused");
        input.removeAttribute("aria-activedescendant");
      }

      // ── Client-side filtering ──

      function filterOptions(query) {
        if (isServerFilter) return; // Server handles filtering via htmx

        var q = query.toLowerCase().trim();
        var options = getOptions();
        var hasVisible = false;

        options.forEach(function (opt) {
          var text = opt.textContent.trim().toLowerCase();
          var match = !q || text.indexOf(q) >= 0;
          opt.hidden = !match;
          opt.style.display = match ? "" : "none";
          if (match) hasVisible = true;
        });

        // Show/hide "no results" message
        var noResults = listbox.querySelector(".rhx-combobox__no-results");
        if (!hasVisible && !noResults) {
          noResults = document.createElement("div");
          noResults.className = "rhx-combobox__no-results";
          noResults.textContent = "No results found";
          listbox.appendChild(noResults);
        } else if (hasVisible && noResults) {
          noResults.remove();
        }

        // Reset focus to first visible
        var visible = getVisibleOptions();
        if (visible.length) {
          setFocused(0, visible);
        } else {
          focusedIdx = -1;
          clearFocused();
        }
      }

      // ── Selection ──

      function selectOption(option) {
        if (!option || option.getAttribute("aria-disabled") === "true") return;

        var value = option.getAttribute("data-value") || "";
        var text = option.textContent.trim();

        // Deselect previous
        var prev = listbox.querySelector(".rhx-combobox__option--selected");
        if (prev) {
          prev.classList.remove("rhx-combobox__option--selected");
          prev.setAttribute("aria-selected", "false");
        }

        // Select new
        option.classList.add("rhx-combobox__option--selected");
        option.setAttribute("aria-selected", "true");

        // Update display input
        input.value = text;
        lastQuery = text;

        // Update hidden input
        if (hiddenInput) {
          hiddenInput.value = value;
          hiddenInput.dispatchEvent(new Event("input", { bubbles: true }));
          hiddenInput.dispatchEvent(new Event("change", { bubbles: true }));
        }

        combobox.dispatchEvent(new CustomEvent("rhx:combobox:change", {
          bubbles: true,
          detail: { value: value, text: text }
        }));

        close();
        input.focus();
      }

      // ── Input events ──

      input.addEventListener("input", function () {
        var query = input.value;
        if (query !== lastQuery) {
          lastQuery = query;
          if (!isOpen()) open();
          filterOptions(query);

          // Clear hidden value when user types (value not yet committed)
          if (hiddenInput) hiddenInput.value = "";
        }
      });

      input.addEventListener("focus", function () {
        if (!isOpen() && input.value === "") {
          open();
        }
      });

      input.addEventListener("keydown", function (e) {
        var options = getVisibleOptions();

        switch (e.key) {
          case "ArrowDown":
            e.preventDefault();
            if (!isOpen()) {
              open();
            } else {
              var next = focusedIdx + 1;
              if (next < options.length) setFocused(next, options);
            }
            break;

          case "ArrowUp":
            e.preventDefault();
            if (!isOpen()) {
              open();
            } else {
              var prev = focusedIdx - 1;
              if (prev >= 0) setFocused(prev, options);
            }
            break;

          case "Enter":
            if (isOpen() && focusedIdx >= 0 && focusedIdx < options.length) {
              e.preventDefault();
              selectOption(options[focusedIdx]);
            }
            break;

          case "Escape":
            if (isOpen()) {
              e.preventDefault();
              close();
              // Restore last committed value
              if (hiddenInput && hiddenInput.value) {
                var selected = listbox.querySelector(".rhx-combobox__option--selected");
                if (selected) input.value = selected.textContent.trim();
              }
            }
            break;

          case "Home":
            if (isOpen()) {
              e.preventDefault();
              if (options.length) setFocused(0, options);
            }
            break;

          case "End":
            if (isOpen()) {
              e.preventDefault();
              if (options.length) setFocused(options.length - 1, options);
            }
            break;

          case "Tab":
            if (isOpen()) close();
            break;
        }
      });

      // ── Trigger button ──
      if (triggerBtn) {
        triggerBtn.addEventListener("click", function (e) {
          e.preventDefault();
          if (isOpen()) {
            close();
          } else {
            open();
            input.focus();
          }
        });
      }

      // ── Option click ──
      listbox.addEventListener("click", function (e) {
        var option = e.target.closest(OPTION_SELECTOR);
        if (option && listbox.contains(option)) {
          selectOption(option);
        }
      });

      // ── Click outside ──
      document.addEventListener("click", function (e) {
        if (isOpen() && !combobox.contains(e.target)) {
          close();
        }
      });

      // ── Re-init after htmx swap (server-side filtering) ──
      if (isServerFilter) {
        listbox.addEventListener("htmx:afterSwap", function () {
          var options = getVisibleOptions();
          if (options.length) {
            setFocused(0, options);
          }
          setMaxHeight();
        });
      }
    });
  }

  if (window.RHX) {
    window.RHX.register("combobox", initComboboxes);
  }
})();
