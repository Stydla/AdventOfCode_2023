using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20
{
  internal class Data : DataBase
  {

    public Machine Machine { get; set; }
    public Data(string input) : base(input)
    {
      // optionaly parse data
      Machine = new Machine(Lines);
    }

    public object Solve1()
    {

      for(int i =0; i < 1000; i++)
      {
        Machine.PushButton();
      }
      return Machine.LowSignalCount * Machine.HighSignalCount;

      //long cnt = 0;
      //while(!solutions.Contains(Machine.GetCustomHash()))
      //{
      //  solutions.Add(Machine.GetCustomHash());
      //  Machine.PushButton();
      //  cnt++;
      //}

      //long totalHigh = 0;
      //long totalLow = 0;

      //long repeatCount = 1000 / cnt;
      //long rest = 1000 % cnt;

      //totalHigh = repeatCount * Machine.HighSignalCount;
      //totalLow = repeatCount * Machine.LowSignalCount;

      //Machine.LowSignalCount = 0;
      //Machine.HighSignalCount = 0;
      //for (int i = 0; i < rest; i++)
      //{
      //  Machine.PushButton();
      //}

      //totalHigh += Machine.HighSignalCount;
      //totalLow += Machine.LowSignalCount;

      //return totalHigh * totalLow;
    }

    public object Solve2()
    {

      long cnt = 0;
      while(!Machine.PushButton2())
      {
        cnt++;
      }
      return cnt;
    }


  }
}
