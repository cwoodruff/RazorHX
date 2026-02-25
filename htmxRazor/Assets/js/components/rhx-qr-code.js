/**
 * htmxRazor QR Code
 * Lightweight QR code generator rendered to <canvas>.
 * Supports versions 1-10, byte mode, all EC levels (L/M/Q/H).
 * Re-renders on attribute changes via MutationObserver for htmx compatibility.
 */
(function () {
  "use strict";

  // ═══════════════════════════════════════════════
  //  GF(256) arithmetic — primitive polynomial 0x11D
  // ═══════════════════════════════════════════════

  var GF_EXP = new Uint8Array(512);
  var GF_LOG = new Uint8Array(256);
  (function () {
    var v = 1;
    for (var i = 0; i < 255; i++) {
      GF_EXP[i] = v;
      GF_LOG[v] = i;
      v = (v << 1) ^ (v & 128 ? 0x11d : 0);
    }
    for (var i = 255; i < 512; i++) GF_EXP[i] = GF_EXP[i - 255];
  })();

  function gfMul(a, b) {
    if (a === 0 || b === 0) return 0;
    return GF_EXP[GF_LOG[a] + GF_LOG[b]];
  }

  // ═══════════════════════════════════════════════
  //  Reed-Solomon error correction
  // ═══════════════════════════════════════════════

  function rsGenPoly(n) {
    var poly = [1];
    for (var i = 0; i < n; i++) {
      var next = new Array(poly.length + 1);
      for (var k = 0; k < next.length; k++) next[k] = 0;
      for (var j = 0; j < poly.length; j++) {
        next[j] ^= poly[j];
        next[j + 1] ^= gfMul(poly[j], GF_EXP[i]);
      }
      poly = next;
    }
    return poly;
  }

  function rsEncode(data, eccCount) {
    var gen = rsGenPoly(eccCount);
    var result = new Array(eccCount);
    for (var i = 0; i < eccCount; i++) result[i] = 0;
    for (var i = 0; i < data.length; i++) {
      var coef = data[i] ^ result[0];
      for (var j = 0; j < eccCount - 1; j++) result[j] = result[j + 1];
      result[eccCount - 1] = 0;
      if (coef !== 0) {
        for (var j = 0; j < eccCount; j++) {
          result[j] ^= gfMul(gen[j + 1], coef);
        }
      }
    }
    return result;
  }

  // ═══════════════════════════════════════════════
  //  QR code data tables
  // ═══════════════════════════════════════════════

  // Error correction params per version: [eccPerBlock, [[g1Count, g1DataCW], [g2Count, g2DataCW]?]]
  var EC = [
    null,
    /* v1  */ { L: [7, [[1, 19]]], M: [10, [[1, 16]]], Q: [13, [[1, 13]]], H: [17, [[1, 9]]] },
    /* v2  */ { L: [10, [[1, 34]]], M: [16, [[1, 28]]], Q: [22, [[1, 22]]], H: [28, [[1, 16]]] },
    /* v3  */ { L: [15, [[1, 55]]], M: [26, [[1, 44]]], Q: [18, [[2, 17]]], H: [22, [[2, 13]]] },
    /* v4  */ { L: [20, [[1, 80]]], M: [18, [[2, 32]]], Q: [26, [[2, 24]]], H: [16, [[4, 9]]] },
    /* v5  */ { L: [26, [[1, 108]]], M: [24, [[2, 43]]], Q: [18, [[2, 15], [2, 16]]], H: [22, [[2, 11], [2, 12]]] },
    /* v6  */ { L: [18, [[2, 68]]], M: [16, [[4, 27]]], Q: [24, [[4, 19]]], H: [28, [[4, 15]]] },
    /* v7  */ { L: [20, [[2, 78]]], M: [18, [[4, 31]]], Q: [18, [[2, 14], [4, 15]]], H: [26, [[4, 13], [1, 14]]] },
    /* v8  */ { L: [24, [[2, 97]]], M: [22, [[2, 38], [2, 39]]], Q: [22, [[4, 18], [2, 19]]], H: [26, [[4, 14], [2, 15]]] },
    /* v9  */ { L: [30, [[2, 116]]], M: [22, [[3, 36], [2, 37]]], Q: [20, [[4, 16], [4, 17]]], H: [24, [[4, 12], [4, 13]]] },
    /* v10 */ { L: [18, [[2, 68], [2, 69]]], M: [26, [[4, 43], [1, 44]]], Q: [24, [[6, 19], [2, 20]]], H: [28, [[6, 15], [2, 16]]] }
  ];

  // Alignment pattern center positions per version
  var ALIGN = [
    null, [], [6, 18], [6, 22], [6, 26], [6, 30],
    [6, 34], [6, 22, 38], [6, 24, 42], [6, 26, 46], [6, 28, 52]
  ];

  // Remainder bits per version (v1-10)
  var REMAINDER = [0, 0, 7, 7, 7, 7, 7, 0, 0, 0, 0];

  // EC level indicator bits: L=01, M=00, Q=11, H=10
  var EC_INDICATOR = { L: 1, M: 0, Q: 3, H: 2 };

  // ═══════════════════════════════════════════════
  //  Data encoding (byte mode)
  // ═══════════════════════════════════════════════

  function textToBytes(text) {
    var bytes = [];
    for (var i = 0; i < text.length; i++) {
      var c = text.charCodeAt(i);
      if (c < 0x80) {
        bytes.push(c);
      } else if (c < 0x800) {
        bytes.push(0xc0 | (c >> 6), 0x80 | (c & 0x3f));
      } else if (c >= 0xd800 && c < 0xdc00 && i + 1 < text.length) {
        var lo = text.charCodeAt(++i);
        var cp = 0x10000 + ((c - 0xd800) << 10) + (lo - 0xdc00);
        bytes.push(0xf0 | (cp >> 18), 0x80 | ((cp >> 12) & 0x3f),
          0x80 | ((cp >> 6) & 0x3f), 0x80 | (cp & 0x3f));
      } else {
        bytes.push(0xe0 | (c >> 12), 0x80 | ((c >> 6) & 0x3f), 0x80 | (c & 0x3f));
      }
    }
    return bytes;
  }

  function getDataCapacity(version, ecLevel) {
    var ec = EC[version][ecLevel];
    var groups = ec[1];
    var total = 0;
    for (var g = 0; g < groups.length; g++) total += groups[g][0] * groups[g][1];
    return total;
  }

  function getMinVersion(dataLen, ecLevel) {
    for (var v = 1; v <= 10; v++) {
      var capacity = getDataCapacity(v, ecLevel);
      var countBits = v <= 9 ? 8 : 16;
      var maxChars = Math.floor((capacity * 8 - 4 - countBits) / 8);
      if (dataLen <= maxChars) return v;
    }
    return -1;
  }

  function createDataBits(data, version, ecLevel) {
    var capacity = getDataCapacity(version, ecLevel);
    var countBits = version <= 9 ? 8 : 16;
    var bits = [];

    // Mode indicator: byte mode = 0100
    bits.push(0, 1, 0, 0);

    // Character count
    for (var i = countBits - 1; i >= 0; i--)
      bits.push((data.length >> i) & 1);

    // Data bytes
    for (var i = 0; i < data.length; i++)
      for (var b = 7; b >= 0; b--)
        bits.push((data[i] >> b) & 1);

    // Terminator (up to 4 zero bits)
    var totalBits = capacity * 8;
    var termLen = Math.min(4, totalBits - bits.length);
    for (var i = 0; i < termLen; i++) bits.push(0);

    // Pad to byte boundary
    while (bits.length % 8 !== 0) bits.push(0);

    // Pad codewords (236, 17 alternating)
    var padBytes = [236, 17];
    var padIdx = 0;
    while (bits.length < totalBits) {
      var pb = padBytes[padIdx % 2];
      for (var b = 7; b >= 0; b--) bits.push((pb >> b) & 1);
      padIdx++;
    }

    // Convert bits to codewords
    var codewords = [];
    for (var i = 0; i < bits.length; i += 8) {
      var val = 0;
      for (var b = 0; b < 8; b++) val = (val << 1) | (bits[i + b] || 0);
      codewords.push(val);
    }
    return codewords;
  }

  // ═══════════════════════════════════════════════
  //  Block splitting, EC generation, interleaving
  // ═══════════════════════════════════════════════

  function createBlocks(dataCW, version, ecLevel) {
    var ec = EC[version][ecLevel];
    var eccPerBlock = ec[0];
    var groups = ec[1];
    var blocks = [];
    var offset = 0;

    for (var g = 0; g < groups.length; g++) {
      var count = groups[g][0];
      var dcw = groups[g][1];
      for (var b = 0; b < count; b++) {
        var blockData = dataCW.slice(offset, offset + dcw);
        var blockEcc = rsEncode(blockData, eccPerBlock);
        blocks.push({ data: blockData, ecc: blockEcc });
        offset += dcw;
      }
    }
    return { blocks: blocks, eccPerBlock: eccPerBlock };
  }

  function interleave(blocks, eccPerBlock) {
    var result = [];
    var maxDataLen = 0;
    for (var i = 0; i < blocks.length; i++)
      if (blocks[i].data.length > maxDataLen) maxDataLen = blocks[i].data.length;

    // Interleave data codewords
    for (var i = 0; i < maxDataLen; i++)
      for (var b = 0; b < blocks.length; b++)
        if (i < blocks[b].data.length) result.push(blocks[b].data[i]);

    // Interleave EC codewords
    for (var i = 0; i < eccPerBlock; i++)
      for (var b = 0; b < blocks.length; b++)
        result.push(blocks[b].ecc[i]);

    return result;
  }

  function codewordsToBits(codewords, version) {
    var bits = [];
    for (var i = 0; i < codewords.length; i++)
      for (var b = 7; b >= 0; b--)
        bits.push((codewords[i] >> b) & 1);

    // Remainder bits
    var rem = REMAINDER[version];
    for (var i = 0; i < rem; i++) bits.push(0);

    return bits;
  }

  // ═══════════════════════════════════════════════
  //  Matrix construction
  // ═══════════════════════════════════════════════

  function createMatrix(size) {
    var modules = [];
    var isFunc = [];
    for (var r = 0; r < size; r++) {
      modules[r] = new Array(size);
      isFunc[r] = new Array(size);
      for (var c = 0; c < size; c++) {
        modules[r][c] = false;
        isFunc[r][c] = false;
      }
    }
    return { modules: modules, isFunc: isFunc, size: size };
  }

  function setModule(m, row, col, dark, isFunction) {
    if (row >= 0 && row < m.size && col >= 0 && col < m.size) {
      m.modules[row][col] = dark;
      if (isFunction) m.isFunc[row][col] = true;
    }
  }

  function placeFinderPattern(m, row, col) {
    for (var dr = -1; dr <= 7; dr++) {
      for (var dc = -1; dc <= 7; dc++) {
        var r = row + dr, c = col + dc;
        if (r < 0 || r >= m.size || c < 0 || c >= m.size) continue;
        var dark =
          (dr >= 0 && dr <= 6 && (dc === 0 || dc === 6)) ||
          (dc >= 0 && dc <= 6 && (dr === 0 || dr === 6)) ||
          (dr >= 2 && dr <= 4 && dc >= 2 && dc <= 4);
        setModule(m, r, c, dark, true);
      }
    }
  }

  function placeAlignmentPattern(m, row, col) {
    for (var dr = -2; dr <= 2; dr++) {
      for (var dc = -2; dc <= 2; dc++) {
        var dark = Math.abs(dr) === 2 || Math.abs(dc) === 2 || (dr === 0 && dc === 0);
        setModule(m, row + dr, col + dc, dark, true);
      }
    }
  }

  function placeFunctionPatterns(m, version) {
    var size = m.size;

    // Finder patterns + separators
    placeFinderPattern(m, 0, 0);
    placeFinderPattern(m, 0, size - 7);
    placeFinderPattern(m, size - 7, 0);

    // Timing patterns
    for (var i = 8; i < size - 8; i++) {
      setModule(m, 6, i, i % 2 === 0, true);
      setModule(m, i, 6, i % 2 === 0, true);
    }

    // Alignment patterns
    var positions = ALIGN[version];
    if (positions.length > 0) {
      for (var i = 0; i < positions.length; i++) {
        for (var j = 0; j < positions.length; j++) {
          var r = positions[i], c = positions[j];
          // Skip if overlapping finder patterns
          if (r <= 8 && c <= 8) continue;
          if (r <= 8 && c >= size - 8) continue;
          if (r >= size - 8 && c <= 8) continue;
          placeAlignmentPattern(m, r, c);
        }
      }
    }

    // Dark module
    setModule(m, 4 * version + 9, 8, true, true);

    // Reserve format info areas
    for (var i = 0; i < 8; i++) {
      setModule(m, 8, i, false, true);
      setModule(m, 8, size - 1 - i, false, true);
      setModule(m, i, 8, false, true);
      setModule(m, size - 1 - i, 8, false, true);
    }
    setModule(m, 8, 8, false, true);

    // Reserve version info areas (v >= 7)
    if (version >= 7) {
      for (var i = 0; i < 6; i++) {
        for (var j = 0; j < 3; j++) {
          setModule(m, i, size - 11 + j, false, true);
          setModule(m, size - 11 + j, i, false, true);
        }
      }
    }
  }

  // ═══════════════════════════════════════════════
  //  Data placement (zigzag)
  // ═══════════════════════════════════════════════

  function placeData(m, bits) {
    var size = m.size;
    var bitIdx = 0;
    var upward = true;

    for (var right = size - 1; right >= 1; right -= 2) {
      if (right === 6) right = 5; // skip timing column

      for (var vert = 0; vert < size; vert++) {
        var row = upward ? (size - 1 - vert) : vert;

        for (var dx = 0; dx <= 1; dx++) {
          var col = right - dx;
          if (col < 0) continue;
          if (m.isFunc[row][col]) continue;

          m.modules[row][col] = bitIdx < bits.length ? bits[bitIdx] === 1 : false;
          bitIdx++;
        }
      }

      upward = !upward;
    }
  }

  // ═══════════════════════════════════════════════
  //  Masking
  // ═══════════════════════════════════════════════

  var MASK_FNS = [
    function (r, c) { return (r + c) % 2 === 0; },
    function (r, c) { return r % 2 === 0; },
    function (r, c) { return c % 3 === 0; },
    function (r, c) { return (r + c) % 3 === 0; },
    function (r, c) { return (Math.floor(r / 2) + Math.floor(c / 3)) % 2 === 0; },
    function (r, c) { return (r * c) % 2 + (r * c) % 3 === 0; },
    function (r, c) { return ((r * c) % 2 + (r * c) % 3) % 2 === 0; },
    function (r, c) { return ((r + c) % 2 + (r * c) % 3) % 2 === 0; }
  ];

  function applyMask(m, maskIdx) {
    var fn = MASK_FNS[maskIdx];
    for (var r = 0; r < m.size; r++)
      for (var c = 0; c < m.size; c++)
        if (!m.isFunc[r][c] && fn(r, c))
          m.modules[r][c] = !m.modules[r][c];
  }

  function calcPenalty(m) {
    var size = m.size;
    var penalty = 0;
    var mod = m.modules;

    // Rule 1: Runs of same color (rows and columns)
    for (var r = 0; r < size; r++) {
      var runLen = 1;
      for (var c = 1; c < size; c++) {
        if (mod[r][c] === mod[r][c - 1]) {
          runLen++;
        } else {
          if (runLen >= 5) penalty += runLen - 2;
          runLen = 1;
        }
      }
      if (runLen >= 5) penalty += runLen - 2;
    }
    for (var c = 0; c < size; c++) {
      var runLen = 1;
      for (var r = 1; r < size; r++) {
        if (mod[r][c] === mod[r - 1][c]) {
          runLen++;
        } else {
          if (runLen >= 5) penalty += runLen - 2;
          runLen = 1;
        }
      }
      if (runLen >= 5) penalty += runLen - 2;
    }

    // Rule 2: 2x2 blocks of same color
    for (var r = 0; r < size - 1; r++)
      for (var c = 0; c < size - 1; c++)
        if (mod[r][c] === mod[r][c + 1] &&
          mod[r][c] === mod[r + 1][c] &&
          mod[r][c] === mod[r + 1][c + 1])
          penalty += 3;

    // Rule 3: Finder-like patterns (1011101 0000 or 0000 1011101)
    for (var r = 0; r < size; r++) {
      for (var c = 0; c <= size - 11; c++) {
        if (mod[r][c] && !mod[r][c + 1] && mod[r][c + 2] && mod[r][c + 3] &&
          mod[r][c + 4] && !mod[r][c + 5] && mod[r][c + 6] &&
          !mod[r][c + 7] && !mod[r][c + 8] && !mod[r][c + 9] && !mod[r][c + 10])
          penalty += 40;
        if (!mod[r][c] && !mod[r][c + 1] && !mod[r][c + 2] && !mod[r][c + 3] &&
          mod[r][c + 4] && !mod[r][c + 5] && mod[r][c + 6] && mod[r][c + 7] &&
          mod[r][c + 8] && !mod[r][c + 9] && mod[r][c + 10])
          penalty += 40;
      }
    }
    for (var c = 0; c < size; c++) {
      for (var r = 0; r <= size - 11; r++) {
        if (mod[r][c] && !mod[r + 1][c] && mod[r + 2][c] && mod[r + 3][c] &&
          mod[r + 4][c] && !mod[r + 5][c] && mod[r + 6][c] &&
          !mod[r + 7][c] && !mod[r + 8][c] && !mod[r + 9][c] && !mod[r + 10][c])
          penalty += 40;
        if (!mod[r][c] && !mod[r + 1][c] && !mod[r + 2][c] && !mod[r + 3][c] &&
          mod[r + 4][c] && !mod[r + 5][c] && mod[r + 6][c] && mod[r + 7][c] &&
          mod[r + 8][c] && !mod[r + 9][c] && mod[r + 10][c])
          penalty += 40;
      }
    }

    // Rule 4: Dark module proportion
    var darkCount = 0;
    for (var r = 0; r < size; r++)
      for (var c = 0; c < size; c++)
        if (mod[r][c]) darkCount++;

    var ratio = darkCount * 100 / (size * size);
    penalty += Math.floor(Math.abs(ratio - 50) / 5) * 10;

    return penalty;
  }

  // ═══════════════════════════════════════════════
  //  Format & version information
  // ═══════════════════════════════════════════════

  function encodeFormatInfo(ecLevel, maskIdx) {
    var data = (EC_INDICATOR[ecLevel] << 3) | maskIdx;
    var rem = data;
    for (var i = 0; i < 10; i++)
      rem = (rem << 1) ^ ((rem >> 9) ? 0x537 : 0);
    var bits = ((data << 10) | rem) ^ 0x5412;
    return bits;
  }

  function placeFormatInfo(m, formatBits) {
    var size = m.size;

    // Around top-left finder
    for (var i = 0; i < 6; i++)
      m.modules[8][i] = ((formatBits >> (14 - i)) & 1) === 1;
    m.modules[8][7] = ((formatBits >> 8) & 1) === 1;
    m.modules[8][8] = ((formatBits >> 7) & 1) === 1;
    m.modules[7][8] = ((formatBits >> 6) & 1) === 1;
    for (var i = 0; i < 6; i++)
      m.modules[5 - i][8] = ((formatBits >> (5 - i)) & 1) === 1;

    // Around top-right and bottom-left finders
    for (var i = 0; i < 8; i++)
      m.modules[8][size - 1 - i] = ((formatBits >> i) & 1) === 1;
    for (var i = 0; i < 7; i++)
      m.modules[size - 1 - i][8] = ((formatBits >> (14 - i)) & 1) === 1;
  }

  function encodeVersionInfo(version) {
    var rem = version;
    for (var i = 0; i < 12; i++)
      rem = (rem << 1) ^ ((rem >> 11) ? 0x1f25 : 0);
    return (version << 12) | rem;
  }

  function placeVersionInfo(m, version) {
    if (version < 7) return;
    var versionBits = encodeVersionInfo(version);
    var size = m.size;

    for (var i = 0; i < 18; i++) {
      var bit = ((versionBits >> i) & 1) === 1;
      var row = Math.floor(i / 3);
      var col = size - 11 + (i % 3);
      m.modules[row][col] = bit;
      m.modules[col][row] = bit;
    }
  }

  // ═══════════════════════════════════════════════
  //  Main QR generation
  // ═══════════════════════════════════════════════

  function generateQR(text, ecLevel) {
    if (!text) return null;
    ecLevel = ecLevel || "M";

    var data = textToBytes(text);
    var version = getMinVersion(data.length, ecLevel);
    if (version < 0) return null;

    var dataCW = createDataBits(data, version, ecLevel);
    var blockInfo = createBlocks(dataCW, version, ecLevel);
    var interleavedCW = interleave(blockInfo.blocks, blockInfo.eccPerBlock);
    var bits = codewordsToBits(interleavedCW, version);

    var size = version * 4 + 17;
    var m = createMatrix(size);
    placeFunctionPatterns(m, version);
    placeData(m, bits);

    // Try all 8 masks, choose lowest penalty
    var bestMask = 0;
    var bestPenalty = Infinity;

    for (var mask = 0; mask < 8; mask++) {
      // Clone modules for testing
      var testMod = [];
      for (var r = 0; r < size; r++) testMod[r] = m.modules[r].slice();

      applyMask(m, mask);
      var formatBits = encodeFormatInfo(ecLevel, mask);
      placeFormatInfo(m, formatBits);
      placeVersionInfo(m, version);

      var penalty = calcPenalty(m);
      if (penalty < bestPenalty) {
        bestPenalty = penalty;
        bestMask = mask;
      }

      // Restore modules
      for (var r = 0; r < size; r++) m.modules[r] = testMod[r];
    }

    // Apply best mask
    applyMask(m, bestMask);
    var formatBits = encodeFormatInfo(ecLevel, bestMask);
    placeFormatInfo(m, formatBits);
    placeVersionInfo(m, version);

    return m.modules;
  }

  // ═══════════════════════════════════════════════
  //  Canvas rendering
  // ═══════════════════════════════════════════════

  function drawRoundedRect(ctx, x, y, w, h, r) {
    r = Math.min(r, w / 2, h / 2);
    ctx.beginPath();
    ctx.moveTo(x + r, y);
    ctx.lineTo(x + w - r, y);
    ctx.quadraticCurveTo(x + w, y, x + w, y + r);
    ctx.lineTo(x + w, y + h - r);
    ctx.quadraticCurveTo(x + w, y + h, x + w - r, y + h);
    ctx.lineTo(x + r, y + h);
    ctx.quadraticCurveTo(x, y + h, x, y + h - r);
    ctx.lineTo(x, y + r);
    ctx.quadraticCurveTo(x, y, x + r, y);
    ctx.closePath();
    ctx.fill();
  }

  function renderQR(canvas) {
    var value = canvas.getAttribute("data-rhx-qr-value") || "";
    var size = parseInt(canvas.getAttribute("data-rhx-qr-size"), 10) || 128;
    var fill = canvas.getAttribute("data-rhx-qr-fill") || "#000000";
    var bg = canvas.getAttribute("data-rhx-qr-background") || "#ffffff";
    var radius = parseFloat(canvas.getAttribute("data-rhx-qr-radius")) || 0;
    var ec = canvas.getAttribute("data-rhx-qr-ec") || "M";

    canvas.width = size;
    canvas.height = size;
    var ctx = canvas.getContext("2d");

    ctx.fillStyle = bg;
    ctx.fillRect(0, 0, size, size);

    if (!value) return;

    var modules = generateQR(value, ec);
    if (!modules) return;

    var moduleCount = modules.length;
    var cellSize = size / moduleCount;

    ctx.fillStyle = fill;
    for (var r = 0; r < moduleCount; r++) {
      for (var c = 0; c < moduleCount; c++) {
        if (modules[r][c]) {
          if (radius > 0) {
            drawRoundedRect(ctx, c * cellSize, r * cellSize, cellSize, cellSize, radius * cellSize);
          } else {
            ctx.fillRect(c * cellSize, r * cellSize, cellSize, cellSize);
          }
        }
      }
    }
  }

  // ═══════════════════════════════════════════════
  //  Component initialization
  // ═══════════════════════════════════════════════

  function initQrCodes(root) {
    var canvases = root.querySelectorAll("[data-rhx-qr-code]");
    canvases.forEach(function (canvas) {
      if (canvas._rhxQrInit) return;
      canvas._rhxQrInit = true;

      renderQR(canvas);

      // MutationObserver for attribute changes (htmx swap updates)
      var observer = new MutationObserver(function (mutations) {
        for (var i = 0; i < mutations.length; i++) {
          var attr = mutations[i].attributeName;
          if (attr && attr.indexOf("data-rhx-qr-") === 0) {
            renderQR(canvas);
            return;
          }
        }
      });

      observer.observe(canvas, { attributes: true });
    });
  }

  if (window.RHX) {
    window.RHX.register("qr-code", initQrCodes);
  }
})();
