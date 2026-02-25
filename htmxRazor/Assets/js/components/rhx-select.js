/**
 * htmxRazor Select
 * Open/close toggling, keyboard navigation (Arrow keys, Home/End, Enter/Space, Escape, Tab),
 * single and multi-select, type-ahead, form sync via hidden inputs, click-outside dismissal,
 * clear button, and ARIA state management.
 */
(function () {
  "use strict";

  var OPTION_SELECTOR =
    "[role='option']:not([aria-disabled='true']):not(.rhx-select__option--disabled)";

  function initSelects(root) {
    var selects = root.querySelectorAll("[data-rhx-select]");
    selects.forEach(function (select) {
      if (select._rhxSelectInit) return;
      select._rhxSelectInit = true;

      var trigger = select.querySelector(".rhx-select__trigger");
      var listbox = select.querySelector("[role='listbox']");
      var valueDisplay = select.querySelector(".rhx-select__value");
      var clearBtn = select.querySelector(".rhx-select__clear");
      var isMultiple = select.hasAttribute("data-rhx-select-multiple");

      if (!trigger || !listbox) return;

      var focusedIdx = -1;
      var typeAheadBuffer = "";
      var typeAheadTimer = null;

      function getOptions() {
        return Array.from(listbox.querySelectorAll(OPTION_SELECTOR));
      }

      function getHiddenInput() {
        return select.querySelector("[data-rhx-select-value]");
      }

      function getValuesContainer() {
        return select.querySelector("[data-rhx-select-values]");
      }

      function open() {
        listbox.hidden = false;
        trigger.setAttribute("aria-expanded", "true");
        setMaxHeight();

        // Focus first selected or first option
        var options = getOptions();
        var selected = listbox.querySelector(".rhx-select__option--selected");
        var idx = selected ? options.indexOf(selected) : 0;
        if (idx >= 0 && options.length) {
          setFocused(idx, options);
        }
      }

      function close() {
        listbox.hidden = true;
        trigger.setAttribute("aria-expanded", "false");
        focusedIdx = -1;
        clearFocused();
        trigger.focus();
      }

      function isOpen() {
        return !listbox.hidden;
      }

      function toggle() {
        isOpen() ? close() : open();
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
        options = options || getOptions();
        clearFocused();
        if (idx >= 0 && idx < options.length) {
          focusedIdx = idx;
          options[idx].setAttribute("data-rhx-focused", "");
          options[idx].scrollIntoView({ block: "nearest" });
          trigger.setAttribute("aria-activedescendant",
            options[idx].id || "rhx-opt-" + idx);
        }
      }

      function clearFocused() {
        var prev = listbox.querySelector("[data-rhx-focused]");
        if (prev) prev.removeAttribute("data-rhx-focused");
        trigger.removeAttribute("aria-activedescendant");
      }

      // ── Selection ──

      function selectOption(option) {
        if (!option || option.getAttribute("aria-disabled") === "true") return;

        var value = option.getAttribute("data-value") || "";
        var text = option.textContent.trim();

        if (isMultiple) {
          toggleMultiOption(option, value, text);
        } else {
          selectSingle(option, value, text);
          close();
        }

        select.dispatchEvent(new CustomEvent("rhx:select:change", {
          bubbles: true,
          detail: { value: isMultiple ? getMultiValues() : value, text: text }
        }));
      }

      function selectSingle(option, value, text) {
        // Deselect previous
        var prev = listbox.querySelector(".rhx-select__option--selected");
        if (prev) {
          prev.classList.remove("rhx-select__option--selected");
          prev.setAttribute("aria-selected", "false");
        }

        // Select new
        option.classList.add("rhx-select__option--selected");
        option.setAttribute("aria-selected", "true");

        // Update display
        valueDisplay.textContent = text;

        // Update hidden input
        var hidden = getHiddenInput();
        if (hidden) {
          hidden.value = value;
          hidden.dispatchEvent(new Event("input", { bubbles: true }));
          hidden.dispatchEvent(new Event("change", { bubbles: true }));
        }

        // Show clear button
        if (clearBtn) clearBtn.hidden = false;
      }

      function toggleMultiOption(option, value, text) {
        var isSelected = option.getAttribute("aria-selected") === "true";

        if (isSelected) {
          option.classList.remove("rhx-select__option--selected");
          option.setAttribute("aria-selected", "false");
          removeTag(value);
          removeHiddenValue(value);
        } else {
          option.classList.add("rhx-select__option--selected");
          option.setAttribute("aria-selected", "true");
          addTag(value, text);
          addHiddenValue(value);
        }
      }

      function getMultiValues() {
        var container = getValuesContainer();
        if (!container) return [];
        return Array.from(container.querySelectorAll("input")).map(function (i) {
          return i.value;
        });
      }

      // ── Tag management (multi-select) ──

      function addTag(value, text) {
        var tag = document.createElement("span");
        tag.className = "rhx-select__tag";
        tag.setAttribute("data-value", value);
        tag.innerHTML = text +
          '<button class="rhx-select__tag-remove" type="button" aria-label="Remove ' +
          text + '">&times;</button>';

        tag.querySelector(".rhx-select__tag-remove").addEventListener("click", function (e) {
          e.stopPropagation();
          var opt = listbox.querySelector("[data-value='" + value + "']");
          if (opt) {
            opt.classList.remove("rhx-select__option--selected");
            opt.setAttribute("aria-selected", "false");
          }
          removeTag(value);
          removeHiddenValue(value);
          select.dispatchEvent(new CustomEvent("rhx:select:change", {
            bubbles: true,
            detail: { value: getMultiValues() }
          }));
        });

        // Remove placeholder if present
        var ph = valueDisplay.querySelector(".rhx-select__placeholder");
        if (ph) ph.remove();

        valueDisplay.appendChild(tag);
      }

      function removeTag(value) {
        var tag = valueDisplay.querySelector("[data-value='" + value + "']");
        if (tag) tag.remove();

        // Restore placeholder if empty
        if (!valueDisplay.querySelector(".rhx-select__tag")) {
          var ph = select.querySelector(".rhx-select__trigger");
          var phAttr = valueDisplay.getAttribute("data-rhx-placeholder");
          if (phAttr) {
            var span = document.createElement("span");
            span.className = "rhx-select__placeholder";
            span.textContent = phAttr;
            valueDisplay.appendChild(span);
          }
        }
      }

      // ── Hidden input management (multi-select) ──

      function addHiddenValue(value) {
        var container = getValuesContainer();
        if (!container) return;
        var name = container.getAttribute("data-name") || "";
        var input = document.createElement("input");
        input.type = "hidden";
        input.name = name;
        input.value = value;
        container.appendChild(input);
      }

      function removeHiddenValue(value) {
        var container = getValuesContainer();
        if (!container) return;
        var inputs = container.querySelectorAll("input");
        for (var i = 0; i < inputs.length; i++) {
          if (inputs[i].value === value) {
            inputs[i].remove();
            break;
          }
        }
      }

      // ── Clear ──

      function clearSelection() {
        if (isMultiple) {
          var options = listbox.querySelectorAll(".rhx-select__option--selected");
          options.forEach(function (opt) {
            opt.classList.remove("rhx-select__option--selected");
            opt.setAttribute("aria-selected", "false");
          });
          valueDisplay.innerHTML = "";
          var container = getValuesContainer();
          if (container) container.innerHTML = "";
        } else {
          var prev = listbox.querySelector(".rhx-select__option--selected");
          if (prev) {
            prev.classList.remove("rhx-select__option--selected");
            prev.setAttribute("aria-selected", "false");
          }
          valueDisplay.innerHTML = "";
          var hidden = getHiddenInput();
          if (hidden) {
            hidden.value = "";
            hidden.dispatchEvent(new Event("input", { bubbles: true }));
            hidden.dispatchEvent(new Event("change", { bubbles: true }));
          }
        }

        if (clearBtn) clearBtn.hidden = true;

        select.dispatchEvent(new CustomEvent("rhx:select:change", {
          bubbles: true,
          detail: { value: isMultiple ? [] : "" }
        }));
      }

      // ── Type-ahead ──

      function handleTypeAhead(char) {
        clearTimeout(typeAheadTimer);
        typeAheadBuffer += char.toLowerCase();
        typeAheadTimer = setTimeout(function () { typeAheadBuffer = ""; }, 500);

        var options = getOptions();
        for (var i = 0; i < options.length; i++) {
          var text = options[i].textContent.trim().toLowerCase();
          if (text.startsWith(typeAheadBuffer)) {
            setFocused(i, options);
            if (!isOpen()) {
              selectOption(options[i]);
            }
            break;
          }
        }
      }

      // ── Event handlers ──

      trigger.addEventListener("click", function (e) {
        e.preventDefault();
        toggle();
      });

      trigger.addEventListener("keydown", function (e) {
        var options = getOptions();

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
          case " ":
            e.preventDefault();
            if (isOpen() && focusedIdx >= 0 && focusedIdx < options.length) {
              selectOption(options[focusedIdx]);
            } else {
              toggle();
            }
            break;

          case "Escape":
            if (isOpen()) {
              e.preventDefault();
              close();
            }
            break;

          case "Home":
            e.preventDefault();
            if (isOpen() && options.length) setFocused(0, options);
            break;

          case "End":
            e.preventDefault();
            if (isOpen() && options.length) setFocused(options.length - 1, options);
            break;

          case "Tab":
            if (isOpen()) close();
            break;

          default:
            if (e.key.length === 1 && !e.ctrlKey && !e.metaKey) {
              handleTypeAhead(e.key);
            }
            break;
        }
      });

      // ── Option click ──
      listbox.addEventListener("click", function (e) {
        var option = e.target.closest(OPTION_SELECTOR);
        if (option && listbox.contains(option)) {
          selectOption(option);
        }
      });

      // ── Clear button ──
      if (clearBtn) {
        clearBtn.addEventListener("click", function (e) {
          e.stopPropagation();
          clearSelection();
        });
      }

      // ── Click outside ──
      document.addEventListener("click", function (e) {
        if (isOpen() && !select.contains(e.target)) {
          close();
        }
      });
    });
  }

  if (window.RHX) {
    window.RHX.register("select", initSelects);
  }
})();
