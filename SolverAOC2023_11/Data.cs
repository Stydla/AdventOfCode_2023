using AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_11
{
  internal class Data : DataBase
  {


    public List<Field> AllFields { get; set; } = new List<Field>();
    public List<Field> Galaxies { get; set; } = new List<Field>();

    public long Part1Multiplier;
    public long Part2Multiplier;

    public List<int> VerticalSpaces { get; set; } = new List<int>();
    public List<int> HorizontalSpaces { get; set; } = new List<int>();

    public Data(string input) : base(input)
    {
      // optionaly parse data
      Part1Multiplier = long.Parse(Lines[0]);
      Part2Multiplier = long.Parse(Lines[1]);
      for (int i = 2; i < Lines.Count; i++)
      {
        string line = Lines[i];
        for(int j = 0; j < line.Length; j++)
        {
          char c = line[j];

          Point2D location = new Point2D(j, i - 2);
          Field f = new Field(location, c);
          AllFields.Add(f);
          if(f.IsGalaxy())
          {
            Galaxies.Add(f);
          }
        }
      }

      int maxX = AllFields.Max(x => x.Location.X);
      int maxY = AllFields.Max(y => y.Location.Y);

      for (int i = 0; i <= maxX; i++)
      {
        var column = AllFields.Where(x => x.Location.X == i);
        if (column.All(x => x.Value == '.'))
        {
          VerticalSpaces.Add(i);
        }
      }

      for (int i = 0; i <= maxY; i++)
      {
        var column = AllFields.Where(x => x.Location.Y == i);
        if (column.All(x => x.Value == '.'))
        {
          HorizontalSpaces.Add(i);
        }
      }

    }

    long GetAllDistances(long multiplier)
    {
      long total = 0;
      for (int i = 0; i < Galaxies.Count; i++)
      {
        Field f = Galaxies[i];
        for (int j = i + 1; j < Galaxies.Count; j++)
        {
          Field f2 = Galaxies[j];
          long dist = f.GetDistance(f2, multiplier, VerticalSpaces, HorizontalSpaces);
          total += dist;
        }
      }
      return total;
    }

    public object Solve1()
    {
      return GetAllDistances(Part1Multiplier);
    }

    public object Solve2()
    {
      return GetAllDistances(Part2Multiplier);
    }


  }
}
