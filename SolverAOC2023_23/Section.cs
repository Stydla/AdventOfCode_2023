using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  public class Section
  {
   

    public int SectionID { get; }
    public List<Field> Fields { get; } = new List<Field>();

    public List<Field> InputFields { get; } = new List<Field>();
    public List<Field> OutputFields { get; } = new List<Field>();


    public HashSet<Section> Parents { get; } = new HashSet<Section>();
    public HashSet<Section> Childrens { get; } = new HashSet<Section>();

    public List<DistanceItem> Distances { get; } = new List<DistanceItem>();

    public Dictionary<Field, int> MaxDistances { get; } = new Dictionary<Field, int>();

    public Section(int sectionID)
    {
      SectionID = sectionID;
    }

    public override string ToString()
    {
      return $"{SectionID,3}: Fields: {Fields.Count}, Inputs: {InputFields.Count}, Outputs: {OutputFields.Count}";

    }

    internal void Assign(Field startField, Dictionary<Point2D, Field> allFields)
    {

      HashSet<Field> current = new HashSet<Field> { startField };

      while(current.Count > 0)
      {
        HashSet<Field> next = new HashSet<Field>();

        foreach(Field f in current)
        {
          Fields.Add(f);

          foreach(Field neighbour in f.GetEmptyNeighbours(allFields))
          {
            if (Fields.Contains(neighbour)) continue;
            next.Add(neighbour);
          }
        }
        current = next;
      }

      foreach(Field f in Fields)
      {
        foreach(var kv in f.GetSlopeNeighbours(allFields))
        {
          switch (kv.Value)
          {
            case SlopeDirection.IN:
              InputFields.Add(kv.Key);

              break;
            case SlopeDirection.OUT:
              OutputFields.Add(kv.Key);
              break;
          }
        }
      }

    }

    internal void AssignRelations(List<Section> sections)
    {
      foreach(Field output in OutputFields)
      {
        foreach (Section s in sections)
        {
          if (s.InputFields.Contains(output))
          {
            if (!Childrens.Contains(s))
            {
              Childrens.Add(s);
            }
          }
        }
      }

      foreach (Field input in InputFields)
      {
        foreach (Section s in sections)
        {
          if (s.OutputFields.Contains(input))
          {
            if (!Parents.Contains(s))
            {
              Parents.Add(s);
            }
          }
        }
      }
    }

    public void SolveMaxDistances(Field startField, Field endField)
    {

      Dictionary<Point2D, Field> AllFields = new Dictionary<Point2D, Field>();
      foreach(Field f in Fields)
      {
        AllFields.Add(f.Location, f);
      }
      foreach(Field f in InputFields)
      {
        if(!AllFields.ContainsKey(f.Location)) AllFields.Add(f.Location, f);
      }
      foreach (Field f in OutputFields)
      {
        if(!AllFields.ContainsKey(f.Location)) AllFields.Add(f.Location, f);
      }

      List<Field> fromFields = InputFields.ToList();
      if (Fields.Contains(startField)) fromFields.Add(startField);
      List<Field> toFields = OutputFields.ToList();
      if (Fields.Contains(endField)) toFields.Add(endField);

      foreach (Field from in fromFields)
      {
        foreach(Field to in toFields)
        {
          RecData rd =new RecData();
          rd.From = from;
          rd.To = to;
          rd.Current = from;
          rd.AllFields = AllFields;
          
          SolveMaxDistances(rd);

          DistanceItem di = new DistanceItem();
          di.From = from;
          di.To = to;
          di.Distance = rd.MaxDistance;
          Distances.Add(di);
        }
      }
    }

    private void SolveMaxDistances(RecData recursionData)
    {
      if(recursionData.Current == recursionData.To)
      {
        if(recursionData.MaxDistance < recursionData.Distance)
        {
          recursionData.MaxDistance = recursionData.Distance;
        }
        return;
      }

      Field current = recursionData.Current;
      recursionData.Visited.Add(current);

      foreach (Field f in current.GetEmptyNeighbours(recursionData.AllFields))
      {
        if (recursionData.Visited.Contains(f)) continue;

        recursionData.Current = f;
        recursionData.Distance++;
        SolveMaxDistances(recursionData);
        recursionData.Distance--;
      }

      foreach (Field f in current.GetSlopeNeighbours(recursionData.AllFields).Keys)
      {
        if (recursionData.Visited.Contains(f)) continue;

        recursionData.Current = f;
        recursionData.Distance++;
        SolveMaxDistances(recursionData);
        recursionData.Distance--;
      }

      recursionData.Current = current;

      recursionData.Visited.Remove(current);
    }

    internal void Solve(int currentDistance, ref int maxDistance, Field from, Field finalField)
    {
      if(this.Fields.Contains(finalField))
      {
        DistanceItem dist = Distances.First(x => x.To == finalField && x.From == from);
        if(currentDistance + dist.Distance > maxDistance) maxDistance = currentDistance + dist.Distance;
        return;
      }


      foreach(Field output in OutputFields)
      {
        DistanceItem dist = Distances.First(x => x.From == from && x.To == output);
        currentDistance += dist.Distance;
        Childrens.First(x=>x.InputFields.Contains(output)).Solve(currentDistance, ref maxDistance, output, finalField);
        currentDistance -= dist.Distance;
      }

    }

    private class RecData
    {
      public Dictionary<Point2D, Field> AllFields { get; set; } = new Dictionary<Point2D, Field>();
      public Field From { get; set; }
      public Field To { get; set; }
      public Field Current { get; set; }
      public HashSet<Field> Visited { get; } = new HashSet<Field>();
      public int Distance { get; set; }
      public int MaxDistance { get; set; } = -1;

      public List<DistanceItem> Distances { get; } = new List<DistanceItem>();
    }

  }

  public class DistanceItem
  {
    public Field From { get; set; }
    public Field To { get; set; } 
    public int Distance { get; set; }
  }
}
