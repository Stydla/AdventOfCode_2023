using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_17
{
  internal class Field
  {

    public Point2D Location { get; }

    public int Value { get;}

    public Dictionary<EntranceKey, Path> Paths { get; } = new Dictionary<EntranceKey, Path>();

    public Field(Point2D location, int value)
    {
      this.Location = location;
      this.Value = value;
    }

    public override string ToString()
    {
      return $"{Value} {Location}";
    }

  }
}
