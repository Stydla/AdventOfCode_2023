using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

      foreach(Section section in Sections)
      {
        section.AssignRelations(Sections);
      }
    }


    public int Solve()
    {

      return 0;
    }


  }
}
