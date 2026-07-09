#!/usr/bin/env python3
"""wind.py -- method-level tree-shaker for the AtCoder C# single-file solution.

Reads a solution file (main.cs) that consists of a top-level "solution" section
followed by a big library of top-level type declarations, and emits a version
containing only the library that is actually referenced by the solution.

Static "factory bag" classes (Nms, Graph, Sugaku, ...) are shaken at *method*
granularity so that, e.g., calling Nms.Fenwick(...) pulls in FenwickTree but not
the entire library.

Reachability is name-based and deliberately conservative: it may keep a few
extra overloads/members, but it never drops something that is in use, so the
output always compiles.

Two identifier positions are distinguished to keep false positives down:
  * "bare"  - identifier NOT immediately preceded by '.'  (type position:
              `new Foo()`, `Foo.Bar`, `(Foo)`, `: Foo`, `<Foo>` ...).
  * "dot"   - identifier immediately preceded by '.'       (member access:
              `Nms.Fenwick`, `set.Count`).
A top-level *type* is pulled in only when its name appears "bare" (a type name
never appears as `.Type` here), so `set.Count` no longer drags in `class Count`.
A util-class *member* is pulled in when its name appears in either position.

Usage:  python3 wind.py main.cs      # pruned source -> stdout, summary -> stderr
"""

import re
import sys

# Static classes that get split at member granularity. Every other top-level
# type is included/excluded as a whole unit.
MEMBER_LEVEL_CLASSES = {"Nms", "StringExt", "JaggedArrayExt", "Graph", "Sugaku"}

IDENT_RE = re.compile(r"[A-Za-z_][A-Za-z0-9_]*")

# A top-level (column 0) type declaration.
TYPE_DECL_RE = re.compile(
    r"^(?:(?:public|internal|private|protected|sealed|static|abstract|partial|"
    r"readonly|file|unsafe|ref)\s+)*"
    r"(?:class|struct|interface|enum|record)\s+"
    r"([A-Za-z_][A-Za-z0-9_]*)"
)


def blank_literals_and_comments(src: str) -> str:
    """Return a copy of src with string/char literals and comments replaced by
    same-length runs of spaces (newlines preserved), so brace counting on the
    result is not confused by braces that live inside literals or comments.
    """
    out = []
    i = 0
    n = len(src)

    def emit_blank(s: str):
        out.append("".join("\n" if c == "\n" else " " for c in s))

    while i < n:
        c = src[i]
        two = src[i:i + 2]

        if two == "//":
            j = src.find("\n", i)
            if j == -1:
                j = n
            emit_blank(src[i:j])
            i = j
            continue

        if two == "/*":
            j = src.find("*/", i + 2)
            j = n if j == -1 else j + 2
            emit_blank(src[i:j])
            i = j
            continue

        # verbatim / interpolated string prefixes: @, $@, @$, $
        if c in "@$":
            k = i
            prefix = ""
            while k < n and src[k] in "@$":
                prefix += src[k]
                k += 1
            if k < n and src[k] == '"':
                if "@" in prefix:  # verbatim: "" escapes a quote
                    j = k + 1
                    while j < n:
                        if src[j] == '"':
                            if src[j + 1:j + 2] == '"':
                                j += 2
                                continue
                            j += 1
                            break
                        j += 1
                    emit_blank(src[i:j])
                    i = j
                    continue
                else:  # $"..." non-verbatim -> fall through to regular string
                    out.append(prefix)
                    i = k
                    c = src[i]
            else:
                out.append(src[i:k])
                i = k
                continue

        if c == '"':
            j = i + 1
            while j < n:
                if src[j] == "\\":
                    j += 2
                    continue
                if src[j] == '"':
                    j += 1
                    break
                j += 1
            emit_blank(src[i:j])
            i = j
            continue

        if c == "'":
            j = i + 1
            while j < n:
                if src[j] == "\\":
                    j += 2
                    continue
                if src[j] == "'":
                    j += 1
                    break
                j += 1
            emit_blank(src[i:j])
            i = j
            continue

        out.append(c)
        i += 1

    return "".join(out)


def find_block_end(mask: str, open_brace_pos: int) -> int:
    """Index just past the '}' matching the '{' at open_brace_pos (mask has
    literals blanked)."""
    depth = 0
    i = open_brace_pos
    n = len(mask)
    while i < n:
        c = mask[i]
        if c == "{":
            depth += 1
        elif c == "}":
            depth -= 1
            if depth == 0:
                return i + 1
        i += 1
    return n


def scan_idents(mask_text: str):
    """Return (bare, dot): identifier tokens not preceded / preceded by '.'."""
    bare, dot = set(), set()
    for m in IDENT_RE.finditer(mask_text):
        s = m.start()
        if s > 0 and mask_text[s - 1] == ".":
            dot.add(m.group())
        else:
            bare.add(m.group())
    return bare, dot


def split_top_level(src: str, mask: str):
    """Return (header, [[name, block_text, start, end], ...])."""
    lines = src.splitlines(keepends=True)
    offsets = []
    off = 0
    for ln in lines:
        offsets.append(off)
        off += len(ln)
    mask_lines = mask.splitlines(keepends=True)

    blocks = []
    first_start = None
    li = 0
    while li < len(lines):
        m = TYPE_DECL_RE.match(mask_lines[li])
        if m:
            name = m.group(1)
            start = offsets[li]
            if first_start is None:
                first_start = start
            brace = mask.find("{", start)
            end = find_block_end(mask, brace)
            blocks.append([name, src[start:end], start, end])
            while li < len(lines) and offsets[li] < end:
                li += 1
        else:
            li += 1

    if first_start is None:
        return src, []
    return src[:first_start], blocks


def _member_name(seg_mask: str) -> str:
    """Declared name of a class member (method / property / field / const)."""
    # Drop attributes and array-rank brackets so their parens/brackets don't
    # confuse the return-type vs name split (e.g. [MethodImpl(...)] , int[]).
    clean = re.sub(r"\[[^\]]*\]", "", seg_mask)

    def first(ch_positions):
        vals = [p for p in ch_positions if p != -1]
        return min(vals) if vals else len(clean)

    paren = clean.find("(")
    brace = clean.find("{")
    arrow = clean.find("=>")
    eq = clean.find("=")
    semi = clean.find(";")
    cut = first([paren, brace, arrow, eq, semi])

    decl = clean[:cut]
    if paren != -1 and paren == cut:
        # method: strip the name's own generic parameter list, e.g. `Set<T>`
        decl = re.sub(r"<[^<>(){}]*>\s*$", "", decl)

    ids = IDENT_RE.findall(decl)
    return ids[-1] if ids else ""


def _make_member(seg: str, seg_mask: str):
    bare, dot = scan_idents(seg_mask)
    return {"name": _member_name(seg_mask), "text": seg, "bare": bare, "dot": dot}


def split_members(block_text: str, block_mask: str):
    """Split a static class body into (shell_open, members, shell_close)."""
    open_pos = block_mask.find("{")
    class_end = find_block_end(block_mask, open_pos)
    shell_open = block_text[:open_pos + 1]
    shell_close = block_text[class_end - 1:]

    body = block_text[open_pos + 1:class_end - 1]
    body_mask = block_mask[open_pos + 1:class_end - 1]

    members = []
    i = 0
    n = len(body)
    cur_start = 0
    while i < n:
        c = body_mask[i]
        if c == "{":
            end = find_block_end(body_mask, i)
            seg = body[cur_start:end]
            if seg.strip():
                members.append(_make_member(seg, body_mask[cur_start:end]))
            i = cur_start = end
            continue
        if c == ";":
            end = i + 1
            seg = body[cur_start:end]
            if seg.strip():
                members.append(_make_member(seg, body_mask[cur_start:end]))
            i = cur_start = end
            continue
        i += 1

    tail = body[cur_start:]
    if tail.strip():
        members.append(_make_member(tail, body_mask[cur_start:]))

    return shell_open, members, shell_close


def is_static_class(block_text: str) -> bool:
    first_line = block_text.lstrip().splitlines()[0] if block_text.strip() else ""
    return first_line.startswith("static class") or first_line.startswith("public static class")


def main():
    if len(sys.argv) < 2:
        sys.stderr.write("usage: wind.py <file.cs>\n")
        return 2
    with open(sys.argv[1], "r", encoding="utf-8") as f:
        src = f.read().replace("\r\n", "\n").replace("\r", "\n")

    mask = blank_literals_and_comments(src)
    header, blocks = split_top_level(src, mask)
    if not blocks:
        sys.stdout.write(src)
        sys.stderr.write("wind: no library types found; emitted source unchanged.\n")
        return 0

    header_mask = mask[:len(header)]

    units = []          # {kind, name, bare, dot, included, ...}
    block_infos = []    # parallel to blocks
    for (name, text, start, end) in blocks:
        bmask = mask[start:end]
        if name in MEMBER_LEVEL_CLASSES and is_static_class(text):
            shell_open, members, shell_close = split_members(text, bmask)
            block_infos.append({
                "name": name, "member_level": True,
                "shell_open": shell_open, "shell_close": shell_close,
                "members": members,
            })
            for mem in members:
                units.append({
                    "kind": "member", "class": name, "name": mem["name"],
                    "bare": mem["bare"], "dot": mem["dot"],
                    "included": False, "ref": mem,
                })
        else:
            bare, dot = scan_idents(bmask)
            block_infos.append({"name": name, "member_level": False, "text": text})
            units.append({
                "kind": "type", "name": name,
                "bare": bare, "dot": dot, "included": False,
            })

    # Reachability fixpoint.
    ref_bare, ref_dot = scan_idents(header_mask)
    changed = True
    while changed:
        changed = False
        for u in units:
            if u["included"]:
                continue
            if u["kind"] == "type":
                hit = u["name"] in ref_bare
            else:  # member: matched in either position
                hit = u["name"] in ref_dot or u["name"] in ref_bare
            if hit:
                u["included"] = True
                ref_bare |= u["bare"]
                ref_dot |= u["dot"]
                changed = True

    # Emit.
    included_member_ids = {id(u["ref"]) for u in units
                           if u["kind"] == "member" and u["included"]}
    included_type_names = {u["name"] for u in units
                           if u["kind"] == "type" and u["included"]}
    kept_types, dropped_types = [], []
    out = [header.rstrip("\n")]

    for info in block_infos:
        if info["member_level"]:
            kept = [m for m in info["members"] if id(m) in included_member_ids]
            if kept:
                kept_types.append(info["name"])
                out.append(info["shell_open"] + "".join(m["text"] for m in kept) + info["shell_close"])
            else:
                dropped_types.append(info["name"])
        else:
            if info["name"] in included_type_names:
                kept_types.append(info["name"])
                out.append(info["text"])
            else:
                dropped_types.append(info["name"])

    result = "\n\n".join(p for p in out if p.strip()) + "\n"
    sys.stdout.write(result)

    orig_bytes = len(src.encode("utf-8"))
    new_bytes = len(result.encode("utf-8"))
    sys.stderr.write(
        f"wind: kept {len(kept_types)} type(s): {', '.join(kept_types) or '(none)'}\n"
        f"wind: dropped {len(dropped_types)} type(s)\n"
        f"wind: {orig_bytes} -> {new_bytes} bytes "
        f"({100 * new_bytes // max(orig_bytes, 1)}% of original)\n"
    )
    return 0


if __name__ == "__main__":
    sys.exit(main())
