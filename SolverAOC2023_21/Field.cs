using AoCLib;
using AoCLib.BFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_21
{
  

  internal class Field : IBFSNode<BFSFieldContext>
  {


    public char Type { get; }

    public Point2D Location { get; }

    public bool IsStart { get; } = false;

    public bool Value { get; set; } = false;
   

    public Field(char type, Point2D location)
    {
      if(type == 'S')
      {
        Type = '.';
        IsStart = true;
      } else
      {
        Type = type;
      }
      Location = location;
    }

    private Field(Field original)
    {
      Type = original.Type;
      Location = original.Location;
      IsStart = original.IsStart;
      Value = original.Value;
    }

    internal Field Copy()
    {
      return new Field(this);
    }

    public IEnumerable<IBFSNode<BFSFieldContext>> GetNeighbours(BFSFieldContext context)
    {
      foreach(Point2D p in Location.GetNeightbours4())
      {
        if(context.AllFields.TryGetValue(p, out Field field))
        {
          yield return field;
        }

      }
    }

    public bool IsOpen(BFSFieldContext context)
    {
      return true;
      //if(context.VisitedFields.Contains(this))
      //{
      //  return false;
      //}
      //context.VisitedFields.Add(this);
      //return true;
    }

    public override string ToString()
    {
      return $"{Location}";
      

    }
  }

  internal class BFSFieldContext
  {
    public Dictionary<Point2D, Field> AllFields { get; }

    public HashSet<Field> VisitedFields { get; } = new HashSet<Field> { };

    public BFSFieldContext(Dictionary<Point2D, Field> allFields)
    {
      AllFields = allFields;
    }
  }
}
