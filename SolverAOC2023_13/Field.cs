using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_13
{
  internal class Field
  {

    public char Value { get; private set; }
    public Point2D Location { get; }

    public Field(char value, Point2D location)
    {
      Value = value;
      Location = location;
    }

    internal void SwitchValue()
    {
      Value = Value == '.' ? '#' : '.';
    }
  }
}
