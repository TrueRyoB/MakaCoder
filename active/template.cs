using System.Text;using System.Numerics;using System.Runtime.CompilerServices;using System;using System.Collections.Generic;using System.Linq;using System.IO;using System.Security.Cryptography;using System.Buffers;using System.Diagnostics;
#nullable enable


var fs = new FastScanner();
var sb = new StringBuilder();








/*
AHC典型

ビームサーチ: 
上位の貪欲結果を幾つか保持する (幅 | 深さ)

zobrish hash: 
同型の離散集合が状態を圧迫するのを防ぐ (多様性)

山登り | 焼きなまし法: 
解を少しずつ変更するのを繰り返して改善
局所解防止のため、解が悪化する場合にも遷移 その頻度をゲーム中に下げ続ける (多様性)

モンテカルロ法: 
各有限操作の有効度を乱択上のシミュレーションで評価

*/


















































Console.Write(sb.ToString());
Console.Out.Flush();
Environment.Exit(0);


static class Nms
{
  public static T[] Array<T>(int n, Func<T> f)
  {
    var a = new T[n];
    for (int i = 0; i < n; i++) a[i] = f();
    return a;
  }
  public static T[] Array<T>(int n, Func<int, T> f)
  {
    var a = new T[n];
    for (int i = 0; i < n; i++) a[i] = f(i);
    return a;
  }
  public static T[] Array<T>(int n, T val)
  {
    var a = new T[n];
    for (int i = 0; i < n; i++) a[i] = val;
    return a;
  }
  public static T[] PrefixFold<T, M>(T[] v) where M : IMonoid<T>
  {
    var a = new T[v.Length];
    for (int i = 0; i < a.Length; i++) a[i] = M.Op(v[i], i > 0 ? a[i - 1] : M.Id);
    return a;
  }
  public static T[] PrefixFold<T>(T[] v, T ide, Func<T, T, T> op)
  {
    var a = new T[v.Length];
    for (int i = 0; i < a.Length; ++i) a[i] = op(v[i], i > 0 ? a[i - 1] : ide);
    return a;
  }
  public static T[] SuffixFold<T, M>(ReadOnlySpan<T> v) where M : IMonoid<T>
  {
    var a = new T[v.Length];
    for (int i = a.Length - 1; i >= 0; i--) a[i] = M.Op(v[i], i + 1 < a.Length ? a[i + 1] : M.Id);
    return a;
  }
  public static T[] SuffixFold<T>(T[] v, T ide, Func<T, T, T> op)
  {
    var a = new T[v.Length];
    for (int i = a.Length - 1; i >= 0; --i) a[i] = op(v[i], i + 1 < a.Length ? a[i + 1] : ide);
    return a;
  }
  public static T[][] Matrix<T>(int h, int w, Func<T> f)
  {
    var a = new T[h][];
    for (int i = 0; i < h; ++i)
    {
      a[i] = new T[w];
      for (int j = 0; j < w; ++j) a[i][j] = f();
    }
    return a;
  }
  public static T[][] Matrix<T>(int h, int w, Func<int, int, T> f)
  {
    var a = new T[h][];
    for (int i = 0; i < h; ++i)
    {
      a[i] = new T[w];
      for (int j = 0; j < w; ++j) a[i][j] = f(i, j);
    }
    return a;
  }
  public static T[][] Matrix<T>(int h, int w, T val)
  {
    var a = new T[h][];
    for (int i = 0; i < h; ++i)
    {
      a[i] = new T[w];
      for (int j = 0; j < w; ++j) a[i][j] = val;
    }
    return a;
  }
  public static string Boolean(bool a)
  {
    return a ? "Yes\n" : "No\n";
  }
  public static List<T>[] Graph<T>(int n)
  {
    var a = new List<T>[n];
    for (int i = 0; i < n; ++i) a[i] = [];
    return a;
  }
  public static List<int>[] Graph(int n)
    => Graph<int>(n);

  public static List<T>[] Graph<T>(int n, int m, FastScanner fs)
  {
    var g = Graph<T>(n);

    if (typeof(T) == typeof(int))
    {
      while (m-- > 0)
      {
        var (a, b) = fs.Int2();
        --a; --b;

        g[a].Add((T)(object)b);
        g[b].Add((T)(object)a);
      }
      return g;
    }

    if (typeof(T) == typeof((int to, long cost)))
    {
      while (m-- > 0)
      {
        var (a, b) = fs.Int2();
        var c = fs.Long();
        --a; --b;

        g[a].Add((T)(object)(b, c));
        g[b].Add((T)(object)(a, c));
      }
      return g;
    }

    throw new NotSupportedException($"Graph<{typeof(T).Name}> is not supported.");
  }

  public static List<T>[] DirectedGraph<T>(int n, int m, FastScanner fs)
  {
    var g = Graph<T>(n);

    if (typeof(T) == typeof(int))
    {
      while (m-- > 0)
      {
        var (a, b) = fs.Int2();
        --a; --b;

        g[a].Add((T)(object)b);
      }
      return g;
    }

    if (typeof(T) == typeof((int to, long cost)))
    {
      while (m-- > 0)
      {
        var (a, b) = fs.Int2();
        var c = fs.Long();
        --a; --b;

        g[a].Add((T)(object)(b, c));
      }
      return g;
    }

    throw new NotSupportedException($"DirectedGraph<{typeof(T).Name}> is not supported.");
  }

  public static int[] InOrder<T>(T[] a) where T : IComparable<T>
  {
    var sorted = a.ToArray();
    System.Array.Sort(sorted);

    var uniq = new List<T>(sorted.Length);
    foreach (var x in sorted)
    {
      if (uniq.Count == 0 || uniq[^1].CompareTo(x) != 0) uniq.Add(x);
    }

    var res = new int[a.Length];

    for (int i = 0; i < a.Length; ++i)
    {
      int l = -1, r = uniq.Count;
      while (l + 1 < r)
      {
        int m = l + (r - l) / 2;
        if (uniq[m].CompareTo(a[i]) < 0) l = m;
        else r = m;
      }
      res[i] = r;
    }
    return res;
  }

  public static T[] SubArray<T>(ReadOnlySpan<T> a, ReadOnlySpan<int> index)
  {
    foreach (var e in index) if (e < 0 || e >= a.Length) throw new Exception("Out of bounds.");

    int n = index.Length;
    var res = new T[n];
    for (int i = 0; i < n; ++i) res[i] = a[index[i]];
    return res;
  }
  public static void Permutations(int n, Action<ReadOnlySpan<int>> action)
  {
    if (n <= 0) throw new Exception("Invalid array length");
    if (n >= 12) throw new Exception("TLE alert");

    var a = new int[n];
    for (int i = 0; i < n; ++i) a[i] = i;

    while (true)
    {
      action(a.AsSpan());

      int i = n - 2;
      while (i >= 0 && a[i] >= a[i + 1]) --i;
      if (i < 0) return;

      int j = n - 1;
      while (a[i] >= a[j]) --j;

      (a[i], a[j]) = (a[j], a[i]);
      System.Array.Reverse(a, i + 1, n - i - 1);
    }
  }

  public static IEnumerable<int> Range(int n, bool foward)
  {
    if (foward) for (int i = 0; i < n; ++i) yield return i;
    else for (int i = n - 1; i >= 0; --i) yield return i;
  }

  public static Tensor<T> Tensor<T>(T val, params int[] shape)
    => new(val, shape);
  public static Tensor<T> Tensor<T>(Func<T> f, params int[] shape)
    => new(f, shape);
  public static Tensor<T> Tensor<T>(Func<int[], T> f, params int[] shape)
    => new(f, shape);
  public static StackArray<T> Stack<T>()
    => new StackArray<T>();
  public static Deque<T> Deque<T>()
    => new Deque<T>();
  public static TopKSet<T> TopKSet<T>(int k, IComparer<T>? comp = null)
    => new TopKSet<T>(k, comp);
  public static MatrixMod MatrixMod(int rows, int cols, long mod)
    => new MatrixMod(rows, cols, mod);
  public static Dinic MaxFlowSolver<T>(int n)
    => new Dinic(n);
  public static LazySegment<TNode, TLazy, TOp> LazySegmentTree<TNode, TLazy, TOp>(int n) where TOp : struct, ILazyOp<TNode, TLazy>
    => new LazySegment<TNode, TLazy, TOp>(n);
  public static LazySegment<TNode, TLazy, TOp> LazySegmentTree<TNode, TLazy, TOp>(IReadOnlyList<TNode> nodes) where TOp : struct, ILazyOp<TNode, TLazy>
    => new LazySegment<TNode, TLazy, TOp>(nodes);
  public static Segment<T> SegmentTree<T>(T identity, Func<T, T, T> op, int size, T val)
    => new Segment<T>(identity, op, size, val);
  public static Segment<T> SegmentTree<T>(T identity, Func<T, T, T> op, T[] data)
    => new Segment<T>(identity, op, data);
  public static UnionFind UnionFind(int n)
    => new UnionFind(n);
  public static AvlSet<T> Set<T>() where T : IComparable<T>
    => new AvlSet<T>();
  public static AvlSet<T> Set<T>(bool allowDuplicates) where T : IComparable<T>
    => new AvlSet<T>(allowDuplicates);
  public static AvlSet<T> Set<T>(params T[] vals) where T : IComparable<T>
    => new AvlSet<T>(vals);
  public static AvlSet<T> Set<T>(bool allowDuplicates, params T[] vals) where T : IComparable<T>
    => new AvlSet<T>(allowDuplicates, vals);
  public static System.Collections.Generic.Queue<T> Queue<T>()
    => new System.Collections.Generic.Queue<T>();
  public static IntervalSet IntervalSet()
    => new IntervalSet();
  public static WrappedDictionary<T, U> Dictionary<T, U>() where T : notnull
    => new WrappedDictionary<T, U>();
  public static PriorityQueue<T, U> PriorityQueue<T, U>()
    => new PriorityQueue<T, U>();
  public static RollingHashDeque<T> RollingHash<T>()
    => new RollingHashDeque<T>();
  public static RollingHashDeque<T> RollingHash<T>(T[] init)
    => new RollingHashDeque<T>(init);
  public static FenwickTree<T> Fenwick<T>(int n) where T : IBinaryInteger<T>
    => new FenwickTree<T>(n);
  public static XorShiftRandom Random(int seed) 
    => new XorShiftRandom(seed);
}

sealed class FenwickTree<T> where T : IBinaryInteger<T>
{
  private readonly int n;
  private readonly T[] data; // Fenwick
  private readonly T[] arr;  // 実値を保持

  public FenwickTree(int n)
  {
    this.n = n;
    data = new T[n + 1]; // 1-indexed
    arr = new T[n];
  }

  // A[k] += x
  public void Add(int k, T x)
  {
    arr[k] += x;
    for (int i = k + 1; i <= n; i += i & -i)
      data[i] += x;
  }

  // A[k] = x
  public void Set(int k, T x)
  {
    T diff = x - arr[k];
    arr[k] = x;
    for (int i = k + 1; i <= n; i += i & -i)
      data[i] += diff;
  }

  // [0, r]
  public T Sum(int r)
  {
    T res = T.Zero;
    for (int i = r + 1; i > 0; i -= i & -i)
      res += data[i];
    return res;
  }

  // [l, r)
  public T Sum(int l, int r)
  {
    return Sum(r + 1) - Sum(l);
  }

  public T this[int k]
  {
    get => arr[k];
    set => Set(k, value);
  }
}

public sealed class RollingHashDeque<T>
{
  private T[] _buf;
  private int _head;
  private int _count;

  // Shared per closed generic type, so hashes are comparable between instances.
  private static readonly ulong B1;
  private static readonly ulong B2;
  private static readonly ulong InvB1;
  private static readonly ulong InvB2;
  private static readonly ulong Salt1;
  private static readonly ulong Salt2;
  private static readonly ulong MixSalt;

  private ulong _h1;
  private ulong _h2;
  private ulong _pow1; // B1^Count
  private ulong _pow2; // B2^Count

  static RollingHashDeque()
  {
    ulong s = NextU64();

    B1 = RandomOddNotOne(ref s);
    B2 = RandomOddNotOne(ref s);
    while (B2 == B1) B2 = RandomOddNotOne(ref s);

    InvB1 = ModInversePow2(B1);
    InvB2 = ModInversePow2(B2);

    Salt1 = SplitMix64(ref s);
    Salt2 = SplitMix64(ref s);
    MixSalt = SplitMix64(ref s);
  }

  public RollingHashDeque()
  {
    _buf = new T[4];
    _head = 0;
    _count = 0;
    _h1 = 0;
    _h2 = 0;
    _pow1 = 1;
    _pow2 = 1;
  }

  public RollingHashDeque(T[] init) : this()
  {
    if (init == null) throw new ArgumentNullException(nameof(init));
    foreach (var v in init) PushBack(v);
  }

  public ulong Peek() => MixPair(_h1, _h2);

  public ulong Assign(T[] arr)
  {
    this.Clear();
    foreach (var e in arr) this.PushFront(e);
    return Peek();
  }

  public ulong PushFront(T val)
  {
    EnsureCapacity(_count + 1);

    if (--_head < 0) _head += _buf.Length;
    _buf[_head] = val;

    var (x1, x2) = Fingerprint(val);

    _h1 = unchecked(x1 * _pow1 + _h1);
    _h2 = unchecked(x2 * _pow2 + _h2);

    _pow1 = unchecked(_pow1 * B1);
    _pow2 = unchecked(_pow2 * B2);

    _count++;
    return Peek();
  }

  public ulong PushBack(T val)
  {
    EnsureCapacity(_count + 1);

    int tail = _head + _count;
    if (tail >= _buf.Length) tail -= _buf.Length;
    _buf[tail] = val;

    var (x1, x2) = Fingerprint(val);

    _h1 = unchecked(_h1 * B1 + x1);
    _h2 = unchecked(_h2 * B2 + x2);

    _pow1 = unchecked(_pow1 * B1);
    _pow2 = unchecked(_pow2 * B2);

    _count++;
    return Peek();
  }

  public ulong PopFront()
  {
    if (_count == 0) throw new InvalidOperationException("RollingHashDeque is empty.");

    T val = _buf[_head];
    var (x1, x2) = Fingerprint(val);

    ulong newPow1 = unchecked(_pow1 * InvB1);
    ulong newPow2 = unchecked(_pow2 * InvB2);

    _h1 = unchecked(_h1 - x1 * newPow1);
    _h2 = unchecked(_h2 - x2 * newPow2);

    _pow1 = newPow1;
    _pow2 = newPow2;

    if (++_head == _buf.Length) _head = 0;

    _count--;
    if (_count == 0)
    {
      _head = 0;
      _pow1 = 1;
      _pow2 = 1;
      _h1 = 0;
      _h2 = 0;
    }

    return Peek();
  }

  public ulong PopBack()
  {
    if (_count == 0) throw new InvalidOperationException("RollingHashDeque is empty.");

    int tail = _head + _count - 1;
    if (tail >= _buf.Length) tail -= _buf.Length;

    T val = _buf[tail];
    var (x1, x2) = Fingerprint(val);

    _h1 = unchecked((_h1 - x1) * InvB1);
    _h2 = unchecked((_h2 - x2) * InvB2);

    _pow1 = unchecked(_pow1 * InvB1);
    _pow2 = unchecked(_pow2 * InvB2);

    _count--;
    if (_count == 0)
    {
      _head = 0;
      _pow1 = 1;
      _pow2 = 1;
      _h1 = 0;
      _h2 = 0;
    }

    return Peek();
  }

  public int Count() => _count;

  public ulong Parse(T[] arr)
  {
    if (arr == null) throw new ArgumentNullException(nameof(arr));

    ulong h1 = 0, h2 = 0;
    foreach (var v in arr)
    {
      var (x1, x2) = Fingerprint(v);
      h1 = unchecked(h1 * B1 + x1);
      h2 = unchecked(h2 * B2 + x2);
    }
    return MixPair(h1, h2);
  }

  public void Clear()
  {
    _head = 0;
    _count = 0;
    _h1 = 0;
    _h2 = 0;
    _pow1 = 1;
    _pow2 = 1;
  }

  public T this[int k]
  {
    get
    {
      if ((uint)k >= (uint)_count) throw new IndexOutOfRangeException();
      int idx = _head + k;
      if (idx >= _buf.Length) idx -= _buf.Length;
      return _buf[idx];
    }
  }

  private void EnsureCapacity(int needed)
  {
    if (_buf.Length >= needed) return;

    int newCap = _buf.Length << 1;
    if (newCap < needed) newCap = needed;

    T[] nb = new T[newCap];

    if (_count > 0)
    {
      int right = Math.Min(_buf.Length - _head, _count);
      Array.Copy(_buf, _head, nb, 0, right);
      int left = _count - right;
      if (left > 0) Array.Copy(_buf, 0, nb, right, left);
    }

    _buf = nb;
    _head = 0;
  }

  private static (ulong, ulong) Fingerprint(T value)
  {
    ulong raw = Encode(value);
    return (Mix64(raw ^ Salt1), Mix64(raw ^ Salt2));
  }

  private static ulong Encode(T value)
  {
    object? o = value;
    if (o == null) return 0UL;

    return o switch
    {
      byte v => v,
      sbyte v => unchecked((ulong)(long)v),
      short v => unchecked((ulong)(long)v),
      ushort v => v,
      int v => unchecked((ulong)(long)v),
      uint v => v,
      long v => unchecked((ulong)v),
      ulong v => v,
      char v => v,
      bool v => v ? 1UL : 0UL,
      _ => unchecked((ulong)o.GetHashCode())
    };
  }

  private static ulong MixPair(ulong a, ulong b)
  {
    return Mix64(a ^ RotL(b, 23) ^ MixSalt);
  }

  private static ulong Mix64(ulong x)
  {
    unchecked
    {
      x ^= x >> 30;
      x *= 0xBF58476D1CE4E5B9UL;
      x ^= x >> 27;
      x *= 0x94D049BB133111EBUL;
      x ^= x >> 31;
      return x;
    }
  }

  private static ulong RotL(ulong x, int k)
      => (x << k) | (x >> (64 - k));

  private static ulong ModInversePow2(ulong x)
  {
    // x must be odd.
    unchecked
    {
      ulong inv = 1;
      for (int i = 0; i < 6; i++)
        inv *= 2 - x * inv;
      return inv;
    }
  }

  private static ulong RandomOddNotOne(ref ulong s)
  {
    ulong x;
    do
    {
      x = SplitMix64(ref s) | 1UL;
    } while (x == 1UL);
    return x;
  }

  private static ulong SplitMix64(ref ulong x)
  {
    unchecked
    {
      x += 0x9E3779B97F4A7C15UL;
      ulong z = x;
      z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
      z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
      return z ^ (z >> 31);
    }
  }

  private static ulong NextU64()
  {
    Span<byte> b = stackalloc byte[8];
    RandomNumberGenerator.Fill(b);
    return BitConverter.ToUInt64(b);
  }
}

sealed class WrappedDictionary<T, U> : IEnumerable<T> where T : notnull
{
  private readonly Dictionary<T, U> _dict;
  private readonly Func<U> _factory;

  public WrappedDictionary(Func<U>? factory = null, IEqualityComparer<T>? comparer = null)
  {
    _dict = new Dictionary<T, U>(comparer);
    _factory = factory ?? Activator.CreateInstance<U>;
  }

  public U this[T key]
  {
    get
    {
      if (!_dict.TryGetValue(key, out var value))
      {
        value = _factory();
        _dict[key] = value;
      }
      return value;
    }
    set
    {
      _dict[key] = value;
    }
  }

  public bool TryGetValue(T key, out U value)
      => _dict.TryGetValue(key, out value!);

  public bool ContainsKey(T key)
      => _dict.ContainsKey(key);

  public bool Remove(T key)
      => _dict.Remove(key);

  public void Clear()
      => _dict.Clear();

  public int Count => _dict.Count;

  public ICollection<T> Keys => _dict.Keys;
  public ICollection<U> Values => _dict.Values;

  public void Add(T key, U value)
      => _dict.Add(key, value);

  public U Ensure(T key)
  {
    if (!_dict.TryGetValue(key, out var value))
    {
      value = _factory();
      _dict[key] = value;
    }
    return value;
  }

  public IEnumerator<T> GetEnumerator()
      => _dict.Keys.GetEnumerator();

  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      => GetEnumerator();
}

class AvlSet<T> : IEnumerable<T> where T : IComparable<T>
{
  // ─────────────────────────────────────────────────────────────────────────
  // Node
  // ─────────────────────────────────────────────────────────────────────────
  private sealed class Node
  {
    public T Key;
    public int Count;   // multiplicity (always 1 when allowDuplicates=false)
    public int Size;    // subtree total element count (sum of multiplicities)
    public int Height;
    public Node? Left, Right;   // FIX CS8618: nullable

    public Node(T key)
    {
      Key = key;
      Count = 1;
      Size = 1;
      Height = 1;
    }
  }

  // ─────────────────────────────────────────────────────────────────────────
  // Fields
  // ─────────────────────────────────────────────────────────────────────────
  private Node? _root;            // FIX CS8618: nullable
  private readonly bool _allowDuplicates;

  // ─────────────────────────────────────────────────────────────────────────
  // Constructors
  // ─────────────────────────────────────────────────────────────────────────

  /// <summary>Create an empty set. Pass allowDuplicates=true for multiset behaviour.</summary>
  public AvlSet(bool allowDuplicates = false)
  {
    _allowDuplicates = allowDuplicates;
  }

  /// <summary>Create a set pre-loaded with the given values.</summary>
  public AvlSet(params T[] vals) : this(false)
  {
    foreach (var v in vals) Add(v);
  }

  /// <summary>Create a set/multiset pre-loaded with the given values.</summary>
  public AvlSet(bool allowDuplicates, params T[] vals) : this(allowDuplicates)
  {
    foreach (var v in vals) Add(v);
  }

  // ─────────────────────────────────────────────────────────────────────────
  // Properties
  // ─────────────────────────────────────────────────────────────────────────

  /// <summary>Total number of elements (counting duplicates in multiset mode).</summary>
  public int Count { get { return GetSize(_root); } }

  // ─────────────────────────────────────────────────────────────────────────
  // Public API
  // ─────────────────────────────────────────────────────────────────────────

  /// <summary>
  /// Add x to the set.
  /// Returns true if the element count increased
  /// (always true in multiset mode; false in set mode when x already exists).
  /// </summary>
  public bool Add(T x)
  {
    int before = Count;
    _root = Insert(_root, x);
    return Count > before;
  }

  /// <summary>
  /// Remove one occurrence of x.
  /// Returns true if an element was actually removed.
  /// </summary>
  public bool Remove(T x)
  {
    int before = Count;
    _root = Delete(_root, x);
    return Count < before;
  }

  /// <summary>Returns true if x is present in the set.</summary>
  public bool Contains(T x) { return FindNode(_root, x) != null; }

  // ── Bounds ───────────────────────────────────────────────────────────────

  /// <summary>
  /// Returns the smallest element &gt;= x (lower bound / ceiling).
  /// Throws InvalidOperationException if no such element exists.
  /// </summary>
  public T LowerBound(T x)
  {
    T val;
    if (TryLowerBound(x, out val!)) return val;
    throw new InvalidOperationException("No element >= " + x + ".");
  }

  /// <summary>Try version of LowerBound. Returns false if no element &gt;= x.</summary>
  public bool TryLowerBound(T x, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out T val)
  {
    Node? n = LowerBoundNode(_root, x);
    if (n == null) { val = default!; return false; }   // FIX CS8600/CS8603: default!
    val = n.Key;
    return true;
  }

  /// <summary>
  /// Returns the largest element &lt;= x (upper bound / floor).
  /// Throws InvalidOperationException if no such element exists.
  /// </summary>
  public T UpperBound(T x)
  {
    T val;
    if (TryUpperBound(x, out val!)) return val;
    throw new InvalidOperationException("No element <= " + x + ".");
  }

  /// <summary>Try version of UpperBound. Returns false if no element &lt;= x.</summary>
  public bool TryUpperBound(T x, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out T val)
  {
    Node? n = UpperBoundNode(_root, x);
    if (n == null) { val = default!; return false; }   // FIX CS8600/CS8603: default!
    val = n.Key;
    return true;
  }

  // ── Count queries ─────────────────────────────────────────────────────────

  /// <summary>Number of elements strictly less than x.</summary>
  public int CountLessThan(T x) { return CountLess(_root, x); }

  /// <summary>Number of elements strictly greater than x.</summary>
  public int CountMoreThan(T x) { return Count - CountLessOrEqual(_root, x); }

  // ── Order statistics ──────────────────────────────────────────────────────

  /// <summary>
  /// Returns the k-th smallest element (0-indexed, multiplicities counted).
  /// Throws ArgumentOutOfRangeException if k is out of range.
  /// </summary>
  public T ElementAt(int k)
  {
    if (k < 0 || k >= Count)
      throw new ArgumentOutOfRangeException("k",
          "Index " + k + " is out of range [0, " + (Count - 1) + "].");
    return KthSmallest(_root, k);
  }

  // ── Min / Max ─────────────────────────────────────────────────────────────

  /// <summary>Returns the smallest element. Throws if the set is empty.</summary>
  public T PeekMin()
  {
    EnsureNotEmpty();
    return NodeMin(_root!).Key;
  }

  /// <summary>Returns the largest element. Throws if the set is empty.</summary>
  public T PeekMax()
  {
    EnsureNotEmpty();
    return NodeMax(_root!).Key;
  }

  /// <summary>Removes and returns the smallest element. Throws if the set is empty.</summary>
  public T PopMin()
  {
    EnsureNotEmpty();
    T val = NodeMin(_root!).Key;
    Remove(val);
    return val;
  }

  /// <summary>Removes and returns the largest element. Throws if the set is empty.</summary>
  public T PopMax()
  {
    EnsureNotEmpty();
    T val = NodeMax(_root!).Key;
    Remove(val);
    return val;
  }

  // ── Misc ──────────────────────────────────────────────────────────────────

  /// <summary>Removes all elements from the set.</summary>
  public void Clear() { _root = null; }

  /// <summary>Returns a sorted string representation: {a, b, c, ...}</summary>
  public override string ToString()
  {
    var sb = new StringBuilder("{");
    bool first = true;
    foreach (T item in this)
    {
      if (!first) sb.Append(", ");
      sb.Append(item);
      first = false;
    }
    sb.Append('}');
    return sb.ToString();
  }

  // ─────────────────────────────────────────────────────────────────────────
  // IEnumerable<T>  –  in-order (ascending) traversal
  // ─────────────────────────────────────────────────────────────────────────

  /// <summary>Enumerates elements in ascending order (duplicates repeated in multiset mode).</summary>
  public IEnumerator<T> GetEnumerator() { return InOrder(_root).GetEnumerator(); }

  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

  private IEnumerable<T> InOrder(Node? node)
  {
    if (node == null) yield break;
    foreach (T v in InOrder(node.Left)) yield return v;
    for (int i = 0; i < node.Count; i++) yield return node.Key;
    foreach (T v in InOrder(node.Right)) yield return v;
  }

  // ─────────────────────────────────────────────────────────────────────────
  // AVL tree internals
  // ─────────────────────────────────────────────────────────────────────────

  private static int GetHeight(Node? n) { return n == null ? 0 : n.Height; }
  private static int GetSize(Node? n) { return n == null ? 0 : n.Size; }

  private static void Pull(Node n)
  {
    n.Height = 1 + Math.Max(GetHeight(n.Left), GetHeight(n.Right));
    n.Size = GetSize(n.Left) + n.Count + GetSize(n.Right);
  }

  private static int Bf(Node n) { return GetHeight(n.Left) - GetHeight(n.Right); }

  private static Node RotL(Node x)
  {
    Node y = x.Right!;   // caller guarantees Right is non-null
    x.Right = y.Left;
    y.Left = x;
    Pull(x);
    Pull(y);
    return y;
  }

  private static Node RotR(Node y)
  {
    Node x = y.Left!;    // caller guarantees Left is non-null
    y.Left = x.Right;
    x.Right = y;
    Pull(y);
    Pull(x);
    return x;
  }

  private static Node Balance(Node n)
  {
    Pull(n);
    int bf = Bf(n);
    if (bf > 1)
    {
      if (Bf(n.Left!) < 0) n.Left = RotL(n.Left!);
      return RotR(n);
    }
    if (bf < -1)
    {
      if (Bf(n.Right!) > 0) n.Right = RotR(n.Right!);
      return RotL(n);
    }
    return n;
  }

  // ── Insert ────────────────────────────────────────────────────────────────

  private Node Insert(Node? n, T key)
  {
    if (n == null) return new Node(key);
    int cmp = key.CompareTo(n.Key);
    if (cmp == 0)
    {
      if (_allowDuplicates) { n.Count++; n.Size++; }
      return n;
    }
    if (cmp < 0) n.Left = Insert(n.Left, key);
    else n.Right = Insert(n.Right, key);
    return Balance(n);
  }

  // ── Delete ────────────────────────────────────────────────────────────────

  private Node? Delete(Node? n, T key)
  {
    if (n == null) return null;
    int cmp = key.CompareTo(n.Key);
    if (cmp < 0)
    {
      n.Left = Delete(n.Left, key);
    }
    else if (cmp > 0)
    {
      n.Right = Delete(n.Right, key);
    }
    else
    {
      // Found
      if (n.Count > 1) { n.Count--; n.Size--; return n; }
      if (n.Left == null) return n.Right;
      if (n.Right == null) return n.Left;
      // Replace with in-order successor
      Node succ = NodeMin(n.Right);
      n.Key = succ.Key;
      n.Count = succ.Count;
      n.Right = DeleteMin(n.Right);   // FIX CS8603: DeleteMin returns Node?
    }
    return Balance(n);
  }

  // Delete the minimum node (all its copies) from the subtree.
  private static Node? DeleteMin(Node n)   // FIX CS8603: return Node?
  {
    if (n.Left == null) return n.Right;
    n.Left = DeleteMin(n.Left);
    return Balance(n);
  }

  // ── Lookup helpers ────────────────────────────────────────────────────────

  private static Node? FindNode(Node? n, T key)
  {
    while (n != null)
    {
      int cmp = key.CompareTo(n.Key);
      if (cmp == 0) return n;
      n = cmp < 0 ? n.Left : n.Right;
    }
    return null;
  }

  // Smallest key >= x
  private static Node? LowerBoundNode(Node? n, T x)   // FIX CS8603: return Node?
  {
    Node? result = null;
    while (n != null)
    {
      int cmp = x.CompareTo(n.Key);
      if (cmp <= 0) { result = n; n = n.Left; }
      else n = n.Right;
    }
    return result;
  }

  // Largest key <= x
  private static Node? UpperBoundNode(Node? n, T x)   // FIX CS8603: return Node?
  {
    Node? result = null;
    while (n != null)
    {
      int cmp = x.CompareTo(n.Key);
      if (cmp >= 0) { result = n; n = n.Right; }
      else n = n.Left;
    }
    return result;
  }

  // Number of elements strictly less than x
  private static int CountLess(Node? n, T x)
  {
    int cnt = 0;
    while (n != null)
    {
      int cmp = x.CompareTo(n.Key);
      if (cmp <= 0) n = n.Left;
      else { cnt += GetSize(n.Left) + n.Count; n = n.Right; }
    }
    return cnt;
  }

  // Number of elements <= x
  private static int CountLessOrEqual(Node? n, T x)
  {
    int cnt = 0;
    while (n != null)
    {
      int cmp = x.CompareTo(n.Key);
      if (cmp < 0)
      {
        n = n.Left;
      }
      else if (cmp == 0)
      {
        cnt += GetSize(n.Left) + n.Count;
        break;
      }
      else
      {
        cnt += GetSize(n.Left) + n.Count;
        n = n.Right;
      }
    }
    return cnt;
  }

  // 0-indexed k-th smallest (multiplicities counted)
  private static T KthSmallest(Node? n, int k)   // FIX CS8603: param Node?
  {
    while (n != null)
    {
      int leftSize = GetSize(n.Left);
      if (k < leftSize)
      {
        n = n.Left;
      }
      else if (k < leftSize + n.Count)
      {
        return n.Key;
      }
      else
      {
        k -= leftSize + n.Count;
        n = n.Right;
      }
    }
    // Unreachable when called from ElementAt (which bounds-checks k), but
    // required to satisfy the compiler's definite-assignment analysis.
    throw new InvalidOperationException("k out of range.");
  }

  private static Node NodeMin(Node n)
  {
    while (n.Left != null) n = n.Left;
    return n;
  }

  private static Node NodeMax(Node n)
  {
    while (n.Right != null) n = n.Right;
    return n;
  }

  private void EnsureNotEmpty()
  {
    if (_root == null)
      throw new InvalidOperationException("The set is empty.");
  }
}

public struct Interval : IComparable<Interval>
{
  public long L, R;
  public Interval(long l, long r) { L = l; R = r; }
  public int CompareTo(Interval other) => L.CompareTo(other.L);
  public long Length => R - L;
}

public sealed class IntervalSet
{
  private readonly SortedSet<Interval> set = new SortedSet<Interval>();

  public long TotalArea { get; private set; } = 0;
  public int Count => set.Count;

  public IntervalSet() { }

  public long Add(long l, long r)
  {
    if (l >= r) return 0;

    long prevArea = TotalArea;
    var next = GetLowerBound(l);

    var prev = GetLessEqual(l);
    if (prev.HasValue && prev.Value.R >= l)
    {
      l = Math.Min(l, prev.Value.L);
      r = Math.Max(r, prev.Value.R);
      RemoveInternal(prev.Value);
    }

    foreach (var item in set.GetViewBetween(new Interval(l, 0), new Interval(r, long.MaxValue)))
    {
      r = Math.Max(r, item.R);
    }


    var targets = new List<Interval>();
    foreach (var item in set.GetViewBetween(new Interval(l, 0), new Interval(r, long.MaxValue)))
    {
      targets.Add(item);
    }
    foreach (var t in targets) RemoveInternal(t);

    var newInterval = new Interval(l, r);
    AddInternal(newInterval);

    return TotalArea - prevArea;
  }
  public long Remove(long l, long r)
  {
    if (l >= r) return 0;

    long prevArea = TotalArea;
    var targets = new List<Interval>();

    var prev = GetLessEqual(l);
    if (prev.HasValue && prev.Value.R > l) targets.Add(prev.Value);


    foreach (var item in set.GetViewBetween(new Interval(l, 0), new Interval(r - 1, long.MaxValue)))
    {
      if (!targets.Contains(item)) targets.Add(item);
    }

    foreach (var t in targets)
    {
      RemoveInternal(t);
      if (t.L < l) AddInternal(new Interval(t.L, l));
      if (t.R > r) AddInternal(new Interval(r, t.R));
    }

    return TotalArea - prevArea;
  }
  public int RangeCount(long l, long r)
  {
    if (l >= r) return 0;
    int cnt = 0;
    var it = GetLessEqual(l);
    var startL = it.HasValue ? it.Value.L : l;

    foreach (var item in set.GetViewBetween(new Interval(startL, 0), new Interval(r - 1, long.MaxValue)))
    {
      if (Math.Max(l, item.L) < Math.Min(r, item.R)) ++cnt;
    }
    return cnt;
  }
  public long SumLength(long l, long r)
  {
    if (l >= r) return 0;
    long sum = 0;

    var it = GetLessEqual(l);
    var startL = it.HasValue ? it.Value.L : l;

    foreach (var item in set.GetViewBetween(new Interval(startL, 0), new Interval(r - 1, long.MaxValue)))
    {
      long overlapL = Math.Max(l, item.L);
      long overlapR = Math.Min(r, item.R);
      if (overlapL < overlapR) sum += (overlapR - overlapL);
    }
    return sum;
  }

  public (long length, long L, long R) GetInfo(long p)
  {
    var res = GetLessEqual(p);
    if (res.HasValue && res.Value.L <= p && p < res.Value.R)
    {
      return (res.Value.Length, res.Value.L, res.Value.R);
    }
    return (0, 0, 0);
  }
  public Interval? LowerBound(long p)
  {
    var view = set.GetViewBetween(new Interval(p, long.MinValue), new Interval(long.MaxValue, long.MaxValue));
    return view.Count > 0 ? view.Min : (Interval?)null;
  }
  public Interval? UpperBound(long p)
  {
    var view = set.GetViewBetween(new Interval(p + 1, long.MinValue), new Interval(long.MaxValue, long.MaxValue));
    return view.Count > 0 ? view.Min : (Interval?)null;
  }

  public long Xor(long l, long r)
  {
    if (l >= r) return 0;
    long prevArea = TotalArea;

    var targets = new List<Interval>();
    var prev = GetLessEqual(l);
    if (prev.HasValue && prev.Value.R > l) targets.Add(prev.Value);
    foreach (var item in set.GetViewBetween(new Interval(l, 0), new Interval(r - 1, long.MaxValue)))
    {
      if (!targets.Contains(item)) targets.Add(item);
    }

    List<Interval> nextIntervals = new List<Interval>();
    long cur = l;
    foreach (var t in targets)
    {
      if (cur < t.L) nextIntervals.Add(new Interval(cur, t.L));
      if (t.L < l) nextIntervals.Add(new Interval(t.L, l));
      if (t.R > r) nextIntervals.Add(new Interval(r, t.R));
      cur = Math.Max(cur, t.R);
    }
    if (cur < r) nextIntervals.Add(new Interval(cur, r));

    foreach (var t in targets) RemoveInternal(t);
    foreach (var n in nextIntervals) AddInternal(n);

    return TotalArea - prevArea;
  }

  private void AddInternal(Interval iv)
  {
    if (iv.L >= iv.R) return;
    if (set.Add(iv)) TotalArea += iv.Length;
  }

  private void RemoveInternal(Interval iv)
  {
    if (set.Remove(iv)) TotalArea -= iv.Length;
  }

  private Interval? GetLessEqual(long l)
  {
    var view = set.GetViewBetween(new Interval(long.MinValue, 0), new Interval(l, long.MaxValue));
    return view.Count > 0 ? view.Max : (Interval?)null;
  }

  private Interval? GetLowerBound(long l) => LowerBound(l);
}

sealed class Deque<T>(int capacity = 16)
{
  private T[] _buf = new T[capacity];
  private int _head = 0;
  private int _count = 0;
  public int Count => _count;
  public int Capacity => _buf.Length;

  private int ToIndex(int i)
  {
    int res = _head + i;
    if (res >= Capacity) res -= Capacity;
    return res;
  }

  public T this[int i]
  {
    get
    {
      if ((uint)i >= (uint)_count) throw new ArgumentOutOfRangeException();
      return _buf[ToIndex(i)];
    }
    set
    {
      if ((uint)i >= (uint)_count) throw new ArgumentOutOfRangeException();
      _buf[ToIndex(i)] = value;
    }
  }

  public void PushBack(T val)
  {
    if (Count == Capacity) Expand();
    _buf[ToIndex(_count++)] = val;
  }

  public void PushFront(T val)
  {
    if (_count == Capacity) Expand();
    _head = (_head + Capacity - 1) % Capacity;
    _buf[_head] = val;
    _count++;
  }

  public T PopBack()
  {
    if (_count == 0) throw new InvalidOperationException();
    int idx = ToIndex(_count - 1);
    T val = _buf[idx];
    _count--;
    return val;
  }

  public T PopFront()
  {
    if (_count == 0) throw new InvalidOperationException();
    T val = _buf[_head];
    _head = (_head + 1 == Capacity ? 0 : _head + 1);
    _count--;
    return val;
  }

  public T Front()
  {
    if (_count == 0) throw new InvalidOperationException();
    return _buf[_head];
  }

  public T Back()
  {
    if (_count == 0) throw new InvalidOperationException();
    return _buf[ToIndex(_count - 1)];
  }

  private void Expand()
  {
    int newCap = Capacity << 1;
    var newBuf = new T[newCap];

    for (int i = 0; i < Count; ++i) newBuf[i] = this[i];

    _buf = newBuf;
    _head = 0;
  }
}

sealed class TopKSet<T>(int k, IComparer<T>? comp = null)
{
  private readonly int k = k;
  private readonly List<T> a = new();
  private readonly IComparer<T> comp = comp ?? Comparer<T>.Default;

  public int Count => a.Count;
  public T this[int i] => a[i];

  public void Push(T val)
  {
    int idx = LowerBound(val);
    a.Insert(idx, val);

    if (a.Count > k) a.RemoveAt(0);
  }

  public T PopMin()
  {
    var v = a[0];
    a.RemoveAt(0);
    return v;
  }
  public T PopMax()
  {
    int last = a.Count - 1;
    var v = a[last];
    a.RemoveAt(last);
    return v;
  }

  public bool TryPop(T val)
  {
    int idx = LowerBound(val);
    if (idx == a.Count || comp.Compare(a[idx], val) != 0) return false;

    a.RemoveAt(idx);
    return true;
  }

  private int LowerBound(T val)
  {
    int l = -1, r = a.Count;
    while (l + 1 < r)
    {
      int m = l + (r - l) / 2;
      if (comp.Compare(a[m], val) <= 0) l = m;
      else r = m;
    }
    return l;
  }
}

public static class StringExt
{
  public static string Reverse(this string s)
  {
    Span<char> span = s.Length <= 256 ? stackalloc char[s.Length] : new char[s.Length];

    s.AsSpan().CopyTo(span);
    span.Reverse();

    return new string(span);
  }
}

public static class JaggedArrayExt
{
  public static T[][] DeepCopy<T>(this T[][] a)
  {
    var b = new T[a.Length][];
    for (int i = 0; i < a.Length; ++i)
      b[i] = (T[])a[i].Clone();
    return b;
  }
}

sealed class Tensor<T>
{
  public readonly int[] Shape;
  public readonly int[] Stride;
  public readonly T[] Data;
  public Tensor(T val, params int[] shape)
    : this(shape)
   => Array.Fill(Data, val);
  public Tensor(Func<T> f, params int[] shape)
    : this(shape)
  {
    for (int i = 0; i < Data.Length; ++i) Data[i] = f();
  }
  public Tensor(Func<int[], T> f, params int[] shape)
    : this(shape)
  {
    var idx = new int[shape.Length];

    void Fill(int d)
    {
      if (d == shape.Length)
      {
        Data[ToFlatDex(idx)] = f(idx);
        return;
      }
      for (int i = 0; i < shape[d]; ++i)
      {
        idx[d] = i;
        Fill(d + 1);
      }
    }
    Fill(0);
  }
  private Tensor(int[] shape)
  {
    Shape = shape;
    Stride = new int[shape.Length];

    int n = 1;
    for (int i = shape.Length - 1; i >= 0; --i)
    {
      Stride[i] = n;
      n *= shape[i];
    }
    Data = new T[n];
  }
  private int ToFlatDex(int[] idx)
  {
    int k = 0;
    for (int i = 0; i < idx.Length; ++i) k += idx[i] * Stride[i];
    return k;
  }
  public T this[params int[] idx]
  {
    get
     => Data[ToFlatDex(idx)];
    set
     => Data[ToFlatDex(idx)] = value;
  }
  public override string ToString()
  {
    var sb = new StringBuilder();
    var idx = new int[Shape.Length];

    void BuildString(int d)
    {
      if (d == Shape.Length)
      {
        var v = Data[ToFlatDex(idx)];
        sb.Append(v?.ToString());
        return;
      }
      sb.Append('[');

      for (int i = 0; i < Shape[d]; ++i)
      {
        if (i > 0) sb.Append(", ");
        idx[d] = i;
        BuildString(d + 1);
      }
      sb.Append(']');
    }
    BuildString(0);
    return sb.ToString();
  }
}

public sealed class MatrixMod
{
  private readonly long[,] a;

  public int Rows { get; }
  public int Cols { get; }
  public long Mod { get; }

  public MatrixMod(int rows, int cols, long mod)
  {
    if (rows <= 0) throw new ArgumentOutOfRangeException(nameof(rows));
    if (cols <= 0) throw new ArgumentOutOfRangeException(nameof(cols));
    if (mod <= 0) throw new ArgumentOutOfRangeException(nameof(mod));

    Rows = rows;
    Cols = cols;
    Mod = mod;
    a = new long[rows, cols];
  }

  private MatrixMod(long[,] src, long mod)
  {
    Rows = src.GetLength(0);
    Cols = src.GetLength(1);
    Mod = mod;
    a = (long[,])src.Clone();
  }

  public long this[int r, int c]
  {
    get => a[r, c];
    set => a[r, c] = Normalize(value);
  }

  private long Normalize(long x)
  {
    x %= Mod;
    if (x < 0) x += Mod;
    return x;
  }

  private long Add(long x, long y)
  {
    x = Normalize(x);
    y = Normalize(y);
    return x >= Mod - y ? x - (Mod - y) : x + y;
  }

  private long Sub(long x, long y)
  {
    x = Normalize(x);
    y = Normalize(y);
    return x >= y ? x - y : x + (Mod - y);
  }

  private long Mul(long x, long y)
  {
    x = Normalize(x);
    y = Normalize(y);
    return (long)((BigInteger)x * y % Mod);
  }

  private bool TryModInverse(long x, out long inv)
  {
    x = Normalize(x);
    if (x == 0)
    {
      inv = 0;
      return false;
    }

    BigInteger a = x, b = Mod;
    BigInteger u = 1, v = 0;

    while (b != 0)
    {
      BigInteger q = a / b;
      (a, b) = (b, a - q * b);
      (u, v) = (v, u - q * v);
    }

    if (a != 1)
    {
      inv = 0;
      return false;
    }

    inv = (long)((u % Mod + Mod) % Mod);
    return true;
  }

  private void SwapRows(int r1, int r2)
  {
    if (r1 == r2) return;

    for (int j = 0; j < Cols; j++)
      (a[r1, j], a[r2, j]) = (a[r2, j], a[r1, j]);
  }

  public static MatrixMod Identity(int n, long mod)
  {
    var m = new MatrixMod(n, n, mod);
    for (int i = 0; i < n; i++) m.a[i, i] = 1 % mod;
    return m;
  }

  public MatrixMod Clone() => new MatrixMod(a, Mod);

  public MatrixMod Multiply(MatrixMod other)
  {
    if (other is null) throw new ArgumentNullException(nameof(other));
    if (Cols != other.Rows) throw new ArgumentException("Matrix size mismatch.");
    if (Mod != other.Mod) throw new ArgumentException("Mod mismatch.");

    var res = new MatrixMod(Rows, other.Cols, Mod);

    for (int i = 0; i < Rows; i++)
    {
      for (int k = 0; k < Cols; k++)
      {
        long aik = a[i, k];
        if (aik == 0) continue;

        for (int j = 0; j < other.Cols; j++)
        {
          res.a[i, j] = Add(res.a[i, j], Mul(aik, other.a[k, j]));
        }
      }
    }

    return res;
  }

  public MatrixMod Pow(long exp)
  {
    if (exp < 0) throw new ArgumentOutOfRangeException(nameof(exp));
    if (Rows != Cols) throw new InvalidOperationException("Pow is only available for square matrices.");

    var result = Identity(Rows, Mod);
    var baseMat = Clone();

    while (exp > 0)
    {
      if ((exp & 1) != 0) result = result.Multiply(baseMat);
      exp >>= 1;
      if (exp > 0) baseMat = baseMat.Multiply(baseMat);
    }

    return result;
  }

  // reduced = true  -> RREF まで掃き出す
  // reduced = false -> 上三角っぽい形まで
  public int GaussJordan(bool reduced = true)
  {
    int rank = 0;

    for (int col = 0, row = 0; col < Cols && row < Rows; col++)
    {
      int pivot = -1;
      long pivotInv = 0;

      for (int i = row; i < Rows; i++)
      {
        if (TryModInverse(a[i, col], out pivotInv))
        {
          pivot = i;
          break;
        }
      }

      if (pivot == -1) continue;

      SwapRows(row, pivot);

      // pivot row を 1 に正規化
      for (int j = col; j < Cols; j++)
        a[row, j] = Mul(a[row, j], pivotInv);

      if (reduced)
      {
        // 上下両方を消す
        for (int i = 0; i < Rows; i++)
        {
          if (i == row) continue;
          long factor = a[i, col];
          if (factor == 0) continue;

          for (int j = col; j < Cols; j++)
            a[i, j] = Sub(a[i, j], Mul(factor, a[row, j]));
        }
      }
      else
      {
        // 下だけ消す
        for (int i = row + 1; i < Rows; i++)
        {
          long factor = a[i, col];
          if (factor == 0) continue;

          for (int j = col; j < Cols; j++)
            a[i, j] = Sub(a[i, j], Mul(factor, a[row, j]));
        }
      }

      row++;
      rank++;
    }

    return rank;
  }

  public int Rank()
  {
    var tmp = Clone();
    return tmp.GaussJordan(reduced: false);
  }

  public MatrixMod Inverse()
  {
    if (Rows != Cols) throw new InvalidOperationException("Inverse is only available for square matrices.");

    int n = Rows;
    var aug = new MatrixMod(n, n * 2, Mod);

    for (int i = 0; i < n; i++)
    {
      for (int j = 0; j < n; j++)
        aug.a[i, j] = a[i, j];

      aug.a[i, n + i] = 1 % Mod;
    }

    int rank = aug.GaussJordan(reduced: true);
    if (rank != n) throw new InvalidOperationException("Matrix is not invertible under this modulus.");

    var inv = new MatrixMod(n, n, Mod);
    for (int i = 0; i < n; i++)
      for (int j = 0; j < n; j++)
        inv.a[i, j] = aug.a[i, n + j];

    return inv;
  }
}

readonly struct ModInt : IEquatable<ModInt>
{
  private readonly long _value;
  public static long Mod { get; private set; }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private ModInt(long value, bool _) => _value = value;

  public long Value => _value;

  public static void SetMod(long mod)
  {
    if (mod <= 1) throw new ArgumentOutOfRangeException(nameof(mod));
    Mod = mod;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ModInt(long value)
  {
    long v = value % Mod;
    if (v < 0) v += Mod;
    _value = v;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator ModInt(long x) => new ModInt(x);
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static explicit operator long(ModInt x) => x._value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ModInt operator +(ModInt a, ModInt b)
  {
    long res = a._value + b._value;
    if (res >= Mod) res -= Mod;
    return new ModInt(res, true);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ModInt operator -(ModInt a, ModInt b)
  {
    long res = a._value - b._value;
    if (res < 0) res += Mod;
    return new ModInt(res, true);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ModInt operator *(ModInt a, ModInt b)
  {
    return new ModInt(a._value * b._value % Mod, true);
  }

  public ModInt Pow(long n)
  {
    if (n < 0) return Inverse().Pow(-n);
    ModInt res = 1, a = this;
    while (n > 0)
    {
      if ((n & 1) == 1) res *= a;
      a *= a;
      n >>= 1;
    }
    return res;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ModInt Inverse()
  {
    return Pow(Mod - 2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ModInt operator /(ModInt a, ModInt b) => a * b.Inverse();

  public bool Equals(ModInt other) => _value == other._value;
  public override bool Equals(object? obj) => obj is ModInt other && Equals(other);
  public override int GetHashCode() => _value.GetHashCode();
  public override string ToString() => _value.ToString();

  public static ModInt operator ++(ModInt a) => a + 1;
  public static ModInt operator --(ModInt a) => a - 1;
  public static ModInt operator +(ModInt a, long b) => a + new ModInt(b);
  public static ModInt operator +(long a, ModInt b) => new ModInt(a) + b;
  public static ModInt operator -(ModInt a, long b) => a - new ModInt(b);
  public static ModInt operator -(long a, ModInt b) => new ModInt(a) - b;
  public static ModInt operator *(ModInt a, long b) => a * new ModInt(b);
  public static ModInt operator *(long a, ModInt b) => new ModInt(a) * b;
  public static ModInt operator /(ModInt a, long b) => a / new ModInt(b);

  public static ModInt operator /(long a, ModInt b) => new ModInt(a) / b;
  public static bool operator ==(ModInt a, ModInt b) => a._value == b._value;
  public static bool operator !=(ModInt a, ModInt b) => a._value != b._value;
}

sealed class Count
{
  private readonly int n;
  private readonly long mod;
  private readonly long[] fact;
  private readonly long[] inv;

  public Count(int n, long mod)
  {
    ++n;
    this.n = n;
    this.mod = mod;
    fact = new long[n];

    fact[0] = fact[1] = 1 % mod;
    for (int i = 2; i < n; ++i) fact[i] = fact[i - 1] * i % mod;

    inv = new long[n];
    inv[n - 1] = Sugaku.ModPow(fact[n - 1], mod - 2, mod);
    for (int i = n - 2; i >= 0; --i) inv[i] = inv[i + 1] * (i + 1) % mod;
  }

  public long C(int a, int b)
   => (a < 0 || b < 0 || b > a) ? 0 : fact[a] * inv[a - b] % mod * inv[b] % mod;

  public long H(int a, int b)
    => C(a + b - 1, a - 1);
}

interface IMonoid<T>
{
  static abstract T Id { get; }
  static abstract T Op(T a, T b);
}

static class Binary<T> where T : IBinaryInteger<T>
{
  public static readonly T mod3 = T.CreateChecked(Sugaku.MOD3);
  public static readonly T mod7 = T.CreateChecked(Sugaku.MOD7);

  public static T Add(T a, T b, T mod)
    => (a + b) % mod;

  public struct Sum3 : IMonoid<T>
  {
    public static T Id => T.Zero;
    public static T Op(T a, T b)
      => Add(a, b, mod3);
  }

  public struct Sum7 : IMonoid<T>
  {
    public static T Id => T.Zero;
    public static T Op(T a, T b)
      => Add(a, b, mod7);
  }

  public struct Sum : IMonoid<T>
  {
    public static T Id => T.Zero;
    public static T Op(T a, T b)
     => a + b;
  }

  public static T Mul(T a, T b, T mod)
  {
    if (a < T.Zero || a >= mod) a = (a + mod) % mod;
    if (b < T.Zero || b >= mod) b = (b + mod) % mod;

    return a * b % mod;
  }
  public struct Prod3 : IMonoid<T>
  {
    public static T Id => T.One;
    public static T Op(T a, T b)
      => Mul(a, b, mod3);
  }
  public struct Prod7 : IMonoid<T>
  {
    public static T Id => T.One;
    public static T Op(T a, T b)
      => Mul(a, b, mod7);
  }

  public static T Gcd(T a, T b)
  {
    while (b != T.Zero) (a, b) = (b, a % b);
    return a;
  }
  public struct GCD : IMonoid<T>
  {
    public static T Id => T.Zero;
    public static T Op(T a, T b)
      => Gcd(a, b);
  }
}

class Dinic
{
  private sealed class Edge(int to, int rev, long cap)
  {
    public int To = to;
    public int Rev = rev;
    public long Cap = cap;
  }

  private readonly int _n;
  private readonly List<Edge>[] _g;
  private readonly int[] _level;
  private readonly int[] _it;

  public Dinic(int n)
  {
    _n = n;
    _g = new List<Edge>[n];
    for (int i = 0; i < n; i++) _g[i] = new List<Edge>();
    _level = new int[n];
    _it = new int[n];
  }

  public void AddEdge(int from, int to, long cap)
  {
    int fromIndex = _g[from].Count;
    int toIndex = _g[to].Count;
    _g[from].Add(new Edge(to, toIndex, cap));
    _g[to].Add(new Edge(from, fromIndex, 0));
  }

  public void AddUndirectedEdge(int u, int v, long cap)
  {
    AddEdge(u, v, cap);
    AddEdge(v, u, cap);
  }

  public long MaxFlow(int s, int t)
  {
    long flow = 0;
    const long INF = long.MaxValue / 4;

    while (Bfs(s, t))
    {
      Array.Fill(_it, 0);
      while (true)
      {
        long f = Dfs(s, t, INF);
        if (f == 0) break;
        flow += f;
      }
    }

    return flow;
  }

  public bool[] MinCut(int s)
  {
    var visited = new bool[_n];
    var q = new Queue<int>();
    visited[s] = true;
    q.Enqueue(s);

    while (q.Count > 0)
    {
      int v = q.Dequeue();
      foreach (var e in _g[v])
      {
        if (e.Cap > 0 && !visited[e.To])
        {
          visited[e.To] = true;
          q.Enqueue(e.To);
        }
      }
    }

    return visited;
  }

  public List<int> GetSsideVertices(int s)
  {
    var side = MinCut(s);
    var res = new List<int>();
    for (int i = 0; i < _n; i++)
    {
      if (side[i]) res.Add(i);
    }
    return res;
  }

  private bool Bfs(int s, int t)
  {
    Array.Fill(_level, -1);
    var q = new Queue<int>();
    _level[s] = 0;
    q.Enqueue(s);

    while (q.Count > 0)
    {
      int v = q.Dequeue();
      foreach (var e in _g[v])
      {
        if (e.Cap <= 0 || _level[e.To] >= 0) continue;
        _level[e.To] = _level[v] + 1;
        if (e.To == t) return true;
        q.Enqueue(e.To);
      }
    }

    return _level[t] >= 0;
  }

  private long Dfs(int v, int t, long f)
  {
    if (v == t) return f;

    for (; _it[v] < _g[v].Count; _it[v]++)
    {
      var e = _g[v][_it[v]];
      if (e.Cap <= 0 || _level[v] >= _level[e.To]) continue;

      long d = Dfs(e.To, t, Math.Min(f, e.Cap));
      if (d <= 0) continue;

      e.Cap -= d;
      _g[e.To][e.Rev].Cap += d;
      return d;
    }

    return 0;
  }
}

/// <summary>
/// Better be implemented as a struct for a static dispatch
/// </summary>
public interface ILazyOp<TNode, TLazy>
{
  TNode Op(TNode l, TNode r);
  TNode E { get; }
  TNode Mapping(TLazy f, TNode x, int len);
  TLazy Composition(TLazy f, TLazy g);
  TLazy Id { get; }
  bool IsId(TLazy f);
}

public sealed class LazySegment<TNode, TLazy, TOp>
    where TOp : struct, ILazyOp<TNode, TLazy>
{
  private readonly int _n;
  private readonly int _size;
  private readonly int _log;

  private readonly TNode[] _d;
  private readonly TLazy[] _lz;
  private static readonly TOp _op = default;
  public int Length => _n;
  public LazySegment(int n)
  {
    if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
    _n = n;

    _log = 0;
    _size = 1;
    while (_size < _n) { _size <<= 1; _log++; }

    _d = new TNode[2 * _size];
    _lz = new TLazy[_size];

    var e = _op.E;
    var id = _op.Id;
    for (int i = 0; i < 2 * _size; i++) _d[i] = e;
    for (int i = 0; i < _size; i++) _lz[i] = id;
  }
  public LazySegment(IReadOnlyList<TNode> v) : this(v.Count)
  {
    for (int i = 0; i < _n; i++) _d[_size + i] = v[i];
    for (int i = _size - 1; i >= 1; i--) Update(i);
  }
  public void Set(int p, TNode x)
  {
    if ((uint)p >= (uint)_n) throw new ArgumentOutOfRangeException(nameof(p));
    p += _size;
    PushPath(p);
    _d[p] = x;
    for (int i = 1; i <= _log; i++) Update(p >> i);
  }
  public TNode Get(int p)
  {
    if ((uint)p >= (uint)_n) throw new ArgumentOutOfRangeException(nameof(p));
    p += _size;
    PushPath(p);
    return _d[p];
  }
  public TNode Prod(int l, int r)
  {
    if (l < 0 || r < l || r > _n) throw new ArgumentOutOfRangeException();
    if (l == r) return _op.E;

    int l0 = l + _size, r0 = r + _size;
    PushToEdge(l0);
    PushToEdge(r0 - 1);

    TNode sml = _op.E, smr = _op.E;
    int lp = l0, rp = r0;
    while (lp < rp)
    {
      if ((lp & 1) == 1) sml = _op.Op(sml, _d[lp++]);
      if ((rp & 1) == 1) smr = _op.Op(_d[--rp], smr);
      lp >>= 1; rp >>= 1;
    }
    return _op.Op(sml, smr);
  }
  public TNode AllProd() => _d[1];
  public void Apply(int p, TLazy f)
  {
    if ((uint)p >= (uint)_n) throw new ArgumentOutOfRangeException(nameof(p));
    p += _size;
    PushPath(p);
    _d[p] = _op.Mapping(f, _d[p], 1);
    for (int i = 1; i <= _log; i++) Update(p >> i);
  }
  public void Apply(int l, int r, TLazy f)
  {
    if (l < 0 || r < l || r > _n) throw new ArgumentOutOfRangeException();
    if (l == r) return;

    int l0 = l + _size, r0 = r + _size;

    for (int i = _log; i >= 1; i--)
    {
      if (((l0 >> i) << i) != l0) Push(l0 >> i, 1 << i);
      if (((r0 >> i) << i) != r0) Push((r0 - 1) >> i, 1 << i);
    }

    int l1 = l0, r1 = r0, leftLen = 1, rightLen = 1;
    while (l1 < r1)
    {
      if ((l1 & 1) == 1) AllApply(l1++, f, leftLen);
      if ((r1 & 1) == 1) AllApply(--r1, f, rightLen);
      l1 >>= 1; r1 >>= 1;
      leftLen <<= 1; rightLen <<= 1;
    }

    for (int i = 1; i <= _log; i++)
    {
      if (((l0 >> i) << i) != l0) Update(l0 >> i);
      if (((r0 >> i) << i) != r0) Update((r0 - 1) >> i);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void Update(int k)
      => _d[k] = _op.Op(_d[k << 1], _d[k << 1 | 1]);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void AllApply(int k, TLazy f, int len)
  {
    _d[k] = _op.Mapping(f, _d[k], len);
    if (k < _size)
      _lz[k] = _op.Composition(f, _lz[k]);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void Push(int k, int len)
  {
    if(_op.IsId(_lz[k])) return;

    int half=len>>1;

    AllApply(k << 1, _lz[k], half);
    AllApply(k << 1 | 1, _lz[k], half);
    _lz[k] = _op.Id;
  }

  private void PushPath(int k)
  {
    for (int i = _log; i >= 1; i--)
    {
      int node = k >> i;
      Push(node, 1 << i);
    }
  }

  private void PushToEdge(int p)
  {
    for (int i = _log; i >= 1; i--)
      Push(p >> i, 1 << i);
  }
}


class Segment<T>
{
  private readonly T[] v;
  private readonly Func<T, T, T> op;
  private readonly T ide;
  private readonly int n;
  public Segment(T identity, Func<T, T, T> op, int size, T val)
  {
    this.op = op;
    ide = identity;

    int k = 1;
    while (k < size) k <<= 1;

    n = k;
    v = new T[2 * k];

    for (int i = n; i < 2 * n; ++i) v[i] = val;
    for (int i = n - 1; i >= 1; --i) v[i] = op(v[2 * i], v[1 + 2 * i]);
  }
  public Segment(T identity, Func<T, T, T> op, IReadOnlyList<T> data)
  {
    this.op = op;
    ide = identity;

    int k = 1;
    while (k < data.Count) k <<= 1;

    n = k;
    v = new T[2 * k];

    for (int i = 0; i < n; ++i) v[i + n] = (i < data.Count) ? data[i] : ide;
    for (int i = n - 1; i >= 1; --i) v[i] = op(v[2 * i], v[2 * i + 1]);
  }

  public T Query(int a, int b)
  {
    if (a > b) (a, b) = (b, a);

    T rec(int a, int b, int k, int l, int r)
    {
      if (r <= a || b <= l) return ide;
      else if (a <= l && r <= b) return v[k];
      int m = l + (r - l) / 2;
      return op(rec(a, b, k * 2, l, m), rec(a, b, k * 2 + 1, m, r));
    }

    return rec(a, b + 1, 1, 0, n);
  }

  public void Set(int i, T val)
  {
    v[i + n] = val;
    for (int u = (i + n) / 2; u > 0; u >>= 1)
    {
      v[u] = op(v[2 * u], v[2 * u + 1]);
    }
  }

  public T this[int k]
  {
    get
      => v[k + n];

    set
      => Set(k, value);
  }
}

// sus
class UnionFind
{
  private readonly int[] par;
  private readonly int[] rank;
  private readonly int[] size;
  private readonly long[] diff;
  private int components;

  public UnionFind(int n, long sumUnity = 0)
  {
    par = new int[n];
    rank = new int[n];
    size = new int[n];
    diff = new long[n];

    for (int i = 0; i < n; ++i)
    {
      par[i] = i;
      rank[i] = 0;
      size[i] = 1;
      diff[i] = sumUnity;
    }

    components = n;
  }

  public int Root(int x)
  {
    if (par[x] == x) return x;

    int r = Root(par[x]);
    diff[x] += diff[par[x]];
    return par[x] = r;
  }

  public long Potential(int x)
  {
    Root(x);
    return diff[x];
  }

  public bool Same(int x, int y)
   => Root(x) == Root(y);

  public long Diff(int x, int y)
    => Potential(y) - Potential(x);

  public bool Merge(int x, int y, long w = 0)
  {
    w += Potential(x);
    w -= Potential(y);

    x = Root(x);
    y = Root(y);

    if (x == y) return false;

    if (rank[x] < rank[y])
    {
      (x, y) = (y, x);
      w = -w;
    }

    if (rank[x] == rank[y]) rank[x]++;

    par[y] = x;
    diff[y] = w;
    size[x] += size[y];
    --components;

    return true;
  }
  public void Roots(Action<int> action)
  {
    for (int i = 0; i < par.Length; ++i) if (Root(i) == i) action(i);
  }

  public int Size()
    => components;

  public int Size(int x)
    => size[Root(x)];
}

class StackArray<T>(int n = 128)
{
  private T[] v = new T[n];
  private int count = 0;
  private int n = n;

  public void Push(T val)
  {
    if (count == n)
    {
      var x = new T[2 * n];
      Array.Copy(v, x, count);
      v = x;
      n <<= 1;
    }
    v[count++] = val;
  }

  public T Peek()
  {
    Validate(0);
    return v[count - 1];
  }

  public T Pop()
  {
    Validate(0);
    T item = v[--count];
    v[count] = default!;
    return item;
  }

  public int Count => count;

  public T this[int k]
  {
    get
    {
      Validate(k);
      return v[k];
    }
  }

  private void Validate(int t)
  {
    if (t < 0 || t >= count) throw new IndexOutOfRangeException();
  }
}

static class Graph
{
  public static readonly int[] D = [-1, 0, 1, 0, -1, -1, 1, 1, -1];
  public const int TRUE = 1;
  public const int FALSE = 0;
  public static IEnumerable<(int, int, int)> Adjacent(int i, int j, int H, int W, int p = 4)
  {
    for (int k = 0; k < p; ++k)
    {
      int ni = i + D[k], nj = j + D[k + 1];
      if (ni < 0 || nj < 0 || ni >= H || nj >= W) continue;
      yield return (ni, nj, k);
    }
    yield break;
  }

  public static bool NextPermutation<T>(T[] a) where T : IComparable<T>
  {
    int n = a.Length;
    int i = n - 2;

    while (i >= 0 && a[i].CompareTo(a[i + 1]) >= 0) --i;
    if (i < 0) return false;

    int j = n - 1;
    while (a[i].CompareTo(a[j]) >= 0) --j;

    (a[i], a[j]) = (a[j], a[i]);
    Array.Reverse(a, i + 1, n - i - 1);

    return true;
  }
  public static void ForEachCombination(int n, int k, Action<ReadOnlySpan<int>> action)
  {
    var a = new int[k];
    for (int i = 0; i < k; ++i) a[i] = i;

    while (true)
    {
      action(a.AsSpan());

      int i = k - 1;
      for (; i >= 0; --i) if (a[i] != i + n - k) break;
      if (i < 0) return;
      a[i]++;
      for (int j = i + 1; j < k; ++j) a[j] = a[j - 1] + 1;
    }
  }
  public static IEnumerable<int> Rep(int M)
  {
    for (int i = 0; i < M; ++i) yield return i;
  }

  public static UnionFind SCC(List<int>[] graph, List<int>[] inv)
  {
    int N = graph.Length;

    var seen = Nms.Array(N, false);
    var ck = Nms.Stack<int>();

    var dfs = Nms.Stack<(int, bool)>();

    for (int i = 0; i < N; ++i) if (!seen[i])
    {
      dfs.Push((i, false));
      while (dfs.Count > 0)
      {
        var (u, post) = dfs.Pop();

        if (post)
        {
          ck.Push(u);
          continue;
        }

        seen[u] = true;
        dfs.Push((u, true));
        foreach (var v in graph[u]) if (!seen[v]) dfs.Push((v, false));
      }
    }

    Array.Fill(seen, false);

    var uf = new UnionFind(N);

    while (ck.Count > 0)
    {
      var s = ck.Pop();
      if (seen[s]) continue;
      dfs.Push((s, true));

      while (dfs.Count > 0)
      {
        var (u, _) = dfs.Pop();
        uf.Merge(u, s);
        foreach (var v in inv[u]) if (!seen[v])
        {
          seen[v] = true;
          dfs.Push((v, true));
        }
      }
    }

    return uf;
  }

  // sus?
  public static T[] RootedTree<T>(List<int>[] edges, Func<T, T, T> op, T ide) where T : IBinaryInteger<T>
  {
    int N = edges.Length;
    var dp = Nms.Array(N, T.Zero);

    void bottomup(int u, int p)
    {
      dp[u] = ide;
      foreach (var v in edges[u]) if (v != p)
      {
        bottomup(v, u);
        dp[u] = op(dp[u], dp[v]);
      }
    }
    bottomup(0, -1);

    var res = Nms.Array(N, T.Zero);

    void topdown(int u, int p)
    {
      int M = edges[u].Count;

      var sdp = Nms.SubArray<T>(dp, edges[u].ToArray());

      var prefix = Nms.PrefixFold<T>(sdp, ide, op);
      var suffix = Nms.SuffixFold<T>(sdp, ide, op);

      res[u] = prefix[M - 1];

      for (int i = 0; i < M; ++i) if (edges[u][i] != p)
      {
        dp[u] = op(i > 0 ? prefix[i - 1] : ide, i + 1 < M ? suffix[i + 1] : ide);
        topdown(edges[u][i], u);
      }
    }
    topdown(0, -1);

    return res;
  }
}

sealed class TimeKeeper(double timeThreshold)
{
  private readonly Stopwatch stopwatch=Stopwatch.StartNew();
  private readonly double timeThreshold=timeThreshold;
  private double currentTime=0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Set()
    => currentTime = stopwatch.Elapsed.TotalMilliseconds;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double Get()
    => currentTime;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool IsTimeOver()
    => currentTime >= timeThreshold;
}

sealed class XorShiftRandom(int seed=0)
{
  private readonly Random rng=new Random(seed);

  public int NextInt(int m)
    => rng.Next(m);
  
  public double NextDouble()
    => rng.NextDouble();
  
  public double NextLog()
    => Math.Log(rng.NextDouble());

}

static class Sugaku
{
  public const long MOD7 = 1000000007L;
  public const long MOD3 = 998244353L;
  public const int INF = 1001001001;
  public const long LINF = 1001001001001001001L;

  public static T GCD<T>(T a, T b) where T : IBinaryInteger<T>
  {
    a = T.Abs(a);
    b = T.Abs(b);
    while (b != T.Zero) (a, b) = (b, a % b);
    return a;
  }
  public static T LCM<T>(T a, T b) where T : IBinaryInteger<T>
    => a / GCD(a, b) * b;

  public static string ToBase(long N, int k)
  {
    if (k >= 10 || k < 1) throw new Exception("Invalid base number.");
    string res = "";
    while (N > 0)
    {
      char c = (char)((N % k) + '0');
      res += c;
      N /= k;
    }
    return res.Reverse();
  }

  public static double Median<T>(ReadOnlySpan<T> a) where T : INumber<T>
  {
    if (a.Length == 0) throw new ArgumentException("empty");

    var b = a.ToArray();
    Array.Sort(b);

    int n = b.Length;
    if ((n & 1) == 1) return double.CreateChecked(b[n / 2]);
    return double.CreateChecked(b[n / 2 - 1] + b[n / 2]) / 2.0;
  }

  public static T MinPositivePreferred<T>(T a, T b) where T : INumber<T>
  {
    bool pa = a > T.Zero;
    bool pb = b > T.Zero;

    if (pa && pb) return T.Min(a, b);
    if (pa) return a;
    if (pb) return b;
    return T.Min(a, b);
  }

  public static T PopCount<T>(T a) where T : IBinaryInteger<T>
   => T.PopCount(a);
  public static long Inv3(long b)
    => Pow3(b, MOD3 - 2);
  public static long Inv7(long b)
    => Pow7(b, MOD7 - 2);
  public static long Pow3(long b, long r)
    => ModPow(b, r, MOD3);

  public static long Pow7(long b, long r)
    => ModPow(b, r, MOD7);

  public static long ModPow(long b, long r, long MOD)
  {
    long res = 1;
    b %= MOD;
    while (r > 0)
    {
      if ((r & 1) != 0) res = res * b % MOD;
      b = b * b % MOD;
      r >>= 1;
    }
    return res;
  }

  public static bool ChMax<T>(ref T a, T b) where T : INumber<T>
  {
    if (a.CompareTo(b) < 0)
    {
      a = b;
      return true;
    }
    return false;
  }
  public static bool ChMin<T>(ref T a, T b) where T : INumber<T>
  {
    if (a.CompareTo(b) > 0)
    {
      a = b;
      return true;
    }
    return false;
  }
  public static T Ceil<T>(T a, T b) where T : IBinaryInteger<T>
    => -Floor(-a, b);
  public static T Floor<T>(T a, T b) where T : IBinaryInteger<T>
  {
    T q = a / b, r = a % b;
    if (r != T.Zero && (a ^ b) < T.Zero) --q;
    return q;
  }
}

class FastScanner
{
  private readonly Stream stream = Console.OpenStandardInput();
  private readonly byte[] buffer = new byte[1 << 16];
  private int ptr = 0, len = 0;

  private byte Read()
  {
    if (ptr >= len)
    {
      len = stream.Read(buffer, 0, buffer.Length);
      ptr = 0;
      if (len == 0) return 0;
    }
    return buffer[ptr++];
  }

  public int Int()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return 0;
    int sign = 1;
    if (c == '-')
    {
      sign = -1;
      c = Read();
    }
    int val = c - '0';
    while ((c = Read()) >= '0')
    {
      checked
      {
        val = val * 10 + c - '0';
      }
    }
    return val * sign;
  }

  public (int, int) Int2()
    => (Int(), Int());

  public (int, int, int) Int3()
    => (Int(), Int(), Int());

  public (int, int, int, int) Int4()
    => (Int(), Int(), Int(), Int());

  public long Long()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return 0;
    int sign = 1;
    if (c == '-')
    {
      sign = -1;
      c = Read();
    }
    long val = c - '0';
    while ((c = Read()) >= '0')
      val = val * 10 + c - '0';
    return val * sign;
  }

  public (long, long) Long2()
    => (Long(), Long());

  public (long, long, long) Long3()
    => (Long(), Long(), Long());

  public (long, long, long, long) Long4()
    => (Long(), Long(), Long(), Long());

  public (int, long) IntLong()
    => (Int(), Long());

  public int[] Digits()
  {
    var s = this.String();
    var res = new int[s.Length];
    for (int i = 0; i < s.Length; ++i) res[i] = s[i] - '0';
    return res;
  }

  public string String()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return "";
    var sb = new StringBuilder();
    do
    {
      sb.Append((char)c);
      c = Read();
    } while (c > ' ');
    return sb.ToString();
  }
  public (string, string) String2()
    => (String(), String());
  public (string, string, string) String3()
    => (String(), String(), String());

  public double Double()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return 0;

    int sign = 1;
    if (c == '-')
    {
      sign = -1;
      c = Read();
    }

    double val = 0;

    // integer part
    while (c >= '0' && c <= '9')
    {
      val = val * 10 + (c - '0');
      c = Read();
    }

    // fractional part
    if (c == '.')
    {
      double scale = 1;
      while ((c = Read()) >= '0' && c <= '9')
      {
        scale *= 0.1;
        val += (c - '0') * scale;
      }
    }

    // exponent part
    if (c == 'e' || c == 'E')
    {
      int esign = 1;
      int exp = 0;

      c = Read();
      if (c == '-')
      {
        esign = -1;
        c = Read();
      }
      else if (c == '+')
      {
        c = Read();
      }

      while (c >= '0' && c <= '9')
      {
        exp = exp * 10 + (c - '0');
        c = Read();
      }

      val *= Math.Pow(10, esign * exp);
    }

    return val * sign;
  }

  public (double, double) Double2()
    => (Double(), Double());
  public (double, double, double) Double3()
    => (Double(), Double(), Double());

  public char Char()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return '\0';
    return (char)c;
  }
}