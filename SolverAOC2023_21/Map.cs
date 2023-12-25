using AoCLib;
using AoCLib.BFS;
using AoCLib.Enums;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_21
{
  internal class Map
  {

    public Dictionary<Point2D, Field> BasePlots { get; } = new Dictionary<Point2D, Field>();
    public Dictionary<Point2D, Field> ExtendedPlots { get; } = new Dictionary<Point2D, Field>();
    public Dictionary<Point2D, Field> Rocks { get; } = new Dictionary<Point2D, Field>();
    public Dictionary<Point2D, Field> ExtendedRocks { get; } = new Dictionary<Point2D, Field>();

    public Dictionary<Point2D, Field> ForSolve { get; } = new Dictionary<Point2D, Field>();
    HashSet<Point2D> Visited { get; } = new HashSet<Point2D>();

    public Field StartField { get; }

    public Bounds2D BaseBounds { get; set; }
    public Bounds2D ExtendedBounds { get; set; }

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

      Extend(allFields, ExtendCount);

    }

    private int ExtendCount = 5;

    public Map(Map source)
    {
      BasePlots = source.BasePlots;
      Rocks = source.Rocks;

      BaseBounds = source.BaseBounds;
      Visited = source.Visited;

      StartField = source.StartField;
    }

    public void Extend(Dictionary<Point2D, Field> allFields, int count)
    {
      int blockLengthX = BaseBounds.X.Max + 1;
      int blockLengthY = BaseBounds.Y.Max + 1;

      int minX = BaseBounds.X.Min - count * blockLengthX;
      int maxX = BaseBounds.X.Max + count * blockLengthX;
      int minY = BaseBounds.Y.Min - count * blockLengthY;
      int maxY = BaseBounds.Y.Max + count * blockLengthY;

      for (int i = minX; i <= maxX ; i++)
      {
        for(int j = minY; j <= maxY ; j++)
        {
          int x = Mod(j, blockLengthX);
          int y = Mod(i, blockLengthY);
          Point2D baseLocation = new Point2D(x, y);
          char type = allFields[baseLocation].Type;

          Point2D location = new Point2D(j, i);
          Field field = new Field(type, location);
          if (field.Type == '.')
          {
            ExtendedPlots.Add(location, field);
          }
          if (field.Type == '#')
          {
            ExtendedRocks.Add(location, field);
          }
        }
      }
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

    public Dictionary<Point2D, List<DistanceItem>> GetDistances()
    {
      Dictionary<Point2D, List<DistanceItem>> ret = new Dictionary<Point2D, List<DistanceItem>>();

      BFS<BFSFieldContext> BFS = new BFS<BFSFieldContext>(StartField, new BFSFieldContext(ExtendedPlots));

      foreach (Field f in ExtendedPlots.Values)
      {
        if(!BFS.IsReachable(f)) continue;
        int dist = BFS.GetDistance(f);

        int x = Mod(f.Location.X, (int)BaseBounds.X.Max + 1);
        int y = Mod(f.Location.Y, (int)BaseBounds.Y.Max + 1);
        Point2D baseLocation = new Point2D(x, y);
        
        if(!ret.ContainsKey(baseLocation))
        {
          ret.Add(baseLocation, new List<DistanceItem>());
        }

        EDirection9 dir = EDirection9.NONE;
        if(f.Location.X < baseLocation.X && f.Location.Y < baseLocation.Y)
        {
          dir = EDirection9.UP_LEFT;
        }
        if (f.Location.X == baseLocation.X && f.Location.Y < baseLocation.Y)
        {
          dir = EDirection9.UP;
        }
        if (f.Location.X > baseLocation.X && f.Location.Y < baseLocation.Y)
        {
          dir = EDirection9.UP_RIGHT;
        }

        if (f.Location.X < baseLocation.X && f.Location.Y == baseLocation.Y)
        {
          dir = EDirection9.LEFT;
        }
        if (f.Location.X > baseLocation.X && f.Location.Y == baseLocation.Y)
        {
          dir = EDirection9.RIGHT;
        }

        if (f.Location.X < baseLocation.X && f.Location.Y > baseLocation.Y)
        {
          dir = EDirection9.DOWN_LEFT;
        }
        if (f.Location.X == baseLocation.X && f.Location.Y > baseLocation.Y)
        {
          dir = EDirection9.DOWN;
        }
        if (f.Location.X > baseLocation.X && f.Location.Y > baseLocation.Y)
        {
          dir = EDirection9.DOWN_RIGHT;
        }



        ret[baseLocation].Add(new DistanceItem(baseLocation, f.Location, dist, dir));
      }


      return ret;
    }


    public long Solve(long steps)
    {
      long res = 0;
      Dictionary<Point2D, List<DistanceItem>> distances = GetDistances();


      int index1 = ExtendCount - 1;
      int index2 = ExtendCount - 2;
      int index3 = ExtendCount - 3;

      //left
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul =kv.Value.Where(x => x.Position == EDirection9.LEFT).ToList();
        int minY = ul.Max(x => x.Location.Y);
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == minY).OrderBy(x=>x.Distance).ToList();

        
        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        if(steps > xOffset)
        {
          long cnt = (steps - xOffset) / xDist;
          if(xDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
        }
      }

      //Right
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.RIGHT).ToList();
        int minY = ul.Max(x => x.Location.Y);
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == minY).OrderBy(x => x.Distance).ToList();

        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        if (steps > xOffset)
        {
          long cnt = (steps - xOffset) / xDist;
          if (xDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
        }
      }

      // Up
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.UP).ToList();
        int maxX = ul.Max(x => x.Location.X);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == maxX).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > yOffset)
        {
          long cnt = (steps - yOffset) / yDist;
          if (yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
        }
      }

      // Down
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.DOWN).ToList();
        int minX = ul.Min(x => x.Location.X);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == minX).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > yOffset)
        {
          long cnt = (steps - yOffset) / yDist;
          if (yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
         
        }
      }


      // Up Left
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.UP_LEFT).ToList();
        int maxX = ul.Max(x => x.Location.X);
        int maxY = ul.Max(x => x.Location.Y);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == maxX).OrderBy(x => x.Distance).ToList();
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == maxY).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");
        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > xOffset)
        {
          long cntX = (steps - xOffset) / xDist;
          long cntY = (steps - yOffset) / yDist;
          long cnt = (cntX * cntY + 1) / 2;
          if (xDist % 2 == 0 || yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
          Debug.WriteLine(cnt);
        }
      }


      // Up Right
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.UP_RIGHT).ToList();
        int minX = ul.Min(x => x.Location.X);
        int minY = ul.Min(x => x.Location.Y);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == minX).OrderBy(x => x.Distance).ToList();
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == minY).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");
        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > xOffset)
        {
          long cntX = (steps - xOffset) / xDist;
          long cntY = (steps - yOffset) / yDist;
          long cnt = (cntX * cntY + 1) / 2;
          if (xDist % 2 == 0 || yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
        }
      }


      // Down Left
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.DOWN_LEFT).ToList();
        int maxX = ul.Max(x => x.Location.X);
        int minY = ul.Min(x => x.Location.Y);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == maxX).OrderBy(x => x.Distance).ToList();
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == minY).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");
        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > xOffset)
        {
          long cntX = (steps - xOffset) / xDist;
          long cntY = (steps - yOffset) / yDist;
          long cnt = (cntX * cntY + 1) / 2;
          if (xDist % 2 == 0 || yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
          Debug.WriteLine(cnt);
        }
      }

      // Down Right
      foreach (var kv in distances)
      {
        Point2D baseLocation = kv.Key;
        List<DistanceItem> ul = kv.Value.Where(x => x.Position == EDirection9.DOWN_RIGHT).ToList();
        int minX = ul.Min(x => x.Location.X);
        int minY = ul.Min(x => x.Location.Y);
        List<DistanceItem> yDists = ul.Where(x => x.Location.X == minX).OrderBy(x => x.Distance).ToList();
        List<DistanceItem> xDists = ul.Where(x => x.Location.Y == minY).OrderBy(x => x.Distance).ToList();

        if ((yDists[index1].Distance + yDists[index3].Distance) / 2 != yDists[index2].Distance) throw new Exception("Dist Error");
        if ((xDists[index1].Distance + xDists[index3].Distance) / 2 != xDists[index2].Distance) throw new Exception("Dist Error");

        int xDist = xDists[index1].Distance - xDists[index2].Distance;
        int xOffset = xDists[0].Distance;

        int yDist = yDists[index1].Distance - yDists[index2].Distance;
        int yOffset = yDists[0].Distance;

        if (steps > xOffset)
        {
          long cntX = (steps - xOffset) / xDist;
          long cntY = (steps - yOffset) / yDist;
          long cnt = (cntX * cntY + 1) / 2;
          if (xDist % 2 == 0 || yDist % 2 == 0)
          {
            cnt = cnt / 2;
          }
          res += cnt;
          Debug.WriteLine(cnt);
        }
      }











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


    internal long GetRachableCount()
    {
      return 0;

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
        SolvedDistance sd = new SolvedDistance(basePoint, baseDistanceItemTmp.Distance, (int)BaseBounds.X.Max + 1, (int)BaseBounds.Y.Max + 1);
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
