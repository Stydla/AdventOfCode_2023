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

    public object Solve1()
    {
      Map current = Map;
      current.AddForSolve(current.StartField);

      for(int i = 0; i < Steps1; i++)
      {
        current = current.GetNextMap();
      }
      
      return current.GetRachableCount();
    }

    public object Solve2()
    {
      Map current = Map;
      current.ForSolve.Add(current.StartField.Location, current.StartField);
      for (int i = 0; i < Steps2; i++)
      {
        current = current.GetNextMap();
      }
      //Debug.WriteLine(current.Print());

      long res = current.Solve2(Steps2);
      return res;

    }


  }
}

