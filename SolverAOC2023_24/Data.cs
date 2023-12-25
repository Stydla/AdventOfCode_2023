using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_24
{
  internal class Data : DataBase
  {

    public List<Hailstone> Hailstones { get; set; } = new List<Hailstone>();

    public double MinX, MaxX, MinY, MaxY;

    public double EPS = 10e-20;

    public Data(string input) : base(input)
    {
      // optionaly parse data

      string[] limits = Lines[0].Split(',');
      MinX = double.Parse(limits[0]);
      MaxX = double.Parse(limits[1]);
      MinY = double.Parse(limits[2]);
      MaxY = double.Parse(limits[3]);

      for(int i = 1; i < Lines.Count; i++)
      {
        string line = Lines[i];
        Hailstone hs = new Hailstone(line);
        Hailstones.Add(hs);
      }
    }

    public object Solve1()
    {

      int cnt = 0;
      for (int i = 0; i < Hailstones.Count; i++)
      {
        Hailstone h1 = Hailstones[i];
        for(int j = i + 1; j < Hailstones.Count; j++)
        {
          Hailstone h2 = Hailstones[j];
          Collision collision = h1.GetCollisionPosition(h2);
          if(collision.Position.X + EPS >= MinX &&
            collision.Position.X - EPS <= MaxX &&
            collision.Position.Y + EPS >= MinY &&
            collision.Position.Y -EPS <= MaxY &&
            collision.CollisionTime1 >= 0 &&
            collision.CollisionTime2 >= 0)
          {
            cnt++;
          }
        }
      }

      return cnt;
    }


    public object Solve2()
    {

      long S_lowerThen = long.MaxValue;
      long V_greaterThen = long.MinValue;

      long S_greaterThen = long.MinValue;
      long V_lowerThen = long.MinValue;

      S_lowerThen = (long)Hailstones.Min(x=>x.Position.X);
      V_greaterThen = (long)Hailstones.Max(x=>x.V_X);

      S_greaterThen = (long)Hailstones.Max(x => x.Position.X);
      V_lowerThen = (long)Hailstones.Min(x => x.V_X);

      //for (int i = 19; i  <= 25; i++)
      //{
      //  for(int j = -3; j > -20; j--) 
      //  { 
      //    Coords c = new Coords(i, 0, 0);
      //    Hailstone hs = new Hailstone(c, j, 0, 0);

      //    foreach(Hailstone hsTarget in Hailstones)
      //    {
      //      double collisionTime = hs.GetCollisionTimeX(hsTarget);
      //      Debug.WriteLine($"S:{i}, V:{j}   | {collisionTime}");
      //    }
      //    Debug.WriteLine("");
      //  }
      //}
      return 0;
    }


  }
}
