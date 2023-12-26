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

      long S_lowerThenX = long.MaxValue;
      long V_greaterThenX = long.MinValue;

      long S_greaterThenX = long.MinValue;
      long V_lowerThenX = long.MinValue;
      long S_greaterThenY = long.MinValue;
      long V_lowerThenY = long.MinValue;
      long S_greaterThenZ = long.MinValue;
      long V_lowerThenZ = long.MinValue;

      S_lowerThenX = (long)Hailstones.Min(x=>x.Position.X);
      V_greaterThenX = (long)Hailstones.Max(x=>x.V_X);

      S_greaterThenX = (long)Hailstones.Max(x => x.Position.X);
      V_lowerThenX = (long)Hailstones.Min(x => x.V_X);
      S_greaterThenY = (long)Hailstones.Max(x => x.Position.Y);
      V_lowerThenY = (long)Hailstones.Min(x => x.V_Y);
      S_greaterThenZ = (long)Hailstones.Max(x => x.Position.Z);
      V_lowerThenZ = (long)Hailstones.Min(x => x.V_Z);


      Coords coord = new Coords(S_greaterThenX, S_greaterThenY, S_greaterThenZ);
      double V_X = V_lowerThenX - 1;
      double V_Y = V_lowerThenY - 1;
      double V_Z = V_lowerThenZ - 1;

      bool change = true;
      while(change) 
      {
        change = false;

        Hailstone hs = new Hailstone(coord, V_X, V_Y, V_Z);
        foreach (Hailstone hsTarget in Hailstones)
        {
          double collisionTimeX = hs.GetCollisionTimeX(hsTarget);
          if (collisionTimeX < 0)
          {
            coord.X++;
            change = true;
          }
          double collisionTimeY = hs.GetCollisionTimeY(hsTarget);
          if (collisionTimeY < 0)
          {
            coord.Y++;
            change = true;
          }
          double collisionTimeZ = hs.GetCollisionTimeZ(hsTarget);
          if (collisionTimeZ < 0)
          {
            coord.Z++;
            change = true;
          }
          Debug.WriteLine($"{hs} | {collisionTimeX} {collisionTimeY} {collisionTimeZ}");
        }
      }

      //for (int i = 19; i <= 25; i++)
      //{
      //  for (int j = -3; j > -20; j--)
      //  {
      //    Coords c = new Coords(i, 20, 20);
      //    Hailstone hs = new Hailstone(c, j, 20,20);

      //    foreach (Hailstone hsTarget in Hailstones)
      //    {
      //      double collisionTimeX = hs.GetCollisionTimeX(hsTarget);
      //      double collisionTimeY = hs.GetCollisionTimeY(hsTarget);
      //      double collisionTimeZ = hs.GetCollisionTimeZ(hsTarget);
      //      Debug.WriteLine($"S:{i}, V:{j}   | {collisionTimeX} {collisionTimeY} {collisionTimeZ}");
      //    }
      //    Debug.WriteLine("");
      //  }
      //}
      return 0;
    }
  }
}
