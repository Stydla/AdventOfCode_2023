using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_16
{
  internal class Field
  {

    public Point2D Location { get; }

    public char Value { get; }

    public Dictionary<Point2D, Field> AllFields { get; }

    public Dictionary<EDirection4, bool> Visited { get; } = new Dictionary<EDirection4, bool>();  

    public Field(Point2D location, char value, Dictionary<Point2D, Field> allFields)
    {
      Location = location;
      Value = value;
      AllFields = allFields;
    }


    private void AddFieldIfExists(List<MoveState> list, EDirection4 dir)
    {
      Point2D next = Location.Move(dir);
      if(AllFields.ContainsKey(next))
      {
        list.Add(new MoveState(AllFields[next], dir));
      }
    }

    private void AddVisited(EDirection4 dir)
    {
      if (Visited.ContainsKey(dir)) return;
      Visited[dir] = true;
    }

    public List<MoveState> Move(EDirection4 direction)
    {
      List<MoveState> ret = new List<MoveState>();
      if (Visited.ContainsKey(direction)) return ret;

      AddVisited(direction);


      switch (Value)
      {
        case '-':
          {
            switch (direction)
            {
              case EDirection4.UP:
                AddFieldIfExists(ret, EDirection4.LEFT);
                AddFieldIfExists(ret, EDirection4.RIGHT);
                break;
              case EDirection4.RIGHT:
                AddFieldIfExists(ret, EDirection4.RIGHT);
                break;
              case EDirection4.DOWN:
                AddFieldIfExists(ret, EDirection4.LEFT);
                AddFieldIfExists(ret, EDirection4.RIGHT);
                break;
              case EDirection4.LEFT:
                AddFieldIfExists(ret, EDirection4.LEFT);
                break;
            }
            break;
          }
        case '|':
          {
            switch (direction)
            {
              case EDirection4.UP:
                AddFieldIfExists(ret, EDirection4.UP);
                break;
              case EDirection4.RIGHT:
                AddFieldIfExists(ret, EDirection4.UP);
                AddFieldIfExists(ret, EDirection4.DOWN);
                break;
              case EDirection4.DOWN:
                AddFieldIfExists(ret, EDirection4.DOWN);
                break;
              case EDirection4.LEFT:
                AddFieldIfExists(ret, EDirection4.UP);
                AddFieldIfExists(ret, EDirection4.DOWN);
                break;
            }
            break;
          }
        case '\\':
          {
            switch (direction)
            {
              case EDirection4.UP:
                AddFieldIfExists(ret, EDirection4.LEFT);
                break;
              case EDirection4.RIGHT:
                AddFieldIfExists(ret, EDirection4.DOWN);
                break;
              case EDirection4.DOWN:
                AddFieldIfExists(ret, EDirection4.RIGHT);
                break;
              case EDirection4.LEFT:
                AddFieldIfExists(ret, EDirection4.UP);
                break;
            }
            break;
          }
        case '/':
          {
            switch (direction)
            {
              case EDirection4.UP:
                AddFieldIfExists(ret, EDirection4.RIGHT);
                break;
              case EDirection4.RIGHT:
                AddFieldIfExists(ret, EDirection4.UP);
                break;
              case EDirection4.DOWN:
                AddFieldIfExists(ret, EDirection4.LEFT);
                break;
              case EDirection4.LEFT:
                AddFieldIfExists(ret, EDirection4.DOWN);
                break;
            }
            break;
          }
        case '.':
          {
            AddFieldIfExists(ret, direction);
            break;
          }
        default: throw new Exception($"Invalid value {Value}");
      }
      return ret;
    }

  }
}
