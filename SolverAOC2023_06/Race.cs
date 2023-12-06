using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_06
{
  internal class Race
  {
    public long Time { get; set; }
    public long Distance { get; set; }

    public Race(long time, long distance)
    {
      Time = time;
      Distance = distance;
    } 

    public long GetDistance(long holdTime)
    {
      return (Time - holdTime) * holdTime;
    }

    public List<long> GetAllDistances()
    {
      List<long> res = new List<long>();
      for(int i = 0; i < Time; i++)
      {
        res.Add(GetDistance(i));
      }
      return res;
    }

    public int Solve()
    {
      long t = Time;
      long d = Distance;
      long x1 = (-t - (long)Math.Sqrt(t * t - 4*d)) / 2;
      long x2 = (-t + (long)Math.Sqrt(t * t - 4*d)) / 2;

      return (int)(x2 - x1);

    }

  }
}
