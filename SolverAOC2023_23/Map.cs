using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Map
  {

    public Dictionary<Point2D, Field> AllFields { get; } = new Dictionary<Point2D, Field>();
    public Bounds2D Bounds { get; }

    public List<Section> Sections { get; } = new List<Section>();

    public Field StartField { get; }
    public Field EndField { get; }

    public Map(List<string> inputLines)
    {
      for(int i = 0; i < inputLines.Count; i++)
      {
        string line = inputLines[i];
        for (int j = 0; j < line.Length; j++)
        {
          Point2D location = new Point2D(j, i);
          char inputType = line[j];
          Field f = new Field(location, inputType);

          AllFields.Add(location, f);
        }
      }

      Bounds = new Bounds2D(AllFields.Values.Select(x=>x.Location).ToList());

      StartField = AllFields.Values.Where(x => x.Location.Y == 0 && x.Location.X == 1).First();
      EndField = AllFields.Values.Where(x => x.Location.Y == Bounds.Y.Max && x.Location.X == Bounds.X.Max - 1).First();

      CreateSections();
      MergeSections();
      AssignSectionRelations();


    }

    private void SolveDistances()
    {
      foreach(Section s  in Sections)
      {
        s.SolveMaxDistances(StartField, EndField);
      }
    }

    private string Print()
    {
      StringBuilder sb = new StringBuilder();
      for(int i = 0; i <= Bounds.Y.Max; i++)
      {
        for(int j = 0; j <= Bounds.X.Max; j++)
        {
          Point2D p = new Point2D(j, i);
          Field f = AllFields[p];
          Section s = Sections.FirstOrDefault(x => x.Fields.Contains(f));
          if(s != null)
          {
            sb.Append((char)(s.SectionID+100 + 'A'));
          } else
          {
            if(f.InputType == '#')
            {
              sb.Append('█');
            } else
            {
              sb.Append(f.InputType);
            }
          }
        }
        sb.AppendLine();
      }
      return sb.ToString(); 
    }

    private void AssignSectionRelations()
    {
      foreach(Section s in Sections)
      {
        s.AssignRelations(Sections);
      }
    }

    private void CreateSections()
    {
      List<Field> current = new List<Field> { StartField };

      int sectionID = 0;

      foreach(Field f in AllFields.Values)
      {
        if (f.InputType != '.') continue;
        if (Sections.Any(x => x.Fields.Contains(f))) continue;

        Section section = new Section(sectionID++);

        section.Assign(f, AllFields);
        Sections.Add(section);
      }
    }

    private void MergeSections()
    {      
      while(true)
      {
        Section section = Sections.FirstOrDefault(x => x.InputFields.Count == 1 || x.OutputFields.Count == 1);
        if (section == null) return;

        if(section.InputFields.Count == 1)
        {
          Field connectionField = section.InputFields[0];
          Section otherSection = Sections.First(x => x.OutputFields.Contains(connectionField));
          otherSection.OutputFields.Remove(connectionField);
          otherSection.OutputFields.AddRange(section.OutputFields);
          otherSection.Fields.AddRange(section.Fields);
          otherSection.Fields.Add(connectionField);
          connectionField.InputType = '.';
          Sections.Remove(section);
        } else if (section.OutputFields.Count == 1)
        {
          Field connectionField = section.OutputFields[0];
          Section otherSection = Sections.First(x => x.InputFields.Contains(connectionField));
          otherSection.InputFields.Remove(connectionField);
          otherSection.InputFields.AddRange(section.InputFields);
          otherSection.Fields.AddRange(section.Fields);
          otherSection.Fields.Add(connectionField);
          connectionField.InputType = '.';
          Sections.Remove(section);
        }
      }
    }


    public int Solve()
    {
      SolveDistances();
      Section startSection = Sections.First(x => x.Fields.Contains(StartField));

      int distance = 0;
      startSection.Solve(0, ref distance, StartField, EndField);

      return distance;
    }

    private Dictionary<Field, Node> Nodes { get; set; } = new Dictionary<Field, Node>();

    private int nodeId = 0;
    private Node GetOrCreateNode(Field field)
    {
      if (!Nodes.ContainsKey(field))
      {
        Nodes.Add(field, new Node(nodeId++));
      }
      return Nodes[field];
    }

    public int Solve2()
    {

      Section startSection = Sections.First(x => x.Fields.Contains(StartField));
      HashSet<Field> crossroads = AllFields.Values.Where(x => x.InputType == '.' && x.GetEmptyNeighbours(AllFields).Count > 2).ToHashSet();
      crossroads.Add(StartField);
      crossroads.Add(EndField);
      foreach(Field cr in crossroads)
      {
        GetOrCreateNode(cr);
      }
      List<Field> crossroadsList = crossroads.ToList();

      for(int i = 0; i < crossroadsList.Count; i++)
      {
        Field cross1 = crossroadsList[i];
        crossroads.Remove(cross1);
        for (int j = i + 1; j < crossroadsList.Count; j++)
        {
          Field cross2 = crossroadsList[j];
          int distance = -1;
          cross1.GetDistance(crossroads, new HashSet<Field>(), AllFields, cross2, 0, ref distance);
          if(distance != -1)
          {
            Node n1 = GetOrCreateNode(cross1);
            Node n2 = GetOrCreateNode(cross2);
            Edge edge = new Edge(n1, n2, distance);
          }
        }
        crossroads.Add(cross1);
      }

      Node start = GetOrCreateNode(StartField);
      Node end = GetOrCreateNode(EndField);
      int maxDist = -1;
      start.Solve(0, ref maxDist, end, new HashSet<Node>());

      return maxDist;

    }


  }
}
