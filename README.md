# MakaCoder
My repository for competitive programming.
<br>
<br>
[![Rating](https://badgen.org/img/atcoder/TrueRyoB/rating/algorithm?style=for-the-badge&label=Algorithm)](https://atcoder.jp/users/TrueRyoB?contestType=algo)

## Commands / コマンド

Bare commands, active only inside this repo (via a `chpwd` hook in `~/.zshrc`).
このリポジトリ内でのみ有効なコマンドです（`~/.zshrc` の `chpwd` フックで有効化）。

| Command | English | 日本語 |
| --- | --- | --- |
| `rst` | Reset `main.cs` from `active/template.cs`. | `active/template.cs` から `main.cs` を初期化。 |
| `run` | Build & run `main.cs` (`run <file>` reads stdin from a file). | `main.cs` をビルド＆実行（`run <file>` でファイルを標準入力に）。 |
| `wind` | Copy a submission-ready `main.cs` (only the used library) to the clipboard. | 使用中のライブラリだけを含む提出用 `main.cs` をクリップボードにコピー。 |
| `stress` | Random-test `main.cs` against a brute-force reference; print mismatching cases. | `main.cs` を総当たり解と乱数テストで比較し、不一致ケースを表示。 |

### `stress` details / 詳細

Write a generator and a reference solution, then run `stress`.
ジェネレータと参照解を用意して `stress` を実行します。

- `gen.cs` / `gen.py` — generator: takes a seed arg, prints one small-case input.
  ジェネレータ: シードを引数に取り、小さなケースの入力を 1 つ出力。
- `brute.cs` / `brute.py` — reference: reads that input, prints the correct answer.
  参照解: その入力を読み、正しい答えを出力。
- `main.cs` — your solution under test. あなたの検証対象の解。

`.cs` / `.py` are auto-detected per file. `.cs`/`.py` はファイルごとに自動判別。

| Usage | English | 日本語 |
| --- | --- | --- |
| `stress` | 300 cases, stop at first mismatch. | 300 ケース、最初の不一致で停止。 |
| `stress <N>` | Run `N` cases. | `N` ケース実行。 |
| `stress -k` | Keep going; list every mismatch. | 続行して全ての不一致を表示。 |
| `stress --init` | Scaffold starter `gen.cs` / `brute.cs`. | `gen.cs` / `brute.cs` の雛形を生成。 |

On mismatch it prints the seed, input, expected (brute) and got (`main.cs`).
不一致時はシード・入力・期待値（参照解）・実際の出力（`main.cs`）を表示します。

Build artifacts (`.stress/`) and `gen.*` / `brute.*` are git-ignored — testing never dirties git.
ビルド成果物（`.stress/`）と `gen.*` / `brute.*` は git 管理外 — テストで git が汚れません。
