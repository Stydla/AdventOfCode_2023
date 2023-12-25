using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_24
{
  internal class Hailstone
  {

    public Coords Position { get; set; }

    public double V_X { get; set; }
    public double V_Y { get; set; }
    public double V_Z { get; set; }

    public Hailstone(string input)
    {
      string[] parts = input.Split('@');

      string[] pos = parts[0].Split(',').Select(a => a.Trim()).ToArray();
      string[] vel = parts[1].Split(',').Select(a => a.Trim()).ToArray();

      double x = double.Parse(pos[0]);
      double y = double.Parse(pos[1]);
      double z = double.Parse(pos[2]);
      Position = new Coords(x, y, z);

      V_X = double.Parse(vel[0]);
      V_Y = double.Parse(vel[1]);
      V_Z = double.Parse(vel[2]);
    }

    public Hailstone(Coords position, double v_X, double v_Y, double v_Z) 
    {
      Position = position;
      V_X = v_X;
      V_Y = v_Y;
      V_Z = v_Z;
    }

    public Collision GetCollisionPosition(Hailstone hs2)
    {

      double v1x = V_X;
      double v1y = V_Y;
      double v2x = hs2.V_X;
      double v2y = hs2.V_Y;

      double s1x = Position.X;
      double s1y = Position.Y;
      double s2x = hs2.Position.X;
      double s2y = hs2.Position.Y;

      double t2 = (v1x * (s2y - s1y) - v1y * (s2x - s1x)) / (v1y * v2x - v1x * v2y);
      double t1 = (s2x - s1x + v2x * t2) / v1x;

      double x = s2x + v2x * t2;
      double y = s2y + v2y * t2;
      double z = hs2.Position.Z + hs2.V_Z * t2;
      Coords position = new Coords(x, y, z);

      return new Collision(t1, t2, position);

    }

    public double GetCollisionTimeX(Hailstone hs)
    {
      return (hs.Position.X - Position.X) / (V_X - hs.V_X);
    }
  }
}

