using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20
{
  internal class Data : DataBase
  {

    public Machine Machine { get; set; }
    public Data(string input) : base(input)
    {
      // optionaly parse data
      Machine = new Machine(Lines);
    }

    public object Solve1()
    {

      for(int i = 0; i < 1000; i++)
      {
        Machine.PushButton();
      }
      return Machine.LowSignalCount * Machine.HighSignalCount;

    }

    public object Solve2()
    {

      Machine.InterestingModules.Add("ft");
      Machine.InterestingModules.Add("sv");
      Machine.InterestingModules.Add("ng");
      Machine.InterestingModules.Add("jz");

      Dictionary<string, ulong> counts = new Dictionary<string, ulong>();
     
      long cnt = 0;
      while (counts.Values.Count < 4)
      {
        Machine.PushButton2();
        cnt++;
        foreach (string module in Machine.InterestingModulesHit)
        {
          if (!counts.ContainsKey(module))
          {
            counts.Add(module, (ulong)cnt);
          }
        }
      }

      ulong res = 1;

      foreach (ulong val in counts.Values)
      {
        ulong gcd = ModuloMath.GCD(val, res);
        res = res * val / gcd;
      }

      return res;
    }

  }
}

