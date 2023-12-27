using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Data : DataBase
  {

    public Map Map { get; set; }

    public Data(string input) : base(input)
    {
      // optionaly parse data


    }

    public object Solve1()
    {
      Map = new Map(Lines);
      int ret = Map.Solve();
      return ret;
    }

    public object Solve2()
    {
      List<string> lines = new List<string>();
      foreach(string line in Lines) 
      {
        string newLine = line.Replace('>', '.');
        newLine = newLine.Replace('v', '.');
        newLine = newLine.Replace('<', '.');
        newLine = newLine.Replace('^', '.');
        lines.Add(newLine);
      }

      Map = new Map(lines);
      int ret = Map.Solve2();
      return ret;
    }


  }
}
