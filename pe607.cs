using System.Text;using System.Numerics;using System.Runtime.CompilerServices;using System;using System.Collections.Generic;using System.Linq;using System.IO;using System.Security.Cryptography;using System.Buffers;using System.Diagnostics;
#nullable enable


var fs = new FastScanner();
var sb = new StringBuilder();


double T=13.4738;
double v=10;
double D=100;

static double sigma(Func<int, double> f, int s, int t)
{
  double res=0;
  for(int i=s; i<=t; ++i) res+=f(i);
  return res;
}

// marsh angle
var mangle=Math.Asin((T*v - D) / (10 * sigma((k)=>10.0/k - 1, 5, 9)));
// Console.WriteLine("mangle: " + mangle);

// perpendicular angle
var pangle = Math.PI / 2.0;

//angle, height diff
(double, double) nxt(double u, double D, double p)
  => (Math.Asin(p*Math.Sin(pangle-u)), D * Math.Sin(u) * Math.Sin(mangle) / Math.Sin(mangle - u));

// appropriate AoI for y-value to culminates in 0
double solve(double t)
{
  var fangle = pangle + t - mangle;

  var (zangle, zdiff)=nxt(fangle, 10, 9/10.0);
  var (aangle, adiff)=nxt(zangle, 5, 8/9.0);
  var (bangle, bdiff)=nxt(aangle, 5, 7/8.0);
  var (cangle, cdiff)=nxt(bangle, 5, 6/7.0);
  var (dangle, ddiff)=nxt(cangle, 5, 5/6.0);

  
  return zdiff+adiff+bdiff+cdiff+ddiff;
}

double aoi=0, akane=mangle;
int K=10000;
while(K-->0)
{
  var m=aoi+(akane-aoi)/2;

  if(solve(m) <= 0) aoi=m;
  else akane=m;
}

sb.Append("aoi: " + aoi).AppendLine();

//angle, distance traveled
(double, double) finishup(double u, double D, double p)
  => (Math.Asin(p*Math.Sin(pangle-u)), D * Math.Sin(mangle-u) * Math.Sin(pangle-mangle));

// t = d / v
double dsum=0;
{
  var (zangle, zdiff)=finishup(aoi, 10, 9/10.0);
  var (aangle, adiff)=finishup(pangle-zangle, 5, 8/9.0);
  var (bangle, bdiff)=finishup(pangle-aangle, 5, 7/8.0);
  var (cangle, cdiff)=finishup(pangle-bangle, 5, 6/7.0);
  var (dangle, ddiff)=finishup(pangle-cangle, 5, 5/6.0);

  dsum=zdiff + adiff * 10 / 9.0 + bdiff * 10 / 8.0 + cdiff * 10 / 7.0 + ddiff * 10 / 6.0;
}

double t=dsum/10;
sb.Append(t.ToString("F10")).AppendLine();




















Console.Write(sb.ToString());
Console.Out.Flush();
Environment.Exit(0);

static class Graph
{
  public static readonly int[] D = [-1, 0, 1, 0, -1, -1, 1, 1, -1];}

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
