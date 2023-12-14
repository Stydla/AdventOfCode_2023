using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_14
{
  internal class Data : DataBase
  {

    
    public Map Map { get; set; }

    public Data(string input) : base(input)
    {
      // optionaly parse data
      Map = new Map(Lines);
    }

    public object Solve1()
    {
      Map.Move(AoCLib.Enums.EDirection4.UP);
      return Map.GetValue();
    }


    public object Solve2()
    {
      Dictionary<string, int> Maps = new Dictionary<string, int>();
      Map tmpMap = Map;


      int index = 0;
      while(!Maps.ContainsKey(tmpMap.GetHash()))
      {
        Maps.Add(tmpMap.GetHash(), index++);
        tmpMap.Move(AoCLib.Enums.EDirection4.UP);
        tmpMap.Move(AoCLib.Enums.EDirection4.LEFT);
        tmpMap.Move(AoCLib.Enums.EDirection4.DOWN);
        tmpMap.Move(AoCLib.Enums.EDirection4.RIGHT);
        
      }
      int indexOfMap = Maps[tmpMap.GetHash()];
      int current = Maps.Count;

      int diff = current - indexOfMap;

      int missing = (1000000000 - current) % diff;

      for(int i = 0; i < missing; i++)
      {
        tmpMap.Move(AoCLib.Enums.EDirection4.UP);
        tmpMap.Move(AoCLib.Enums.EDirection4.LEFT);
        tmpMap.Move(AoCLib.Enums.EDirection4.DOWN);
        tmpMap.Move(AoCLib.Enums.EDirection4.RIGHT);
      }

      return tmpMap.GetValue();
    }


  }
}
