using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_21
{
  internal class Data : DataBase
  {

    public Map Map { get; set; }
    public int Steps1 { get; set; }
    public int Steps2 { get; set; }

    public Data(string input) : base(input)
    {

      Steps1 = int.Parse(Lines[0]);
      Steps2 = int.Parse(Lines[1]);
      Map = new Map(Lines.Skip(2).ToList());
      Map.StartField.Value = true;
    
    }

    public Map CreateDefaultMap()
    {
      Map tmp = new Map(Lines.Skip(2).ToList());
      tmp.StartField.Value = true;
      tmp.AddForSolve(tmp.StartField);
      return tmp;
    }

    public object Solve1()
    {
      Map current = Map;
      current.AddForSolve(current.StartField);

      int iterationCount = 2;
      while (true)
      {
        long res1 = CreateDefaultMap().Solve(Steps1, iterationCount);
        long res2 = CreateDefaultMap().Solve(Steps1, iterationCount + 1);
        iterationCount++;
        if (res1 == res2) return res1;
      }

    }

    public object Solve2()
    {
      Map current = Map;
      current.AddForSolve(current.StartField);

      int iterationCount = 2;
      while (true)
      {
        long res1 = CreateDefaultMap().Solve(Steps2, iterationCount);
        long res2 = CreateDefaultMap().Solve(Steps2, iterationCount + 1);
        iterationCount++;
        if (res1 == res2) return res1;
      }


    }


  }
}

