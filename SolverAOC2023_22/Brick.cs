using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_22
{
  internal class Brick
  {

    public Point3D PointStart { get; set; }
    public Point3D PointEnd { get; set; }
    public Bounds3D Bounds { get; set; }

    public HashSet<Point3D> AllPoints { get; set; } = new HashSet<Point3D>();

    public HashSet<Point3D> UpPointsLayer { get; set; } = new HashSet<Point3D>();
    public HashSet<Point3D> DownPointsLayer { get; set; } = new HashSet<Point3D>();


    public List<Brick> SupportedBy { get; set; } = new List<Brick>();
    public List<Brick> Supports { get; set; } = new List<Brick>();

    public Brick(string input)
    {
      string[] points = input.Split('~');

      PointStart = Point3D.Parse(points[0]);
      PointEnd = Point3D.Parse(points[1]);
      Bounds = new Bounds3D(new List<Point3D> { PointStart, PointEnd });

    }



    public void GenerateAllPoints()
    {
      for (int i = Bounds.Z.Min; i <= Bounds.Z.Max; i++)
      {
        for (int j = Bounds.Y.Min; j <= Bounds.Y.Max; j++)
        {
          for (int k = Bounds.X.Min; k <= Bounds.X.Max; k++)
          {
            Point3D p = new Point3D(k, j, i);
            AllPoints.Add(p);
          }
        }
      }
      DownPointsLayer = GetDownPointsLayer(0);
      UpPointsLayer = GetUpPointsLayer(0);
    }

    public HashSet<Point3D> GetDownPointsLayer(int offset)
    {
      HashSet<Point3D> ret = new HashSet<Point3D>();
      for (int j = Bounds.Y.Min; j <= Bounds.Y.Max; j++)
      {
        for (int k = Bounds.X.Min; k <= Bounds.X.Max; k++)
        {
          Point3D p = new Point3D(k, j, Bounds.Z.Min + offset);
          ret.Add(p);
        }
      }
      return ret;
    }

    public HashSet<Point3D> GetUpPointsLayer(int offset)
    {
      HashSet<Point3D> ret = new HashSet<Point3D>();
      for (int j = Bounds.Y.Min; j <= Bounds.Y.Max; j++)
      {
        for (int k = Bounds.X.Min; k <= Bounds.X.Max; k++)
        {
          Point3D p = new Point3D(k, j, Bounds.Z.Max + offset);
          ret.Add(p);
        }
      }
      return ret;
    }

    public int CanMoveDownBy(List<Brick> bricks)
    {
      

      int cnt = 0;
      bool canMove = true;
      while (canMove)
      {
        HashSet<Point3D> downPoints = GetDownPointsLayer(-cnt - 1);
        foreach(Point3D p in downPoints)
        {
          
          if (bricks.Any(x => x.Bounds.Z.Max == p.Z && x.UpPointsLayer.Contains(p)))
          {
            canMove = false;
            break;
          }
        }
        if (Bounds.Z.Min - (cnt + 1) == 0) break;
        if (canMove) cnt++;
      }
      
      return cnt;
    }

    public void MoveDown(int cnt)
    {
      HashSet<Point3D> newPoints = new HashSet<Point3D>();
      foreach(Point3D p in AllPoints)
      {
        newPoints.Add(p.Move(EDirection3D_6.DOWN, cnt));
      }
      AllPoints = newPoints;

      PointStart = null;
      PointEnd = null;
      Bounds = new Bounds3D(AllPoints.ToList());
      DownPointsLayer = GetDownPointsLayer(0);
      UpPointsLayer = GetUpPointsLayer(0);
    }

    internal void FillSupportedBricks(IOrderedEnumerable<Brick> bricks)
    {
      var downPoints = GetDownPointsLayer(-1);
      var downBricks = bricks.Where(x => x.UpPointsLayer.Any(bp => downPoints.Contains(bp)));
      SupportedBy.AddRange(downBricks);

      var upPoints = GetUpPointsLayer(1);
      var upBricks = bricks.Where(x => x.DownPointsLayer.Any(bp => upPoints.Contains(bp)));
      Supports.AddRange(upBricks);
      
    }



    internal void GetFallCount(HashSet<Brick> fallingBricks)
    {
      List<Brick> nextFallingBricks = Supports.Where(x=>x.SupportedBy.Except(fallingBricks).Count() == 0).ToList();

      foreach(Brick b in nextFallingBricks)
      {
        fallingBricks.Add(b);
      }
      foreach (Brick b in nextFallingBricks)
      {
        b.GetFallCount(fallingBricks);
      }

    }
  }
}

