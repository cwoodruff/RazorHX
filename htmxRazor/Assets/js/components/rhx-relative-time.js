/**
 * htmxRazor Relative Time — Auto-updater
 * Periodically refreshes relative time text for long-lived pages.
 */
(function () {
  "use strict";

  function updateAll(root) {
    var els = root.querySelectorAll("[data-rhx-relative-time]");
    els.forEach(function (el) {
      var iso = el.getAttribute("data-rhx-relative-time");
      var fmt = el.getAttribute("data-rhx-relative-format") || "long";
      var num = el.getAttribute("data-rhx-relative-numeric") || "always";
      var text = formatRelative(new Date(iso), new Date(), fmt, num);
      if (text) el.textContent = text;
    });
  }

  function formatRelative(date, now, format, numeric) {
    var diff = (now - date) / 1000;
    var isFuture = diff < 0;
    var abs = Math.abs(diff);

    if (abs < 10) {
      if (format === "narrow" || format === "short") return "now";
      return isFuture ? "in a moment" : "just now";
    }

    var value, unit, shortUnit;

    if (abs < 60) {
      value = Math.round(abs);
      unit = "second"; shortUnit = "s";
    } else if (abs < 3600) {
      value = Math.floor(abs / 60);
      unit = "minute"; shortUnit = "m";
    } else if (abs < 86400) {
      value = Math.floor(abs / 3600);
      unit = "hour"; shortUnit = "h";
    } else if (abs < 604800) {
      value = Math.floor(abs / 86400);
      unit = "day"; shortUnit = "d";
    } else if (abs < 2592000) {
      value = Math.floor(abs / 604800);
      unit = "week"; shortUnit = "w";
    } else if (abs < 31536000) {
      value = Math.floor(abs / 2592000);
      unit = "month"; shortUnit = "mo";
    } else {
      value = Math.floor(abs / 31536000);
      unit = "year"; shortUnit = "y";
    }

    if (numeric === "auto" && value === 1) {
      if (unit === "day") return isFuture ? "tomorrow" : "yesterday";
      if (format === "long") {
        if (unit === "week") return isFuture ? "next week" : "last week";
        if (unit === "month") return isFuture ? "next month" : "last month";
        if (unit === "year") return isFuture ? "next year" : "last year";
      }
    }

    if (format === "narrow") return value + shortUnit;
    if (format === "short") {
      return isFuture ? "in " + value + shortUnit : value + shortUnit + " ago";
    }

    var plural = value !== 1 ? "s" : "";
    if (isFuture) return "in " + value + " " + unit + plural;
    return value + " " + unit + plural + " ago";
  }

  // Update every 30 seconds
  setInterval(function () {
    updateAll(document);
  }, 30000);

  // Auto-init
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", function () {
      updateAll(document);
    });
  } else {
    updateAll(document);
  }

  // htmx support
  document.addEventListener("htmx:afterSettle", function (e) {
    updateAll(e.detail.elt);
  });
})();
