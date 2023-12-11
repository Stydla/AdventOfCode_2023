using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_11
{
  internal class Field
  {

    public char Value { get; }
    public Point2D Location { get; }

    public Field(Point2D location, char value)
    {
      Value = value;
      Location = location;
    }

    public bool IsGalaxy()
    {
      return Value == '#';
    }

    public long GetDistance(Field other, long multiplier, List<int> vertical, List<int> horizontal)
    {
      long cnt1 = vertical.Count(x => x > Math.Min(Location.X, other.Location.X) && x < Math.Max(Location.X, other.Location.X));
      long cnt2 = horizontal.Count(x => x > Math.Min(Location.Y, other.Location.Y) && x < Math.Max(Location.Y, other.Location.Y));

      return Location.ManhattanDistance(other.Location) + (cnt1 + cnt2) * (multiplier -1);

    }

  }
}
