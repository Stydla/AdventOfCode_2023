using AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_05
{
  internal class Data : DataBase
  {
    public List<long> Seeds { get; } = new List<long>();
    public Maps Maps { get; private set; }


    public Data(string input) : base(input)
    {
      string[] seedsStr = Lines[0].Substring(7).Split(' ');
      Seeds.AddRange(seedsStr.Select(x => long.Parse(x)));

      StringBuilder sb = new StringBuilder();
      for(int i = 2; i < Lines.Count; i++)
      {
        sb.AppendLine(Lines[i]);
      }
      Maps = new Maps(sb.ToString());
    }

    public object Solve1()
    {
      List<long> results = new List<long>();
      foreach(long seed in Seeds)
      {
        long target = Maps.GetTarget(seed);
        results.Add(target);
      }

      return results.Min();
    }

    public object Solve2()
    {
      List<long> results = new List<long>();
      for(int i = 0; i < Seeds.Count ; i += 2)
      {
        Interval intr = new Interval(Seeds[i], Seeds[i] + Seeds[i + 1] -1);
        long result = Maps.Solve2(intr);
        results.Add(result);
      }

      return results.Min();
    }


  }
}
