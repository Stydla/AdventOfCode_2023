using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_24
{
  internal class Collision
  {

    public double CollisionTime1 { get; set; }
    public double CollisionTime2 { get; set; }
    public Coords Position { get; set; }  

    public Collision(double collisionTime1, double collisionTime2, Coords position)
    {
      CollisionTime1 = collisionTime1;
      CollisionTime2 = collisionTime2;
      Position = position;
    }

    public override string ToString()
    {
      return $"t1:{CollisionTime1}, t2:{CollisionTime2}, {Position}";
    } 
  }
}
