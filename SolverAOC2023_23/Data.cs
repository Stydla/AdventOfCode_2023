using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Data : DataBase
  {

    public Map Map { get; set; }

    public Data(string input) : base(input)
    {
      // optionaly parse data

      Map = new Map(Lines);

    }

    public object Solve1()
    {
      int ret = Map.Solve();
      return ret;
    }

    public object Solve2()
    {
      throw new NotImplementedException();
    }


  }
}
