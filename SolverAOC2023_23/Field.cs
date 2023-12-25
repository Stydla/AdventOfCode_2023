using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Field
  {

    public Point2D Location { get; }

    public char InputType { get; }

    public Field(Point2D location, char inputType)
    {
      Location = location;
      InputType = inputType;
    }

    internal Dictionary<Field, SlopeDirection> GetSlopeNeighbours(Dictionary<Point2D, Field> allFields)
    {
      Dictionary<Field, SlopeDirection> ret = new Dictionary<Field, SlopeDirection>();

      foreach (var kv in Location.GetNeightboursDict4())
      {
        if (!allFields.ContainsKey(kv.Value)) continue;


        Field neighbour = allFields[kv.Value];
        EDirection4 dir = kv.Key;
        switch (neighbour.InputType)
        {
          case '^':
            if (dir == EDirection4.UP) ret.Add(neighbour, SlopeDirection.OUT);
            if (dir == EDirection4.DOWN) ret.Add(neighbour, SlopeDirection.IN);
            break;
          case '>':
            if (dir == EDirection4.RIGHT) ret.Add(neighbour, SlopeDirection.OUT);
            if (dir == EDirection4.LEFT) ret.Add(neighbour, SlopeDirection.IN);
            break;
          case 'v':
            if (dir == EDirection4.DOWN) ret.Add(neighbour, SlopeDirection.OUT);
            if (dir == EDirection4.UP) ret.Add(neighbour, SlopeDirection.IN);
            break;
          case '<':
            if (dir == EDirection4.LEFT) ret.Add(neighbour, SlopeDirection.OUT);
            if (dir == EDirection4.RIGHT) ret.Add(neighbour, SlopeDirection.IN);
            break;
        }
      }
      return ret;
    }

      internal List<Field> GetEmptyNeighbours(Dictionary<Point2D, Field> allFields)
    {
      List<Field> ret = new List<Field>();

      foreach(var kv in Location.GetNeightboursDict4())
      {
        if (!allFields.ContainsKey(kv.Value)) continue;


        Field neighbour = allFields[kv.Value];
        EDirection4 dir = kv.Key;
        switch(neighbour.InputType)
        {
          case '.':
            ret.Add(neighbour);
            break;
          case '#':
            break;
        }
      }
      return ret;
    }

  }

  public enum SlopeDirection
  {
    IN,
    OUT
  }
}
