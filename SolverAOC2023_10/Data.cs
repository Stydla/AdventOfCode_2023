using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_10
{
  internal class Data : DataBase
  {


    public Dictionary<Point2D, Field> AllFields = new Dictionary<Point2D, Field>();
    public Field StartField { get; set; }
    public int MaxX { get; set; }
    public int MaxY { get; set; }
    public Data(string input) : base(input)
    {
      // optionaly parse data
      int x = 0, y = 0;
      foreach(string line in Lines)
      {
        Point2D first = new Point2D(-1, y);
        AllFields.Add(first, new Field('.', first));

        x = 0;
        foreach(char c in line)
        {
          Point2D location = new Point2D(x, y);
          Field f = new Field(c, location);
          AllFields.Add(location, f);
          x++;
        }

        Point2D last = new Point2D(x, y);
        AllFields.Add(last, new Field('.', last));

        y++;
      }

      MaxX = AllFields.Values.Max(a => a.Location.X);
      MaxY = AllFields.Values.Max(a => a.Location.Y);

      for(int i = -1; i <= MaxX; i++)
      {
        Point2D loc = new Point2D(i, -1);
        AllFields.Add(loc, new Field('.', loc));

        loc = new Point2D(i, MaxY + 1);
        AllFields.Add(loc, new Field('.', loc));
      }
      MaxY++;

      foreach (Field field in AllFields.Values)
      {
        field.AssignNeighbours(AllFields);
      }

      StartField = AllFields.Values.Where(a => a.OriginalType == 'S').First();

      StartField.InitTube(AllFields);

      SolveDistances();
      AssignOutside();

     
    }

    private bool AssignFieldTypes()
    {
      bool change = false;
      foreach(Field f in AllFields.Values.Where(x=>x.OutsideIndex != -1))
      {
        foreach(Point2D oPoint in f.OutsidePoints)
        {
          Field testField = AllFields[oPoint];
          if(testField.FieldType == EFieldType.Unknown)
          {
            testField.FieldType = EFieldType.Outside;
            change = true;
          }
        }
        foreach (Point2D oPoint in f.InsidePoints)
        {
          Field testField = AllFields[oPoint];
          if (testField.FieldType == EFieldType.Unknown)
          {
            testField.FieldType = EFieldType.Inside;
            change = true;
          }
        }
      }
      return change;
    }

    private bool AssignSides()
    {
      bool change = false;
      foreach(Field field in AllFields.Values.Where(x=>x.FieldType == EFieldType.Pipe) )
      {
        if (field.OutsideIndex != -1) continue;
        for (int i = 0; i < 2; i++)
        {
          List<Field> fields = new List<Field>();
          foreach(Point2D pTmp in field.Sides[i])
          {
            fields.Add(AllFields[pTmp]);
          }
          //var fields = AllFields.Values.Where(x => field.Sides[i].Contains(x.Location));
          if (fields.Any(x => x.FieldType == EFieldType.Outside))
          {
            field.OutsideIndex = i;
            change = true;
          }

          if (fields.Any(x => x.FieldType == EFieldType.Inside))
          {
            field.OutsideIndex = (i + 1) % 2;
            change = true;
          }
        } 
      }
      return change;
    }

    private void AssignOutside()
    {
      Point2D p = new Point2D(-1, -1);
      Field startField = AllFields[p];
      
      List<Field> current = new List<Field> { startField };
      List<Field> next = new List<Field>();

      while(current.Count > 0)
      {
        foreach(Field field in current)
        {
          if(field.FieldType != EFieldType.Unknown)
          {
            continue;
          }

          field.FieldType = EFieldType.Outside;

          foreach(Point2D neighbourPoint  in field.Location.GetNeightbours4())
          {
            if(AllFields.ContainsKey(neighbourPoint))
            {
              Field neighbourField = AllFields[neighbourPoint];
              if (neighbourField.FieldType == EFieldType.Unknown)
              {
                next.Add(neighbourField);
              }
            }
          }
        }
        current = next;
        next = new List<Field>();
      }

    }

    public void SolveDistances()
    {
      Field current = StartField;
      Field prev = StartField.Neighbours[0];
      Field endField = prev;

      int distance = 0;
      current.Distance = distance++;
      current.FieldType = EFieldType.Pipe;

      while (current != endField)
      {
        Field next = current.GetNext(prev);
        prev = current;
        current = next;

        current.Distance = distance++;
        current.FieldType = EFieldType.Pipe;
      }
    }

    public object Solve1()
    {
      return (AllFields.Values.Max(x => x.Distance) + 1) / 2;
    }

    private void RecAssing()
    {
      bool change = true;
      while (change)
      {
        change = false;
        if (AssignSides())
        {
          change = true;
        }
        if (AssignFieldTypes())
        {
          change = true;
        }
        if (AssignUnknown())
        {
          change = true;
        }
      }
    }

    private bool AssignUnknown()
    {
      bool change = false;
      foreach(Field field in AllFields.Values.Where(x=>x.FieldType == EFieldType.Unknown))
      { 
        foreach(Point2D p in field.Location.GetNeightbours8())
        {
          if (AllFields[p].FieldType == EFieldType.Outside)
          {
            field.FieldType = EFieldType.Outside;
            change = true;
          }
          if (AllFields[p].FieldType == EFieldType.Inside)
          {
            field.FieldType = EFieldType.Inside;
            change = true;
          }
        }
      }
      return change;
    }

    public object Solve2()
    {
      RecAssing();

      Field prev = AllFields.Values.First(x=>x.FieldType == EFieldType.Pipe && x.OutsideIndex != -1);
      Field current = prev.Neighbours[0];
      Field endField = prev;

      while (current != endField)
      {
        Field next = current.GetNextAndAssignSide(prev);
        prev = current;
        current = next;
      }

      RecAssing();

      //Debug.WriteLine(PrintByType());

      return AllFields.Values.Count(x=>x.FieldType == EFieldType.Inside);
    }


    public string Print()
    {
      StringBuilder sb = new StringBuilder();
      for(int i = -1; i <= MaxY; i++ )
      {
        for(int j = -1; j <= MaxX; j++ )
        {
          Point2D p = new Point2D(j, i);
          Field f = AllFields[p];
          sb.Append(f.NewType);
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }

    public string PrintByType()
    {
      StringBuilder sb = new StringBuilder();
      for (int i = -1; i <= MaxY; i++)
      {
        for (int j = -1; j <= MaxX; j++)
        {
          Point2D p = new Point2D(j, i);
          Field f = AllFields[p];
          sb.Append(f.NewType2);
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }

  }
}
