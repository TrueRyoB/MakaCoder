using System.Text; using System.Numerics; using System.Runtime.CompilerServices; using System; using System.Collections.Generic; using System.Linq; using System.IO;
#nullable enable


var fs = new FastScanner();
var sb = new StringBuilder();






















































Console.Write(sb.ToString());
return 0;


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
    for (int i = 0; i< a.Length; ++i) a[i]=op(v[i], i > 0 ? a[i-1] : ide);
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
    for (int i=a.Length-1; i>=0; --i) a[i]=op(v[i], i+1<a.Length ? a[i+1] : ide);
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

  public static int[] InOrder<T>(T[] a) where T : IComparable<T>
  {
    var sorted=a.ToArray();
    System.Array.Sort(sorted);

    var uniq=new List<T>(sorted.Length);
    foreach(var x in sorted)
    {
      if(uniq.Count==0 || uniq[^1].CompareTo(x)!=0) uniq.Add(x);
    }

    var res=new int[a.Length];

    for(int i=0; i<a.Length; ++i)
    {
      int l=-1, r=uniq.Count;
      while(l+1<r)
      {
        int m=l+(r-l)/2;
        if(uniq[m].CompareTo(a[i])<0) l=m;
        else r=m;
      }
      res[i]=r;
    }
    return res;
  }

  public static T[] SubArray<T>(ReadOnlySpan<T> a, ReadOnlySpan<int> index)
  {
    foreach(var e in index) if(e<0 || e>=a.Length) throw new Exception("Out of bounds.");

    int n=index.Length;
    var res = new T[n];
    for(int i=0; i<n; ++i) res[i]=a[index[i]];
    return res;
  }
  public static void Permutations(int n, Action<ReadOnlySpan<int>> action)
  {
    if(n<=0) throw new Exception("Invalid array length");
    if(n>=12) throw new Exception("TLE alert");

    var a=new int[n];
    for (int i=0; i<n; ++i) a[i]=i;

    while(true)
    {
      action(a.AsSpan());

      int i=n-2;
      while(i>=0 && a[i]>=a[i+1]) --i;
      if(i<0) return;

      int j=n-1;
      while(a[i]>=a[j]) --j;

      (a[i], a[j])=(a[j], a[i]);
      System.Array.Reverse(a, i+1, n-i-1);
    }
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
  public static TopKSet<T> TopKSet<T>(int k, IComparer<T>? comp=null)
    => new TopKSet<T>(k, comp);
  public static Matrix<T> RegularMatrix<T>(int n, Func<T, T, T> add, Func<T, T, T> mul, T addIdentity, T mulIdentity, IEqualityComparer<T>? comparer = null)
    => new Matrix<T>(n, add, mul, addIdentity, mulIdentity, comparer);
  public static Dinic MaxFlowSolver<T>(int n)
    => new Dinic(n);
  public static LazySegment<TNode, TLazy> LazySegmentTree<TNode, TLazy>(int n,Func<TNode, TNode, TNode> op, TNode e, Func<TLazy, TNode, int, TNode> mapping, Func<TLazy, TLazy, TLazy> composition, TLazy id)
    => new LazySegment<TNode, TLazy>(n, op, e, mapping, composition, id);
  public static Segment<T> SegmentTree<T>(T identity, Func<T, T, T> op, int size, T val)
    => new Segment<T>(identity, op, size, val);
  public static Segment<T> SegmentTree<T>(T identity, Func<T, T, T> op, T[] data)
    => new Segment<T>(identity, op, data);
  public static UnionFind UnionFind(int n)
    => new UnionFind(n);
}

sealed class Deque<T>(int capacity=16)
{
  private T[] _buf=new T[capacity];
  private int _head=0;
  private int _count=0;
  public int Count => _count;
  public int Capacity => _buf.Length;

  private int ToIndex(int i)
  {
    int res=_head+i;
    if(res>=Capacity) res-=Capacity;
    return res;
  }

  public T this[int i]
  {
    get
    {
      if((uint)i>=(uint)_count) throw new ArgumentOutOfRangeException();
      return _buf[ToIndex(i)];
    }
    set
    {
      if((uint)i>=(uint)_count) throw new ArgumentOutOfRangeException();
      _buf[ToIndex(i)]=value;
    }
  }

  public void PushBack(T val)
  {
    if(Count==Capacity) Expand();
    _buf[ToIndex(_count++)]=val;
  }

  public void PushFront(T val)
  {
    if(_count==Capacity) Expand();
    _head = (_head+Capacity-1)%Capacity;
    _buf[_head]=val;
    _count++;
  }

  public T PopBack()
  {
    if(_count==0) throw new InvalidOperationException();
    int idx=ToIndex(_count-1);
    T val=_buf[idx];
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

  public T PeekFront()
  {
    if (_count == 0) throw new InvalidOperationException();
    return _buf[_head];
  }

  public T PeekBack()
  {
    if (_count == 0) throw new InvalidOperationException();
    return _buf[ToIndex(_count - 1)];
  }

  private void Expand()
  {
    int newCap = Capacity << 1;
    var newBuf= new T[newCap];

    for(int i=0; i<Count; ++i) newBuf[i]=this[i];

    _buf=newBuf;
    _head=0;
  }
}

sealed class TopKSet<T>(int k, IComparer<T>? comp=null)
{
  private readonly int k=k;
  private readonly List<T> a=new();
  private readonly IComparer<T> comp=comp??Comparer<T>.Default;
  
  public int Count=>a.Count;
  public T this[int i] => a[i];

  public void Push(T val)
  {
    int idx=LowerBound(val);
    a.Insert(idx, val);
    
    if(a.Count>k) a.RemoveAt(0);
  }

  public T PopMin()
  {
    var v=a[0];
    a.RemoveAt(0);
    return v;
  }
  public T PopMax()
  {
    int last=a.Count-1;
    var v=a[last];
    a.RemoveAt(last);
    return v;
  }

  public bool TryPop(T val)
  {
    int idx=LowerBound(val);
    if(idx==a.Count || comp.Compare(a[idx], val)!=0) return false;

    a.RemoveAt(idx);
    return true;
  }
  
  private int LowerBound(T val)
  {
    int l=-1, r=a.Count;
    while(l+1<r)
    {
      int m=l+(r-l)/2;
      if(comp.Compare(a[m], val)<=0) l=m;
      else r=m;
    }
    return l;
  }
}

public static class StringExt
{
  public static string Reverse(this string s)
  {
    Span<char> span = s.Length<=256 ? stackalloc char[s.Length] : new char[s.Length];

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
    for(int i=0; i<Data.Length; ++i) Data[i]=f();
  }
  public Tensor(Func<int[], T> f, params int[] shape)
    :this(shape)
  {
    var idx=new int[shape.Length];

    void Fill(int d)
    {
      if(d==shape.Length)
      {
        Data[ToFlatDex(idx)]=f(idx);
        return;
      }
      for(int i=0; i<shape[d]; ++i)
      {
        idx[d]=i;
        Fill(d+1);
      }
    }
    Fill(0);
  }
  private Tensor(int[] shape)
  {
    Shape=shape;
    Stride=new int[shape.Length];

    int n=1;
    for(int i=shape.Length-1; i>=0; --i)
    {
      Stride[i]=n;
      n*=shape[i];
    }
    Data=new T[n];
  }
  private int ToFlatDex(int[] idx)
  {
    int k=0;
    for(int i=0; i<idx.Length; ++i) k+=idx[i]*Stride[i];
    return k;
  }
  public T this[params int[] idx]
  {
    get
     => Data[ToFlatDex(idx)];
    set
     => Data[ToFlatDex(idx)]=value;
  }
  public override string ToString()
  {
    var sb=new StringBuilder();
    var idx=new int[Shape.Length];

    void BuildString(int d)
    {
      if(d==Shape.Length)
      {
        var v=Data[ToFlatDex(idx)];
        sb.Append(v?.ToString());
        return;
      }
      sb.Append('[');
      
      for(int i=0; i<Shape[d]; ++i)
      {
        if(i>0) sb.Append(", ");
        idx[d]=i;
        BuildString(d+1);
      }
      sb.Append(']');
    }
    BuildString(0);
    return sb.ToString();
  }
}

sealed class Matrix<T>
{
  private readonly T[] data;

  public int N { get; }
  public Func<T, T, T> Add { get; }
  public Func<T, T, T> Mul { get; }
  public T AddIdentity { get; }
  public T MulIdentity { get; }
  private readonly IEqualityComparer<T> comparer;

  public Matrix(
      int n,
      Func<T, T, T> add,
      Func<T, T, T> mul,
      T addIdentity,
      T mulIdentity,
      IEqualityComparer<T>? comparer = null)
  {
    if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
    N = n;
    Add = add ?? throw new ArgumentNullException(nameof(add));
    Mul = mul ?? throw new ArgumentNullException(nameof(mul));
    AddIdentity = addIdentity;
    MulIdentity = mulIdentity;
    this.comparer = comparer ?? EqualityComparer<T>.Default;

    data = new T[n * n];
    Array.Fill(data, addIdentity);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private int Idx(int i, int j) => i * N + j;

  public T this[int i, int j]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => data[Idx(i, j)];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    set => data[Idx(i, j)] = value;
  }

  public static Matrix<T> Identity(
      int n,
      Func<T, T, T> add,
      Func<T, T, T> mul,
      T addIdentity,
      T mulIdentity,
      IEqualityComparer<T>? comparer = null)
  {
    var m = new Matrix<T>(n, add, mul, addIdentity, mulIdentity, comparer);
    for (int i = 0; i < n; i++)
      m.data[i * n + i] = mulIdentity;
    return m;
  }

  public Matrix<T> Multiply(Matrix<T> other)
  {
    if (other is null) throw new ArgumentNullException(nameof(other));
    if (N != other.N) throw new ArgumentException("Matrix size mismatch.");

    int n = N;
    var res = new Matrix<T>(n, Add, Mul, AddIdentity, MulIdentity, comparer);

    var a = data;
    var b = other.data;
    var c = res.data;

    var add = Add;
    var mul = Mul;
    var zero = AddIdentity;
    var eq = comparer;

    for (int i = 0; i < n; i++)
    {
      int rowA = i * n;
      int rowC = i * n;

      for (int k = 0; k < n; k++)
      {
        T aik = a[rowA + k];
        if (eq.Equals(aik, zero)) continue;

        int rowB = k * n;
        for (int j = 0; j < n; j++)
        {
          int idx = rowC + j;
          c[idx] = add(c[idx], mul(aik, b[rowB + j]));
        }
      }
    }

    return res;
  }

  public Matrix<T> Pow(long exp)
  {
    if (exp < 0) throw new ArgumentOutOfRangeException(nameof(exp));

    int n = N;
    var result = Identity(n, Add, Mul, AddIdentity, MulIdentity, comparer);
    var baseMat = this;
    long e = exp;

    while (e > 0)
    {
      if ((e & 1) != 0) result = result.Multiply(baseMat);
      e >>= 1;
      if (e > 0) baseMat = baseMat.Multiply(baseMat);
    }

    return result;
  }
}

readonly struct ModInt : IEquatable<ModInt>
{
  private readonly long _value;

  private static long _mod;
  private static bool _modIsSet;

  public long Value => _value;
  public static long Mod => _mod;

  public static void SetMod(long mod)
  {
    if (mod <= 1) throw new ArgumentOutOfRangeException(nameof(mod), "mod must be > 1.");
    _mod = mod;
    _modIsSet = true;
  }

  private static void EnsureModSet()
  {
    if (!_modIsSet)
      throw new InvalidOperationException("Mod is not set. Call ModInt.SetMod(mod) first.");
  }

  private static long Normalize(long x)
  {
    EnsureModSet();
    x %= _mod;
    if (x < 0) x += _mod;
    return x;
  }

  public ModInt(long value)
  {
    _value = Normalize(value);
  }

  public static implicit operator ModInt(int x) => new ModInt(x);
  public static implicit operator ModInt(long x) => new ModInt(x);

  public static explicit operator int(ModInt x) => checked((int)x._value);
  public static explicit operator long(ModInt x) => x._value;

  private static long MulMod(long a, long b)
  {
    return (long)(((BigInteger)a * b) % _mod);
  }

  public ModInt Pow(long exp)
  {
    if (exp < 0) throw new ArgumentOutOfRangeException(nameof(exp));
    EnsureModSet();

    long baseVal = _value;
    long result = 1 % _mod;

    while (exp > 0)
    {
      if ((exp & 1) != 0) result = MulMod(result, baseVal);
      baseVal = MulMod(baseVal, baseVal);
      exp >>= 1;
    }

    return new ModInt(result);
  }

  public ModInt Inverse()
  {
    if (_value == 0) throw new DivideByZeroException();
    return Pow(_mod - 2);
  }

  public static ModInt operator +(ModInt a, ModInt b)
  {
    EnsureModSet();
    long v = a._value + b._value;
    if (v >= _mod) v -= _mod;
    return new ModInt(v);
  }

  public static ModInt operator -(ModInt a, ModInt b)
  {
    EnsureModSet();
    long v = a._value - b._value;
    if (v < 0) v += _mod;
    return new ModInt(v);
  }

  public static ModInt operator *(ModInt a, ModInt b)
  {
    EnsureModSet();
    return new ModInt(MulMod(a._value, b._value));
  }

  public static ModInt operator /(ModInt a, ModInt b)
  {
    return a * b.Inverse();
  }

  public static ModInt operator -(ModInt a)
  {
    EnsureModSet();
    return a._value == 0 ? a : new ModInt(_mod - a._value);
  }

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

  public bool Equals(ModInt other) => _value == other._value;
  public override bool Equals(object? obj) => obj is ModInt other && Equals(other);
  public override int GetHashCode() => _value.GetHashCode();
  public override string ToString() => _value.ToString();
}

class Count
{
  private readonly int n;
  private readonly long mod;
  private readonly long[] fact;
  private readonly long[] inv;

  public Count(int n, long mod)
  {
    this.n = n;
    this.mod = mod;
    fact = new long[n];

    fact[0] = fact[1] = 1 % mod;
    for (int i = 2; i < n; ++i) fact[i] = fact[i - 1] * i % mod;

    inv = new long[n];
    inv[n - 1] = Sugaku.ModPow(fact[n - 1], mod - 2, mod);
    for (int i = n - 2; i >= 0; --i) inv[i] = inv[i + 1] * (i + 2) % mod;
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
     => a+b;
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

    for(int i=0; i<n; ++i) v[i+n]=(i<data.Count) ? data[i] : ide;
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
    v[i+n]=val;
    for(int u=(i+n)/2; u>0; u>>=1)
    {
      v[u]=op(v[2*u], v[2*u + 1]);
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
    T item=v[--count];
    v[count]=default!;
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
  public static IEnumerable<(int, int, int)> Adjacent(int i, int j, int H, int W, int p=4)
  {
    for(int k=0; k<p; ++k)
    {
      int ni=i+D[k], nj=j+D[k+1];
      if(ni<0 || nj<0 || ni>=H || nj>=W) continue;
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
    var a=new int[k];
    for(int i=0; i<k; ++i) a[i]=i;
    
    while(true)
    {
      action(a.AsSpan());

      int i=k-1;
      for(; i>=0; --i) if(a[i]!=i+n-k) break;
      if(i<0) return;
      a[i]++;
      for(int j=i+1; j<k; ++j) a[j]=a[j-1]+1;
    }
  }
  public static IEnumerable<int> Rep(int M)
  {
    for(int i=0; i<M; ++i) yield return i;
  }

  public static UnionFind SCC(List<int>[] graph, List<int>[] inv)
  {
    int N=graph.Length;

    var seen=Nms.Array(N, false);
    var ck=Nms.Stack<int>();
    
    var dfs=Nms.Stack<(int, bool)>();

    for(int i=0; i<N; ++i) if (!seen[i])
    {
      dfs.Push((i, false));
      while(dfs.Count>0)
      {
        var (u, post) = dfs.Pop();

        if(post)
        {
          ck.Push(u); 
          continue;
        }

        seen[u]=true;
        dfs.Push((u, true));
        foreach(var v in graph[u]) if(!seen[v]) dfs.Push((v, false));
      }
    }

    Array.Fill(seen, false);

    var uf = new UnionFind(N);

    while(ck.Count>0)
    {
      var s=ck.Pop();
      if(seen[s]) continue;
      dfs.Push((s, true));

      while(dfs.Count>0)
      {
        var (u, _)=dfs.Pop();
        uf.Merge(u, s);
        foreach(var v in inv[u]) if(!seen[v])
        {
          seen[v]=true;
          dfs.Push((v, true));
        }
      }
    }

    return uf;
  }

  // sus?
  public static T[] RootedTree<T>(List<int>[] edges, Func<T, T, T> op, T ide) where T: IBinaryInteger<T>
  {
    int N=edges.Length;
    var dp=Nms.Array(N, T.Zero);

    void bottomup(int u, int p)
    {
      dp[u]=ide;
      foreach(var v in edges[u]) if(v != p)
      {
        bottomup(v, u);
        dp[u]=op(dp[u], dp[v]);
      }
    }
    bottomup(0, -1);

    var res=Nms.Array(N, T.Zero);

    void topdown(int u, int p)
    {
      int M=edges[u].Count;

      var sdp = Nms.SubArray<T>(dp, edges[u].ToArray());
      
      var prefix=Nms.PrefixFold<T>(sdp, ide, op);
      var suffix=Nms.SuffixFold<T>(sdp, ide, op);

      res[u]=prefix[M-1];

      for(int i=0; i<M; ++i) if(edges[u][i]!=p)
      {
        dp[u]=op(i>0 ? prefix[i-1] : ide, i+1<M ? suffix[i+1] : ide);
        topdown(edges[u][i], u);
      }
    }
    topdown(0, -1);

    return res;
  }
}

static class Sugaku
{
  public const long MOD7 = 1000000007L;
  public const long MOD3 = 998244353L;
  public const int INF = 1001001001;
  public const long LINF = 1001001001001001001L;

  public static string ToBase(long N, int k)
  {
    if(k>=10 || k<1) throw new Exception("Invalid base number.");
    string res="";
    while(N>0)
    {
      char c=(char)((N%k)+'0');
      res+=c;
      N/=k;
    }
    return res.Reverse();
  }

  public static double Median<T>(ReadOnlySpan<T> a) where T : INumber<T>
  {
    if (a.Length == 0) throw new ArgumentException("empty");

    var b=a.ToArray();
    Array.Sort(b);

    int n=b.Length;
    if((n&1)==1) return double.CreateChecked(b[n/2]);
    return double.CreateChecked(b[n/2-1]+b[n/2]) / 2.0;
  }

  public static T MinPositivePreferred<T>(T a, T b) where T : INumber<T>
  {
    bool pa=a>T.Zero;
    bool pb=b>T.Zero;

    if(pa&&pb) return T.Min(a, b);
    if(pa) return a;
    if(pb) return b;
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
      val = val * 10 + c - '0';
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