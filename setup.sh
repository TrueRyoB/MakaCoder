#!/usr/bin/env bash
#
# setup.sh — enable the bare `rst` / `run` / `wind` / `stress` commands for this repo.
#
# The commands are plain executables in this repo's root, but they're only convenient
# if you can type them without a `./` prefix while inside the repo. This script installs
# a small zsh `chpwd` hook into ~/.zshrc that defines them as shell functions whenever
# your working directory is inside this repo, and unsets them when you leave.
#
# Safe to re-run: it replaces its own marker-delimited block instead of appending a new
# copy. To add a future command, edit the COMMANDS list below and re-run `./setup.sh`.
#
# Usage:
#   ./setup.sh            # install/refresh the block in ~/.zshrc
#   then: source ~/.zshrc # (or open a new shell)

set -euo pipefail

# Absolute path to this repo (the directory this script lives in).
REPO="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Commands to expose. Each must be an executable file at the repo root.
COMMANDS=(rst run wind stress)

RC="$HOME/.zshrc"
BEGIN="# >>> abc competitive-programming commands >>>"
END="# <<< abc competitive-programming commands <<<"

# Build the function/unset lines from COMMANDS so the two stay in sync.
defs=""
names=""
for c in "${COMMANDS[@]}"; do
  defs+="    ${c}() { \"\$_abc_repo\"/${c} \"\$@\"; }"$'\n'
  names+="${c} "
done

block="$BEGIN
_abc_repo=\"$REPO\"
_abc_sync_cmds() {
  local top; top=\$(git -C \"\$PWD\" rev-parse --show-toplevel 2>/dev/null)
  if [[ -n \"\$top\" && \"\$top\" -ef \"\$_abc_repo\" ]]; then
$defs  else
    unset -f ${names% } 2>/dev/null
  fi
}
autoload -Uz add-zsh-hook && add-zsh-hook chpwd _abc_sync_cmds && _abc_sync_cmds
$END"

# Remove any previous block, then append the fresh one.
if [[ -f "$RC" ]] && grep -qF "$BEGIN" "$RC"; then
  tmp="$(mktemp)"
  awk -v b="$BEGIN" -v e="$END" '
    $0 == b {skip=1}
    skip && $0 == e {skip=0; next}
    !skip {print}
  ' "$RC" > "$tmp"
  mv "$tmp" "$RC"
fi

printf '\n%s\n' "$block" >> "$RC"

echo "Installed abc commands (${COMMANDS[*]}) into $RC"
echo "Run:  source ~/.zshrc   (or open a new terminal)"
