using AoCLib;
using AoCLib.BFS;
using AoCLib.Enums;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_21
{
  internal class Map
  {

    public Dictionary<Point2D, Field> BasePlots { get; } = new Dictionary<Point2D, Field>();
    public Dictionary<Point2D, Field> Rocks { get; } = new Dictionary<Point2D, Field>();

    public Dictionary<Point2D, Field> ForSolve { get; } = new Dictionary<Point2D, Field>();
    HashSet<Point2D> Visited { get; } = new HashSet<Point2D>();

    public Field StartField { get; }

    public Bounds2D BaseBounds { get; set; }
    public Map(List<string> input)
    {
      Dictionary<Point2D, Field> allFields  = new Dictionary<Point2D, Field>();
      for (int i = 0; i < input.Count; i++)
      {
        string line = input[i];
        for (int j = 0; j < line.Length; j++)
        {
          char type = line[j];
          Point2D location = new Point2D(j, i);
          Field field = new Field(type, location);
          allFields.Add(location, field);
          if(field.Type == '.')
          {
            BasePlots.Add(location, field);
          } 
          if(field.Type == '#')
          {
            Rocks.Add(location, field);
          }
        }
      }
      StartField = BasePlots.Values.First(x => x.IsStart);
      BaseBounds = new Bounds2D(allFields.Values.Select(x=>x.Location).ToList());

    }


    public Map(Map source)
    {
      BasePlots = source.BasePlots;
      Rocks = source.Rocks;

      BaseBounds = source.BaseBounds;
      Visited = source.Visited;

      StartField = source.StartField;
    }



    internal Map GetNextMap()
    {
      Map nextMap = new Map(this);

      foreach(Field f in ForSolve.Values)
      {

        foreach(Point2D newLocation in f.Location.GetNeightbours4())
        {
          Field targetField = nextMap.GetField(newLocation);
          if(targetField != null && targetField.Type != '#')
          {
            if(!nextMap.Visited.Contains(newLocation))
            {
              nextMap.AddForSolve(targetField);
            }
          }
        }
      }

      return nextMap;
    }



    public void AddForSolve(Field f)
    {
      ForSolve.Add(f.Location, f);
      Visited.Add(f.Location);
    }

    private int Mod(int x, int m)
    {
      return (x % m + m) % m;
    }

    private Field GetField(Point2D location)
    {
      if(!BasePlots.ContainsKey(location))
      {
        if (Visited.Contains(location)) return null;

        int x = Mod(location.X, (int)BaseBounds.X.Max + 1);
        int y = Mod(location.Y, (int)BaseBounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);
        if(BasePlots.ContainsKey(baseLocation))
        {
          
          Field newField =  new Field('.', location);
          BasePlots.Add(location, newField);
          return newField;
        } else
        {
          return null;
        }
      }
      return BasePlots[location];
    }


    public long Solve(long steps, int iterationCount)
    {

      int realSteps = ((int)steps + 2) / 2;

      int mod = BaseBounds.X.Max + 1;
      Dictionary<int, List<long>> counts = new Dictionary<int, List<long>>();
      List<long> firstCounts = new List<long>();
      for(int j = 0; j < mod; j++)
      {
        counts.Add(j, new List<long>());
      }

      Map tmp = this;
      int iX = 0;
      long solvedCount = 0;
      while(counts.Any(x=>x.Value.Count < iterationCount))
      {
        if (steps % 2 == iX % 2)
        {
          counts[(iX/2) % mod].Add(tmp.ForSolve.Count);
          firstCounts.Add(tmp.ForSolve.Count);
          solvedCount++;
        }
        tmp = tmp.GetNextMap();
        iX++;
        
      }

      long res = 0;

      int firstTake = (int)Math.Min(solvedCount, realSteps);
      long cnt = firstCounts.Take(firstTake).Sum();
      res += cnt;
      Debug.WriteLine($"Fist count: {cnt}");

      long n = Math.Max((realSteps - solvedCount) / mod, 0);
      cnt = 0;
      for(int i = 0; i < mod; i++)
      {
        long d = counts[i][iterationCount - 1] - counts[i][iterationCount - 2];
        long a1 = counts[i][iterationCount - 1] + d;
        long aN = a1 + (n-1) * d;
        long s = n * (a1 + aN) / 2;
        cnt+= s;
      }
      res += cnt;
      Debug.WriteLine($"Computed count: {cnt}");

      long alreadySolved = n * mod + firstTake;
      cnt = 0;
      if(alreadySolved < realSteps)
      {
        long n2 = (realSteps - solvedCount) % mod;
        for (int i = 0; i < n2; i++)
        {

          long d = counts[i][iterationCount - 1] - counts[i][iterationCount - 2];
          long a1 = counts[i][iterationCount - 1];
          long aN = a1 + (n + 1 ) * d;
          cnt += aN;
        }
      }
      res += cnt;
      Debug.WriteLine($"Rest count: {cnt}");

      return res;
    }
    public Dictionary<Point2D, DistanceItem> GetDistances2()
    {
      Dictionary<Point2D, DistanceItem> ret = new Dictionary<Point2D,DistanceItem>();

      BFS<BFSFieldContext> BFS = new BFS<BFSFieldContext>(StartField, new BFSFieldContext(BasePlots));

      foreach (Field f in BasePlots.Values)
      {
        if (!BFS.IsReachable(f)) continue;
        int dist = BFS.GetDistance(f);

        int x = Mod(f.Location.X, (int)BaseBounds.X.Max + 1);
        int y = Mod(f.Location.Y, (int)BaseBounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);

        ret.Add(f.Location, new DistanceItem(baseLocation, f.Location, dist, EDirection9.NONE));
      }

      return ret;
    }

    public string Print()
    {

      Dictionary<Point2D, DistanceItem> distances = GetDistances2();
      Bounds2D bounds = new Bounds2D(BasePlots.Values.Select(x => x.Location).ToList());
      StringBuilder sb = new StringBuilder();
      for (int i = (int)bounds.Y.Min; i <= bounds.Y.Max; i++)
      {
        for (int j = (int)bounds.X.Min; j <= bounds.X.Max; j++)
        {
          Point2D point2D = new Point2D(j, i);
          if (distances.ContainsKey(point2D))
          {
            DistanceItem di = distances[point2D];
            char c = Convert.ToChar(1000 + (di.Distance % ((int)BaseBounds.X.Max+1)));
            if(di.Distance % 2 == 0)
            {
              sb.Append('.');
            } else
            {
              sb.Append(c);
            }
            
          }
          else
          {
            sb.Append('█');
          }
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }

  }

  public class DistanceItem
  {
    public Point2D BaseLocation;
    public Point2D Location;
    public int Distance;

    public EDirection9 Position;

    public DistanceItem(Point2D baseLocation, Point2D location, int distance, EDirection9 position)
    {
      this.BaseLocation = baseLocation;
      this.Location = location;
      this.Distance = distance;
      this.Position = position;
    }
    public override string ToString()
    {
      return $"{BaseLocation}   {Location}   {Distance}  {Position}";
    }

  }

}
