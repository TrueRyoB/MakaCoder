#!/usr/bin/env bash
# rst -- reset main.cs and gen.cs from active/template.cs (CRLF -> LF).
set -euo pipefail

repo="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
template="$repo/active/template.cs"

if [[ ! -f "$template" ]]; then
  echo "rst: template not found: $template" >&2
  exit 1
fi

# Copy, stripping any carriage returns so the targets stay LF-only.
for target in main.cs gen.cs; do
  tr -d '\r' < "$template" > "$repo/$target"
  echo "rst: $target reset from active/template.cs ($(wc -l < "$repo/$target" | tr -d ' ') lines, LF)"
done
