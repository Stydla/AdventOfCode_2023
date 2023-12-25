using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_24
{
  internal class Coords
  {
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Coords(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    public override string ToString()
    {
      return $"{X}, {Y}, {Z}";
    }
  }
}
