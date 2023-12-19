using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_19
{
  internal class Rating
  {

    public string RawInput { get; }

    public long X { get; set;}
    public long M { get; set;} 
    public long A { get; set;}
    public long S { get; set; }

    public long Value
    {
      get
      {
        return X + M + A + S;
      }
    }

    public Rating(long x, long m, long a, long s)
    {
      X = x;
      M = m;
      A = a;
      S = s;
    }

    public Rating(string input)
    {
      RawInput = input;
      string tmp = RawInput.Substring(1, RawInput.Length - 2);

      string[] items = tmp.Split(',');

      X = long.Parse(items[0].Split('=')[1]);
      M = long.Parse(items[1].Split('=')[1]);
      A = long.Parse(items[2].Split('=')[1]);
      S = long.Parse(items[3].Split('=')[1]);
    }

    public override string ToString()
    {
      return $"{RawInput}| x = {X}, m = {M}, a = {A}, s = {S}";
    }
  }
}
