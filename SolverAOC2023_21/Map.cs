using AoCLib;
using AoCLib.BFS;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_21
{
  internal class Map
  {

    public Dictionary<Point2D, Field> Plots { get; } = new Dictionary<Point2D, Field>();
    public Dictionary<Point2D, Field> Rocks { get; } = new Dictionary<Point2D, Field>();

    public Dictionary<Point2D, Field> ForSolve { get; } = new Dictionary<Point2D, Field>();
    HashSet<Point2D> Visited { get; } = new HashSet<Point2D>();

    public Field StartField { get; }

    public Bounds2D Bounds { get; set; }

    public List<int> Counts { get; set; } = new List<int>();

    public int CountType = 0;

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
            Plots.Add(location, field);
          } 
          if(field.Type == '#')
          {
            Rocks.Add(location, field);
          }
        }
      }
      StartField = Plots.Values.First(x => x.IsStart);
      Bounds = new Bounds2D(allFields.Values.Select(x=>x.Location).ToList());

      Counts.Add(0);
      Counts.Add(0);

      
      
    }

    public Map(Map source)
    {
      Plots = source.Plots;
      Rocks = source.Rocks;

      Bounds = source.Bounds;
      Visited = source.Visited;

      Counts = source.Counts.ToList();

      CountType = (source.CountType + 1) % 2;

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
      Counts[CountType]++;
      ForSolve.Add(f.Location, f);
      Visited.Add(f.Location);
    }

    private int Mod(int x, int m)
    {
      return (x % m + m) % m;
    }

    private Field GetField(Point2D location)
    {
      if(!Plots.ContainsKey(location))
      {
        if (Visited.Contains(location)) return null;

        int x = Mod(location.X, (int)Bounds.X.Max + 1);
        int y = Mod(location.Y, (int)Bounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);
        if(Plots.ContainsKey(baseLocation))
        {
          
          Field newField =  new Field('.', location);
          Plots.Add(location, newField);
          return newField;
        } else
        {
          return null;
        }
      }
      return Plots[location];
    }

    public Dictionary<Point2D, List<DistanceItem>> GetDistances()
    {
      Dictionary<Point2D, List<DistanceItem>> ret = new Dictionary<Point2D, List<DistanceItem>>();

      BFS<BFSFieldContext> BFS = new BFS<BFSFieldContext>(StartField, new BFSFieldContext(Plots));

      foreach (Field f in Plots.Values)
      {
        if(!BFS.IsReachable(f)) continue;
        int dist = BFS.GetDistance(f);

        int x = Mod(f.Location.X, (int)Bounds.X.Max + 1);
        int y = Mod(f.Location.Y, (int)Bounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);
        
        if(!ret.ContainsKey(baseLocation))
        {
          ret.Add(baseLocation, new List<DistanceItem>());
        }
        ret[baseLocation].Add(new DistanceItem(baseLocation, f.Location, dist));
      }


      return ret;
    }

    public Dictionary<Point2D, DistanceItem> GetDistances2()
    {
      Dictionary<Point2D, DistanceItem> ret = new Dictionary<Point2D,DistanceItem>();

      BFS<BFSFieldContext> BFS = new BFS<BFSFieldContext>(StartField, new BFSFieldContext(Plots));

      foreach (Field f in Plots.Values)
      {
        if (!BFS.IsReachable(f)) continue;
        int dist = BFS.GetDistance(f);

        int x = Mod(f.Location.X, (int)Bounds.X.Max + 1);
        int y = Mod(f.Location.Y, (int)Bounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);

        ret.Add(f.Location, new DistanceItem(baseLocation, f.Location, dist));
      }


      return ret;
    }

    public string Print()
    {

      Dictionary<Point2D, DistanceItem> distances = GetDistances2();
      Bounds2D bounds = new Bounds2D(Plots.Values.Select(x => x.Location).ToList());
      StringBuilder sb = new StringBuilder();
      for (int i = (int)bounds.Y.Min; i <= bounds.Y.Max; i++)
      {
        for (int j = (int)bounds.X.Min; j <= bounds.X.Max; j++)
        {
          Point2D point2D = new Point2D(j, i);
          if (distances.ContainsKey(point2D))
          {
            DistanceItem di = distances[point2D];
            char c = Convert.ToChar(1000 + (di.Distance % ((int)Bounds.X.Max+1)));
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


    internal long GetRachableCount()
    {
      return Counts[CountType];

      //return Plots.Values.Count(x => x.Value);
    }

    public long Solve2(long steps)
    {
      

      long res = 0;
      Dictionary<Point2D, List<DistanceItem>>  dist = GetDistances();

      List<SolvedDistance> solvedDistances = new List<SolvedDistance>();

      foreach(var kv in dist) 
      {
        Point2D basePoint = kv.Key;
        List<DistanceItem> di = kv.Value;

        DistanceItem baseDistanceItemTmp; 

        baseDistanceItemTmp = di.First(x=>x.Location.Equals(basePoint));
        SolvedDistance sd = new SolvedDistance(basePoint, baseDistanceItemTmp.Distance, (int)Bounds.X.Max + 1, (int)Bounds.Y.Max + 1);
        solvedDistances.Add(sd);

        solvedDistances.Add(sd);
        solvedDistances.Add(sd);
        solvedDistances.Add(sd);


      }
      foreach (SolvedDistance sd in solvedDistances)
      {
        long xCnt = (steps - sd.Offset) / sd.DistanceX;
        long yCnt = (steps - sd.Offset) / sd.DistanceY;

        long tmp = (xCnt+1) * (yCnt)/2;
        res += tmp;
      }
      //Debug.WriteLine(Print());
      return res;
    }
  }

  public class SolvedDistance
  {
    public Point2D Location;
    public int Offset;
    public int DistanceX;
    public int DistanceY;

    public SolvedDistance(Point2D location, int offset, int distanceX, int distanceY)
    {
      Location = location;
      Offset = offset;
      DistanceX = distanceX;
      DistanceY = distanceY;
    }
  }

  public class DistanceItem
  {
    public Point2D BaseLocation;
    public Point2D Location;
    public int Distance;

    public DistanceItem(Point2D baseLocation, Point2D location, int distance)
    {
      this.BaseLocation = baseLocation;
      this.Location = location;
      this.Distance = distance;
    }
    public override string ToString()
    {
      return $"{BaseLocation}   {Location}   {Distance}";
    }

  }

}
