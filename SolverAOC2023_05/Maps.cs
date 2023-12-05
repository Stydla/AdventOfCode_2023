using AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_05
{
  internal class Maps
  {

    public List<MapItem> MapList { get; } = new List<MapItem>();

    public Maps(string input)
    {
      using(StringReader reader = new StringReader(input))
      {
        StringBuilder sbMap = new StringBuilder();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          if(line == "")
          {
            MapList.Add(new MapItem(sbMap.ToString()));
            sbMap.Clear();
          } else
          {
            sbMap.AppendLine(line);
          }
        }
        MapList.Add(new MapItem(sbMap.ToString()));
      }
    }

    internal long GetTarget(long seed)
    {
      long current = seed;
      foreach(MapItem mi in MapList)
      {
        current = mi.GetTarget(current);
      }
      return current;
    }

    internal long Solve2(Interval seedInterval)
    {

      List<Interval> current = new List<Interval>();
      current.Add(seedInterval);
      
      foreach (MapItem mi in MapList)
      {
        current = mi.Solve(current);
      }

      long res = current.Min(x => x.StartPoint.Value);

      return res; 
    }
  }
}
