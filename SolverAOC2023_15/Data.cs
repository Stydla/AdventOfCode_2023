using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_15
{
  internal class Data : DataBase
  {

    private List<Hash> Hashes { get; } = new List<Hash>();

    public Data(string input) : base(input)
    {
      // optionaly parse data
      string[] tmp = input.Split(',');

      foreach (string s in tmp)
      {
        Hash h = new Hash(s);
        Hashes.Add(h);
      }

    }

    public object Solve1()
    {
      long total = 0;
      foreach(Hash h in Hashes)
      {
        total += h.Value;
      }
      return total;
    }

    public object Solve2()
    {
      Boxes b = new Boxes();
      foreach (Hash h in Hashes)
      {
        b.SolveHash(h);
      }
      return b.GetFocusingPower();
    }


  }
}
