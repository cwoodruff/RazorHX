/**
 * RazorHX File Input
 * Drag-and-drop zone highlighting, file list display, and size validation.
 */
(function () {
  "use strict";

  function formatFileSize(bytes) {
    if (bytes === 0) return "0 B";
    var units = ["B", "KB", "MB", "GB"];
    var i = Math.floor(Math.log(bytes) / Math.log(1024));
    i = Math.min(i, units.length - 1);
    return (bytes / Math.pow(1024, i)).toFixed(i === 0 ? 0 : 1) + " " + units[i];
  }

  function initFileInputs(root) {
    var fileInputs = root.querySelectorAll("[data-rhx-file-input]");
    fileInputs.forEach(function (fi) {
      if (fi._rhxFileInputInit) return;
      fi._rhxFileInputInit = true;

      var dropzone = fi.querySelector(".rhx-file-input__dropzone");
      var native = fi.querySelector(".rhx-file-input__native");
      var fileList = fi.querySelector(".rhx-file-input__file-list");
      var maxSize = parseInt(fi.getAttribute("data-rhx-max-size") || "0", 10);

      if (!native || !dropzone) return;

      // ── Drag-and-drop ──

      dropzone.addEventListener("dragenter", function (e) {
        e.preventDefault();
        dropzone.setAttribute("data-rhx-dragover", "");
      });

      dropzone.addEventListener("dragover", function (e) {
        e.preventDefault();
        dropzone.setAttribute("data-rhx-dragover", "");
      });

      dropzone.addEventListener("dragleave", function (e) {
        e.preventDefault();
        // Only remove if leaving the dropzone itself
        if (!dropzone.contains(e.relatedTarget)) {
          dropzone.removeAttribute("data-rhx-dragover");
        }
      });

      dropzone.addEventListener("drop", function (e) {
        e.preventDefault();
        dropzone.removeAttribute("data-rhx-dragover");

        if (e.dataTransfer && e.dataTransfer.files.length) {
          native.files = e.dataTransfer.files;
          updateFileList(e.dataTransfer.files);
          native.dispatchEvent(new Event("change", { bubbles: true }));
        }
      });

      // ── File selection ──

      native.addEventListener("change", function () {
        updateFileList(native.files);
      });

      function updateFileList(files) {
        if (!fileList) return;
        fileList.innerHTML = "";

        var validFiles = [];
        var hasRejected = false;

        for (var i = 0; i < files.length; i++) {
          var file = files[i];
          var item = document.createElement("div");
          item.className = "rhx-file-input__file-item";

          var nameSpan = document.createElement("span");
          nameSpan.className = "rhx-file-input__file-name";
          nameSpan.textContent = file.name;
          item.appendChild(nameSpan);

          var sizeSpan = document.createElement("span");
          sizeSpan.className = "rhx-file-input__file-size";
          sizeSpan.textContent = formatFileSize(file.size);
          item.appendChild(sizeSpan);

          // Size validation — reject files that exceed maxSize
          if (maxSize > 0 && file.size > maxSize) {
            hasRejected = true;
            item.className += " rhx-file-input__file-item--error";
            var errorSpan = document.createElement("span");
            errorSpan.className = "rhx-file-input__file-error";
            errorSpan.textContent = "Exceeds " + formatFileSize(maxSize);
            item.appendChild(errorSpan);
          } else {
            validFiles.push(file);
          }

          fileList.appendChild(item);
        }

        // Enforce max size by keeping only valid files in the input
        if (maxSize > 0 && hasRejected) {
          try {
            var dt = new DataTransfer();
            for (var j = 0; j < validFiles.length; j++) {
              dt.items.add(validFiles[j]);
            }
            native.files = dt.files;
          } catch (e) {
            // Fallback: clear input entirely if DataTransfer not supported
            if (validFiles.length === 0) {
              native.value = "";
            }
          }
        }

        fi.dispatchEvent(new CustomEvent("rhx:file-input:change", {
          bubbles: true,
          detail: { files: native.files }
        }));
      }
    });
  }

  // ── Registration ──
  if (typeof RHX !== 'undefined' && RHX.register) {
    RHX.register('file-input', initFileInputs);
  }

  // Auto-init
  function initAll() {
    initFileInputs(document);
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
      initFileInputs(el);
    }
  });
})();
