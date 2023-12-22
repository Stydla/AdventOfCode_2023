using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_22
{
  internal class Space
  {

    public List<Brick> Bricks { get; set; } = new List<Brick>();

    

    public Space(List<string> input)
    {
      foreach(string line in input)
      {
        Brick b = new Brick(line);
        b.GenerateAllPoints();
        Bricks.Add(b);
      }
    }

    public void Settle()
    {
      var bricks = Bricks.OrderBy(x => x.Bounds.Z.Min);
      foreach(Brick brick in bricks)
      {
        int cnt = brick.CanMoveDownBy(Bricks);
        brick.MoveDown(cnt);
      }

      foreach (Brick brick in bricks)
      {
        brick.FillSupportedBricks(bricks);
      }
    }

    public int GetDesitegrableBricksCount()
    {
      int desintegrable = 0;

      HashSet<Brick> bricks = new HashSet<Brick>();
      foreach(Brick brick in Bricks)
      {
        if(brick.Supports.All(x=>x.SupportedBy.Count > 1))
        {
          desintegrable++;
        }
      }
      return desintegrable;
    }

    internal int GetFallCount()
    {
      int total = 0;

      foreach(Brick b in Bricks)
      {
        HashSet<Brick> fallingBricks = new HashSet<Brick>() { b };
        b.GetFallCount(fallingBricks);

        total += (fallingBricks.Count - 1);
      }
      return total;
    }
  }
}
