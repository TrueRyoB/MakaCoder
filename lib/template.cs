using System.Text;
using System.Numerics;

var fs = new FastScanner();
var sb = new StringBuilder();






Console.Write(sb.ToString());


class Nms
{
  public static T[] Array<T>(int n, Func<T> f)
  {
    var a = new T[n];
    for (int i = 0; i < n; i++) a[i] = f();
    return a;
  }
  public static T[] Array<T>(int n, T val)
  {
    var a = new T[n];
    for (int i = 0; i < n; i++) a[i] = val;
    return a;
  }
  public static T[] Prefix<T>(T[] v) where T : INumber<T>
  {
    var a = new T[v.Length];
    for (int i = 0; i < a.Length; i++) 
    {
      a[i] = (i > 0 ? a[i-1] : T.Zero) + v[i];
    }
    return a;
  }
  public static void Prefix<T>(T[] dest, T[] src) where T : INumber<T>
  {
    if (dest.Length != src.Length) throw new ArgumentException();

    for(int i=0; i<dest.Length; ++i) 
    {
      dest[i] = (i > 0 ? dest[i-1] : T.Zero) + src[i];
    }
  }
  public static T[] Suffix<T>(T[] v) where T : INumber<T>
  {
    var a = new T[v.Length];
    for (int i = a.Length-1; i >= 0; i--)
    {
      a[i] = (i+1 < a.Length ? a[i+1] : T.Zero) + v[i];
    }
    return a;
  }
  public static void Suffix<T>(T[] dest, T[] src) where T : INumber<T>
  {
    if (a.Length != v.Length) throw new ArgumentException();
    for(int i = dest.Length-1; i>=0; --i) 
    {
      dest[i] = (i+1 < dest.Length ? dest[i+1] : T.Zero) + src[i];
    }
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
  public static List<int>[] Graph(int n)
  {
    var a = new List<int>[n];
    for (int i = 0; i < n; ++i) a[i] = [];
    return a;
  }
  public static void Mwah(bool b = true)
  {
    if (b) Console.WriteLine("MWAH!");
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

class LazySegment<TNode, TLazy>
{
  private readonly int _n;
  private readonly int _size;
  private readonly int _log;

  private readonly TNode[] _d;
  private readonly TLazy[] _lz;

  private readonly Func<TNode, TNode, TNode> _op;
  private readonly TNode _e;

  // mapping(f, x, len) = 区間長 len のノード x に遅延 f を適用した結果
  private readonly Func<TLazy, TNode, int, TNode> _mapping;

  // composition(f, g) = f ∘ g
  private readonly Func<TLazy, TLazy, TLazy> _composition;
  private readonly TLazy _id;

  public int Length => _n;

  public LazySegment(
      int n,
      Func<TNode, TNode, TNode> op,
      TNode e,
      Func<TLazy, TNode, int, TNode> mapping,
      Func<TLazy, TLazy, TLazy> composition,
      TLazy id)
  {
    if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
    _n = n;
    _op = op ?? throw new ArgumentNullException(nameof(op));
    _e = e;
    _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
    _composition = composition ?? throw new ArgumentNullException(nameof(composition));
    _id = id;

    _log = 0;
    _size = 1;
    while (_size < _n)
    {
      _size <<= 1;
      _log++;
    }

    _d = new TNode[2 * _size];
    _lz = new TLazy[_size];

    for (int i = 0; i < 2 * _size; i++) _d[i] = _e;
    for (int i = 0; i < _size; i++) _lz[i] = _id;
  }

  public LazySegment(
      IReadOnlyList<TNode> v,
      Func<TNode, TNode, TNode> op,
      TNode e,
      Func<TLazy, TNode, int, TNode> mapping,
      Func<TLazy, TLazy, TLazy> composition,
      TLazy id)
      : this(v.Count, op, e, mapping, composition, id)
  {
    for (int i = 0; i < _n; i++) _d[_size + i] = v[i];
    for (int i = _size - 1; i >= 1; i--) Update(i);
  }

  private void Update(int k)
  {
    _d[k] = _op(_d[k << 1], _d[k << 1 | 1]);
  }

  private void AllApply(int k, TLazy f, int len)
  {
    _d[k] = _mapping(f, _d[k], len);
    if (k < _size)
    {
      _lz[k] = _composition(f, _lz[k]);
    }
  }

  private void Push(int k, int len)
  {
    if (EqualityComparer<TLazy>.Default.Equals(_lz[k], _id)) return;

    int half = len >> 1;
    AllApply(k << 1, _lz[k], half);
    AllApply(k << 1 | 1, _lz[k], half);
    _lz[k] = _id;
  }

  private void PushPath(int k)
  {
    for (int i = _log; i >= 1; i--)
    {
      int node = k >> i;
      int len = 1 << i;
      Push(node, len);
    }
  }

  public void Set(int p, TNode x)
  {
    if ((uint)p >= (uint)_n) throw new ArgumentOutOfRangeException(nameof(p));

    p += _size;
    PushPath(p);

    _d[p] = x;
    for (int i = 1; i <= _log; i++)
    {
      int k = p >> i;
      Update(k);
    }
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

    if (l == r) return _e;

    l += _size;
    r += _size;

    PushToEdge(l);
    PushToEdge(r - 1);

    TNode sml = _e;
    TNode smr = _e;

    while (l < r)
    {
      if ((l & 1) == 1) sml = _op(sml, _d[l++]);
      if ((r & 1) == 1) smr = _op(_d[--r], smr);
      l >>= 1;
      r >>= 1;
    }

    return _op(sml, smr);
  }

  private void PushToEdge(int p)
  {
    for (int i = _log; i >= 1; i--)
    {
      int k = p >> i;
      int len = 1 << i;
      Push(k, len);
    }
  }

  public TNode AllProd() => _d[1];

  public void Apply(int p, TLazy f)
  {
    if ((uint)p >= (uint)_n) throw new ArgumentOutOfRangeException(nameof(p));

    p += _size;
    PushPath(p);

    _d[p] = _mapping(f, _d[p], 1);

    for (int i = 1; i <= _log; i++)
    {
      int k = p >> i;
      Update(k);
    }
  }

  public void Apply(int l, int r, TLazy f)
  {
    if (l < 0 || r < l || r > _n) throw new ArgumentOutOfRangeException();

    if (l == r) return;

    int l0 = l + _size;
    int r0 = r + _size;

    // 端点付近の遅延を落とす
    for (int i = _log; i >= 1; i--)
    {
      if (((l0 >> i) << i) != l0)
      {
        int k = l0 >> i;
        Push(k, 1 << i);
      }
      if (((r0 >> i) << i) != r0)
      {
        int k = (r0 - 1) >> i;
        Push(k, 1 << i);
      }
    }

    int l1 = l0;
    int r1 = r0;
    int leftLen = 1;
    int rightLen = 1;

    while (l1 < r1)
    {
      if ((l1 & 1) == 1) AllApply(l1++, f, leftLen);
      if ((r1 & 1) == 1) AllApply(--r1, f, rightLen);
      l1 >>= 1;
      r1 >>= 1;
      leftLen <<= 1;
      rightLen <<= 1;
    }

    for (int i = 1; i <= _log; i++)
    {
      if (((l0 >> i) << i) != l0)
      {
        int k = l0 >> i;
        Update(k);
      }
      if (((r0 >> i) << i) != r0)
      {
        int k = (r0 - 1) >> i;
        Update(k);
      }
    }
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
    for (int i = n - 1; i >= 1; --i) v[i] = op(v[i / 2], v[1 + i / 2]);
  }
  public Segment(T identity, Func<T, T, T> op, IReadOnlyList<T> data)
  {
    this.op = op;
    ide = identity;

    int k = 1;
    while (k < data.Count) k <<= 1;

    n = k;
    v = new T[2 * k];

    for (int i = n; i < 2 * n; ++i) v[i] = data[i - n];
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
    int u = i + n;
    while (u > 0)
    {
      if (u >= n) v[u] = val;
      else v[u] = op(v[2 * u], v[2 * u + 1]);
      u /= 2;
    }
  }

  public T this[int k] => v[k + n];
}

class UnionFind
{
  private readonly int[] root;
  private readonly int[] size;
  private int sz;
  public UnionFind(int n)
  {
    root = new int[n];
    size = new int[n];
    for (int i = 0; i < n; ++i)
    {
      root[i] = i;
      size[i] = 1;
    }
    sz = n;
  }
  public int Root(int k)
  {
    return root[k] == k ? k : root[k] = Root(root[k]);
  }
  public int Merge(int a, int b)
  {
    a = Root(a); b = Root(b);
    if (a == b) return a;
    if (size[a] < size[b]) (a, b) = (b, a);
    root[b] = a;
    size[a] += size[b];
    --sz;
    return a;
  }
  public bool Same(int a, int b) => Root(a) == Root(b);

  public int Count => sz;
  public int this[int k] => size[Root(k)];
}

class StackArray<T>(int n = 100)
{
  private T[] v = new T[n];
  private int count = 0;
  private int n = n;

  public void Push(T val)
  {
    if (count == n)
    {
      var x = new T[2 * n];
      for (int i = 0; i < count; ++i) x[i] = v[i];
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
    return v[--count];
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

class Graph
{
  public static readonly int[] D = [-1, 0, 1, 0, -1, -1, 1, 1, -1];
  public const int TRUE = 1;
  public const int FALSE = 0;
  public static long MinPositive(long a, long b)
  {
    return (a < 0 || b < 0) ? Math.Max(a, b) : Math.Min(a, b);
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
}

class Sugaku
{
  public const long MOD7 = 1000000007L;
  public const long MOD3 = 998244353L;
  public const int INF = 1001001001;
  public const long LINF = 1001001001001001001L;

  public static int PopCount<T>(T a) where T : IBinaryInteger<T>
  {
    int cnt = 0;
    while (a.CompareTo(0) > 0)
    {
      if ((a & T.One) == T.One) ++cnt;
      a >>= 1;
    }
    return cnt;
  }

  public static long ModPow(long b, long r, long MOD)
  {
    long res = 1;
    while (r > 0)
    {
      if ((r & 1) != 0) res = res * b % MOD;
      b = b * b % MOD;
      r >>= 1;
    }
    return res;
  }

  public static bool ChMax<T>(ref T a, T b) where T : IComparable<T>
  {
    if (a.CompareTo(b) < 0)
    {
      a = b;
      return true;
    }
    return false;
  }
  public static bool ChMin<T>(ref T a, T b) where T : IComparable<T>
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
      val = val * 10 + c - '0';
    return val * sign;
  }

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

  public char Char()
  {
    int c;
    while ((c = Read()) <= ' ') if (c == 0) return '\0';
    return (char)c;
  }
}