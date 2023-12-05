using AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_05
{
  internal class MapItem
  {

    public string Name { get; set; }  
    public List<Range> Ranges { get; } = new List<Range>();


    public MapItem(string input)
    {
      using(StringReader reader = new StringReader(input))
      {
        Name = reader.ReadLine();

        string line;
        while((line = reader.ReadLine()) != null)
        {
          string[] nums = line.Split(' ');

          long destination = long.Parse(nums[0]);
          long source = long.Parse(nums[1]);
          long length = long.Parse(nums[2]);

          Range r = new Range(destination, source, length);
          Ranges.Add(r);
        }
      }
      Ranges = Ranges.OrderBy(x => x.Source).ToList();

      List<Range> newRanges = new List<Range>();

      if (Ranges[0].Source != 0)
      {
        newRanges.Add(new Range(0, 0, Ranges[0].Source));
      }
      for (int i = 0; i < Ranges.Count - 1; i++)
      {
        Range r = Ranges[i];
        Range rNext = Ranges[i + 1];
        if (r.Source + r.Length != rNext.Source)
        {
          long from = r.Source + r.Length;
          long length = rNext.Source - from;
          newRanges.Add(new Range(from, from, length));
        }
      }

      long lastSource = Ranges.Last().Source + Ranges.Last().Length;
      long lastLength = Ranges.Last().Source + Ranges.Last().Length;
      
      newRanges.Add(new Range(lastSource, lastSource, long.MaxValue - lastSource));
      
      Ranges.AddRange(newRanges);


    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(Name);
      foreach(Range r in Ranges)
      {
        stringBuilder.AppendLine(r.ToString());
      }
      return stringBuilder.ToString();
    }

    public long GetTarget(long source)
    {
      foreach(Range range in Ranges)
      {
        if(range.Contains(source))
        {
          return range.GetTarget(source);
        }
      }
      return source;

    }

    internal List<Interval> Solve(List<Interval> intervals)
    {

      List<Interval> result = new List<Interval>();
      foreach (Range r in Ranges)
      {
        Interval intTmp = new Interval(r.Source, r.Source +  r.Length - 1);
        
        long offset = r.Destination - r.Source;
        foreach(Interval interval in intervals)
        {
          Interval intersect = interval.Intersect(intTmp);
          if(intersect != null)
          {
            Interval newInterval = new Interval(intersect.StartPoint.Value + offset, intersect.EndPoint.Value + offset);
            result.Add(newInterval);
          }
        }
      }

      return result;
    }
  }
}
