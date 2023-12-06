using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_06
{
  internal class Data : DataBase
  {

    public List<Race> Races = new List<Race>();
    public Race Race2 { get; }
    public Data(string input) : base(input)
    {
      string [] times = Lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
      string[] distances = Lines[1].Split(new char[] {' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

      for(int i = 0; i < times.Length - 1; i++)
      {
        int time = int.Parse(times[i + 1]);
        int distance = int.Parse(distances[i + 1]);
        Race r = new Race(time, distance);
        Races.Add(r);
      }
      long time2 = long.Parse(Lines[0].Substring("Time:".Length).Replace(" ", ""));
      long distance2 = long.Parse(Lines[1].Substring("Distance:".Length).Replace(" ", ""));

      Race2 = new Race(time2, distance2);

    }

    public object Solve1()
    {
      int res = 1;
      foreach(Race r in Races)
      {
        int cnt = r.GetAllDistances().Where(x => x > r.Distance).Count();
        res*= cnt;

      }
      return res;
      
    }

    public object Solve2()
    {
      int cnt = Race2.Solve();
      return cnt;
    }


  }
}
