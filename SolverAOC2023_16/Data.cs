using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_16
{
  internal class Data : DataBase
  {

    public Dictionary<Point2D, Field> Fields { get; set; } 

    public Data(string input) : base(input)
    {
      // optionaly parse data
      Fields = new Dictionary<Point2D, Field>();
      int i = 0;
      foreach (string line in Lines)
      {
        int j = 0;
        foreach (char c in line)
        {
          Point2D p = new Point2D(j, i);
          Field f = new Field(p, c, Fields);
          Fields.Add(p, f);
          j++;
        }
        i++;
      }
    }

    private void Init()
    {
      foreach(Field f in Fields.Values)
      {
        f.Visited.Clear();
      }
    }

    public object Solve1()
    {
      return SolveFor(new Point2D(0, 0), EDirection4.RIGHT);
    }

    private int SolveFor(Point2D point, EDirection4 dir)
    {
      Init();

      List<MoveState> current = new List<MoveState>();
      List<MoveState> next = new List<MoveState>();

      Point2D start = point;
      Field first = Fields[start];
      current.Add(new MoveState(first, dir));

      while (current.Count > 0)
      {
        foreach (MoveState ms in current)
        {
          next.AddRange(ms.Move());
        }

        current = next;
        next = new List<MoveState>();
      }

      return Fields.Values.Count(x => x.Visited.Count > 0);
    }

    public object Solve2()
    {
      int max = 0;

      Init();
      int maxX = Fields.Values.Max(x=>x.Location.X);
      int maxY = Fields.Values.Max(y=>y.Location.Y);

      for(int i = 0; i <= maxX; i++)
      {
        int tmp;
        tmp = SolveFor(new Point2D(i, 0), EDirection4.DOWN);
        if (tmp > max) max = tmp;

        tmp = SolveFor(new Point2D(i, maxY), EDirection4.UP);
        if (tmp > max) max = tmp;
      }

      for (int i = 0; i <= maxY; i++)
      {
        int tmp;
        tmp = SolveFor(new Point2D(0, i), EDirection4.RIGHT);
        if (tmp > max) max = tmp;

        tmp = SolveFor(new Point2D(maxX, i), EDirection4.LEFT);
        if (tmp > max) max = tmp;
      }

      return max;
    }


  }
}
