/* ================================================================
   htmxRazor Client-Side Validation
   Hooks into HTML5 Constraint Validation API on forms marked with
   data-rhx-validate. Provides instant feedback before htmx submit.
   ================================================================ */
(function () {
  'use strict';

  function initValidation(root) {
    root.querySelectorAll('form[data-rhx-validate]').forEach(function (form) {
      if (form._rhxValidationInit) return;
      form._rhxValidationInit = true;

      // Take over validation display
      form.setAttribute('novalidate', '');

      form.addEventListener('submit', function (e) {
        if (!validateForm(form)) {
          e.preventDefault();
          e.stopImmediatePropagation();
        }
      });

      // Live feedback on input
      form.addEventListener('input', function (e) {
        var input = e.target;
        if (!isFormControl(input)) return;
        if (input.checkValidity()) {
          clearFieldError(input, form);
        }
      });

      // Also validate on blur for fields not yet touched
      form.addEventListener('focusout', function (e) {
        var input = e.target;
        if (!isFormControl(input)) return;
        if (input.value && !input.checkValidity()) {
          showFieldError(input, form);
        }
      });
    });
  }

  function isFormControl(el) {
    return el && (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA' || el.tagName === 'SELECT');
  }

  function validateForm(form) {
    var valid = true;
    var firstInvalid = null;

    form.querySelectorAll('input, textarea, select').forEach(function (input) {
      if (input.type === 'hidden') return;

      if (!input.checkValidity()) {
        valid = false;
        showFieldError(input, form);
        if (!firstInvalid) firstInvalid = input;
      } else {
        clearFieldError(input, form);
      }
    });

    // Focus first invalid field
    if (firstInvalid) {
      firstInvalid.focus();
    }

    return valid;
  }

  function showFieldError(input, form) {
    var wrapper = findWrapper(input);
    var errorEl = findErrorElement(input, form);

    if (wrapper) {
      wrapper.setAttribute('data-rhx-invalid', 'true');
    }

    if (errorEl) {
      errorEl.textContent = input.validationMessage;
      errorEl.removeAttribute('hidden');
      errorEl.classList.add('rhx-validation-message--error');
    }
  }

  function clearFieldError(input, form) {
    var wrapper = findWrapper(input);
    var errorEl = findErrorElement(input, form);

    if (wrapper) {
      wrapper.removeAttribute('data-rhx-invalid');
    }

    if (errorEl) {
      errorEl.textContent = '';
      errorEl.setAttribute('hidden', '');
      errorEl.classList.remove('rhx-validation-message--error');
    }
  }

  function findWrapper(input) {
    // Walk up to find the htmxRazor form control wrapper
    var el = input.parentElement;
    while (el) {
      var cls = el.className || '';
      if (typeof cls === 'string' &&
          (cls.indexOf('rhx-input') !== -1 ||
           cls.indexOf('rhx-textarea') !== -1 ||
           cls.indexOf('rhx-select') !== -1 ||
           cls.indexOf('rhx-combobox') !== -1 ||
           cls.indexOf('rhx-checkbox') !== -1 ||
           cls.indexOf('rhx-switch') !== -1 ||
           cls.indexOf('rhx-radio-group') !== -1 ||
           cls.indexOf('rhx-slider') !== -1 ||
           cls.indexOf('rhx-number-input') !== -1 ||
           cls.indexOf('rhx-rating') !== -1 ||
           cls.indexOf('rhx-color-picker') !== -1 ||
           cls.indexOf('rhx-file-input') !== -1)) {
        return el;
      }
      el = el.parentElement;
    }
    return null;
  }

  function findErrorElement(input, form) {
    var name = input.name || input.id;
    if (!name) return null;

    // Try standalone validation message first
    var errorId = name.replace(/\./g, '_').replace(/\[/g, '_').replace(/\]/g, '_') + '-error';
    var el = form.querySelector('#' + CSS.escape(errorId));
    if (el) return el;

    // Try the __error element inside the wrapper
    var wrapper = findWrapper(input);
    if (wrapper) {
      return wrapper.querySelector('[class*="__error"]');
    }

    return null;
  }

  // Register with RHX for init + htmx re-init
  if (window.RHX && window.RHX.register) {
    RHX.register('validation', initValidation);
  } else {
    document.addEventListener('DOMContentLoaded', function () {
      initValidation(document);
    });
  }
})();
