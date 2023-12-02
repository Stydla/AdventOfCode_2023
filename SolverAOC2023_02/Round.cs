using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_02
{
  internal class Round
  {
    
    public Round(string round)
    {
      Regex r = new Regex("(\\d+) ((red)|(blue)|(green))");
      string[] colors = round.Split(',');
      foreach(string color in colors)
      {
        Match m = r.Match(color);
        int cnt = int.Parse(m.Groups[1].Value);

        if (m.Groups[3].Success) R = cnt;
        if (m.Groups[4].Success) B = cnt;
        if (m.Groups[5].Success) G = cnt;
      }
    }

    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
  }
}
