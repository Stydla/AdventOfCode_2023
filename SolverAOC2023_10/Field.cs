using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_10
{
  internal class Field
  {

    public EFieldType FieldType { get; set; } = EFieldType.Unknown;
    public char OriginalType { get; }

    public Point2D Location { get; }
    public int Distance { get; set; }

    public List<EDirection4> Directions { get; } = new List<EDirection4>();
    public List<Field> Neighbours { get; private set; } = new List<Field>();



    public List<List<Point2D>> Sides { get; private set; } = new List<List<Point2D>>()
    {
      new List<Point2D>(),
      new List<Point2D>()
    };

    public int OutsideIndex { get; set; } = -1;

    public List<Point2D> OutsidePoints
    {
      get
      {
        if(OutsideIndex != - 1)
        {
          return Sides[OutsideIndex];
        }
        return null;
      }
    }
    public List<Point2D> InsidePoints
    {
      get
      {
        if (OutsideIndex != -1)
        {
          return Sides[(OutsideIndex+1) % 2];
        }
        return null;
      }
    }


    public Field(char originalType, Point2D location)
    {
      OriginalType = originalType;
      Location = location;
      switch(OriginalType)
      {
        case '|':
          {
            Directions.Add(EDirection4.UP);
            Directions.Add(EDirection4.DOWN);
            
            break;
          }
        case '-':
          {
            Directions.Add(EDirection4.LEFT);
            Directions.Add(EDirection4.RIGHT);
            
            break;
          }
        case 'L':
          {
            Directions.Add(EDirection4.UP);
            Directions.Add(EDirection4.RIGHT);
            
            break;
          }
        case 'J':
          {
            Directions.Add(EDirection4.UP);
            Directions.Add(EDirection4.LEFT);
            
            break;
          }
        case '7':
          {
            Directions.Add(EDirection4.LEFT);
            Directions.Add(EDirection4.DOWN);
            
            break;
          }
        case 'F':
          {
            Directions.Add(EDirection4.RIGHT);
            Directions.Add(EDirection4.DOWN);
            
            break;
          }
        case '.':
        case 'S':
          {
            break;
          }
        default:
          {
            throw new Exception($"Unknown input character {OriginalType}");
          }
      }
      AddSides();
    }

    private void AddSides()
    {
      if(Directions.Contains(EDirection4.UP) &&  Directions.Contains(EDirection4.DOWN))
      {
        Sides[0].Add(Location.UL());
        Sides[0].Add(Location.L());
        Sides[0].Add(Location.DL());
        Sides[1].Add(Location.UR());
        Sides[1].Add(Location.R());
        Sides[1].Add(Location.DR());
      }
      if(Directions.Contains(EDirection4.RIGHT) && Directions.Contains(EDirection4.LEFT))
      {
        Sides[0].Add(Location.UL());
        Sides[0].Add(Location.U());
        Sides[0].Add(Location.UR());
        Sides[1].Add(Location.DL());
        Sides[1].Add(Location.D());
        Sides[1].Add(Location.DR());
      }
      if(Directions.Contains(EDirection4.UP) && Directions.Contains(EDirection4.RIGHT))
      {
        Sides[0].Add(Location.UL());
        Sides[0].Add(Location.L());
        Sides[0].Add(Location.DL());
        Sides[0].Add(Location.D());
        Sides[0].Add(Location.DR());
        Sides[1].Add(Location.UR());
      }
      if (Directions.Contains(EDirection4.UP) && Directions.Contains(EDirection4.LEFT))
      {
        Sides[0].Add(Location.UL());
        Sides[1].Add(Location.UR());
        Sides[1].Add(Location.R());
        Sides[1].Add(Location.DR());
        Sides[1].Add(Location.D());
        Sides[1].Add(Location.DL());
      }
      if (Directions.Contains(EDirection4.LEFT) && Directions.Contains(EDirection4.DOWN))
      {
        Sides[0].Add(Location.DL());
        Sides[1].Add(Location.UL());
        Sides[1].Add(Location.U());
        Sides[1].Add(Location.UR());
        Sides[1].Add(Location.R());
        Sides[1].Add(Location.DR());
      }
      if (Directions.Contains(EDirection4.RIGHT) && Directions.Contains(EDirection4.DOWN))
      {
        Sides[0].Add(Location.DR());
        Sides[1].Add(Location.DL());
        Sides[1].Add(Location.L());
        Sides[1].Add(Location.UL());
        Sides[1].Add(Location.U());
        Sides[1].Add(Location.UR());
      }
    }

    public char NewType
    {
      get
      {
        switch(OriginalType)
        {
          case 'L': return '└';
          case 'J': return '┘';
          case '7': return '┐';
          case 'F': return '┌';
          case '-': return '─';
          case '|': return '│';
          default: return OriginalType;

        }
      }
    }

    public char NewType2
    {
      get
      {
        switch (FieldType)
        {
          case EFieldType.Unknown:
            return '?';
          case EFieldType.Pipe:
            switch (OriginalType)
            {
              case 'L': return '└';
              case 'J': return '┘';
              case '7': return '┐';
              case 'F': return '┌';
              case '-': return '─';
              case '|': return '│';
              default: return OriginalType;
            }
          case EFieldType.Outside:
            return 'O';
          case EFieldType.Inside:
            return 'I';
        }
        return '!';
      }
    }

    public void AssignNeighbours(Dictionary<Point2D, Field> fields)
    {
      foreach(EDirection4 dir in Directions)
      {
        Point2D loc = Location.Move(dir);
        if(fields.ContainsKey(loc))
        {
          Field f = fields[loc];
          Neighbours.Add(f);
        }
      }
    }

    public Field GetNext(Field comeFrom)
    {
      return Neighbours.Where(x => x != comeFrom).First();
    }

    public Field GetNextAndAssignSide(Field comeFrom)
    {
      Field next = Neighbours.Where(x => x != comeFrom).First();

      for(int i = 0; i < 2; i++)
      {
        if (Sides[i].Any(x => comeFrom.OutsidePoints.Contains(x)))
        {
          OutsideIndex = i;
        }

        if (Sides[i].Any(x => comeFrom.InsidePoints.Contains(x)))
        {
          OutsideIndex = (i +1) % 2;
        }
      }
      return next;
    }

    internal void InitTube(Dictionary<Point2D, Field> allFields)
    {
      foreach(EDirection4 dir in Enum.GetValues(typeof(EDirection4)))
      {
        Point2D loc = Location.Move(dir);
        if(allFields.ContainsKey(loc))
        {
          if (allFields[loc].Neighbours.Contains(this))
          {
            Directions.Add(dir);
          }
        }
      }
      AssignNeighbours(allFields);
      AddSides();
    }
  }

  enum EFieldType
  {
    Unknown,
    Pipe,
    Outside,
    Inside
  }
}
