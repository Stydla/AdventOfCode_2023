using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Section
  {
   

    public int SectionID { get; }
    public List<Field> Fields { get; } = new List<Field>();

    public List<Field> InputFields { get; } = new List<Field>();
    public List<Field> OutputFields { get; } = new List<Field>();


    public List<Section> Parents { get; } = new List<Section>();
    public List<Section> Childrens { get; } = new List<Section>();

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
        Childrens.AddRange(sections.Where(x => x.InputFields.Contains(output)));
      }

      foreach (Field input in InputFields)
      {
        Parents.AddRange(sections.Where(x => x.OutputFields.Contains(input)));
      }
    }
  }
}
