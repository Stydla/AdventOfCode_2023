using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_17
{
  internal class Data : DataBase
  {

    public Dictionary<Point2D, Field> AllFields { get; } = new Dictionary<Point2D, Field>();
    public Field StartField { get;  }
    public Field EndField { get; }

    public Data(string input) : base(input)
    {
      // optionaly parse data

      for(int i = 0; i < Lines.Count; i++)
      {
        string line = Lines[i]; 
        for(int j = 0; j < line.Length; j++)
        {
          int val = line[j] - '0';
          Point2D location = new Point2D(j, i);
          Field f = new Field(location, val);
          AllFields.Add(location, f);
        }
      }
      StartField = AllFields[new Point2D(0, 0)];
      EndField = AllFields[new Point2D(AllFields.Values.Max(x => x.Location.X), AllFields.Values.Max(x => x.Location.Y))];
    }


    public int Solve(bool part2)
    {
      SortedList<int, List<Path>> paths = new SortedList<int, List<Path>>();

      Path path = new Path();
      PathStep pathItem = new PathStep(EDirection4.RIGHT, StartField);
      path.Add(pathItem);
      if (paths.ContainsKey(path.Distance))
      {
        paths[path.Distance].Add(path);
      }
      else
      {
        paths.Add(path.Distance, new List<Path>());
        paths[path.Distance].Add(path);
      }


      path = new Path();
      pathItem = new PathStep(EDirection4.DOWN, StartField);
      path.Add(pathItem);
      if (paths.ContainsKey(path.Distance))
      {
        paths[path.Distance].Add(path);
      }
      else
      {
        paths.Add(path.Distance, new List<Path>());
        paths[path.Distance].Add(path);
      }

      while (paths.Count > 0)
      {
        int key = paths.First().Key;
        Path bestPath = paths[key].First();
        List<Path> nextPaths = bestPath.GetNextPaths(AllFields, part2);
        paths[key].Remove(bestPath);
        if (paths[key].Count == 0)
        {
          paths.Remove(key);
        }

        foreach (Path nextPath in nextPaths)
        {
          if (!paths.ContainsKey(nextPath.Distance))
          {
            paths.Add(nextPath.Distance, new List<Path>());
          }
          paths[nextPath.Distance].Add(nextPath);
        }
      }

      if(part2)
      {
        var tmp = EndField.Paths.Where(x => new EntranceKey(x.Value.Directions).IsValid2(true));
        return tmp.Min(x => x.Value.Distance) - StartField.Value;
      }

      return EndField.Paths.Min(x => x.Value.Distance) - StartField.Value;
    }

    public object Solve1()
    {
      return Solve(false);
    }


    public object Solve2()
    {
      return Solve(true);
    }


  }
}
