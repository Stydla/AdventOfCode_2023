using AoCLib;
using AoCLib.BFS;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  public class Field 
  {

    public Point2D Location { get; }

    public char InputType { get; set; }
    
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


    public void GetDistance(HashSet<Field> forbidden, HashSet<Field> visited, Dictionary<Point2D, Field> allFields, Field target, int currentDistance, ref int maxDistance)
    {
      if(this == target)
      {
        if(maxDistance < currentDistance) maxDistance = currentDistance;
        return;
      }

      if (forbidden.Contains(this)) return;
      if(visited.Contains(this)) return;

      visited.Add(this);
      currentDistance++;
      foreach (Field neighbour in this.GetEmptyNeighbours(allFields))
      {
        neighbour.GetDistance(forbidden, visited, allFields, target, currentDistance, ref maxDistance);
      }
      currentDistance--;
      visited.Remove(this);
    }

    
  }

  public enum SlopeDirection
  {
    IN,
    OUT
  }
}
