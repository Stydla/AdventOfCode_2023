using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_22
{
  internal class Data : DataBase
  {

    public Space Space { get; set; }

    public Data(string input) : base(input)
    {
      // optionaly parse data
      Space = new Space(Lines);

    }

    public object Solve1()
    {
      Space.Settle();
      int count = Space.GetDesitegrableBricksCount();
      return count;
    }

    public object Solve2()
    {
      Space.Settle();
      int count = Space.GetFallCount();
      return count;
    }


  }
}
