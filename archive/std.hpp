//repository: https://github.com/TrueRyoB/AlgoSubmission
#ifndef LIB_STD
#define LIB_STD

#include <vector>
#include <iostream>
#include <numeric>
#include <cmath>
#include <functional>
#include <cstdint>
#include <string>
#include <cassert>

#include <algorithm>
#include <set>
#include <queue>

using namespace std;
using ll = long long;

constexpr ll LINF = 1001001001001001001ll;
constexpr int d[] = {0, -1, 0, 1, 0};
template <typename T> inline void chmin(T& a, const T& b) {if (a > b) a = b; }
template <typename T> inline void chmax(T& a, const T& b) {if (a < b) a = b; }

#define endl '\n'
template <typename T> ostream& operator<<(ostream& os, const vector<T>& v) { for(const auto& e:v)os<<e<<'\t'; return os; }

template<typename T> inline int sz(const vector<T>&v) { return (int)v.size(); }
inline int sz(const string&v) { return (int)v.size(); }
template<typename T> inline int sz(const set<T>&v) { return (int)v.size(); }

constexpr ll pow10(int n) {
  ll x=1;
  while(n--) x*=10;
  return x;
}

#define mint Modint<MOD>

#endif