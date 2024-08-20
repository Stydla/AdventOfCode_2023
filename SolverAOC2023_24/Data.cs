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

      HashSet<int> impossibleX = new HashSet<int>();
      HashSet<int> impossibleY = new HashSet<int>();
      HashSet<int> impossibleZ = new HashSet<int>();
      for (int i = 0; i < Hailstones.Count; i++)
      {
        for(int j = i + 1; j < Hailstones.Count; j++)
        {
          Hailstone h1 = Hailstones[i];
          Hailstone h2 = Hailstones[j];
          if((h1.Position.X > h2.Position.X && h1.V_X > h2.V_X)||
            (h1.Position.X < h2.Position.X && h1.V_X < h2.V_X))
          {
            int min = (int)Math.Min(h1.V_X, h2.V_X);
            int max = (int)Math.Max(h1.V_X, h2.V_X);
            for(int remove = min; remove <= max; remove++)
            {
              if (!impossibleX.Contains(remove))
              {
                impossibleX.Add(remove);
              }
            }
          }

          if ((h1.Position.Y > h2.Position.Y && h1.V_Y > h2.V_Y) ||
            (h1.Position.Y < h2.Position.Y && h1.V_Y < h2.V_Y))
          {
            int min = (int)Math.Min(h1.V_Y, h2.V_Y);
            int max = (int)Math.Max(h1.V_Y, h2.V_Y);
            for (int remove = min; remove <= max; remove++)
            {
              if (!impossibleY.Contains(remove))
              {
                impossibleY.Add(remove);
              }
            }
          }

          if ((h1.Position.Z > h2.Position.Z && h1.V_Z > h2.V_Z) ||
            (h1.Position.Z < h2.Position.Z && h1.V_Z < h2.V_Z))
          {
            int min = (int)Math.Min(h1.V_Z, h2.V_Z);
            int max = (int)Math.Max(h1.V_Z, h2.V_Z);
            for (int remove = min; remove <= max; remove++)
            {
              if (!impossibleZ.Contains(remove))
              {
                impossibleZ.Add(remove);
              }
            }
          }
        }
      }

     


      int iBounds = (int)Hailstones.Max(x => x.V_X) - (int)Hailstones.Min(x => x.V_X);
      int jBounds = (int)Hailstones.Max(x => x.V_Y) - (int)Hailstones.Min(x => x.V_Y);
      int kBounds = (int)Hailstones.Max(x => x.V_Z) - (int)Hailstones.Min(x => x.V_Z);

      for (int i = 0; i <= iBounds; i++)
      {
        if(impossibleX.Contains(i)) continue;
        
        for (int j = -jBounds; j <= jBounds; j++)
        {
          if (impossibleY.Contains(j)) continue;
          for (int k = -kBounds; k <= kBounds; k++)
          {
            if (impossibleZ.Contains(k)) continue;

            List<Collision> collisions = new List<Collision>();

            Hailstone h = Hailstones[0];
            Hailstone h1 = new Hailstone(h.Position, h.V_X - i, h.V_Y - j, h.V_Z - k);

            for (int hi = 1; hi < Hailstones.Count; hi++)
            {
              Hailstone hNext = Hailstones[hi];

              Hailstone h2 = new Hailstone(hNext.Position, hNext.V_X - i, hNext.V_Y - j, hNext.V_Z - k);

              Collision collision = h1.GetCollisionPosition(h2);

              if(double.IsNaN(collision.CollisionTime1) || double.IsInfinity(collision.CollisionTime1))
              {
                continue;
              }
              collisions.Add(collision);
              h1 = h2;

              if (collisions.Count > 1)
              {
                double eps = 0.0000001;
                double EpsX = (collisions[0].Position.X + collisions.Last().Position.X + 10.0) * eps;
                double EpsY = (collisions[0].Position.Y + collisions.Last().Position.Y + 10.0) * eps;
                double EpsZ = (collisions[0].Position.Z + collisions.Last().Position.Z + 10.0) * eps;
                if (Math.Abs(collisions[0].Position.X - collisions.Last().Position.X) > EpsX ||
                  Math.Abs(collisions[0].Position.Y - collisions.Last().Position.Y) > EpsY ||
                  Math.Abs(collisions[0].Position.Z - collisions.Last().Position.Z) > EpsZ)
                {
                  break;
                }
              }
             
            }
            if (collisions.Count > 2)
            {
              return collisions[0].Position.X + collisions[0].Position.Y + collisions[0].Position.Z;
            }
          }
        }
      }


      return 0;
    }
  }
}
