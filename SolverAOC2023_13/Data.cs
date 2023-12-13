using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_13
{
  internal class Data : DataBase
  {


    public List<Map> Maps { get; set; } = new List<Map>();

    public Data(string input) : base(input)
    {
      // optionaly parse data
      List<string> mapLines = new List<string>();
      for(int i = 0; i < Lines.Count; i++)
      {
        string line = Lines[i];
        if(string.IsNullOrEmpty(line))
        {
          Map mapTmp = new Map(mapLines);
          Maps.Add(mapTmp);
          mapLines.Clear();
        }
        else
        {
          mapLines.Add(line);
        }
      }
      Map m = new Map(mapLines);
      Maps.Add(m);
    }

    public object Solve1()
    {
      long result = 0;
      foreach(Map map in Maps)
      {
        long tmpRes = map.Solve();
        result += tmpRes;
      }
      //12864
      return result;
    }

    public object Solve2()
    {
      long result = 0;
      foreach (Map map in Maps)
      {
        long tmpRes = map.Solve2();
        result += tmpRes;
      }
      //12864
      return result;
    }


  }
}
